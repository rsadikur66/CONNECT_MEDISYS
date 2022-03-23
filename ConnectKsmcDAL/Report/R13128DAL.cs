using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ConnectKsmcDAL.Report
{
    public class R13128DAL : CommonDAL
    {
        public IEnumerable<dynamic> GetWorkstations(string lang)
        {
            return QueryList<dynamic>($@"SELECT A.T_WS_CODE, A.T_LANG{lang}_NAME NAME
                                        FROM T13004 A, T13011 B 
                                        WHERE A.T_WS_CODE = B.T_WS_CODE AND B.T_ACTIVE_FLAG IS NOT NULL 
                                        GROUP BY A.T_LANG{lang}_NAME, A.T_WS_CODE 
                                        ORDER BY 1");
        }

        public IEnumerable<dynamic> GetAllByReqNo(string reqno)
        {
            return QueryList<dynamic>($@"SELECT DISTINCT T13015.T_REQUEST_NO, 
                                        NVL((SELECT MAX(T13039.T_LAB_NO) FROM T13039 WHERE T13039.T_REQUEST_NO = '{reqno}'),NVL(T13017.T_LAB_NO, 'NOT PROVIDED')) T_LAB_NO,
                                        TRUNC(T13015.T_REQUEST_DATE) T_REQUEST_DATE, T13017.T_WS_CODE, T13015.T_LAB_REF_NO, T13017.T_ANALYSIS_CODE
                                        FROM T13015, T03003, T13017
                                        WHERE T13015.T_PAT_NO = T03003.T_TMP_PAT_NO AND T13015.T_REQUEST_NO = T13017.T_REQUEST_NO
                                        AND T13015.T_REQUEST_NO = '{reqno}' AND nvl(T13017.T_LAB_NO,'NOT PROVIDED') IS NOT NULL");
        }

        public IEnumerable<dynamic> GetOrgans(string reqno, string labno)
        {
            return QueryList<dynamic>($@"SELECT T_ORGANISM_CODE1, T_ORGANISM_CODE2, T_ORGANISM_CODE3 
                                        FROM T13039 WHERE T_LAB_NO = '{labno}' AND T_REQUEST_NO = '{reqno}'");
        }

        public DataTable GetAllR13128(string lang, string reqno, string workstation, string analysis)
        {
            switch (workstation)
            {
                case "03":
                case "12":
                    return ReportQuery($@"SELECT DISTINCT T13015.T_PAT_NO, T13015.T_REQUEST_NO, T13015.T_LAB_REF_NO REF_HOSP_NO  
                                        , T13015.T_REQUEST_DATE, T13015.T_REQUEST_TIME, T13015.T_SPEC_NO, T13016.T_LAB_NO  
                                        ,(SELECT MAX(T13017.T_RECEIVED_DATE) FROM T13017 WHERE T13017.T_REQUEST_NO= '{reqno}' AND T13017.T_WS_CODE = '{workstation}' AND T13017.T_SPECIMEN_CODE IS NOT NULL) T_RECEIVED_DATE   
                                        ,(SELECT MAX(T13017.T_RECEIVED_TIME) FROM T13017 WHERE T13017.T_REQUEST_NO= '{reqno}' AND T13017.T_WS_CODE = '{workstation}' AND T13017.T_SPECIMEN_CODE IS NOT NULL) T_RECEIVED_TIME  
                                        , T13018.T_NOTES , NVL(T13018.T_UPD_DATE,T13018.T_ENTRY_DATE)T_UPD_DATE   
                                        , T03003.T_FIRST_LANG{lang}_NAME || ' ' || T03003.T_FATHER_LANG{lang}_NAME || ' ' || T03003.T_GFATHER_LANG{lang}_NAME || ' ' || T03003.T_FAMILY_LANG{lang}_NAME T_PAT_NAME, T03003.T_BIRTH_DATE  
                                        , NVL((SELECT DISTINCT T13005.T_LANG{lang}_NAME FROM T13005 WHERE T13005.T_SPECIMEN_CODE = (SELECT MAX(DISTINCT(T13017.T_SPECIMEN_CODE)) FROM T13017 WHERE T13017.T_REQUEST_NO= '{reqno}' AND T13017.T_WS_CODE = '{workstation}' AND T13017.T_SPECIMEN_CODE IS NOT NULL)), '') T_SPECIMEN_NAME  
                                        , NVL((select T02006.T_LANG{lang}_NAME  from T02006 WHERE  T03003.T_GENDER = T02006.T_SEX_CODE ),'') T_GENDER , NVL((SELECT DISTINCT T02003.T_LANG{lang}_NAME FROM T02003 WHERE T03003.T_NTNLTY_CODE = T02003.T_NTNLTY_CODE), '') T_NATIONALITY  
                                        , T02049.T_LANG{lang}_NAME T_LOCATION , T13004.T_LANG{lang}_NAME || ' Department' T_LOCATION_NAME  
                                        , NVL((SELECT DISTINCT T13006.T_LANG{lang}_NAME FROM T13006 WHERE T13006.T_RESULT_CODE = T13018.T_RESULT_VALUE ),T13018.T_RESULT_VALUE) T_RESULT_VALUE  
                                        , (SELECT distinct T13011.T_LANG{lang}_NAME FROM T13011  WHERE T13011.T_ANALYSIS_CODE =T13018.T_ANALYSIS_CODE AND T13011.T_WS_CODE = T13016.T_WS_CODE) T_ANALYSIS_NAME  
                                        , DECODE((SELECT T13011.T_RESULT_TYPE FROM T13011 WHERE T13011.T_WS_CODE= T13016.T_WS_CODE AND T13011.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE)  
                                        , '1', (SELECT T13014.T_NR_FROM ||'-'||T13014.T_NR_TO FROM T13014 WHERE T13014.T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') AND T13014.T_WS_CODE= T13016.T_WS_CODE AND T13014.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE)   
                                        , '2', (SELECT T13014.T_NR_FROM ||'-'||T13014.T_NR_TO FROM T13014 WHERE T13014.T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') AND T13014.T_WS_CODE= T13016.T_WS_CODE AND T13014.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE AND T13014.T_GENDER = T03003.T_GENDER)  
                                        , '3', (SELECT T13014.T_NR_FROM ||'-'||T13014.T_NR_TO FROM T13014 WHERE T13014.T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') AND T13014.T_WS_CODE= T13016.T_WS_CODE AND T13014.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE  
                                        AND TO_NUMBER(NVL(TRUNC(MONTHS_BETWEEN(SYSDATE,T03003.T_BIRTH_DATE)/12,0),0) * 365)+TRUNC(SYSDATE)-(ADD_MONTHS(T03003.T_BIRTH_DATE,(TRUNC((MONTHS_BETWEEN(TRUNC(SYSDATE),T03003.T_BIRTH_DATE))/12))*12+(MOD((MONTHS_BETWEEN(TRUNC(SYSDATE),T03003.T_BIRTH_DATE)),12))))  
                                        BETWEEN ((T_YEAR_FROM * 365 )+T_DAYS_FROM) AND ((T_YEAR_TO * 365)+T_DAYS_TO)),'4',(SELECT T_NR_FROM ||'-'||T_NR_TO FROM T13014  
                                        WHERE T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') AND T_WS_CODE= T13016.T_WS_CODE AND T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE  
                                        AND TO_NUMBER(NVL(TRUNC(MONTHS_BETWEEN(SYSDATE,T03003.T_BIRTH_DATE)/12,0),0) * 365)+TRUNC(SYSDATE)-(ADD_MONTHS(T03003.T_BIRTH_DATE,(TRUNC((MONTHS_BETWEEN(TRUNC(SYSDATE),T03003.T_BIRTH_DATE))/12))*12+(MOD((MONTHS_BETWEEN(TRUNC(SYSDATE),T03003.T_BIRTH_DATE)),12))))  
                                        BETWEEN ((T_YEAR_FROM * 365 )+T_DAYS_FROM) AND ((T_YEAR_TO * 365)+T_DAYS_TO) AND T_GENDER = T03003.T_GENDER),'')||'  '||(SELECT T_UM FROM T13011 WHERE T_WS_CODE = T13016.T_WS_CODE AND T13011.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE) T_RANGE  
                                        FROM T13015, T13016  ,T13018, T03003, T02049,T13004   
                                        WHERE T13015.T_REQUEST_NO = T13016.T_REQUEST_NO  
                                        AND T13016.T_REQUEST_NO = T13018.T_REQUEST_NO  
                                        AND T13016.T_WS_CODE = T13018.T_WS_CODE   
                                        AND T03003.T_TMP_PAT_NO = T13015.T_PAT_NO   
                                        AND T13015.T_REFERRAL_CODE = T02049.T_REFERRAL_CODE   
                                        AND T13016.T_WS_CODE = T13004.T_WS_CODE   
                                        AND T13015.T_REQUEST_NO = '{reqno}'  
                                        AND T13016.T_WS_CODE = '{workstation}'  
                                        AND NVL(T13016.T_LAB_NO,'') IS NOT NULL  
                                        AND T13018.T_CLOSE_FLAG IS NOT NULL  
                                        ORDER BY T_RESULT_VALUE");
                case "13":
                    return ReportQuery($@"SELECT DISTINCT T13015.T_PAT_NO, T13015.T_REQUEST_NO, T13015.T_LAB_REF_NO REF_HOSP_NO, T13015.T_REQUEST_DATE, T13015.T_REQUEST_TIME, T13015.T_SPEC_NO, T13016.T_LAB_NO 
                                        ,(SELECT MAX(T13017.T_RECEIVED_DATE) FROM T13017 WHERE T13017.T_REQUEST_NO=' REQUEST_NO + ' AND T13017.T_WS_CODE = ' WS_CODE + ' AND T13017.T_SPECIMEN_CODE IS NOT NULL) T_RECEIVED_DATE  
                                        ,(SELECT MAX(T13017.T_RECEIVED_TIME) FROM T13017 WHERE T13017.T_REQUEST_NO=' REQUEST_NO + ' AND T13017.T_WS_CODE = ' WS_CODE + ' AND T13017.T_SPECIMEN_CODE IS NOT NULL) T_RECEIVED_TIME 
                                        , T03003.T_FIRST_LANG{lang}_NAME || ' ' || T03003.T_FATHER_LANG{lang}_NAME || ' ' || T03003.T_GFATHER_LANG{lang}_NAME || ' ' || T03003.T_FAMILY_LANG{lang}_NAME T_PAT_NAME, T03003.T_BIRTH_DATE 
                                        , NVL((SELECT DISTINCT T13005.T_LANG{lang}_NAME FROM T13005 
                                                WHERE T13005.T_SPECIMEN_CODE = (SELECT MAX(DISTINCT(T13017.T_SPECIMEN_CODE)) 
                                                                                FROM T13017
                                                                                WHERE T13017.T_REQUEST_NO='{reqno}' AND T13017.T_WS_CODE = '{workstation}' 
                                                                                AND T13017.T_SPECIMEN_CODE IS NOT NULL)), '') T_SPECIMEN_NAME 
                                        , NVL((SELECT T02006.T_LANG{lang}_NAME  FROM T02006 WHERE  T03003.T_GENDER = T02006.T_SEX_CODE ),'') T_GENDER 
                                        , NVL((SELECT DISTINCT T02003.T_LANGV_NAME FROM T02003 WHERE T03003.T_NTNLTY_CODE = T02003.T_NTNLTY_CODE), '') T_NATIONALITY 
                                        , T02049.T_LANG{lang}_NAME T_LOCATION , T13004.T_LANG{lang}_NAME || ' Department' T_LOCATION_NAME , T13018.T_NOTES, T13018.T_UPD_DATE 
                                        , nvl((SELECT DISTINCT T13006.T_LANG{lang}_NAME FROM T13006 WHERE T13006.T_RESULT_CODE = T13018.T_RESULT_VALUE),T13018.T_RESULT_VALUE)T_RESULT_VALUE 
                                        , (SELECT T13011.T_LANG{lang}_NAME FROM T13011 WHERE T13011.T_WS_CODE = T13016.T_WS_CODE AND T13011.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE) T_ANALYSIS_NAME 
                                        , DECODE((SELECT T13011.T_RESULT_TYPE FROM T13011 WHERE T13011.T_WS_CODE= T13016.T_WS_CODE AND T13011.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE) 
                                        , '1', (SELECT T13014.T_NR_FROM ||'-'||T13014.T_NR_TO FROM T13014 WHERE T13014.T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') AND T13014.T_WS_CODE= T13016.T_WS_CODE AND T13014.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE)  
                                        , '2', (SELECT T13014.T_NR_FROM ||'-'||T13014.T_NR_TO FROM T13014 WHERE T13014.T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') AND T13014.T_WS_CODE= T13016.T_WS_CODE AND T13014.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE AND T13014.T_GENDER = T03003.T_GENDER) 
                                        , '3', (SELECT T13014.T_NR_FROM ||'-'||T13014.T_NR_TO FROM T13014 WHERE T13014.T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') AND T13014.T_WS_CODE= T13016.T_WS_CODE AND T13014.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE 
                                        AND TO_NUMBER(NVL(TRUNC(MONTHS_BETWEEN(SYSDATE,T03003.T_BIRTH_DATE)/12,0),0) * 365) + TRUNC(SYSDATE)-(ADD_MONTHS(T03003.T_BIRTH_DATE,(TRUNC((MONTHS_BETWEEN(TRUNC(SYSDATE),T03003.T_BIRTH_DATE))/12))*12+(MOD((MONTHS_BETWEEN(TRUNC(SYSDATE),T03003.T_BIRTH_DATE)),12)))) 
                                        BETWEEN ((T_YEAR_FROM * 365 ) + T_DAYS_FROM) AND ((T_YEAR_TO * 365) + T_DAYS_TO)),'4',(SELECT T_NR_FROM ||'-'||T_NR_TO FROM T13014 
                                        WHERE T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') AND T_WS_CODE= T13016.T_WS_CODE AND T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE 
                                        AND TO_NUMBER(NVL(TRUNC(MONTHS_BETWEEN(SYSDATE,T03003.T_BIRTH_DATE)/12,0),0) * 365) + TRUNC(SYSDATE)-(ADD_MONTHS(T03003.T_BIRTH_DATE,(TRUNC((MONTHS_BETWEEN(TRUNC(SYSDATE),T03003.T_BIRTH_DATE))/12))*12+(MOD((MONTHS_BETWEEN(TRUNC(SYSDATE),T03003.T_BIRTH_DATE)),12)))) 
                                        BETWEEN ((T_YEAR_FROM * 365 ) + T_DAYS_FROM) AND ((T_YEAR_TO * 365) + T_DAYS_TO) AND T_GENDER = T03003.T_GENDER),'')||'  '||(SELECT T_UM FROM T13011 WHERE T_WS_CODE = T13016.T_WS_CODE AND T13011.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE) T_RANGE 
                                        FROM T13015, T13016,  T13018 , T02049,T13004, T03003  
                                        WHERE T13015.T_REQUEST_NO = T13016.T_REQUEST_NO 
                                        AND T13016.T_REQUEST_NO = T13018.T_REQUEST_NO  
                                        AND T13016.T_WS_CODE = T13018.T_WS_CODE  
                                        AND T03003.T_TMP_PAT_NO = T13015.T_PAT_NO  
                                        AND T13015.T_REFERRAL_CODE = T02049.T_REFERRAL_CODE  
                                        AND T13016.T_WS_CODE = T13004.T_WS_CODE  
                                        AND T13015.T_REQUEST_NO = '{reqno}' 
                                        AND T13016.T_WS_CODE = '{workstation}' 
                                        AND T13018.T_CLOSE_FLAG IS NOT NULL 
                                        ORDER BY T_RESULT_VALUE");
                case "17":
                case "02":
                    return ReportQuery($@"SELECT T13015.T_PAT_NO
                                        , SYSDEV.T13_GET_LAB_NO(t13018.t_request_no,t13018.t_ws_code ) T_LAB_NO
                                        , SYSDEV.T13_GET_SPECIMEN(t13018.t_request_no,t13018.t_ws_code ) T_SPECIMEN_NAME
                                        , SYSDEV.T13_GET_RECEIVED_DT(t13018.t_request_no,t13018.t_ws_code ) T_RECEIVED_DATE
                                        , T13011.T_LANG{lang}_NAME T_ANALYSIS_NAME, T13015.T_REQUEST_NO, T13015.T_LAB_REF_NO REF_HOSP_NO
                                        , T13015.T_REQUEST_DATE, T13015.T_REQUEST_TIME, T13015.T_SPEC_NO  
                                        , T03003.T_FIRST_LANG{lang}_NAME || ' ' || T03003.T_FATHER_LANG{lang}_NAME || ' ' || T03003.T_GFATHER_LANG{lang}_NAME || ' ' || T03003.T_FAMILY_LANG{lang}_NAME T_PAT_NAME
                                        , T03003.T_BIRTH_DATE 
                                        , NVL((select T02006.T_LANG{lang}_NAME  from T02006 WHERE  T03003.T_GENDER = T02006.T_SEX_CODE ),'') T_GENDER 
                                        , NVL((SELECT DISTINCT T02003.T_LANG{lang}_NAME FROM T02003 WHERE T03003.T_NTNLTY_CODE = T02003.T_NTNLTY_CODE), '') T_NATIONALITY  
                                        , T02049.T_LANG{lang}_NAME T_LOCATION 
                                        , T13004.T_LANG{lang}_NAME || ' Department' T_LOCATION_NAME 
                                        , T13018.T_NOTES, T13018.T_UPD_DATE  
                                        , NVL((SELECT DISTINCT T13006.T_LANG{lang}_NAME FROM T13006 WHERE T13006.T_RESULT_CODE = T13018.T_RESULT_VALUE),T13018.T_RESULT_VALUE)T_RESULT_VALUE  
                                        , SYSDEV.T13_GET_NR_RANGE(T13018.T_REQUEST_NO,T13018.T_WS_CODE,T13018.T_ANALYSIS_CODE ,T13018.T_ANALYZER_ID,T13011.T_RESULT_TYPE) T_RANGE 
                                        FROM T13015, T03003,  T13018 , T02049  ,T13011 ,t13004
                                        WHERE T13015.T_REQUEST_NO = T13018.T_REQUEST_NO
                                        AND T03003.T_TMP_PAT_NO = T13015.T_PAT_NO  
                                        AND T13015.T_REFERRAL_CODE = T02049.T_REFERRAL_CODE  
                                        AND T13018.T_WS_CODE = T13004.T_WS_CODE 
                                        AND T13015.T_REQUEST_NO = '{reqno}' 
                                        AND T13018.T_WS_CODE = '{workstation}'
                                        AND T13011.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE  
                                        AND T13011.T_WS_CODE = T13018.T_WS_CODE 
                                        AND T13018.T_CLOSE_FLAG IS NOT NULL 
                                        AND ( T13018.T_RESULT_VALUE IS NOT NULL OR T13018.T_NOTES IS NOT NULL) 
                                        ORDER BY T13011.T_LANG{lang}_NAME");
                case "10":
                    string analysisCode = "";
                    switch (analysisCode)
                    {
                        case "1002":
                            analysisCode = $" AND T13017.T_ANALYSIS_CODE='{analysisCode}' AND  T13018.T_ANALYSIS_CODE in('1043','1044','1045','1046','1047','1048','1049','1050','1051','1052','1053')";
                            break;
                        case "1001":
                        case "1062":
                        case "1063":
                        case "10201":
                        case "10204":
                        case "10207":
                        case "10210":
                            analysisCode = $" AND T13017.T_ANALYSIS_CODE='{analysisCode}' AND T13018.T_ANALYSIS_CODE not in('1043','1044','1045','1046','1047','1048','1049','1050','1051','1052','1053')";
                            break;
                    }
                    return ReportQuery($@"SELECT DISTINCT T13015.T_PAT_NO, T13015.T_REQUEST_NO, T13015.T_LAB_REF_NO REF_HOSP_NO, T13015.T_REQUEST_DATE, T13015.T_REQUEST_TIME, T13015.T_SPEC_NO, T13016.T_LAB_NO, T13017.T_RECEIVED_DATE, T13017.T_RECEIVED_TIME 
                                         , T03003.T_FIRST_LANG{lang}_NAME || ' ' || T03003.T_FATHER_LANG{lang}_NAME || ' ' || T03003.T_GFATHER_LANG{lang}_NAME || ' ' || T03003.T_FAMILY_LANG{lang}_NAME T_PAT_NAME, T03003.T_BIRTH_DATE 
                                         , NVL((SELECT DISTINCT T13005.T_LANG{lang}_NAME FROM T13005 WHERE T13005.T_SPECIMEN_CODE = T13017.T_SPECIMEN_CODE), '') T_SPECIMEN_NAME 
                                         ,  NVL((select T02006.T_LANG{lang}_NAME  from T02006 WHERE  T03003.T_GENDER = T02006.T_SEX_CODE ),'') T_GENDER , NVL((SELECT DISTINCT T02003.T_LANG{lang}_NAME FROM T02003 WHERE T03003.T_NTNLTY_CODE = T02003.T_NTNLTY_CODE), '') T_NATIONALITY 
                                         , T02049.T_LANG{lang}_NAME T_LOCATION , T13004.T_LANG{lang}_NAME || ' Department' T_LOCATION_NAME , T13018.T_NOTES, T13018.T_UPD_DATE 
                                         , nvl((SELECT DISTINCT T13006.T_LANG{lang}_NAME FROM T13006 WHERE T13006.T_RESULT_CODE = T13018.T_RESULT_VALUE),T13018.T_RESULT_VALUE)T_RESULT_VALUE 
                                         , (SELECT T13011.T_LANG{lang}_NAME FROM T13011 WHERE T13011.T_WS_CODE = T13016.T_WS_CODE AND T13011.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE) T_ANALYSIS_NAME 
                                         , DECODE((SELECT T13011.T_RESULT_TYPE FROM T13011 WHERE T13011.T_WS_CODE= T13016.T_WS_CODE AND T13011.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE) 
                                         , '1', (SELECT T13014.T_NR_FROM ||'-'||T13014.T_NR_TO FROM T13014 WHERE T13014.T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') AND T13014.T_WS_CODE= T13016.T_WS_CODE AND T13014.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE)  
                                         , '2', (SELECT T13014.T_NR_FROM ||'-'||T13014.T_NR_TO FROM T13014 WHERE T13014.T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') AND T13014.T_WS_CODE= T13016.T_WS_CODE AND T13014.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE AND T13014.T_GENDER = T03003.T_GENDER) 
                                         , '3', (SELECT T13014.T_NR_FROM ||'-'||T13014.T_NR_TO FROM T13014 WHERE T13014.T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') AND T13014.T_WS_CODE= T13016.T_WS_CODE AND T13014.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE 
                                         AND TO_NUMBER(NVL(TRUNC(MONTHS_BETWEEN(SYSDATE,T03003.T_BIRTH_DATE)/12,0),0) * 365) + TRUNC(SYSDATE)-(ADD_MONTHS(T03003.T_BIRTH_DATE,(TRUNC((MONTHS_BETWEEN(TRUNC(SYSDATE),T03003.T_BIRTH_DATE))/12))*12+(MOD((MONTHS_BETWEEN(TRUNC(SYSDATE),T03003.T_BIRTH_DATE)),12)))) 
                                         BETWEEN ((T_YEAR_FROM * 365 ) + T_DAYS_FROM) AND ((T_YEAR_TO * 365) + T_DAYS_TO)),'4',(SELECT T_NR_FROM ||'-'||T_NR_TO FROM T13014 
                                         WHERE T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') AND T_WS_CODE= T13016.T_WS_CODE AND T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE 
                                         AND TO_NUMBER(NVL(TRUNC(MONTHS_BETWEEN(SYSDATE,T03003.T_BIRTH_DATE)/12,0),0) * 365) + TRUNC(SYSDATE)-(ADD_MONTHS(T03003.T_BIRTH_DATE,(TRUNC((MONTHS_BETWEEN(TRUNC(SYSDATE),T03003.T_BIRTH_DATE))/12))*12+(MOD((MONTHS_BETWEEN(TRUNC(SYSDATE),T03003.T_BIRTH_DATE)),12)))) 
                                         BETWEEN ((T_YEAR_FROM * 365 ) + T_DAYS_FROM) AND ((T_YEAR_TO * 365) + T_DAYS_TO) AND T_GENDER = T03003.T_GENDER),(SELECT T13014.T_NR_FROM ||'-'||T13014.T_NR_TO FROM T13014 WHERE T13014.T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'05') AND T13014.T_WS_CODE= T13016.T_WS_CODE AND T13014.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE) 
                                         )||' '||(SELECT T_UM FROM T13011 WHERE T_WS_CODE = T13016.T_WS_CODE AND T13011.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE) T_RANGE 
                                         FROM T13015, T13016, T13017, T03003,  T13018 , T02049,T13004 
                                         WHERE T13015.T_REQUEST_NO = T13016.T_REQUEST_NO 
                                         AND T13015.T_REQUEST_NO = T13017.T_REQUEST_NO 
                                         AND T13016.T_WS_CODE = T13017.T_WS_CODE 
                                         AND T13016.T_WS_CODE = T13018.T_WS_CODE 
                                         AND T03003.T_TMP_PAT_NO = T13015.T_PAT_NO 
                                         AND T13015.T_REFERRAL_CODE = T02049.T_REFERRAL_CODE 
                                         AND T13016.T_WS_CODE = T13004.T_WS_CODE 
                                         AND T13015.T_REQUEST_NO = T13018.T_REQUEST_NO 
                                         AND T13015.T_REQUEST_NO = '{reqno}' 
                                         AND T13016.T_WS_CODE = '{workstation}' 
                                         AND T13017.T_SPECIMEN_CODE IS NOT NULL 
                                         '{analysisCode}'
                                          AND T13018.T_CLOSE_FLAG IS NOT NULL
                                          ORDER BY T_RESULT_VALUE");
                case "22":
                    return ReportQuery($@"SELECT DISTINCT T13015.T_PAT_NO, T13015.T_REQUEST_NO, T13015.T_LAB_REF_NO REF_HOSP_NO, T13015.T_REQUEST_DATE, T13015.T_REQUEST_TIME, T13015.T_SPEC_NO
                                        , T13017.T_LAB_NO, T13017.T_RECEIVED_DATE, T13017.T_RECEIVED_TIME
                                        , T03003.T_FIRST_LANG{lang}_NAME || ' ' || T03003.T_FATHER_LANG{lang}_NAME || ' ' || T03003.T_GFATHER_LANG{lang}_NAME || ' ' || T03003.T_FAMILY_LANG{lang}_NAME T_PAT_NAME
                                        , T03003.T_BIRTH_DATE, NVL((SELECT DISTINCT T13005.T_LANG{lang}_NAME FROM T13005 WHERE T13005.T_SPECIMEN_CODE = T13017.T_SPECIMEN_CODE), '') T_SPECIMEN_NAME
                                        , NVL((SELECT T02006.T_LANG{lang}_NAME FROM T02006 WHERE T03003.T_GENDER = T02006.T_SEX_CODE ), '') T_GENDER
                                        , NVL((SELECT DISTINCT T02003.T_LANG{lang}_NAME FROM T02003 WHERE T03003.T_NTNLTY_CODE = T02003.T_NTNLTY_CODE), '') T_NATIONALITY, T02049.T_LANG{lang}_NAME T_LOCATION
                                        , T13004.T_LANG{lang}_NAME || ' Department' T_LOCATION_NAME, T13018.T_NOTES, T13018.T_UPD_DATE
                                        , NVL((SELECT DISTINCT T13006.T_LANG{lang}_NAME FROM T13006 WHERE T13006.T_RESULT_CODE = T13018.T_RESULT_VALUE), T13018.T_RESULT_VALUE) T_RESULT_VALUE
                                        , (SELECT T13011.T_LANG{lang}_NAME FROM T13011 WHERE T13011.T_WS_CODE = T13017.T_WS_CODE AND T13011.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE) T_ANALYSIS_NAME
                                        , DECODE((SELECT T13011.T_RESULT_TYPE FROM T13011 WHERE T13011.T_WS_CODE= T13017.T_WS_CODE AND T13011.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE), 
                                        '1', (SELECT T_NR_FROM ||'-'||T_NR_TO FROM T13014 WHERE T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') AND T13014.T_WS_CODE = T13017.T_WS_CODE 
                                        AND T13014.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE), 
                                        '2', (SELECT T_NR_FROM ||'-'||T_NR_TO FROM T13014 WHERE T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') AND T13014.T_WS_CODE = T13017.T_WS_CODE 
                                        AND T13014.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE AND T13014.T_GENDER = T03003.T_GENDER), 
                                        '3', (SELECT T_NR_FROM ||'-'||T_NR_TO FROM T13014 WHERE T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') AND T_WS_CODE = T13017.T_WS_CODE 
                                        AND T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE AND TO_NUMBER(NVL(TRUNC(MONTHS_BETWEEN(SYSDATE,T03003.T_BIRTH_DATE)/12,0),0) * 365) + 
                                        TRUNC(SYSDATE)-(ADD_MONTHS(T03003.T_BIRTH_DATE, (TRUNC((MONTHS_BETWEEN(TRUNC(SYSDATE), T03003.T_BIRTH_DATE))/12))*12+(MOD((MONTHS_BETWEEN(TRUNC(SYSDATE), 
                                        T03003.T_BIRTH_DATE)), 12)))) BETWEEN ((T_YEAR_FROM * 365 ) + T_DAYS_FROM) AND ((T_YEAR_TO * 365) + T_DAYS_TO)), 
                                        '4', (SELECT T_NR_FROM ||'-'||T_NR_TO FROM T13014  WHERE T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') AND T_WS_CODE = T13017.T_WS_CODE AND 
                                        T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE AND TO_NUMBER(NVL(TRUNC(MONTHS_BETWEEN(SYSDATE,T03003.T_BIRTH_DATE)/12,0),0) * 365) + 
                                        TRUNC(SYSDATE)-(ADD_MONTHS(T03003.T_BIRTH_DATE, (TRUNC((MONTHS_BETWEEN(TRUNC(SYSDATE), T03003.T_BIRTH_DATE))/12))*12+(MOD((MONTHS_BETWEEN(TRUNC(SYSDATE), 
                                        T03003.T_BIRTH_DATE)), 12)))) BETWEEN ((T_YEAR_FROM * 365 ) + T_DAYS_FROM) AND ((T_YEAR_TO * 365) + T_DAYS_TO) AND T_GENDER = T03003.T_GENDER), 
                                        (SELECT T_NR_FROM ||'-'||T_NR_TO FROM T13014 WHERE T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'05') AND T_WS_CODE = T13017.T_WS_CODE AND 
                                        T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE))||' '||(SELECT T_UM FROM T13011 WHERE T_WS_CODE = T13017.T_WS_CODE AND T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE) T_RANGE 
                                        FROM T13015, T13017, T03003,  T13018 , T02049, T13004 
                                        WHERE T13015.T_REQUEST_NO = T13017.T_REQUEST_NO 
                                        AND T13017.T_WS_CODE = T13018.T_WS_CODE 
                                        AND T03003.T_TMP_PAT_NO = T13015.T_PAT_NO 
                                        AND T13015.T_REFERRAL_CODE = T02049.T_REFERRAL_CODE 
                                        AND T13017.T_WS_CODE = T13004.T_WS_CODE 
                                        AND T13015.T_REQUEST_NO = T13018.T_REQUEST_NO 
                                        AND T13015.T_REQUEST_NO = '{reqno}' 
                                        AND T13017.T_WS_CODE = '{workstation}' 
                                        AND T13018.T_CLOSE_FLAG IS NOT NULL 
                                        ORDER BY T_RESULT_VALUE");
                case "08":
                    return ReportQuery($@"SELECT DISTINCT T13015.T_PAT_NO, T13015.T_REQUEST_NO, T13015.T_LAB_REF_NO REF_HOSP_NO, T13015.T_REQUEST_DATE, T13015.T_REQUEST_TIME, T13015.T_SPEC_NO 
                                        , NVL((SELECT MAX(T13017.T_SPECIMEN_CODE) FROM T13017 WHERE T13017.T_REQUEST_NO= '{reqno}'),'') T_LAB_NO 
                                        , T13017.T_RECEIVED_DATE, T13017.T_RECEIVED_TIME 
                                        , T03003.T_FIRST_LANG{lang}_NAME || ' ' || T03003.T_FATHER_LANG{lang}_NAME || ' ' || T03003.T_GFATHER_LANG{lang}_NAME || ' ' || T03003.T_FAMILY_LANG{lang}_NAME T_PAT_NAME, T03003.T_BIRTH_DATE 
                                        , NVL((SELECT max( T13005.T_LANG{lang}_NAME) FROM T13005 WHERE T13005.T_SPECIMEN_CODE = (select max(T13017.T_SPECIMEN_CODE) FROM T13017 WHERE T13017.t_request_no= ' REQUEST_NO + ')), '') T_SPECIMEN_NAME 
                                        , NVL((select T02006.T_LANG{lang}_NAME from T02006 WHERE T03003.T_GENDER = T02006.T_SEX_CODE ), '') T_GENDER 
                                        , NVL((SELECT DISTINCT T02003.T_LANG{lang}_NAME FROM T02003 WHERE T03003.T_NTNLTY_CODE = T02003.T_NTNLTY_CODE), '') T_NATIONALITY, T02049.T_LANG{lang}_NAME T_LOCATION, T13004.T_LANG{lang}_NAME || ' Department' T_LOCATION_NAME, T13018.T_NOTES, T13018.T_UPD_DATE
                                        , NVL((SELECT DISTINCT T13006.T_LANG{lang}_NAME FROM T13006 WHERE T13006.T_RESULT_CODE = T13018.T_RESULT_VALUE), T13018.T_RESULT_VALUE) T_RESULT_VALUE 
                                        , (SELECT T13011.T_LANG{lang}_NAME FROM T13011 WHERE T13011.T_WS_CODE = T13018.T_WS_CODE AND T13011.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE) T_ANALYSIS_NAME 
                                        , DECODE((SELECT T13011.T_RESULT_TYPE FROM T13011 WHERE T13011.T_WS_CODE= T13018.T_WS_CODE AND T13011.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE), '1', 
                                                 (SELECT T_NR_FROM ||'-'||T_NR_TO FROM T13014 WHERE T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') AND T13014.T_WS_CODE = T13018.T_WS_CODE AND T13014.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE), '2', 
                                                 (SELECT T_NR_FROM ||'-'||T_NR_TO FROM T13014 WHERE T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') AND T13014.T_WS_CODE = T13018.T_WS_CODE AND T13014.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE AND T13014.T_GENDER = T03003.T_GENDER), '3', 
                                                 (SELECT T_NR_FROM ||'-'||T_NR_TO FROM T13014 WHERE T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') AND T_WS_CODE = T13018.T_WS_CODE AND T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE 
                                                  AND TO_NUMBER(NVL(TRUNC(MONTHS_BETWEEN(SYSDATE,T03003.T_BIRTH_DATE)/12,0),0) * 365) + TRUNC(SYSDATE)-(ADD_MONTHS(T03003.T_BIRTH_DATE, (TRUNC((MONTHS_BETWEEN(TRUNC(SYSDATE), T03003.T_BIRTH_DATE))/12))*12+(MOD((MONTHS_BETWEEN(TRUNC(SYSDATE), T03003.T_BIRTH_DATE)), 12)))) 
                                                  BETWEEN ((T_YEAR_FROM * 365 ) + T_DAYS_FROM) AND ((T_YEAR_TO * 365) + T_DAYS_TO)), '4', 
                                                 (SELECT T_NR_FROM ||'-'||T_NR_TO FROM T13014  WHERE T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') AND T_WS_CODE = T13018.T_WS_CODE AND T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE 
                                                  AND TO_NUMBER(NVL(TRUNC(MONTHS_BETWEEN(SYSDATE,T03003.T_BIRTH_DATE)/12,0),0) * 365) + TRUNC(SYSDATE)-(ADD_MONTHS(T03003.T_BIRTH_DATE, (TRUNC((MONTHS_BETWEEN(TRUNC(SYSDATE), T03003.T_BIRTH_DATE))/12))*12+(MOD((MONTHS_BETWEEN(TRUNC(SYSDATE), T03003.T_BIRTH_DATE)), 12)))) 
                                                  BETWEEN ((T_YEAR_FROM * 365 ) + T_DAYS_FROM) AND ((T_YEAR_TO * 365) + T_DAYS_TO) AND T_GENDER = T03003.T_GENDER), 
                                                  (SELECT T_NR_FROM ||'-'||T_NR_TO FROM T13014 WHERE T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'05') AND T_WS_CODE = T13018.T_WS_CODE AND T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE))||' '||(SELECT T_UM FROM T13011 WHERE T_WS_CODE = T13018.T_WS_CODE AND T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE) T_RANGE  
                                        FROM T13015,  T03003,  T13018 , T02049, T13004 ,T13017 
                                        WHERE T13015.T_REQUEST_NO = T13018.T_REQUEST_NO  
                                        AND T13017.T_WS_CODE = T13018.T_WS_CODE  
                                        AND T03003.T_TMP_PAT_NO = T13015.T_PAT_NO  
                                        AND T13015.T_REFERRAL_CODE = T02049.T_REFERRAL_CODE  
                                        AND T13018.T_WS_CODE = T13004.T_WS_CODE  
                                        AND T13015.T_REQUEST_NO = T13017.T_REQUEST_NO  
                                        AND T13015.T_REQUEST_NO = '{reqno}' 
                                        AND T13017.T_WS_CODE = '{workstation}' 
                                        AND T13018.T_CLOSE_FLAG IS NOT NULL AND T13017.T_RECEIVED_TIME  IS NOT NULL 
                                        ORDER BY T_RESULT_VALUE");
                case "15":
                case "19":
                case "25":
                    return ReportQuery($@"SELECT DISTINCT T13015.T_PAT_NO, T13015.T_REQUEST_NO, T13015.T_LAB_REF_NO REF_HOSP_NO, T13015.T_REQUEST_DATE, T13015.T_REQUEST_TIME, T13015.T_SPEC_NO, 
                                        T13017.T_LAB_NO, T13017.T_RECEIVED_DATE, T13017.T_RECEIVED_TIME, 
                                        T03003.T_FIRST_LANG{lang}_NAME || ' ' || T03003.T_FATHER_LANG{lang}_NAME || ' ' || T03003.T_GFATHER_LANG{lang}_NAME || ' ' || T03003.T_FAMILY_LANG{lang}_NAME T_PAT_NAME, 
                                        T03003.T_BIRTH_DATE, NVL((SELECT DISTINCT T13005.T_LANG{lang}_NAME FROM T13005 WHERE T13005.T_SPECIMEN_CODE = T13017.T_SPECIMEN_CODE), '') T_SPECIMEN_NAME, 
                                        NVL((select T02006.T_LANG{lang}_NAME from T02006 WHERE T03003.T_GENDER = T02006.T_SEX_CODE ), '') T_GENDER, 
                                        NVL((SELECT DISTINCT T02003.T_LANG{lang}_NAME FROM T02003 WHERE T03003.T_NTNLTY_CODE = T02003.T_NTNLTY_CODE), '') T_NATIONALITY, 
                                        T02049.T_LANG{lang}_NAME T_LOCATION, T13004.T_LANG{lang}_NAME || ' Department' T_LOCATION_NAME, T13018.T_NOTES, T13018.T_UPD_DATE, 
                                        NVL((SELECT DISTINCT T13006.T_LANG{lang}_NAME FROM T13006 WHERE T13006.T_RESULT_CODE = T13018.T_RESULT_VALUE), T13018.T_RESULT_VALUE) T_RESULT_VALUE, 
                                        (SELECT T13011.T_LANG{lang}_NAME FROM T13011 WHERE T13011.T_WS_CODE = T13017.T_WS_CODE AND T13011.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE) T_ANALYSIS_NAME, 
                                        DECODE((SELECT T13011.T_RESULT_TYPE FROM T13011 WHERE T13011.T_WS_CODE= T13017.T_WS_CODE AND T13011.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE), 
                                        '1', (SELECT T_NR_FROM ||'-'||T_NR_TO FROM T13014 WHERE T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') 
                                        AND T13014.T_WS_CODE = T13017.T_WS_CODE AND T13014.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE), 
                                        '2', (SELECT T_NR_FROM ||'-'||T_NR_TO FROM T13014 WHERE T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') 
                                        AND T13014.T_WS_CODE = T13017.T_WS_CODE AND T13014.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE AND T13014.T_GENDER = T03003.T_GENDER), 
                                        '3', (SELECT T_NR_FROM ||'-'||T_NR_TO FROM T13014 WHERE T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') 
                                        AND T_WS_CODE = T13017.T_WS_CODE AND T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE AND 
                                        TO_NUMBER(NVL(TRUNC(MONTHS_BETWEEN(SYSDATE,T03003.T_BIRTH_DATE)/12,0),0) * 365) + TRUNC(SYSDATE)-(ADD_MONTHS(T03003.T_BIRTH_DATE, 
                                        (TRUNC((MONTHS_BETWEEN(TRUNC(SYSDATE), T03003.T_BIRTH_DATE))/12))*12+(MOD((MONTHS_BETWEEN(TRUNC(SYSDATE), T03003.T_BIRTH_DATE)), 12)))) 
                                        BETWEEN ((T_YEAR_FROM * 365 ) + T_DAYS_FROM) AND ((T_YEAR_TO * 365) + T_DAYS_TO)), 
                                        '4', (SELECT T_NR_FROM ||'-'||T_NR_TO FROM T13014  WHERE T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') 
                                        AND T_WS_CODE = T13017.T_WS_CODE AND T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE AND 
                                        TO_NUMBER(NVL(TRUNC(MONTHS_BETWEEN(SYSDATE,T03003.T_BIRTH_DATE)/12,0),0) * 365) + TRUNC(SYSDATE)-(ADD_MONTHS(T03003.T_BIRTH_DATE, 
                                        (TRUNC((MONTHS_BETWEEN(TRUNC(SYSDATE), T03003.T_BIRTH_DATE))/12))*12+(MOD((MONTHS_BETWEEN(TRUNC(SYSDATE), T03003.T_BIRTH_DATE)), 12)))) 
                                        BETWEEN ((T_YEAR_FROM * 365 ) + T_DAYS_FROM) AND ((T_YEAR_TO * 365) + T_DAYS_TO) AND T_GENDER = T03003.T_GENDER), 
                                        (SELECT T_NR_FROM ||'-'||T_NR_TO FROM T13014 WHERE T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'05') AND T_WS_CODE = T13017.T_WS_CODE 
                                        AND T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE))||' '||(SELECT T_UM FROM T13011 WHERE T_WS_CODE = T13017.T_WS_CODE AND T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE) T_RANGE 
                                        FROM T13015, T13017, T03003,  T13018 , T02049, T13004 
                                        WHERE T13015.T_REQUEST_NO = T13017.T_REQUEST_NO 
                                        AND T13017.T_WS_CODE = T13018.T_WS_CODE
                                        AND T03003.T_TMP_PAT_NO = T13015.T_PAT_NO 
                                        AND T13015.T_REFERRAL_CODE = T02049.T_REFERRAL_CODE 
                                        AND T13017.T_WS_CODE = T13004.T_WS_CODE 
                                        AND T13015.T_REQUEST_NO = T13018.T_REQUEST_NO 
                                        AND T13015.T_REQUEST_NO = '{reqno}' 
                                        AND T13017.T_WS_CODE = '{workstation}' 
                                        AND T13018.T_CLOSE_FLAG IS NOT NULL 
                                        ORDER BY T_RESULT_VALUE");
                default:
                    return ReportQuery($@"SELECT DISTINCT T13015.T_PAT_NO, T13015.T_REQUEST_NO, T13015.T_LAB_REF_NO REF_HOSP_NO, T13015.T_REQUEST_DATE, T13015.T_REQUEST_TIME, T13015.T_SPEC_NO, 
                                        T13017.T_LAB_NO, T13017.T_RECEIVED_DATE, T13017.T_RECEIVED_TIME, 
                                        T03003.T_FIRST_LANG{lang}_NAME || ' ' || T03003.T_FATHER_LANG{lang}_NAME || ' ' || T03003.T_GFATHER_LANG{lang}_NAME || ' ' || T03003.T_FAMILY_LANG{lang}_NAME T_PAT_NAME, 
                                        T03003.T_BIRTH_DATE, NVL((SELECT DISTINCT T13005.T_LANG{lang}_NAME FROM T13005 WHERE T13005.T_SPECIMEN_CODE = T13017.T_SPECIMEN_CODE), '') T_SPECIMEN_NAME, 
                                        NVL((select T02006.T_LANG{lang}_NAME from T02006 WHERE T03003.T_GENDER = T02006.T_SEX_CODE ), '') T_GENDER, 
                                        NVL((SELECT DISTINCT T02003.T_LANG{lang}_NAME FROM T02003 WHERE T03003.T_NTNLTY_CODE = T02003.T_NTNLTY_CODE), '') T_NATIONALITY, 
                                        T02049.T_LANG{lang}_NAME T_LOCATION, T13004.T_LANG{lang}_NAME || ' Department' T_LOCATION_NAME, T13018.T_NOTES, T13018.T_UPD_DATE, 
                                        NVL((SELECT DISTINCT T13006.T_LANG{lang}_NAME FROM T13006 WHERE T13006.T_RESULT_CODE = T13018.T_RESULT_VALUE), T13018.T_RESULT_VALUE) T_RESULT_VALUE, 
                                        (SELECT T13011.T_LANG{lang}_NAME FROM T13011 WHERE T13011.T_WS_CODE = T13017.T_WS_CODE AND T13011.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE) T_ANALYSIS_NAME, 
                                        DECODE((SELECT T13011.T_RESULT_TYPE FROM T13011 WHERE T13011.T_WS_CODE= T13017.T_WS_CODE AND T13011.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE), 
                                        '1', (SELECT T_NR_FROM ||'-'||T_NR_TO FROM T13014 WHERE T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') 
                                        AND T13014.T_WS_CODE = T13017.T_WS_CODE AND T13014.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE), 
                                        '2', (SELECT T_NR_FROM ||'-'||T_NR_TO FROM T13014 WHERE T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') 
                                        AND T13014.T_WS_CODE = T13017.T_WS_CODE AND T13014.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE AND T13014.T_GENDER = T03003.T_GENDER), 
                                        '3', (SELECT T_NR_FROM ||'-'||T_NR_TO FROM T13014 WHERE T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') 
                                        AND T_WS_CODE = T13017.T_WS_CODE AND T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE AND 
                                        TO_NUMBER(NVL(TRUNC(MONTHS_BETWEEN(SYSDATE,T03003.T_BIRTH_DATE)/12,0),0) * 365) + TRUNC(SYSDATE)-(ADD_MONTHS(T03003.T_BIRTH_DATE, 
                                        (TRUNC((MONTHS_BETWEEN(TRUNC(SYSDATE), T03003.T_BIRTH_DATE))/12))*12+(MOD((MONTHS_BETWEEN(TRUNC(SYSDATE), T03003.T_BIRTH_DATE)), 12)))) 
                                        BETWEEN ((T_YEAR_FROM * 365 ) + T_DAYS_FROM) AND ((T_YEAR_TO * 365) + T_DAYS_TO)), 
                                        '4', (SELECT T_NR_FROM ||'-'||T_NR_TO FROM T13014  WHERE T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') 
                                        AND T_WS_CODE = T13017.T_WS_CODE AND T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE AND 
                                        TO_NUMBER(NVL(TRUNC(MONTHS_BETWEEN(SYSDATE,T03003.T_BIRTH_DATE)/12,0),0) * 365) + TRUNC(SYSDATE)-(ADD_MONTHS(T03003.T_BIRTH_DATE, 
                                        (TRUNC((MONTHS_BETWEEN(TRUNC(SYSDATE), T03003.T_BIRTH_DATE))/12))*12+(MOD((MONTHS_BETWEEN(TRUNC(SYSDATE), T03003.T_BIRTH_DATE)), 12)))) 
                                        BETWEEN ((T_YEAR_FROM * 365 ) + T_DAYS_FROM) AND ((T_YEAR_TO * 365) + T_DAYS_TO) AND T_GENDER = T03003.T_GENDER), 
                                        (SELECT T_NR_FROM ||'-'||T_NR_TO FROM T13014 WHERE T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'05') AND T_WS_CODE = T13017.T_WS_CODE 
                                        AND T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE))||' '||(SELECT T_UM FROM T13011 WHERE T_WS_CODE = T13017.T_WS_CODE AND T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE) T_RANGE 
                                        FROM T13015, T13017, T03003,  T13018 , T02049, T13004 
                                        WHERE T13015.T_REQUEST_NO = T13017.T_REQUEST_NO 
                                        AND T13017.T_WS_CODE = T13018.T_WS_CODE 
                                         AND T13017.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE  
                                        AND T03003.T_TMP_PAT_NO = T13015.T_PAT_NO 
                                        AND T13015.T_REFERRAL_CODE = T02049.T_REFERRAL_CODE 
                                        AND T13017.T_WS_CODE = T13004.T_WS_CODE 
                                        AND T13015.T_REQUEST_NO = T13018.T_REQUEST_NO 
                                        AND T13015.T_REQUEST_NO = '{reqno}' 
                                        AND T13017.T_WS_CODE = '{workstation}' 
                                        AND T13018.T_CLOSE_FLAG IS NOT NULL 
                                        ORDER BY T_RESULT_VALUE");
            }
        }

       /** for PM page
        public DataTable GetAllR13128PM(string reqno, string workstation, string analysis)
        {
            if(workstation == "03")
                return ReportQuery($@"SELECT DISTINCT T13015.T_PAT_NO, T13015.T_REQUEST_NO, T13015.T_LAB_REF_NO REF_HOSP_NO, T13015.T_REQUEST_DATE, T13015.T_REQUEST_TIME, T13015.T_SPEC_NO, T13016.T_LAB_NO  
                                    ,(SELECT MAX(T13017.T_RECEIVED_DATE) FROM T13017 WHERE T13017.T_REQUEST_NO='{reqno}' AND T13017.T_WS_CODE = '{workstation}' AND T13017.T_SPECIMEN_CODE IS NOT NULL) T_RECEIVED_DATE   
                                    ,(SELECT MAX(T13017.T_RECEIVED_TIME) FROM T13017 WHERE T13017.T_REQUEST_NO='{reqno}' AND T13017.T_WS_CODE = '{workstation}' AND T13017.T_SPECIMEN_CODE IS NOT NULL) T_RECEIVED_TIME  
                                    , T03001.T_FIRST_LANG2_NAME || ' ' || T03001.T_FATHER_LANG2_NAME || ' ' || T03001.T_GFATHER_LANG2_NAME || ' ' || T03001.T_FAMILY_LANG2_NAME T_PAT_NAME, T03001.T_BIRTH_DATE  
                                    , NVL((SELECT DISTINCT T13005.T_LANG2_NAME FROM T13005 
                                            WHERE T13005.T_SPECIMEN_CODE = (SELECT MAX(DISTINCT(T13017.T_SPECIMEN_CODE)) FROM T13017 
                                                                            WHERE T13017.T_REQUEST_NO='{reqno}' AND T13017.T_WS_CODE = '{workstation}' AND T13017.T_SPECIMEN_CODE IS NOT NULL)), '') T_SPECIMEN_NAME  
                                    , NVL((SELECT T02006.T_LANG2_NAME  FROM T02006 WHERE  T03001.T_GENDER = T02006.T_SEX_CODE ),'') T_GENDER 
                                    , NVL((SELECT DISTINCT T02003.T_LANG2_NAME FROM T02003 WHERE T03001.T_NTNLTY_CODE = T02003.T_NTNLTY_CODE), '') T_NATIONALITY  
                                    , T02049.T_LANG2_NAME T_LOCATION, T13004.T_LANG2_NAME || 'Department' T_LOCATION_NAME, T13018.T_NOTES, T13018.T_UPD_DATE  
                                    , NVL((SELECT DISTINCT T13006.T_LANG2_NAME FROM T13006 WHERE T13006.T_RESULT_CODE = T13018.T_RESULT_VALUE),T13018.T_RESULT_VALUE)T_RESULT_VALUE  
                                    , (SELECT T13011.T_LANG2_NAME FROM T13011 WHERE T13011.T_WS_CODE = T13016.T_WS_CODE AND T13011.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE) T_ANALYSIS_NAME  
                                    , DECODE((SELECT T13011.T_RESULT_TYPE FROM T13011 WHERE T13011.T_WS_CODE= T13016.T_WS_CODE AND T13011.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE)  
                                    , '1', (SELECT T13014.T_NR_FROM ||'-'||T13014.T_NR_TO FROM T13014 WHERE T13014.T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') AND T13014.T_WS_CODE= T13016.T_WS_CODE AND T13014.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE)   
                                    , '2', (SELECT T13014.T_NR_FROM ||'-'||T13014.T_NR_TO FROM T13014 WHERE T13014.T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') AND T13014.T_WS_CODE= T13016.T_WS_CODE AND T13014.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE AND T13014.T_GENDER = T03001.T_GENDER)  
                                    , '3', (SELECT T13014.T_NR_FROM ||'-'||T13014.T_NR_TO FROM T13014 WHERE T13014.T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') AND T13014.T_WS_CODE= T13016.T_WS_CODE AND T13014.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE  
                                    AND TO_NUMBER(NVL(TRUNC(MONTHS_BETWEEN(SYSDATE,T03001.T_BIRTH_DATE)/12,0),0) * 365)+TRUNC(SYSDATE)-(ADD_MONTHS(T03001.T_BIRTH_DATE,(TRUNC((MONTHS_BETWEEN(TRUNC(SYSDATE),T03001.T_BIRTH_DATE))/12))*12+(MOD((MONTHS_BETWEEN(TRUNC(SYSDATE),T03001.T_BIRTH_DATE)),12))))  
                                    BETWEEN ((T_YEAR_FROM * 365 )+T_DAYS_FROM) AND ((T_YEAR_TO * 365)+T_DAYS_TO)),'4',(SELECT T_NR_FROM ||'-'||T_NR_TO FROM T13014  
                                    WHERE T_ANALYZER_ID = NVL(T13018.T_ANALYZER_ID,'02') AND T_WS_CODE= T13016.T_WS_CODE AND T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE  
                                    AND TO_NUMBER(NVL(TRUNC(MONTHS_BETWEEN(SYSDATE,T03001.T_BIRTH_DATE)/12,0),0) * 365)+TRUNC(SYSDATE)-(ADD_MONTHS(T03001.T_BIRTH_DATE,(TRUNC((MONTHS_BETWEEN(TRUNC(SYSDATE),T03001.T_BIRTH_DATE))/12))*12+(MOD((MONTHS_BETWEEN(TRUNC(SYSDATE),T03001.T_BIRTH_DATE)),12))))  
                                    BETWEEN ((T_YEAR_FROM * 365 )+T_DAYS_FROM) AND ((T_YEAR_TO * 365)+T_DAYS_TO) AND T_GENDER = T03001.T_GENDER),'')||'  '||(SELECT T_UM FROM T13011 WHERE T_WS_CODE = T13016.T_WS_CODE AND T13011.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE) T_RANGE  
                                    FROM T13015, T13016,  T13018 , T02049,T13004, T03001   
                                    WHERE T13015.T_REQUEST_NO = T13016.T_REQUEST_NO  
                                    AND T13016.T_REQUEST_NO = T13018.T_REQUEST_NO   
                                    AND T13016.T_WS_CODE = T13018.T_WS_CODE   
                                    AND T03001.T_PAT_NO = T13015.T_PAT_NO   
                                    AND T13015.T_REFERRAL_CODE = T02049.T_REFERRAL_CODE   
                                    AND T13016.T_WS_CODE = T13004.T_WS_CODE   
                                    AND T13015.T_REQUEST_NO = '{reqno}'  
                                    AND T13016.T_WS_CODE = '{workstation}'  
                                    AND T13018.T_CLOSE_FLAG IS NOT NULL  
                                    ORDER BY T_RESULT_VALUE");

            return ReportQuery($@"SELECT T13015.T_PAT_NO, SYSDEV.T13_GET_LAB_NO(T13018.T_REQUEST_NO,T13018.T_WS_CODE ) T_LAB_NO
                                , SYSDEV.T13_GET_SPECIMEN(T13018.T_REQUEST_NO,T13018.T_WS_CODE) T_SPECIMEN_NAME
                                , SYSDEV.T13_GET_RECEIVED_DT(T13018.T_REQUEST_NO,T13018.T_WS_CODE) T_RECEIVED_DATE
                                , T13011.T_LANG2_NAME T_ANALYSIS_NAME, T13015.T_REQUEST_NO, T13015.T_LAB_REF_NO REF_HOSP_NO, T13015.T_REQUEST_DATE
                                , T13015.T_REQUEST_TIME, T13015.T_SPEC_NO, T03001.T_BIRTH_DATE  
                                , T03001.T_FIRST_LANG2_NAME || ' ' || T03001.T_FATHER_LANG2_NAME || ' ' || T03001.T_GFATHER_LANG2_NAME || ' ' || T03001.T_FAMILY_LANG2_NAME T_PAT_NAME
                                , NVL((select T02006.T_LANG2_NAME  from T02006 WHERE  T03001.T_GENDER = T02006.T_SEX_CODE ),'') T_GENDER 
                                , NVL((SELECT DISTINCT T02003.T_LANG2_NAME FROM T02003 WHERE T03001.T_NTNLTY_CODE = T02003.T_NTNLTY_CODE), '') T_NATIONALITY  
                                , T02049.T_LANG2_NAME T_LOCATION, T13004.T_LANG2_NAME || ' Department' T_LOCATION_NAME, T13018.T_NOTES, T13018.T_UPD_DATE  
                                , nvl((SELECT DISTINCT T13006.T_LANG2_NAME FROM T13006 WHERE T13006.T_RESULT_CODE = T13018.T_RESULT_VALUE),T13018.T_RESULT_VALUE)T_RESULT_VALUE  
                                , SYSDEV.T13_GET_NR_RANGE(T13018.T_REQUEST_NO,T13018.T_WS_CODE,T13018.T_ANALYSIS_CODE ,T13018.T_ANALYZER_ID,T13011.T_RESULT_TYPE) T_RANGE 
                                FROM T13015, T03001,  T13018 , T02049  ,T13011 ,t13004
                                WHERE T13015.T_REQUEST_NO = T13018.T_REQUEST_NO
                                AND T03001.T_PAT_NO = T13015.T_PAT_NO  
                                AND T13015.T_REFERRAL_CODE = T02049.T_REFERRAL_CODE  
                                AND T13018.T_WS_CODE = T13004.T_WS_CODE 
                                AND T13015.T_REQUEST_NO = '{reqno}' 
                                AND T13018.T_WS_CODE = '{workstation}'
                                AND T13011.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE  
                                AND T13011.T_WS_CODE = T13018.T_WS_CODE 
                                AND T13018.T_CLOSE_FLAG IS NOT NULL 
                                AND ( T13018.T_RESULT_VALUE IS NOT NULL OR T13018.T_NOTES IS NOT NULL) 
                                ORDER BY T13011.T_LANG2_NAME ");
        }
       **/
        public DataTable GetUN(string reqno)
        {
                return ReportQuery($@"SELECT T01009.T_USER_NAME FROM T01009 
                                    WHERE T01009.T_EMP_CODE = (SELECT MAX(T13018.T_UPD_USER) FROM T13018 
                                                                WHERE T13018.T_REQUEST_NO = '{reqno}')");
        }

        public DataTable GetAllR13053(string lang, string reqno) 
        {
            return ReportQuery($@"SELECT DISTINCT T13015.T_REQUEST_NO, T13015.T_PAT_NO, T03003.T_BIRTH_DATE
                                , T03003.T_FIRST_LANG{lang}_NAME || ' ' || T03003.T_FATHER_LANG{lang}_NAME || ' ' || T03003.T_GFATHER_LANG{lang}_NAME || ' ' || T03003.T_FAMILY_LANG{lang}_NAME AS T_PAT_NAME
                                , T02006.T_LANG{lang}_NAME GENDER, T02003.T_LANG{lang}_NAME NATIONALITY, T02049.T_LANG{lang}_NAME LOCATION, T13015.T_EXTERNAL_FLAG, T13015.T_SPEC_NO REFERRAL_NO
                                , T_LAB_REF_NO REFERRAL_HOSP_NO, T13015.T_LOCATION_CODE, T13017.T_LAB_NO T_LAB_NO, T13017.T_RECEIVED_DATE
                                , DECODE(T13016.T_PREG_YN,'1','a') LAB_PREG_YN
                                , DECODE(T13016.T_CONT_YN,'1','a') LAB_CONT_YN
                                , DECODE(T13016.T_HRT_YN,'1','a') LAB_HRT_YN
                                , DECODE(T13016.T_POST_PART_YN,'1','a') LAB_POST_PART_YN
                                , DECODE(T13016.T_POST_MENO_YN,'1','a') LAB_POST_MENO_YN
                                , DECODE(T13016.T_CHEM_IRRA_YN,'1','a') LAB_CHEM_IRRA_YN
                                , DECODE(T13016.T_IUCD_YN,'1','a') LAB_IUCD_YN
                                , DECODE(T13016.T_VAGIN_YN,'1','a') LAB_VAGIN_YN
                                , DECODE(T13016.T_ABNO_BLE_YN,'1','a') LAB_ABNO_BLE_YN
                                , TO_CHAR(T13047.T_CLOSE_DATE,'DD/MM/YYYY') ||' ' || HIJRAH(T13047.T_CLOSE_DATE) || ' H' COMPLETE_DATE
                                , LTRIM(RTRIM(T02029.T_NAME_GIVEN)) || ' ' || LTRIM(RTRIM(T02029.T_NAME_FATHER)) || ' ' || LTRIM(RTRIM(T02029.T_NAME_FAMILY)) || ' ' || LTRIM(RTRIM(T02029.T_NAME_GFATHER)) PATHOLOGIST 
                                FROM T13015,T13017,T13016,T03003,T02003,T02006,T02049,T13047,T02029  
                                WHERE T13015.T_REQUEST_NO = T13017.T_REQUEST_NO AND T13015.T_REQUEST_NO = T13016.T_REQUEST_NO 
                                AND T13017.T_REQUEST_NO = T13016.T_REQUEST_NO AND T13015.T_PAT_NO = T03003.T_TMP_PAT_NO  
                                AND T03003.T_NTNLTY_CODE = T02003.T_NTNLTY_CODE AND T03003.T_GENDER = T02006.T_SEX_CODE 
                                AND T13015.T_REFERRAL_CODE = T02049.T_REFERRAL_CODE AND T13017.T_LAB_NO IS NOT NULL  
                                AND T13047.T_CLOSE_FLAG IS NOT NULL AND T13047.T_REQUEST_NO = T13015.T_REQUEST_NO
                                AND T02029.T_EMP_NO = T13047.T_PATH_CODE AND T13017.T_ANALYSIS_CODE='50001' 
                                AND T13016.T_ANALYSIS_CODE='50001' 
                                AND T13015.T_REQUEST_NO = '{reqno}'");
        }

        public DataTable GetAllR13053Sub(string reqno) 
        {
            return ReportQuery($@"SELECT DECODE(T_ADEN_END_YN ,'1','a') T_ADEN_END_YN, 
                                DECODE(T_ADEN_ENM_YN,'1','a') T_ADEN_ENM_YN, DECODE(T_ADEN_EXT_YN,'1','a') T_ADEN_EXT_YN, 
                                DECODE(T_AGC_END_YN ,'1','a') T_AGC_END_YN, DECODE(T_AGC_ENM_YN,'1','a') T_AGC_ENM_YN, 
                                DECODE(T_AGC_GLA_YN,'1','a') T_AGC_GLA_YN, T_COMMENTS LAB_COMMENTS, T_PATH_CODE LAB_PATH_CODE, 
                                DECODE(T_REC_ROU_YN,'1','a') REC_ROU, DECODE(T_AGC_GLA_YN,'1','a') T_AGC_GLA_YN,
                                DECODE(T_REC_REP_YN ,'1','a') T_REC_REP_YN, DECODE(T_REC_END_YN,'1','a') T_REC_END_YN, 
                                DECODE(T_REC_DC_YN ,'1','a') T_REC_DC_YN, T_REC_MONTH LAB_REC_MONTH, 
                                DECODE(T_REC_COLP_YN,'1','a') T_REC_COLP_YN, DECODE(T_SM_UNSTAT_YN,'1','a') T_SM_UNSTAT_YN, 
                                DECODE(T_SM_NEG_YN,'1','a') T_SM_NEG_YN, DECODE(T_SM_LSIL_YN,'1','a') T_SM_LSIL_YN, 
                                DECODE(T_SM_HSIL_YN,'1','a') T_SM_HSIL_YN, DECODE(T_SM_SCC_YN,'1','a') T_SM_SCC_YN,  
                                DECODE(T_SM_ASCUS_YN,'1','a') T_SM_ASCUS_YN, DECODE(T_SM_ASCH_YN,'1','a') T_SM_ASCH_YN, 
                                DECODE(T_SM_ENDCELL_YN,'1','a') T_SM_ENDCELL_YN, DECODE(T_SM_AGC_YN,'1','a') T_SM_AGC_YN, 
                                DECODE(T_SM_AIS_YN,'1','a') T_SM_AIS_YN, DECODE(T_SM_ADEN_YN,'1','a') T_SM_ADEN_YN, 
                                DECODE(T_ORG_CAN_YN,'1','a') T_ORG_CAN_YN, DECODE(T_ORG_HPV_YN,'1','a') T_ORG_HPV_YN, 
                                DECODE(T_ORG_HSV_YN,'1','a') T_ORG_HSV_YN, DECODE(T_ORG_ACTI_YN ,'1','a') T_ORG_ACTI_YN,  
                                DECODE(T_ORG_TV_YN,'1','a') T_ORG_TV_YN, DECODE(T_ORG_SHIFT_YN,'1','a') T_ORG_SHIFT_YN, 
                                DECODE(T_OTH_ATR_YN,'1','a') OTHER1, DECODE(T_OTH_CEL_YN,'1','a') OTHER2, 
                                DECODE(T_OTH_KER_YN,'1','a') OTHER3, T_REQUEST_NO REQUEST_NO 
                                FROM T13047 WHERE T_CLOSE_FLAG IS NOT NULL AND T_REQUEST_NO = '{reqno}");
        }

        public DataTable GetAllR13001(string lang, string reqno, string labno)
        {
            return ReportQuery($@"SELECT DISTINCT T13015.T_PAT_NO, T13015.T_REQUEST_NO, T13015.T_LAB_REF_NO REF_HOSP_NO, T13015.T_SPEC_NO REF_NO  
                                , T03003.T_FIRST_LANG{lang}_NAME || ' ' || T03003.T_FATHER_LANG{lang}_NAME || ' ' || T03003.T_GFATHER_LANG{lang}_NAME || ' ' || T03003.T_FAMILY_LANG{lang}_NAME T_PAT_NAME 
                                , T03003.T_BIRTH_DATE 
                                , NVL((select T02006.T_LANG{lang}_NAME from T02006 where T03003.T_GENDER = T02006.T_SEX_CODE),'') T_GENDER 
                                , NVL((select T02003.T_LANG{lang}_NAME from t02003 where  T03003.T_NTNLTY_CODE = T02003.T_NTNLTY_CODE),'') T_NATIONALITY 
                                , T02049.T_LANG{lang}_NAME T_LOCATION  
                                , T13017.T_LAB_NO CENTRAL_LAB_NO, HIJRAH(T13017.T_RECEIVED_DATE) T_RECEIVED_DATE_H  
                                , NVL((SELECT T_LANG{lang}_NAME FROM  T13005 WHERE T13005.T_SPECIMEN_CODE = T13017.T_SPECIMEN_CODE),'')T_NATURE_OP 
                                , NVL((SELECT LTRIM(RTRIM(T02029.T_NAME_GIVEN)) ||' '|| LTRIM(RTRIM(T02029.T_NAME_FAMILY)) FROM T02029 WHERE T02029.T_EMP_NO = T13015.T_DOC_CODE  ),'') T_PHYISCIAN 
                                , T13016.T_COMMENT_LINE SAMPLE_FOR_EXAM, T13015.T_CLINIC_DATA, T13015.T_COMMENT_LINE CLINICIAL_DIAGNOSIS 
                                FROM T13015, T13016, T13017, T03003, T02049 
                                WHERE T13015.T_REQUEST_NO = T13016.T_REQUEST_NO 
                                AND T13016.T_REQUEST_NO = T13017.T_REQUEST_NO 
                                AND T03003.T_TMP_PAT_NO = T13015.T_PAT_NO 
                                AND T13015.T_REFERRAL_CODE = T02049.T_REFERRAL_CODE 
                                AND T13017.T_REQUEST_NO = '{reqno}' 
                                AND T13017.T_LAB_NO = '{labno}' 
                                AND T13017.T_WS_CODE = '05'");
        }

        public DataTable GetAllR13001Sub(string reqno, string labno)
        {
            return ReportQuery($@"SELECT T_REQ_NO REQUEST_NO, T_GROSS GROSS_DSCRIP, T_MICROSCOPY MICRO, T_FINAL_DIAGNOSIS DIAGNOSIS,  
                                NVL(T_TOPO_CODE1, ' ') T_TOPO_CODE1, NVL(T_DISEASE_CODE1, ' ') T_DISEASE_CODE1, 
                                NVL(T_TOPO_CODE2, ' ') T_TOPO_CODE2, 
                                NVL(T_DISEASE_CODE2, ' ') T_DISEASE_CODE2,  
                                NVL(T_TOPO_CODE3, ' ') T_TOPO_CODE3, NVL(T_DISEASE_CODE3, ' ') T_DISEASE_CODE3, T_ADDEN ADDENDUM,  
                                NVL(T_ENTRY_DATE,T_UPD_DATE)T_ENTRY_DATE, 
                                (SELECT MAX( T_USER_NAME) FROM T01009 WHERE T_EMP_CODE = NVL(T13001.T_UPD_USER,T13001.T_ENTRY_USER)) AMEND_USER,  
                                (SELECT LTRIM(RTRIM(T_NAME_GIVEN)) ||' '|| LTRIM(RTRIM(T_NAME_FAMILY )) FROM T02029 WHERE T_EMP_NO = T13001.T_PATHOLOGIST) T_PATHOLOGIST,
                                T_CLOSE_DATE,T_CLOSE_FLAG ,T_IMMUN_CHEMISTRY 
                                FROM T13001 
                                WHERE T_CLOSE_FLAG IS NOT NULL AND T_LAB_NO ='{labno}'  
                                AND T_REQ_NO = '{reqno}'");
        }

        public DataTable GetAllR13031(string lang, string reqno, string labno)
        {
            return ReportQuery($@"SELECT T13031.T_ENTRY_DATE, NVL(T13031.T_UPD_USER, T13031.T_ENTRY_USER) UPD_USER, T01009.T_USER_NAME
                                , T13015.T_PAT_NO, T13015.T_REQUEST_NO, T03003.T_FIRST_LANG{lang}_NAME || ' ' || T03003.T_FATHER_LANG{lang}_NAME || ' ' || T03003.T_GFATHER_LANG{lang}_NAME || ' ' || T03003.T_FAMILY_LANG{lang}_NAME AS T_PAT_NAME
                                , T03003.T_BIRTH_DATE, T02006.T_LANG{lang}_NAME GENDER, T13015.T_REQUEST_DATE, T13015.T_REQUEST_TIME, T02049.T_LANG{lang}_NAME AS LOCATION, T13015.T_COMMENT_LINE
                                , DECODE(T13015.T_PAT_TYPE,'1','InPatient','2','Out patient','3','Emergency','4','New Born','5','Day Case',NULL) AS PAT_TYPE, T13031.T_SKIN_SLIT
                                , T13031.T_NASAL_SMEAR, T13031.T_OTHER_SITE, T13031.T_OTHER_RESULT, T13031.T_REMARKS, T13031.T_LAB_NO, T13015.T_LAB_REF_NO 
                                FROM T13015, T13031, T01009, T03003, T02006, T02049 
                                WHERE T13015.T_REQUEST_NO = T13031.T_REQUEST_NO 
                                AND NVL(T13031.T_UPD_USER, T13031.T_ENTRY_USER) = T01009.T_EMP_CODE 
                                AND T13015.T_PAT_NO = T03003.T_TMP_PAT_NO 
                                AND T03003.T_GENDER = T02006.T_SEX_CODE 
                                AND T13015.T_REFERRAL_CODE = T02049.T_REFERRAL_CODE 
                                AND T13015.T_REQUEST_NO = NVL('{reqno}', T13015.T_REQUEST_NO) 
                                AND T13031.T_LAB_NO = '{labno}'");
        }

        public DataTable GetRD(string reqno, string labno)
        {
            return ReportQuery($@"SELECT MAX(T_SPECIMEN_NO),MAX(T_RECEIVED_DATE),MAX(T_RECEIVED_TIME) FROM T13017 
                                  WHERE T_REQUEST_NO = '{reqno}' AND T_LAB_NO = '{labno}'");
        }

        public DataTable GetSP(string labno)
        {
            return ReportQuery($@"SELECT T13017.T_SPECIMEN_CODE, T_LANG2_NAME FROM T13017, T13005 
                                  WHERE T13005.T_SPECIMEN_CODE = T13017.T_SPECIMEN_CODE AND T13017.T_LAB_NO = '{labno}'");
        }

        public DataTable GetAllR13068(string lang, string reqno, string labno)
        {
            return ReportQuery($@"SELECT HIJRAAH(T_ENTRY_DATE)T_ENTRY_DATE,HIJRAAH(T_UPD_DATE)T_UPD_DATE,T_UPD_USER
                                , T_ENTRY_USER, T_REQUEST_NO, T_SERIAL_NO, T_SAMPLE, T_COLONY_CNT, T_REMARK
                                , T_MPN_COLI, T_MPN_FEC, T_FEC_STR, T_SAL_SHIG, T_SOURCE, T_LAB_NO, T_NO_SAMPLE
                                , T_RECEIVED_NO, T_RECEIVED_DATE_H
                                ,(SELECT T_FIRST_LANG{lang}_NAME || ' ' || T_FATHER_LANG{lang}_NAME || ' ' || T_GFATHER_LANG{lang}_NAME ||' ' || T_FAMILY_LANG{lang}_NAME  
                                  FROM T03003 WHERE T_TMP_PAT_NO IN (SELECT T_PAT_NO FROM T13015 WHERE T_REQUEST_NO = '{reqno}')) T_SENDER
                                ,(SELECT MAX(T_LANG{lang}_NAME) FROM T02049 WHERE T_REFERRAL_CODE IN (SELECT MAX(T_REFERRAL_CODE) FROM T13015 WHERE T_REQUEST_NO ='{reqno}'))AS REFERALCODE   
                                ,(SELECT T01009.T_USER_NAME FROM T01009 WHERE T01009.T_EMP_CODE=T13068.T_ENTRY_USER)username  
                                ,(SELECT T01009.T_USER_NAME FROM T01009 WHERE T01009.T_EMP_CODE=T13068.T_UPD_USER)UPD_USER_N  
                                ,(SELECT TO_CHAR(MAX(TRUNC(T13015.T_REQUEST_DATE)))FROM T13015 WHERE T13015.T_REQUEST_NO = T13068.T_REQUEST_NO) AS REQUEST_DATE  
                                FROM T13068
                                WHERE T_REQUEST_NO = '{reqno}' AND T_LAB_NO = '{labno}'");
        }

        public DataTable GetAllR13069(string lang, string reqno, string labno)
        {
            return ReportQuery($@"SELECT HIJRAAH(T_ENTRY_DATE)T_ENTRY_DATE,HIJRAAH(T_UPD_DATE)T_UPD_DATE,T_UPD_USER, T_ENTRY_USER,  T_LAB_NO, T_REQUEST_NO
                                , T_SERIAL_NO, T_SAMPLE, T_TOTAL_CNT, T_REMARK, T_STAP_ENT, T_SAL_SHIG, T_COLI_FAE, T_SPO_ORG, T_FUNGUS, T_ABAEROB
                                ,(SELECT T_FIRST_LANG{lang}_NAME || ' ' || T_FATHER_LANG{lang}_NAME || ' ' || T_GFATHER_LANG{lang}_NAME || ' ' || T_FAMILY_LANG{lang}_NAME  
                                  FROM T03003 WHERE T_TMP_PAT_NO IN (SELECT T_PAT_NO FROM T13015 WHERE T_REQUEST_NO = '{reqno}')) AS T_SENDER 
                                ,(SELECT MAX(T_LANG{lang}_NAME) FROM T02049 WHERE T_REFERRAL_CODE IN (SELECT MAX(T_REFERRAL_CODE) FROM T13015 WHERE T_REQUEST_NO ='{reqno}')) AS REFERALCODE   
                                ,(SELECT T01009.T_USER_NAME FROM T01009 WHERE T01009.T_EMP_CODE=T13069.T_ENTRY_USER) AS USERNAME  
                                ,(SELECT T01009.T_USER_NAME FROM T01009 WHERE T01009.T_EMP_CODE=T13069.T_UPD_USER) AS UPD_USER_N  
                                ,(SELECT TO_CHAR(MAX(TRUNC(T13015.T_REQUEST_DATE))) FROM T13015 WHERE T13015.T_REQUEST_NO = T13069.T_REQUEST_NO)AS REQUEST_DATE  
                                FROM T13069 
                                WHERE T_REQUEST_NO = '{reqno}' AND T_LAB_NO = '{labno}'");
        }

        public DataTable GetAllR131401(string lang, string reqno)
        {
            return ReportQuery($@"SELECT T13015.T_REQUEST_NO,T13015.T_PAT_NO,T_BIRTH_DATE,T13015.T_REQUEST_DATE
                                , SUBSTR(T_REQUEST_TIME,1,2) || ':' || SUBSTR(T_REQUEST_TIME,3,2) REQUEST_TIME 
                                , LTRIM(RTRIM(NVL(T_FIRST_LANG{lang}_NAME,'')))||' '||LTRIM(RTRIM(NVL(T_FATHER_LANG{lang}_NAME,'')))||' '||LTRIM(RTRIM(NVL(T_GFATHER_LANG{lang}_NAME,''))) PAT_NAME
                                , TRUNC(MONTHS_BETWEEN(SYSDATE, T03003.T_BIRTH_DATE) / 12, 0) ||' YRS '||TRUNC(MOD(MONTHS_BETWEEN(SYSDATE, T03003.T_BIRTH_DATE), 12), 0)||' MOS' AGE
                                , (SELECT DISTINCT T_LANG{lang}_NAME FROM T02006 WHERE T_SEX_CODE=T03003.T_GENDER) GENDER
                                , (SELECT DISTINCT T_LANG{lang}_NAME  FROM T02003 WHERE T_NTNLTY_CODE=T03003.T_NTNLTY_CODE)NATIONALITY
                                , NVL(NVL(( SELECT MAX(T_LANG{lang}_NAME) FROM T02049 WHERE T_REFERRAL_CODE IN (SELECT MAX(T_REFERRAL_CODE) FROM T13015 WHERE T_REQUEST_NO ='{reqno}' ))
                                , (SELECT  DISTINCT T_LANG{lang}_NAME FROM T02042 WHERE T_LOC_CODE = T13015.T_LOCATION_CODE))
                                , (SELECT  DISTINCT T_CLINIC_NAME_LANG{lang} FROM T07001 WHERE T_CLINIC_CODE=T13015.T_LOCATION_CODE)) AS LOCATION
                                , (SELECT DISTINCT T_LANG{lang}_NAME FROM T13005 WHERE T_SPECIMEN_CODE=T13017.T_SPECIMEN_CODE AND T13015.T_REQUEST_NO=T13017.T_REQUEST_NO )SPECIMEN_NAME
                                , (SELECT T_USER_NAME FROM T01009 WHERE T_EMP_CODE = NVL(t13018.t_upd_user, t13018.t_entry_user)) UPD_USER 
                                , DOCTOR_NAME(T_DOC_CODE) DOCTOR_NAME,T13011.T_LANG{lang}_NAME ANALYSIS,T13015.T_SPEC_NO
                                , T13015.T_LAB_REF_NO,T13015.T_COMMENT_LINE,T13017.T_LAB_NO,T13017.T_SPECIMEN_CODE,T13017.T_RECEIVED_DATE 
                                , T13017.T_RECEIVED_TIME,T13017.T_STAFF_CODE, T13017.T_COLLECTION_DATE,T13017.T_COLLECTION_TIME
                                , T13011.T_RESULT_TYPE RESULT_TYPE,T13011.T_UM UNITS,T13018.T_RESULT_VALUE,T13006.T_LANG{lang}_NAME RESULT, T13018.T_ANTIBIOTIC_SENS 
                                , T13018.T_ANTIBIOTIC_RESI,T13018.T_NOTES
                                FROM T13015,T03003,T13017,T13011,T13018 ,T13006 
                                WHERE T13015.T_REQUEST_NO = '{reqno}'
                                AND T13015.T_PAT_NO=T03003.T_TMP_PAT_NO AND T13015.T_REQUEST_NO=T13017.T_REQUEST_NO 
                                AND T13015.T_REQUEST_NO = T13018.T_REQUEST_NO
                                AND T13011.T_WS_CODE = T13018.T_WS_CODE
                                AND T13006.T_RESULT_CODE = T13018.T_RESULT_VALUE
                                AND T13011.T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE
                                AND T13018.T_CLOSE_FLAG IS NOT NULL ORDER BY T13018.T_ANALYSIS_CODE"); 
        }

        public DataTable GetAllR13101(string lang, string reqno, string labno) 
        {
            return ReportQuery($@"SELECT DISTINCT T13017.T_WS_CODE, T13039.T_REQUEST_NO, T13017.T_LAB_NO, T13015.T_PAT_NO
                                , T03003.T_FIRST_LANG{lang}_NAME || ' ' || T03003.T_FATHER_LANG{lang}_NAME || ' ' || T03003.T_GFATHER_LANG{lang}_NAME || ' ' || T03003.T_FAMILY_LANG{lang}_NAME T_PAT_NAME
                                , T03003.T_BIRTH_DATE, T02006.T_LANG{lang}_NAME T_GENDER, T13015.T_LAB_REF_NO REF_HOSP_NO, T13015.T_SPEC_NO, T13005.T_LANG{lang}_NAME SPECIMEN_NAME
                                , DECODE(T13015.T_PAT_TYPE,'1','InPatient','2','Out patient','3','Emergency','4','New Born','5','Day Case',NULL) AS PAT_TYPE
                                , HIJRAH(T13017.T_RECEIVED_DATE) LAB_RECEIVED_DATE_H, T13017.T_RECEIVED_DATE, T02049.T_LANG{lang}_NAME AS LOCATION
                                , NVL(T13039.T_REMARKS,T_TECH_COMMENT) COMMENTS , T13015.T_COMMENT_LINE DIAGNOSIS
                                , DECODE (ORG1.T_LANG{lang}_NAME,NULL,ORG1.T_LANG{lang}_NAME,'Organisms Found :'||ORG1.T_LANG{lang}_NAME)  T_ORGANISM_1, ORG2.T_LANG{lang}_NAME T_ORGANISM_2, ORG3.T_LANG{lang}_NAME T_ORGANISM_3
                                , T13032.T_LANG{lang}_NAME RESULT, NVL(T13039.T_REMARKS, T13039.T_TECH_COMMENT) REMARKS
                                , NVL(T13039.T_ENTRY_DATE,T13015.T_ENTRY_DATE) REPORT_DATE, NVL(T13039.T_UPD_DATE, T13039.T_ENTRY_DATE) T_UPD_DATE  
                                , REP.T_USER_NAME REPORTED_BY, REV.T_USER_NAME REVIEWED_BY  
                                FROM T13039, T13017, T13015, T03003, T02006, T13005, T02049, T13032, T01009 REP, T01009 REV, T13033 ORG1, T13033 ORG2, T13033 ORG3  
                                WHERE T13015.T_REQUEST_NO = T13039.T_REQUEST_NO   
                                AND T13017.T_REQUEST_NO = T13015.T_REQUEST_NO   
                                AND T13017.T_LAB_NO = T13039.T_LAB_NO   
                                AND T13015.T_PAT_NO = T03003.T_TMP_PAT_NO   
                                AND T03003.T_GENDER = T02006.T_SEX_CODE(+)  
                                AND T13005.T_SPECIMEN_CODE(+) = T13017.T_SPECIMEN_CODE   
                                AND T13015.T_REFERRAL_CODE = T02049.T_REFERRAL_CODE(+)  
                                AND T13032.T_NEG_RESULT_CODE(+) = T13039.T_NEG_RESULT_CODE   
                                AND REP.T_EMP_CODE(+) = T13039.T_ENTRY_USER  
                                AND REP.T_EMP_CODE(+) = T13039.T_ENTRY_USER  
                                AND REV.T_EMP_CODE(+) = T13039.T_UPD_USER  
                                AND T13039.T_ORGANISM_CODE1 = ORG1.T_ORGANISM_CODE(+)   
                                AND T13039.T_ORGANISM_CODE2 = ORG2.T_ORGANISM_CODE(+)  
                                AND T13039.T_ORGANISM_CODE3 = ORG3.T_ORGANISM_CODE(+)  
                                AND T13017.T_LAB_NO = NVL('{labno}', 'B32005156')   
                                AND T13017.T_REQUEST_NO = NVL('{reqno}', '0002185434')");
        }

        public DataTable GetAllR13101Sub(string lang, string labno) 
        {
            return ReportQuery($@"SELECT T13040.T_NVL_RING RING, T13040.T_ANTIBIOTIC_CODE, T13040.T_RESULT_RING1 RING,  T13040.T_ANTIBIOTIC_FLAG1 F1
                                , T13040.T_ANTIBIOTIC_FLAG2 F2, T13040.T_ANTIBIOTIC_FLAG3 F3, T13034.T_LANG{lang}_NAME T_ANTIBIOTIC_NAME  
                                FROM T13040, T13034  
                                WHERE T13040.T_ANTIBIOTIC_CODE = T13034.T_ANTIBIOTIC_CODE   
                                AND T_NVL_RING ='1' 
                                AND (T13040.T_ANTIBIOTIC_FLAG1 in ('i', 's', 'r') 
                                OR T13040.T_ANTIBIOTIC_FLAG2 IN ('i', 's', 'r') 
                                OR T13040.T_ANTIBIOTIC_FLAG3 IN ('i', 's', 'r'))   
                                AND T_LAB_NO= '{labno}'   
                                UNION ALL   
                                SELECT T13040.T_NVL_RING RING, T13040.T_ANTIBIOTIC_CODE, T13040.T_RESULT_RING2 RING, T13040.T_ANTIBIOTIC_FLAG1 F1
                                , T13040.T_ANTIBIOTIC_FLAG2 F2, T13040.T_ANTIBIOTIC_FLAG3 F3, T13034.T_LANG{lang}_NAME T_ANTIBIOTIC_NAME  
                                FROM T13040, T13034  
                                WHERE T13040.T_ANTIBIOTIC_CODE = T13034.T_ANTIBIOTIC_CODE   
                                AND T_NVL_RING ='2' 
                                AND (T13040.T_ANTIBIOTIC_FLAG1 IN ('i', 's', 'r') 
                                OR T13040.T_ANTIBIOTIC_FLAG2 IN ('i', 's', 'r') 
                                OR T13040.T_ANTIBIOTIC_FLAG3 IN ('i', 's', 'r'))   
                                AND T_LAB_NO= '{labno}'  
                                UNION ALL  
                                SELECT T13040.T_NVL_RING RING, T13040.T_ANTIBIOTIC_CODE, T13040.T_RESULT_RING3 RING, T13040.T_ANTIBIOTIC_FLAG1 F1
                                , T13040.T_ANTIBIOTIC_FLAG2 F2, T13040.T_ANTIBIOTIC_FLAG3 F3, T13034.T_LANG{lang}_NAME T_ANTIBIOTIC_NAME  
                                FROM T13040, T13034  
                                WHERE T13040.T_ANTIBIOTIC_CODE = T13034.T_ANTIBIOTIC_CODE   
                                AND T_NVL_RING ='3' 
                                AND (T13040.T_ANTIBIOTIC_FLAG1 IN ('i', 's', 'r') 
                                OR T13040.T_ANTIBIOTIC_FLAG2 IN ('i', 's', 'r') 
                                OR T13040.T_ANTIBIOTIC_FLAG3 IN ('i', 's', 'r'))   
                                AND T_LAB_NO= '{labno}'");
        }

        public DataTable GetAllR13030(string lang, string reqno, string labno) 
        {
            return ReportQuery($@"SELECT T13015.T_PAT_NO,T03003.T_FIRST_LANG{lang}_NAME || ' ' || T03003.T_FATHER_LANG{lang}_NAME || ' ' || T03003.T_GFATHER_LANG{lang}_NAME || ' ' || T03003.T_FAMILY_LANG{lang}_NAME T_PAT_NAME   
                                , T02006.T_LANG{lang}_NAME T_GENDER, T13030.T_SPECIMEN_CODE T_SPEC_NO, T13005.T_LANG{lang}_NAME SPECIMEN_NAME 
                                , T13015.T_LAB_REF_NO REF_HOSP_NO, T02049.T_LANG{lang}_NAME AS LOCATION, T13015.T_REQUEST_NO,T13015.T_REQUEST_DATE, T13015.T_PRIORITY_CODE
                                , T13015.T_LOCATION_CODE, T13015.T_PAT_TYPE, T13015.T_STATUS_CODE, T13015.T_EXTERNAL_FLAG, T13030.T_ENTRY_DATE
                                , NVL(T13030.T_UPD_USER,T13030.T_ENTRY_USER) UPD_USER, HIJRAH(T13015.T_REQUEST_DATE) REQUEST_DATE
                                ,(SELECT T_USER_NAME FROM T01009 WHERE T_EMP_CODE=NVL(T13030.T_UPD_USER,T13030.T_ENTRY_USER)) AS ReportedBy
                                , DECODE(T13015.T_PAT_TYPE,'1','InPatient','2','Out patient','3','Emergency','4','New Born','5','Day Case',null ) PAT_TYPE 
                                , TRUNC(MONTHS_BETWEEN(SYSDATE, T03003.T_BIRTH_DATE) / 12, 0) ||' Yrs '||TRUNC(MOD(MONTHS_BETWEEN(SYSDATE, T03003.T_BIRTH_DATE), 12), 0)||' Mos' AGE
                                , T13030.T_LAB_NO, T13030.T_RESULT,T13030.T_POTENCY_REAGENT || ' mIU/mL'  T_POTENCY_REAGENT, T13030.T_REMARKS   
                                FROM T13015,T13030,T03003,T02006,T13005,T02049 
                                WHERE T13015.T_REQUEST_NO = T13030.T_REQUEST_NO   
                                AND T13015.T_PAT_NO = T03003.T_TMP_PAT_NO    
                                AND T03003.T_GENDER = T02006.T_SEX_CODE(+)   
                                AND T13005.T_SPECIMEN_CODE = T13030.T_SPECIMEN_CODE  
                                AND T13015.T_REFERRAL_CODE = T02049.T_REFERRAL_CODE  
                                AND T13030.T_LAB_NO = NVL('{labno}', 'B32005156')  
                                AND T13015.T_REQUEST_NO = NVL('{reqno}', '0002185434')");
        }

        public DataTable GetAllR13102(string lang, string reqno, string labno)
        {
            return ReportQuery($@"SELECT DISTINCT T13017.T_WS_CODE, T13039.T_REQUEST_NO, NVL(T13039.T_LAB_NO
                                , NVL(T13017.T_LAB_NO,'NOT PROVIDED')) T_LAB_NO, T13015.T_PAT_NO  
                                , T03003.T_FIRST_LANG{lang}_NAME || ' ' || T03003.T_FATHER_LANG{lang}_NAME || ' ' || T03003.T_GFATHER_LANG{lang}_NAME || ' ' || T03003.T_FAMILY_LANG{lang}_NAME T_PAT_NAME  
                                , T03003.T_BIRTH_DATE, T02006.T_LANG{lang}_NAME T_GENDER, T13015.T_LAB_REF_NO REF_HOSP_NO, T13015.T_SPEC_NO, T13005.T_LANG{lang}_NAME SPECIMEN_NAME  
                                , DECODE(T13015.T_PAT_TYPE,'1','InPatient','2','Out patient','3','Emergency','4','New Born','5','Day Case',NULL) AS PAT_TYPE  
                                , HIJRAH(T13017.T_RECEIVED_DATE) LAB_RECEIVED_DATE_H, T13017.T_RECEIVED_DATE, T02049.T_LANG{lang}_NAME AS LOCATION  
                                , T13017.T_COMMENT_LINE COMMENTS, T13015.T_COMMENT_LINE DIAGNOSIS, T13032.T_LANG{lang}_NAME RESULT
                                , NVL(T13039.T_REMARKS, T13039.T_TECH_COMMENT) REMARKS  
                                , REP.T_USER_NAME REPORTED_BY, T13039.T_ENTRY_DATE REPORT_DATE, REV.T_USER_NAME REVIEWED_BY, T13039.T_UPD_DATE  
                                FROM T13015, T13017, T13039, T03003, T02006, T13005, T02049, T13032, T01009 REP, T01009 REV  
                                WHERE T13015.T_REQUEST_NO = T13017.T_REQUEST_NO  
                                AND T13015.T_REQUEST_NO = T13039.T_REQUEST_NO   
                                AND T13015.T_PAT_NO = T03003.T_TMP_PAT_NO  
                                AND T03003.T_GENDER = T02006.T_SEX_CODE(+) 
                                AND T13005.T_SPECIMEN_CODE = T13017.T_SPECIMEN_CODE  
                                AND T13015.T_REFERRAL_CODE = T02049.T_REFERRAL_CODE  
                                AND T13032.T_NEG_RESULT_CODE(+) = T13039.T_NEG_RESULT_CODE  
                                AND REP.T_EMP_CODE(+) =  T13039.T_ENTRY_USER  
                                AND REV.T_EMP_CODE(+) =  T13039.T_UPD_USER  
                                AND T13039.T_LAB_NO = NVL('{labno}', 'B32005156')  
                                AND T13015.T_REQUEST_NO = NVL('{reqno}', '0002185434')");
        }

        public DataTable GetHeader()
        {
            return ReportQuery($@"SELECT * FROM T01028 WHERE T_SITE_CODE = 00003");
        }
    }
}
