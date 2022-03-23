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
    }
}