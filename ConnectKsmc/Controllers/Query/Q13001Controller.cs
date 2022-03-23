using ConnectKsmcDAL.Query;
using FastReport.Export.Pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.IO;
using System.Linq;

namespace ConnectKsmc.Controllers.Query
{
    public class Q13001Controller : Controller
    {
        private readonly Q13001DAL q13001Dal = new();
        private readonly IWebHostEnvironment hostingEnvironment;
        public Q13001Controller(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }
        [HttpGet("/api/q13001/getPatInfo")]
        public IActionResult GetPatInfo(string patNo)
        {
            string role = HttpContext.Session.GetString("ROLE_CODE");
            var auth = q13001Dal.GetRolePermission("Q13001", role)?.T_QRY_ACC.ToString();
            if (auth is null || auth != "1") return Unauthorized();
            var data = q13001Dal.GetPatInfo(HttpContext.Session.GetString("USER_LANG"), patNo, HttpContext.Session.GetString("SITE_CODE"));
            return Ok(data);
        }
        [HttpGet("/api/q13001/getRequestInfo")]
        public IActionResult GetRequestInfo(string patNo, string lab)
        {
            if (!HttpContext.Session.Keys.Any())
                return Unauthorized();
            var userCode = HttpContext.Session.GetString("EMP_CODE");
            string role = HttpContext.Session.GetString("ROLE_CODE");
            var auth = q13001Dal.GetRolePermission("Q13001", role)?.T_QRY_ACC.ToString();
            if (auth is null || auth != "1") return Unauthorized();
            var data = q13001Dal.GetRequestInfo(patNo, lab, userCode, role);
            return Ok(data);
        }
        [HttpGet("/api/q13001/getRequestDetail")]
        public IActionResult GetRequestDetail(string reqNo, string wsCode)
        {
            string role = HttpContext.Session.GetString("ROLE_CODE");
            var auth = q13001Dal.GetRolePermission("Q13001", role)?.T_QRY_ACC.ToString();
            if (auth is null || auth != "1") return Unauthorized();
            var data = q13001Dal.GetRequestDetail(reqNo, wsCode, HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/q13001/getReport")]
        public IActionResult GetReport(dynamic reqInfo, string reqNo, string labNo, string reportID)
        {
            var auth = q13001Dal.GetRolePermission("Q13001", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            try
            {
                using var report = new FastReport.Report();
                if (string.IsNullOrEmpty(reportID))
                {
                    var data = q13001Dal.GetReportID(Convert.ToString(reqInfo.T_ANALYSIS_CODE), Convert.ToString(reqInfo.T_WS_CODE));
                    if (data != null)
                    {
                        reportID = data.ToList()[0].T_REPORT_ID;
                    }
                }
                DataSet ds = q13001Dal.GetRequestData(reqNo, labNo, HttpContext.Session.GetString("USER_LANG"));

                //DataSet ds = new DataSet();
                //ds.Tables.Add(data);
                //ds.Tables.Add(header);
                //ds.Tables.Add(doctor);
                ds.WriteXmlSchema($"{hostingEnvironment.WebRootPath}/reports/XML/T13001.xml");

                report.Load($"{hostingEnvironment.WebRootPath}/reports/{reportID}.frx");
                report.RegisterData(ds.Tables[0], "T13015");
                report.RegisterData(ds.Tables[1], "T13001");
                report.RegisterData(ds.Tables[2], "T01028");
                //report.SetParameterValue("SiteNameArb", HttpContext.Session.GetString("SITE_NAME_ARB"));
                //report.SetParameterValue("SiteNameEng", HttpContext.Session.GetString("SITE_NAME_ENG"));
                //report.SetParameterValue("visitDate", visitDate);
                report.Prepare();
                using var ms = new MemoryStream();
                var pdfExport = new PDFExport();
                report.Export(pdfExport, ms);
                return File(ms.ToArray(), "Application/PDF");
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }
    }
}
