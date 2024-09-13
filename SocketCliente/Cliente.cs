using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SocketCliente {

    class Cliente {

        private Socket handler;
        private IPEndPoint remoteEP;

        public Cliente(AddressFamily addressFamily, IPEndPoint endPoint) {

            this.handler = new Socket(addressFamily, SocketType.Stream, ProtocolType.Tcp);
            this.remoteEP = endPoint;
        }

        public void Conectar() {

            handler.Connect(remoteEP);
            Console.WriteLine("Conectado al servidor");
        }

        public string Identificar(string nickname) {

            string jsonNick = Mensaje.Identify("IDENTIFY", nickname);
            byte[] msg = Encoding.UTF8.GetBytes(jsonNick + "<EOM>");

            handler.Send(msg);

            byte[] bytes = new byte[1024];
            int bytesRec = handler.Receive(bytes);
            string serverResp = Encoding.UTF8.GetString(bytes, 0, bytesRec).Replace("<EOM>", "").Trim();

            Response response = Mensaje.Parsear<Response>(serverResp);

            return response?.Result ?? "ERROR";
        }

        public void IniciarConectado() {

            Conectado conectado = new Conectado(handler);
        }
    }
}