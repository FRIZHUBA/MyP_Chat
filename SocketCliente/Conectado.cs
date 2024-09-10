using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace SocketCliente {

    class Conectado {

        private Thread recibir;
        private Thread enviar;
        private Socket? handler;

        public Conectado(){

            this.recibir = new Thread(receive);
            this.enviar = new Thread(send);

            this.recibir.Start();
            this.enviar.Start();
        }

        public void receive(){

            while (true) {

                string data = null;
                byte[] bytes = null;

                while (true) {

                    try {

                        bytes = new byte[1024];
                        int byteRec = this.handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, byteRec);

                        if (data.IndexOf("<EOM>") > -1) break;

                    } catch (Exception e) {

                        Console.WriteLine(e.ToString());
                    }
                }

                data = data.Replace("<EOM>", "");
                Console.WriteLine(data);
            }
        }

        public void send(){

            string mensaje = "";

            Console.WriteLine("Ingrese un mensaje");

            while (mensaje != "exit") {

                mensaje = Console.ReadLine();

                if (mensaje != "exit") {

                    byte[] msg = Encoding.ASCII.GetBytes(mensaje + "<EOM>");
                    int byteSent = this.handler.Send(msg);
                }
            }

            this.handler.Shutdown(SocketShutdown.Both);
            this.handler.Close();

            Environment.Exit(0);
        }

        public Socket Handler { get => handler; set => handler = value; }
    }
}