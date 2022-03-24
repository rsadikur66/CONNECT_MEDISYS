using ConnectKsmcDAL.Report;
using FastReport.Export.Pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConnectKsmc.Controllers.Report
{
    public class R13011Controller : Controller
    {
        private readonly R13011DAL r13011DAL = new();       
        private readonly IWebHostEnvironment hostingEnvironment;

        public R13011Controller(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {         
            this.hostingEnvironment = hostingEnvironment;
        }


        [HttpGet("/api/r13011/getReport")]
        public IActionResult GetReport(string reqNo)
        {
            // var auth = r30180DAL.GetRolePermission("R13015", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            var auth = "1";
            if (auth == null || auth != "1") return Unauthorized();
            try
            {
                using var report = new FastReport.Report();
                var header = r13011DAL.GetHeaderData(HttpContext.Session.GetString("SITE_CODE"));
                var master = r13011DAL.GetMasterData(reqNo);
                var patInfo = r13011DAL.GetPatInfoData(reqNo);

                header.TableName = "header";
                master.TableName = "master";
                patInfo.TableName = "patInfo";

                //DataSet ds = new DataSet();
                //ds.Tables.Add(header);
                //ds.Tables.Add(master);
                //ds.Tables.Add(patInfo);

                //ds.WriteXmlSchema($"{hostingEnvironment.WebRootPath}/reports/XML/R13011.xml");

                report.Load($"{hostingEnvironment.WebRootPath}/reports/R13011.frx");
                report.RegisterData(header, "header");
                report.RegisterData(master, "master");
                report.RegisterData(patInfo, "patInfo");
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
