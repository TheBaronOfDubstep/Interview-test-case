using System;
using System.Collections.Generic;
using System.Text.Json;

namespace NHI_Interview_Case.Models
{
    public class GithubUserDetails : GithubUserLogins
    {
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public string Location { get; set; }
        public int PublicRepos { get; set; }
        public List<GithubRepo> Top10Repos { get; private set; } = new List<GithubRepo>();

        public static new GithubUserDetails FromJson(string json)
        {
            using JsonDocument jd = JsonDocument.Parse(json);
            return FromJson(jd.RootElement);
        }

        public static new GithubUserDetails FromJson(JsonElement json)
        {
            return new GithubUserDetails
            {
                Login = json.GetProperty("login").GetString(),
                Created = json.GetProperty("created_at").GetDateTime(),
                Location = json.GetProperty("location").GetString(),
                Name = json.GetProperty("name").GetString(),
                Id = json.GetProperty("id").GetInt64(),
                PublicRepos = json.GetProperty("public_repos").GetInt32()
            };
        }
    }
}
