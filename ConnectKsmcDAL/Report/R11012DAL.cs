﻿using System.Data;

namespace ConnectKsmcDAL.Report
{
    public class R11012DAL : CommonDAL
    {
        public DataTable GetHeaderData(string siteCode)
        {
            return ReportQuery($@"SELECT T_SITE_CODE , T_COUNTRY_LANG1_NAME , T_COUNTRY_LANG2_NAME , T_MIN_LANG1_NAME , T_MIN_LANG2_NAME , T_SITE_LANG1_NAME , T_SITE_LANG2_NAME , T_LOGO_ID , T_LOGO_NAME , T_LOGO , T_LCENCE_NO FROM t01028 WHERE T_SITE_CODE IN(SELECT T_SITE_CODE FROM t01001)");
        }
        public DataTable GetReport(string orderNo)
        {
            var chk = QueryString($@"SELECT  T11011.T_TMP_FLAG  FROM T11011,T11012 WHERE T11011.T_ORDER_NO = T11012.T_ORDER_NO AND T11011.T_ORDER_NO ='{orderNo}'");
            if (chk == null)
            {
                return ReportQuery($@"SELECT '!'|| T11011.T_pat_NO || '!' ORDER_BARCODE , T11011.T_ORDER_NO , T11011.T_IPOP_FLAG , (CASE WHEN T11011.T_IPOP_FLAG not in('2','3') THEN ( SELECT T_LANG2_NAME FROM T02042 WHERE T_LOC_CODE =T11011.T_FRM_LOCATION) WHEN T11011.T_IPOP_FLAG='2' THEN (SELECT T_CLINIC_NAME_LANG2 FROM T07001 WHERE T_CLINIC_CODE = NVL(T11011.T_FRM_LOCATION,T_CLINIC_CODE)) WHEN T11011.T_IPOP_FLAG='3' THEN (SELECT T_LANG2_NAME FROM T04003 WHERE T_ER_LOCATION = T11011.T_FRM_LOCATION) ELSE '' END )T_LOC_NAME, '!' || T11011.T_ORDER_NO || '!' ORDER_BARCODE , T11011.T_TMP_FLAG , T11011.T_ORDER_DATE , (SELECT T_URGENCY_DSCRPTN_LANG2 FROM T11002 WHERE T_URGENCY_CODE = T_PRIORITY_CODE ) T_PRIORITY_CODE , decode(T_IV_CONSTRAST,'1','Yes','2','No') IV_CONSTRAST , decode(T_ALERGY_FLAG ,'1','Yes','2','No') ALLERGY_FLAG , DECODE(T_LAB_REQ,'1','Available','2','Requesting') lab_result , decode(T_ANASTHESIA,'1','Yes','0','No') ANASTHESIA , (select t_lang2_name from t11024 WHERE T_INF_PREC = t11011.t_inf_prec) INF_PRECATION , DECODE(T_URGENT_REP,'1','Yes') URGENT_REP , T11011.T_PAT_NO , T11011.T_REF_DOC , T11011.T_FRM_LOCATION , T11011.T_CLINICAL_DATA , T11011.T_REQUEST_DOC , (Select max(T_MOBILE_NO) from t01009,t35001 where t_emp_code = t_user_code and t_doc_code = T11011.T_REQUEST_DOC ) T_DOC_MOBILE, ( SELECT T_NAME_GIVEN || ' '|| T_NAME_FATHER || ' '||T_NAME_GFATHER FROM T02029 WHERE T_EMP_NO = T11011.T_REQUEST_DOC ) T_DOC_NAME , DECODE(T11011.T_IPOP_FLAG,'1','Inpatient','2','OPD','3','ER',null) IPOP_NAME, DECODE(T_ALERGY_FLAG,'1','Yes','2','No',null) T_alergy_flag ,  TO_CHAR( T_LMP_DATE ,'dd/MM/yyyy') T_LMP_DATE, DECODE(T_PREGNANCY_FLAG,'1','Yes','2','No',null) T_PREGNANCY_FLAG , T11011.T_TMP_FLAG , T11011.T_CLINIC_CODE , T11011.T_CLINIC_SPCLTY_CODE , decode(T11011.T_X_HOSP,'1','Central','2','Pedia','3','Maternity' , NULL) T_X_HOSP , T11011.T_ORDER_TIME , T11012.T_TYPE_CODE , ( select t_lang2_name from t11100 where t_main_proc_code = T11012.T_TYPE_CODE) T_TYPE_NAME, ( SELECT T_PROC_DSCRPTN_LANG2 FROM t11001 WHERE T_PROC_CODE = T11012.T_PROC_CODE) T_PROC_NAME, T11012.T_PROC_CODE , T11012.T_PROC_NOTES ,T11012.T_INDICATION, T03001.T_FIRST_LANG2_NAME||' '||T03001.T_FATHER_LANG2_NAME||' '|| T03001.T_GFATHER_LANG2_NAME||' '||T03001.T_FAMILY_LANG2_NAME T_PAT_NAME, T03001.T_BIRTH_DATE , (SELECT TRUNC(MONTHS_BETWEEN(SYSDATE,T03001.T_BIRTH_DATE)/12) FROM DUAL ) AGE_Y, (SELECT TRUNC(MOD(MONTHS_BETWEEN(SYSDATE, T03001.T_BIRTH_DATE), 12)) FROM DUAL ) AGE_M, (SELECT trunc(sysdate) - add_months(T03001.T_BIRTH_DATE, trunc(months_between(sysdate,T03001.T_BIRTH_DATE))) as days FROM DUAL)AGE_D, T03001.T_GENDER , ( CASE WHEN T03001.T_GENDER ='2' THEN 'LMP DATE - Pregnancy' ELSE ' ' END ) T_PREGNANCY, T02006.T_LANG2_NAME T_GENDER_NAME, NVL(T03001.T_X_RMC_CHRTNO,T03001.T_X_HOSP1_PAT_NO) HOSP_NO, T03001.T_NTNLTY_CODE , T02003.T_LANG2_NAME T_NTNLTY_NAME, ( SELECT CASE WHEN COUNT(*) >0 THEN 'Secondary Procedure' ELSE '' END SECONDARY FROM T11013 WHERE T_ORDER_NO = '{orderNo}') SECONDARY FROM T11011 JOIN T11012 ON T11011.T_ORDER_NO = T11012.T_ORDER_NO LEFT JOIN T03001 ON T11011.T_PAT_NO = T03001.T_PAT_NO LEFT JOIN T02006 ON T03001.T_GENDER = T02006.T_SEX_CODE LEFT JOIN T02003 ON T03001.T_NTNLTY_CODE = T02003.T_NTNLTY_CODE WHERE T11011.T_ORDER_NO ='{orderNo}' ");
            }
            else
            {
                return ReportQuery($@"SELECT '!'|| T11011.T_pat_NO || '!' ORDER_BARCODE , T11011.T_ORDER_NO , T11011.T_IPOP_FLAG , (CASE WHEN T11011.T_IPOP_FLAG not in('2','3') THEN ( SELECT T_LANG2_NAME FROM T02042 WHERE T_LOC_CODE =T11011.T_FRM_LOCATION) WHEN T11011.T_IPOP_FLAG='2' THEN (SELECT T_CLINIC_NAME_LANG2 FROM T07001 WHERE T_CLINIC_CODE = NVL(T11011.T_FRM_LOCATION,T_CLINIC_CODE)) WHEN T11011.T_IPOP_FLAG='3' THEN (SELECT T_LANG2_NAME FROM T04003 WHERE T_ER_LOCATION = T11011.T_FRM_LOCATION) ELSE '' END )T_LOC_NAME, '!' || T11011.T_ORDER_NO || '!' ORDER_BARCODE , T11011.T_TMP_FLAG , T11011.T_ORDER_DATE , (SELECT T_URGENCY_DSCRPTN_LANG2 FROM T11002 WHERE T_URGENCY_CODE = T_PRIORITY_CODE ) T_PRIORITY_CODE , decode(T_IV_CONSTRAST,'1','Yes','2','No') IV_CONSTRAST , decode(T_ALERGY_FLAG ,'1','Yes','2','No') ALLERGY_FLAG , DECODE(T_LAB_REQ,'1','Available','2','Requesting') lab_result , decode(T_ANASTHESIA,'1','Yes','0','No') ANASTHESIA , (select t_lang2_name from t11024 WHERE T_INF_PREC = t11011.t_inf_prec) INF_PRECATION , DECODE(T_URGENT_REP,'1','Yes') URGENT_REP , T11011.T_PAT_NO , T11011.T_REF_DOC , T11011.T_FRM_LOCATION , T11011.T_CLINICAL_DATA , T11011.T_REQUEST_DOC , (Select max(T_MOBILE_NO) from t01009,t35001 where t_emp_code = t_user_code and t_doc_code = T11011.T_REQUEST_DOC ) T_DOC_MOBILE, ( SELECT T_NAME_GIVEN || ' '|| T_NAME_FATHER || ' '||T_NAME_GFATHER FROM T02029 WHERE T_EMP_NO = T11011.T_REQUEST_DOC ) T_DOC_NAME , DECODE(T11011.T_IPOP_FLAG,'1','Inpatient','2','OPD','3','ER',null) IPOP_NAME, DECODE(T_ALERGY_FLAG,'1','Yes','2','No',null) T_alergy_flag ,  TO_CHAR( T_LMP_DATE ,'dd/MM/yyyy') T_LMP_DATE, DECODE(T_PREGNANCY_FLAG,'1','Yes','2','No',null) T_PREGNANCY_FLAG , T11011.T_TMP_FLAG , T11011.T_CLINIC_CODE , T11011.T_CLINIC_SPCLTY_CODE , decode(T11011.T_X_HOSP,'1','Central','2','Pedia','3','Maternity' , NULL) T_X_HOSP , T11011.T_ORDER_TIME , T11012.T_TYPE_CODE , ( select t_lang2_name from t11100 where t_main_proc_code = T11012.T_TYPE_CODE) T_TYPE_NAME, ( SELECT T_PROC_DSCRPTN_LANG2 FROM t11001 WHERE T_PROC_CODE = T11012.T_PROC_CODE) T_PROC_NAME, T11012.T_PROC_CODE , T11012.T_PROC_NOTES ,T11012.T_INDICATION, T03001.T_FIRST_LANG2_NAME||' '||T03001.T_FATHER_LANG2_NAME||' '|| T03001.T_GFATHER_LANG2_NAME||' '||T03001.T_FAMILY_LANG2_NAME T_PAT_NAME, T03001.T_BIRTH_DATE , (SELECT TRUNC(MONTHS_BETWEEN(SYSDATE,T03001.T_BIRTH_DATE)/12) FROM DUAL ) AGE_Y, (SELECT TRUNC(MOD(MONTHS_BETWEEN(SYSDATE, T03001.T_BIRTH_DATE), 12)) FROM DUAL ) AGE_M, (SELECT trunc(sysdate) - add_months(T03001.T_BIRTH_DATE, trunc(months_between(sysdate,T03001.T_BIRTH_DATE))) as days FROM DUAL)AGE_D, T03001.T_GENDER , ( CASE WHEN T03001.T_GENDER ='2' THEN 'LMP DATE - Pregnancy' ELSE ' ' END ) T_PREGNANCY, T02006.T_LANG2_NAME T_GENDER_NAME, T03001.T_NTNLTY_CODE , T02003.T_LANG2_NAME T_NTNLTY_NAME, ( SELECT CASE WHEN COUNT(*) >0 THEN 'Secondary Procedure' ELSE '' END SECONDARY FROM T11013 WHERE T_ORDER_NO = '{orderNo}') SECONDARY FROM T11011 JOIN T11012 ON T11011.T_ORDER_NO = T11012.T_ORDER_NO LEFT JOIN T03003 T03001 ON T11011.T_PAT_NO = T03001.T_TMP_PAT_NO LEFT JOIN T02006 ON T03001.T_GENDER = T02006.T_SEX_CODE LEFT JOIN T02003 ON T03001.T_NTNLTY_CODE = T02003.T_NTNLTY_CODE WHERE T11011.T_ORDER_NO ='{orderNo}' ");
            }
        }
        public DataTable SecoundaryProName(string orderNo)
        {
            return ReportQuery($@"SELECT T_PROC_DSCRPTN_LANG2 SECONDARY_PROCEDURE ,A.T_PROC_CODE SECONDARY_CODE FROM T11001 A,T11013 B WHERE A.T_PROC_CODE = B.T_PROC_CODE AND B.T_ORDER_NO = '{orderNo}'");
        }
    }
}
