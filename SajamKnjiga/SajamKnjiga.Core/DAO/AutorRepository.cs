using SajamKnjiga.Core.Models;

namespace SajamKnjiga.Core.DAO
{
    public class AutorRepository : JsonRepository<Autor>
    {
        public AutorRepository() : base("autori.json")
        {
        }
    }
}