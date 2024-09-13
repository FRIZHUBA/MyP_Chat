using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace SocketServidor {
    class Conectado {

        private string nick;
        private Socket handler;
        private Thread recibir;

        public Conectado(Socket socket, string username) {

            this.handler = socket;
            this.nick = username;
            
            this.recibir = new Thread(Receive);
            this.recibir.Start();
        }

        public void Send(string mensaje) {

            byte[] msg = Encoding.UTF8.GetBytes(mensaje + "<EOM>");
            handler.Send(msg);

            Console.WriteLine("Enviado: " + mensaje);
        }

        public void Receive() {

            while(true) {

                string data = RecibirDatos();

                if (! string.IsNullOrEmpty(data)) {

                    Console.WriteLine($"{nick}: {data}");
                }
            }
        }

        private string RecibirDatos() {

            string data = "";
            byte[] bytes = new byte[1024];

            while (true) {

                try {

                    int byteRec = this.handler.Receive(bytes);

                    data += Encoding.UTF8.GetString(bytes, 0, byteRec);

                    if (data.IndexOf("<EOM>") > -1) break;

                } catch (Exception e) {

                    Console.WriteLine(e.ToString());
                    return null;
                }
            }

            return data.Replace("<EOM>", "").Trim();
        }

        public string Nick { get => nick; }
        public Socket Handler { get => handler; }
    }
}