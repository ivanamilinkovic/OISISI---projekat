using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using SajamKnjiga.Core.Interfaces;
using SajamKnjiga.Core.Utils;

namespace SajamKnjiga.Core.DAO
{
    public abstract class JsonRepository<T> : IRepository<T> where T : class, IEntity
    {
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            WriteIndented = true,                                   
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,  
            Converters = { new JsonStringEnumConverter() }          
        };

        private readonly string _filePath;
        protected List<T> Items;

        protected JsonRepository(string fileName)
        {
            _filePath = DataPaths.GetFilePath(fileName);
            Items = Load();
        }

        public List<T> GetAll()
        {
            return Items;
        }

        public T? GetById(int id)
        {
            return Items.FirstOrDefault(e => e.Id == id);
        }

        public void Add(T entity)
        {
            entity.Id = Items.Count == 0 ? 1 : Items.Max(e => e.Id) + 1;
            Items.Add(entity);
            Save();
        }

        public void Update(T entity)
        {
            int index = Items.FindIndex(e => e.Id == entity.Id);
            if (index < 0)
                throw new KeyNotFoundException("Entitet sa Id=" + entity.Id + " ne postoji.");
            Items[index] = entity;
            Save();
        }

        public void Delete(int id)
        {
            Items.RemoveAll(e => e.Id == id);
            Save();
        }

        public void Save()
        {
            string json = JsonSerializer.Serialize(Items, Options);
            File.WriteAllText(_filePath, json);
        }

        private List<T> Load()
        {
            if (!File.Exists(_filePath))
                return new List<T>();

            string json = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(json))
                return new List<T>();

            return JsonSerializer.Deserialize<List<T>>(json, Options) ?? new List<T>();
        }
    }
}