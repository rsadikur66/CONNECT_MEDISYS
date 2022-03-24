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
    public class R13021Controller : Controller
    {
        private readonly R13021DAL r13021DAL = new();
        private readonly IWebHostEnvironment hostingEnvironment;
        public R13021Controller(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }
        [HttpGet("/api/r13021/getReport")]
        public IActionResult GetReport(string reqNo)
        {
            // var auth = r13021DAL.GetRolePermission("R13021", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            var auth = "1";
            if (auth == null || auth != "1") return Unauthorized();
            try
            {
                using var report = new FastReport.Report();
                var data = r13021DAL.GetReportData(reqNo);
                var header = r13021DAL.GetHeaderData(HttpContext.Session.GetString("SITE_CODE"));
                var patient = r13021DAL.GetPatientInfo(reqNo);
                data.TableName = "R13021";
                header.TableName = "header";
                patient.TableName = "patient";
               // DataSet ds = new DataSet();
                //ds.Tables.Add(data);
                //ds.Tables.Add(header);
                //ds.Tables.Add(patient);
                //ds.WriteXmlSchema($"{hostingEnvironment.WebRootPath}/XML/R13021.xml");

                report.Load($"{hostingEnvironment.WebRootPath}/reports/R13021.frx");
                report.RegisterData(data, "R13021");
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
