using SajamKnjiga.Core.Interfaces;

namespace SajamKnjiga.Core.Models
{
    public class Autor : IEntity
    {
        public int Id { get; set; }
        public string Ime { get; set; } = "";
        public string Prezime { get; set; } = "";
        public DateTime DatumRodjenja { get; set; }
        public Adresa Adresa { get; set; } = new Adresa();
        public string Telefon { get; set; } = "";
        public string Email { get; set; } = "";
        public string BrojLicneKarte { get; set; } = "";
        public int GodineIskustva { get; set; }

        public List<int> KnjigeIds { get; set; } = new List<int>();

        public override string ToString()
        {
            return Ime + " " + Prezime + " | LK: " + BrojLicneKarte + " | " + Email;
        }
    }
}