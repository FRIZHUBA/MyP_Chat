using System;
using System.Net;
using System.Net.Sockets;

namespace SocketCliente {

    class Program {

        static void Main(string[] args) {

            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 12500);

            try {

                Cliente cliente = new Cliente(ipAddress.AddressFamily, remoteEP);
                cliente.Conectar();

                bool userExist = true;

                do {

                    Console.WriteLine("Ingrese su nickname:");
                    string nickname = Console.ReadLine();

                    if (string.IsNullOrEmpty(nickname)) {
                        
                        Console.WriteLine("Su nickname no puede estar vacío");
                        continue;
                    }

                    string respuesta = cliente.Identificar(nickname);

                    if (respuesta == "USER_ALREADY_EXISTS") {

                        Console.WriteLine($"El nombre de usuario '{nickname}' ya existe, por favor elija otro");
                        userExist = true;

                    } else if (respuesta == "SUCCESS") {

                        Console.WriteLine($"Usuario '{nickname}' registrado exitosamente");
                        userExist = false;

                    } else {

                        Console.WriteLine("Error desconocido");
                        userExist = true;
                    }
                
                } while (userExist);

                cliente.IniciarConectado();

            } catch (Exception e) {

                Console.WriteLine(e.ToString());
            }
        }
    }
}