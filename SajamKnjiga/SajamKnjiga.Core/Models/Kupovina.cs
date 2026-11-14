using SajamKnjiga.Core.Interfaces;

namespace SajamKnjiga.Core.Models
{
    public class Kupovina : IEntity
    {
        public int Id { get; set; }
        public int PosetilacId { get; set; }
        public int KnjigaId { get; set; }
        public DateTime DatumKupovine { get; set; }
        public int Ocena { get; set; }
        public string Komentar { get; set; } = "";

        public override string ToString()
        {
            return "Posetilac " + PosetilacId + " | Knjiga " + KnjigaId + " | " + DatumKupovine.ToString("dd.MM.yyyy.") + " | Ocena: " + Ocena;
        }
    }
}