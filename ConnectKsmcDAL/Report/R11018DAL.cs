﻿using System.Data;

namespace ConnectKsmcDAL.Report
{
    public class R11018DAL : CommonDAL
    {
        public DataTable GetHeaderData(string siteCode)
        {
            return ReportQuery($@"SELECT T_SITE_CODE , T_COUNTRY_LANG1_NAME , T_COUNTRY_LANG2_NAME , T_MIN_LANG1_NAME , T_MIN_LANG2_NAME , T_SITE_LANG1_NAME , T_SITE_LANG2_NAME , T_LOGO_ID , T_LOGO_NAME , T_LOGO , T_LCENCE_NO FROM t01028 WHERE T_SITE_CODE IN(SELECT T_SITE_CODE FROM t01001)");
        }
        public DataTable GetReportData(string orderNo)
        {
            var chk = QueryString($@"SELECT  T11011.T_TMP_FLAG  FROM T11011,T11012 WHERE T11011.T_ORDER_NO = T11012.T_ORDER_NO AND T11011.T_ORDER_NO ='{orderNo}'");
            if (chk == null)
            {
                return ReportQuery($@" SELECT T11011.T_ORDER_NO , '!' || T11011.T_PAT_NO || '!' ORDER_BARCODE , T11011.T_TMP_FLAG , T11011.T_ORDER_DATE , DECODE(T_PRIORITY_CODE,'1','PROTABLE ROUTINE','2','ROUTINE','3','EMERGENCY','4','VERY URGENT','4','PROTABLE EMERGENCY') T_PRIORITY_CODE , T11011.T_PAT_NO , T11011.T_REF_DOC , T11011.T_CLINIC_CODE , T11011.T_FRM_LOCATION , ( CASE WHEN T11011.T_FRM_LOCATION IS NOT NULL THEN ( SELECT T_LANG2_NAME FROM T02042 WHERE T_LOC_CODE = T11011.T_FRM_LOCATION ) WHEN T11011.T_CLINIC_CODE IS NOT NULL THEN ( SELECT T_CLINIC_NAME_LANG2 FROM T07001 WHERE T_CLINIC_CODE = T11011.T_CLINIC_CODE ) ELSE'' END ) FRM_LOC_CLINC_NAME, T11011.T_CLINICAL_DATA , T11011.T_REQUEST_DOC , DECODE(T11011.T_IPOP_FLAG,'1','INPATIENT','2','OPD','3','ER',NULL) T_IPOP_FLAG , T11011.T_CLINIC_SPCLTY_CODE , DECODE(T11011.T_X_HOSP,'1','CENTRAL','2','PEDIA','3','MATERNITY' , NULL) T_X_HOSP , DECODE(T_ALERGY_FLAG,'1','YES','2','NO',NULL) T_ALERGY_FLAG , TO_CHAR( T_LMP_DATE,'dd/MM/yyyy' ) T_LMP_DATE , DECODE(T_PREGNANCY_FLAG,'1','YES','2','NO',NULL) T_PREGNANCY , T11011.T_ORDER_TIME , T11012.T_TYPE_CODE , (SELECT T_LANG2_NAME FROM T11100 WHERE T_MAIN_PROC_CODE = T11012.T_TYPE_CODE)T_TYPE_NAME, T11012.T_PROC_CODE , (SELECT T_PROC_DSCRPTN_LANG2 FROM T11001 WHERE T_PROC_CODE = T11012.T_PROC_CODE) T_PROC_DSCRPTN, T11012.T_PROC_NOTES , T11012.T_INDICATION, T_PAT.T_FIRST_LANG2_NAME||' '||T_PAT.T_FATHER_LANG2_NAME||' '|| T_PAT.T_GFATHER_LANG2_NAME||' '||T_PAT.T_FAMILY_LANG2_NAME T_PAT_NAME , T_PAT.T_BIRTH_DATE , (SELECT TRUNC(MONTHS_BETWEEN(SYSDATE,T_PAT.T_BIRTH_DATE)/12) FROM DUAL ) AGE_Y, (SELECT TRUNC(MOD(MONTHS_BETWEEN(SYSDATE, T_PAT.T_BIRTH_DATE), 12)) FROM DUAL ) AGE_M, (SELECT TRUNC(SYSDATE) - ADD_MONTHS(T_PAT.T_BIRTH_DATE, TRUNC(MONTHS_BETWEEN(SYSDATE,T_PAT.T_BIRTH_DATE))) AS DAYS FROM DUAL)AGE_D, T_PAT.T_GENDER , NVL(T_PAT.T_X_RMC_CHRTNO,T_PAT.T_X_HOSP1_PAT_NO)T_X_HOSP1_PAT_NO , T_PAT.T_NTNLTY_CODE , T_GEN.T_LANG2_NAME T_GENDER_NAME,   T_NAT.T_LANG2_NAME T_NATIONALITY_NAME, T_PAT.T_PAYOR_Y , (CASE WHEN T_PAT.T_PAYOR_Y='1' THEN 'PAID TREATMENT' WHEN T_PAT.T_PAYOR_Y='2' THEN 'ELIGIBLE' WHEN T_PAT.T_PAYOR_Y='3' THEN 'ROYAL ORDER' ELSE '' END )T_PAYOR_Y_STATUS, (SELECT MAX(T_MOBILE_NO) FROM T01009,T35001 WHERE T_EMP_CODE = T_USER_CODE AND T_DOC_CODE = T11011.T_REQUEST_DOC) T_DOC_MOBILE_NO, (SELECT T_NAME_GIVEN || ' '|| T_NAME_FATHER || ' '||T_NAME_GFATHER DOC_NAME FROM T02029 WHERE T_EMP_NO = T11011.T_REQUEST_DOC ) T_DOC_NAME FROM T11011 JOIN T11012 ON T11011.T_ORDER_NO = T11012.T_ORDER_NO JOIN T03001 T_PAT ON T11011.T_PAT_NO = T_PAT.T_PAT_NO JOIN T02006 T_GEN ON T_GEN.T_SEX_CODE = T_PAT.T_GENDER JOIN T02003 T_NAT ON T_PAT.T_NTNLTY_CODE = T_NAT.T_NTNLTY_CODE WHERE T11011.T_ORDER_NO = '{orderNo}'");
            }
            else
            {
                return ReportQuery($@"SELECT T11011.T_ORDER_NO , '!' || T11011.T_PAT_NO || '!' ORDER_BARCODE , T11011.T_TMP_FLAG , T11011.T_ORDER_DATE , DECODE(T_PRIORITY_CODE,'1','PROTABLE ROUTINE','2','ROUTINE','3','EMERGENCY','4','VERY URGENT','4','PROTABLE EMERGENCY') T_PRIORITY_CODE , T11011.T_PAT_NO , T11011.T_REF_DOC , T11011.T_CLINIC_CODE , T11011.T_FRM_LOCATION , ( CASE WHEN T11011.T_FRM_LOCATION IS NOT NULL THEN ( SELECT T_LANG2_NAME FROM T02042 WHERE T_LOC_CODE = T11011.T_FRM_LOCATION ) WHEN T11011.T_CLINIC_CODE IS NOT NULL THEN (SELECT T_CLINIC_NAME_LANG2 FROM T07001 WHERE T_CLINIC_CODE = T11011.T_CLINIC_CODE ) ELSE'' END ) FRM_LOC_CLINC_NAME, T11011.T_CLINICAL_DATA , T11011.T_REQUEST_DOC , DECODE(T11011.T_IPOP_FLAG,'1','INPATIENT','2','OPD','3','ER',NULL) T_IPOP_FLAG , T11011.T_CLINIC_SPCLTY_CODE , DECODE(T11011.T_X_HOSP,'1','CENTRAL','2','PEDIA','3','MATERNITY' , NULL) T_X_HOSP , DECODE(T_ALERGY_FLAG,'1','YES','2','NO',NULL) T_ALERGY_FLAG , TO_CHAR( T_LMP_DATE,'dd/MM/yyyy' ) T_LMP_DATE , DECODE(T_PREGNANCY_FLAG,'1','YES','2','NO',NULL) T_PREGNANCY , T11011.T_ORDER_TIME , T11012.T_TYPE_CODE , (SELECT T_LANG2_NAME FROM T11100 WHERE T_MAIN_PROC_CODE = T11012.T_TYPE_CODE )T_TYPE_NAME, T11012.T_PROC_CODE , (SELECT T_PROC_DSCRPTN_LANG2 FROM T11001 WHERE T_PROC_CODE = T11012.T_PROC_CODE ) T_PROC_DSCRPTN, T11012.T_PROC_NOTES , T11012.T_INDICATION, T_PAT.T_FIRST_LANG2_NAME ||' ' ||T_PAT.T_FATHER_LANG2_NAME ||' ' || T_PAT.T_GFATHER_LANG2_NAME ||' ' ||T_PAT.T_FAMILY_LANG2_NAME T_PAT_NAME , T_PAT.T_BIRTH_DATE , (SELECT TRUNC(MONTHS_BETWEEN(SYSDATE,T_PAT.T_BIRTH_DATE)/12) FROM DUAL ) AGE_Y, (SELECT TRUNC(MOD(MONTHS_BETWEEN(SYSDATE, T_PAT.T_BIRTH_DATE), 12)) FROM DUAL ) AGE_M, (SELECT TRUNC(SYSDATE) - ADD_MONTHS(T_PAT.T_BIRTH_DATE, TRUNC(MONTHS_BETWEEN(SYSDATE,T_PAT.T_BIRTH_DATE))) AS DAYS FROM DUAL )AGE_D, T_PAT.T_GENDER ,  T_GEN.T_LANG2_NAME T_GENDER_NAME,   T_NAT.T_LANG2_NAME T_NATIONALITY_NAME,T_PAT.T_NTNLTY_CODE , T_PAT.T_PAYOR_Y , ( CASE WHEN T_PAT.T_PAYOR_Y='1' THEN 'PAID TREATMENT' WHEN T_PAT.T_PAYOR_Y='2' THEN 'ELIGIBLE' WHEN T_PAT.T_PAYOR_Y='3' THEN 'ROYAL ORDER' ELSE '' END )T_PAYOR_Y_STATUS, (SELECT MAX(T_MOBILE_NO) FROM T01009, T35001 WHERE T_EMP_CODE = T_USER_CODE AND T_DOC_CODE = T11011.T_REQUEST_DOC ) T_DOC_MOBILE_NO, (SELECT T_NAME_GIVEN || ' ' || T_NAME_FATHER || ' ' ||T_NAME_GFATHER DOC_NAME FROM T02029 WHERE T_EMP_NO = T11011.T_REQUEST_DOC ) T_DOC_NAME FROM T11011 JOIN T11012 ON T11011.T_ORDER_NO = T11012.T_ORDER_NO JOIN T03003 T_PAT ON T11011.T_PAT_NO = T_PAT.T_TMP_PAT_NO JOIN T02006 T_GEN ON T_GEN.T_SEX_CODE = T_PAT.T_GENDER JOIN T02003 T_NAT ON T_PAT.T_NTNLTY_CODE = T_NAT.T_NTNLTY_CODE WHERE T11011.T_ORDER_NO = '{orderNo}'");
            }
        }
        public DataTable GetQuestionaries(string orderNo)
        {
            return ReportQuery($@"SELECT Decode(T_I_CAR ,null,'0','1') Cardiac , Decode(T_I_CAR ,null,'1','0') Cardiac_1, Decode(T_I_SUR ,null,'0','1') Surgical , Decode(T_I_SUR ,null,'1','0') Surgical_1 , Decode(T_I_INT ,null,'0','1') Intracranial, Decode(T_I_INT ,null,'1','0') Intracranial_1, Decode(T_I_ART ,null,'0','1') Artificial, Decode(T_I_ART ,null,'1','0') Artificial_1, Decode(T_I_IMF ,null,'0','1') Intraocular, Decode(T_I_IMF ,null,'1','0') Intraocular_1, Decode(T_I_JPF ,null,'0','1') Joint, Decode(T_I_JPF ,null,'1','0') Joint_1, Decode(T_I_MRI ,null,'0','1') MRI, Decode(T_I_MRI ,null,'1','0') MRI_1, Decode(T_I_PSUR ,null,'0','1') Previous, Decode(T_I_PSUR ,null,'1','0') Previous_1, Decode(T_I_PRE ,null,'0','1') pre, Decode(T_I_PRE ,null,'1','0') pre_1, Decode(T_I_KIDNY ,null,'0','1') kid, Decode(T_I_KIDNY ,null,'1','0') kid_1, Decode(T_I_NEURO ,null,'0','1') ner, Decode(T_I_NEURO ,null,'1','0') ner_1, Decode(T_I_HRAID ,null,'0','1') hraid, Decode(T_I_HRAID ,null,'1','0') hraid_1, Decode(T_I_CLAUS ,null,'0','1') claus, Decode(T_I_CLAUS ,null,'1','0') claus_1, Decode(T_I_IPCARD ,null,'0','1') ipcard, Decode(T_I_IPCARD ,null,'1','0') ipcard_1, Decode(T_I_PRGSTU ,null,'0','1') prgstu, Decode(T_I_PRGSTU ,null,'1','0') prgstu_1, Decode(T_I_PENIM ,null,'0','1') penim, Decode(T_I_PENIM ,null,'1','0') penim_1, Decode(T_M_PATCH ,null,'0','1') mpatch, Decode(T_M_PATCH ,null,'1','0') mpatch_1, T_I_COMPAT , Decode(T_I_IVCFIL ,null,'0','1') IVCFIL , Decode(T_I_IVCFIL ,null,'1','0') IVCFIL_1 , Decode(T_I_VASCLIP ,null,'0','1') VASCLIP , Decode(T_I_VASCLIP ,null,'1','0') VASCLIP_1 , Decode(T_I_DENT ,null,'0','1')DENT , Decode(T_I_DENT ,null,'1','0')DENT_1 , Decode(T_I_SWANS ,null,'0','1') SWANS, Decode(T_I_SWANS ,null,'1','0') SWANS_1 FROM T11019 WHERE T_ORDER_NO ='{orderNo}'");
        }
    }
}
