using Akavache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace YoApp.Clients.Persistence
{
    /// <summary>
    /// Base class that can be inherited to obtain all basic Akavache CRUD operations.
    /// </summary>
    public class AkavacheContext : IKeyValueStore
    {
        private readonly IBlobCache _blobCache;

        public AkavacheContext()
        {
            BlobCache.ApplicationName = "com.rubit0.YoApp";
            BlobCache.EnsureInitialized();

            _blobCache = BlobCache.UserAccount;
        }

        /// <summary>
        /// Get an object by the provided key.
        /// Returns null if no object is found.
        /// </summary>
        /// <param name="key">Lookup key</param>
        /// <returns>T</returns>
        public virtual async Task<T> Get<T>(string key) where T : class, IKeyProvider
        {
            try
            {
                return await _blobCache.GetObject<T>(key);
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        /// Get an object by the provided key.
        /// Returns null if no object is found.
        /// </summary>
        /// <param name="key">Lookup key</param>
        /// <returns>T as Observable Callback</returns>
        public virtual IObservable<T> GetObservable<T>(string key) where T : class, IKeyProvider
        {
            return _blobCache.GetObject<T>(key)
                .Catch(Observable.Return<T>(null));
        }

        /// <summary>
        /// Get all objects by the provided keys.
        /// Returns null if no object is found.
        /// </summary>
        /// <param name="keys">Keys to look-up</param>
        /// <returns>Key-Value pair Dictionary</returns>
        public virtual async Task<IDictionary<string, T>> Get<T>(IEnumerable<string> keys) where T : class, IKeyProvider
        {
            try
            {
                return await _blobCache.GetObjects<T>(keys);
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        /// Get all objects for T.
        /// Returns null if there are no objects for the given type.
        /// </summary>
        /// <returns>All objects of T</returns>
        public virtual async Task<IEnumerable<T>> GetAll<T>() where T : class, IKeyProvider
        {
            try
            {
                return await _blobCache.GetAllObjects<T>();
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        /// Get all objects for T as an IObservable to subscribe to it's completion callback.
        /// Returns null if there are no objects for the given type.
        /// </summary>
        /// <returns>All objects of T wrapped in IObservable</returns>
        public virtual IObservable<IEnumerable<T>> GetAllObservable<T>() where T : class, IKeyProvider
        {
            return _blobCache.GetAllObjects<T>()
                .Catch(Observable.Return<IEnumerable<T>>(null));
        }

        /// <summary>
        /// Insert a new entity with the key provided by IAkavacheObject.
        /// </summary>
        /// <param name="entity">entity to insert</param>
        public virtual async Task Insert<T>(T entity) where T : class, IKeyProvider
        {
            await _blobCache
                .InsertObject(entity.Key, entity);
        }

        /// <summary>
        /// Insert a range of new object with the keys provided by IAkavacheObject.
        /// </summary>
        /// <param name="entities">entities to insert</param>
        public virtual async Task InsertRange<T>(IEnumerable<T> entities) where T : class, IKeyProvider
        {
            var dict = entities
                .ToDictionary(k => k.Key, e => e);

            await _blobCache.InsertObjects(dict);
        }

        /// <summary>
        /// Check if a entity is already persisted.
        /// </summary>
        /// <param name="entity">Target entity</param>
        /// <returns>Is the entity contained in the store?</returns>
        public virtual async Task<bool> Contains<T>(T entity) where T : class, IKeyProvider
        {
            var obj = await Get<T>(entity.Key);
            return obj != null && obj.Equals(entity);
        }

        /// <summary>
        /// Remove an entity by the given key from the store.
        /// </summary>
        /// <param name="entity">Source key to remove by.</param>
        public virtual async Task Remove<T>(T entity) where T : class, IKeyProvider
        {
            await _blobCache.InvalidateObject<T>(entity.Key);
        }

        /// <summary>
        /// Remove a range of entities by the provided keys.
        /// </summary>
        /// <param name="keys">Source keys to remove by.</param>
        public virtual async Task RemoveRange<T>(IEnumerable<string> keys) where T : class, IKeyProvider
        {
            await _blobCache.InvalidateObjects<T>(keys);
        }

        /// <summary>
        /// Remove all entities by the given type.
        /// </summary>
        public virtual async Task RemoveAll<T>() where T : class, IKeyProvider
        {
            await _blobCache.InvalidateAllObjects<T>();
        }

        /// <summary>
        /// Persist all changes to the store.
        /// </summary>
        public virtual async Task Persist()
        {
            await _blobCache.Flush();
        }
    }
}
