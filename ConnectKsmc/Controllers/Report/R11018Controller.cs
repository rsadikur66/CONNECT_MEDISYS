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
    public class R11018Controller : Controller
    {
        private readonly R11018DAL r11018DAL;
        private readonly IWebHostEnvironment hostingEnvironment;
        public R11018Controller(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }
        [HttpGet("/api/r11018/getReport")]
        public IActionResult GetReport(string orderNo)
        {
            var auth = r11018DAL.GetRolePermission("R11018", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            try
            {
                using var report = new FastReport.Report();
                var data = r11018DAL.GetReportData(orderNo);
                var Questions = r11018DAL.GetQuestionaries(orderNo);
                var header = r11018DAL.GetHeaderData(HttpContext.Session.GetString("SITE_CODE"));
                data.TableName = "R11018";
                Questions.TableName = "Questions";
                header.TableName = "header";
                report.Load($"{hostingEnvironment.WebRootPath}/reports/R11018.frx");
                report.RegisterData(data, "R11018");
                report.RegisterData(Questions, "Questions");
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