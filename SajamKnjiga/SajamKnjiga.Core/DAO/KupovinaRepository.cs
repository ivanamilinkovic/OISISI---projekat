using SajamKnjiga.Core.Models;

namespace SajamKnjiga.Core.DAO
{
    public class KupovinaRepository : JsonRepository<Kupovina>
    {
        public KupovinaRepository() : base("kupovine.json")
        {
        }
    }
}