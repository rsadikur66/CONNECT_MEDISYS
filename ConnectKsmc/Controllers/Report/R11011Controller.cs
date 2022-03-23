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
    public class R11011Controller : Controller
    {
        private readonly R11011DAL r11011DAL;
        private readonly IWebHostEnvironment hostingEnvironment;
        public R11011Controller(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }
        [HttpGet("/api/r11011/getReport")]
        public IActionResult GetReport(string orderNo)
        {
            var auth = r11011DAL.GetRolePermission("R11011", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            try
            {
                using var report = new FastReport.Report();
                var data = r11011DAL.GetReportData(orderNo);
                var Chaid = r11011DAL.GetQuestionaries(orderNo);
                var header = r11011DAL.GetHeaderData(HttpContext.Session.GetString("SITE_CODE"));
                data.TableName = "R11011";
                Chaid.TableName = "R11011_Chaid";
                header.TableName = "header";
                report.Load($"{hostingEnvironment.WebRootPath}/reports/R11011.frx");
                report.RegisterData(data, "R11011");
                report.RegisterData(Chaid, "R11011_Chaid");
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