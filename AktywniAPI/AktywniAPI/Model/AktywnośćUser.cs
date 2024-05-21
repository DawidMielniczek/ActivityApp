using System.Text.Json.Serialization;

namespace AktywniAPI.Model
{
    public class AktywnośćUser
    {
        public int AktywnośćId { get; set; }

        public TimeSpan GodzinaOd { get; set; }

        public TimeSpan GodzinaDo { get; set; }
       
        // [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime Data { get; set; }

        public string Opis { get; set; } = string.Empty;
        public string MiejsceDoc { get; set; } = string.Empty;
        public int IlośćMiejsc { get; set; }
        public int IdAktywności {get; set; }    
        public int Count { get; set; }
        public string Nazwa { get; set; } = string.Empty;
        public string Obrazek { get; set; } = string.Empty;

    }
}
