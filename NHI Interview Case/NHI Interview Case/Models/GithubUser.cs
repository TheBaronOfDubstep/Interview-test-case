using System;
using System.Text.Json;

namespace NHI_Interview_Case.Models
{
    public class GithubUser
    {
        public string Login { get; set; }
        public long Id { get; set; }


        public static GithubUser FromJson(string json)
        {
            using JsonDocument jd = JsonDocument.Parse(json);
            return FromJson(jd.RootElement);
        }

        public static GithubUser FromJson(JsonElement json)
        {
            return new GithubUser
            {
                Login = json.GetProperty("login").GetString(),
                Id = json.GetProperty("id").GetInt64()
            };
        }
    }
}
