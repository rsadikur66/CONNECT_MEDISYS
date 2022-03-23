using System.Collections.Generic;
using System.Data;

namespace ConnectKsmcDAL.Transaction
{
    public class T06209DAL : CommonDAL
    {
        public IEnumerable<dynamic> GetBMIindex(string lang)
        {
            return QueryList<dynamic>($"select T_LANG{lang}_NAME, T_INDEX_CODE from T06210 where T_GROUP_CODE = '05'and T_ACTIVE_FLAG = '1' order by T_INDEX_CODE");
        }
        public IEnumerable<dynamic> GetBPindex(string lang)
        {
            return QueryList<dynamic>($"select T_LANG2_NAME, T_INDEX_CODE from T06210 where T_GROUP_CODE = '01' and T_ACTIVE_FLAG = '1' order by T_INDEX_CODE");
        }
        public IEnumerable<dynamic> GetTempindex(string lang)
        {
            return QueryList<dynamic>($"select T_LANG{lang}_NAME, T_INDEX_CODE from T06210 where T_GROUP_CODE = '03' and T_ACTIVE_FLAG = '1' order by T_INDEX_CODE");
        }
        public IEnumerable<dynamic> GetPulseindex(string lang)
        {
            return QueryList<dynamic>($"select T_LANG{lang}_NAME, T_INDEX_CODE from T06210 where T_GROUP_CODE = '02' and T_ACTIVE_FLAG = '1' order by T_INDEX_CODE");
        }
        public IEnumerable<dynamic> GetRRindex(string lang)
        {
            return QueryList<dynamic>($"select T_LANG2_NAME, T_INDEX_CODE from T06210 where T_GROUP_CODE = '04' and T_ACTIVE_FLAG = '1' order by T_INDEX_CODE");
        }
        public IEnumerable<dynamic> GetGLindex(string lang)
        {
            return QueryList<dynamic>($"select T_LANG2_NAME, T_INDEX_CODE from T06210 where T_GROUP_CODE = '06' and T_ACTIVE_FLAG = '1' order by T_INDEX_CODE");
        }
        public IEnumerable<dynamic> GetMedHxindex(string lang)
        {
            return QueryList<dynamic>($"select T_LANG{lang}_NAME, T_LANG2_NAME from T06210 where T_GROUP_CODE = '07' and T_ACTIVE_FLAG = '1' order by T_INDEX_CODE");
        }
        public IEnumerable<dynamic> GetAllergyDietindex(string lang)
        {
            return QueryList<dynamic>($"select T_LANG2_NAME, T_INDEX_CODE from T06210 where T_GROUP_CODE = '08' and T_ACTIVE_FLAG = '1' order by T_INDEX_CODE");
        }
        public IEnumerable<dynamic> GetAllergyMedindex(string lang)
        {
            return QueryList<dynamic>($"select T_LANG2_NAME, T_INDEX_CODE from T06210 where T_GROUP_CODE = '09' and T_ACTIVE_FLAG = '1' order by T_INDEX_CODE");
        }
        public IEnumerable<dynamic> GetRecommendationDropDownlist(string lang)
        {
            return QueryList<dynamic>($"select T_INDEX_CODE, T_LANG2_NAME from T06210 where T_GROUP_CODE = '10' and T_ACTIVE_FLAG = '1' order by T_INDEX_CODE");
        }
        public IEnumerable<dynamic> GetPatListPopData(string PatNo,string lang)
        {
            return QueryList<dynamic>($"select t.t_pat_no, t.pat_type, t.T_WARD_NO , LTRIM(t.T_FIRST_LANG{lang}_NAME) || ' ' || t.T_FATHER_LANG{lang}_NAME || ' '|| t.T_FAMILY_LANG{lang}_NAME PAT_NAME , DECODE(PAT_TYPE,'1','IP','2','OP','3','ER',NULL) TYPE, t.T_MRTL_STATUS ,t.T_GENDER ,t.T_NTNLTY_CODE , (select x.T_LANG{lang}_NAME from T02007 x where x.t_mrtl_status_code = t.T_MRTL_STATUS) T_MRTL_DESC, (select T_LANG{lang}_NAME from T02003 where t_ntnlty_code = t.T_NTNLTY_CODE) ntnlty_dscrptn, (select T_LANG{lang}_NAME from T02006 where t_sex_code = t.T_GENDER) gender_dscrptn, (select trunc(months_between(sysdate,t.T_BIRTH_DATE)/12) from dual) AGE_Y, (SELECT trunc(mod(months_between(sysdate, t.T_BIRTH_DATE), 12)) from dual) AGE_M from v05051 t WHERE t.T_PAT_NO = '{PatNo}' ORDER BY t.T_FIRST_LANG2_NAME");
        }
        public IEnumerable<dynamic> GetDoctorListPopData(string lang)
        {
            return QueryList<dynamic>($"select T_DOC_CODE, T_NAME_GIVEN||' '|| T_NAME_FATHER||' '|| T_NAME_GFATHER||' '|| T_NAME_FAMILY DOCTOR_NAME from t02029 ,T35001,T01009 where T01009.t_emp_code = T35001.t_user_code and T35001.t_doc_code = t02029.t_emp_no and T35001.T_DOC_CODE IN (SELECT DISTINCT T_DOC_CODE FROM T26015 WHERE NVL(T_ON_DUTY_RESIDENT_FLAG,'N')='Y') and T01009.T_ACTIVE_FLAG = '1' ORDER BY 2");
        }
        public IEnumerable<dynamic> GetPatientVitalDetails(string patNumber, string patType)
        {
            return QueryList<dynamic>($"select j.*,TO_CHAR(j.T_ENTRY_DATE,'dd/MM/yyyy') ENTRY_DATE,(select x.T_USER_NAME from T01009 x where x.T_EMP_CODE = j.T_ENTRY_USER) ENTRY_USER ,TO_CHAR(j.T_UPD_DATE , 'dd/MM/yyyy') UPDATE_DATE,(SELECT 	x.T_USER_NAME FROM T01009 x WHERE x.T_EMP_CODE = j.T_UPD_USER) UPDATE_USER from T06209 j WHERE T_PAT_NO = '{patNumber}' AND T_EPISODE_TYPE = '{patType}' AND T_ENTRY_DATE = TRUNC(SYSDATE) order by T_RECORD_NO desc");
        }
        public IEnumerable<dynamic> GetPatRiskFactor(string patNumber)
        {
            return QueryList<dynamic>($"select * from T06212 where T_PAT_NO = '{patNumber}' and ROWNUM= 1");
        }
        public dynamic GetDocWardEpiDateByType(string PAT_TYPE, string PatNo, string lang)
        {
            if (PAT_TYPE == "1")
            {
                return QueryList<dynamic>($"SELECT m.t_admit_doc_code, m.t_admit_ward_no, m.t_episode_no,TO_CHAR(m.t_admit_date,'dd/MM/yyyy') t_admit_date, (select T_NAME_GIVEN||' '||T_NAME_FATHER||' '||T_NAME_FAMILY DOC_NAME from T02029 where t_emp_no = m.t_admit_doc_code and t_emp_active='1') DOC_NAME, (select T_LANG2_NAME from T02042 where T_LOC_CODE = m.T_ADMIT_WARD_NO) CLINIC_NAME FROM t05010 m WHERE t_pat_no = '{PatNo}' AND t_episode_no = (SELECT MAX (t_episode_no) FROM t05010 WHERE t_pat_no = '{PatNo}')");
            }
            else if (PAT_TYPE == "2")
            {
                return QueryList<dynamic>($@"SELECT t_arrival_no,to_char(T_ARRIVAL_DATE,'dd/MM/yyyy') T_ARRIVAL_DATE,t_visit_no,t_clinic_doc_code,t07003.t_clinic_code, (select T_NAME_GIVEN||' '||T_NAME_FATHER||' '||T_NAME_FAMILY DOC_NAME from T02029 where t_emp_no = t07003.t_clinic_doc_code and t_emp_active='1') DOC_NAME, (select T_CLINIC_NAME_LANG2 from t07001 where t07001.T_CLINIC_CODE = t07003.t_clinic_code) CLINIC_NAME FROM t07003 WHERE t_pat_no = '{PatNo}' AND t_appt_date = TRUNC (SYSDATE) AND t_appt_no = (SELECT MAX (t_appt_no) FROM t07003 WHERE t_pat_no = '{PatNo}' AND t_appt_date = TRUNC (SYSDATE) )");
            }
            else if (PAT_TYPE == "3")
            {
                return QueryList<dynamic>($@"SELECT t_entry_date, t_episode_no,T_PAT_NO,t_admit_doc_code, t_admit_location, (select T_NAME_GIVEN||' '||T_NAME_FATHER||' '||T_NAME_FAMILY from T02029 where T02029.T_EMP_NO = t04007.t_admit_doc_code and T02029.t_emp_active='1') DOC_NAME, (select T_LANG2_NAME from T04003 where T_ER_LOCATION = t04007.t_admit_location) CLINIC_NAME FROM t04007 WHERE t_pat_no = '{PatNo}' AND t_episode_end_date IS NULL AND t_episode_start_date >= TRUNC (SYSDATE) - 4");
            }
            else
            {
                return null;
            }
        }
        public string SaveData(dynamic data, string user, string siteCode)
        {
            string sms = "";
            dynamic RECORD_RISK = QuerySingle<dynamic>($"select Count(*) countNumber from T06212 where T_PAT_NO = '{data.T_PAT_NO}' and  ROWNUM= 1");
            if (RECORD_RISK.countNumber > 0)
            {
                Command($"UPDATE T06212 SET T_MED_DM = '{data.DM_FLAG}',T_MED_HTN = '{data.HTN_FLAG}',T_RF_HYPERLIPIDEMIA='{data.HYPER_FLAG}',T_RF_IHD='{data.HD_FLAG}', T_RF_CVA = '{data.CVA_FLAG}',T_RF_RENAL_FAILURE = '{data.RENAL_FLAG}',T_UPD_USER = '{user}', T_UPD_DATE = TRUNC(SYSDATE) WHERE T_PAT_NO = '{data.T_PAT_NO}' and ROWNUM= 1 ");
            }else
            {
                Command($"insert into T06212 (T_ENTRY_USER, T_ENTRY_DATE,T_MED_DM,T_MED_HTN,T_RF_HYPERLIPIDEMIA,T_RF_IHD,T_RF_CVA,T_RF_RENAL_FAILURE,) values ('{user}',TRUNC(SYSDATE),'{data.DM_FLAG}','{data.HTN_FLAG}','{data.HYPER_FLAG}','{data.HD_FLAG}','{data.CVA_FLAG}','{data.RENAL_FLAG}')");
            }
            dynamic RECORD = QueryList<dynamic>($"select t_record_no from T06209 where t_pat_no='{data.PAT_NO}' and t_entry_date = trunc(sysdate)");
            if (RECORD.Count > 0)
            {
                Command($"UPDATE t06209 SET T_WEIGHT = '{data.WEIGHT}',T_HEIGHT = '{data.HEIGHT}',T_BMI='{data.BMI}',T_BMI_INDEX='{data.BMI_INDEX}', T_BP_SYSTOLIC = '{data.BP_SYS}',T_BP_DIASTOLIC='{data.BP_DIA}',T_BODY_TEMP='{data.BODY_TEMP}',T_BODY_TEMP_INDEX='{data.BODY_TEMP_INDEX}',T_PULSE='{data.PULSE}',T_PULSE_INDEX ='{data.PULSE_INDEX}',T_RESPIRATION_RATE='{data.RESPIRATION}',T_RR_INDEX='{data.RR_INDEX}',T_SPO='{data.SPO2}',T_HEAD_CIRCUMFERENCE_FLAG='{data.HaedFlagCheck}',T_CURCUM_HAND='{data.HAND_CIR}',T_HEAD_CIRCUMFERENCE='{data.HEAD_CIR}',T_GL_FASTING='{data.GL_FAST}',T_GL_RANDOM='{data.GL_RANDOM}',T_GL_INDEX='{data.GL_INDEX}',T_MEDICAL_HISTORY='{data.MED_HIS}',T_NOTE='{data.NOTE}',T_ALLERGY_FLAG='{data.ALLERGY_FLAG}',T_ALLERGY_DIET='{data.ALLERGY_DIET}',T_ALLERGY_MEDICATION='{data.ALLERGY_MED}',T_ALLERGY_OTHERS='{data.ALLERGY_OTHERS}',T_RECOMMENDATIONS='{data.RECOM}',T_SMOKING_YN='{data.SMOKE}',T_STICK_PER_DAY='{data.STICK_DAY}',T_PREGNENCY_YN='{data.PREG_STATUS}',T_PREGNENCY_WEEK='{data.PREG_WEEK}',T_LMP_DATE='{data.LMP_DATE}',T_PREGNENCY_GRAVIDA='{data.GRAVIDA}',T_PREGNENCY_PARA='{data.PARA}', T_PREGNENCY_ABORTION='{data.ABORTION}',T_PRGNNCY_TITANUS_TOXOID_FLAG='{data.TETENUS_TOX}',T_TITANUS_TOXOID_LAST_DOSE_DT='{data.LAST_DOSE}',T_PREGNENCY_LACTATION='{data.LOCATION}',T_UPD_USER = '{user}', T_UPD_DATE = TRUNC(SYSDATE) WHERE T_PAT_NO = '{data.T_PAT_NO}' and T_RECORD_NO = '{RECORD[0].T_RECORD_NO}'");
            }
            else
            {
                var RECORD_NO = QueryString($"select NVL(MAX(T_RECORD_NO), 0)+1   from T06209   where T_PAT_NO = '{data.PAT_NO}'");
                Command($"INSERT INTO T06209 (T_ENTRY_USER, T_ENTRY_DATE, T_ENTRY_TIME, T_PAT_NO, T_RECORD_NO, T_EPISODE_TYPE, T_EPISODE_NO, T_CLINIC_CODE, T_CLINIC_DOC_CODE, T_BP_SYSTOLIC, T_BP_DIASTOLIC, T_BP_INDEX, T_PULSE, T_PULSE_INDEX, T_BODY_TEMP, T_BODY_TEMP_INDEX, T_RESPIRATION_RATE, T_RR_INDEX, T_WEIGHT, T_HEIGHT, T_BMI, T_BMI_INDEX, T_CURCUM_HAND, T_GL_RANDOM, T_GL_FASTING, T_GL_INDEX, T_MEDICAL_HISTORY, T_ALLERGY_DIET, T_ALLERGY_MEDICATION, T_ALLERGY_OTHERS, T_SMOKING_YN, T_STICK_PER_DAY, T_PREGNENCY_YN, T_PREGNENCY_WEEK, T_NOTE, T_SPO, T_RECOMMENDATIONS, T_LMP_DATE, T_HEAD_CIRCUMFERENCE_FLAG, T_HEAD_CIRCUMFERENCE, T_ALLERGY_FLAG, T_PREGNENCY_GRAVIDA, T_PREGNENCY_PARA, T_PREGNENCY_ABORTION, T_PRGNNCY_TITANUS_TOXOID_FLAG, T_TITANUS_TOXOID_LAST_DOSE_DT, T_PREGNENCY_LACTATION ) VALUES ('{user}',TRUNC(SYSDATE),TO_CHAR(SYSDATE, 'HH24:MI'),'{data.PAT_NO}','{RECORD_NO}','{data.EPISODE_TYPE}','{data.EPISODE_NO}','{data.CLINIC_CODE}','{data.CLINIC_DOC_CODE}','{data.BP_SYS}','{data.BP_DIA}','{data.BP_INDEX}', '{data.PULSE}','{data.PULSE_INDEX}','{data.BODY_TEMP}','{data.BODY_TEMP_INDEX}','{data.RESPIRATION}','{data.RR_INDEX}','{data.WEIGHT}','{data.HEIGHT}','{data.BMI}','{data.BMI_INDEX}','{data.HAND_CIR}','{data.GL_RANDOM}','{data.GL_FAST}','{data.GL_INDEX}', '{data.MED_HIS}','{data.ALLERGY_DIET}','{data.ALLERGY_MED}','{data.ALLERGY_OTHERS}','{data.SMOKE}','{data.STICK_DAY}','{data.PREG_STATUS}','{data.PREG_WEEK}','{data.NOTE}','{data.SPO2}','{data.RECOM}','{data.LMP_DATE}','{data.HaedFlagCheck}', '{data.HEAD_CIR}', '{data.ALLERGY_FLAG}','{data.GRAVIDA}','{data.PARA}', '{data.ABORTION}','{data.TETENUS_TOX}','{data.LAST_DOSE}','{data.LOCATION}')");
            }
            return sms;
        }
        public DataTable ReportHeader()
        {
            return ReportQuery($@"SELECT T_SITE_CODE , T_COUNTRY_LANG1_NAME ,T_COUNTRY_LANG2_NAME ,T_MIN_LANG1_NAME ,T_MIN_LANG2_NAME ,T_SITE_LANG1_NAME ,T_SITE_LANG2_NAME ,T_LOGO_ID ,T_LOGO_NAME, T_LOGO ,t_lcence_no from t01028 where t_site_code in(select t_site_code from t01001)");
        }
        public DataTable ReportQueryOne(string T_REQUEST_NO)
        {
            return ReportQuery($@"SELECT T_LOCATION_CODE , T_PAT_NO , T_PAT_TYPE , T_PRIORITY_CODE , T_REQUEST_DATE , T_REQUEST_NO REQUEST_NO , T_CLINIC_DATA , T_EXTERNAL_FLAG , T_SPEC_NO FROM T13015 WHERE T_REQUEST_NO = '{T_REQUEST_NO}' GROUP BY T_LOCATION_CODE , T_PAT_NO , T_PAT_TYPE , T_PRIORITY_CODE , T_REQUEST_DATE , T_REQUEST_NO , T_CLINIC_DATA , T_EXTERNAL_FLAG , T_SPEC_NO");
        }
        public DataTable ReportQuerySecond(string T_REQUEST_NO)
        {
            return ReportQuery($@"SELECT T13015.T_PAT_NO PRS_PAT_NO ,T13018.T_REQUEST_NO, T13015.T_EXTERNAL_FLAG EXTERNAL_FLAG , T13018.T_WS_CODE, T13018.T_ANALYSIS_CODE, T13018.T_RESULT_VALUE, T13018.T_NOTES, T13064.T_GROUP_ANALYSIS FROM T13018,T13064,T13015 WHERE T13018.T_ANALYSIS_CODE = T13064.T_ANALYSIS_CODE AND T_CLOSE_FLAG IS NOT NULL AND T_RESULT_VALUE IS NOT NULL AND T13018.T_ANALYSIS_CODE IN ('17011','17012','17013','17014', '17015','17016','17017','17022','17025','17071','17072','17073','17074','17075','17076','17077','17078', '17079','17080','17081','17082','17083','17084','13005','13016','13024','13025','13017','13006','13008','13009','13012','13050','17085','17086','17087', '17088','17089','17194','17195') AND T13018.T_REQUEST_NO='{T_REQUEST_NO}' AND T13015.T_REQUEST_NO = T13018.T_REQUEST_NO ORDER by T13018.T_ANALYSIS_CODE ASC");
        }
    }
}
