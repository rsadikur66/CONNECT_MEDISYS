using System;
using System.Collections.Generic;

namespace ConnectKsmcDAL
{
    public class CommonDAL : DatabaseDAL
    {
        public dynamic GetUserPermission(string T_FORM_CODE, string T_USER_CODE)
        {
            return QuerySingle<dynamic>($"SELECT T_OPN_ACC, T_INS_ACC, T_AMD_ACC, T_DEL_ACC, T_QRY_ACC FROM T01011 WHERE T_FORM_CODE = '{T_FORM_CODE}' AND T_USER_CODE = '{T_USER_CODE}'");
        }

        public dynamic GetRolePermission(string T_FORM_CODE, string T_ROLE_CODE)
        {
            return QuerySingle<dynamic>($"SELECT T_OPN_ACC, T_INS_ACC, T_AMD_ACC, T_DEL_ACC, T_QRY_ACC FROM T01008 WHERE T_FORM_CODE = '{T_FORM_CODE}' AND T_ROLE_CODE = '{T_ROLE_CODE}'");
        }

        public dynamic GetFormInfo(string T_FORM_CODE, string LANGUAGE)
        {
            return QuerySingle<dynamic>($"SELECT T_FORM_CODE, T_LANG{LANGUAGE}_NAME T_FORM_TITLE FROM T01003 WHERE T_FORM_CODE = '{T_FORM_CODE}'");
        }

        public IEnumerable<dynamic> GetAllMessage(string T_MSG_CODE, string LANGUAGE)
        {
            var query = $"SELECT T_MSG_CODE CODE, T_LANG{LANGUAGE}_MSG TEXT FROM T01004 WHERE T_MSG_CODE IN ({T_MSG_CODE})";
            return QueryList<dynamic>(query);
        }

        public IEnumerable<dynamic> GetLabelText(string T_FORM_CODE, string LANGUAGE)
        {
            return QueryList<dynamic>($"SELECT T_LABEL_NAME, T_LANG{LANGUAGE}_TEXT T_LABEL_TEXT FROM T01200 WHERE T_FORM_CODE = '{T_FORM_CODE}'");
        }

        public IEnumerable<dynamic> GetFormLabel(string T_FORM_CODE, string Language)
        {
            var query = $"SELECT T_LABEL_NAME, T_LANG{Language}_TEXT T_LABEL_TEXT FROM T01200 WHERE T_FORM_CODE = '{T_FORM_CODE}'";
            return QueryList<dynamic>(query);
        }

        public IEnumerable<dynamic> GetFormLabelForEdit(string T_FORM_CODE)
        {
            var query = $"SELECT T_FORM_CODE, T_LABEL_NAME, T_LANG1_TEXT, T_LANG2_TEXT FROM T01200 WHERE T_FORM_CODE = '{T_FORM_CODE}'";
            return QueryList<dynamic>(query);
        }

        public bool UpdateFormLabel(string T_FORM_CODE, string T_LABEL_NAME, string T_LANG1_TEXT, string T_LANG2_TEXT)
        {
            var command = $"UPDATE T01200 SET T_LANG1_TEXT = '{T_LANG1_TEXT.Replace("'", "''")}', T_LANG2_TEXT = '{T_LANG2_TEXT.Replace("'", "''")}' WHERE T_FORM_CODE = '{T_FORM_CODE}' AND T_LABEL_NAME = '{T_LABEL_NAME}'";
            return Command(command);
        }

        public string GetUserMsg(string T_MSG_CODE, string LANGUAGE)
        {
            return Convert.ToString(ReportQuery($"SELECT T_MSG_CODE ||' : '||T_{LANGUAGE}_MSG MSG FROM T01004 WHERE T_MSG_CODE = '{T_MSG_CODE}'").Rows[0][0]);
        }
    }
}