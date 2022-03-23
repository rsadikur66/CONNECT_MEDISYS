using ConnectKsmcDAL.Transaction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Data;
using FastReport.Export.Pdf;

namespace ConnectKsmc.Controllers.Transaction
{
    public class T07027Controller : Controller
    {
        private readonly T07027DAL t07027Dal = new();
        private readonly IWebHostEnvironment hostEnvironment;
        public T07027Controller(IConfiguration configuration,IWebHostEnvironment hostingEnvironment)
        {
            this.hostEnvironment = hostingEnvironment;
        }
        [HttpGet("/api/t07027/getPatInformation")]
        public IActionResult GetConnectKsmc(string patNo)
        {
            var auth = t07027Dal.GetRolePermission("T07027", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t07027Dal.GetPatInfo(patNo, HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t07027/getClinicSpcltyList")]
        public IActionResult GetClinicSpcltyList()
        {
            var auth = t07027Dal.GetRolePermission("T07027", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t07027Dal.GetClinicSpcltyList(HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t07027/getClinicList")]
        public IActionResult GetClinicList(string SPCLTY_CODE)
        {
            var auth = t07027Dal.GetRolePermission("T07027", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t07027Dal.GetClinicList(SPCLTY_CODE,HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t07027/getSpcltyAndDocByClnCode")]
        public IActionResult GetSpcltyAndDocByClnCode(string T_CLINIC_CODE,string spcltyCode)
        {
            var auth = t07027Dal.GetRolePermission("T07027", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t07027Dal.GetSpcltyAndDocByClnCode(T_CLINIC_CODE, spcltyCode, HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }

        [HttpGet("/api/t07027/getClinicDocList")]
        public IActionResult GetClinicDocList()
        {
            var auth = t07027Dal.GetRolePermission("T07027", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t07027Dal.GetClinicDocList(HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }

        [HttpGet("/api/t07027/getAllAppDates")]
        public IActionResult GetAllAppDates()
        {
            var auth = t07027Dal.GetRolePermission("T07027", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t07027Dal.GetAllAppDates(HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t07027/getPatReqData")]
        public IActionResult GetPatReqData(string PAT_NUMBER)
        {
            var auth = t07027Dal.GetRolePermission("T07027", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t07027Dal.GetPatReqData(PAT_NUMBER,HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        
        [HttpGet("/api/t07027/generateRequestNo")]
        public IActionResult GenerateRequestNo()
        {
            var auth = t07027Dal.GetRolePermission("T07027", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t07027Dal.GenerateRequestNo();
            return Ok(data);
        }

         [HttpPost("/api/t07027/saveData")]
        public IActionResult InsertData([FromBody] dynamic data)
        {
            var auth = t07027Dal.GetRolePermission("T07027", HttpContext.Session.GetString("ROLE_CODE"))?.T_INS_ACC.ToString();
            if (auth is null || auth != "1") return Unauthorized();
            using var trans = new TransactionScope();
            var t07027Insert = t07027Dal.InsertT07027(data.PAT_NO?.ToString(), data.APPT_DATE?.ToString(), data.CLINIC_SPCLTY?.ToString(), data.CLINIC_CODE?.ToString(),data.CLINIC_DOC_CODE?.ToString(), data.REQ_NO?.ToString(), data.REQ_TIME?.ToString(),HttpContext.Session.GetString("EMP_CODE"));
            if (t07027Insert)
                trans.Complete();
            else
                return BadRequest(new { msg = "Data Save failed" });
            return Created("", t07027Insert);
        }

        [HttpGet("/api/t07027/getReport")]
        public IActionResult CreateReport(string T_REQUEST_NO)
        {
            var auth = t07027Dal.GetRolePermission("T07027", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            try
            {
                using var report = new FastReport.Report();
                //DataSet ds = new DataSet();
                DataTable dtReportR07027 = t07027Dal.CreateReprotData(T_REQUEST_NO);
                dtReportR07027.TableName = "R07027";
                //ds.Tables.Add(dtReportR07027);
                //ds.WriteXmlSchema($"{hostEnvironment.WebRootPath}/xml/R07027.xml");
                report.Load($"{hostEnvironment.WebRootPath}/reports/R07027.frx");
                report.RegisterData(dtReportR07027, "R07027");
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
