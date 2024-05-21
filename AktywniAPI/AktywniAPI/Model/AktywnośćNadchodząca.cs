namespace AktywniAPI.Model
{
    public class AktywnośćNadchodząca
    {
        public int AktywnośćId { get; set; }
        public string Nazwa { get; set; } = string.Empty;
        public string Obrazek { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public TimeSpan GodzinaOd { get; set; }
        public TimeSpan GodzinaDo { get; set; }
        public string MiejsceDoc { get; set; } = string.Empty;
        public string Opis { get; set; } = string.Empty;    
        public int Count { get; set; }

    }
}

