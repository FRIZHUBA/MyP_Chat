using Newtonsoft.Json;

namespace SocketServidor {

    class Mensaje {

        public static T Parsear<T>(string jsonText) {

            try {

                return JsonConvert.DeserializeObject<T>(jsonText);

            } catch (Exception e) {

                Console.WriteLine($"Error al deserializar el JSON: {e.Message}");
                return default(T);
            }
        }

        public static string Response(string operation, string result, string extra) {
            
            var msg = new {
                type = "RESPONSE",
                operation = operation,
                result = result,
                extra = extra
            };

            return JsonConvert.SerializeObject(msg);
        }

        public static string Nuevo(string type, string username) {

            var msg = new {
                type = type,
                username = username
            };

            return JsonConvert.SerializeObject(msg);
        }
    } 

    class IdentifyMessage {
        public string Type { get; set; }
        public string Username { get; set; }
    }
}