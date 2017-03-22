using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoApp.Clients.Persistence
{
    public interface IRealmStore : IDisposable
    {
        Realm Instance { get; }

        void Add<T>(T entity) where T : RealmObject;
        void AddRange<T>(IEnumerable<T> entities) where T : RealmObject;
        Task AddAsync<T>(T entity) where T : RealmObject;
        Task AddRangeAsync<T>(IEnumerable<T> entities) where T : RealmObject;
        Task Clear<T>() where T : RealmObject;
        T Find<T>(string key) where T : RealmObject;
        T Find<T>(int key) where T : RealmObject;
        IRealmCollection<T> GetAll<T>() where T : RealmObject;
        IQueryable<T> GetQuerry<T>() where T : RealmObject;
        Task Remove<T>(T entity) where T : RealmObject;
        Task RemoveRange<T>(IQueryable<T> entities) where T : RealmObject;
        Task Update<T>(Action<T> transaction, T entity) where T : RealmObject;
    }
}