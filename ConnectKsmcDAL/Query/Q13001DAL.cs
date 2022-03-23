using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ConnectKsmcDAL.Query
{
    public class Q13001DAL : CommonDAL
    {
        public dynamic GetMinMaxDate(string patNo, string episodeNo, string dateOn, string btnType)
        {
            var query = "";
            if (btnType == "next")
            {
                query = $@"SELECT to_char(MIN(T_ENTRY_DATE),'dd/MM/yyyy') EntryDate FROM V13001_LOG	WHERE T_PAT_NO='{patNo}' 
                        AND T_EPISODE_NO=NVL({episodeNo},T_EPISODE_NO) AND T_ENTRY_DATE > to_date('{dateOn}','dd/MM/yyyy')";
            }
            else
            {
                query = $@"SELECT to_char(MAX(T_ENTRY_DATE),'dd/MM/yyyy') EntryDate FROM V13001_LOG WHERE T_PAT_NO='{patNo}' 
                        AND T_EPISODE_NO=NVL({episodeNo},T_EPISODE_NO) AND T_ENTRY_DATE < to_date('{dateOn}','dd/MM/yyyy')";
            }
            return QuerySingle<dynamic>(query);
        }
        public dynamic GetPatInfo(string Language, string T_PAT_NO, string T_SITE_CODE)
        {
            var query = $@"SELECT PAT_NAME(T03001.T_PAT_NO, '2')  T_PAT_NAME,PAT_NAME(T03001.T_PAT_NO, '1')  T_PAT_NAME_ARB,T02006.T_LANG{Language}_NAME T_GENDER, TRUNC(MONTHS_BETWEEN(SYSDATE, T_BIRTH_DATE ) / 12, 0) YEARS, 
                TRUNC(MOD(MONTHS_BETWEEN(SYSDATE, T_BIRTH_DATE), 12), 0) MONTHS,TRUNC(MOD(MONTHS_BETWEEN(SYSDATE, T_BIRTH_DATE), 30), 0) DAYS, T02003.T_LANG{Language}_NAME T_NATIONALITY 
                FROM T02003, T02006, T03001 WHERE T_GENDER = T_SEX_CODE(+) AND TO_NUMBER(T02003.T_NTNLTY_CODE) = TO_NUMBER(T03001.T_NTNLTY_CODE) AND T_PAT_NO = '{T_PAT_NO}'";
            return QuerySingle<dynamic>(query);
        }
        public dynamic GetRequestInfo(string patNo, string lab, string userId, string roleCode)
        {
            string qry = $@"SELECT T_WS_CODE,T_MULTI_LOC FROM T13019 WHERE T_EMP_CODE='{userId}'";
            IEnumerable<dynamic> user_ws = QueryList<dynamic>(qry);
            IEnumerable<dynamic> user_cnt = QueryList<dynamic>($@"SELECT COUNT(*) NO_OF_CNT FROM T13122 WHERE T_EMP_CODE ='{userId}'");
            if (roleCode == "0013" || roleCode == "0225" || roleCode == "001")
            {
                if (string.IsNullOrEmpty(user_ws.ToList()[0].T_WS_CODE))
                {
                    if (user_ws.ToList()[0].T_WS_CODE == "94")
                    {
                        var ws1 = "10"; var ws2 = "17"; var ws3 = "02";
                        qry = $@" AND T_CLOSE_FLAG IS NOT NULL AND T_WS_CODE in ('{ws1}','{ws2}','{ws3}') AND T_REQUEST_NO IN(SELECT T_REQUEST_NO FROM T13015 WHERE T_PAT_NO='{patNo}') GROUP BY T_WS_CODE, T_REQUEST_NO, T_UPD_DATE";
                    }
                    else if (user_ws.ToList()[0].T_WS_CODE == "99")
                    {
                        qry = $@" AND T_CLOSE_FLAG IS NOT NULL AND T_REQUEST_NO IN(SELECT T_REQUEST_NO FROM T13015 WHERE T_PAT_NO='{patNo}') GROUP BY T_WS_CODE,T_REQUEST_NO,T_UPD_DATE";
                    }
                    else if (user_ws.ToList()[0].T_WS_CODE == "97")
                    {
                        var ws1 = "06"; var ws2 = "17"; var ws3 = "02"; var WS4 = "24"; var ws5 = "10";
                        qry = $@" AND T_REQUEST_NO IN  (SELECT T_REQUEST_NO FROM T13015 WHERE T_PAT_NO='{patNo}') AND T_CLOSE_FLAG IS NOT NULL AND T_WS_CODE in (LTRIM(RTRIM('{ws1}')), LTRIM(RTRIM('{ws2}')), LTRIM(RTRIM('{ws3}')), LTRIM(RTRIM('{WS4}')), LTRIM(RTRIM('{ws5}')))
                            GROUP BY T_WS_CODE,T_REQUEST_NO,T_UPD_DATE";
                    }
                    else if (user_ws.ToList()[0].T_WS_CODE == "98")
                    {
                        var ws1 = "02"; var ws2 = "17"; var ws3 = "26";
                        qry = $@" AND T_REQUEST_NO IN  (SELECT T_REQUEST_NO FROM T13015 WHERE T_PAT_NO = '{patNo}') AND T_CLOSE_FLAG IS NOT NULL AND T_WS_CODE in (LTRIM(RTRIM('{ws1}')), LTRIM(RTRIM('{ws2}')), LTRIM(RTRIM('{ws3}')))
                                GROUP BY T_WS_CODE,T_REQUEST_NO,T_UPD_DATE";
                    }
                    else if (!string.IsNullOrEmpty(user_ws.ToList()[0].T_MULTI_LOC))
                    {
                        qry = $@" AND T_REQUEST_NO IN  (SELECT T_REQUEST_NO FROM T13015 WHERE T_PAT_NO = '{patNo}') AND T_CLOSE_FLAG IS NOT NULL AND T_WS_CODE in (select ltrim(rtrim(t_ws_code)) from V131958 WHERE T_EMP_CODE ='{userId}' )
                              GROUP BY T_WS_CODE,T_REQUEST_NO,T_UPD_DATE";
                    }
                    else
                    {
                        if (user_ws.ToList()[0].T_WS_CODE > 0)// FOR MARRIAGE RESULT
                        {
                            var ws1 = "13"; var ws2 = "17"; var ws3 = "11";
                            qry = $@" AND T_CLOSE_FLAG IS NOT NULL AND T_WS_CODE in ('{ws1}','{ws2}','{ws3}') AND T_REQUEST_NO IN(SELECT T_REQUEST_NO FROM T13015 WHERE T_PAT_NO = '{patNo}')
                                      GROUP BY T_WS_CODE, T_REQUEST_NO, T_UPD_DATE";
                        }
                        else
                        {
                            qry = $@" AND T_CLOSE_FLAG IS NOT NULL AND T_WS_CODE = '{user_ws.ToList()[0].T_WS_CODE}' AND  T_REQUEST_NO IN(SELECT T_REQUEST_NO FROM T13015 WHERE T_PAT_NO = '{patNo}')
                                      GROUP BY T_WS_CODE, T_REQUEST_NO, T_UPD_DATE";
                        }
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(lab))
                {
                    qry = $@" AND T_CLOSE_FLAG IS NOT NULL AND T_WS_CODE='{lab}' AND T_REQUEST_NO IN(SELECT T_REQUEST_NO FROM T13015 WHERE T_PAT_NO='{patNo}') GROUP BY T_WS_CODE,T_REQUEST_NO,T_UPD_DATE";
                }
                else
                {
                    qry = $@" AND T_CLOSE_FLAG IS NOT NULL AND T_REQUEST_NO IN(SELECT T_REQUEST_NO FROM T13015 WHERE T_PAT_NO='{patNo}') GROUP BY T_WS_CODE,T_REQUEST_NO,T_UPD_DATE";

                }
            }
            return QueryList<dynamic>($@"SELECT T_REQUEST_NO,T_WS_CODE,(SELECT T_LANG2_NAME FROM T13004	WHERE T_WS_CODE = T13018.T_WS_CODE) T_WS_NAME,
                                        --(SELECT T_LAB_NO FROM T13017 WHERE T_REQUEST_NO = T13018.T_REQUEST_NO) T_LAB_NO,
	                                    (SELECT T_REQUEST_DATE FROM T13015 WHERE T_REQUEST_NO = T13018.T_REQUEST_NO) T_REQUEST_DATE FROM T13018 WHERE 1=1 {qry} 
                                        ORDER BY T_UPD_DATE DESC");
        }
        public dynamic GetRequestDetail(string reqNo,string ws_code,string lang)
        {
            var qry = $@"select t18.t_ws_code,t18.t_analyzer_id,t18.t_result_value,t18.t_prev_result,t18.t_analysis_code,t11.t_lang{lang}_name ANALISYS,DECODE(t11.t_result_type, 
                        '1', (SELECT T_NR_FROM ||'-'||T_NR_TO FROM T13014 WHERE T_ANALYZER_ID = NVL(T18.T_ANALYZER_ID,'02') 
                        AND T13014.T_WS_CODE = t18.T_WS_CODE AND T13014.T_ANALYSIS_CODE = T18.T_ANALYSIS_CODE), 
                        '2', (SELECT T_NR_FROM ||'-'||T_NR_TO FROM T13014 WHERE T_ANALYZER_ID = NVL(T18.T_ANALYZER_ID,'02') 
                        AND T13014.T_WS_CODE = t18.T_WS_CODE AND T13014.T_ANALYSIS_CODE = T18.T_ANALYSIS_CODE AND T13014.T_GENDER = T01.T_GENDER ), 
                        '3', (SELECT T_NR_FROM ||'-'||T_NR_TO FROM T13014 WHERE T_ANALYZER_ID = NVL(T18.T_ANALYZER_ID,'02') 
                        AND T_WS_CODE = t18.T_WS_CODE AND T_ANALYSIS_CODE = T18.T_ANALYSIS_CODE AND 
                        TO_NUMBER(NVL(TRUNC(MONTHS_BETWEEN(SYSDATE,T01.T_BIRTH_DATE)/12,0),0) * 365) + TRUNC(SYSDATE)-(ADD_MONTHS(T01.T_BIRTH_DATE, 
                        (TRUNC((MONTHS_BETWEEN(TRUNC(SYSDATE), T01.T_BIRTH_DATE))/12))*12+(MOD((MONTHS_BETWEEN(TRUNC(SYSDATE), T01.T_BIRTH_DATE)), 12)))) 
                        BETWEEN ((T_YEAR_FROM * 365 ) + T_DAYS_FROM) AND ((T_YEAR_TO * 365) + T_DAYS_TO)), 
                        '4', (SELECT T_NR_FROM ||'-'||T_NR_TO FROM T13014  WHERE T_ANALYZER_ID = NVL(T18.T_ANALYZER_ID,'02') 
                        AND T_WS_CODE = t18.T_WS_CODE AND T_ANALYSIS_CODE = T18.T_ANALYSIS_CODE AND 
                        TO_NUMBER(NVL(TRUNC(MONTHS_BETWEEN(SYSDATE,T01.T_BIRTH_DATE)/12,0),0) * 365) + TRUNC(SYSDATE)-(ADD_MONTHS(T01.T_BIRTH_DATE, 
                        (TRUNC((MONTHS_BETWEEN(TRUNC(SYSDATE), T01.T_BIRTH_DATE))/12))*12+(MOD((MONTHS_BETWEEN(TRUNC(SYSDATE), T01.T_BIRTH_DATE)), 12)))) 
                        BETWEEN ((T_YEAR_FROM * 365 ) + T_DAYS_FROM) AND ((T_YEAR_TO * 365) + T_DAYS_TO) AND T_GENDER = T01.T_GENDER), 
                        (SELECT DISTINCT T_NR_FROM ||'-'||T_NR_TO FROM T13014 WHERE T_ANALYZER_ID = NVL(T18.T_ANALYZER_ID,'05') AND T_WS_CODE=t18.T_WS_CODE AND 
                        T_ANALYSIS_CODE=T18.T_ANALYSIS_CODE))||' '||(SELECT T_UM FROM T13011 WHERE T_WS_CODE=T18.T_WS_CODE AND T_ANALYSIS_CODE=T18.T_ANALYSIS_CODE) T_RANGE,
                        T18.T_NOTES,T18.T_NOTES2 from T13018 t18,t13011 t11,T13015 T15,T03001 T01,T13004 T04 where t18.T_REQUEST_NO='{reqNo}' and t18.t_analysis_code=t11.t_analysis_code
                        and t18.t_ws_code=t11.t_ws_code and t18.t_ws_code=t04.t_ws_code and t04.t_ws_code='{ws_code}' and t11.t_active_flag is not null AND T18.T_REQUEST_NO=T15.T_REQUEST_NO(+) AND T01.T_PAT_NO=T15.T_PAT_NO ORDER BY t18.t_analysis_code";
            return QueryList<dynamic>(qry);
        }
        public DataSet GetRequestData(string reqNo,string labNo,string lang)
        {
            DataSet ds = new DataSet();
            var qry1 = $@"SELECT T15.T_REQUEST_NO,T15.T_REQUEST_DATE,T15.T_REQUEST_TIME REQUEST_TIME,T15.T_PAT_NO,T15.T_EXTERNAL_FLAG,T15.T_PAT_TYPE,T15.T_LOCATION_CODE,T15.T_CLINIC_DATA,
                        DOCTOR_NAME(T15.T_DOC_CODE) DOCTOR_NAME,T15.T_SPEC_NO REF_HOS_NO,T15.T_COMMENT_LINE CLINICIAL_DIAGNOSIS,T15.T_DOC_CODE,T16.T_WS_CODE,T16.T_ANALYSIS_CODE,
                        T17.T_SPECIMEN_CODE,T17.T_LAB_NO LAB_CODE_NO,T17.T_RECEIVED_DATE,T17.T_RECEIVED_TIME,T17.T_COLLECTION_DATE COOLECTION_DATE,T17.T_COLLECTION_TIME COLL_TIME
                        FROM T13015 T15,T13016 T16,T13017 T17 WHERE T15.T_REQUEST_NO=T16.T_REQUEST_NO AND T16.T_REQUEST_NO=T17.T_REQUEST_NO AND T15.T_REQUEST_NO=T17.T_REQUEST_NO
                        T16.T_ANALYSIS_CODE = T17.T_ANALYSIS_CODE AND T16.T_WS_CODE   = T17.T_WS_CODE AND T16.T_WS_CODE IN ('05','17') 
                        AND T16.T_REQUEST_NO='{reqNo}' AND T17.T_WS_CODE IN ('05','17') AND T17.T_LAB_NO='{labNo}' AND T17.T_REQUEST_NO='{reqNo}' AND T15.T_REQUEST_NO='{reqNo}'";
            DataTable dt1= ReportQuery(qry1);
            var qry2 = $@"SELECT T_REQ_NO REQUEST_NO,T_MICROSCOPY MICRO,T_FROZEN FROZEN,T_GROSS,T_GROSS_ARB,T_FINAL_DIAGNOSIS  DIAGNOSIS,T_FINAL_DIAG_HEADING DIAG_HEADING,
                        T_TOPO_CODE1,T_DISEASE_CODE1,T_TOPO_CODE2,T_DISEASE_CODE2,T_TOPO_CODE3,T_DISEASE_CODE3,T_IMMUN_CHEMISTRY,T_ADDEN ADDENDUM FROM T13001 
                        WHERE T_CLOSE_FLAG IS NOT NULL AND T_LAB_NO='{labNo}' AND T_REQ_NO='{reqNo}'";
            DataTable dt2= ReportQuery(qry2);
            var qry3 = $@"SELECT T_SITE_CODE,T_COUNTRY_LANG1_NAME,T_COUNTRY_LANG2_NAME,T_MIN_LANG1_NAME,T_MIN_LANG2_NAME,T_SITE_LANG1_NAME,T_SITE_LANG2_NAME,T_LOGO_ID,
                        T_LOGO_NAME,T_LOGO,T_LCENCE_NO  FROM T01028 WHERE T_SITE_CODE ='00001' ";
            DataTable dt3= ReportQuery(qry3);
            ds.Tables.Add(dt1);
            ds.Tables.Add(dt2);
            ds.Tables.Add(dt3);
            return ds;
        }
        public dynamic GetReportID(string analysisCode,string wsCode)
        {
            var query = $@"SELECT T_REPORT_ID FROM t13062 WHERE T_EXTERNAL_FLAG IS NULL AND T_ANALYSIS_CODE ='{analysisCode}' AND NVL(T_WS_CODE,'{wsCode}') = '{wsCode}'";
            return QueryList<dynamic>(query);
        }

        public DataTable ReportHeader()
        {
            return ReportQuery($@"SELECT T_SITE_CODE , T_COUNTRY_LANG1_NAME ,T_COUNTRY_LANG2_NAME ,T_MIN_LANG1_NAME ,T_MIN_LANG2_NAME ,T_SITE_LANG1_NAME ,T_SITE_LANG2_NAME ,T_LOGO_ID ,T_LOGO_NAME, T_LOGO ,t_lcence_no from t01028 where t_site_code in(select t_site_code from t01001)");
        }
        public DataTable ReportQueryOne(string T_REQUEST_NO)
        {
            var Q1 = ReportQuery($@"SELECT M.*,'' T_RECEIVED_DATE,(select floor(months_between(trunc(sysdate), M.T_BIRTH_DATE)/12 ) || '???' from dual) AGE ,CASE WHEN M.T_GENDER = 'M' THEN 'ذكر' WHEN M.T_GENDER ='F' THEN 'أنثى' ELSE (SELECT t_lang1_name  FROM t02006  WHERE t_sex_code = M.T_GENDER) END GENDER_NAME,(SELECT t_lang1_name FROM t02003 WHERE t_ntnlty_code = M.T_NTNLTY_CODE) NATIONALITY_NAME FROM ( SELECT t15.T_LOCATION_CODE, t15.T_PAT_NO, t15.T_PAT_TYPE, t15.T_PRIORITY_CODE, t15.T_REQUEST_DATE, t15.T_REQUEST_NO REQUEST_NO, t15.T_CLINIC_DATA, t15.T_EXTERNAL_FLAG, T_SPEC_NO, (select case when ct > 0 then (SELECT T_LANG2_NAME FROM T02042 WHERE T_LOC_CODE = t15.T_LOCATION_CODE) else (select T_CLINIC_NAME_LANG2 from T07001 where t_clinic_code = t15.T_LOCATION_CODE) end ddd from (select count(*) ct FROM T02042 WHERE T_LOC_CODE = t15.T_LOCATION_CODE)) LOCATION, decode(t15.T_EXTERNAL_FLAG,null,(SELECT LTRIM(RTRIM(T_FIRST_LANG1_NAME)) ||' '||LTRIM(RTRIM(T_FATHER_LANG1_NAME)) ||' '||LTRIM(RTRIM( T_GFATHER_LANG1_NAME))||' '|| LTRIM(RTRIM(T_FAMILY_LANG1_NAME)) FROM T03001 WHERE T_PAT_NO = t15.T_PAT_NO),(SELECT LTRIM(RTRIM(T_FIRST_LANG1_NAME)) ||' '||LTRIM(RTRIM(T_FATHER_LANG1_NAME)) ||' '||LTRIM(RTRIM( T_GFATHER_LANG1_NAME))||' '|| LTRIM(RTRIM(T_FAMILY_LANG1_NAME)) FROM T03003 WHERE T_TMP_PAT_NO = t15.T_PAT_NO )) PAT_NAME, decode(t15.T_EXTERNAL_FLAG,null,(select LTRIM(RTRIM(T_FIRST_LANG2_NAME)) ||' '|| LTRIM(RTRIM(T_FATHER_LANG2_NAME)) ||' '|| LTRIM(RTRIM( T_GFATHER_LANG2_NAME))||' '||LTRIM(RTRIM(T_FAMILY_LANG2_NAME)) from T03001 WHERE T_PAT_NO = t15.T_PAT_NO),( SELECT LTRIM(RTRIM(T_FIRST_LANG2_NAME)) ||' '||LTRIM(RTRIM(T_FATHER_LANG2_NAME)) ||' '||LTRIM(RTRIM( T_GFATHER_LANG2_NAME))||' '|| LTRIM(RTRIM(T_FAMILY_LANG2_NAME)) FROM T03003 WHERE T_TMP_PAT_NO = t15.T_PAT_NO )) PAT_NAME_eng, decode(t15.T_EXTERNAL_FLAG, null,(select T_NTNLTY_CODE from T03001 WHERE T_PAT_NO = t15.T_PAT_NO) ,(select nvl(T_NTNLTY_CODE,'99') from T03003 WHERE T_TMP_PAT_NO = t15.T_PAT_NO )) T_NTNLTY_CODE, decode(t15.T_EXTERNAL_FLAG,null,(select T_GENDER from T03001 WHERE T_PAT_NO = t15.T_PAT_NO),(select T_GENDER from T03003 WHERE T_TMP_PAT_NO = t15.T_PAT_NO)) T_GENDER, decode(t15.T_EXTERNAL_FLAG,null,(select T_BIRTH_DATE from T03001 WHERE T_PAT_NO = t15.T_PAT_NO),(select T_BIRTH_DATE from T03003 WHERE T_TMP_PAT_NO = t15.T_PAT_NO)) T_BIRTH_DATE, decode(t15.T_EXTERNAL_FLAG,null,(select NVL(T_X_RMC_CHRTNO,T_X_HOSP_NO) from T03001 WHERE T_PAT_NO = t15.T_PAT_NO),null) HOSP_NO, decode(t15.T_EXTERNAL_FLAG,null,(select T_NTNLTY_ID from T03001 WHERE T_PAT_NO = t15.T_PAT_NO),(select T_NTNLTY_ID from T03003 WHERE T_TMP_PAT_NO = t15.T_PAT_NO)) T_NTNLTY_ID , decode(t15.T_EXTERNAL_FLAG,null,(select T_MOBILE_NO from T03001 WHERE T_PAT_NO = t15.T_PAT_NO),(select nvl(T_MOBILE_NO,'') from T03003 WHERE T_TMP_PAT_NO = t15.T_PAT_NO)) T_MOBILE_NO , decode(t15.T_EXTERNAL_FLAG,null,(select T_ADDRESS1 from T03001 WHERE T_PAT_NO = t15.T_PAT_NO),(select nvl(T_ADDRESS1,' ') from T03003 WHERE T_TMP_PAT_NO = t15.T_PAT_NO)) T_ADDRESS1,t15.T_LAB_NO FROM T13015 t15 WHERE t15.T_REQUEST_NO = '{T_REQUEST_NO}' GROUP BY t15.T_LOCATION_CODE, t15.T_PAT_NO, t15.T_PAT_TYPE, t15.T_PRIORITY_CODE, t15.T_REQUEST_DATE, t15.T_REQUEST_NO, t15.T_CLINIC_DATA, t15.T_EXTERNAL_FLAG, t15.T_SPEC_NO,t15.T_LAB_NO) M");
            var LabNumber = ReportQuery($@"SELECT MAX(t_lab_no) T_LAB_NO,MAX(t_received_date) rdate ,decode(MAX(t_received_date),'',to_char(MAX(t_received_date),'dd-MON-yy'), HIJRA_DATE(MAX(t_received_date))) t_received_date FROM t13017 WHERE t_lab_no     IS NOT NULL  AND t_analysis_code ='17047'  AND t_ws_code='17'  AND t_request_no= '{T_REQUEST_NO}' group by t_lab_no,t_received_date");
            if (LabNumber.Rows.Count < 1)
            {
                LabNumber = ReportQuery($@"select t_lab_no lab_no	from t13017	where t_analysis_code ='17047'	and t_ws_code ='17' and t_request_no = '{T_REQUEST_NO}'");
            }
            Q1.Rows[0]["T_RECEIVED_DATE"] = LabNumber.Rows[0]["T_RECEIVED_DATE"];
            Q1.Rows[0]["T_LAB_NO"] = LabNumber.Rows[0]["T_LAB_NO"];
            return Q1;
        }

        public DataTable ReportQuerySecond(string T_REQUEST_NO)
        {
            return ReportQuery($@"SELECT y.*,CASE WHEN y.RES_VALUE is NULL THEN y.T_RESULT_VALUE ELSE y.RES_VALUE END T_RES_VALUE, CASE WHEN y.LAB_RESULT_TYPE = '1' THEN (select DISTINCT T_NR_FROM||' - ' || T_NR_TO normal_to from t13014 where T_ANALYSIS_CODE = y.T_ANALYSIS_CODE and t_ws_code = NVL(y.t_ws_code ,t_ws_code )) WHEN y.LAB_RESULT_TYPE = '2' THEN (select DISTINCT T_NR_FROM||' - ' || T_NR_TO from t13014 where T_ANALYSIS_CODE = y.T_ANALYSIS_CODE and t_ws_code = NVL(y.t_ws_code ,t_ws_code ) and t_gender = y.PRS_SEX_NUM1) WHEN y.LAB_RESULT_TYPE = '3' THEN (select DISTINCT T_NR_FROM||' - ' || T_NR_TO from t13014 where T_ANALYSIS_CODE = y.T_ANALYSIS_CODE and t_ws_code = NVL(y.T_WS_CODE ,t_ws_code ) and to_number(nvl(y.CF_AGE1,0) * 365) + to_number(nvl(y.CF_DAYS,0)) BETWEEN ((t_year_from * 365 ) + t_days_from) AND ((t_year_to * 365 ) + t_days_to )) WHEN y.LAB_RESULT_TYPE = '4' THEN (select DISTINCT T_NR_FROM||' - ' || T_NR_TO from t13014 where T_ANALYSIS_CODE = y.T_ANALYSIS_CODE and t_ws_code = NVL(y.T_WS_CODE ,T_WS_CODE ) and to_number(nvl(y.CF_AGE1,0) * 365) + to_number(nvl(y.CF_DAYS,0)) BETWEEN ((t_year_from * 365 ) + t_days_from) AND ((t_year_to * 365 ) + t_days_to ) and ROWNUM=1) ELSE '' END normal_to FROM (SELECT q.*, (SELECT TRUNC(SYSDATE)- q.CUR_DAY FROM dual) CF_DAYS from (SELECT p.*,(SELECT ADD_MONTHS(p.CF_B_DATE,P.AGE_YEARS*12+p.AGE_MONTH) from dual) CUR_DAY FROM (SELECT k.*, (SELECT t_lang2_name || ' ' || k.CF_UNIT_MEAS FROM t13006 WHERE t_RESULT_CODE = k.T_RESULT_VALUE) RES_VALUE, (SELECT TRUNC(MONTHS_BETWEEN(sysdate,k.CF_B_DATE)/12,0) FROM dual) CF_AGE1, (SELECT  cast(MONTHS_BETWEEN(TRUNC(SYSDATE),k.CF_B_DATE)/12 as NUMBER(10,3)) FROM dual) AGE_YEARS,  cast ((SELECT mod(MONTHS_BETWEEN(TRUNC(SYSDATE),k.CF_B_DATE)/12,12) FROM dual) as NUMBER(10,3)) AGE_MONTH FROM (SELECT j.*, (SELECT t_lang2_name FROM t13011 WHERE t_ANALYSIS_CODE = j.t_ANALYSIS_CODE AND t_WS_CODE = j.T_WS_CODE) ANALYSIS, (SELECT t_UM FROM t13011 WHERE T_ANALYSIS_CODE = j.T_ANALYSIS_CODE AND T_WS_CODE = '10') CF_UNIT_MEAS, (select t_result_type from t13011 where t_analysis_code = j.T_ANALYSIS_CODE and t_ws_code = j.T_WS_CODE) LAB_RESULT_TYPE, CASE WHEN j.EXTERNAL_FLAG is null THEN (SELECT t_gender FROM t03001 WHERE T_PAT_NO = j.PRS_PAT_NO) ELSE (SELECT t_gender FROM t03003 WHERE T_TMP_PAT_NO = j.PRS_PAT_NO) END PRS_SEX_NUM1, CASE WHEN j.EXTERNAL_FLAG is NULL THEN (SELECT t_birth_date FROM t03001 WHERE T_PAT_NO = j.PRS_PAT_NO) ELSE (SELECT t_birth_date FROM t03003 WHERE T_TMP_PAT_NO = j.PRS_PAT_NO) END CF_B_DATE FROM (SELECT T13015.T_PAT_NO PRS_PAT_NO, T13015.T_EXTERNAL_FLAG EXTERNAL_FLAG, T13018.T_WS_CODE, T13018.T_ANALYSIS_CODE, T13018.T_RESULT_VALUE, T13018.T_NOTES, t13018.T_UPD_USER, (SELECT t_user_name FROM t01009 WHERE t_emp_code = t13018.T_UPD_USER) user_name, t13018.T_UPD_DATE, HIJRAAH(t13018.T_UPD_DATE) hijri, T13064.T_GROUP_ANALYSIS FROM T13018, T13064, T13015 WHERE T13018.T_ANALYSIS_CODE = T13064.T_ANALYSIS_CODE AND T_CLOSE_FLAG IS NOT NULL AND T_RESULT_VALUE IS NOT NULL AND T13018.T_ANALYSIS_CODE IN ('17011', '17012', '17013', '17014', '17015', '17016', '17017', '17022', '17025', '17071', '17072', '17073', '17074', '17075', '17076', '17077', '17078', '17079', '17080', '17081', '17082', '17083', '17084', '13005', '13016', '13024', '13025', '13017', '13006', '13008', '13009', '13012', '13050', '17085', '17086', '17087', '17088', '17089', '17194', '17195') AND T13018.T_REQUEST_NO = '{T_REQUEST_NO}' AND T13015.T_REQUEST_NO = T13018.T_REQUEST_NO ORDER BY T13018.T_ANALYSIS_CODE ASC) j) k) p) q) y");
        }

        public DataTable ReportQuerySecondT13166A(string T_REQUEST_NO)
        {
            return ReportQuery($@"SELECT v.*, CASE WHEN v.LAB_RESULT_TYPE = '1' THEN (SELECT DISTINCT T_NR_FROM || ' - ' || T_NR_TO FROM t13014 WHERE T_ANALYSIS_CODE = v.T_ANALYSIS_CODE AND t_ws_code = NVL(v.t_ws_code, t_ws_code)) WHEN v.LAB_RESULT_TYPE = '2' THEN (SELECT DISTINCT T_NR_FROM || ' - ' || T_NR_TO FROM t13014 WHERE T_ANALYSIS_CODE = v.T_ANALYSIS_CODE AND t_ws_code = NVL(v.t_ws_code, t_ws_code) AND t_gender = v.PRS_SEX_NUM1) WHEN v.LAB_RESULT_TYPE = '3' THEN (SELECT DISTINCT T_NR_FROM || ' - ' || T_NR_TO FROM t13014 WHERE T_ANALYSIS_CODE = v.T_ANALYSIS_CODE AND t_ws_code = NVL(v.T_WS_CODE, t_ws_code) AND to_number(nvl(v.CF_AGE1, 0) * 365) + to_number(nvl(v.CF_DAYS, 0)) BETWEEN ((t_year_from * 365) + t_days_from) AND ((t_year_to * 365) + t_days_to)) WHEN v.LAB_RESULT_TYPE = '4' THEN (SELECT DISTINCT T_NR_FROM || ' - ' || T_NR_TO FROM t13014 WHERE T_ANALYSIS_CODE = v.T_ANALYSIS_CODE AND t_ws_code = NVL(v.T_WS_CODE, T_WS_CODE) AND to_number(nvl(v.CF_AGE1, 0) * 365) + to_number(nvl(v.CF_DAYS, 0)) BETWEEN ((t_year_from * 365) + t_days_from) AND ((t_year_to * 365) + t_days_to) AND ROWNUM = 1) ELSE '' END normal_range FROM ( SELECT h.*, (SELECT TRUNC(SYSDATE) - h.CUR_DAY FROM dual) CF_DAYS FROM ( SELECT c.*, (SELECT ADD_MONTHS(c.CF_B_DATE, c.YEARS * 12 + c.MON) FROM dual) CUR_DAY FROM ( SELECT m.* , CASE WHEN m.CF_RESULT_VAL IS NULL THEN m.T_RESULT_VALUE || ' ' || m.CF_UNIT_MEAS ELSE m.CF_RESULT_VAL END CF_RESULT_VALUE, CASE WHEN m.LAB_ANALYSIS_DESC IS NOT NULL THEN m.LAB_ANALYSIS_DESC ELSE m.CF_RESULT_VAL END CF_ANALYSIS_RESULT, (SELECT trunc(MONTHS_BETWEEN(TRUNC(SYSDATE),m.CF_B_DATE) / 12) FROM dual) YEARS , cast((SELECT mod(MONTHS_BETWEEN(TRUNC(SYSDATE), m.CF_B_DATE) / 12, 12) FROM dual) AS NUMBER(10, 3)) MON, (SELECT TRUNC(MONTHS_BETWEEN(sysdate, m.CF_B_DATE) / 12, 0) FROM dual) CF_AGE1 FROM ( SELECT w.*, (SELECT t_lang2_name ||' ' || w.CF_UNIT_MEAS FROM t13006 WHERE t_RESULT_CODE = w.T_RESULT_VALUE ) CF_RESULT_VAL, CASE WHEN w.EXTERNAL_FLAG IS NULL THEN ( SELECT t_gender FROM t03001 WHERE T_PAT_NO = w.PRS_PAT_NO) ELSE (SELECT t_gender FROM t03003 WHERE T_TMP_PAT_NO = w.PRS_PAT_NO) END PRS_SEX_NUM1, CASE WHEN w.EXTERNAL_FLAG IS NULL THEN (SELECT t_birth_date FROM t03001 WHERE T_PAT_NO = w.PRS_PAT_NO) ELSE (SELECT t_birth_date FROM t03003 WHERE T_TMP_PAT_NO = w.PRS_PAT_NO) END CF_B_DATE FROM ( SELECT T13015.T_PAT_NO PRS_PAT_NO , T13015.T_EXTERNAL_FLAG EXTERNAL_FLAG , T13018.T_WS_CODE, T13018.T_ANALYSIS_CODE, T13018.T_RESULT_VALUE, T13018.T_NOTES, (SELECT t_user_name FROM t01009 WHERE t_emp_code = t13018.T_UPD_USER) user_name, t13018.T_UPD_DATE, HIJRAAH(t13018.T_UPD_DATE) hijri, (SELECT t_lang2_name FROM t13011 WHERE t_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE AND t_WS_CODE = T13018.T_WS_CODE) LAB_ANALYSIS_DESC, (SELECT t_UM FROM t13011 WHERE T_ANALYSIS_CODE = T13018.T_ANALYSIS_CODE AND T_WS_CODE = '10') CF_UNIT_MEAS, (select t_result_type from t13011 where t_analysis_code = T13018.T_ANALYSIS_CODE and t_ws_code = T13018.T_WS_CODE) LAB_RESULT_TYPE FROM T13018,T13015 WHERE T13018.T_REQUEST_NO='{T_REQUEST_NO}' AND T_CLOSE_FLAG IS NOT NULL AND T_RESULT_VALUE IS NOT NULL AND T13018.T_ANALYSIS_CODE IN ('17011','17012','17013','17014', '17015','17016','17017','17022','17025', '17061','17071','17072','17073','17074','17075','17076','17077','17078', '17079','17080','17081','17082','17083','17084','17161','17162','17163','13005','13016','13024','13025','13017','13006','13008','13009','13012','13050','17085','17086','17087', '17088','17089','2816','1109','1110','13155','13179') AND T13015.T_REQUEST_NO = T13018.T_REQUEST_NO) w) m) c) h) v");
        }
    }
}