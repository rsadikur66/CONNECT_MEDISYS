using ConnectKsmcDAL.Transaction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ConnectKsmc.Controllers.Transaction
{
    public class T06201Controller : Controller
    {
        private readonly T06201DAL t06201Dal = new();

        [HttpGet("/api/t06201/getPatientType")]
        public IActionResult GetPatientType(string patNo)
        {
            string emp_code = HttpContext.Session.GetString("EMP_CODE");
            var auth = t06201Dal.GetRolePermission("T06201", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t06201Dal.GetPatientType();
            return Ok(data);
        }
        [HttpGet("/api/t06201/getPatientTypeInfo")]
        public IActionResult GetPatientTypeInfo(string value, string patNo)
        {
            string emp_code = HttpContext.Session.GetString("EMP_CODE");
            var auth = t06201Dal.GetRolePermission("T06201", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t06201Dal.GetPatientTypeInfo(value, patNo);
            return Ok(data);
        }
        [HttpGet("/api/t06201/getPatientInfo")]
        public IActionResult GetPatientInfo(string patNo)
        {
            var auth = t06201Dal.GetRolePermission("T06201", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t06201Dal.GetPatientInfo(patNo, HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t06201/getDetails")]
        public IActionResult GetDetails(string patNo, string patType)
        {
            var auth = t06201Dal.GetRolePermission("T06201", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t06201Dal.GetDetails(patNo, patType);
            return Ok(data);
        }
        [HttpPost("/api/t06201/saveData")]
        public IActionResult SaveData([FromBody] dynamic datas)
        {
            var user = HttpContext.Session.GetString("EMP_CODE");
            var auth = t06201Dal.GetRolePermission("T12305", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t06201Dal.SaveData(datas, user);
            return Ok(JsonConvert.SerializeObject(data));
        }
        [HttpGet("/api/t06201/getDoctorInfo")]
        public IActionResult GetDoctorInfo()
        {
            var auth = t06201Dal.GetRolePermission("T06201", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t06201Dal.GetDoctorInfo(HttpContext.Session.GetString("EMP_CODE"));
            return Ok(data);
        }
    }
}