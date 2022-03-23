using ConnectKsmcDAL.Report;
using FastReport.Export.Pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.IO;

namespace ConnectKsmc.Controllers.Report
{
    public class R07061CController : Controller
    {
        private readonly R07061CDAL r07061CDAL = new();
        private readonly IWebHostEnvironment hostingEnvironment;
        public R07061CController(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }
        [HttpGet("/api/r07061C/getReport")]
        public IActionResult GetReport(string reqtNo)
        {
            var auth = r07061CDAL.GetRolePermission("R11018", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            try
            {
                using var report = new FastReport.Report();
                var data = r07061CDAL.GetReportData(reqtNo);
                var header = r07061CDAL.GetHeaderData(HttpContext.Session.GetString("SITE_CODE"));
                data.TableName = "R07061";
                header.TableName = "header";
                report.Load($"{hostingEnvironment.WebRootPath}/reports/R07061C.frx");
                report.RegisterData(data, "R07061");
                report.RegisterData(header, "header");
                report.SetParameterValue("SiteNameArb", HttpContext.Session.GetString("SITE_NAME_ARB"));
                report.SetParameterValue("SiteNameEng", HttpContext.Session.GetString("SITE_NAME_ENG"));
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