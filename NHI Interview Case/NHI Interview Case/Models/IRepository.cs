using System.Collections.Generic;

namespace NHI_Interview_Case.Models
{
    public interface IRepository<T> where T : class, new()
    {
        /// <summary>
        /// Data storage object
        /// </summary>
        IEnumerable<T> Storage { get; }

        void AddItem(T item);
        void RemoveItem(T item);

        bool HasItem(T item);
        bool HasItem(string key);

        T Get(string key);

    }
}
