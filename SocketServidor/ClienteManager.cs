using System.Collections.Generic;

namespace SocketServidor {

    class ClienteManager {

        private Dictionary<string, Conectado> clientes = new Dictionary<string, Conectado>();

        public void AddCliente(Conectado cliente) {

            clientes[cliente.Nick] = cliente;
        }

        public bool ContieneCliente(string nickname) {

            return clientes.ContainsKey(nickname);
        }

        public void EnviarGeneral(string mensaje, Conectado emisor) {

            foreach (var cliente in clientes.Values) {

                if (cliente != emisor) {

                    cliente.Send(mensaje);
                }
            }
        }
    }
}