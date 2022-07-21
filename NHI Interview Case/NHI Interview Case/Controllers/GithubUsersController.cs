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
    [ApiVersion("v1.0")]
    [Route("api/[controller]")]
    public class GithubUsersController : ControllerBase
    {

        public GithubUsersController()
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadHttpRequestException))]
        [RequireHttps]
        public ActionResult GetUserLogins()
        {
            return RedirectToAction("GetUserLogins", new { start = 0, pagesize = 10 });
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
#if DEBUG
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
#endif
        [Route("paged/{start}/{pagesize}")]
        [RequireHttps]
        public async Task<ActionResult<string[]>> GetUserLogins(int start, int pagesize)
        {
            if (start < 0 || pagesize < 0)
            {
#if DEBUG
                return BadRequest("Check input parameters");
#else
                return NoContent();
#endif
            }
            List<string> _u = new List<string>();
            try
            {
                using (HttpClient c = new HttpClient())
                {
                    c.DefaultRequestHeaders.Add("User-Agent", "Crew Chief Divider");
                    HttpResponseMessage res = await c.GetAsync(string.Format("https://api.github.com/users?since={0}&per_page={1}", start, pagesize));

                    J.JsonDocument jdoc = J.JsonDocument.Parse(res.Content.ReadAsStringAsync().Result);
                    foreach (J.JsonElement je in jdoc.RootElement.EnumerateArray())
                    {
                        _u.Add(je.GetProperty("login").GetString());
                    }
                    return Ok(_u);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                return BadRequest(ex.Message);
#else
                return NoContent();
#endif
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
#if DEBUG
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
#endif
        [Route("{username}")]
        [RequireHttps]
        public async Task<ActionResult<GithubUserDetails>> GetUserDetails(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
#if DEBUG
                return BadRequest("Check input parameters");
#else
                return NoContent();
#endif
            }
            try
            {
                using (IRepository rep = HttpContext.RequestServices.GetService<IRepository>())
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
            }
            catch (Exception ex)
            {
#if DEBUG
                return BadRequest(ex.Message);
#else
                return NoContent();
#endif
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
#if DEBUG
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
#endif
        [RequireHttps]
        [Route("cached")]
        public ActionResult<GithubUserDetails[]> GetAllCached()
        {
            try
            {
                using (IRepository repo = HttpContext.RequestServices.GetService<IRepository>())
                {
                    return Ok(repo.Users);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                return BadRequest(ex.Message);
#else
                return NoContent();
#endif
            }
        }
    }
}
