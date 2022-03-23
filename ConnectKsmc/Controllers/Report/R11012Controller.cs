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
    public class R11012Controller : Controller
    {
        private readonly R11012DAL r11012DAL;
        private readonly IWebHostEnvironment hostingEnvironment;

        public R11012Controller(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }
        [HttpGet("/api/r11012/getReport")]
        public IActionResult GetReport(string orderNo)
        {
            var auth = r11012DAL.GetRolePermission("R11012", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            try
            {
                using var report = new FastReport.Report();
                var data = r11012DAL.GetReport(orderNo);
                var Secoundary = r11012DAL.SecoundaryProName(orderNo);
                var header = r11012DAL.GetHeaderData(HttpContext.Session.GetString("SITE_CODE"));
                data.TableName = "R11012";
                Secoundary.TableName = "Secoundary";
                header.TableName = "header";
                report.Load($"{hostingEnvironment.WebRootPath}/reports/R11012.frx");
                report.RegisterData(data, "R11012");
                report.RegisterData(Secoundary, "Secoundary");
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