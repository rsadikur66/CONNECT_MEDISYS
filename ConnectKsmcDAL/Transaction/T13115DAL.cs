using System.Collections.Generic;

namespace ConnectKsmcDAL.Transaction
{
    public class T13115DAL : CommonDAL
    {
        public IEnumerable<dynamic> GetAllPatientType(string lang)
        {
            var query = $"SELECT T_LANG{lang}_NAME PATIENT_TYPE, T_EPISODE_TYPE FROM T11025";
            return QueryList<dynamic>(query);
        }
        public IEnumerable<dynamic> GetPriorities(string lang)
        {
            var query = $"SELECT T_LANG{lang}_NAME PRIORITY_NAME, T_PRIORITY_CODE PRIORITY_CODE FROM T13003";
            return QueryList<dynamic>(query);
        }
        public IEnumerable<dynamic> GetAllWorkStation(string lang)
        {
            var query = $"SELECT T_WS_CODE CODE, T_LANG{lang}_NAME NAME FROM T13004 where T_WS_ACTIVE is not null ORDER BY 2";
            return QueryList<dynamic>(query);
        }
        public IEnumerable<dynamic> GetAnalysisNew(string wsCode, string lang)
        {
            var query = $@"SELECT T_LANG2_NAME ANALYSIS_NAME,T_ANALYSIS_CODE,T_GROUP_FLAG,T_SINGLE_FLAG FROM T13011 WHERE T_WS_CODE='{wsCode}' AND T_ACTIVE_FLAG IS NOT NULL AND T_DISPLAY_FLAG IS NULL ORDER BY T_GROUP_FLAG,T_LANG2_NAME";
            return QueryList<dynamic>(query);
        }
    }
}