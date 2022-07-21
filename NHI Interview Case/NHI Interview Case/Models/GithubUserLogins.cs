using System;
using System.Text.Json;

namespace NHI_Interview_Case.Models
{
    public class GithubUserLogins
    {
        public string Login { get; set; }
        public long Id { get; set; }


        public static GithubUserLogins FromJson(string json)
        {
            using JsonDocument jd = JsonDocument.Parse(json);
            return FromJson(jd.RootElement);
        }

        public static GithubUserLogins FromJson(JsonElement json)
        {
            return new GithubUserLogins
            {
                Login = json.GetProperty("login").GetString(),
                Id = json.GetProperty("id").GetInt64()
            };
        }
    }
}
