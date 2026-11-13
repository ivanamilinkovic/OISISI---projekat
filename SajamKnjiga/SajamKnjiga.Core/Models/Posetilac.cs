using SajamKnjiga.Core.Interfaces;

namespace SajamKnjiga.Core.Models
{
    public class Posetilac : IEntity
    {
        public int Id { get; set; }
        public string Ime { get; set; } = "";
        public string Prezime { get; set; } = "";
        public DateTime DatumRodjenja { get; set; }
        public Adresa Adresa { get; set; } = new Adresa();
        public string KontaktTelefon { get; set; } = "";
        public string Email { get; set; } = "";
        public string BrojClanskeKarte { get; set; } = "";
        public int TrenutnaGodinaClanstva { get; set; }
        public StatusPosetioca Status { get; set; }
        public double ProsecnaOcena { get; set; }

        public List<int> KupovineIds { get; set; } = new List<int>();
        public List<int> ListaZeljaIds { get; set; } = new List<int>();

        public override string ToString()
        {
            return BrojClanskeKarte + " | " + Ime + " " + Prezime + " | " + Status;
        }
    }
}