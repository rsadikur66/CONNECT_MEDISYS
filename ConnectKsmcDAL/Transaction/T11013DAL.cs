using System;
using System.Collections.Generic;
using System.Data;

namespace ConnectKsmcDAL.Transaction
{
    public class T11013DAL : CommonDAL
    {
        public IEnumerable<dynamic> GetAllPatientType(string lang)
        {
            var query = $"SELECT T_LANG{lang}_NAME PATIENT_TYPE, T_EPISODE_TYPE FROM T11025";
            return QueryList<dynamic>(query);
        }
        public IEnumerable<dynamic> GetAllINFPREC(string lang)
        {
            var query = $"SELECT T_LANG{lang}_NAME INF_PREC, T_INF_PREC FROM T11024";
            return QueryList<dynamic>(query);
        }
        public IEnumerable<dynamic> GetAllPriority(string lang)
        {
            var query = $"SELECT T_URGENCY_DSCRPTN_LANG{lang} PRIORITY_CODE,T_URGENCY_CODE from t11002 order by T_URGENCY_CODE";
            return QueryList<dynamic>(query);
        }
        public IEnumerable<dynamic> GetPriorities(string lang)
        {
            var query = $"SELECT T_LANG{lang}_NAME PRIORITY_NAME, T_PRIORITY_CODE PRIORITY_CODE FROM T13003";
            return QueryList<dynamic>(query);
        }
        public IEnumerable<dynamic> GetMeritalStatus(string lang)
        {
            var query = $"SELECT T_LANG{lang}_NAME NAME, T_MRTL_STATUS_CODE CODE from T02007";
            return QueryList<dynamic>(query);
        }
        public dynamic GetPatientInfo(string patNo, string lang)
        {
            var query = $@"SELECT T_FIRST_LANG{lang}_NAME||' '||T_FATHER_LANG{lang}_NAME||' '||T_GFATHER_LANG{lang}_NAME||' '||T_FAMILY_LANG{lang}_NAME patient_name,
                           t1.T_RLGN_CODE,t2.T_LANG{lang}_NAME religion,t3.T_LANG{lang}_NAME meritalStatus,t4.T_LANG{lang}_NAME nationality,
                           t5.T_LANG{lang}_NAME gender,T_GENDER,t1.T_NTNLTY_CODE,T_BIRTH_DATE,T_MRTL_STATUS,
                           TRUNC(MONTHS_BETWEEN(sysdate, T_BIRTH_DATE) / 12, 0) AGE_YRS,
                           TRUNC(MOD(MONTHS_BETWEEN(sysdate, T_BIRTH_DATE), 12), 0) AGE_MOS
                           FROM T03001 t1 left join T02005 t2 on t2.T_RLGN_CODE=t1.T_RLGN_CODE left join T02007 t3 on t3.t_mrtl_status_code = t1.T_MRTL_STATUS
                           left join T02003 t4 on t4.t_ntnlty_code=t1.T_NTNLTY_CODE left join T02006 t5 on t5.t_sex_code=t1.T_GENDER
                           where T_pat_no='{patNo}'";
            return QuerySingle<dynamic>(query);
        }
        public IEnumerable<dynamic> GetAllRadiologyType(string lang)
        {
            var query = $"SELECT T_LANG{lang}_NAME TYPE_NAME, T_MAIN_PROC_CODE FROM T11100";
            return QueryList<dynamic>(query);
        }
        public IEnumerable<dynamic> GetLocationByPatType(string patType,string patNo,string lang)
        {
            var query = "";
            if (patType == "2")
            {
                query = $@"SELECT t1.T_CLINIC_CODE LOC_CODE,T_CLINIC_NAME_LANG{lang} LOC_NAME FROM T07001 t1,T07003 t3 WHERE t1.T_CLINIC_CODE=t3.T_CLINIC_CODE 
                        AND t3.T_APPT_DATE=TRUNC(SYSDATE) AND t3.T_ARRIVAL_STATUS IN('1','3') AND t3.T_PAT_NO='{patNo}' ORDER BY 1";
            }
            else if (patType == "3")
            {
                query = $@"SELECT T_LANG{lang}_NAME LOC_NAME,T_ER_LOCATION LOC_CODE FROM T04003 ORDER BY 1";
            }
            else
                query = $@"SELECT T_LANG{lang}_NAME LOC_NAME,T_LOC_CODE LOC_CODE FROM T02042 ORDER BY 1";
            return QueryList<dynamic>(query);
        }
        public IEnumerable<dynamic> GetAllDoctor(string lang)
        {
            string lName = lang == "1" ? "_ARB" : "";
            var query = $@"SELECT T_DOC_CODE,T_NAME_GIVEN{lName}||' '||T_NAME_FATHER{lName}||' '||T_NAME_GFATHER{lName}||' '||T_NAME_FAMILY DOC_NAME  
                        FROM T02029,T02039 WHERE T_DOC_CODE = T_EMP_NO AND T_EMP_ACTIVE='1'";
            return QueryList<dynamic>(query);
        }
        public dynamic GetDoctorInfo(string lang, string userId)
        {
            string lName = lang == "1" ? "_ARB" : "";
            var query = $@"SELECT t01.T_DOC_CODE,T_NAME_GIVEN{lName}||' '||T_NAME_FATHER{lName}||' '||T_NAME_GFATHER{lName}||' '||T_NAME_FAMILY DOC_NAME FROM T35001 t01,
                        T02029 t29 WHERE t01.T_USER_CODE = '{userId}' and t01.T_DOC_CODE(+)=t29.T_EMP_NO";
            return QuerySingle<dynamic>(query);
        }
        public IEnumerable<dynamic> GetAllProcedure(string typeCode,string lang)
        {
            string extraQuery = typeCode == null ? "" : $@" AND T_MAIN_PROC_CODE = '{typeCode}'";
            var query = $@"SELECT T_PROC_CODE,T_PROC_DSCRPTN_LANG{lang} PROC_DSCRPTN FROM T11001 WHERE T_ACTIVE_FLAG IS NOT NULL {extraQuery} ORDER BY T_PROC_CODE";
            return QueryList<dynamic>(query);
        }
        public dynamic GetPatientDetails(string patNo,string lang)
        {
            var query = $@"SELECT T_WARD_NO T_FRM_LOCATION,T_X_HOSP_CODE HOSPITAL,'1' T_PAT_TYPE,T_LANG{lang}_NAME WARD_DSC
                           FROM T05005,T02042 WHERE  T_PAT_NO = '{patNo}' AND T_WARD_NO = T_LOC_CODE";
            dynamic result = QuerySingle<dynamic>(query);
            if (result ==null)
            {
                query = $@"SELECT T03.T_CLINIC_CODE T_FRM_LOCATION,DECODE(T_CLINIC_HOSP_CODE,'1','1','2','2','3','3','1') HOSPITAL,
                           '2' T_PAT_TYPE,T_CLINIC_NAME_LANG{lang} WARD_DSC FROM T07003 T03,T07001 T01 WHERE T_PAT_NO='{patNo}'
                           AND T_APPT_DATE = TRUNC(SYSDATE) AND T_ARRIVAL_STATUS ='1' AND T01.T_CLINIC_CODE = T03.T_CLINIC_CODE";
                result = QuerySingle<dynamic>(query);
                if (result == null)
                {
                    query = $@"SELECT '3' T_PAT_TYPE,T1.T_ADMIT_LOCATION T_FRM_LOCATION,DECODE(T2.T_HOSP_CODE,'1','1','2','2','3','3','1') HOSPITAL,
                               T_LANG{lang}_NAME WARD_DSC FROM T04007 T1,T04003 T2 WHERE T_PAT_NO='{patNo}' AND T1.T_ADMIT_LOCATION=T2.T_ER_LOCATION AND T_EPISODE_END_DATE IS NULL";
                    result = QuerySingle<dynamic>(query);
                }
            }       
            return result;
        }
        public IEnumerable<dynamic> GetRadiologyRequestList(string patNo,string lang,string siteCode)
        {
            var query = $@"SELECT t11.T_ORDER_NO,to_char(t11.T_ORDER_DATE,'dd/MM/yyyy') T_ORDER_DATE,t11.T_ORDER_TIME,t11.T_ORDER_STATUS,t11.T_PRIORITY, t11.T_PAT_NO,t11.T_REQUEST_DOC,t11.T_CLINIC_CODE,
                        t03.T_LANG{lang}_NAME NTNLTY,PAT_NAME(t01.T_PAT_NO, '{lang}') PAT_NAME,t06.T_LANG{lang}_NAME GENDER,t42.T_LANG{lang}_NAME LOCATION_NAME,
                        CASE WHEN t01.T_BIRTH_DATE IS NOT NULL THEN TRUNC(MONTHS_BETWEEN(SYSDATE , t01.T_BIRTH_DATE) / 12, 0) ELSE 0 END AGE_YRS,
                        CASE WHEN t01.T_BIRTH_DATE IS NOT NULL THEN TRUNC(MOD(MONTHS_BETWEEN(SYSDATE, t01.T_BIRTH_DATE), 12), 0) ELSE 0 END AGE_MOS,t12.T_TYPE_CODE,
                        t29.T_NAME_GIVEN ||'' || t29.T_NAME_FAMILY DOC_NAME,t11.T_ENTRY_DATE,(SELECT T_USER_NAME FROM T01009 WHERE T_EMP_CODE = t11.T_ENTRY_USER) ENTRY_USER
                        FROM T11011 t11,T11012 t12,T03001 t01,T02006 t06,T02003 t03,T02029 t29,T02042 t42
                        WHERE t11.T_PAT_NO =t01.T_PAT_NO AND t01.T_GENDER=t06.T_SEX_CODE(+) AND t11.T_ORDER_NO=t12.T_ORDER_NO(+) 
                        AND t01.T_NTNLTY_CODE=t03.T_NTNLTY_CODE(+) AND t11.T_REQUEST_DOC=t29.T_EMP_NO(+) AND t11.T_CLINIC_CODE=t42.T_LOC_CODE(+)
                        AND t11.T_SITE_CODE='{siteCode}' and t11.T_PAT_NO='{patNo}' ORDER BY TO_DATE(T_ORDER_DATE, 'dd/MM/yyyy') DESC";
            return QueryList<dynamic>(query);            
        }
        public dynamic GetRadiologyRequestDetails(string orderNo, string lang, string siteCode)
        {
            dynamic T11011 = new System.Dynamic.ExpandoObject();
            var query = $@"SELECT t11.T_ORDER_NO,to_char(t11.T_ORDER_DATE,'dd/MM/yyyy') T_ORDER_DATE,t11.T_PRIORITY,t11.T_PAT_NO,t11.T_FRM_LOCATION,t11.T_CLINICAL_DATA,
                        t11.T_REQUEST_DOC,t11.T_IPOP_FLAG,t11.T_CLINIC_CODE,t11.T_X_HOSP,t11.T_ORDER_TIME,t11.T_ALERGY_FLAG,to_char(t11.T_LMP_DATE,'dd/MM/yyyy') T_LMP_DATE,
                        t11.T_PREGNANCY_FLAG,t11.T_IV_CONSTRAST,t11.T_LAB_REQ,t11.T_ANASTHESIA,t11.T_INF_PREC,t11.T_URGENT_REP,PAT_NAME(t01.T_PAT_NO, '{lang}') PAT_NAME,
                        t06.T_LANG{lang}_NAME GENDER,t03.T_LANG{lang}_NAME NTNLTY,CASE WHEN t01.T_BIRTH_DATE IS NOT NULL THEN TRUNC(MONTHS_BETWEEN(SYSDATE , t01.T_BIRTH_DATE) / 12, 0) ELSE 0 END AGE_YRS,
                        CASE WHEN t01.T_BIRTH_DATE IS NOT NULL THEN TRUNC(MOD(MONTHS_BETWEEN(SYSDATE, t01.T_BIRTH_DATE), 12), 0) ELSE 0 END AGE_MOS,
                        t42.T_LANG{lang}_NAME LOCATION_NAME,t29.T_NAME_GIVEN ||''|| t29.T_NAME_FAMILY DOC_NAME FROM T11011 t11,T03001 t01,T02006 t06,T02003 t03,T02029 t29,
                        T02042 t42,t11002 t02 WHERE t11.T_ORDER_NO='{orderNo}' and t11.T_PAT_NO =t01.T_PAT_NO AND t01.T_GENDER=t06.T_SEX_CODE(+) 
                        AND t01.T_NTNLTY_CODE=t03.T_NTNLTY_CODE(+) AND t11.T_REQUEST_DOC=t29.T_EMP_NO(+) AND t11.T_CLINIC_CODE=t42.T_LOC_CODE(+)
                        AND t11.T_CLINIC_CODE=t02.T_URGENCY_CODE(+) AND t11.T_SITE_CODE='{siteCode}'";
            T11011 = QuerySingle<dynamic>(query);
            query = $@"SELECT T1.T_ORDER_NO,T1.T_TYPE_CODE,T1.T_PROC_CODE,T1.T_PROC_NOTES,T1.T_PRIORITY_CODE,T1.T_INDICATION,T2.T_PROC_DSCRPTN_LANG{lang} 
                        PROCEDURE_DSCRPTN,T0.T_LANG{lang}_NAME TYPE_NAME FROM T11012 T1,T11001 T2,T11100 T0 WHERE T1.T_PROC_CODE=T2.T_PROC_CODE(+) 
                        AND T1.T_TYPE_CODE=T0.T_MAIN_PROC_CODE(+) AND T1.T_ORDER_NO='{orderNo}'";
            T11011.T11012 = QuerySingle<dynamic>(query);
            query = $@"SELECT T_PROC_DSCRPTN_LANG{lang} NAME,A.T_PROC_CODE CODE,T_SUB_ORDER FROM T11001 A,T11013 B WHERE A.T_PROC_CODE=B.T_PROC_CODE AND B.T_ORDER_NO='{orderNo}' ORDER BY B.T_SUB_ORDER";
            T11011.T11013 = QueryList<dynamic>(query);
            query = $@"SELECT T.T_M_FEED,T.T_M_CON,T.T_M_CHL,T.T_M_HRT,T.T_M_HHL,T.T_M_MAM,T.T_M_DATE,T.T_M_PMAM,T.T_M_LES,T.T_M_AXI,T.T_M_DWM,T.T_M_NIP,T.T_M_TUM,
                    T.T_M_BRE,T.T_M_BCAU,T.T_M_HCAN,T.T_M_FAMB,T.T_M_FMSD,T.T_M_CLI,T.T_I_PRE,T.T_I_CAR,T.T_I_SUR,T.T_I_INT,T.T_I_ART,T.T_I_IMF,T.T_I_JPF,T.T_I_MRI,
                    T.T_I_PSUR,T.T_M_NCHD,T.T_M_LWH,T.T_I_KIDNE,T.T_I_NEURO,T.T_I_HRAID,T.T_I_CLAUS,T.T_I_IPCARD,T.T_I_PRGSTU,T.T_I_PENIM,T.T_I_MEDPAT,T.T_M_AGEDIA,
                    T.T_M_HCHEM,T.T_M_HRAD,T.T_M_MSLR,T.T_M_AXIR,T.T_M_AXIL,T.T_M_NIPPR,T.T_M_NIPPL,T.T_M_SKNR,T.T_M_SKNL,T.T_M_NIPDSR,T.T_M_NIPDSL,T.T_M_PAINR,
                    T.T_M_PAINL,T.T_M_FMHIS,T.T_M_MSLL,T.T_M_TUMSPY,T.T_M_PATCH,T.T_M_OTHERS,T.T_I_KIDNY,T.T_MRI_FLAG,T.T_I_COMPAT,T.T_I_IVCFIL,T.T_I_VASCLIP,T.T_I_DENT,
                    T.T_I_SWANS,T.T_M_COMM,T.T_MAM_FLAG FROM T11019 T WHERE T.T_ORDER_NO='{orderNo}'";
            T11011.T11019 = QuerySingle<dynamic>(query);
            return T11011;
        }
        public DataTable GetRadiologyData(string orderNo, string siteCode, string lang)
        {
            var query = $@" SELECT t11.T_PAT_NO,PAT_NAME(t01.T_PAT_NO, '{lang}') PAT_NAME,t06.T_LANG{lang}_NAME GENDER,t03.T_LANG{lang}_NAME NTNLTY,
                        CASE WHEN t01.T_BIRTH_DATE IS NOT NULL THEN TRUNC(MONTHS_BETWEEN(SYSDATE , t01.T_BIRTH_DATE) / 12, 0) ELSE 0 END AGE_YRS,
                        CASE WHEN t01.T_BIRTH_DATE IS NOT NULL THEN TRUNC(MOD(MONTHS_BETWEEN(SYSDATE, t01.T_BIRTH_DATE), 12), 0) ELSE 0 END AGE_MOS,
                        t25.T_LANG{lang}_NAME PATIENT_TYPE,t42.T_LANG{lang}_NAME LOCATION_NAME, decode(T_X_HOSP,'1','Central','2','Pedia','3','Maternity',null) T_X_HOSP,
                        DECODE(T_PREGNANCY_FLAG,'1','Yes','2','No',null) T_PREGNANCY,t24.T_LANG{lang}_NAME INF_PREC,t11.T_ORDER_NO,'!'||t11.T_ORDER_NO||'!' ORDER_BARCODE,
                        to_char(t11.T_ORDER_DATE,'dd/MM/yyyy') T_ORDER_DATE,t11.T_ORDER_TIME,t11.T_ORDER_STATUS,to_char(t11.T_LMP_DATE,'dd/MM/yyyy') T_LMP_DATE,
                        decode(T_LAB_REQ,'1','Available','2','Requesting',null) T_LAB_REQ,T_URGENT_REP,DECODE(T_IV_CONSTRAST,'1','Yes','2','No',null) T_IV_CONSTRAST,
                        DECODE(T_ALERGY_FLAG,'1','Yes','2','No',null) T_ALERGY,DECODE(T_ANASTHESIA,'1','Yes','2','No',null) T_ANASTHESIA,
                        t29.T_NAME_GIVEN ||''|| t29.T_NAME_FAMILY DOC_NAME,t11.T_CLINICAL_DATA, t00.T_LANG{lang}_NAME TYPE_NAME,t_01.T_PROC_DSCRPTN_LANG{lang} PROC_DSCRPTN,
                        t02.T_URGENCY_DSCRPTN_LANG{lang} PRIORITY,t12.T_PROC_NOTES,t12.T_INDICATION
                        FROM T11011 t11,T11012 t12,T03001 t01,T02006 t06,T02003 t03,T02029 t29,T02042 t42,t11002 t02,T11001 t_01,T11100 t00,T11024 t24,T11025 t25
                        WHERE t11.T_ORDER_NO='{orderNo}' and t11.T_ORDER_NO=t12.T_ORDER_NO and t11.T_PAT_NO =t01.T_PAT_NO            
                        AND t01.T_GENDER=t06.T_SEX_CODE(+) 
                        AND t01.T_NTNLTY_CODE=t03.T_NTNLTY_CODE(+)
                        AND t11.T_REQUEST_DOC=t29.T_EMP_NO(+)
                        AND t11.T_CLINIC_CODE=t42.T_LOC_CODE(+)
                        AND t11.T_SITE_CODE='{siteCode}' 
                        AND t12.T_PRIORITY_CODE=t02.T_URGENCY_CODE(+)
                        AND t12.T_PROC_CODE=t_01.T_PROC_CODE(+)
                        AND t12.T_TYPE_CODE=t00.T_MAIN_PROC_CODE(+)
                        AND t11.T_INF_PREC=t24.T_INF_PREC(+)
                        AND t11.T_IPOP_FLAG=t25.T_EPISODE_TYPE(+)";
                        return ReportQuery(query);
        }
        public DataTable GetMamoMRIdata(string orderNo)
        {
            var query =  $@"SELECT T_M_FEED,T_M_CON,T_M_CHL,T_M_HRT,T_M_HCAN,T_M_HRAD,T_M_MSLR,T_M_MSLL,T_M_AXIR,T_M_AXIL,T_M_NIPPR,T_M_NIPPL,T_M_SKNR,T_M_SKNL,T_M_NIPDSR,
                        T_M_NIPDSL,T_M_PAINR,T_M_PAINL,T_M_HCHEM,T_M_TUMSPY,T_M_FMHIS,T_M_AGEDIA,T_M_HHL,T_M_MAM,T_M_DATE,T_M_LWH,T_M_AXI,T_M_LES,T_M_FAMB, T_M_FMHIS,
                        Decode(T_M_PMAM,'1','To exclude the Malignancy','2','Routine screening','3','Follow-up existing lesion', null) Purpose,T_I_NEURO,T_I_HRAID,
                        Decode(T_M_NIP,'1','Yes','2','No','3','Right','4','Left','5','Multiporal','6','Uniporal') Nipple,T_M_TUM,T_I_PRGSTU,T_I_PENIM,T_M_PATCH,T_I_COMPAT,
                        Decode(T_M_BRE,'1','Yes','2','No','3','Right', '4', 'Left') Breast_surgery,T_M_BCAU,T_M_HCAN, T_M_OTHERS,T_I_VASCLIP,T_I_DENT,T_I_SWANS,
                        Decode(T_M_DWM,'1','Days','2','Weeks','3','Months') Complaint,Decode(T_M_FMSD,'1','Mother','2','Sister','3','Daughter','4','Others') Family_yes,
                        Decode(T_M_CLI, '1', 'Benign', '2', 'Malignant') Clinical,T_I_CAR,T_I_SUR,T_I_INT,T_I_ART,T_I_IMF,T_I_JPF,T_I_MRI,T_I_PSUR,T_I_PRE,T_I_KIDNY,
                        T_I_IVCFIL,T_I_CLAUS,T_I_IPCARD FROM T11019 WHERE T_ORDER_NO='{orderNo}'";
            return ReportQuery(query);
        }
        public DataTable GetProcedureData(string orderNo,string lang)
        {
            var query = $@"SELECT T_PROC_DSCRPTN_LANG{lang} SECONDARY_PROCEDURE,A.T_PROC_CODE SECONDARY_CODE FROM T11001 A,T11013 B WHERE A.T_PROC_CODE=B.T_PROC_CODE AND B.T_ORDER_NO='{orderNo}'";
            return ReportQuery(query);
        }
        string GetOrderNo(string hospital)
        {
            string exQry = hospital == "1" ? " T11_CEN_SEQ.nextval " : hospital == "2" ? " T11_PED_SEQ.nextval " : hospital == "3" ? " T11_MAT_SEQ.nextval " : " T11_CEN_SEQ.nextval ";
            //string orderNo = QueryString($@"select {exQry} SEQ from dual");
            string orderNo = QueryString($@"select max(T_ORDER_NO)+1 from T11011");
            //orderNo = hospital + orderNo.PadLeft(7,'0');
            string existingOrderNo = QueryString($@"SELECT DISTINCT '1' T_ORDER_NO FROM T11011 WHERE T_ORDER_NO = '{orderNo}'");
            if (!string.IsNullOrEmpty(existingOrderNo))
            {
                GetOrderNo(hospital);
            }
            return orderNo;
        }
        public string SaveT11011(dynamic T11, string userId, string siteCode)
        {
            string command = "";
            bool result = false;
            dynamic T12 = T11.T11012;
            dynamic T13 = T11.T11013;
            dynamic T19 = T11.T11019;
            if (Convert.ToString(T11.T_ORDER_NO.Value) != "")
            {
                command = $@"UPDATE T11011 SET T_IPOP_FLAG='{T11.T_IPOP_FLAG}',T_FRM_LOCATION='{T11.T_FRM_LOCATION}',
                        T_ORDER_DATE=to_date('{T11.T_ORDER_DATE}','dd/MM/yyyy'),T_ORDER_TIME='{T11.T_ORDER_TIME}',T_X_HOSP='{T11.HOSPITAL}',T_ALERGY_FLAG='{T11.T_ALERGY_FLAG}',
                        T_LMP_DATE=to_date('{T11.T_LMP_DATE}','dd/MM/yyyy'),T_TMP_FLAG='{T11.TMP_FLAG}',T_PREGNANCY_FLAG='{T11.T_PREGNANCY_FLAG}',T_LAB_REQ='{T11.T_LAB_REQ}',
                        T_IV_CONSTRAST='{T11.T_IV_CONSTRAST}',T_ANASTHESIA='{T11.T_ANASTHESIA}',T_INF_PREC='{T11.T_INF_PREC}',T_URGENT_REP='{T11.T_URGENT_REP}',
                        T_UPD_DATE=TRUNC(SYSDATE),T_UPD_USER='{userId}',T_CLINICAL_DATA='{T11.CLINICAL_DATA}',T_PROVISIONAL_DIAG='{T11.PROVISIONAL_DIAG}',T_REQUEST_DOC='{T11.T_REQUEST_DOC}',
                        T_CLINIC_CODE='{T11.T_CLINIC_CODE}',T_CLINIC_SPCLTY_CODE='{T11.CLINIC_SPCLTY}' WHERE T_ORDER_NO='{T11.T_ORDER_NO}' AND T_PAT_NO='{T11.T_PAT_NO}' AND T_SITE_CODE='{siteCode}'";
                result = Command(command);
                if (result)
                {
                    command = $@"UPDATE T11012 SET T_TYPE_CODE='{T12.T_TYPE_CODE}',T_PROC_CODE= '{T12.T_PROC_CODE}',T_PROC_NOTES='{T12.T_PROC_NOTES}',
                                T_PRIORITY_CODE='{T12.T_PRIORITY_CODE}',T_RESULT_NOTES='{T12.T_RESULT_NOTES}',T_INDICATION='{T12.T_INDICATION}',
                                T_UPD_DATE=TRUNC(SYSDATE),T_UPD_USER='{ userId}' WHERE T_ORDER_NO='{T11.T_ORDER_NO}'";
                    result = Command(command);
                    command = $@"DELETE FROM T11013 WHERE T_ORDER_NO='{T11.T_ORDER_NO.Value}' ";
                    result = Command(command);
                    command = $@"DELETE FROM T11019 WHERE T_ORDER_NO='{T11.T_ORDER_NO.Value}' ";
                    result = Command(command);
                }
            }
            else
            {
                string orderNo = GetOrderNo(T11.HOSPITAL.Value);
                command = $@"INSERT INTO T11011 (T_IPOP_FLAG,T_FRM_LOCATION,T_ORDER_NO,T_ORDER_DATE,T_ORDER_TIME,T_X_HOSP,T_ALERGY_FLAG,
                        T_LMP_DATE,T_PREGNANCY_FLAG,T_IV_CONSTRAST,T_LAB_REQ,T_ANASTHESIA,T_INF_PREC,T_URGENT_REP,T_TMP_FLAG,T_PAT_NO,
                        T_ENTRY_DATE,T_ENTRY_USER,T_CLINICAL_DATA,T_PROVISIONAL_DIAG,T_REQUEST_DOC,T_CLINIC_CODE,T_CLINIC_SPCLTY_CODE,T_SITE_CODE) VALUES 
                        ('{T11.T_IPOP_FLAG}','{T11.T_FRM_LOCATION}','{orderNo}',to_date('{T11.T_ORDER_DATE}','dd/MM/yyyy'),'{T11.T_ORDER_TIME}',
                        '{T11.HOSPITAL}','{T11.T_ALERGY_FLAG}',to_date('{T11.T_LMP_DATE}','dd/MM/yyyy'),'{T11.T_PREGNANCY_FLAG}','{T11.T_IV_CONSTRAST}',
                        '{T11.T_LAB_REQ}','{T11.T_ANASTHESIA}','{T11.T_INF_PREC}','{T11.T_URGENT_REP}','{T11.TMP_FLAG}','{T11.T_PAT_NO}',
                        TRUNC(SYSDATE),'{userId}','{T11.CLINICAL_DATA}','{T11.PROVISIONAL_DIAG}','{T11.T_REQUEST_DOC}','{T11.T_CLINIC_CODE}',
                        '{T11.CLINIC_SPCLTY}','{siteCode}')";
                result = Command(command);
                if (result)
                {
                    command = $@"INSERT INTO T11012(T_ORDER_NO,T_TYPE_CODE ,T_PROC_CODE,T_PROC_NOTES,T_PRIORITY_CODE,T_RESULT_NOTES,
                                T_INDICATION,T_ENTRY_DATE,T_ENTRY_USER) VALUES ('{orderNo}','{T12.T_TYPE_CODE}','{T12.T_PROC_CODE}','{T12.T_PROC_NOTES}',
                                '{T12.T_PRIORITY_CODE}','{T12.T_RESULT_NOTES}','{T12.T_INDICATION}',trunc(sysdate),'{ userId}')";
                    result = Command(command);
                }
                T11.T_ORDER_NO = orderNo;
            }
            this.SaveT11013(T13, T11.T_ORDER_NO.Value, userId);
            if (T12.T_TYPE_CODE.Value == "0002" || T12.T_TYPE_CODE.Value == "0008")
            {
                this.SaveT11019(T19, Convert.ToString(T11.T_ORDER_NO.Value), T11.T_PAT_NO.Value, userId);
            }
            return T11.T_ORDER_NO;
        }
        void SaveT11013(dynamic T13, dynamic orderNo, string userId)
        {
            string command = "";
            if (T13 != null)
            {
                foreach (var item in T13)
                {
                    command = $@"INSERT INTO T11013(T_ENTRY_USER,T_ENTRY_DATE,T_ORDER_NO,T_PROC_CODE ,T_SUB_ORDER) VALUES ('{ userId}',
                             trunc(sysdate),'{orderNo}','{item.CODE}','{T13.IndexOf(item)}')";
                    Command(command);
                }
            }
        }
        bool SaveT11019(dynamic T19, string orderNo, string patNo, string userId)
        {
            string command = "";
            command = $@"INSERT INTO T11019(T_ENTRY_USER,T_ENTRY_DATE,T_ORDER_NO,T_PAT_NO,T_M_FEED,T_M_CON,T_M_CHL,T_M_HRT,T_M_HHL,
                          T_M_MAM,T_M_DATE,T_M_PMAM,T_M_LES,T_M_AXI,T_M_DWM,T_M_NIP,T_M_TUM,T_M_BRE,T_M_BCAU,T_M_HCAN,T_M_FAMB,
                          T_M_FMSD,T_M_CLI,T_I_PRE,T_I_CAR,T_I_SUR,T_I_INT,T_I_ART,T_I_IMF,T_I_JPF,T_I_MRI,T_I_PSUR,T_M_NCHD,
                          T_M_LWH,T_I_KIDNE,T_I_NEURO,T_I_HRAID,T_I_CLAUS,T_I_IPCARD,T_I_PRGSTU,T_I_PENIM,T_I_MEDPAT,T_M_AGEDIA,  
                          T_M_HCHEM,T_M_HRAD,T_M_MSLR,T_M_AXIR,T_M_AXIL,T_M_NIPPR,T_M_NIPPL,T_M_SKNR,T_M_SKNL,T_M_NIPDSR,T_M_NIPDSL,
                          T_M_PAINR,T_M_PAINL,T_M_FMHIS,T_M_MSLL,T_M_TUMSPY,T_M_PATCH,T_M_OTHERS,T_I_KIDNY,T_MRI_FLAG,T_I_COMPAT,
                          T_I_IVCFIL,T_I_VASCLIP,T_I_DENT,T_I_SWANS,T_M_COMM,T_MAM_FLAG) VALUES ('{ userId}',trunc(sysdate),
                          '{orderNo}','{patNo}','{T19.T_M_FEED.Value}','{T19.T_M_CON.Value}','{T19.T_M_CHL.Value}','{T19.T_M_HRT.Value}','{T19.T_M_HHL.Value}',
                          '{T19.T_M_MAM.Value}',to_date('{T19.T_M_DATE.Value}','dd/MM/yyyy'),'{T19.T_M_PMAM.Value}','{T19.T_M_LES.Value}','{T19.T_M_AXI.Value}','{T19.T_M_DWM.Value}','{T19.T_M_NIP.Value}',
                          '{T19.T_M_TUM.Value}','{T19.T_M_BRE.Value}','{T19.T_M_BCAU.Value}','{T19.T_M_HCAN.Value}','{T19.T_M_FAMB.Value}','{T19.T_M_FMSD.Value}','{T19.T_M_CLI.Value}',
                          '{T19.T_I_PRE.Value}','{T19.T_I_CAR.Value}','{T19.T_I_SUR.Value}','{T19.T_I_INT.Value}','{T19.T_I_ART.Value}','{T19.T_I_IMF.Value}','{T19.T_I_JPF.Value}',
                          '{T19.T_I_MRI.Value}','{T19.T_I_PSUR.Value}','{T19.T_M_NCHD.Value}','{T19.T_M_LWH.Value}','{T19.T_I_KIDNE.Value}','{T19.T_I_NEURO.Value}','{T19.T_I_HRAID.Value}',
                          '{T19.T_I_CLAUS.Value}','{T19.T_I_IPCARD.Value}','{T19.T_I_PRGSTU.Value}','{T19.T_I_PENIM.Value}','{T19.T_I_MEDPAT.Value}','{T19.T_M_AGEDIA.Value}', 
                          '{T19.T_M_HCHEM.Value}','{T19.T_M_HRAD.Value}','{T19.T_M_MSLR.Value}','{T19.T_M_AXIR.Value}','{T19.T_M_AXIL.Value}','{T19.T_M_NIPPR.Value}','{T19.T_M_NIPPL.Value}',
                          '{T19.T_M_SKNR.Value}','{T19.T_M_SKNL.Value}','{T19.T_M_NIPDSR.Value}','{T19.T_M_NIPDSL.Value}','{T19.T_M_PAINR.Value}','{T19.T_M_PAINL.Value}','{T19.T_M_FMHIS.Value}',
                          '{T19.T_M_MSLL.Value}','{T19.T_M_TUMSPY.Value}','{T19.T_M_PATCH.Value}','{T19.T_M_OTHERS.Value}','{T19.T_I_KIDNY.Value}','{T19.T_MRI_FLAG.Value}','{T19.T_I_COMPAT.Value}',
                          '{T19.T_I_IVCFIL.Value}','{T19.T_I_VASCLIP.Value}','{T19.T_I_DENT.Value}','{T19.T_I_SWANS.Value}','{T19.T_M_COMM.Value}','{T19.T_MAM_FLAG.Value}')";
            return Command(command);
        }
    }
}