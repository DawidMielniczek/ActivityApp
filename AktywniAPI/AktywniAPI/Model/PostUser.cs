namespace AktywniAPI.Model
{
    public class PostUser
    {
        public int IdPost { get; set; }
        public string Nazwa { get; set; } = string.Empty;
        public string Obrazek { get; set; } = string.Empty;
        public string Opis { get; set; } = string.Empty;
        public DateTime DataWpisu { get; set; }
        public int IdU { get; set; }
        public int Wyświetlenia { get; set; }
        public string Temat { get; set; } = string.Empty;
        public string Nick { get; set; } = string.Empty;
        public int IdAktywności { get; set; }
    }
}
