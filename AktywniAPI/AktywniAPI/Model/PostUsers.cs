namespace AktywniAPI.Model
{
    public class PostUsers
    {
        public int IdPost { get; set; }
        public string Opis { get; set; } = string.Empty;
        public DateTime DataWpisu { get; set; }
        public int IdU { get; set; }
        public int IdAktywności { get; set; }
        public int Wyświetlenia { get; set; }
        public string Temat { get; set; } = string.Empty;
    }
}
