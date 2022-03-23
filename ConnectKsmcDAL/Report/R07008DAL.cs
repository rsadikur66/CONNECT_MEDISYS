using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ConnectKsmcDAL.Report
{
    public class R07008DAL : CommonDAL
    {
        public IEnumerable<dynamic> GetClinicName(string lang)
        {
            return QueryList<dynamic>($"SELECT T_CLINIC_CODE, T_CLINIC_NAME_LANG{lang} NAME FROM T07001 WHERE T_ACTIVE_FLAG='1' ORDER BY T_CLINIC_CODE");
        }
        public DataTable GetData(string lang, string datefrom, string dateto, string clinic)
        {
            var query = "";
            if (clinic == null || clinic == "")
            {
                query = $@"SELECT T07003.T_CLINIC_CODE,T07001.T_CLINIC_NAME_LANG{lang} CLINIC_NAME,T07003.T_CLINIC_DOC_CODE,DOCTOR_NAME(T07003.T_CLINIC_DOC_CODE) DOC_NAME,
                            T07003.T_REF_REQNO REF_REQ_NO,to_char(T07003.T_APPT_DATE,'dd/MM/yyyy') T_APPT_DATE,T07003.T_APPT_TIME,T07003.T_APPT_TYPE,T07006.T_LANG{lang}_NAME APPT_TYPE,
                            T07003.T_PAT_NO, PAT_NAME(T07003.T_PAT_NO,{lang}) PAT_NAME,PAT_YEAR(T07003.T_PAT_NO) YEARS, PAT_MONTH(T07003.T_PAT_NO) MONTHS, 
                            PAT_GENDER(T07003.T_PAT_NO, {lang}) PAT_GENDER FROM T07003,T07001,T02039,T02029,T07006
                            WHERE T07003.T_CLINIC_CODE = T07001.T_CLINIC_CODE(+)
                            AND T02039.T_DOC_CODE = T07003.T_CLINIC_DOC_CODE(+)
                            AND T02029.T_EMP_NO = T02039.T_DOC_CODE(+)
                            --AND T07061.T_PAT_NO(+) = T07003.T_PAT_NO
                            AND T07006.T_APPT_TYPE(+) = T07003.T_APPT_TYPE
                            AND T07003.T_PAT_NO IS NOT NULL
                            AND T07003.T_APPT_DATE BETWEEN TO_DATE('{datefrom}','dd/MM/yyyy') AND TO_DATE('{dateto}','dd/MM/yyyy')  order by T_APPT_DATE,T07003.T_APPT_TIME";
            }
            else
            {
                query = $@"SELECT T07003.T_CLINIC_CODE,T07001.T_CLINIC_NAME_LANG{lang} CLINIC_NAME,T07003.T_CLINIC_DOC_CODE,DOCTOR_NAME(T07003.T_CLINIC_DOC_CODE) DOC_NAME,
                            T07003.T_REF_REQNO REF_REQ_NO,to_char(T07003.T_APPT_DATE,'dd/MM/yyyy') T_APPT_DATE,T07003.T_APPT_TIME,T07003.T_APPT_TYPE,T07006.T_LANG{lang}_NAME APPT_TYPE,
                            T07003.T_PAT_NO, PAT_NAME(T07003.T_PAT_NO,{lang}) PAT_NAME,PAT_YEAR(T07003.T_PAT_NO) YEARS, PAT_MONTH(T07003.T_PAT_NO) MONTHS, 
                            PAT_GENDER(T07003.T_PAT_NO, {lang}) PAT_GENDER FROM T07003,T07001,T02039,T02029,T07006
                            WHERE T07003.T_CLINIC_CODE = T07001.T_CLINIC_CODE(+)
                            AND T02039.T_DOC_CODE = T07003.T_CLINIC_DOC_CODE(+)
                            AND T02029.T_EMP_NO = T02039.T_DOC_CODE(+)
                            --AND T07061.T_PAT_NO(+) = T07003.T_PAT_NO
                            AND T07006.T_APPT_TYPE(+) = T07003.T_APPT_TYPE
                            AND T07003.T_PAT_NO IS NOT NULL
                            AND T07003.T_APPT_DATE BETWEEN TO_DATE('{datefrom}','dd/MM/yyyy') AND TO_DATE('{dateto}','dd/MM/yyyy') 
                            AND T07003.T_CLINIC_CODE = '{clinic}'  order by T_APPT_DATE,T07003.T_APPT_TIME";
            }           
            return ReportQuery(query);
        }
        public IEnumerable<dynamic> GetAppointmentData(string lang, string datefrom, string dateto, string clinic)
        {
            var query = "";
            if (clinic == null || clinic == "")
            {
                query = $@"SELECT T07003.T_CLINIC_CODE,T07001.T_CLINIC_NAME_LANG{lang} CLINIC_NAME,T07003.T_CLINIC_DOC_CODE,DOCTOR_NAME(T07003.T_CLINIC_DOC_CODE) DOC_NAME,
                            T07003.T_REF_REQNO REF_REQ_NO,to_char(T07003.T_APPT_DATE,'dd/MM/yyyy') T_APPT_DATE,T07003.T_APPT_TIME,T07003.T_APPT_TYPE,T07006.T_LANG{lang}_NAME APPT_TYPE,
                            T07003.T_PAT_NO, PAT_NAME(T07003.T_PAT_NO,{lang}) PAT_NAME,PAT_YEAR(T07003.T_PAT_NO) YEARS, PAT_MONTH(T07003.T_PAT_NO) MONTHS, 
                            PAT_GENDER(T07003.T_PAT_NO, {lang}) PAT_GENDER FROM T07003,T07001,T02039,T02029,T07006
                            WHERE T07003.T_CLINIC_CODE = T07001.T_CLINIC_CODE(+)
                            AND T02039.T_DOC_CODE = T07003.T_CLINIC_DOC_CODE(+)
                            AND T02029.T_EMP_NO = T02039.T_DOC_CODE(+)
                            --AND T07061.T_PAT_NO(+) = T07003.T_PAT_NO
                            AND T07006.T_APPT_TYPE(+) = T07003.T_APPT_TYPE
                            AND T07003.T_PAT_NO IS NOT NULL
                            AND T07003.T_APPT_DATE BETWEEN nvl(TO_DATE('{datefrom}','dd/MM/yyyy'),T_APPT_DATE) AND nvl(TO_DATE('{dateto}','dd/MM/yyyy'),T_APPT_DATE) order by T_APPT_DATE,T07003.T_APPT_TIME";
            }
            else
            {
                query = $@"SELECT T07003.T_CLINIC_CODE,T07001.T_CLINIC_NAME_LANG{lang} CLINIC_NAME,T07003.T_CLINIC_DOC_CODE,DOCTOR_NAME(T07003.T_CLINIC_DOC_CODE) DOC_NAME,
                            T07003.T_REF_REQNO REF_REQ_NO,to_char(T07003.T_APPT_DATE,'dd/MM/yyyy') T_APPT_DATE,T07003.T_APPT_TIME,T07003.T_APPT_TYPE,T07006.T_LANG{lang}_NAME APPT_TYPE,
                            T07003.T_PAT_NO, PAT_NAME(T07003.T_PAT_NO,{lang}) PAT_NAME,PAT_YEAR(T07003.T_PAT_NO) YEARS, PAT_MONTH(T07003.T_PAT_NO) MONTHS, 
                            PAT_GENDER(T07003.T_PAT_NO, {lang}) PAT_GENDER FROM T07003,T07001,T02039,T02029,T07006
                            WHERE T07003.T_CLINIC_CODE = T07001.T_CLINIC_CODE(+)
                            AND T02039.T_DOC_CODE = T07003.T_CLINIC_DOC_CODE(+)
                            AND T02029.T_EMP_NO = T02039.T_DOC_CODE(+)
                            --AND T07061.T_PAT_NO(+) = T07003.T_PAT_NO
                            AND T07006.T_APPT_TYPE(+) = T07003.T_APPT_TYPE
                            AND T07003.T_PAT_NO IS NOT NULL
                            AND T07003.T_APPT_DATE BETWEEN TO_DATE(nvl('{datefrom}',T_APPT_DATE),'dd/MM/yyyy') AND TO_DATE(nvl('{dateto}',T_APPT_DATE),'dd/MM/yyyy')
                            AND T07003.T_CLINIC_CODE='{clinic}' order by T_APPT_DATE,T07003.T_APPT_TIME";
            }           
            return QueryList<dynamic>(query);
        }
    }
}
