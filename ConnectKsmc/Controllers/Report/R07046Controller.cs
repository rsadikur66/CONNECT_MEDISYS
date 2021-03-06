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
    public class R07046Controller : Controller
    {
        private readonly R07046DAL r07046DAL = new();
        private readonly IWebHostEnvironment hostingEnvironment;
        public R07046Controller(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }
        [HttpGet("/api/r07046/getReport")]
        public IActionResult GetReport(string apptNo)
        {
            var auth = r07046DAL.GetRolePermission("R07046", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            try
            {
                using var report = new FastReport.Report();
                var data = r07046DAL.GetBodyData(apptNo);
                var header = r07046DAL.GetHeaderData(HttpContext.Session.GetString("SITE_CODE"));
                data.TableName = "R07046";
                header.TableName = "header";
                report.Load($"{hostingEnvironment.WebRootPath}/reports/R07046.frx");
                report.RegisterData(data, "R07046");
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