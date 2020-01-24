namespace LRUCache.Core
{
    public interface ICache<T>
    {
        /// <summary>
        /// Get item stored in cache. If no item found null is returned
        /// </summary>
        /// <param name="key">Key for requested item</param>
        /// <returns>Item from cache or null</returns>
         T Get(string key);

         /// <summary>
         /// Store item in cache
         /// </summary>
         /// <param name="key">Key to associate with given value</param>
         /// <param name="value">Item to store</param>
         void Add(string key, T value);
    }
}