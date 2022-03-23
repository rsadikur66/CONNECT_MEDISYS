using ConnectKsmcDAL.Transaction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Transactions;

namespace ConnectKsmc.Controllers.Transaction
{
    public class T07026Controller : Controller
    {
        private readonly T07026DAL t07026Dal = new();

        [HttpGet("/api/t07026/getAssignDoctor")]
        public IActionResult GetAssignDoctor()
        {
            var auth = t07026Dal.GetRolePermission("T07026", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            try
            {
                var data = t07026Dal.GetAssignDoctor(HttpContext.Session.GetString("EMP_CODE"));
                return Ok(JsonConvert.SerializeObject(data));
            }
            catch (Exception)
            {
                return Ok(JsonConvert.SerializeObject(HttpContext.Session.GetString("EMP_CODE")));
            }
        }

        [HttpGet("/api/t07026/GetAllDoctors")]
        public IActionResult GetAllDoctors(string clinicCode, string apptDate)
        {
            var auth = t07026Dal.GetRolePermission("T07026", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t07026Dal.GetAllDoctors(clinicCode, apptDate, HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }

        [HttpGet("/api/t07026/GetAllClinics")]
        public IActionResult GetAllClinics(string doctorCode)
        {
            var auth = t07026Dal.GetRolePermission("T07026", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t07026Dal.GetAllClinics(doctorCode, HttpContext.Session.GetString("ROLE_CODE"), HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }

        [HttpGet("/api/t07026/getAllApptTypes")]
        public IActionResult GetAllApptTypes()
        {
            var auth = t07026Dal.GetRolePermission("T07026", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t07026Dal.GetAllApptTypes(HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }

        [HttpGet("/api/t07026/getAllArrivalStatus")]
        public IActionResult GetAllArrivalStatus()
        {
            var auth = t07026Dal.GetRolePermission("T07026", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t07026Dal.GetAllArrivalStatus(HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }

        [HttpGet("/api/t07026/getAllICD10")]
        public IActionResult GetAllICD10()
        {
            var auth = t07026Dal.GetRolePermission("T07026", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t07026Dal.GetAllICD10();
            return Ok(data);
        }

        [HttpGet("/api/t07026/getAllDocArrivalStatus")]
        public IActionResult GetAllDocArrivalStatus()
        {
            var auth = t07026Dal.GetRolePermission("T07026", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t07026Dal.GetAllDocArrivalStatus(HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }

        [HttpGet("/api/t07026/isDocOnVacation")]
        public IActionResult IsDocOnVacation(string doctorCode, string apptDate)
        {
            var auth = t07026Dal.GetRolePermission("T07026", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t07026Dal.IsDocOnVacation(doctorCode, apptDate);
            return Ok(data);
        }

        [HttpGet("/api/t07026/getAllAppointments")]
        public IActionResult GetAllAppointments(string doctorCode, string clinicCode, string scheduleRule, string apptDate)
        {
            var auth = t07026Dal.GetRolePermission("T07026", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t07026Dal.GetAllAppointments(doctorCode, clinicCode, scheduleRule, apptDate, HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }

        [HttpGet("/api/t07026/getClinicType")]
        public IActionResult GetClinicType(string clinicCode, string scheduleRule)
        {
            var auth = t07026Dal.GetRolePermission("T07026", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t07026Dal.GetClinicType(clinicCode, scheduleRule);
            if (data == "") data = "1";
            return Ok(JsonConvert.SerializeObject(data));
        }

        [HttpGet("/api/t07026/getFollowupAppointments")]
        public IActionResult GetFollowupAppointments(string doctorCode, string clinicCode)
        {
            var auth = t07026Dal.GetRolePermission("T07026", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t07026Dal.GetFollowupAppointments(doctorCode, clinicCode);
            return Ok(data);
        }

        [HttpGet("/api/t07026/checkUserIsConsultant")]
        public IActionResult CheckUserIsConsultant()
        {
            var auth = t07026Dal.GetRolePermission("T07026", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t07026Dal.CheckUserIsConsultant(HttpContext.Session.GetString("EMP_CODE"));
            return Ok(data);
        }

        [HttpGet("/api/t07026/getFollowupAppointmentsByDays")]
        public IActionResult GetFollowupAppointmentsByDays(string days, string clinicCode)
        {
            var auth = t07026Dal.GetRolePermission("T07026", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t07026Dal.GetFollowupAppointmentsByDays(days, clinicCode);
            return Ok(data);
        }
    }
}
