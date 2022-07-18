using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using J = System.Text.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using NHI_Interview_Case.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace NHI_Interview_Case.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GithubUsersController : ControllerBase
    {

        public GithubUsersController()
        {
        }

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
            List<GithubUser> _u = new List<GithubUser>();
            try
            {
                using (HttpClient c = new HttpClient())
                {
                    c.DefaultRequestHeaders.Add("User-Agent", "Crew Chief Divider");
                    HttpResponseMessage res = await c.GetAsync(string.Format("https://api.github.com/users?since={0}&per_page={1}", start, pagesize));

                    J.JsonDocument jdoc = J.JsonDocument.Parse(res.Content.ReadAsStringAsync().Result);
                    foreach (J.JsonElement je in jdoc.RootElement.EnumerateArray())
                    {
                        _u.Add(GithubUser.FromJson(je));
                    }
                    return Ok(_u);
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
            IRepository rep = HttpContext.RequestServices.GetService<IRepository>();
            try
            {
                if (rep.HasItem(username)) return Ok(rep.GetGithubUserDetails(username));
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
                    rep.AddItem(user);
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
