using System.Data;

namespace ConnectKsmcDAL.Report
{
    public class R07046DAL : CommonDAL
    {
        public DataTable GetHeaderData(string siteCode)
        {
            return ReportQuery($@"select T_SITE_CODE , T_COUNTRY_LANG1_NAME , T_COUNTRY_LANG2_NAME , T_MIN_LANG1_NAME , T_MIN_LANG2_NAME , T_SITE_LANG1_NAME , T_SITE_LANG2_NAME , T_LOGO_ID , T_LOGO_NAME , T_LOGO , t_lcence_no from t01028 where t_site_code in(select t_site_code from t01001)");
        }
        public DataTable GetBodyData(string apptNo)
        {
            return ReportQuery($@"SELECT t07003.T_PAT_NO , T_CLINIC_NAME_LANG1, T_FIRST_LANG1_NAME ||' ' ||T_FATHER_LANG1_NAME ||' ' ||T_GFATHER_LANG1_NAME ||' ' ||T_FAMILY_LANG1_NAME pat_name, TRUNC(MONTHS_BETWEEN(sysdate, t_birth_date) / 12, 0) YY, t03001.T_NTNLTY_CODE , t_arrival_date T_ARRIVAL_DATE_ENG, hijraah(t_arrival_date) T_ARRIVAL_DATE_HIJ,hijraah(sysdate) T_PRINT_DATE_HIJ, (CASE WHEN t_arrival_time is not null THEN CASE WHEN t_arrival_time>='2400' THEN CASE WHEN SUBSTR( TO_CHAR(TO_DATE(LPAD((t_arrival_time-1200), 4, '0'), 'HH24MI'), 'HH:MI AM') , -2, 2)='AM' THEN SUBSTR(t_arrival_time, 1,2)||':'||SUBSTR(t_arrival_time, 3,2)||' '|| (select t_lang1_name from days where t_day='1') ELSE SUBSTR(t_arrival_time, 1,2)||':'||SUBSTR(t_arrival_time, 3,2)||' '|| (select t_lang1_name from days where t_day='2') END ELSE CASE WHEN SUBSTR( TO_CHAR(TO_DATE(LPAD((t_arrival_time-1200), 4, '0'), 'HH24MI'), 'HH:MI AM') , -2, 2)='AM' THEN SUBSTR(t_arrival_time, 1,2)||':'||SUBSTR(t_arrival_time, 3,2)||' '|| (select t_lang1_name from days where t_day='1') ELSE SUBSTR(t_arrival_time, 1,2)||':'||SUBSTR(t_arrival_time, 3,2)||' '|| (select t_lang1_name from days where t_day='2') END END ELSE '' end ) T_TIME, t_arrival_time , '!' || T03001.T_PAT_NO || '!' BARCODE_NO, T_NLTY.T_LANG2_NAME T_NATIONALITY, (SELECT T_LANG1_NAME FROM T07046 where t_flag='1')DES, t07003.T_appt_no FROM t07003, t07001, t03001 JOIN T02003 t_nlty on t03001.T_NTNLTY_CODE =t_nlty.T_NTNLTY_CODE WHERE t07001.t_clinic_code=t07003.t_clinic_code AND t07003.t_pat_no =t03001.t_pat_no and t07003.T_appt_no ='{apptNo}'");
        }
    }
}
