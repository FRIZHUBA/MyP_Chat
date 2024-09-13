using Newtonsoft.Json;

namespace SocketCliente {

    class Mensaje {

        public static string Identify(string type, string username) {

            var msg = new {
                type = type,
                username = username
            };

            return JsonConvert.SerializeObject(msg);
        }

        public static T Parsear<T>(string jsonString) {

            try {

                return JsonConvert.DeserializeObject<T>(jsonString);

            } catch {
                
                return default;
            }
        }
    }

    class Response {
        public string Type { get; set; }
        public string Operation { get; set; }

        public string Result { get; set; }
        public string Extra { get; set; }
    }

    class NewUser {

        public string Type { get; set; }
        public string Username { get; set; }
    }
}