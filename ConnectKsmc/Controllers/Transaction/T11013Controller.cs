using ConnectKsmcDAL.Transaction;
using FastReport.Export.Pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Transactions;

namespace Connect.Controllers.Transaction
{
    public class T11013Controller : Controller
    {
        private readonly T11013DAL iT11013 = new();
        private readonly IWebHostEnvironment hostingEnvironment;
        public T11013Controller(IWebHostEnvironment _hostingEnvironment)
        {
            this.hostingEnvironment = _hostingEnvironment;
        }
        [HttpGet("/api/t11013/getAllPatientType")]
        public IActionResult GetAllPatientType()
        {
            if (!HttpContext.Session.Keys.Any()) return Unauthorized();
            var data = iT11013.GetAllPatientType(HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t11013/getAllINFPREC")]
        public IActionResult GetAllINFPREC()
        {
            if (!HttpContext.Session.Keys.Any()) return Unauthorized();
            var data = iT11013.GetAllINFPREC(HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t11013/getAllPriority")]
        public IActionResult GetAllPriority()
        {
            if (!HttpContext.Session.Keys.Any()) return Unauthorized();
            var data = iT11013.GetAllPriority(HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t11013/getPriorities")]
        public IActionResult GetPriorities()
        {
            if (!HttpContext.Session.Keys.Any()) return Unauthorized();
            var data = iT11013.GetPriorities(HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t11013/getMeritalStatus")]
        public IActionResult GetMeritalStatus()
        {
            if (!HttpContext.Session.Keys.Any()) return Unauthorized();
            var data = iT11013.GetMeritalStatus(HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t11013/getPatientInfo")]
        public IActionResult GetPatientInfo(string patNo)
        {
            if (!HttpContext.Session.Keys.Any()) return Unauthorized();
            var data = iT11013.GetPatientInfo(patNo, HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t11013/getAllType")]
        public IActionResult GetAllType()
        {
            if (!HttpContext.Session.Keys.Any()) return Unauthorized();
            var data = iT11013.GetAllRadiologyType(HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t11013/getLocationByPatType")]
        public IActionResult GetLocationByPatType(string patType, string patNo)
        {
            var auth = iT11013.GetRolePermission("T11013", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth is null || auth != "1") return Unauthorized();
            var data = iT11013.GetLocationByPatType(patType, patNo, HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t11013/getAllDoctor")]
        public IActionResult GetAllDoctor()
        {
            var auth = iT11013.GetRolePermission("T11013", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth is null || auth != "1") return Unauthorized();
            var data = iT11013.GetAllDoctor(HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t11013/getDoctorInfo")]
        public IActionResult GetDoctorInfo()
        {
            var auth = iT11013.GetRolePermission("T11013", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth is null || auth != "1") return Unauthorized();
            var data = iT11013.GetDoctorInfo(HttpContext.Session.GetString("USER_LANG"), HttpContext.Session.GetString("EMP_CODE"));
            return Ok(data);
        }
        [HttpGet("/api/t11013/getAllProcedure")]
        public IActionResult GetAllProcedure(string procType)
        {
            var auth = iT11013.GetRolePermission("T11013", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth is null || auth != "1") return Unauthorized();
            var data = iT11013.GetAllProcedure(procType, HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
         [HttpGet("/api/t11013/getPatientDetails")]
        public IActionResult GetPatientDetails(string patNo)
        {
            var auth = iT11013.GetRolePermission("T11013", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth is null || auth != "1") return Unauthorized();
            var data = iT11013.GetPatientDetails(patNo, HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t11013/getRadiologyReport")]
        public IActionResult GetRadiologyReport(string orderNo, string radiologyType)
        {
            string role = HttpContext.Session.GetString("ROLE_CODE");
            var auth = iT11013.GetRolePermission("T11013", role)?.T_QRY_ACC.ToString();
            if (auth is null || auth != "1") return Unauthorized();
            try
            {
                using (var report = new FastReport.Report())
                {
                    string siteCode = HttpContext.Session.GetString("SITE_CODE");
                    string lang = HttpContext.Session.GetString("USER_LANG");
                    DataSet dt = new DataSet();
                    DataTable data = iT11013.GetRadiologyData(orderNo, siteCode, lang);
                    dt.Tables.Add(data);
                    dt.DataSetName = "R11011";
                    if (radiologyType == "mamo")
                    {
                        DataTable data1 = iT11013.GetMamoMRIdata(orderNo);
                        dt.Tables.Add(data1);
                        report.RegisterData(data, "Table1");
                        report.RegisterData(data1, "Table2");
                        report.Load($"{hostingEnvironment.WebRootPath}/reports/R11011.frx");
                    }
                    else if (radiologyType == "MRI")
                    {
                        DataTable data1 = iT11013.GetMamoMRIdata(orderNo);
                        dt.Tables.Add(data1);
                        report.RegisterData(data, "Table1");
                        report.RegisterData(data1, "Table2");
                        report.Load($"{hostingEnvironment.WebRootPath}/reports/R11018.frx");
                    }
                    else
                    {
                        DataTable data2 = iT11013.GetProcedureData(orderNo, lang);
                        dt.Tables.Add(data2);
                        report.RegisterData(data, "Table1");
                        report.RegisterData(data2, "Table3");
                        report.Load($"{hostingEnvironment.WebRootPath}/reports/R11012.frx");
                    }
                    //dt.WriteXmlSchema($"{hostingEnvironment.WebRootPath}/xml/R11011.xml");
                    report.RegisterData(dt, "R11011");
                    report.Prepare();
                    using (var ms = new MemoryStream())
                    {
                        var pdfExport = new PDFExport();
                        report.Export(pdfExport, ms);
                        return File(ms.ToArray(), "Application/PDF");
                    }
                }
            }
            catch (Exception e)
            {
                string text = e.Message;
                Console.WriteLine(e.Message);
                return BadRequest(new { msg = "no data found." + e.Message });
            }
        }
        [HttpGet("/api/t11013/getRadiologyRequestList")]
        public IActionResult GetRadiologyRequestList(string patNo)
        {
            var auth = iT11013.GetRolePermission("T11013", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth is null || auth != "1") return Unauthorized();
            var data = iT11013.GetRadiologyRequestList(patNo, HttpContext.Session.GetString("USER_LANG"), HttpContext.Session.GetString("SITE_CODE"));
            return Ok(data);
        }
        [HttpGet("/api/t11013/getRadiologyRequestDetails")]
        public IActionResult GetRadiologyRequestDetails(string orderNo)
        {
            var auth = iT11013.GetRolePermission("T11013", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth is null || auth != "1") return Unauthorized();
            var data = iT11013.GetRadiologyRequestDetails( orderNo, HttpContext.Session.GetString("USER_LANG"), HttpContext.Session.GetString("SITE_CODE"));
            return Ok(data);
        }
        [HttpPost("/api/t11013/saveT11013")]
        public IActionResult SaveT11013([FromBody] dynamic t11013)
        {
            var auth = iT11013.GetRolePermission("T11013", HttpContext.Session.GetString("ROLE_CODE"))?.T_INS_ACC.ToString();
            if (auth is null || auth != "1") return Unauthorized();
            using (var trans = new TransactionScope())
            {
                string orderNo = "";
                orderNo = iT11013.SaveT11011(t11013, HttpContext.Session.GetString("EMP_CODE"), HttpContext.Session.GetString("SITE_CODE"));
                if (!string.IsNullOrEmpty(orderNo))
                {
                    trans.Complete();
                    return Created("", orderNo);
                }
                return BadRequest(new { msg = "Data save failed" });
            }
        }
    }
}