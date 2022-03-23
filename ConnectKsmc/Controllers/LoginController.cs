using ConnectKsmcDAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConnectKsmc.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginDAL loginDal = new();

        [HttpGet("/api/checkStatus")]
        public IActionResult IsLoggedIn()
        {
            if (HttpContext.Session.GetString("EMP_CODE") != null)
                return Ok(new { msg = "Logged In" });
            else
                return BadRequest(new { msg = "Not Logged In" });
        }

        [HttpPost("/api/login")]
        public IActionResult Login([FromBody] dynamic login)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { msg = "Invalid data, please try again" });

            var user = loginDal.LoginUser(login.T_LOGIN_NAME.ToString().ToUpper(), login.T_PWD.ToString().ToUpper());
            if (user != null)
            {
                HttpContext.Session.SetString("EMP_CODE", user.T_EMP_CODE as string);
                HttpContext.Session.SetString("ROLE_CODE", user.T_ROLE_CODE as string);
                //HttpContext.Session.SetString("SITE_CODE", user.T_SITE_CODE as string);
                //HttpContext.Session.SetString("SITE_NAME_ARB", user.SITE_NAME_ARB as string);
                //HttpContext.Session.SetString("SITE_NAME_ENG", user.SITE_NAME_ENG as string);
                HttpContext.Session.SetString("USER_LANG", user.T_USER_LANG as string);
                return Ok(new
                {
                    BasePath = HttpContext.Request.PathBase,
                    EmpCode = user.T_EMP_CODE,
                    UserName = user.T_USER_NAME,
                    UserRole = user.T_ROLE_CODE,
                    UserLang = user.T_USER_LANG,
                    //SiteCode = user.T_SITE_CODE
                    //SiteNameArb = user.SITE_NAME_ARB,
                    //SiteNameEng = user.SITE_NAME_ENG
                });
            }
            return BadRequest(new { msg = "Invalid ID or Password" });
        }

        [HttpPost("/api/logout")]
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("EMP_CODE") != null)
            {
                HttpContext.Session.Clear();
                return Ok(new { msg = "Logged out successfully" });
            }
            return BadRequest(new { msg = "User already logged out!!!" });
        }
    }
}