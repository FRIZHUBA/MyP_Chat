using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SocketCliente {

    class Program {

        static void Main(string[] args) {

            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 12000);

            try {

                Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                
                sender.Connect(remoteEP);

                Console.WriteLine("Ingrese su nickname");
                string mensaje = Console.ReadLine();
                byte[] msg = Encoding.ASCII.GetBytes(mensaje + "<EOM>");
                sender.Send(msg);

                Conectado c = new Conectado();
                c.Handler = sender;

            } catch (Exception e) {

                Console.WriteLine(e.ToString());
            }
        }
    }
}