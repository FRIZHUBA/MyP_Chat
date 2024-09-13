using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace SocketCliente {

    class Conectado {

        private Thread recibir;
        private Thread enviar;
        private Socket handler;

        public Conectado(Socket socket){

            this.handler = socket;

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
                Console.WriteLine(data);
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