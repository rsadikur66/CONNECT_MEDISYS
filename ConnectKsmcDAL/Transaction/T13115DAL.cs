using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

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
        //GetAnalysisByWS
        public IEnumerable<dynamic> GetAnalysisByWS(string wsCode)
        {
            var query = $"select t_analysis_code CODE, t_lang2_name NAME ,T_WS_CODE from t13011 where t_ws_code =  '{wsCode}' and t_active_flag is not null and t_display_flag is null order by 2";
            return QueryList<dynamic>(query);
        }
        public IEnumerable<dynamic> GetAnalysisNew(string wsCode, string lang)
        {
            var query = $@"SELECT T_LANG2_NAME ANALYSIS_NAME,T_ANALYSIS_CODE,T_WS_CODE,(select distinct t_lang2_name from t13004 where t_ws_code=T13011.t_ws_code) WS_NAME,T_GROUP_FLAG,T_SINGLE_FLAG
            FROM T13011 WHERE (T_WS_CODE=nvl('{wsCode}',T_WS_CODE) or upper(t_lang2_name) like '%{wsCode.ToUpper()}%')  AND T_ACTIVE_FLAG IS NOT NULL AND T_DISPLAY_FLAG IS NULL ORDER BY T_GROUP_FLAG,T_LANG2_NAME";
            return QueryList<dynamic>(query);
        }
        public IEnumerable<dynamic> GetAnalysis()
        {
            var query = $@"SELECT T_LANG2_NAME NAME,T_ANALYSIS_CODE CODE,T_WS_CODE,T_GROUP_FLAG,T_SINGLE_FLAG FROM T13011  ORDER BY T_GROUP_FLAG,T_LANG2_NAME";
            return QueryList<dynamic>(query);
        }
        //select t_site_code CODE,t_lang2_name NAME  from t13048

        public IEnumerable<dynamic> GetNatOR(string code, string lang)
        {
            var query = $@"SELECT T_SITE_CODE CODE,T_LANG{lang}_NAME NAME FROM t13048 WHERE T_SITE_CODE=nvl('{code}',T_SITE_CODE)  ORDER BY T_LANG{lang}_NAME";
            return QueryList<dynamic>(query);
        }

        public bool CheckAppointment(string patNo)
        {
            return QueryString($@"select distinct '1' appointment from v07028 where t_pat_no ='{patNo}' and  t_appt_date = trunc(sysdate) and t_arrival_status in ('1','3')") == "1";
        }

        public IEnumerable<dynamic> GetPatInfo(string patNo, string lang)
        {
            var query = $@"SELECT DISTINCT T03001.T_PAT_NO,PAT_NAME(T03001.T_PAT_NO,'{lang}')PAT_NAME, PAT_YEAR(T03001.T_PAT_NO) YEARS,
            PAT_MONTH(T03001.T_PAT_NO) MONTHS,PAT_GENDER(T03001.T_PAT_NO, '{lang}')GENDER,PAT_NATIONALITY(T03001.T_PAT_NO,'{lang}')NATIONALITY,T03001.T_GENDER,
            (select distinct T_LANG{lang}_NAME from T02007  where  t_mrtl_status_code=t03001.t_mrtl_status)MRTL_STATUS            
            from t03001 WHERE T_PAT_NO = '{patNo}'";
            return QueryList<dynamic>(query);
        }
        public IEnumerable<dynamic> GetRequestT13015(string requestNo, string lang)
        {
            var query = $@"SELECT DISTINCT T03001.T_PAT_NO,PAT_NAME(T03001.T_PAT_NO,'{lang}')PAT_NAME, PAT_YEAR(T03001.T_PAT_NO) YEARS,PAT_MONTH(T03001.T_PAT_NO) MONTHS,PAT_GENDER(T03001.T_PAT_NO,'{lang}')GENDER,PAT_NATIONALITY(T03001.T_PAT_NO,'{lang}')NATIONALITY,T03001.T_GENDER,(SELECT DISTINCT T_LANG2_NAME FROM T02007  WHERE  T_MRTL_STATUS_CODE=T03001.T_MRTL_STATUS)MRTL_STATUS,
            T13015.T_PAT_TYPE,T13015.T_PRIORITY_CODE,T13015.T_CLINIC_DATA,T13015.T_EPISODE_NO,to_char(T13015.T_SPECIMEN_TAKEN_DATE,'dd/MM/yyyy')T_SPECIMEN_TAKEN_DATE , INITCAP(to_char(T13015.T_SPECIMEN_TAKEN_DATE,'day')) DAY_NAME, T13015.T_COMMENT_LINE,T13015.T_STATUS_CODE,T13015.T_REQUEST_NO,to_char(T13015.T_REQUEST_DATE,'dd/MM/yyyy') T_REQUEST_DATE, T13015.T_REQUEST_TIME,T13015.T_LAB_NO,T13015.T_INDICATION,T13015.T_LMP,T13015.T_SPEC_EXAM,T13015.T_COLP_FIND 
            FROM T03001,T13015 
            WHERE T13015.T_REQUEST_NO='{requestNo}' AND T03001.T_PAT_NO=T13015.T_PAT_NO";
            return QueryList<dynamic>(query);
        }
        public IEnumerable<dynamic> GetRequestT13016(string requestNo)
        {
            var query = $@"SELECT '' SL_NO ,T13016.T_WS_CODE,T13004.T_LANG2_NAME WS_NAME , T13016.T_ANALYSIS_CODE,T13011.T_LANG2_NAME ANALYSIS_NAME,
        T13016.T_TB_DIAG,T13016.T_COMMENT_LINE as COMMENTS,T13016.T_SPECIMEN_CODE,t13048.T_LANG2_NAME SPECIMEN_NAME,T13016.T_GROUP_FLAG, T13016.T_ABNO_BLE_YN,
        T13016.T_VAGIN_YN,T13016.T_IUCD_YN,T13016.T_CHEM_IRRA_YN,T13016.T_POST_MENO_YN,T13016.T_POST_PART_YN,T13016.T_HRT_YN,T13016.T_CONT_YN,T13016.T_PREG_YN,
        '1' IS_UPDATE,T13016.T_WS_CODE WS_O,T13016.T_ANALYSIS_CODE ANA_O
        from T13016,T13004,T13011,T13048 WHERE T13016.T_REQUEST_NO='{requestNo}'
        AND T13016.T_WS_CODE=T13004.T_WS_CODE AND T13016.T_ANALYSIS_CODE=T13011.T_ANALYSIS_CODE AND T13016.T_SPECIMEN_CODE=T13048.T_SITE_CODE";
            return QueryList<dynamic>(query);
        }

        public IEnumerable<dynamic> GetAnalysisRestriction(string wsCode, string anaCode)
        {
            var query = "";
            if (wsCode == "12" && anaCode == "12018")
                query = $@"select t_pretest_code,t_group_flag,t_single_flag,   t_stat_req FROM  t13011  where  t_ws_code ='{wsCode}'  AND t_analysis_code = '{anaCode}' AND t_active_flag IS NOT NULL";
            else
                query = $@"select t_pretest_code,t_group_flag,t_single_flag,'' t_stat_req FROM  t13011 where t_ws_code = '{wsCode}' AND t_analysis_code = '{anaCode}'  AND t_display_flag IS NULL AND t_active_flag IS NOT NULL";

            return QueryList<dynamic>(query);

            #region analysisCondition
            /*
             IF :priority = '1' AND :stat_req IS NULL THEN
              message('Cant request Stat for ' || :analysis_dscrptn   || ' ! Request as Routine');

            ------------------------------------
            :pretest_code IS NOT NULL THEN 
              :pretest_dscrptn := pretest(:pretest_code);


            IF :bt_block.t_ws_code = '01' AND :t_analysis_code IN (
            '0007',
            '0140'
            ) AND ltrim(rtrim(upper(:gender_dscrptn))) NOT LIKE 'M%' THEN
            user_message('S0566');
            END IF;

            IF :bt_block.t_ws_code = '01' AND :t_analysis_code IN (
            '0137'
            ) AND ltrim(rtrim(upper(:gender_dscrptn))) NOT LIKE 'F%' THEN
            user_message('S0566');
            END IF;

            IF :bt_block.t_ws_code = '22' AND :t_analysis_code = '22001' AND :t_external_flag IS NULL THEN
            get_previous_result(:cd_block.pat_no);
            END IF;

            
            record Exist 
            -------------
            select count(*)record_exists from t13016 where  t_ws_code= :t_ws_code and  t_analysis_code = :t_analysis_code and t_request_no= :request_no;
	        if record_exists > 0  and :bt_block.t_ws_code != '01' then user_message ('S0144');
            
             S0145 record cancel already
             --------------------
            select count(*) record_cancelled from   t13015 where  t_request_no    = :request_no and    t_status_code   in ('0','6');
	        if record_cancelled > 0 then user_message ('S0145');
            
             
             t_ws_code ='22' and :t_analysis_code ='22001' and :t_external_flag is null then
             SELECT T_LANG2_NAME bloodGroup,T_REQUEST_NO,t_pat_no FROM V13181 WHERE T_PAT_NO =nvl(:PAT_NO,T_PAT_NO) ORDER BY  T_ANALYSIS_CODE DESC
            message: Patient already have Blood Group with previous request#' || r_req_no ||'  ' || bloodGroup
             
             */
            #endregion
        }
        //Save data into t13015 t13016 
        public dynamic Insert13115(dynamic t13115, string empCode, string siteCode)
        {
            //return GetRequestDateTime("0011334841");
            var requestNo = (string)t13115.requestNo;
            if (String.IsNullOrEmpty(requestNo))
                requestNo = GetRequestNo();

            var t13015 = t13115.t13015;
            var requestList = t13115.requestList;

            var patno = (string)t13015[0].T_PAT_NO;
            var docCode = (string)t13015[0].T_DOC_CODE;
            var location = (string)t13015[0].T_LOCATION_CODE;
            var patType = (string)t13015[0].T_PAT_TYPE;
            var priority = (string)t13015[0].T_PRIORITY_CODE;
            var clinicData = (string)t13015[0].T_CLINIC_DATA;
            var episode = (string)t13015[0].T_EPISODE_NO;
            var sDate = (string)t13015[0].T_SPECIMEN_TAKEN_DATE;
            var specimenDate = sDate == "" ? null : DateTime.ParseExact(sDate, "dd/MM/yyyy", null).ToString("dd-MMM-yyyy");
            var commentLine = (string)t13015[0].T_COMMENT_LINE;
            var labno = (string)t13015[0].T_LAB_NO;
            var indication = (string)t13015[0].T_INDICATION;
            var lmp = (string)t13015[0].T_LMP;
            var specExam = (string)t13015[0].T_SPEC_EXAM;
            var colpFind = (string)t13015[0].T_COLP_FIND;


            //apply when need to add new in 16 

            var isInsert15 = InsertT13015(patno, docCode, location, patType, priority, clinicData, episode, specimenDate, commentLine, empCode, requestNo, labno,
                indication, lmp, specExam, colpFind);
            var isInsert16 = false;
            if (isInsert15)
            {
                foreach (var request in requestList)
                {//WS_O, ANA_O 
                    isInsert16 = InsertT13016(empCode, requestNo, (string)request.T_WS_CODE, (string)request.T_ANALYSIS_CODE, (string)request.COMMENTS,
                        (string)request.T_SPECIMEN_CODE, (string)request.T_SINGLE_FLAG, (string)request.T_GROUP_FLAG, (string)request.T_TB_DIAG,
                        (string)request.T_ABNO_BLE_YN, (string)request.T_VAGIN_YN, (string)request.T_IUCD_YN, (string)request.T_CHEM_IRRA_YN,
                        (string)request.T_POST_MENO_YN, (string)request.T_POST_PART_YN, (string)request.T_HRT_YN, (string)request.T_CONT_YN,
                        (string)request.T_PREG_YN, (string)request.T_LAB_NO, (string)request.WS_O, (string)request.ANA_O);
                    if (!isInsert16)
                    {
                        isInsert16 = false;
                        break;
                    }
                }
            }
            return isInsert16 ? GetRequestDateTime(requestNo) : null;
        }

        public string GetRequestNo()
        {
            return QueryString("select lpad(t_lab_seq.nextval,10,'0')  req_no from dual");
        }

        public bool InsertT13015(string T_PAT_NO, string T_DOC_CODE, string T_LOCATION_CODE, string T_PAT_TYPE, string T_PRIORITY_CODE, string T_CLINIC_DATA, string T_EPISODE_NO, string T_SPECIMEN_TAKEN_DATE, string T_COMMENT_LINE, string T_ENTRY_USER, string T_REQUEST_NO, string T_LAB_NO, string T_INDICATION, string T_LMP, string T_SPEC_EXAM, string T_COLP_FIND)
        {
            var ins15 = false;
            if (requestIsExist(T_REQUEST_NO))
            {
                //update 
                ins15 = Command($@"update t13015 set T_PAT_TYPE='{T_PAT_TYPE}',T_PRIORITY_CODE='{T_PRIORITY_CODE}',T_CLINIC_DATA='{T_CLINIC_DATA}',T_EPISODE_NO='{T_EPISODE_NO}',
                T_SPECIMEN_TAKEN_DATE='{T_SPECIMEN_TAKEN_DATE}',T_COMMENT_LINE='{T_COMMENT_LINE}',T_UPD_DATE=TRUNC(SYSDATE),T_UPD_USER='{T_ENTRY_USER}',
                T_LAB_NO='{T_LAB_NO}',T_INDICATION='{T_INDICATION}',T_LMP='{T_LMP}',T_SPEC_EXAM='{T_SPEC_EXAM}',T_COLP_FIND='{T_COLP_FIND}' 
                where T_REQUEST_NO='{T_REQUEST_NO}'");
            }
            else
            {
                var localIp = GetLocalIpAddress();
                ins15 = Command($@"insert into t13015(T_PAT_NO,T_DOC_CODE,T_LOCATION_CODE,T_PAT_TYPE,T_PRIORITY_CODE,T_CLINIC_DATA,T_EPISODE_NO,T_SPECIMEN_TAKEN_DATE,T_COMMENT_LINE,T_ENTRY_DATE,T_ENTRY_USER,T_STATUS_CODE,T_FORM_NAME, T_TERMINAL_ID,T_REQUEST_NO, T_REQUEST_DATE, T_REQUEST_TIME,T_LAB_NO,T_INDICATION,T_LMP,T_SPEC_EXAM,T_COLP_FIND)
                        values('{T_PAT_NO}','{T_DOC_CODE}','{T_LOCATION_CODE}','{T_PAT_TYPE}','{T_PRIORITY_CODE}','{T_CLINIC_DATA}','{T_EPISODE_NO}','{T_SPECIMEN_TAKEN_DATE}','{T_COMMENT_LINE}',trunc(sysdate),'{T_ENTRY_USER}','2','T13115','{localIp}','{T_REQUEST_NO}',trunc(sysdate),TO_CHAR(SYSDATE,'HH24MI'),'{T_LAB_NO}','{T_INDICATION}','{T_LMP}','{T_SPEC_EXAM}','{T_COLP_FIND}')");
            }
            return ins15;
        }
        public bool InsertT13016(string T_ENTRY_USER, string T_REQUEST_NO, string T_WS_CODE, string T_ANALYSIS_CODE, string T_COMMENT_LINE, string T_SPECIMEN_CODE, string T_SINGLE_FLAG, string T_GROUP_FLAG, string T_TB_DIAG, string T_ABNO_BLE_YN, string T_VAGIN_YN, string T_IUCD_YN, string T_CHEM_IRRA_YN, string T_POST_MENO_YN, string T_POST_PART_YN, string T_HRT_YN, string T_CONT_YN, string T_PREG_YN, string T_LAB_NO, string oWS, string oAna)
        {
            var ins16 = false;
            if (requestIsExist16(T_REQUEST_NO, T_WS_CODE, T_ANALYSIS_CODE))
            {
                ins16 = Command($@"update t13016 set T_UPD_USER='{T_ENTRY_USER}',T_UPD_DATE=TRUNC(SYSDATE),T_COMMENT_LINE='{T_COMMENT_LINE}',T_SPECIMEN_CODE='{T_SPECIMEN_CODE}',T_SINGLE_FLAG='{T_SINGLE_FLAG}',T_GROUP_FLAG='{T_GROUP_FLAG}',T_TB_DIAG='{T_TB_DIAG}',
                T_ABNO_BLE_YN='{T_ABNO_BLE_YN}',T_VAGIN_YN='{T_VAGIN_YN}',T_IUCD_YN='{T_IUCD_YN}',T_CHEM_IRRA_YN='{T_CHEM_IRRA_YN}',T_POST_MENO_YN='{T_POST_MENO_YN}',
                T_POST_PART_YN='{T_POST_PART_YN}',T_HRT_YN='{T_HRT_YN}',T_CONT_YN='{T_CONT_YN}',T_PREG_YN='{T_PREG_YN}',T_LAB_NO='{T_LAB_NO}',T_WS_CODE='{T_WS_CODE}', T_ANALYSIS_CODE='{T_ANALYSIS_CODE}'
                WHERE T_REQUEST_NO='{T_REQUEST_NO}' AND T_WS_CODE='{oWS}' AND T_ANALYSIS_CODE='{oAna}'");
            }
            else
            {
                ins16 = Command($@"INSERT INTO t13016 (T_ENTRY_USER,T_ENTRY_DATE,T_REQUEST_NO,T_WS_CODE,T_ANALYSIS_CODE,T_COMMENT_LINE,T_SPECIMEN_CODE,T_SINGLE_FLAG,T_GROUP_FLAG,T_TB_DIAG,T_ABNO_BLE_YN,T_VAGIN_YN,T_IUCD_YN,T_CHEM_IRRA_YN,T_POST_MENO_YN,T_POST_PART_YN,T_HRT_YN,T_CONT_YN,T_PREG_YN,T_LAB_NO)
            VALUES ('{T_ENTRY_USER}',TRUNC(SYSDATE),'{T_REQUEST_NO}','{T_WS_CODE}','{T_ANALYSIS_CODE}','{T_COMMENT_LINE}','{T_SPECIMEN_CODE}','{T_SINGLE_FLAG}','{T_GROUP_FLAG}','{T_TB_DIAG}','{T_ABNO_BLE_YN}','{T_VAGIN_YN}','{T_IUCD_YN}','{T_CHEM_IRRA_YN}','{T_POST_MENO_YN}','{T_POST_PART_YN}','{T_HRT_YN}','{T_CONT_YN}','{T_PREG_YN}','{T_LAB_NO}')");
            }
            return ins16;
        }



        public dynamic GetRequestDateTime(string reqNo)
        {
            return QuerySingle<dynamic>($"Select T_REQUEST_NO, to_char(T_REQUEST_DATE,'dd/MM/yyyy') T_REQUEST_DATE, T_REQUEST_TIME from t13015 where T_REQUEST_NO='{reqNo}'");
        }

        public bool requestIsExist(string reqNo)
        {
            var isExistReq = QuerySingle<bool>($@"SELECT  COUNT(*) t_request_no  FROM  t13015  WHERE  t_request_no = '{reqNo}'");
            return isExistReq;
        }
        public bool requestIsExist16(string reqNo, string wsCode, string analysisCode)
        {
            var isExistReq = QuerySingle<bool>($@"select  count(*) t_request_no  from  t13016  where  t_request_no = '{reqNo}' and t_ws_code='{wsCode}' and t_analysis_code='{analysisCode}'");
            return isExistReq;
        }
        public IEnumerable<dynamic> GetRequestByPatNo(string patNo)
        {
            return QueryList<dynamic>($"SELECT T_PAT_NO,T_REQUEST_NO,(TO_CHAR(T_REQUEST_DATE,'dd/MM/yyyy'))T_REQUEST_DATE,T_REQUEST_TIME FROM T13015 WHERE T_PAT_NO='{patNo}' ORDER BY  T_REQUEST_NO DESC");
        }
        public string GetLocalIpAddress()
        {
            UnicastIPAddressInformation mostSuitableIp = null;
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var network in networkInterfaces)
            {
                if (network.OperationalStatus != OperationalStatus.Up)
                    continue;
                var properties = network.GetIPProperties();
                if (properties.GatewayAddresses.Count == 0)
                    continue;
                foreach (var address in properties.UnicastAddresses)
                {
                    if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                        continue;
                    if (IPAddress.IsLoopback(address.Address))
                        continue;
                    if (!address.IsDnsEligible)
                    {
                        if (mostSuitableIp == null)
                            mostSuitableIp = address;
                        continue;
                    }
                    // The best IP is the IP got from DHCP server  
                    if (address.PrefixOrigin != PrefixOrigin.Dhcp)
                    {
                        if (mostSuitableIp == null || !mostSuitableIp.IsDnsEligible)
                            mostSuitableIp = address;
                        continue;
                    }
                    return address.Address.ToString();
                }
            }
            return mostSuitableIp != null ? mostSuitableIp.Address.ToString() : "";
        }


    }
}