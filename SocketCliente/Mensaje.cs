using Newtonsoft.Json;

namespace SocketCliente {

    class Mensaje {

        public static T Parsear<T>(string jsonString) {

            try {

                return JsonConvert.DeserializeObject<T>(jsonString);

            } catch {
                
                return default;
            }
        }

        public static bool EsJsonValido(string data) {

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

        public static string Identify(string type, string username) {

            var msg = new {
                type = type,
                username = username
            };

            return JsonConvert.SerializeObject(msg);
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