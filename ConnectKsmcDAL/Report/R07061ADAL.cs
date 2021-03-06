using System.Data;

namespace ConnectKsmcDAL.Report
{
    public class R07061ADAL : CommonDAL
    {
        public DataTable GetHeaderData(string siteCode)
        {
            return ReportQuery($@"SELECT T_SITE_CODE , T_COUNTRY_LANG1_NAME , T_COUNTRY_LANG2_NAME , T_MIN_LANG1_NAME , T_MIN_LANG2_NAME , T_SITE_LANG1_NAME , T_SITE_LANG2_NAME , T_LOGO_ID , T_LOGO_NAME , T_LOGO , T_LCENCE_NO FROM t01028 WHERE T_SITE_CODE IN(SELECT T_SITE_CODE FROM t01001)");
        }
        public DataTable GetReportData(string reqtNo)
        {
            return ReportQuery($@"SELECT '!' || T_REQUEST_NO || '!' BARCODE, T_REQUEST_NO, T_REQUEST_DATE, T07061.T_PAT_NO, T_REQUEST_TIME, T_OBJECT_CONSUL T_REQUEST_RESONE, T_CLINIC_DOC_CODE , T02029.T_NAME_GIVEN ||'' || T02029. T_NAME_FAMILY T_DOC_NAME, TRUNC(MONTHS_BETWEEN(SYSDATE,T03001. T_BIRTH_DATE) / 12, 0) ||'Y ' ||TRUNC(MOD(MONTHS_BETWEEN(SYSDATE,T03001.T_BIRTH_DATE), 12), 0) ||'M ' T_AGE, T_RESPONSE_CONSUL T_RESPONSE_CONSULTATION, T07061.T_RESPONSE_DOC, (SELECT B.T_LANG2_NAME FROM T02039 A, T02040 B WHERE A.T_SPCLTY_CODE=B.T_SPCLTY_CODE AND A.T_DOC_CODE =T07061.T_CLINIC_DOC_CODE )T_RESPONSE_DOC_NAME, TO_CHAR( T_RESPONSE_DATE,'dd/MM/yyyy') T_RESPONSE_DATE, T_RESPONSE_TIME, T_FIRST_LANG2_NAME ||' ' ||T_FATHER_LANG2_NAME ||' ' || T_GFATHER_LANG2_NAME ||' ' ||T_FAMILY_LANG2_NAME T_PAT_NAME, T02003.T_LANG2_NAME T_NATIONALITY, T_MODE_TRANSPORT, ( CASE WHEN T_MODE_TRANSPORT ='1' THEN 'WALKING' WHEN T_MODE_TRANSPORT ='2' THEN 'WHEELCHAIR' WHEN T_MODE_TRANSPORT ='3' THEN 'TROLLEY' WHEN T_MODE_TRANSPORT ='4' THEN 'AMBULANCE' ELSE '' END)T_TRASPORT_MODE, T_REQUEST_TYPE, ( CASE WHEN T_REQUEST_TYPE ='1' THEN 'EMERGENCY' WHEN T_REQUEST_TYPE ='2' THEN 'URGENT' WHEN T_REQUEST_TYPE ='3' THEN 'ROUTINE' WHEN T_REQUEST_TYPE ='4' THEN '4-14 DAYS' ELSE '' END)T_REQUEST_PERIORITY, T_GENDER, ( CASE WHEN T_GENDER ='1' THEN 'MALE' WHEN T_GENDER ='2' THEN 'FEMALE' ELSE '' END)T_GENDER_NAME, T_REQ_SPCLTY , T02040.T_LANG2_NAME T_REQ_SPCLTY_NAME, T07061.T_SUB_SPCLTY , T07055.T_LANG2_NAME T_SUB_SPCLTY_NAME, T_ICD10_CODE , (SELECT T_ICD10_LONG_DESC FROM T06102 WHERE T_ICD10_MAIN_CODE = T_ICD10_CODE AND T_ICD10_TYPE ='1' ) ICD_DESC , (SELECT LANG_NAME FROM V06102 WHERE ICD10_CODE = T_ICD10_CODE ) ICD1_DESC, TO_CHAR(T07061.T_BMI)T_BMI,  TO_CHAR ( T07061.T_BICKS)T_BICKS FROM T07061 JOIN T03001 ON T03001.T_PAT_NO = T07061.T_PAT_NO JOIN T02029 ON T07061.T_CLINIC_DOC_CODE = T02029.T_EMP_NO LEFT JOIN T02040 ON T07061.T_REQ_SPCLTY = T02040.T_SPCLTY_CODE LEFT JOIN T07055 ON T07061.T_REQ_SPCLTY = T07055.T_MAIN_SPEC AND T07061.T_SUB_SPCLTY = T07055.T_SUB_SPEC LEFT JOIN T02003 ON T03001.T_NTNLTY_CODE = T02003.T_NTNLTY_CODE WHERE T_REQUEST_NO ='{reqtNo}'");
        }
    }
}
