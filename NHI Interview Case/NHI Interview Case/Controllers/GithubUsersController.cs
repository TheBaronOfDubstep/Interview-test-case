using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHI_Interview_Case.Model;
using System;
using System.Net.Http;

namespace NHI_Interview_Case.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GithubUsersController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async ActionResult<GitHubUser> GetUsers()
        {
            try
            {
                using (HttpClient c = new HttpClient())
                {
                    HttpResponseMessage res = await c.GetAsync("https://api.github.com/user");
                    if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        // todo: authenticate user
                        //       and rerun response
                        res = await c.GetAsync("https://api.github.com/user")
                    }

                }
            }
            return Forbid();
        }
    }
}
