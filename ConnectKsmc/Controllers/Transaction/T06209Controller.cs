using ConnectKsmcDAL.Transaction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using FastReport.Export.Pdf;
using System.Data;
using Newtonsoft.Json;

namespace ConnectKsmc.Controllers.Transaction
{
    public class T06209Controller : Controller
    {
        private readonly T06209DAL t06209Dal = new();
        private readonly IWebHostEnvironment hostEnvironment;
        public T06209Controller(IWebHostEnvironment hostingEnvironment)
        {
            this.hostEnvironment = hostingEnvironment;
        }
        [HttpGet("/api/t06209/getBMIindex")]
        public IActionResult GetBMIindex()
        {
            var auth = t06209Dal.GetRolePermission("T06209", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t06209Dal.GetBMIindex(HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t06209/getBPindex")]
        public IActionResult GetBPindex()
        {
            var auth = t06209Dal.GetRolePermission("T06209", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t06209Dal.GetBPindex(HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t06209/getTempindex")]
        public IActionResult GetTempindex()
        {
            var auth = t06209Dal.GetRolePermission("T06209", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t06209Dal.GetTempindex(HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t06209/getPulseindex")]
        public IActionResult GetPulseindex()
        {
            var auth = t06209Dal.GetRolePermission("T06209", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t06209Dal.GetPulseindex(HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t06209/getRRindex")]
        public IActionResult GetRRindex()
        {
            var auth = t06209Dal.GetRolePermission("T06209", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t06209Dal.GetRRindex(HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t06209/getGLindex")]
        public IActionResult GetGLindex()
        {
            var auth = t06209Dal.GetRolePermission("T06209", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t06209Dal.GetGLindex(HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t06209/getMedHxindex")]
        public IActionResult GetMedHxindex()
        {
            var auth = t06209Dal.GetRolePermission("T06209", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t06209Dal.GetMedHxindex(HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t06209/getAllergyDietindex")]
        public IActionResult GetAllergyDietindex()
        {
            var auth = t06209Dal.GetRolePermission("T06209", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t06209Dal.GetAllergyDietindex(HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t06209/getAllergyMedindex")]
        public IActionResult GetAllergyMedindex()
        {
            var auth = t06209Dal.GetRolePermission("T06209", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t06209Dal.GetAllergyMedindex(HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t06209/getRecommendationDropDownlist")]
        public IActionResult GetRecommendationDropDownlist()
        {
            var auth = t06209Dal.GetRolePermission("T06209", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t06209Dal.GetRecommendationDropDownlist(HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t06209/getPatListPopData")]
        public IActionResult GetPatListPopData(string PatNo)
        {
            var auth = t06209Dal.GetRolePermission("T06209", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t06209Dal.GetPatListPopData(PatNo,HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t06209/getDoctorListPopData")]
        public IActionResult GetDoctorListPopData()
        {
            var auth = t06209Dal.GetRolePermission("T06209", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t06209Dal.GetDoctorListPopData(HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t06209/getDocWardEpiDateByType")]
        public IActionResult GetDocWardEpiDateByType(string PAT_TYPE, string PatNo)
        {
            var auth = t06209Dal.GetRolePermission("T06209", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t06209Dal.GetDocWardEpiDateByType(PAT_TYPE, PatNo, HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t06209/getPatientVitalDetails")]
        public IActionResult GetPatientVitalDetails(string PAT_NUMBER, string PAT_TYPE)
        {
            var auth = t06209Dal.GetRolePermission("T06209", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t06209Dal.GetPatientVitalDetails(PAT_NUMBER, PAT_TYPE);
            return Ok(data);
        }
        [HttpGet("/api/t06209/getPatRiskFactor")]
        public IActionResult GetPatRiskFactor(string PAT_NUMBER)
        {
            var auth = t06209Dal.GetRolePermission("T06209", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t06209Dal.GetPatRiskFactor(PAT_NUMBER);
            return Ok(data);
        }
        [HttpPost("/api/T06209/saveData")]
        public IActionResult saveData([FromBody] dynamic data)
        {
            var user = HttpContext.Session.GetString("EMP_CODE");
            var siteCode = HttpContext.Session.GetString("SITE_CODE");
            var auth = t06209Dal.GetRolePermission("T06209", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var result = t06209Dal.SaveData(data, user, siteCode);
            return Ok(JsonConvert.SerializeObject(result));
        }
    }
}
