using SajamKnjiga.Core.Models;

namespace SajamKnjiga.Core.DAO
{
    public class KnjigaRepository : JsonRepository<Knjiga>
    {
        public KnjigaRepository() : base("knjige.json")
        {
        }
    }
}