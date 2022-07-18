using System;
using System.Collections.Generic;
using T = System.Timers;

namespace NHI_Interview_Case.Models
{
    public class GithubDataRepository : IRepository
    {
        private readonly List<GithubUserDetails> m_userDetails = new List<GithubUserDetails>();
        public IEnumerable<GithubUserDetails> Users => m_userDetails;

        public bool HasItem(string key)
        {
            return m_userDetails.Exists(u => u.Login == key);
        }

        public void AddItem(GithubUserDetails item)
        {
            if (HasItem(item))
                return;
            m_userDetails.Add(item);
        }

        public void RemoveItem(GithubUserDetails item)
        {
            m_userDetails.Remove(item);
        }

        public bool HasItem(GithubUserDetails item)
        {
            return m_userDetails.Exists(u => u.Id == item.Id);
        }

        public GithubUserDetails GetGithubUserDetails(string key)
        {
            return m_userDetails.Find(u => u.Login == key);
        }
    }
}
