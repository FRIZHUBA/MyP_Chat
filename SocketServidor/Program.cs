using System;
using System.Net;

namespace SocketServidor {

    class Program {

        static void Main(string[] args) {

            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 12500);

            Servidor servidor = new Servidor(ipAddress.AddressFamily, localEndPoint);
            servidor.Iniciar();
        }
    }
}