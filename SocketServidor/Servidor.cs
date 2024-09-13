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
        private Dictionary<string, Conectado> dic;

        public Servidor(AddressFamily addressFamily, IPEndPoint localEndPoint) {

            this.listener = new Socket(addressFamily, SocketType.Stream, ProtocolType.Tcp);
            this.endPoint = localEndPoint;
            this.dic = new Dictionary<string, Conectado>();
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

            while (! nickValido) {

                string data = RecibirDatos(handler);
                IdentifyMessage identify = Mensaje.Parsear<IdentifyMessage>(data);

                if (identify != null && ! dic.ContainsKey(identify.Username)) {

                    Conectado nuevo = new Conectado(handler, identify.Username);
                    dic.Add(identify.Username, nuevo);

                    Console.WriteLine($"Nuevo usuario conectado: {identify.Username}");

                    string response = Mensaje.Response("IDENTIFY", "SUCCESS", identify.Username);
                    handler.Send(Encoding.UTF8.GetBytes(response + "<EOM>"));

                    nickValido = true;

                } else {

                    Console.WriteLine($"El usuario {identify.Username} ya existe.");

                    string error = Mensaje.Response("IDENTIFY", "USER_ALREADY_EXISTS", identify.Username);
                    handler.Send(Encoding.UTF8.GetBytes(error + "<EOM>"));
                }
            }
        }

        private string RecibirDatos(Socket handler) {

            string data = "";
            byte[] bytes;

            while (true) {

                bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);

                data += Encoding.UTF8.GetString(bytes, 0, bytesRec);

                if(data.IndexOf("<EOM>") > -1) break;
            }

            return data.Replace("<EOM>", "").Trim();
        }
    }
}