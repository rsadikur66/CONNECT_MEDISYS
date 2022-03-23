using ConnectKsmcDAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ConnectKsmc.Controllers
{
    public class MenuController : Controller
    {
        private readonly MenuDAL menuDal = new();

        [HttpGet("/api/menu/get")]
        public IActionResult GetMenu(string option, string language)
        {
            if (!HttpContext.Session.Keys.Any())
                return Unauthorized();
            if (language == "1" || language == "2")
                HttpContext.Session.SetString("USER_LANG", language);
            switch (option)
            {
                case "Transaction":
                    var transactionLinks = menuDal.GetMenu(HttpContext.Session.GetString("USER_LANG"), "1", HttpContext.Session.GetString("ROLE_CODE"), HttpContext.Request.PathBase);
                    if (transactionLinks != null)
                        return Ok(transactionLinks);
                    break;
                case "Query":
                    var queryLinks = menuDal.GetMenu(HttpContext.Session.GetString("USER_LANG"), "2", HttpContext.Session.GetString("ROLE_CODE"), HttpContext.Request.PathBase);
                    if (queryLinks != null)
                        return Ok(queryLinks);
                    break;
                case "Report":
                    var reportLinks = menuDal.GetMenu(HttpContext.Session.GetString("USER_LANG"), "3", HttpContext.Session.GetString("ROLE_CODE"), HttpContext.Request.PathBase);
                    if (reportLinks != null)
                        return Ok(reportLinks);
                    break;
                case "Setup":
                    var setupLinks = menuDal.GetMenu(HttpContext.Session.GetString("USER_LANG"), "4", HttpContext.Session.GetString("ROLE_CODE"), HttpContext.Request.PathBase);
                    if (setupLinks != null)
                        return Ok(setupLinks);
                    break;
                case "Security":
                    var securityLinks = menuDal.GetMenu(HttpContext.Session.GetString("USER_LANG"), "5", HttpContext.Session.GetString("ROLE_CODE"), HttpContext.Request.PathBase);
                    if (securityLinks != null)
                        return Ok(securityLinks);
                    break;
            }
            return Ok();
        }
    }
}
