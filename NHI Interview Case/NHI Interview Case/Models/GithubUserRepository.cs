using NHI_Interview_Case.Model;
using System;
using System.Collections.Generic;
using T = System.Timers;

namespace NHI_Interview_Case.Models
{
    public class GithubUserRepository : IRepository<GithubUser>
    {

        private readonly List<GithubUser> m_users = new List<GithubUser>();
        public IEnumerable<GithubUser> Storage => m_users;

        /// <summary>
        /// Adds a GithubUser to the storage collection
        /// </summary>
        /// <param name="item">GithubUser to add</param>
        public void AddItem(GithubUser item)
        {
            // check if already exists
            if (HasItem(item))
                return;
            m_users.Add(item);
        }
        /// <summary>
        /// Checks whether or not the given item is present in the proxy
        /// </summary>
        /// <param name="item">Item to look for.</param>
        /// <returns>true if present, or false if not present</returns>
        public bool HasItem(GithubUser item)
        {
            GithubUser? poss = m_users.Find(u => u.Id == item.Id);
            return poss == null ? false : true;
        }
        /// <summary>
        /// Removes item from storage
        /// </summary>
        /// <param name="item">GithubUser to remove</param>
        public void RemoveItem(GithubUser item)
        {
            m_users.Remove(item);
        }

        public bool HasItem(string key)
        {
            GithubUser? poss = m_users.Find(u => u.Login == key);
            return poss == null ? false : true;
        }

        public GithubUser Get(string key)
        {
            return m_users.Find(u => u.Login == key);
        }
    }
}
