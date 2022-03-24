using ConnectKsmcDAL.Report;
using FastReport.Export.Pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConnectKsmc.Controllers.Report
{
    public class R13111Controller : Controller
    {
        private readonly R13111DAL r13111DAL = new();
        private readonly IWebHostEnvironment hostingEnvironment;
        public R13111Controller(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }
        [HttpGet("/api/r13111/getReport")]
        public IActionResult GetReport(string reqNo)
        {
            // var auth = r13021DAL.GetRolePermission("R13021", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            var auth = "1";
            if (auth == null || auth != "1") return Unauthorized();
            try
            {
                using var report = new FastReport.Report();
                var data = r13111DAL.GetReportData(reqNo);
                var header = r13111DAL.GetHeaderData(HttpContext.Session.GetString("SITE_CODE"));
                var patient = r13111DAL.GetPatientInfo(reqNo);
                data.TableName = "R13111";
                header.TableName = "header";
                patient.TableName = "patient";
                //DataSet ds = new DataSet();
                //ds.Tables.Add(data);
                //ds.Tables.Add(header);
                //ds.Tables.Add(patient);
                //ds.WriteXmlSchema($"{hostingEnvironment.WebRootPath}/XML/R13111.xml");

                report.Load($"{hostingEnvironment.WebRootPath}/reports/R13111.frx");
                report.RegisterData(data, "R13111");
                report.RegisterData(header, "header");
                report.RegisterData(patient, "patient");
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
