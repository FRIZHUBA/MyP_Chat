using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace SocketServidor {
    class Conectado {

        private string nick;
        private Socket handler;
        private Thread recibir;
        private ConsoleColor color;

        public Conectado(Socket socket, string username, ConsoleColor color) {

            this.handler = socket;
            this.nick = username;
            this.color = color;
        }

        public void IniciarRecepcion(ClienteManager clienteManager) {

            this.recibir = new Thread( () => Receive(clienteManager) );
            this.recibir.Start();
        }

        public void Send(string mensaje) {

            byte[] msg = Encoding.UTF8.GetBytes(mensaje + "<EOM>");
            handler.Send(msg);
        }

        public void Receive(ClienteManager clienteManager) {

            while(true) {

                string data = RecibirDatos();

                if (! string.IsNullOrEmpty(data)) {

                    if (EsJsonValido(data)) {

                        IdentifyMessage mensaje = Mensaje.Parsear<IdentifyMessage>(data);

                        if (mensaje != null && mensaje.Type == "NEW_USER") {

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Nuevo usuario conectado: {mensaje.Username}");
                            Console.ResetColor();
                            
                        } else {

                            Console.WriteLine("Error al deserializar el mensaje");
                        }

                    } else {

                        Console.ForegroundColor = color;
                        Console.WriteLine($"{nick}: {data}");
                        Console.ResetColor();

                        clienteManager.EnviarGeneral($"{nick}: {data}", this);
                    }
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

        private bool EsJsonValido(string data) {

            data = data.Trim();

            if ((data.StartsWith("{") && data.EndsWith("}")) || 
                (data.StartsWith("[") && data.EndsWith("]"))) {

                try {

                    var obj = Newtonsoft.Json.Linq.JToken.Parse(data);

                    return true;

                } catch (Exception) {

                    return false;
                }
            }

            return false;
        }

        public string Nick { get => nick; }
        public Socket Handler { get => handler; }
    }
}