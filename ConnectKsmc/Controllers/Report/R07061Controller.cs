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
    public class R07061Controller : Controller
    {
        private readonly R07061DAL r07061DAL;
        private readonly IWebHostEnvironment hostingEnvironment;
        public R07061Controller(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }
        [HttpGet("/api/r07061/getReport")]
        public IActionResult GetReport(string reqtNo)
        {
            var auth = r07061DAL.GetRolePermission("R11018", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            try
            {
                using var report = new FastReport.Report();
                var data = r07061DAL.GetReportData(reqtNo);
                var header = r07061DAL.GetHeaderData(HttpContext.Session.GetString("SITE_CODE"));
                data.TableName = "R07061";
                header.TableName = "header";
                report.Load($"{hostingEnvironment.WebRootPath}/reports/R07061.frx");
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