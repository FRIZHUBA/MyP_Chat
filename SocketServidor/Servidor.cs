using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SocketServidor {

    class Servidor {

        private Socket listener;
        private IPEndPoint endPoint;
        private ClienteManager clienteManager;
        private static readonly List<ConsoleColor> colores = new List<ConsoleColor> {
            ConsoleColor.Blue, 
            ConsoleColor.Green, 
            ConsoleColor.Yellow, 
            ConsoleColor.Magenta, 
            ConsoleColor.Cyan
        };
        private Random random = new Random();

        public Servidor(AddressFamily addressFamily, IPEndPoint localEndPoint) {

            this.listener = new Socket(addressFamily, SocketType.Stream, ProtocolType.Tcp);
            this.endPoint = localEndPoint;
            this.clienteManager = new ClienteManager();
        }

        public void Iniciar() {

            listener.Bind(endPoint);
            listener.Listen(10);
            Console.WriteLine("Esperando conexión...");

            while(true) {

                Socket handler = listener.Accept();
                ManejarCliente(handler);
            }
        }

        private void ManejarCliente(Socket handler) {

            bool nickValido = false;
            Conectado actual = null;

            while (! nickValido) {

                string data = RecibirDatos(handler);
                IdentifyMessage identify = Mensaje.Parsear<IdentifyMessage>(data);

                if (identify != null && ! clienteManager.ContieneCliente(identify.Username)) {

                    actual = new Conectado(handler, identify.Username, AsignarColor());
                    clienteManager.AddCliente(actual);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Nuevo usuario conectado: {identify.Username}");
                    Console.ResetColor();

                    string response = Mensaje.Response("IDENTIFY", "SUCCESS", identify.Username);
                    handler.Send(Encoding.UTF8.GetBytes(response + "<EOM>"));

                    string newUser = Mensaje.Nuevo("NEW_USER", identify.Username);
                    clienteManager.EnviarGeneral(newUser, actual);

                    nickValido = true;

                } else {

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"El usuario {identify.Username} ya existe.");
                    Console.ResetColor();

                    string error = Mensaje.Response("IDENTIFY", "USER_ALREADY_EXISTS", identify.Username);
                    handler.Send(Encoding.UTF8.GetBytes(error + "<EOM>"));
                }
            }

            if (actual != null) {

                actual.IniciarRecepcion(clienteManager);
            }
        }

        private ConsoleColor AsignarColor() {

            return colores[random.Next(colores.Count)];
        }

        private string RecibirDatos(Socket handler) {

            string data = "";
            byte[] bytes = new byte[1024];

            while (true) {

                int bytesRec = handler.Receive(bytes);

                data += Encoding.UTF8.GetString(bytes, 0, bytesRec);

                if(data.IndexOf("<EOM>") > -1) break;
            }

            return data.Replace("<EOM>", "").Trim();
        }
    }
}