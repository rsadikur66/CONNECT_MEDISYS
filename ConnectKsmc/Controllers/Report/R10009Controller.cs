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
    public class R10009Controller : Controller
    {
        private readonly R10009DAL r10009DAL;
        private readonly IWebHostEnvironment hostingEnvironment;
        public R10009Controller(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }
        [HttpGet("/api/r10009/getReport")]
        public IActionResult GetReport(string docCode, string locCode)
        {
            var auth = r10009DAL.GetRolePermission("R10009", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            try
            {
                using var report = new FastReport.Report();
                var data = r10009DAL.GetReport(docCode, locCode);
                var header = r10009DAL.GetHeaderData(HttpContext.Session.GetString("SITE_CODE"));
                data.TableName = "R10009";
                header.TableName = "header";
                report.Load($"{hostingEnvironment.WebRootPath}/reports/R10009.frx");
                report.RegisterData(data, "R10009");
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