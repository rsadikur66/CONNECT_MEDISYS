using System.Collections.Generic;
using System.Data;

namespace ConnectKsmcDAL.Transaction
{
    public class T07027DAL : CommonDAL
    {
        public IEnumerable<dynamic> GetPatInfo(string patNo, string lang)
        {
            return QueryList<dynamic>($"SELECT t.T_FIRST_LANG1_NAME||' '||t.T_FATHER_LANG1_NAME||' '|| t.T_GFATHER_LANG1_NAME || ' ' || t.T_FAMILY_LANG1_NAME ARB_NAME, t.T_FIRST_LANG2_NAME || ' ' || t.T_FATHER_LANG2_NAME || ' ' || t.T_GFATHER_LANG2_NAME || ' ' || t.T_FAMILY_LANG2_NAME ENG_NAME, t.T_RLGN_CODE,t.T_GENDER, t.T_NTNLTY_CODE,t.T_BIRTH_DATE, t.T_MRTL_STATUS, (select T_LANG{lang}_NAME from T02007 where t_mrtl_status_code = t.T_MRTL_STATUS) mrtl_status_dscrptn, (select T_LANG{lang}_NAME from T02003 where t_ntnlty_code = t.T_NTNLTY_CODE) ntnlty_dscrptn, (select T_LANG{lang}_NAME from T02006 where t_sex_code = t.T_GENDER) gender_dscrptn, (select T_LANG{lang}_NAME from T02005 where t_rlgn_code = t.T_RLGN_CODE) rlgn_dscrptn,(select trunc(months_between(sysdate,t.T_BIRTH_DATE)/12) from dual) AGE_Y,(SELECT trunc(mod(months_between(sysdate, t.T_BIRTH_DATE), 12)) from dual) AGE_M, t.T_NTNLTY_ID , t.T_MOBILE_NO ,t.T_PAT_NO FROM T03001 t where t.T_pat_no = '{patNo}'");
        }
        public IEnumerable<dynamic> GetClinicSpcltyList(string lang)
        {
            return QueryList<dynamic>($"SELECT T_SPCLTY_CODE, T_LANG{lang}_NAME T_SPCLTY_NAME FROM T02040");
        }
        public IEnumerable<dynamic> GetClinicList(string SPCLTY_CODE, string lang)
        {
            return QueryList<dynamic>($"SELECT T_CLINIC_CODE,T_CLINIC_NAME_LANG{lang} CLINIC_NAME FROM T07001 WHERE T_ACTIVE_FLAG IS NOT NULL AND T_BOOKING_CLINIC IS NULL AND T_CLINIC_SPCLTY_CODE = NVL({SPCLTY_CODE},T_CLINIC_SPCLTY_CODE) ORDER BY 2");
        }
        public IEnumerable<dynamic> GetSpcltyAndDocByClnCode(string T_CLINIC_CODE, string spcltyCode, string lang)
        {
            lang = lang == "1" ? "_ARB" : "";
            return QueryList<dynamic>($"SELECT t_clinic_name_lang2, t_clinic_spclty_code, t_clinic_doc_code, (select T_LANG2_NAME spclty_Dscrptn from T02040 where t_spclty_code = t_clinic_spclty_code)clinic_spclty_desc, ( SELECT LTRIM(LTRIM(RTRIM( NVL(t2.T_NAME_GIVEN{lang}, ' '), ' '), ' ')|| ' ', ' ') || LTRIM(LTRIM(RTRIM( NVL(t2.T_NAME_FATHER{lang}, ' '), ' '), ' ')|| ' ', ' ') || LTRIM(RTRIM( NVL(t2.T_NAME_FAMILY{lang}, ' '), ' '), ' ') DOC_ARB FROM T02039 t JOIN T02029 t2 ON t.T_DOC_CODE = t2.T_EMP_NO WHERE T_DOC_CODE = t_clinic_doc_code) docName from t07001 where t_clinic_code = '{T_CLINIC_CODE}' and t_clinic_spclty_code = nvl(null,t_clinic_spclty_code) and t_active_flag is not null and t_booking_clinic is null");
        }
        public IEnumerable<dynamic> GetClinicDocList(string lang)
        {
            lang = lang == "1" ? "_ARB" : "";
            return QueryList<dynamic>($"select LTRIM(LTRIM(RTRIM( NVL(T_NAME_GIVEN{lang}, ' '), ' '), ' ')||' ', ' ') || LTRIM(LTRIM(RTRIM( NVL(T_NAME_FATHER{lang}, ' '), ' '), ' ')||' ', ' ') || LTRIM(RTRIM( NVL(T_NAME_FAMILY{lang}, ' '), ' '), ' ') DOC_NAME, T_DOC_CODE from t02029,t02039 where t_emp_no = t_doc_code");
        }
        public IEnumerable<dynamic> GetAllAppDates(string lang)
        {
            return QueryList<dynamic>($"select t_lang{lang}_name NAME,T_TIME_CODE from t07022 order by t_time_code");
        }
        public IEnumerable<dynamic> GetPatReqData(string PAT_NUMBER, string lang)
        {
            return QueryList<dynamic>($"SELECT t.T_REQUEST_NO,t.T_REQUEST_TIME, t.T_PAT_NO , t.T_APPT_DATE , t.T_CLINIC_SPCLTY , ( SELECT T_LANG{lang}_NAME T_SPCLTY_NAME FROM T02040 WHERE T_SPCLTY_CODE = t.T_CLINIC_SPCLTY) SPCLTY_DESC, t.T_CLINIC_CODE , ( SELECT r.T_CLINIC_NAME_LANG{lang} CLINIC_NAME FROM T07001 r WHERE r.T_CLINIC_CODE = t.T_CLINIC_CODE) CLINIC_DESC, t.T_CLINIC_DOC_CODE , (SELECT LTRIM(LTRIM(RTRIM( NVL(T_NAME_GIVEN, ' '), ' '), ' ')|| ' ', ' ') || LTRIM(LTRIM(RTRIM( NVL(T_NAME_FATHER, ' '), ' '), ' ')|| ' ', ' ') || LTRIM(RTRIM( NVL(T_NAME_FAMILY, ' '), ' '), ' ') FROM t02029 t WHERE t.T_EMP_NO = t.T_CLINIC_DOC_CODE) CLINIC_DOC_NAME , TO_CHAR(t.T_REQUEST_DATE, 'dd/MM/yyyy') T_REQUEST_DATE FROM T07027 t WHERE t.t_pat_no = '{PAT_NUMBER}' AND t.t_request_date = trunc(sysdate)");
        }
        public dynamic GenerateRequestNo()
        {
            return QuerySingle<dynamic>($"SELECT LPAD(T07027_SEQ.NEXTVAL,10,'0')  REQ_NO, TO_CHAR(TRUNC(SYSDATE),'dd/MM/yyyy') REQUEST_DATE,TO_CHAR(SYSDATE,'HH24MI') REQUEST_TIMEE FROM DUAL");
        }
        public bool InsertT07027(string PAT_NO, string APPT_DATE, string CLINIC_SPCLTY, string CLINIC_CODE, string CLINIC_DOC_CODE, string REQ_NO, string REQ_TIME, string T_ENTRY_USER)
        {
            return Command($@" INSERT INTO T07027 T (T.T_ENTRY_DATE,T.T_ENTRY_USER,T.T_PAT_NO,T.T_APPT_DATE,T.T_CLINIC_SPCLTY,T.T_CLINIC_CODE,T.T_CLINIC_DOC_CODE,T.T_REQUEST_NO,T.T_REQUEST_DATE,T.T_REQUEST_TIME) VALUES  (TRUNC(SYSDATE),'{T_ENTRY_USER}','{PAT_NO}','{APPT_DATE}','{CLINIC_SPCLTY}','{CLINIC_CODE}','{CLINIC_DOC_CODE}','{REQ_NO}',TRUNC(SYSDATE),'{REQ_TIME}')");
        }
        public DataTable CreateReprotData(string T_REQUEST_NO)
        {
            return ReportQuery($@"SELECT t.T_PAT_NO, ( SELECT z.T_FIRST_LANG2_NAME || ' ' || z.T_FATHER_LANG2_NAME || ' ' || z.T_GFATHER_LANG2_NAME || ' ' || z.T_FAMILY_LANG2_NAME || '-' || z.T_FIRST_LANG1_NAME || ' ' || z.T_FATHER_LANG1_NAME || ' ' || z.T_GFATHER_LANG1_NAME || ' ' || z.T_FAMILY_LANG1_NAME FROM t03001 z WHERE z.t_pat_no = t.T_PAT_NO) PAT_NAME, (SELECT t2.T_LANG2_NAME FROM t03001 y JOIN t02006 t2 ON y.T_GENDER = t2.T_SEX_CODE WHERE y.T_PAT_NO = t.T_PAT_NO ) SEX_DESC, ( SELECT T_NAME_GIVEN ||' '|| T_NAME_FATHER ||' '||T_NAME_FAMILY FROM T02029 WHERE T_EMP_NO = t.T_CLINIC_DOC_CODE) DOCTOR_NAME, (SELECT T_CLINIC_NAME_LANG2 CLINIC_DESC FROM T07001 WHERE T_CLINIC_CODE='A742') CLINIC_DESC, (SELECT T_LANG2_NAME FROM T02040 WHERE T_SPCLTY_CODE=t.T_CLINIC_SPCLTY) CLINIC_SPECIALITY, (select t_lang2_name from t07022 where T_TIME_CODE=t.T_APPT_DATE) APP_DESC, t.T_CLINIC_SPCLTY, t.T_CLINIC_CODE, t.T_CLINIC_DOC_CODE, t.T_REQUEST_NO, t.T_REQUEST_DATE, t.T_REQUEST_TIME, t.t_appt_date, ( SELECT x.t_user_name FROM t01009 x WHERE x.t_emp_code = nvl(t.T_ENTRY_USER, t.T_UPD_USER) )USER_NAME FROM T07027 t WHERE t.T_REQUEST_NO = '{T_REQUEST_NO}'");
        }
    }
}