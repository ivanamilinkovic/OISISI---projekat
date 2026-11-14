using SajamKnjiga.Core.Models;

namespace SajamKnjiga.Core.DAO
{
    public class IzdavacRepository : JsonRepository<Izdavac>
    {
        public IzdavacRepository() : base("izdavaci.json")
        {
        }
    }
}