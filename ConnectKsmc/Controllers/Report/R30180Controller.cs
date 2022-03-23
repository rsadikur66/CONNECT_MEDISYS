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
    public class R30180Controller : Controller
    {
        private readonly R30180DAL r30180DAL;
        private readonly IWebHostEnvironment hostingEnvironment;
        public R30180Controller(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }
        [HttpGet("/api/r30180/getReport")]
        public IActionResult GetReport(string patNo, string seqNo)
        {
            var auth = r30180DAL.GetRolePermission("R06201", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            try
            {
                using var report = new FastReport.Report();
                var header = r30180DAL.GetHeaderData(HttpContext.Session.GetString("SITE_CODE"));
                var master = r30180DAL.GetMasterData(patNo);
                var patInfo = r30180DAL.GetPatInfoData(patNo);
                var doctor = r30180DAL.GetDocInfoData(patNo, seqNo);
                var clinic = r30180DAL.GetClinicInfoData(patNo);
                header.TableName = "header";
                master.TableName = "master";
                patInfo.TableName = "patInfo";
                doctor.TableName = "doctor";
                clinic.TableName = "clinic";
                report.Load($"{hostingEnvironment.WebRootPath}/reports/R30180.frx");
                report.RegisterData(header, "header");
                report.RegisterData(master, "master");
                report.RegisterData(patInfo, "patInfo");
                report.RegisterData(doctor, "doctor");
                report.RegisterData(clinic, "clinic");
                report.SetParameterValue("SiteNameArb", HttpContext.Session.GetString("SITE_NAME_ARB"));
                report.SetParameterValue("SiteNameEng", HttpContext.Session.GetString("SITE_NAME_ENG"));
                report.SetParameterValue("MED_SEQ", seqNo);
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