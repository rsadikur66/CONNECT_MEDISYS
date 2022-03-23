using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;

namespace ConnectKsmcDAL.Transaction
{
    public class T30023DAL : CommonDAL
    {
        public IEnumerable<dynamic> GetAllPatients(string T_SITE_CODE, string T_PAT_NO, string lang)
        {
            //var query = $"SELECT K.*     FROM (SELECT T_FAMILY_LANG2_NAME T_FAMILY_NAME, T_FATHER_LANG2_NAME T_FATHER_NAME,T_GFATHER_LANG2_NAME T_GFATHER_NAME, T_FIRST_LANG2_NAME T_FIRST_NAME,  T_PAT_NO,PAT_YEAR(T_PAT_NO) YRS, PAT_Month(T_PAT_NO) MOS, PAT_NAME(T_PAT_NO, '2') T_PAT_NAME, TO_CHAR(T_BIRTH_DATE, 'DD/MM/YYYY') T_BIRTH_DATE, T03001.T_SITE_CODE, T03001.T_NTNLTY_ID, T_GENDER, T_MRTL_STATUS, T03001.T_NTNLTY_CODE, T03001.T_MOBILE_NO, T_RLGN_CODE, T_JOB_CODE, T_PASSPORT_NO, TO_CHAR(T_ID_ISSUE_DATE, 'DD/MM/YYYY') T_ID_ISSUE_DATE, T_ID_ISSUE_PLACE, T_BIRTH_PLACE, T_ADDRESS1, T_ADDRESS2, T_POSTAL_CODE, T_PHONE_HOME, T_PHONE_WORK, T_MEDICAL_FILE_NO, T_SPECIAL_CASE, T02003.T_LANG{lang}_NAME NATIONALITY, T02006.T_LANG{lang}_NAME GENDER, T02065.T_LANG{lang}_NAME SITE, T03001.T_ENTRY_DATE, E.T_USER_NAME ENTRY_USER, T03001.T_UPD_DATE, U.T_USER_NAME UPDATE_USER FROM T02003, T02006, T02065, T03001, T01009 E,T01009 U WHERE T_GENDER = T_SEX_CODE(+) AND T02003.T_NTNLTY_CODE (+)= T03001.T_NTNLTY_CODE AND T02065.T_SITE_CODE = T03001.T_SITE_CODE AND T03001.T_ENTRY_USER = E.T_EMP_CODE(+) AND T03001.T_UPD_USER = U.T_EMP_CODE(+) AND T_PAT_NO NOT LIKE 'C%' AND ('{T_SITE_CODE}' IS NULL OR T03001.T_SITE_CODE = '{T_SITE_CODE}') AND ('{T_PAT_NO}' IS NULL OR T_PAT_NO = '{T_PAT_NO}') ) K WHERE ROWNUM<1001";
            var query = $"SELECT T_FAMILY_LANG2_NAME T_FAMILY_NAME, T_FATHER_LANG2_NAME T_FATHER_NAME,T_GFATHER_LANG2_NAME T_GFATHER_NAME, T_FIRST_LANG2_NAME T_FIRST_NAME,  T_PAT_NO,PAT_YEAR(T_PAT_NO) YRS, PAT_Month(T_PAT_NO) MOS, PAT_NAME(T_PAT_NO, '2') T_PAT_NAME, TO_CHAR(T_BIRTH_DATE, 'DD/MM/YYYY') T_BIRTH_DATE, T03001.T_SITE_CODE, T03001.T_NTNLTY_ID, T_GENDER, T_MRTL_STATUS, T03001.T_NTNLTY_CODE, T03001.T_MOBILE_NO, T_RLGN_CODE, T_JOB_CODE, T_PASSPORT_NO, TO_CHAR(T_ID_ISSUE_DATE, 'DD/MM/YYYY') T_ID_ISSUE_DATE, T_ID_ISSUE_PLACE, T_BIRTH_PLACE, T_ADDRESS1, T_ADDRESS2, T_POSTAL_CODE, T_PHONE_HOME, T_PHONE_WORK, T_MEDICAL_FILE_NO, T_SPECIAL_CASE, T02003.T_LANG2_NAME NATIONALITY, T02006.T_LANG2_NAME GENDER, T02049.T_LANG2_NAME SITE, T03001.T_ENTRY_DATE, E.T_USER_NAME ENTRY_USER, T03001.T_UPD_DATE, U.T_USER_NAME UPDATE_USER FROM T02003, T02006, T02049, T03001, T01009 E,T01009 U WHERE T_GENDER = T_SEX_CODE(+) AND T02003.T_NTNLTY_CODE (+)= T03001.T_NTNLTY_CODE AND T02049.T_referral_CODE(+) = T03001.T_SITE_CODE AND T03001.T_ENTRY_USER = E.T_EMP_CODE(+) AND T03001.T_UPD_USER = U.T_EMP_CODE(+) AND T_PAT_NO NOT LIKE 'C%' AND ('{T_SITE_CODE}' IS NULL OR nvl(T03001.T_SITE_CODE,'{T_SITE_CODE}')='{T_SITE_CODE}') AND ('{T_PAT_NO}' IS NULL OR T_PAT_NO = '{T_PAT_NO}') AND t_PAT_NO in (SELECT t_pat_no FROM t07003 where t_appt_date =trunc(sysdate))";
            return QueryList<dynamic>(query);
            //t_pat_no in (SELECT t_pat_no FROM t07003 where t_appt_date =trunc(sysdate))
        }
        public dynamic GetDoc(string lang, string spclty)
        {
            var parameters = new[]
               {
                    new OracleParameter("plang", lang),
                    new OracleParameter("pspclty_code", null)
                };
            var data = ExecuteSelectProcedure("P_T30023.LOV_DOCTOR", parameters);
            return data;

        }
        public dynamic GetSpeciality(string lang)
        {
            var parameters = new[]
               {
                    new OracleParameter("plang", lang),
                };
            var data = ExecuteSelectProcedure("P_T30023.lov_speciality", parameters);
            return data;

        }
        public dynamic GetLocation(string lang, string user, string type, string doc)
        {
            var parameters = new[]
               {
                    new OracleParameter("plang", lang),
                    new OracleParameter("papplic_user", user),
                    new OracleParameter("ppat_type", type),
                    new OracleParameter("pdoc_code", doc),
                };
            var data = ExecuteSelectProcedure("P_T30023.lov_stk_loc", parameters);
            return data;

        }
        //public IEnumerable<dynamic> GetICD10(string lang)
        //{
        //    //var parameters = new[]
        //    //   {
        //    //        new OracleParameter("plang", lang)
        //    //    };
        //    //var data = ExecuteSelectProcedure("P_T30023.lov_icd10", parameters);
        //    string data = $@"SELECT icd10_code, lang_name FROM v06102";
        //    return QueryList<dynamic>(data);

