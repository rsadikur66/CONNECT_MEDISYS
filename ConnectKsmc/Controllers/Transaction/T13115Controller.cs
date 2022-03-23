using ConnectKsmcDAL.Transaction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ConnectKsmc.Controllers.Transaction
{
    public class T13115Controller : Controller
    {
        private readonly T13115DAL t13115Dal = new();
       
        [HttpGet("/api/t13115/getAllPatientType")]
        public IActionResult GetAllPatientType()
        {
            if (!HttpContext.Session.Keys.Any()) return Unauthorized();
            var data = t13115Dal.GetAllPatientType(HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t13115/getPriorities")]
        public IActionResult GetPriorities()
        {
            if (!HttpContext.Session.Keys.Any()) return Unauthorized();
            var data = t13115Dal.GetPriorities(HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t13115/getAllWorkStation")]
        public IActionResult GetAllWorkStation()
        {
            if (!HttpContext.Session.Keys.Any()) return Unauthorized();
            var data = t13115Dal.GetAllWorkStation(HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t13115/getAnalysisNew")]
        public IActionResult GetAnalysisNew(string wsCode)
        {
            //var auth = t13115Dal.GetRolePermission("T13115", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            //if (auth is null || auth != "1") return Unauthorized();
            var data = t13115Dal.GetAnalysisNew(wsCode, HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
    }
}