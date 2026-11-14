using SajamKnjiga.Core.Models;

namespace SajamKnjiga.Core.DAO
{
    public class PosetilacRepository : JsonRepository<Posetilac>
    {
        public PosetilacRepository() : base("posetioci.json")
        {
        }
    }
}