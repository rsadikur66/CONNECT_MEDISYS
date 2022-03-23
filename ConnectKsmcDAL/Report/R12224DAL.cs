using System.Data;

namespace ConnectKsmcDAL.Report
{
    public class R12224DAL : CommonDAL
    {
        public DataTable GetData(string siteCode, string rqtNo)
        {
            return ReportQuery($@"SELECT T_1.T_LANG2_NAME, T_1.VIRUS_CODE, T_1.T_UNIT_NO, (SELECT DISTINCT CASE WHEN T_VIOROLOGY_RESULT='1' THEN (select distinct CASE WHEN T_ANTIBODY_1 IN ('Pos' , 'POSITIVE' , 'positive','POS','pos') THEN 'Discard' ELSE 'Negative' END from t12019 where T_UNIT_NO=T_1.T_UNIT_NO) ELSE 'Discard' END FROM T12019 WHERE T_UNIT_NO=T_1.T_UNIT_NO) RESULT_1, ( CASE WHEN (select count(*) B from t12034 where t_virus_code='03' and t_unit_no=T_1.T_UNIT_NO and t_pos1_verify is not null) >0 THEN 'Discard' ELSE ( CASE WHEN( select t_result_val from t12218 where t_test_code='580' and t_unit_no=T_1.T_UNIT_NO) >=1 THEN (CASE WHEN (select count(*) J from t12218 where t_test_code='131' and t_unit_no=T_1.T_UNIT_NO)>0 THEN 'Discard' ELSE ' ' END ) ELSE 'Discard' END ) END ) RESULT_2 FROM ( SELECT T_LANG2_NAME,to_number(T_VIRUS_CODE) VIRUS_CODE,T12075.T_UNIT_NO FROM T12075,T12033 WHERE T_VIRUS_CODE IN ('01','02','03','04','05','06','07','08','09','13') and t12075.t_donation_date between '19-JAN-21' and '19-JAN-21' and t12075.t_unit_no between nvl('H104021002161',t_unit_no) and nvl('H104021002170',t_unit_no) order by t_unit_no,t_virus_code) T_1");
        }
    }
}
