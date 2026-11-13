namespace SajamKnjiga.Core.Models
{
    public class Adresa
    {
        public string Ulica { get; set; } = "";
        public string Broj { get; set; } = "";
        public string Grad { get; set; } = "";
        public string Drzava { get; set; } = "";

        public override string ToString()
        {
            return Ulica + " " + Broj + ", " + Grad + ", " + Drzava;
        }
    }
}