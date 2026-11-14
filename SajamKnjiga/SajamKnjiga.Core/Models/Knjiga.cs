using SajamKnjiga.Core.Interfaces;

namespace SajamKnjiga.Core.Models
{
    public class Knjiga : IEntity
    {
        public int Id { get; set; }
        public string Isbn { get; set; } = "";
        public string Naziv { get; set; } = "";
        public string Zanr { get; set; } = "";
        public int GodinaIzdanja { get; set; }
        public decimal Cena { get; set; }
        public int BrojStrana { get; set; }
        public int? IzdavacId { get; set; }

        public List<int> AutoriIds { get; set; } = new List<int>();
        public List<int> KupciIds { get; set; } = new List<int>();
        public List<int> ListaZeljaIds { get; set; } = new List<int>();

        public override string ToString()
        {
            return Isbn + " | " + Naziv + " | " + Cena + " RSD";
        }
    }
}