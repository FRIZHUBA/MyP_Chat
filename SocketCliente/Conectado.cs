using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace SocketCliente {

    class Conectado {

        private Thread recibir;
        private Thread enviar;
        private Socket handler;
        private static readonly List<ConsoleColor> colores = new List<ConsoleColor> {
            ConsoleColor.Blue,
            ConsoleColor.Green,
            ConsoleColor.Yellow,
            ConsoleColor.Magenta,
            ConsoleColor.Cyan
        };

        private ConsoleColor colorAsignado;
        private static Random random = new Random();

        public Conectado(Socket socket){

            this.handler = socket;

            this.colorAsignado = colores[random.Next(colores.Count)];

            this.recibir = new Thread(Receive);
            this.enviar = new Thread(Send);

            this.recibir.Start();
            this.enviar.Start();
        }

        public void Receive(){

            while (true) {

                string data = null;
                byte[] bytes = new byte[1024];

                while (true) {

                    try {

                        int byteRec = handler.Receive(bytes);
                        data += Encoding.UTF8.GetString(bytes, 0, byteRec);

                        if (data.IndexOf("<EOM>") > -1) break;

                    } catch (Exception e) {

                        Console.WriteLine(e.ToString());
                    }
                }

                data = data.Replace("<EOM>", "");

                if (Mensaje.EsJsonValido(data)) {

                    dynamic mensaje = Mensaje.Parsear<dynamic>(data);

                    if (mensaje.type == "NEW_USER") {

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Nuevo usuario conectado: {mensaje.username}");
                        Console.ResetColor();

                    } else {

                        Console.ForegroundColor = colorAsignado;
                        Console.WriteLine($"{mensaje.username}: {mensaje.mensaje}");
                        Console.ResetColor();
                    }

                } else {

                    Console.ForegroundColor = colorAsignado;
                    Console.WriteLine(data);
                    Console.ResetColor();
                }
            }
        }

        public void Send(){

            string mensaje = "";

            Console.WriteLine("Ingrese un mensaje");

            while (mensaje != "exit") {

                mensaje = Console.ReadLine();

                if (mensaje != "exit") {

                    byte[] msg = Encoding.UTF8.GetBytes(mensaje + "<EOM>");
                    handler.Send(msg);
                }
            }

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
            Environment.Exit(0);
        }
    }
}