namespace SajamKnjiga.Core.Interfaces
{
    public interface IRepository<T> where T : class, IEntity
    {
        List<T> GetAll();
        T? GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}