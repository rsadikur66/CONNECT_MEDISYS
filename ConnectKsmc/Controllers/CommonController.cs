using ConnectKsmcDAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Transactions;

namespace ConnectKsmc.Controllers
{
    public class CommonController : Controller
    {
        private readonly CommonDAL commonDal = new();

        [HttpGet("/api/common/getAllMessage")]
        public IActionResult GetAllMessage(string msgCode)
        {
            var language = HttpContext.Session.Keys.Any() ? HttpContext.Session.GetString("USER_LANG") : "1";
            var formLabel = commonDal.GetAllMessage(msgCode, language);
            return Ok(formLabel);
        }

        [HttpGet("/api/common/getFormInfo")]
        public IActionResult GetFormInfo(string formCode)
        {
            if (!HttpContext.Session.Keys.Any())
                return Unauthorized();
            var formInfo = commonDal.GetFormInfo(formCode, HttpContext.Session.GetString("USER_LANG"));
            return Ok(formInfo);
        }

        [HttpGet("/api/common/getFormLabel")]
        public IActionResult GetFormLabel(string formCode)
        {
            if (!HttpContext.Session.Keys.Any())
                return Unauthorized();
            var formLabel = commonDal.GetFormLabel(formCode, HttpContext.Session.GetString("USER_LANG"));
            return Ok(formLabel);
        }

        [HttpGet("/api/common/getPermission")]
        public IActionResult GetFormPermission(string formCode)
        {
            if (!HttpContext.Session.Keys.Any())
                return Unauthorized();
            var userPermission = commonDal.GetUserPermission(formCode, HttpContext.Session.GetString("EMP_CODE"));
            if (userPermission != null)
            {
                return Ok(new
                {
                    canOpen = userPermission.T_OPN_ACC as string != "2",
                    canSave = userPermission.T_INS_ACC as string != "2",
                    canUpdate = userPermission.T_AMD_ACC as string != "2",
                    canDelete = userPermission.T_DEL_ACC as string != "2",
                    canQuery = userPermission.T_QRY_ACC as string != "2",
                });
            }
            if (HttpContext.Session.GetString("ROLE_CODE") == "0001")
            {
                return Ok(new
                {
                    canOpen = true,
                    canSave = true,
                    canUpdate = true,
                    canDelete = true,
                    canQuery = true,
                });
            }
            var rolePermission = commonDal.GetRolePermission(formCode, HttpContext.Session.GetString("ROLE_CODE"));
            if (rolePermission != null)
            {
                return Ok(new
                {
                    canOpen = rolePermission.T_OPN_ACC as string != "2",
                    canSave = rolePermission.T_INS_ACC as string != "2",
                    canUpdate = rolePermission.T_AMD_ACC as string != "2",
                    canDelete = rolePermission.T_DEL_ACC as string != "2",
                    canQuery = rolePermission.T_QRY_ACC as string != "2",
                });
            }
            return Ok(new
            {
                canOpen = false,
                canSave = false,
                canUpdate = false,
                canDelete = false,
                canQuery = false,
            });
        }

        [HttpGet("/api/common/getFormLabelForEdit")]
        public IActionResult getFormLabelForEdit(string formCode)
        {
            if (!HttpContext.Session.Keys.Any()) return Unauthorized();
            var formLabel = commonDal.GetFormLabelForEdit(formCode);
            return Ok(formLabel);
        }

        [HttpPost("/api/common/updateFormLabel")]
        public IActionResult UpdateFormLabel([FromBody] dynamic t01200)
        {
            if (!HttpContext.Session.Keys.Any()) return Unauthorized();
            if (HttpContext.Session.GetString("ROLE_CODE") != "0001") return Unauthorized();
            var t01200Update = false;
            using (var trans = new TransactionScope())
            {
                foreach (var data in t01200)
                {
                    t01200Update = commonDal.UpdateFormLabel(data.T_FORM_CODE.ToString(), data.T_LABEL_NAME.ToString(), data.T_LANG1_TEXT.ToString(), data.T_LANG2_TEXT.ToString());
                    if (!t01200Update)
                        break;
                }
                if (t01200Update)
                    trans.Complete();
                else
                    return BadRequest(new { msg = "فشلت عملية الحفظ" });
            }
            var formLabel = commonDal.GetFormLabel(t01200[0].T_FORM_CODE.ToString(), HttpContext.Session.GetString("USER_LANG"));
            return Created("", formLabel);
        }
    }
}
