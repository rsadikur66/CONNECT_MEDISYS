namespace ConnectKsmcDAL
{
    public class LoginDAL : DatabaseDAL
    {
        public dynamic LoginUser(string T_LOGIN_NAME, string T_PWD)
        {
            var query = @"SELECT T01009.T_LOGIN_NAME, T01009.T_USER_NAME, T01009.T_PWD, T01009.T_SITE_CODE, T01009.T_EMP_CODE, T01009.T_ROLE_CODE, T01009.T_USER_LANG FROM T01009 WHERE T01009.T_ACTIVE_FLAG = '1' AND T_LOGIN_NAME = :T_LOGIN_NAME AND T_PWD = :T_PWD AND T01009.T_ROLE_CODE IN ('0235', '0001')";
            return LoginQuery<dynamic>(query, new { T_LOGIN_NAME, T_PWD });
        }
    }
}