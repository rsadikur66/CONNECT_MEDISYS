using ConnectKsmcDAL.Transaction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System.Transactions;

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

        [HttpGet("/api/t13115/getAnalysis")]
        public IActionResult GetAnalysis()
        {
            if (!HttpContext.Session.Keys.Any()) return Unauthorized();
            var data = t13115Dal.GetAnalysis();
            return Ok(data);
        }

        [HttpGet("/api/t13115/getAnalysisByWs")]
        public IActionResult getAnalysisByWs(string wsCode)
        {
            if (!HttpContext.Session.Keys.Any()) return Unauthorized();
            var data = t13115Dal.GetAnalysisByWS(wsCode);
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
        [HttpGet("/api/t13115/getAnalysisRestriction")]
        public IActionResult GettAnalysisRestriction(string wsCode, string anaCode)
        {
            var data = t13115Dal.GetAnalysisRestriction(wsCode, anaCode);
            return Ok(data);
        }


        [HttpGet("/api/t13115/getNatOR")]
        public IActionResult GetNatOR(string code)
        {
            var data = t13115Dal.GetNatOR(code, HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t13115/getPatInfo")]
        public IActionResult GetPatInfo(string patNo)
        {
            var auth = t13115Dal.GetRolePermission("T13115", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t13115Dal.GetPatInfo(patNo, HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }

        [HttpGet("/api/t13115/getRequestInfo15")]
        public IActionResult GetRequestT13015(string requestNo)
        {
            var auth = t13115Dal.GetRolePermission("T13115", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t13115Dal.GetRequestT13015(requestNo, HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t13115/getRequestInfo16")]
        public IActionResult GetRequestT13016(string requestNo)
        {
            var data = t13115Dal.GetRequestT13016(requestNo);
            return Ok(data);
        }

        [HttpPost("/api/t13115/insert")]
        public IActionResult InsertT13115([FromBody] dynamic t13115)
        {
            var auth = t13115Dal.GetRolePermission("T13115", HttpContext.Session.GetString("ROLE_CODE"))?.T_INS_ACC.ToString();
            if (auth is null || auth != "1") return Unauthorized();
            using var trans = new TransactionScope();
            var t13015 = t13115.t13015;
            var t13016 = t13115.requestList;
            var requestNo = t13115Dal.Insert13115(t13115, HttpContext.Session.GetString("EMP_CODE"), HttpContext.Session.GetString("SITE_CODE"));
            if (requestNo != null)
                trans.Complete();
            else
                return BadRequest(new { reqNo = "Data Insert failed" });
            return Created("", JsonConvert.SerializeObject(requestNo));
        }
        
        [HttpGet("/api/t13115/getRequestByPatNo")]
        public IActionResult GetRequestByPatNo(string patNo)
        {
            var data = t13115Dal.GetRequestByPatNo(patNo);
            return Ok(data);
        }

    }
}