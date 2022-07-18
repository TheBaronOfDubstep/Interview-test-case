using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using J = System.Text.Json;
using NHI_Interview_Case.Model;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using NHI_Interview_Case.Models;

namespace NHI_Interview_Case.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GithubUsersController : ControllerBase
    {
        private readonly IRepository<GithubUser> m_githubUserRepository = new GithubUserRepository();

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult GetUsers()
        {
            return RedirectToAction("GetUserPaged", new { start = 0, pagesize = 10 });
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("paged/{start}/{pagesize}")]
        public async Task<ActionResult<GithubUser[]>> GetUserPaged(int start, int pagesize)
        {
            try
            {
                using (HttpClient c = new HttpClient())
                {
                    c.DefaultRequestHeaders.Add("User-Agent", "Crew Chief Divider");
                    HttpResponseMessage res = await c.GetAsync(string.Format("https://api.github.com/users?since={0}&per_page={1}", start, pagesize));
                    if (res.StatusCode == System.Net.HttpStatusCode.NotFound)
                        throw new ArgumentException("Unable to page users due to erroneous parameters.");
                    J.JsonDocument jdoc = J.JsonDocument.Parse(res.Content.ReadAsStringAsync().Result);
                    foreach (J.JsonElement je in jdoc.RootElement.EnumerateArray())
                    {
                        m_githubUserRepository.AddItem(GithubUser.FromJson(je));
                    }
                    return Ok(m_githubUserRepository.Storage);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{username}")]
        public async Task<ActionResult<GithubUserDetails>> GetUserDetails(string username)
        {
            try
            {
                using (HttpClient c = new HttpClient())
                {
                    c.DefaultRequestHeaders.Add("User-Agent", "Crew Chief Divider");
                    HttpResponseMessage res = await c.GetAsync(string.Format("https://api.github.com/users/{0}", username));
                    J.JsonDocument jdoc = J.JsonDocument.Parse(res.Content.ReadAsStringAsync().Result);
                    GithubUserDetails user = GithubUserDetails.FromJson(jdoc.RootElement);
                    res = await c.GetAsync(string.Format("https://api.github.com/users/{0}/repos?per_page={1}&page={2}", user.Login, 10, 1));
                    jdoc = J.JsonDocument.Parse(res.Content.ReadAsStringAsync().Result);
                    foreach (J.JsonElement elem in jdoc.RootElement.EnumerateArray())
                    {
                        user.Top10Repos.Add(GithubRepo.FromJson(elem));
                    }
                    return Ok(user);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
