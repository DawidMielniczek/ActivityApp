using System.Text.Json.Serialization;

namespace AktywniAPI.Model
{
    public class Users
    {
        public int IdU { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Nick { get; set; } = string.Empty;
        public string Płeć { get; set; } = String.Empty;
        public float Waga { get; set; } = 0;
        public string Bmi { get; set; } = String.Empty;
       
       // [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime RokUrodzenia { get; set; } 

        public int Wzrost { get; set; } = 0;
    }
}
