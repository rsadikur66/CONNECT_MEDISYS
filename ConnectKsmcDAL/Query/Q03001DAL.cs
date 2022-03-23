using System.Collections.Generic;

namespace ConnectKsmcDAL.Query
{
    public class Q03001DAL : CommonDAL
    {
        public IEnumerable<dynamic> GetAllData(string Language, string T_SITE_CODE)
        {
            var query = $@"SELECT T_PAT_NO, T_FIRST_LANG{Language}_NAME || ' ' || T_FATHER_LANG{Language}_NAME || ' ' || T_GFATHER_LANG{Language}_NAME || ' ' || T_FAMILY_LANG{Language}_NAME T_PAT_NAME,
                T02006.T_LANG{Language}_NAME T_GENDER, TRUNC(MONTHS_BETWEEN(SYSDATE, T_BIRTH_DATE ) / 12, 0) YEARS, TRUNC(MOD(MONTHS_BETWEEN(SYSDATE, T_BIRTH_DATE), 12), 0) MONTHS, T_NTNLTY_ID, T02003.T_LANG{Language}_NAME T_NATIONALITY,
                T_MOBILE_NO FROM T02003, T02006, T03001 WHERE T_GENDER = T_SEX_CODE(+) AND T02003.T_NTNLTY_CODE (+)= T03001.T_NTNLTY_CODE AND T_SITE_CODE = '{T_SITE_CODE}'";
            return QueryList<dynamic>(query);
        }
        public IEnumerable<dynamic> GetAllPatientBySearch(string firstName, string fatherName, string gFather, string lastName, string gender, string ageFrom, string ageTo, string patNo, string rmcNo, string mobileNo, string phone, string meritalSts, string natCode, string nID, string fDate, string tDate, string lang)
        {
            string extraQuery = "";
            if (!string.IsNullOrEmpty(patNo))
            {
                extraQuery = $@" AND t1.T_PAT_NO='{patNo}'";
            }
            if (!string.IsNullOrEmpty(rmcNo))
            {
                extraQuery += $@" AND t1.T_X_RMC_CHRTNO='{rmcNo}'";
            }
            if (!string.IsNullOrEmpty(mobileNo))
            {
                extraQuery += $@" AND t1.T_MOBILE_NO='{mobileNo}'";
            }
            if (!string.IsNullOrEmpty(phone))
            {
                extraQuery += $@" AND t1.T_PHONE_HOME='{phone}'";
            }
            if (!string.IsNullOrEmpty(natCode))
            {
                extraQuery += $@" AND t1.T_NTNLTY_CODE='{natCode}'";
            }
            if (!string.IsNullOrEmpty(gender))
            {
                extraQuery += $@" AND t1.T_GENDER='{gender}'";
            }
            if (!string.IsNullOrEmpty(nID))
            {
                extraQuery += $@" AND t1.T_NTNLTY_ID='{nID}'";
            }
            if (!string.IsNullOrEmpty(meritalSts))
            {
                extraQuery += $@" AND t1.T_MRTL_STATUS='{meritalSts}'";
            }
            if (!string.IsNullOrEmpty(ageFrom) && !string.IsNullOrEmpty(ageTo))
            {
                extraQuery += $@" AND trunc(months_between(sysdate, t1.t_birth_date) / 12, 0) between {ageFrom} and {ageTo}";
            }
            else if (!string.IsNullOrEmpty(ageFrom) && string.IsNullOrEmpty(ageTo))
            {
                extraQuery += $@" AND trunc(months_between(sysdate, t1.t_birth_date) / 12, 0)={ageFrom}";
            }
            else if (string.IsNullOrEmpty(ageFrom) && !string.IsNullOrEmpty(ageTo))
            {
                extraQuery += $@" AND trunc(months_between(sysdate, t1.t_birth_date) / 12, 0)={ageTo}";
            }
            if (!string.IsNullOrEmpty(fDate) && !string.IsNullOrEmpty(tDate))
            {
                extraQuery += $@" AND t1.t_entry_date between to_date('{fDate}','dd/mm/yyyy') and to_date('{tDate}','dd/mm/yyyy')";
            }
            else if (!string.IsNullOrEmpty(fDate) && string.IsNullOrEmpty(tDate))
            {
                extraQuery += $@" AND t1.t_entry_date=to_date('{fDate}','dd/mm/yyyy')";
            }
            else if (string.IsNullOrEmpty(fDate) && !string.IsNullOrEmpty(tDate))
            {
                extraQuery += $@" AND t1.t_entry_date=to_date('{tDate}','dd/mm/yyyy')";
            }
            if (!string.IsNullOrWhiteSpace(firstName))
            {
                extraQuery += $@" AND T_FIRST_LANG{lang}_NAME like '{firstName.Trim() + '%'}'";
            }
            if (!string.IsNullOrWhiteSpace(fatherName))
            {
                extraQuery += $@" AND T_FATHER_LANG{lang}_NAME like '{fatherName.Trim() + '%'}'";
            }
            if (!string.IsNullOrWhiteSpace(gFather))
            {
                extraQuery += $@" AND T_GFATHER_LANG{lang}_NAME like '{gFather.Trim() + '%'}'";
            }
            if (!string.IsNullOrWhiteSpace(lastName))
            {
                extraQuery += $@" AND T_FAMILY_LANG{lang}_NAME like '{lastName.Trim() + '%'}'";
            }
            var query = $@"SELECT T_PAT_NO,T_FIRST_LANG{lang}_NAME||' '||T_FATHER_LANG{lang}_NAME||' '||T_GFATHER_LANG{lang}_NAME||' '||
                        T_FAMILY_LANG{lang}_NAME PATIENT_NAME,T_FIRST_LANG{lang}_NAME T_FIRST_NAME,T_FATHER_LANG{lang}_NAME T_FATHER_NAME,
                        T_GFATHER_LANG{lang}_NAME T_GFATHER_NAME,T_FAMILY_LANG{lang}_NAME T_FAMILY_NAME,t1.T_NTNLTY_CODE,                        
                        T_NTNLTY_ID,T_MOBILE_NO,to_char(t1.T_ENTRY_DATE,'dd/mm/yyyy') T_ENTRY_DATE,t1.T_GENDER,
                        t2.T_LANG{lang}_NAME MeritalStatus,t3.T_LANG{lang}_NAME Nationality,t4.T_LANG{lang}_NAME Gender,
                        t5.T_LANG{lang}_NAME Religion,TRUNC(MONTHS_BETWEEN(sysdate, T_BIRTH_DATE) / 12, 0) AGE_YRS,
                        TRUNC(MOD(MONTHS_BETWEEN(sysdate, T_BIRTH_DATE), 12), 0) AGE_MOS FROM  T03001 t1
                        left join T02007 t2 on t2.t_mrtl_status_code =t1.T_MRTL_STATUS
                        left join T02003 t3 on t3.t_ntnlty_code = t1.T_NTNLTY_CODE
                        left join T02006 t4 on t4.t_sex_code = t1.T_GENDER
                        left join T02005 t5 on t5.t_rlgn_code = t1.T_RLGN_CODE
                        where 1=1 {extraQuery}";
            return QueryList<dynamic>(query);
        }
    }
}