using System.Data;

namespace ConnectKsmcDAL.Report
{
    public class R10009DAL : CommonDAL
    {
        public DataTable GetHeaderData(string siteCode)
        {
            return ReportQuery($@"SELECT T_SITE_CODE , T_COUNTRY_LANG1_NAME , T_COUNTRY_LANG2_NAME , T_MIN_LANG1_NAME , T_MIN_LANG2_NAME , T_SITE_LANG1_NAME , T_SITE_LANG2_NAME , T_LOGO_ID , T_LOGO_NAME , T_LOGO , t_lcence_no FROM t01028 WHERE t_site_code IN(select t_site_code FROM t01001)");
        }
        public DataTable GetReport(string docCode, string locCode)
        {
            return ReportQuery($@"SELECT a.T_ENTRY_DATE, C.T_RACK_ROW_CODE, C.T_LAST_TR_DATE, a.T_ENTRY_USER, a.T_UPD_DATE, a.T_UPD_USER, A.T_PAT_NO , A.T_LOC_CODE, D.T_LANG2_NAME T_LOC_NAME, NULL T_X_HOSP1_PAT_NO , A.T_DOC_CODE , C.T_X_RMC_CHRTNO , SUBSTR(C.T_X_RMC_CHRTNO,6,3) T_X_RMC_CHRTNO, B.T_FIRST_LANG2_NAME ||' ' ||B.T_GFATHER_LANG2_NAME ||' ' ||B.T_FAMILY_LANG2_NAME T_PAT_NAME, (SELECT distinct t_user_name T_LAST_TR_BY FROM T10001,t01009 WHERE T_LAST_TR_BY=t_emp_code and T_FILE_NO = A.T_PAT_NO )T_LAST_TR_BY FROM T10009 A , T03001 B, T10001 C, T10002 D WHERE A.T_PAT_NO = B.T_PAT_NO AND A.T_PAT_NO = C.T_FILE_NO AND A.T_LOC_CODE = D.T_LOC_CODE AND ( A.T_DOC_CODE='{docCode}' or A.T_LOC_CODE = '{locCode}') ORDER BY SUBSTR(C.T_X_RMC_CHRTNO,6,3)");
        }
    }
}
