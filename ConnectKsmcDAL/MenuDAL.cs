using System.Collections.Generic;

namespace ConnectKsmcDAL
{
    public class MenuDAL : DatabaseDAL
    {
        public IEnumerable<dynamic> GetMenu(string USER_LANG, string T_LINK_SEPARATION, string T_ROLE_CODE, string BaseUrl)
        {
            var query = $@"SELECT T_LINK_LABEL_ID, T_LANG{USER_LANG}_NAME T_LINK_LABEL, REPLACE(T_LINK_TEXT, '../', '..{BaseUrl}/') T_LINK_TEXT FROM T01199 WHERE
                T_LINK_TEXT IS NOT NULL AND T_LINK_SEPARATION = '{T_LINK_SEPARATION}' AND T_ROLE_CODE = '{T_ROLE_CODE}' AND T_INACTIVE_FLAG IS NULL ORDER BY T_LINK_LABEL_ID";
            return QueryList<dynamic>(query);
        }
    }
}