using System.Text.Json;

namespace NHI_Interview_Case.Models
{
    public class GithubRepo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }

        public static GithubRepo FromJson(JsonElement json)
        {
            return new GithubRepo()
            {
                Id = json.GetProperty("id").GetInt64(),
                Name = json.GetProperty("name").GetString(),
                FullName = json.GetProperty("full_name").GetString(),
                Url = json.GetProperty("html_url").GetString(),
                Description = json.GetProperty("description").GetString()
            };
        }
    }
}
