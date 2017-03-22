using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YoApp.Clients.Persistence
{
    public interface IKeyValueStore
    {
        Task<bool> Contains<T>(T entity) where T : class, IKeyProvider;
        Task<T> Get<T>(string key) where T : class, IKeyProvider;
        Task<IDictionary<string, T>> Get<T>(IEnumerable<string> keys) where T : class, IKeyProvider;
        Task<IEnumerable<T>> GetAll<T>() where T : class, IKeyProvider;
        IObservable<IEnumerable<T>> GetAllObservable<T>() where T : class, IKeyProvider;
        IObservable<T> GetObservable<T>(string key) where T : class, IKeyProvider;
        Task Insert<T>(T entity) where T : class, IKeyProvider;
        Task InsertRange<T>(IEnumerable<T> entities) where T : class, IKeyProvider;
        Task Persist();
        Task Remove<T>(T entity) where T : class, IKeyProvider;
        Task RemoveAll<T>() where T : class, IKeyProvider;
        Task RemoveRange<T>(IEnumerable<string> keys) where T : class, IKeyProvider;
    }
}