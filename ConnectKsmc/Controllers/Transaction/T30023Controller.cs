using ConnectKsmcDAL.Transaction;
using FastReport.Export.Pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace ConnectKsmc.Controllers.Transaction
{
    public class T30023Controller : Controller
    {
        private readonly T30023DAL t30023Dal = new();
        private readonly IWebHostEnvironment _hostingEnvironment;
        public T30023Controller(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet("/api/t30023/getAllPatient")]
        public IActionResult GetAllPatient(string T_PAT_NO)
        {
            var auth = t30023Dal.GetRolePermission("T30023", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t30023Dal.GetAllPatients(HttpContext.Session.GetString("SITE_CODE"), T_PAT_NO, HttpContext.Session.GetString("USER_LANG"));
            return Ok(data);
        }
        [HttpGet("/api/t30023/getAllData")]
        public IActionResult GetAllData()
        {
            var auth = t30023Dal.GetRolePermission("T30023", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            string lang = HttpContext.Session.GetString("USER_LANG");
            var dataSp = t30023Dal.GetSpeciality(lang);
            //var dataDoc = t30023Dal.GetDoc(lang, "");
            var dataDuration = t30023Dal.GetDurationList(lang);
            var dataMedicine = t30023Dal.GetMedicineList(lang);
            var dataTrade = t30023Dal.GetMedicineListbyTrade(lang);
            var dataFrequency = t30023Dal.GetFrequencyList(lang);
            var dataUM = t30023Dal.GetUMList(lang);
            var dataIns = t30023Dal.GetInsList(lang);
            return Ok(new
            {
                Specialist = JsonConvert.SerializeObject(dataSp),
                Medicine = JsonConvert.SerializeObject(dataMedicine),
                Duration = JsonConvert.SerializeObject(dataDuration),
                Trade = JsonConvert.SerializeObject(dataTrade),
                Frequency = JsonConvert.SerializeObject(dataFrequency),
                UM = JsonConvert.SerializeObject(dataUM),
                Ins = JsonConvert.SerializeObject(dataIns)
            });
        }
        [HttpGet("/api/t30023/getDoc")]
        public IActionResult GetDoc(string spclty)
        {
            var auth = t30023Dal.GetRolePermission("T30023", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            string lang = HttpContext.Session.GetString("USER_LANG");
            var data = t30023Dal.GetDoc(lang, spclty);
            return Ok(JsonConvert.SerializeObject(data));
        }
        [HttpGet("/api/t30023/getSpeciality")]
        public IActionResult GetSpeciality()
        {
            var auth = t30023Dal.GetRolePermission("T30023", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            string lang = HttpContext.Session.GetString("USER_LANG");
            var data = t30023Dal.GetSpeciality(lang);
            return Ok(JsonConvert.SerializeObject(data));
        }
        [HttpGet("/api/t30023/getLocation")]
        public IActionResult GetLocation(string type, string doc)
        {
            var auth = t30023Dal.GetRolePermission("T30023", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized(); string lang = HttpContext.Session.GetString("USER_LANG");
            string user = HttpContext.Session.GetString("EMP_CODE");
            var data = t30023Dal.GetLocation(lang, user, type, doc);
            return Ok(JsonConvert.SerializeObject(data));
        }
        [HttpGet("/api/t30023/getICD10")]
        public IActionResult GetICD10()
        {
            var auth = t30023Dal.GetRolePermission("T30023", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized(); string lang = HttpContext.Session.GetString("USER_LANG");
            var data = t30023Dal.GetICD10(lang);
            return Ok(data);
        }
        [HttpGet("/api/t30023/getSlipList")]
        public IActionResult GetSlipList(string type, string clinic, string patNo, string tempPatNo, string slip)
        {
            var auth = t30023Dal.GetRolePermission("T30023", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            string lang = HttpContext.Session.GetString("USER_LANG");
            var data = t30023Dal.GetSlipList(lang, type, clinic, patNo, tempPatNo, slip);
            return Ok(data);
        }
        [HttpGet("/api/t30023/getMedicineList")]
        public IActionResult GetMedicineList()
        {
            var auth = t30023Dal.GetRolePermission("T30023", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            string lang = HttpContext.Session.GetString("USER_LANG");
            var data = t30023Dal.GetMedicineList(lang);
            return Ok(JsonConvert.SerializeObject(data));
        }
        [HttpGet("/api/t30023/getMedicineListbySpeciality")]
        public IActionResult GetMedicineListbySpeciality(string speciality, string location)
        {
            var auth = t30023Dal.GetRolePermission("T30023", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            string lang = HttpContext.Session.GetString("USER_LANG");
            var data = t30023Dal.GetMedicineListbySpeciality(lang, speciality, location);
            return Ok(JsonConvert.SerializeObject(data));
        }
        [HttpGet("/api/t30023/getMedicineListbyTrade")]
        public IActionResult GetMedicineListbyTrade()
        {
            var auth = t30023Dal.GetRolePermission("T30023", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            string lang = HttpContext.Session.GetString("USER_LANG");
            var data = t30023Dal.GetMedicineListbyTrade(lang);
            return Ok(JsonConvert.SerializeObject(data));
        }
        [HttpGet("/api/t30023/getFrequencyList")]
        public IActionResult GetFrequencyList()
        {
            var auth = t30023Dal.GetRolePermission("T30023", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            string lang = HttpContext.Session.GetString("USER_LANG");
            var data = t30023Dal.GetFrequencyList(lang);
            return Ok(JsonConvert.SerializeObject(data));
        }
        [HttpGet("/api/t30023/getDurationList")]
        public IActionResult GetDurationList()
        {
            var auth = t30023Dal.GetRolePermission("T30023", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            string lang = HttpContext.Session.GetString("USER_LANG");
            var data = t30023Dal.GetDurationList(lang);
            return Ok(JsonConvert.SerializeObject(data));
        }
        [HttpGet("/api/t30023/getUMList")]
        public IActionResult GetUMList()
        {
            var auth = t30023Dal.GetRolePermission("T30023", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            string lang = HttpContext.Session.GetString("USER_LANG");
            var data = t30023Dal.GetUMList(lang);
            return Ok(JsonConvert.SerializeObject(data));
        }
        [HttpGet("/api/t30023/getInsList")]
        public IActionResult GetInsList()
        {
            var auth = t30023Dal.GetRolePermission("T30023", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            string lang = HttpContext.Session.GetString("USER_LANG");
            var data = t30023Dal.GetInsList(lang);
            return Ok(JsonConvert.SerializeObject(data));
        }
        [HttpGet("/api/t30023/getPatData")]
        public IActionResult GetPatData(string pat, string slip)
        {
            var auth = t30023Dal.GetRolePermission("T30023", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            string lang = HttpContext.Session.GetString("USER_LANG");
            var data = t30023Dal.GetPatData(lang, pat, slip);
            return Ok(JsonConvert.SerializeObject(data));
        }
        [HttpGet("/api/t30023/getPatSingle")]
        public IActionResult GetPatSingle(string pat)
        {
            var auth = t30023Dal.GetRolePermission("T30023", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            string lang = HttpContext.Session.GetString("USER_LANG");
            string emp = HttpContext.Session.GetString("EMP_CODE");
            var data = t30023Dal.GetPatSingle(lang, pat, emp);
            return Ok(JsonConvert.SerializeObject(data));
        }
        [HttpGet("/api/t30023/getPatType")]
        public IActionResult GetPatType(string pat)
        {
            var auth = t30023Dal.GetRolePermission("T30023", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t30023Dal.GetPatType(pat);
            return Ok(JsonConvert.SerializeObject(data));
        }
        [HttpGet("/api/t30023/getSlipValidation")]
        public IActionResult GetSlipValidation(string doc, string pat, string slip)
        {
            var auth = t30023Dal.GetRolePermission("T30023", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            string lang = HttpContext.Session.GetString("USER_LANG");
            string user = HttpContext.Session.GetString("EMP_CODE");
            var data = t30023Dal.GetSlipValidation(lang, user, doc, pat, slip);
            return Ok(JsonConvert.SerializeObject(data));
        }
        [HttpGet("/api/t30023/getTradeGen")]
        public IActionResult GetTradeGen()
        {
            var auth = t30023Dal.GetRolePermission("T30023", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            string lang = HttpContext.Session.GetString("USER_LANG");
            var data = t30023Dal.GetTradeGen(lang);
            return Ok(JsonConvert.SerializeObject(data));
        }
        [HttpPost("/api/t30023/save")]
        public IActionResult Save([FromBody] List<dynamic> t30023)
        {
            var auth = t30023Dal.GetRolePermission("T30023", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            string user = HttpContext.Session.GetString("EMP_CODE");
            var data = t30023Dal.Save(t30023, user);
            return Ok(data);
        }
        [HttpPost("/api/t30023/update")]
        public IActionResult Update([FromBody] List<dynamic> t30023)
        {
            var auth = t30023Dal.GetRolePermission("T30023", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            string user = HttpContext.Session.GetString("EMP_CODE");
            var data = t30023Dal.Update(t30023, user);
            return Ok(data);
        }
        [HttpGet("/api/t30023/getSpecialityIns")]
        public IActionResult GetSpecialityIns(string docCode, string specCode, string genCode, string routeCode, string mFormCode)
        {
            var auth = t30023Dal.GetRolePermission("T30023", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            var data = t30023Dal.GetSpecialityIns(docCode, specCode, genCode, routeCode, mFormCode);
            return Ok(data);
        }
        [HttpGet("/api/t30023/getPrescriptionData")]
        public IActionResult GetPrescriptionData(string patNo, string slipNo, string fDate, string tDate)
        {
            var auth = t30023Dal.GetRolePermission("T30023", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            string lang = HttpContext.Session.GetString("USER_LANG");
            var data = t30023Dal.GetPrescriptionData(patNo, slipNo, fDate, tDate, lang);
            return Ok(data);
        }
        [HttpGet("/api/t30023/GetPatInfoT03001")]
        public IActionResult GetPatInfoT03001(string patNo)
        {
            var auth = t30023Dal.GetRolePermission("T30023", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            string lang = HttpContext.Session.GetString("USER_LANG");
            var validityInfo = t30023Dal.GetValitionInfo(patNo);
            if (validityInfo != "0")
            {
                var msg = "";
                if (validityInfo == "1")
                {
                    msg = "Caution....This Patient is Admitted or not valid!!!";
                }
                else if (validityInfo == "2")
                {
                    msg = lang == "1" ? "صحح إدخال تاريخ الميلاد" : "Entry of this Birth Date will cause an improbable age (over 200 yrs)";
                }
                else if (validityInfo == "3")
                {
                    msg = "This Patient can be Prescribed as OPD or Discharge-IP. If you want to Prescribe as Discharge-IP than Please Change the Patient Type !!";
                }
                else if (validityInfo == "4")
                {
                    msg = "This Patient can be Prescribed as OPD or ER Patient. If you want to Prescribe as ER than Please Change the Patient Type !!";
                }
                return Ok(new { validity = validityInfo, msg = msg, data = "" });
            }
            var data = t30023Dal.GetPatInfoT03001(patNo, lang);
            return Ok(new { validity = validityInfo, msg = "", data = Ok(data) });
        }
        [HttpGet("/api/t30023/getMedicineStatus")]
        public IActionResult GetMedicineStatus(string itemCode, string strength, string routeCode, string formCode, string genCode)
        {
            var auth = t30023Dal.GetRolePermission("T30023", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            string lang = HttpContext.Session.GetString("USER_LANG");
            var data = t30023Dal.GetMedicineStatus(itemCode, strength, routeCode, formCode, genCode);
            return Ok(data);
        }
        [HttpGet("/api/t30023/getDrugHistory")]
        public IActionResult GetDrugHistory(string patNo)
        {
            var auth = t30023Dal.GetRolePermission("T30023", HttpContext.Session.GetString("ROLE_CODE"))?.T_QRY_ACC.ToString();
            if (auth == null || auth != "1") return Unauthorized();
            string lang = HttpContext.Session.GetString("USER_LANG");
            var dataAsOP = t30023Dal.GetDrugHistoryAsOutPaient(patNo, lang);
            var dataAsIP = t30023Dal.GetDrugHistoryAsInPaient(patNo, lang);
            return Ok(new { drug_out_pat = dataAsOP, drug_in_pat = dataAsIP });
        }
        [HttpGet("/api/t30023/getReportMedicineHistoryBySlipNo")]
        public IActionResult GetReportMedicineHistoryBySlipNo(string patNo, string slipNo)
        {
            if (!HttpContext.Session.Keys.Any())
                return Unauthorized();
            try
            {
                using (var report = new FastReport.Report())
                {
                    var lang = HttpContext.Session.GetString("USER_LANG");
                    var sitecode = HttpContext.Session.GetString("SITE_CODE");
                    DataSet dt = new DataSet();
                    DataTable Header = t30023Dal.GetReportHeader();
                    DataTable PatInfo = t30023Dal.GetReportPatInfo(sitecode, patNo);
                    DataTable MedicineList = t30023Dal.GetReportMedicineListBySlipNo(HttpContext.Session.GetString("USER_LANG"), HttpContext.Session.GetString("SITE_CODE"), slipNo, patNo);
                    dt.Tables.Add(Header);
                    dt.Tables.Add(PatInfo);
                    dt.Tables.Add(MedicineList);
                    dt.DataSetName = "R30014";
                    Header.TableName = "Header";
                    PatInfo.TableName = "Table1";
                    MedicineList.TableName = "Table2";
                    //MedicineList.Columns.Add("T_MORNING_INSTRUCTION_NAME", typeof(String));
                    //MedicineList.Columns.Add("T_NOON_INSTRUCTION_NAME", typeof(String));
                    //MedicineList.Columns.Add("T_NIGHT_INSTRUCTION_NAME", typeof(String));
                    //for (int i = 0; i < MedicineList.Rows.Count; i++)
                    //{
                    //    if (MedicineList.Rows[i]["T_MORNING_INSTRUCTION"] != null)
                    //    {
                    //        var morning = 
                    //            it30014.GetInstructionName(HttpContext.Session.GetString("USER_LANG"), MedicineList.Rows[i]["T_MORNING_INSTRUCTION"].ToString());
                    //        if (morning.Rows.Count > 0)
                    //            MedicineList.Rows[i]["T_MORNING_INSTRUCTION_NAME"] = morning.Rows[0]["T_INSTRUCTION_NAME"].ToString();
                    //        var name = MedicineList.Rows[i]["T_MORNING_INSTRUCTION_NAME"].ToString();
                    //    }
                    //    if (MedicineList.Rows[i]["T_NOON_INSTRUCTION"] != null)
                    //    {
                    //        var noon =
                    //            it30014.GetInstructionName(HttpContext.Session.GetString("USER_LANG"), MedicineList.Rows[i]["T_NOON_INSTRUCTION"].ToString());
                    //        if (noon.Rows.Count > 0)
                    //            MedicineList.Rows[i]["T_NOON_INSTRUCTION_NAME"] = noon.Rows[0]["T_INSTRUCTION_NAME"].ToString();
                    //        var name = MedicineList.Rows[i]["T_NOON_INSTRUCTION_NAME"].ToString();
                    //    }
                    //    if (MedicineList.Rows[i]["T_NIGHT_INSTRUCTION"] != null)
                    //    {
                    //        var night =
                    //            it30014.GetInstructionName(HttpContext.Session.GetString("USER_LANG"), MedicineList.Rows[i]["T_NIGHT_INSTRUCTION"].ToString());
                    //        if (night.Rows.Count > 0)
                    //            MedicineList.Rows[i]["T_NIGHT_INSTRUCTION_NAME"] = night.Rows[0]["T_INSTRUCTION_NAME"].ToString();
                    //        var name = MedicineList.Rows[i]["T_NIGHT_INSTRUCTION_NAME"].ToString();
                    //    }
                    //}
                    //dt.WriteXmlSchema($"{hostingEnvironment.WebRootPath}/xml/R30014.xml");
                    report.Load($"{_hostingEnvironment.WebRootPath}/reports/R30014.frx");
                    report.RegisterData(Header, "Header");
                    report.RegisterData(PatInfo, "Table1");
                    report.RegisterData(MedicineList, "Table2");
                    report.RegisterData(dt, "R30014");
                    report.SetParameterValue("lang", lang);
                    report.Prepare();
                    using (var ms = new MemoryStream())
                    {
                        var pdfExport = new PDFExport();
                        report.Export(pdfExport, ms);
                        return File(ms.ToArray(), "Application/PDF");
                    }
                }
            }
            catch (Exception e)
            {
                string text = e.Message;
                Console.WriteLine(e.Message);
                return BadRequest(new { msg = "no data found." });
            }
        }
    }
}