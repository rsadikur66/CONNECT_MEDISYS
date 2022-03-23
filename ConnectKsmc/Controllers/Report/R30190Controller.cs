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
    public class R30190Controller : Controller
    {
        private readonly R30190DAL r30190DAL;
        private readonly IWebHostEnvironment hostingEnvironment;
        public R30190Controller(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }
        [HttpGet("/api/r30190/getReport")]
        public IActionResult GetReport(string fromDate, string toDate, string stock)
        {
            var auth = r30190DAL.GetRolePermission("R30190", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            try
            {
                using var report = new FastReport.Report();
                var data = r30190DAL.GetReport(fromDate, toDate, stock);
                var header = r30190DAL.GetHeaderData(HttpContext.Session.GetString("SITE_CODE"));
                data.TableName = "R30190";
                header.TableName = "header";
                report.Load($"{hostingEnvironment.WebRootPath}/reports/R30190.frx");
                report.RegisterData(data, "R30190");
                report.RegisterData(header, "header");
                report.SetParameterValue("SiteNameArb", HttpContext.Session.GetString("SITE_NAME_ARB"));
                report.SetParameterValue("SiteNameEng", HttpContext.Session.GetString("SITE_NAME_ENG"));
                report.SetParameterValue("DateFrom", fromDate);
                report.SetParameterValue("DateTo", toDate);
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