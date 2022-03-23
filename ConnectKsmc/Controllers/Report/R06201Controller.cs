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
    public class R06201Controller : Controller
    {
        private readonly R06201DAL r06201DAL = new();
        private readonly IWebHostEnvironment hostingEnvironment;
        public R06201Controller(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }
        [HttpGet("/api/r06201/getReport")]
        public IActionResult GetReport(string patNo, string visitDate)
        {
            var auth = r06201DAL.GetRolePermission("R06201", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            try
            {
                using var report = new FastReport.Report();
                var data = r06201DAL.GetReport(patNo, visitDate);
                var header = r06201DAL.GetHeaderData(HttpContext.Session.GetString("SITE_CODE"));
                var doctor = r06201DAL.GetDoctorInfo(HttpContext.Session.GetString("EMP_CODE"));
                data.TableName = "R06201";
                header.TableName = "header";
                doctor.TableName = "doctor";
                //DataSet ds = new DataSet();
                //ds.Tables.Add(data);
                //ds.Tables.Add(header);
                //ds.Tables.Add(doctor);
                //ds.WriteXmlSchema($"{hostingEnvironment.WebRootPath}/reports/XML/R06201.xml");

                report.Load($"{hostingEnvironment.WebRootPath}/reports/R06201.frx");
                report.RegisterData(data, "R06201");
                report.RegisterData(header, "header");
                report.RegisterData(doctor, "doctor");
                report.SetParameterValue("SiteNameArb", HttpContext.Session.GetString("SITE_NAME_ARB"));
                report.SetParameterValue("SiteNameEng", HttpContext.Session.GetString("SITE_NAME_ENG"));
                report.SetParameterValue("visitDate", visitDate);
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
