using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoApp.Clients.Persistence
{
    public class RealmContext : IRealmStore
    {
        public Realm Instance => Realm.GetInstance();

        public IQueryable<T> GetQuerry<T>() where T : RealmObject
        {
            return Instance.All<T>();
        }

        public T Find<T>(string key) where T : RealmObject
        {
            return Instance.Find<T>(key);
        }

        public T Find<T>(int key) where T : RealmObject
        {
            return Instance.Find<T>(key);
        }

        public IRealmCollection<T> GetAll<T>() where T : RealmObject
        {
            return Instance.All<T>().AsRealmCollection();
        }

        public void Add<T>(T entity) where T : RealmObject
        {
            Instance.Write(() => Instance.Add(entity));
        }

        public void AddRange<T>(IEnumerable<T> entities) where T : RealmObject
        {
            Instance.Write(() =>
            {
                foreach (var entity in entities)
                {
                    Instance.Add(entity);
                }
            });
        }

        public async Task AddAsync<T>(T entity) where T : RealmObject
        {
            await Instance.WriteAsync(r => r.Add(entity));
        }

        public async Task AddRangeAsync<T>(IEnumerable<T> entities) where T : RealmObject
        {
            await Instance.WriteAsync(r =>
            {
                foreach (var entity in entities)
                {
                    r.Add(entity);
                }
            });
        }

        public async Task Remove<T>(T entity) where T : RealmObject
        {
            await Instance.WriteAsync(r => r.Remove(entity));
        }

        public async Task RemoveRange<T>(IQueryable<T> entities) where T : RealmObject
        {
            await Instance.WriteAsync(r => r.RemoveRange(entities));
        }

        public async Task Update<T>(Action<T> transaction, T entity) where T : RealmObject
        {
            await entity.Realm.WriteAsync((r) =>
            {
                transaction.Invoke(entity);
            });
        }

        public async Task Clear<T>() where T : RealmObject
        {
            await Instance.WriteAsync(r => r.RemoveAll<T>());
        }

        public void Dispose()
        {
            Instance.Dispose();
        }
    }
}
