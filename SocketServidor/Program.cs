using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;

namespace SocketServidor {

    class Program {

        static Dictionary<string, Conectado> dic = new Dictionary<string, Conectado>();

        static void Main(string[] args) {

            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 12000);

            try {

                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                listener.Bind(localEndPoint);
                listener.Listen(10);

                Console.WriteLine("Esperando conexión");

                while (true) {

                    Socket handler = listener.Accept();
                    string data = "";
                    byte[] bytes;

                    while (true) {

                        bytes = new byte[1024];
                        int byteRec = handler.Receive(bytes);

                        data += Encoding.ASCII.GetString(bytes, 0, byteRec);

                        if (data.IndexOf("<EOM>") > -1) break;
                    }

                    data = data.Replace("<EOM>", "");

                    Conectado c = new Conectado();

                    c.Nick = data;
                    c.Handler = handler;

                    dic.Add(data, c);
                }

            } catch (Exception e) {

                Console.WriteLine(e.ToString());
            }
        }
    }
}