        //}
        public IEnumerable<dynamic> GetICD10(string lang)
        {
            return QueryList<dynamic>($"SELECT ICD10_CODE CODE, LANG_NAME NAME FROM V06102 ORDER BY 2");
        }
        public IEnumerable<dynamic> GetSlipList(string lang, string type, string clinic, string patNo, string tempPatNo, string slip)
        {
            //var query = $"SELECT DS.* FROM (SELECT DISTINCT RANK () OVER (PARTITION BY T30023.T_PAT_MEDICINE_SEQ ORDER BY T30023.T_PAT_MEDICINE_SEQ, T30023.T_ENTRY_DATE DESC) RN, T30023.T_PAT_NO, T30023.T_PAT_MEDICINE_SEQ PHM_PAT_MEDICINE_SEQ, T30023.T_ENTRY_DATE PHM_ENTRY_DATE, T30023.T_ORGN_DOC PHM_ORGN_DOC, T_PRESC_LOC, T_APPT_REQ_ID APPT_NO, INITCAP (LTRIM(LTRIM(RTRIM (NVL (T02029.T_NAME_GIVEN,' '),' '),' ') || ' ',' ') || LTRIM(LTRIM (RTRIM (NVL (T02029.T_NAME_FATHER,' '),' ' ), ' ' ) || ' ', ' ' ) || LTRIM (RTRIM (NVL (T02029.T_NAME_FAMILY, ' '), ' ' ), ' ' ) ) DOCTOR FROM T30023, T02029 WHERE ('{patNo}' IS NULL OR T30023.T_PAT_NO = '{patNo}') AND ('{slip}' IS NULL OR T30023.T_PAT_MEDICINE_SEQ = '{slip}') AND T30023.T_ORGN_DOC = T02029.T_EMP_NO AND T_DRUG_INACTIVE_FLAG IS NULL ORDER BY TO_NUMBER (T_PAT_MEDICINE_SEQ) DESC, T30023.T_ENTRY_DATE DESC) DS WHERE DS.RN=1";
            var query = @$"SELECT T30023.T_PAT_MEDICINE_SEQ PHM_PAT_MEDICINE_SEQ, T30023.T_ENTRY_DATE PHM_ENTRY_DATE,T30023.T_ORGN_DOC PHM_ORGN_DOC, T_PRESC_LOC,T_APPT_REQ_ID APPT_NO,
                        T07003.T_VISIT_NO,INITCAP(LTRIM(LTRIM(RTRIM(NVL(T02029.T_NAME_GIVEN, ' '), ' '), ' ') || ' ', ' ') ||LTRIM(LTRIM(RTRIM(NVL(T02029.T_NAME_FATHER, ' '), ' '), ' ') || ' ', ' ') ||
                        LTRIM(RTRIM(NVL(T02029.T_NAME_FAMILY, ' '), ' '), ' ')) DOCTOR,NVL(T30023.T_DRUG_DLVRY_MTHD_CODE_DCTR, '01') T_DRUG_DLVRY_MTHD_CODE_DCTR,T_DOC_MOBILE_NO FROM T30023,T02029,T07003
                        WHERE T30023.T_ORGN_DOC = T02029.T_EMP_NO AND T30023.T_PAT_NO ='{patNo}' AND T30023.T_PAT_NO = T07003.T_PAT_NO and t07003.T_APPT_NO = t30023.T_APPT_REQ_ID
                        and T_DRUG_INACTIVE_FLAG is null group by T30023.T_PAT_MEDICINE_SEQ,T30023.T_ENTRY_DATE,T30023.T_ORGN_DOC, T_PRESC_LOC,T_APPT_REQ_ID,T07003.T_VISIT_NO,
                        INITCAP(LTRIM(LTRIM(RTRIM(NVL(T02029.T_NAME_GIVEN, ' '), ' '), ' ') || ' ', ' ') ||LTRIM(LTRIM(RTRIM(NVL(T02029.T_NAME_FATHER, ' '), ' '), ' ') || ' ', ' ') ||
                        LTRIM(RTRIM(NVL(T02029.T_NAME_FAMILY,' '),' '),' ')),NVL(T30023.T_DRUG_DLVRY_MTHD_CODE_DCTR,'01'),T_DOC_MOBILE_NO ORDER BY to_number(T_pat_medicine_seq) desc,T30023.T_ENTRY_DATE desc";
            return QueryList<dynamic>(query);
            //var parameters = new[]
            //   {
            //        new OracleParameter("plang", lang),
            //        new OracleParameter("ppat_type", type),o9i
            //        new OracleParameter("pproc_clinic", null),
            //        new OracleParameter("ppat_no", patNo),
            //        new OracleParameter("ptmp_pat_no", tempPatNo)
            //    };
            //var data = ExecuteSelectProcedure("P_T30223.lov_pat_medicine_seq", parameters);
            //return data;
        }
        public dynamic GetMedicineList(string lang)
        {
            var parameters = new[]
               {
                    new OracleParameter("plang", lang)
                };
            var data = ExecuteSelectProcedure("P_T30023.lov_drug_medication", parameters);
            return data;
        }
        public dynamic GetMedicineListbySpeciality(string lang, string speciality, string location)
        {
            var parameters = new[]
               {
                    new OracleParameter("plang", lang),
                    new OracleParameter("pspclty_code", speciality),
                    new OracleParameter("pcheck_stk_loc", location)
                };
            var data = ExecuteSelectProcedure("P_T30023.lov_drug_spclty", parameters);
            return data;
        }
        public dynamic GetMedicineListbyTrade(string lang)
        {
            var parameters = new[]
               {
                    new OracleParameter("plang", lang)
                };
            var data = ExecuteSelectProcedure("P_T30023.lov_trade", parameters);
            return data;
        }
        public dynamic GetFrequencyList(string lang)
        {
            var parameters = new[]
               {
                    new OracleParameter("plang", lang)
                };
            var data = ExecuteSelectProcedure("P_T30023.lov_dose_time_daily_frequency", parameters);
            return data;
        }
        public dynamic GetDurationList(string lang)
        {
            var parameters = new[]
               {
                    new OracleParameter("plang", lang)
                };
            var data = ExecuteSelectProcedure("P_T30023.lov_dose_duration", parameters);
            return data;
        }
        public dynamic GetUMList(string lang)
        {
            var parameters = new[]
               {
                    new OracleParameter("plang", lang)
                };
            var data = ExecuteSelectProcedure("P_T30023.lov_um", parameters);
            return data;
        }
        public dynamic GetInsList(string lang)
        {
            var parameters = new[]
               {
                    new OracleParameter("plang", lang)
                };
            var data = ExecuteSelectProcedure("P_T30023.lov_ins", parameters);
            return data;
        }
        public dynamic GetPatData(string lang, string pat, string slip)
        {
            var parameters = new[]
               {
                    new OracleParameter("plang", lang),
                    new OracleParameter("pt_pat_no", pat),
                    new OracleParameter("pt_pat_medicine_seq", slip)
                };
            var data = ExecuteSelectProcedure("P_T30023.getdata_t30023", parameters);
            return data;
        }
        public IEnumerable<dynamic> GetPatSingle(string lang, string pat, string emp)
        {
            var query = @$"SELECT (SELECT MAX (t_pat_medicine_seq) FROM t30023 WHERE t_pat_no = '{pat}') t_pat_medicine_seq,'1' hosp_code, '1001' check_stk_loc,
                   NVL (t14.appt_req_id, t10.t_hc_request_no) appt_req_id,NVL (t14.presc_loc, 'HCRE') presc_loc, '7' t_pat_type,NVL (t03.t_tmp_pat_no, t01.t_pat_no) t_pat_no,
                      NVL (t03.t_first_lang2_name,t01.t_first_lang2_name)|| ' '|| NVL (t03.t_father_lang2_name, t01.t_father_lang2_name)|| ' '
                   || NVL (t03.t_gfather_lang2_name, t01.t_gfather_lang2_name)|| ' '|| NVL (t03.t_family_lang2_name, t01.t_family_lang2_name) pat_name,
                   NVL (t03.t_birth_date, t01.t_birth_date) t_birth_date,NVL (t03.t_gender, t01.t_gender) t_gender,t06.t_lang2_name gender,
                   NVL (t03.t_ntnlty_code, t01.t_ntnlty_code) t_ntnlty_code,t203.t_lang2_name nationality,NVL (t03.t_mrtl_status, t01.t_mrtl_status) t_mrtl_status,
                   t07.t_lang2_name mrtl_status,TRUNC (  MONTHS_BETWEEN (SYSDATE,NVL (t03.t_birth_date,t01.t_birth_date))/ 12,0)|| '.'|| TRUNC (MOD (MONTHS_BETWEEN (SYSDATE,
                    NVL (t03.t_birth_date,t01.t_birth_date)),12),0) age,1 episode_no, t209.t_pregnency_yn, t209.t_pregnency_week,
                   t11.t_visit_no,NVL (t701.t_clinic_code, t42.t_loc_code) t_stk_loc_code,NVL (t701.t_clinic_name_lang2,t42.t_lang2_name) t_stk_loc_desc,
                   t62.t_weight, t62.t_height, t23.t_pat_weight,t23.t_pat_height, tdoc.doc_code, tdoc.doc_name,tdoc.spclty_code, tdoc.spclty_desc
                    FROM t25510 t10 LEFT JOIN t03003 t03 ON t10.t_pat_no = t03.t_tmp_pat_no LEFT JOIN t03001 t01 ON t10.t_pat_no = t01.t_pat_no
                   LEFT JOIN t02006 t06 ON t06.t_sex_code = NVL (t03.t_gender, t01.t_gender) 
                    LEFT JOIN t02003 t203 ON TO_NUMBER (t203.t_ntnlty_code) =NVL (t03.t_ntnlty_code, t01.t_ntnlty_code)
                   LEFT JOIN t02007 t07 ON t07.t_mrtl_status_code =NVL (t03.t_mrtl_status, t01.t_mrtl_status)
                   LEFT JOIN (SELECT   NVL (MAX (t_hc_visit_no), 1) t_visit_no,t_hc_request_no, t_pat_no FROM t25511 WHERE t_hc_request_no IS NOT NULL
                    GROUP BY t_hc_request_no, t_pat_no) t11
                   ON t11.t_pat_no = NVL (t03.t_tmp_pat_no, t01.t_pat_no)
                 AND t11.t_hc_request_no = t10.t_hc_request_no
                   LEFT JOIN
                   (SELECT   MAX (t_episode_no) max_t_episode_no, t06209.* FROM t06209 WHERE t_pregnency_yn IS NOT NULL
                    GROUP BY t_entry_user,t_entry_date,t_entry_time,t_upd_user,t_upd_date,t_pat_no,t_record_no,t_episode_type,t_episode_no,t_clinic_code,
                             t_clinic_doc_code,t_bp_systolic,t_bp_diastolic,t_bp_index,t_pulse,t_pulse_index,t_body_temp,t_body_temp_index,t_respiration_rate,
                             t_rr_index,t_weight,t_height,t_bmi, t_bmi_index,t_curcum_hand, t_gl_random, t_gl_fasting, t_gl_index, t_medical_history,
                             t_allergy_diet, t_allergy_medication, t_allergy_others, t_smoking_yn,t_stick_per_day,t_pregnency_yn,t_pregnency_week,
                             t_note,t_spo,t_recommendations,t_bp_systolic_right, t_bp_diastolic_right, t_hr,t_lmp_date, t_head_circumference_flag,
                             t_head_circumference,t_allergy_flag, t_pregnency_gravida, t_pregnency_para,t_pregnency_abortion,
                             t_prgnncy_titanus_toxoid_flag, t_titanus_toxoid_last_dose_dt, t_pain_score, t_pain_assessment_flag,
                             t_pain_assessment_others, t_vital_sign_seq, t_pregnency_lactation,  t_body_temp_in_frnht, t_height_inch,  t_height_ft
                      HAVING MAX (t_episode_no) = t_episode_no) t209
                   ON t209.t_pat_no = NVL (t03.t_tmp_pat_no, t01.t_pat_no)
                   LEFT JOIN
                   (SELECT t_order_no appt_req_id, t_rsrc_code presc_loc, t_pat_no FROM t37014
                     WHERE t_rsrc_appt_date = TRUNC (SYSDATE)) t14
                   ON t14.t_pat_no = NVL (t03.t_tmp_pat_no, t01.t_pat_no)
                   LEFT JOIN t07001 t701 ON t701.t_clinic_code = 'HCRE'
                   LEFT JOIN t02042 t42
                   ON t42.t_loc_code = 'HCRE'
                 AND 'HCRE' IN (SELECT t_ward_no FROM t05001 WHERE t_ward_no = 'HCRE')
                   LEFT JOIN(SELECT *
                      FROM (SELECT RANK () OVER (PARTITION BY t_pat_no ORDER BY t_pat_no,
                                    ROWID DESC) rn,
                                   t.t_pat_no, ROWID, t.t_weight, t.t_height
                              FROM t06209 t where t.t_pat_no='{pat}')
                     WHERE rn = 1) t62
                   ON t62.t_pat_no = NVL (t03.t_tmp_pat_no, t01.t_pat_no)
                   LEFT JOIN
                   (SELECT   MAX (t_pat_weight) t_pat_weight,
                             MAX (t_pat_height) t_pat_height, t_pat_no
                        FROM (SELECT RANK () OVER (PARTITION BY t_pat_no ORDER BY t_pat_no,
                                      t_upd_date DESC) rn,
                                     t.t_pat_no, t.t_pat_weight,
                                     t.t_pat_height, t.t_upd_date
                                FROM t30023 t where t.t_pat_no='{pat}')
                       WHERE rn = 1
                         AND (   t_pat_weight IS NOT NULL
                              OR t_pat_height IS NOT NULL
                             )
                    GROUP BY t_pat_no) t23
                   ON t23.t_pat_no = NVL (t03.t_tmp_pat_no, t01.t_pat_no)
                   LEFT JOIN
                   (SELECT t29.t_emp_no doc_code,
                              LTRIM
                                 (LTRIM (RTRIM (NVL (t29.t_name_given, ' '), ' ' ), ' ' )|| ' ', ' ')|| LTRIM 
                                 ( LTRIM (RTRIM (NVL (t29.t_name_father, ' ' ),' ' ),' ' )|| ' ', ' ' )
                           || LTRIM (RTRIM (NVL (t29.t_name_family, ' '), ' '), ' ' ) doc_name,
                           t39.t_spclty_code spclty_code,
                           t40.t_lang2_name spclty_desc
                      FROM t02029 t29 LEFT JOIN t02039 t39
                           ON t29.t_emp_no = t39.t_doc_code
                           LEFT JOIN t02040 t40
                           ON t39.t_spclty_code = t40.t_spclty_code
                           ) tdoc ON tdoc.doc_code = '{emp}'
             WHERE t10.t_pat_no = '{pat}' order by 4 desc";
            return QueryList<dynamic>(query);
            #region old
            /*var parameters = new[]
               {
                    new OracleParameter("plang", lang),
                    new OracleParameter("p_pat_no", pat),
                    new OracleParameter("p_emp", emp)

                };
            var data = ExecuteSelectProcedure("P_T30223.vldtn_PatNo", parameters);
            return data;
            */
            #endregion
        }
        public dynamic GetPatType(string pat)
        {
            var parameters = new[]
               {
                    new OracleParameter("p_pat_no", pat)
                };
            var data = ExecuteSelectProcedure("P_T30023.vldtn_pat_type_both_opd_er", parameters);
            return data;
        }
        public dynamic GetSlipValidation(string lang, string user, string doc, string pat, string slip)
        {
            var parameters = new[]
               {
                    new OracleParameter("plang", lang),
                    new OracleParameter("prc_receipt_no", slip),
                    new OracleParameter("pdoc_code", doc),
                    new OracleParameter("papplic_user", user),
                    new OracleParameter("ppat_no", pat)
                };
            var data = ExecuteSelectProcedure("P_T30023.vldtn_rc_receipt_no", parameters);
            return data;
        }
        public dynamic GetTradeGen(string lang)
        {
            var parameters = new[]
               {
                    new OracleParameter("plang", lang)
                };
            var data = ExecuteSelectProcedure("P_T30023.lov_drug_trade", parameters);
            return data;
        }
        public dynamic Save(List<dynamic> t30023, string user)
        {
            var msg = "";
            var slip = "";
            int listCount = 0;
            int t24Count = 0;
            listCount = t30023.Count();
            var pat_medicine_seq = QueryString($@"SELECT LPAD (phm_medicine_seq.NEXTVAL, 10, '0') v_pat_medicine_seq FROM DUAL");
            using (var trans = new TransactionScope())
            {
                if (listCount > 0)
                {
                    foreach (var t24 in t30023)
                    {
                        var request_no = QueryString($@"select OCM_REQUEST_no.nextval  request_no from dual");
                        var save24 = $@"INSERT INTO t30023 (t_pat_no, t_item_code, t_opd_req_item, t_dose_time_daily, t_dose_duration, t_doc_dose_unit, t_issue_um, t_qty, t_remarks, t_entry_date, t_entry_time, t_drug_inactive_flag, t_orgn_doc, t_stk_loc_code, t_receipt_code, t_dose_unit, t_pat_type, t_issue_date, t_pat_medicine_seq, t_entry_user, t_request_route, t_request_mform, t_request_sform, t_request_gcode, t_request_strength, t_diagnosis, t_morning_dose_unit, t_noon_dose_unit, t_night_dose_unit, t_morning_instruction, t_noon_instruction, t_night_instruction, t_ins_flag, t_issued_qty, t_issued_qty_old, t_qty_remaining, t_dispense_duration, t_prv_qty, t_dispense_state, t_request_id, t_print_label_flag, t_external_flag, t_tmp_pat_no, t_saturday, t_sunday, t_monday, t_tuesday, t_wednesday, t_thursday, t_friday, t_copy_prs, t_appt_req_id, t_presc_loc, t_moh_item_code, t_icd9_diag_code, t_lot_code, t_bin_code, t_lot_expiry_date, t_lot_prod_date, t_disp_episode, t_check_flag, t_check_time, t_check_by, t_drug_risk_flag, t_pat_weight, t_pat_height, t_label_notes, t_prepare_by, t_checked_by, t_preparation_date, t_iv_stability_days, t_iv_expiry_date, t_iv_fusion_time, t_iv_fusion_time_unit, t_ivdrug_strength, t_icd10_sub_code, t_icd10_main_code, t_icd10_diag_code, t_no_of_label, t_iv_preparation_time, t_iv_expiry_time, t_doctor_issue_um,T_DOC_MOBILE_NO,T_DRUG_DLVRY_MTHD_CODE_DCTR ) VALUES ('{t24.pt_pat_no}', '{t24.pt_item_code}', '{t24.pt_opd_req_item}', '{t24.pt_dose_time_daily}', '{t24.pt_dose_duration}', '{t24.pt_doc_dose_unit}', '{t24.pt_issue_um}', '{t24.pt_qty}', '{t24.pt_remarks}', TRUNC (SYSDATE), TO_CHAR (SYSDATE, 'HH24MI'),  '{t24.pt_drug_inactive_flag}', '{t24.pt_orgn_doc}', '{t24.pt_stk_loc_code}', '{t24.pt_receipt_code}', '{t24.pt_dose_unit}', '{t24.pt_pat_type}', '{t24.pt_issue_date}', '{pat_medicine_seq}', '{user}', '{t24.pt_request_route}', '{t24.pt_request_mform}', '{t24.pt_request_sform}', '{t24.pt_request_gcode}', '{t24.pt_request_strength}', '{t24.pt_diagnosis}', '{t24.pt_morning_dose_unit}', '{t24.pt_noon_dose_unit}', '{t24.pt_night_dose_unit}', '{t24.pt_morning_instruction}', '{t24.pt_noon_instruction}', '{t24.pt_night_instruction}', '{t24.pt_ins_flag}', '{t24.pt_issued_qty}', '{t24.pt_issued_qty_old}', '{t24.pt_qty_remaining}', '{t24.pt_dispense_duration}', '{t24.pt_prv_qty}', '{t24.pt_dispense_state}', '{request_no}', '{t24.pt_print_label_flag}', '{t24.pt_external_flag}', '{t24.pt_tmp_pat_no}', '{t24.pt_saturday}', '{t24.pt_sunday}', '{t24.pt_monday}', '{t24.pt_tuesday}', '{t24.pt_wednesday}', '{t24.pt_thursday}', '{t24.pt_friday}', '{t24.pt_copy_prs}', '{t24.pt_appt_req_id}', '{t24.pt_presc_loc}', '{t24.pt_moh_item_code}', '{t24.pt_icd9_diag_code}', '{t24.pt_lot_code}', '{t24.pt_bin_code}', '{t24.pt_lot_expiry_date}', '{t24.pt_lot_prod_date}', '{t24.pt_disp_episode}', '{t24.pt_check_flag}', '{t24.pt_check_time}', '{t24.pt_check_by}', '{t24.pt_drug_risk_flag}', '{t24.pt_pat_weight}', '{t24.pt_pat_height}', '{t24.pt_label_notes}', '{t24.pt_prepare_by}', '{t24.pt_checked_by}', '{t24.pt_preparation_date}', '{t24.pt_iv_stability_days}', '{t24.pt_iv_expiry_date}', '{t24.pt_iv_fusion_time}', '{t24.pt_iv_fusion_time_unit}', '{t24.pt_ivdrug_strength}', '{t24.pt_icd10_sub_code}', '{t24.pt_icd10_main_code}', '{t24.pt_icd10_diag_code}', '{t24.pt_no_of_label}', '{t24.pt_iv_preparation_time}', '{t24.pt_iv_expiry_time}', '{t24.pt_doctor_issue_um}', '{t24.T_DOC_MOBILE_NO}', '{t24.T_DRUG_DLVRY_MTHD_CODE_DCTR}' )";
                        if (Command(save24))
                            t24Count++;
                    }
                }
                if (t24Count == listCount)
                {
                    trans.Complete();
                }
            }
            msg = t24Count == listCount ? "Data Saved Successfully" : "Data not Saved";
            slip = t24Count == listCount ? pat_medicine_seq : "";
            var query = $@"SELECT '{msg}' MSG,'{slip}' SLIP  FROM DUAL";
            return QuerySingle<dynamic>(query);
        }
        public dynamic Update(List<dynamic> t30023, string user)
        {
            var msg = "";
            var slip = "";
            int listCount = 0;
            int t24Count = 0;
            listCount = t30023.Count();
            using (var trans = new TransactionScope())
            {
                if (listCount > 0)
                {
                    foreach (var t24 in t30023)
                    {
                        if (t24.prowid != null)
                        {
                            if (string.IsNullOrEmpty(t24.pt_issue_date) && DateTime.Parse(t24.pt_entry_date) == DateTime.Today && !string.IsNullOrEmpty(t24.pt_drug_inactive_flag))
                            {
                                //var update24 = $@"UPDATE t30023 set t_pat_weight='{t24.pt_pat_weight}',t_pat_height='{t24.pt_pat_height}',t_orgn_doc='{t24.pt_orgn_doc}',t_stk_loc_code='{t24.pt_stk_loc_code}',t_diagnosis='{t24.pt_diagnosis}',t_appt_req_id='{t24.pt_appt_req_id}',t_icd10_diag_code='{t24.pt_icd10_diag_code}',t_icd10_diag_code='{t24.pt_icd10_diag_code}',t_presc_loc='{t24.pt_presc_loc}',T_DOC_MOBILE_NO='{t24.T_DOC_MOBILE_NO}',
                                //                T_DRUG_DLVRY_MTHD_CODE_DCTR='{t24.T_DRUG_DLVRY_MTHD_CODE_DCTR}',t_opd_req_item='{t24.pt_opd_req_item}',t_dose_time_daily='{t24.pt_dose_time_daily}',t_dose_duration='{t24.pt_dose_duration}',
                                //                t_morning_dose_unit ='{t24.pt_morning_dose_unit}',t_noon_dose_unit = '{t24.pt_noon_dose_unit}',t_night_dose_unit = '{t24.pt_night_dose_unit}',t_morning_instruction = '{t24.pt_morning_instruction}',
                                //                t_noon_instruction = '{t24.pt_noon_instruction}',t_night_instruction = '{t24.pt_night_instruction}',t_issue_um = '{t24.pt_issue_um}',t_doctor_issue_um = '{t24.pt_doctor_issue_um}',
                                //                t_qty = '{t24.pt_qty}',t_qty_remaining ='{t24.pt_qty_remaining}',t_remarks = '{t24.pt_remarks}',t_doc_dose_unit = '{t24.pt_doc_dose_unit}',t_dose_unit = '{t24.pt_dose_unit}',
                                //                t_moh_item_code = '{t24.pt_moh_item_code}',t_request_strength = '{t24.pt_request_strength}',t_request_route = '{t24.pt_request_route}',t_request_mform = '{t24.pt_request_mform}',
                                //                t_request_sform = '{t24.pt_request_sform}',t_request_gcode ='{t24.pt_request_gcode}',t_drug_inactive_flag = '{t24.pt_drug_inactive_flag}',t_saturday = '{t24.pt_saturday}',
                                //                t_sunday = '{t24.pt_sunday}',t_monday = '{t24.pt_monday}',t_tuesday = '{t24.pt_tuesday}',t_wednesday = '{t24.pt_wednesday}',t_thursday = '{t24.pt_thursday}',t_friday = '{t24.pt_friday}' 
                                //                where t_pat_no='{t24.pt_pat_no}' and t_pat_medicine_seq='{t24.pt_pat_medicine_seq}' and rowid='{t24.prowid}'";
                                var update24 = $@"UPDATE t30023 set t_drug_inactive_flag = '{t24.pt_drug_inactive_flag}' where t_pat_no='{t24.pt_pat_no}' and t_pat_medicine_seq='{t24.pt_pat_medicine_seq}' and rowid='{t24.prowid}'";
                                if (Command(update24))
                                    t24Count++;
                            }
                            else
                            {
                                t24Count++;
                            }
                        }
                        else
                        {
                            var request_no = QueryString($@"select OCM_REQUEST_no.nextval  request_no from dual");
                            var save24 = $@"INSERT INTO t30023 (t_pat_no,t_item_code,t_opd_req_item,t_dose_time_daily,t_dose_duration,t_doc_dose_unit,t_issue_um,t_qty,t_remarks,t_entry_date,t_entry_time,t_drug_inactive_flag,t_orgn_doc,t_stk_loc_code,t_receipt_code,t_dose_unit,t_pat_type,t_issue_date,t_pat_medicine_seq,t_entry_user,t_request_route,t_request_mform,t_request_sform,t_request_gcode,t_request_strength,t_diagnosis,t_morning_dose_unit,t_noon_dose_unit,t_night_dose_unit,t_morning_instruction,t_noon_instruction,t_night_instruction,t_ins_flag,t_issued_qty,t_issued_qty_old,t_qty_remaining,t_dispense_duration,t_prv_qty,t_dispense_state,t_request_id,t_print_label_flag,t_external_flag,t_tmp_pat_no,t_saturday,t_sunday,t_monday,t_tuesday,t_wednesday,t_thursday,t_friday,t_copy_prs,t_appt_req_id, t_presc_loc, t_moh_item_code, t_icd9_diag_code, t_lot_code, t_bin_code, t_lot_expiry_date, t_lot_prod_date, t_disp_episode, t_check_flag, t_check_time, t_check_by, t_drug_risk_flag, t_pat_weight, t_pat_height, t_label_notes, t_prepare_by, t_checked_by, t_preparation_date, t_iv_stability_days, t_iv_expiry_date, t_iv_fusion_time, t_iv_fusion_time_unit, t_ivdrug_strength, t_icd10_sub_code, t_icd10_main_code, t_icd10_diag_code, t_no_of_label, t_iv_preparation_time, t_iv_expiry_time, t_doctor_issue_um ,T_DOC_MOBILE_NO,T_DRUG_DLVRY_MTHD_CODE_DCTR) VALUES ('{t24.pt_pat_no}', '{t24.pt_item_code}', '{t24.pt_opd_req_item}', '{t24.pt_dose_time_daily}', '{t24.pt_dose_duration}', '{t24.pt_doc_dose_unit}', '{t24.pt_issue_um}', '{t24.pt_qty}', '{t24.pt_remarks}', TRUNC (SYSDATE),TO_CHAR (SYSDATE, 'HH24MI'),  '{t24.pt_drug_inactive_flag}', '{t24.pt_orgn_doc}', '{t24.pt_stk_loc_code}', '{t24.pt_receipt_code}', '{t24.pt_dose_unit}', '{t24.pt_pat_type}', '{t24.pt_issue_date}', '{t24.pt_pat_medicine_seq}', '{user}', '{t24.pt_request_route}', '{t24.pt_request_mform}', '{t24.pt_request_sform}', '{t24.pt_request_gcode}', '{t24.pt_request_strength}', '{t24.pt_diagnosis}', '{t24.pt_morning_dose_unit}', '{t24.pt_noon_dose_unit}', '{t24.pt_night_dose_unit}', '{t24.pt_morning_instruction}', '{t24.pt_noon_instruction}', '{t24.pt_night_instruction}', '{t24.pt_ins_flag}', '{t24.pt_issued_qty}', '{t24.pt_issued_qty_old}', '{t24.pt_qty_remaining}', '{t24.pt_dispense_duration}', '{t24.pt_prv_qty}', '{t24.pt_dispense_state}', '{request_no}', '{t24.pt_print_label_flag}', '{t24.pt_external_flag}', '{t24.pt_tmp_pat_no}', '{t24.pt_saturday}', '{t24.pt_sunday}', '{t24.pt_monday}', '{t24.pt_tuesday}', '{t24.pt_wednesday}', '{t24.pt_thursday}', '{t24.pt_friday}', '{t24.pt_copy_prs}', '{t24.pt_appt_req_id}', '{t24.pt_presc_loc}', '{t24.pt_moh_item_code}', '{t24.pt_icd9_diag_code}', '{t24.pt_lot_code}', '{t24.pt_bin_code}', '{t24.pt_lot_expiry_date}', '{t24.pt_lot_prod_date}', '{t24.pt_disp_episode}', '{t24.pt_check_flag}', '{t24.pt_check_time}', '{t24.pt_check_by}', '{t24.pt_drug_risk_flag}', '{t24.pt_pat_weight}', '{t24.pt_pat_height}', '{t24.pt_label_notes}', '{t24.pt_prepare_by}', '{t24.pt_checked_by}', '{t24.pt_preparation_date}', '{t24.pt_iv_stability_days}', '{t24.pt_iv_expiry_date}', '{t24.pt_iv_fusion_time}', '{t24.pt_iv_fusion_time_unit}', '{t24.pt_ivdrug_strength}', '{t24.pt_icd10_sub_code}', '{t24.pt_icd10_main_code}', '{t24.pt_icd10_diag_code}', '{t24.pt_no_of_label}', '{t24.pt_iv_preparation_time}', '{t24.pt_iv_expiry_time}', '{t24.pt_doctor_issue_um}','{t24.T_DOC_MOBILE_NO}','{t24.T_DRUG_DLVRY_MTHD_CODE_DCTR}' )";
                            if (Command(save24))
                                t24Count++;
                        }
                    }
                }
                if (t24Count == listCount)
                {
                    trans.Complete();
                }
            }
            msg = t24Count == listCount ? "Data Updated Successfully" : "Data not Updated";
            slip = t24Count == listCount ? "1" : "";
            var query = $@"SELECT '{msg}' MSG,'{slip}' SLIP  FROM DUAL";
            return QuerySingle<dynamic>(query);
        }
        public string GetSpecialityIns(string docCode, string specCode, string genCode, string routeCode, string mFormCode)
        {
            var data = QueryString($@"SELECT T30_DRUG_SPCLTY_INSTRUCTION('{docCode}', '{specCode}','{genCode}','{routeCode}','{mFormCode}') code FROM DUAL");
            return data;
        }
        //used in q30223
        public IEnumerable<dynamic> GetPrescriptionData(string patNo, string slipNo, string fDate, string tDate, string lang)
        {
            var data = $@"SELECT DISTINCT T30014.T_PAT_MEDICINE_SEQ SLIP_NO,T03001.T_PAT_NO,PAT_NAME(T03001.T_PAT_NO,'{lang}') T_PAT_NAME,PAT_YEAR(T03001.T_PAT_NO) YEARS,
                        PAT_MONTH(T03001.T_PAT_NO) MONTHS,PAT_GENDER(T03001.T_PAT_NO, 2) T_GENDER,TO_CHAR(T30014.T_ENTRY_DATE,'dd/MM/yyyy') T_ENTRY_DATE , 
                        (SELECT T_NAME_GIVEN ||' ' ||T_NAME_FAMILY FROM T02029 WHERE T_EMP_NO=T_ORGN_DOC) DOC_NAME,(SELECT T02040.T_LANG{lang}_NAME FROM T02040,T02039 
                        WHERE T02039.T_DOC_CODE=T_ORGN_DOC AND T02039.T_SPCLTY_CODE=T02040.T_SPCLTY_CODE) SPC_NAME FROM T03001,T30023 T30014 WHERE T30014.T_PAT_NO=T03001.T_PAT_NO 
                        AND (T30014.T_ENTRY_DATE BETWEEN NVL(TO_DATE('{fDate}','dd/MM/yyyy'),T30014.T_ENTRY_DATE) AND NVL(TO_DATE('{tDate}','dd/MM/yyyy'),
                        T30014.T_ENTRY_DATE) AND T30014.T_PAT_NO=NVL('{patNo}',T30014.T_PAT_NO) AND T30014.T_PAT_MEDICINE_SEQ=NVL('{slipNo}',T30014.T_PAT_MEDICINE_SEQ))";
            return QueryList<dynamic>(data);
        }
        public IEnumerable<dynamic> GetPatInfoT03001(string patNo, string lang)
        {
            var data = $@"SELECT DISTINCT T03001.T_PAT_NO,PAT_NAME(T03001.T_PAT_NO,'{lang}') T_PAT_NAME,PAT_YEAR(T03001.T_PAT_NO) YEARS,
            PAT_MONTH(T03001.T_PAT_NO) MONTHS,PAT_GENDER(T03001.T_PAT_NO, {lang}) T_GENDER,PAT_NATIONALITY(T03001.T_PAT_NO,'{lang}')NATIONALITY,
            T07003.T_CLINIC_CODE, T07003.T_CLINIC_DOC_CODE,T07003.T_APPT_DATE,
            T_NAME_GIVEN||' '||T_NAME_FATHER ||' ' ||T_NAME_FAMILY DOC_NAME, T_NAME_GIVEN_ARB||' '||T_NAME_FATHER_ARB||' '||T_NAME_FAMILY_ARB DOC_NAME_A,
            T02039.T_SPCLTY_CODE, T02040.T_LANG{lang}_NAME SPCLTY,T07001.T_CLINIC_NAME_LANG{lang} CLINIC_NAME,T06209.T_HEIGHT,T06209.T_WEIGHT,T_VISIT_NO,T_APPT_NO,'2' T_TYPE_CODE
            FROM T03001,T07003,T02029,T02039,T02040,T07001,T06209 
            WHERE T03001.T_PAT_NO='{patNo}' AND T03001.T_PAT_NO=T07003.T_PAT_NO AND T07003.T_APPT_DATE=TRUNC(SYSDATE)
            AND  T_EMP_NO=T07003.T_CLINIC_DOC_CODE AND T07003.T_CLINIC_DOC_CODE=T02039.T_DOC_CODE AND T02039.T_SPCLTY_CODE(+)=T02040.T_SPCLTY_CODE
            AND T07003.T_CLINIC_CODE=T07001.T_CLINIC_CODE AND T03001.T_PAT_NO =T06209.T_PAT_NO(+) AND nvl(T06209.T_EPISODE_NO,0)=(select nvl(max(T_EPISODE_NO),0) from T06209 WHERE T_PAT_NO='{patNo}')";

            dynamic result = QueryList<dynamic>(data);
            if (result.Count == 0)
            {
                data = $@"SELECT DISTINCT T03001.T_PAT_NO,PAT_NAME(T03001.T_PAT_NO,'{lang}') T_PAT_NAME,PAT_YEAR(T03001.T_PAT_NO) YEARS,'' T_VISIT_NO,'' T_APPT_NO,
                PAT_MONTH(T03001.T_PAT_NO) MONTHS,PAT_GENDER(T03001.T_PAT_NO, '{lang}') T_GENDER,PAT_NATIONALITY(T03001.T_PAT_NO,'{lang}')NATIONALITY,'2' T_TYPE_CODE,
                '' T_CLINIC_CODE, '' T_CLINIC_DOC_CODE,'' T_APPT_DATE,'' DOC_NAME ,'' DOC_NAME_A, '' T_SPCLTY_CODE,'' SPCLTY,'' CLINIC_NAME,'' T_HEIGHT,'' T_WEIGHT  
                FROM T03001 WHERE T03001.T_PAT_NO='{patNo}'";
                result = QueryList<dynamic>(data);
            }
            return result;
        }
        public dynamic GetMedicineStatus(string itemCode, string strength, string routeCode, string formCode, string genCode)
        {
            var data = $@"SELECT (select distinct t_drug_risk_flag from t30006 where t_moh_item_code='{itemCode}' and t_strength='{strength}' and t_route_code='{routeCode}'
                        and t_drug_form_code='{formCode}') IS_RISK,(select T_ANTIOBIOTIC_FLAG from t30004 where T_GEN_CODE='{genCode}') IS_ANTIOBIOTIC FROM DUAL";
            return QuerySingle<dynamic>(data);
        }
        public string GetValitionInfo(string patNo)
        {
            IEnumerable<dynamic> data = QueryList<dynamic>($@"SELECT T_VISIT_NO,T_ICD10_MAIN_CODE||' '||T_ICD10_SUB_CODE,T_APPT_NO,T_CLINIC_CODE,PAT_YEAR('{patNo}') T_AGE FROM T07003 WHERE 
                                        T_PAT_NO='{patNo}' AND TRUNC(T_APPT_DATE)=TRUNC(SYSDATE) AND T_ARRIVAL_STATUS='1' AND T_PAT_NO IS NOT NULL
                                        AND T_APPT_NO IS NOT NULL");
            IEnumerable<dynamic> dataOPD = QueryList<dynamic>($@"SELECT * FROM T05010 WHERE T_PAT_NO='{patNo}' AND (T_DISCHARGE_DATE IS NULL OR T_DISCHARGE_DATE=TRUNC(SYSDATE))
                                          AND T_EPISODE_NO IN (SELECT MAX(T_EPISODE_NO) FROM T05010 WHERE T_PAT_NO='{patNo}')");
            IEnumerable<dynamic> dataER = QueryList<dynamic>($@"SELECT * FROM T04007 WHERE T_PAT_NO='{patNo}' AND (T_EPISODE_END_DATE IS NULL OR T_EPISODE_END_DATE=TRUNC(SYSDATE))");

            if (data.Count() == 0)
            {
                return "1";//1=Caution....This Patient is Admitted or not valid!!!
            }
            if (data.ToList()[0].T_AGE > 200)
            {
                return "2";//2=Entry of this Birth Date will cause an improbable age (over 200 yrs)./صحح إدخال تاريخ الميلاد
            }
            else if (data.ToList().Count() > 0 && dataOPD.ToList().Count() > 0)
            {
                return "3";//3=This Patient can be Prescribed as OPD or Discharge-IP. If you want to Prescribe as Discharge-IP than Please Change the Patient Type !!
            }
            else if (data.ToList().Count() > 0 && dataER.ToList().Count() > 0)
            {
                return "4";//4=This Patient can be Prescribed as OPD or ER Patient. If you want to Prescribe as ER than Please Change the Patient Type !!
            }
            return "0";//0=valid
        }
        public dynamic GetDrugHistoryAsOutPaient(string patNo, string lang)
        {
            var data = $@"SELECT T23.ROWID,T23.T_PAT_NO,T23.T_ITEM_CODE,T23.T_OPD_REQ_ITEM,T23.T_DOSE_TIME_DAILY,T09.T_LANG{lang}_NAME DOSE_TIME_DAILY_DESC,
                   T23.T_DOSE_DURATION,T30010.T_LANG{lang}_NAME DOSE_DURATION_DESC,T23.T_DOC_DOSE_UNIT, T23.T_ISSUE_UM,T30036.T_UM_SHORT_DESC_ARB ISSUE_UM_DESC,
                   T23.T_QTY,T_REMARKS,T23.T_ENTRY_DATE, T23.T_ENTRY_TIME,T23.T_DRUG_INACTIVE_FLAG,T23.T_UPD_DATE,T23.T_UPD_USER, T23.T_ORGN_DOC,
                   T23.T_STK_LOC_CODE, T23.T_RECEIPT_CODE,T23.T_DOSE_UNIT,T23.T_PAT_TYPE,T23.T_ISSUE_DATE,T23.T_PAT_MEDICINE_SEQ,T23.T_ENTRY_USER,T23.T_REQUEST_ROUTE,
                   T23.T_REQUEST_MFORM, T23.T_REQUEST_SFORM,T23.T_REQUEST_GCODE, T23.T_REQUEST_STRENGTH,T23.T_DIAGNOSIS, T23.T_MORNING_DOSE_UNIT,T23.T_NOON_DOSE_UNIT, 
                   T23.T_NIGHT_DOSE_UNIT,T23.T_MORNING_INSTRUCTION,T23.T_NOON_INSTRUCTION,T23.T_NIGHT_INSTRUCTION, T23.T_INS_FLAG,T23.T_ISSUED_QTY,T23.T_ISSUED_QTY_OLD,
                   T23.T_QTY_REMAINING, T23.T_DISPENSE_DURATION,T23.T_PRV_QTY,T23.T_DISPENSE_STATE,T23.T_REQUEST_ID,T23.T_PRINT_LABEL_FLAG,T23.T_EXTERNAL_FLAG,
                   T23.T_TMP_PAT_NO,T23.T_SATURDAY,T23.T_SUNDAY,T23.T_MONDAY,T23.T_TUESDAY,T23.T_WEDNESDAY,T23.T_THURSDAY,T23.T_FRIDAY, T23.T_COPY_PRS,T23.T_APPT_REQ_ID,
                   T23.T_PRESC_LOC, T23.T_MOH_ITEM_CODE FROM T30023 T23,T30009 T09,T30010,T30036,T30006,T30042,T30022 MOR,T30022 NOO,T30022 NIG
                   WHERE T23.T_DOSE_TIME_DAILY = T09.T_FREQUENCY_CODE(+) AND T23.T_DOSE_DURATION = T30010.T_DOSE_DURATION_CODE(+)
                   AND T23.T_ISSUE_UM = T30036.T_UM(+) AND T23.T_ITEM_CODE = T30042.T_ITEM_CODE(+) AND T30042.T_DRUG_MASTER_CODE = T30006.T_DRUG_MASTER_CODE(+)
                   AND T23.T_MORNING_INSTRUCTION = MOR.T_INSTRUCTION_CODE(+) AND T23.T_NOON_INSTRUCTION = NOO.T_INSTRUCTION_CODE(+)
                   AND T23.T_NIGHT_INSTRUCTION = NIG.T_INSTRUCTION_CODE(+) AND T_PAT_NO ='{patNo}'
                   AND T23.T_DRUG_INACTIVE_FLAG IS NULL ORDER BY T23.T_ENTRY_DATE DESC";
            return QueryList<dynamic>(data);
        }
        public dynamic GetDrugHistoryAsInPaient(string patNo, string lang)
        {
            string langArb = lang == "1" ? "_ARB" : "";
            var data = $@"SELECT T23.ROWID,T30004.T_LANG2_NAME||' '||T23.T_REQUEST_STRENGTH||' '||T30004.T_LANG2_NAME||' '||T30001.T_LANG2_NAME T_OPD_REQ_ITEM,
                          T23.T_ENTRY_DATE, T23.T_ENTRY_TIME,T23.T_DOSE_TIME_DAILY,T09.T_LANG{lang}_NAME DOSE_TIME_DAILY_DESC,t23.T_REQUEST_DOC,T_REMARKS,
                          T_NAME_GIVEN{langArb}||' '||T_NAME_FAMILY{langArb} DOC_NAME,T23.T_DOSE_DURATION,T30010.T_LANG{lang}_NAME DOSE_DURATION_DESC,T_STOP_FRM 
                          FROM T30014 T23,T30009 T09,T30010,T30006,T30042,T30002,T30004,T30001,T02029                  
                          WHERE T30002.T_DRUG_ROUTE_CODE=t23.T_REQUEST_ROUTE and T30004.T_GEN_CODE=t23.T_REQUEST_GCODE AND T30001.T_DRUG_FORM_CODE=t23.T_REQUEST_MFORM 
                          AND T02029.T_EMP_NO=t23.T_REQUEST_DOC and T23.T_DOSE_TIME_DAILY=T09.T_FREQUENCY_CODE(+) AND T23.T_DOSE_DURATION=T30010.T_DOSE_DURATION_CODE(+)
                          AND T23.T_REQUEST_GCODE = T30042.T_ITEM_CODE(+) AND T30042.T_DRUG_MASTER_CODE=T30006.T_DRUG_MASTER_CODE(+) AND T_PAT_NO ='{patNo}'
                          AND T23.T_DRUG_INACTIVE_FLAG IS NULL ORDER BY T23.T_ENTRY_DATE DESC,T23.T_ENTRY_TIME DESC";
            return QueryList<dynamic>(data);
        }
        public DataTable GetReportHeader()
        {
            var query = $@"SELECT T_SITE_CODE ,T_COUNTRY_LANG1_NAME , T_COUNTRY_LANG2_NAME , T_MIN_LANG1_NAME , T_MIN_LANG2_NAME , T_SITE_LANG1_NAME , T_SITE_LANG2_NAME , T_LOGO_ID , T_LOGO_NAME , T_LOGO , t_lcence_no FROM t01028 WHERE t_site_code IN (SELECT t_site_code FROM t01001 )";
            return ReportQuery(query);
        }
        public DataTable GetInstructionName(string Language, string T_INSTRUCTION_CODE)
        {
            var query = $@"SELECT T_LANG{Language}_NAME T_INSTRUCTION_NAME FROM T30022 WHERE T_INSTRUCTION_CODE='{T_INSTRUCTION_CODE}'";
            return ReportQuery(query);
        }
        public DataTable GetReportPatInfo(string sitecode, string patNo)
        {
            var query = $@"SELECT T_PAT_NO, T_PAT_NAME, PAT_NAME_EN, PAT_NAME_AR, MAX(T_EPISODE_NO) T_EPISODE_NO, T_GENDER, MONTHS, YEARS, T_BIRTH_DATE, ICUDAY, T_NATIONALITY, T_WARD_NO, T_WARD_DESC, T_BED_DESC, T_ROOM_NO, T_ADMIT_DATE, T_ADMIT_TIME, T_ADMIT_DOC_CODE, Doc_Name, SPC_NAME, T_PROTOCOL_NO, T_MERITAL_STATUS, T_GENDER_CODE FROM (SELECT T03001.T_PAT_NO, PAT_NAME(T03001.T_PAT_NO, '2') T_PAT_NAME, LTRIM(LTRIM(RTRIM(NVL(T03001.T_FIRST_LANG1_NAME, ''), ' '), ' ') || ' ', ' ') || LTRIM(LTRIM(RTRIM(NVL(T03001.T_FATHER_LANG1_NAME, ''), ' '), ' ') || ' ', ' ') || LTRIM(RTRIM(NVL(T03001.T_FAMILY_LANG1_NAME, ''), ' '), ' ') PAT_NAME_AR, LTRIM(LTRIM(RTRIM(NVL(T03001.T_FIRST_LANG2_NAME, ''), ' '), ' ') || ' ', ' ') || LTRIM(LTRIM(RTRIM(NVL(T03001.T_FATHER_LANG2_NAME, ''), ' '), ' ') || ' ', ' ') || LTRIM(RTRIM(NVL(T03001.T_FAMILY_LANG2_NAME, ''), ' '), ' ') PAT_NAME_EN ,NULL T_EPISODE_NO, PAT_GENDER('{patNo}', 2) T_GENDER, T_GENDER T_GENDER_CODE, PAT_YEAR('{patNo}') YEARS, PAT_MONTH('{patNo}') MONTHS, TO_CHAR(T03001.T_BIRTH_DATE, 'dd/mm/yyyy') T_BIRTH_DATE, NULL ICUDAY, PAT_NATIONALITY('{patNo}', 2) T_NATIONALITY, NULL T_WARD_NO, NULL T_WARD_DESC, NULL T_BED_DESC, NULL T_ROOM_NO, NULL T_ADMIT_DATE, NULL T_ADMIT_TIME, T_ORGN_DOC T_ADMIT_DOC_CODE, NULL T_PROTOCOL_NO, T02007.T_LANG2_NAME T_MERITAL_STATUS ,t02029.T_NAME_GIVEN || ' ' || t02029.T_NAME_FAMILY Doc_Name ,T02040.T_LANG2_NAME SPC_NAME FROM T03001 LEFT JOIN T30023 ON T03001.T_PAT_NO=T30023.T_PAT_NO LEFT JOIN T02007 ON T03001.T_MRTL_STATUS = T02007.T_MRTL_STATUS_CODE LEFT JOIN t02029 ON t02029.t_emp_no = T30023.T_ORGN_DOC LEFT JOIN T02040 ON T02040.T_SPCLTY_CODE= T30023.T_STK_LOC_CODE WHERE T30023.T_PAT_NO='{patNo}' ) GROUP BY T_PAT_NO, T_PAT_NAME, PAT_NAME_EN, PAT_NAME_AR, T_GENDER, MONTHS, YEARS, T_BIRTH_DATE, ICUDAY, T_NATIONALITY, T_WARD_NO, T_WARD_DESC, T_BED_DESC, T_ROOM_NO, T_ADMIT_DATE, T_ADMIT_TIME, T_ADMIT_DOC_CODE, Doc_Name, SPC_NAME, T_PROTOCOL_NO, T_MERITAL_STATUS, T_GENDER_CODE";
            return ReportQuery(query);
        }
        public DataTable GetReportMedicineListBySlipNo(string lang, string site, string patSeq, string patNo)
        {
            var query = $@"SELECT LTRIM(LTRIM(RTRIM(NVL(T03001.T_FIRST_LANG1_NAME, ''), ' '), ' ') || ' ', ' ') || LTRIM(LTRIM(RTRIM(NVL(T03001.T_FATHER_LANG1_NAME, ''), ' '), ' ') || ' ', ' ') || LTRIM(RTRIM(NVL(T03001.T_FAMILY_LANG1_NAME, ''), ' '), ' ') PAT_NAME_AR, 
            LTRIM(LTRIM(RTRIM(NVL(T03001.T_FIRST_LANG2_NAME, ''), ' '), ' ') || ' ', ' ') || LTRIM(LTRIM(RTRIM(NVL(T03001.T_FATHER_LANG2_NAME, ''), ' '), ' ') || ' ', ' ') || LTRIM(RTRIM(NVL(T03001.T_FAMILY_LANG2_NAME, ''), ' '), ' ') PAT_NAME_EN, 
            PAT_GENDER('{patNo}', 2) T_GENDER, TO_NUMBER(TRUNC(MONTHS_BETWEEN(SYSDATE ,T03001.T_BIRTH_DATE)/12 ,0)) ||'.' || TO_NUMBER(TRUNC(MOD(MONTHS_BETWEEN(SYSDATE , T03001.T_BIRTH_DATE) ,12) ,0)) YRS, PAT_NATIONALITY('{patNo}', 2) T_NATIONALITY, 
            DECODE(T03001.T_X_PAT_TYPE,'3','NO','Yes') BUSINESSCENTRE, T30023.T_PAT_HEIGHT, T30023.T_PAT_WEIGHT, T30023.T_DIAGNOSIS, T30023.T_ORGN_DOC, LTRIM(LTRIM(RTRIM(NVL(T02029.T_NAME_GIVEN, ''), ' '), ' ') || ' ', ' ') || LTRIM(LTRIM(RTRIM(NVL(T02029.T_NAME_FATHER, ''), ' '), ' ') || ' ', ' ') || LTRIM(RTRIM(NVL(T02029.T_NAME_FAMILY, ''), ' '), ' ') T_ORGN_DOC_DESC , 
            T30023.T_STK_LOC_CODE, T02040.T_LANG2_NAME SPCLTY_DESC, T30023.T_PRESC_LOC T_LOC_CODE, T07.T_CLINIC_NAME_LANG2 T_LOC_DESC, T30023.T_ICD10_DIAG_CODE, V06102.LANG_NAME T_ICD10_DIAG_DESC, T30023.T_PAT_MEDICINE_SEQ, T30023.T_OPD_REQ_ITEM T_MEDICINE_NAME, T30009.T_LANG2_NAME T_FREQUENCY_NAME, T30010.T_LANG2_NAME T_DOSE_DURATION_NAME, 
            TO_CHAR(T30023.T_ENTRY_DATE,'DD/MM/YY') STARTDATE , DECODE(NVL(T30023.T_MORNING_DOSE_UNIT,'0') ||'+' || NVL(T30023.T_NOON_DOSE_UNIT,'0') ||'+' || NVL(T30023.T_NIGHT_DOSE_UNIT,'0'),'0+0+0',NULL, NVL(T30023.T_MORNING_DOSE_UNIT,'0') ||' + ' || NVL(T30023.T_NOON_DOSE_UNIT,'0') ||' + ' || NVL(T30023.T_NIGHT_DOSE_UNIT,'0')) || T_DOSE_UNIT ||' ' ||T30036.T_UM_SHORT_DSCRPTN DOSE_INS, 
            T30023.T_REMARKS ||' ' || RTRIM(LTRIM(NVL2(T30023.T_SATURDAY,'sat',NULL) ||' ' || NVL2(T30023.T_SUNDAY,'sun',NULL) ||' ' || NVL2(T30023.T_MONDAY,'mon',NULL) ||' ' || NVL2(T30023.T_TUESDAY,'tue',NULL) ||' ' || NVL2(T30023.T_WEDNESDAY,'wed',NULL) ||' ' || NVL2(T30023.T_THURSDAY,'thu',NULL) ||' ' || NVL2(T30023.T_FRIDAY,'fri',NULL) )) T_REMARKS ,
            T02029.T_EMP_NO HDM_DOC_CODE,rtrim(T02029.T_name_given,' ')||' ' ||rtrim(T02029.T_NAME_FATHER,' ')||rtrim(T02029.T_NAME_GFATHER,' ')||' ' ||rtrim(T02029.T_name_family,' ') doc_name, T02039.T_SPCLTY_CODE HDM_SPCLTY_CODE, '('||decode(T02029.T_EMP_BADGE_NO,null, T02029.T_emp_phm_code, T02029.T_EMP_BADGE_NO||'-'||nvl(T02029.T_emp_phm_code,'N/A') )||')' HDM_DOC_QUALIF, 
            (select T_LANG2_NAME from T02041 where T_DESIGN_CODE = T02039.T_DESIGNATION) HDM_POST, (select T_LANG2_NAME from T02040 where T_spclty_code = T02039.T_SPCLTY_CODE) HDM_CONS_SPECIALITY FROM T03001 LEFT JOIN T30023 ON T30023.T_PAT_NO=T03001.T_PAT_NO LEFT JOIN V06102 ON V06102.ICD10_CODE=T30023.T_ICD10_DIAG_CODE LEFT JOIN T02029 ON T30023.T_ORGN_DOC = T02029.T_EMP_NO 
            LEFT JOIN T02040 ON T02040.T_SPCLTY_CODE=T30023.T_STK_LOC_CODE LEFT JOIN T07001 T07 ON T07.T_CLINIC_CODE = T30023.T_PRESC_LOC LEFT JOIN T30009 ON T30009.T_FREQUENCY_CODE=T30023.T_DOSE_TIME_DAILY LEFT JOIN T30010 ON T30010.T_DOSE_DURATION_CODE=T30023.T_DOSE_DURATION LEFT JOIN T30036 ON T30036.T_UM =T30023.T_ISSUE_UM left join T02039 on T02039.T_DOC_CODE = T02029.T_EMP_NO 
            WHERE T03001.T_PAT_NO ='{patNo}' AND T30023.T_PAT_MEDICINE_SEQ='{patSeq}' AND T30023.T_DRUG_INACTIVE_FLAG IS NULL";
            return ReportQuery(query);
        }
    }
}