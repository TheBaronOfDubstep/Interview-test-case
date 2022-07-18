using System.Collections.Generic;

namespace NHI_Interview_Case.Models
{
    public interface IRepository
    {
        /// <summary>
        /// Data storage object
        /// </summary>
        IEnumerable<GithubUserDetails> Users { get; }

        void AddItem(GithubUserDetails item);
        void RemoveItem(GithubUserDetails item);

        bool HasItem(GithubUserDetails item);
        bool HasItem(string key);

        GithubUserDetails GetGithubUserDetails(string key);

    }
}
