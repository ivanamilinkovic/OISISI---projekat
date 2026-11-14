using SajamKnjiga.Core.Interfaces;

namespace SajamKnjiga.Core.Models
{
    public class Izdavac : IEntity
    {
        public int Id { get; set; }
        public string Sifra { get; set; } = "";
        public string Naziv { get; set; } = "";
        public int? SefId { get; set; }

        public List<int> AutoriIds { get; set; } = new List<int>();
        public List<int> KnjigeIds { get; set; } = new List<int>();

        public override string ToString()
        {
            return Sifra + " | " + Naziv;
        }
    }
}