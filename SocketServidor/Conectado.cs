using System;
using System.Text;
using System.Threading;
using System.Net.Sockets;

namespace SocketServidor {
    class Conectado {

        private string? nick;
        private Thread recibir;
        private Socket? handler;

        public Conectado() {

            this.recibir = new Thread(receive);
            this.recibir.Start();
        }

        public void send(string mensaje) {

            byte[] msg = Encoding.ASCII.GetBytes(mensaje + "<EOM>");
            int byteSent = this.handler.Send(msg);

            Console.WriteLine("Enviado: " + msg);
        }

        public void receive() {

            while (true) {

                string data = null;
                byte[] bytes = null;

                while (true) {

                    bytes = new byte[1024];
                    int byteRec = this.handler.Receive(bytes);
                    
                    data += Encoding.ASCII.GetString(bytes, 0, byteRec);

                    if (data.IndexOf("<EOM>") > -1) break;
                }

                data = data.Replace("<EOM>", "");

                Console.WriteLine(this.nick + ": " + data);
            }
        }

        public string Nick { get => nick; set => nick = value; }
        public Socket Handler { get => handler; set => handler = value; }
    }
}