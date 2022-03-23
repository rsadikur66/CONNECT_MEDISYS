using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectKsmcDAL.Transaction
{
    public class T06201DAL : CommonDAL
    {
        public dynamic GetPatientType()
        {
            return QueryList<dynamic>($@"SELECT '1' VAL,'IP' NAME FROM DUAL UNION ALL SELECT '3' VAL , 'ER' NAME FROM DUAL UNION ALL SELECT '2' VAL, 'OPD' NAME FROM DUAL");
        }
        public dynamic GetPatientTypeInfo(string value, string patNo)
        {
            object re = new object();
            if (value == "1")
            {
                re = QuerySingle<dynamic>($@"SELECT A.T_ADMIT_DATE,B.T_CLN_DISCH_DATE,A.T_EPISODE_NO FROM t05010 A,T05022 B WHERE A.T_PAT_NO = '{patNo}' AND A.T_PAT_NO=B.T_PAT_NO AND A.T_EPISODE_NO=B.T_EPISODE_NO AND B.T_CLN_DISCH_DATE >= TRUNC(SYSDATE) -1");
            }
            else if (value == "2")
            {
                re = QuerySingle<dynamic>($@"SELECT T_EPISODE_START_DATE, T_EPISODE_END_DATE, T_EPISODE_NO FROM T04007 WHERE T_PAT_NO='02446540' AND (T_EPISODE_START_DATE >= TRUNC(SYSDATE) -1 OR T_EPISODE_END_DATE >= TRUNC(SYSDATE)-1) AND T_EPISODE_NO IN (SELECT MAX(T_EPISODE_NO) FROM T04007 WHERE T_PAT_NO='{patNo}')");
            }
            else if (value == "3")
            {
                re = QuerySingle<dynamic>($@"SELECT DISTINCT T_APPT_DATE FROM v07028 WHERE T_APPT_DATE = TRUNC(SYSDATE) AND T_PAT_NO = '{patNo}' AND T_ARRIVAL_STATUS IS NOT NULL");
            }
            return re;
        }
        public dynamic GetPatientInfo(string patNo, string lang)
        {
            return QuerySingle<dynamic>($@"SELECT T_FIRST_LANG2_NAME ||' ' ||T_FATHER_LANG2_NAME ||' ' || T_GFATHER_LANG2_NAME ||' ' ||T_FAMILY_LANG2_NAME T_NAME, T_GENDER , T06.T_LANG2_NAME GENDER_DES, t01. T_NTNLTY_CODE, T03.T_LANG2_NAME NTNLTY,  TO_CHAR(T_BIRTH_DATE,'dd-MM-yyyy') T_BIRTH_DATE, (SELECT TRUNC(MONTHS_BETWEEN(SYSDATE,T_BIRTH_DATE)/12) FROM DUAL) AGE_Y, (SELECT TRUNC(MOD(MONTHS_BETWEEN(SYSDATE, T_BIRTH_DATE), 12)) FROM DUAL) AGE_M,  T_X_RMC_CHRTNO, T_JOB_CODE , T19.T_LANG2_NAME OCCUPATION, t01. T_pat_no, T_1022.T_ADMIT_DATE, T_1022.T_CLN_DISCH_DATE, T_1022.T_EPISODE_NO,v21. PAT_TYPE FROM T03001 t01 LEFT JOIN T02003 t03 ON t01. T_NTNLTY_CODE= t03.t_ntnlty_code LEFT JOIN T02006 t06 ON t01.T_GENDER = t06.t_sex_code LEFT JOIN V35021 v21 ON  t01.T_pat_no = v21.T_PAT_NO LEFT JOIN T02019 t19 ON t01.T_JOB_CODE = t19.T_JOB_TITLE_CODE LEFT JOIN (SELECT A.T_ADMIT_DATE,B.T_CLN_DISCH_DATE,A.T_EPISODE_NO,A.T_PAT_NO FROM t05010 A,T05022 B WHERE A.T_PAT_NO=B.T_PAT_NO AND A.T_EPISODE_NO=B.T_EPISODE_NO AND B.T_CLN_DISCH_DATE >= TRUNC(SYSDATE) -1) T_1022 ON t01.T_pat_no =T_1022.T_PAT_NO WHERE t01.T_pat_no = '{patNo}'");
        }
        public string SaveData(dynamic datas, string user)
        {
            string sms = "";
            var count = QueryString($@"select count(*) from t06201 where t_entry_date=trunc(sysdate)");
            if (Convert.ToInt32(count) == 0)
            {
                Command($@"	update T_EXPORT_SEQ v set T_EXP_NO='0'");
                var max = QueryString($@"select nvl(MAX(T_SICK_LEAVE_SEQ),0)+1 T_SICK_LEAVE_SEQ from t06201");
                if (Convert.ToInt32(max) > 0)
                {
                    Command($@"INSERT INTO T06201 ( T_PAT_NO,T_PAT_TYPE,T_VISIT_DATE, T_LEAVE_DAYS, T_START_DATE, T_END_DATE, T_SICK_FLAG, T_CONS_DOC, T_FU_FLAG,T_MED_COM_FLAG, T_MED_COM_RSN, T_APPROVAL_FLAG, T_CNT_TREAT_FLAG, T_PRMNT_PAR_FLAG, T_OTHER_FLAG, T_OTHER_RSN, T_TREAT_DOC_CODE, T_TREAT_DOC, T_DOC_CODE, T_OCCUPATION, T_PLACE_WORK, T_LETTER_HEAD, T_LETTER_REF_NO,T_LETTER_DATE,T_SICK_LEAVE_SEQ,T_ENTRY_DATE,T_ENTRY_USER,T_SIGN_DATE) VALUES ('{datas.DataObject.T_PAT_NO}','{datas.DataObject.T_PAT_TYPE}',TO_DATE('{datas.DataObject.T_VISIT_DATE}','dd/MM/yyyy'), '{datas.DataObject.T_LEAVE_DAYS}',TO_DATE('{datas.DataObject.T_START_DATE}','dd/MM/yyyy'), TO_DATE('{datas.DataObject.T_END_DATE}','dd/MM/yyyy'), '{datas.DataObject.T_SICK_FLAG}', '{datas.DataObject.T_CONS_DOC}', '{datas.DataObject.T_FU_FLAG}','{datas.DataObject.T_MED_COM_FLAG}', '{datas.DataObject.T_MED_COM_RSN}', '{datas.DataObject.T_APPROVAL_FLAG}', '{datas.DataObject.T_CNT_TREAT_FLAG}', '{datas.DataObject.T_PRMNT_PAR_FLAG}', '{datas.DataObject.T_OTHER_FLAG}', '{datas.DataObject.T_OTHER_RSN}', '{datas.DataObject.T_TREAT_DOC_CODE}', '{datas.DataObject.T_TREAT_DOC}', '{datas.DataObject.T_DOC_CODE}', '{datas.DataObject.T_OCCUPATION}', '{datas.DataObject.T_PLACE_WORK}', '{datas.DataObject.T_LETTER_HEAD}', '{datas.DataObject.T_LETTER_REF_NO}',TO_DATE('{datas.DataObject.T_LETTER_DATE}','dd/MM/yyyy'),'{max}', TRUNC(SYSDATE),'{datas.DataObject.T_TREAT_DOC_CODE}', TRUNC(SYSDATE))");
                    sms = "Save Successfully";
                }
                else
                {

                }
            }
            else
            {
                sms = "Do not  Save";
            }
            return sms;
        }
        public dynamic GetDoctorInfo(string user)
        {
            var data = "";
            var count = QueryString($@"select CASE WHEN CHK !=0 THEN (select CASE WHEN T_DESIGNATION ='2' THEN(select count(*) v_cnt from v06255 where CONST_DOC='{user}' and T_CONS__CHECK is null) ELSE 0 END T_COUNT from t02039 where T_emp_CODE='{user}' ) ELSE 0 end T_COUNT from (select count(*) CHK from t02039 where T_emp_CODE='{user}' )");
            if (Convert.ToInt32(count) > 0)
            {
                return QuerySingle<dynamic>($@"SELECT RTRIM(A.T_NAME_GIVEN,' ')||' '||RTRIM(A.T_NAME_FATHER,' ') ||' '||RTRIM(A.T_NAME_GFATHER,' ')||' '||RTRIM(A.T_NAME_FAMILY,' ') D_DESC, B.T_DOC_CODE FROM T02029 A, T02039 B WHERE B.T_DOC_CODE = A.T_EMP_NO AND A.T_EMP_ACTIVE ='1' and t_emp_code='{user}'");
            }
            else
            {
                return data;
            }
        }
        public dynamic GetDetails(string patNo, string patType)
        {
            return QuerySingle<dynamic>($@"SELECT DISTINCT T_SICK_LEAVE_SEQ, T_PAT_NO, T_ADM_DATE , T_DISCHARGE_DATE, T_CONS_DOC_CODE, T_VISIT_DATE, T_SICK_FLAG, T_LEAVE_DAYS, T_CONS_DOC, T_LEAVE_DAYS, T_START_DATE, T_END_DATE, T_END_DATE, T_FU_FLAG, T_MED_COM_FLAG, T_MED_COM_RSN, T_APPROVAL_FLAG, T_CNT_TREAT_FLAG, T_PRMNT_PAR_FLAG, T_OTHER_FLAG, T_OTHER_RSN, T_TREAT_DOC_CODE, T_TREAT_DOC, T_BADGE_NO, T_DOC_CODE, T_DOC_CODE, T_OCCUPATION, T_PLACE_WORK, T_DIRECTOR, T_SIGN_DATE, T_LETTER_HEAD, T_LETTER_REF_NO, T_PRINT_FLAG, T_LETTER_DATE, T_MR_NAME, T_OLD_REPORT_NO, T_OLD_REPORT_DATE, T_UPD_DATE, T_UPD_USER, T_SIGN_FLAG, T_ENTRY_USER, T_ENTRY_DATE, T_LEAVE_DAYS_ARB, T_TEMP_NO, T_PAT_TYPE, T_EPISODE_NO, T_EXP_NO FROM t06201 WHERE T_PAT_NO='{patNo}' and T_PAT_TYPE='{patType}' ORDER BY T_SICK_LEAVE_SEQ desc");
        }
    }
}