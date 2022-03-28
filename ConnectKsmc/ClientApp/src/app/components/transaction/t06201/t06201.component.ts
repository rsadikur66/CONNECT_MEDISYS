import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MessageService, PrimeNGConfig } from 'primeng/api';
import { CommonService } from '../../../services/common.service';
import { T06201Service } from '../../../services/transaction/t06201.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 't06201',
  templateUrl: 't06201.component.html',
  providers: [T06201Service]
})

export class T06201Component implements OnInit {
  patientType: any[] = [];
  labelsNew: any = [];
  visitDate: any = '';
  obj: any = new Object();
  tableShowDoctor: boolean = true;
  tableShowPatient: boolean = false;
  tableShow: boolean = false;
  SickLeaveFor: boolean = true;
  Mcr: boolean = true;
  other: boolean = true
   v = 0;
  userForm = new FormGroup({
    'ddlPatientType': new FormControl(),
    'txtYY': new FormControl(),
    'txtMM': new FormControl(),
    'txtDateBirth': new FormControl(),
    'txtNationality': new FormControl(),
    'txtPatientNo': new FormControl(),
    'txtPatientName': new FormControl(),
    'txtSex': new FormControl(),
    'txtOccupation': new FormControl(),
    'txtAdmDateEng': new FormControl(),
    'txtAdmDateAr': new FormControl(),
    'txtDichargeDateEng': new FormControl(),
    'txtDichargeDateAr': new FormControl(),
    'txtEpisode': new FormControl(),

    'txtDateVisitEng': new FormControl(),
    'txtDateVisitAr': new FormControl('', Validators.required),
    'txtSickLeaveDays': new FormControl('', Validators.required),
    'chkSickLeaveFlg': new FormControl('', Validators.required),
    'txtConsDoc': new FormControl(),

    'txtDocDesMor': new FormControl(),
    'txtEndDateEng': new FormControl('', Validators.required),
    'txtEndDateAr': new FormControl('', Validators.required),
    'txtMCR': new FormControl('', Validators.required),
    'txtOtherRsn': new FormControl('', Validators.required),
    'txtBadgeNo': new FormControl(),
    'txtPhysicianName': new FormControl(),
    'txtOccupation_2': new FormControl(),
    'txtPlaceofwork': new FormControl('', Validators.required),
    'txtLetterHead': new FormControl('', Validators.required),
    'txtLetterRef': new FormControl('', Validators.required),
    'txtLetterRefDateEng': new FormControl('', Validators.required),
    'txtLetterRefDateAr': new FormControl('', Validators.required),
    'txtTreatDoc': new FormControl(),
    'txtTreatCode': new FormControl(),
    'txtStartingAr': new FormControl(),
    'txtStartingEng': new FormControl(),

    'txtDoctorId': new FormControl(),
    'txtDoctorName': new FormControl(),
    'txtPatientNo_1': new FormControl(),
    'txtLeaveDays': new FormControl(),
    'txtPatientName_1': new FormControl(),
    'txtDate': new FormControl(),
    'chkFuFlg': new FormControl(),
    'chkMedComFlg': new FormControl(),
    'chkApprovalFlg': new FormControl(),
    'chkConTrearFlg': new FormControl(),
    'chkPrmntParFlg': new FormControl(),
    'chkOtherFlg': new FormControl(),
    'chkOk': new FormControl()
  });
  constructor(private commonService: CommonService, private t06201Service: T06201Service, private route: ActivatedRoute,private messageService: MessageService) { }

  ngOnInit(): void {
    let patNo: any = this.route.snapshot.paramMap.get('patNo');
    this.getPatientType();
    this.getPatientInfo(patNo);
    this.getDocInfo();
    this.commonService.getFormLabel('T06201').subscribe((success: any) => {
      this.labelsNew = success;
    });
  }
  getPatientType() {
    this.t06201Service.getPatientType().subscribe((success: any) => {
      this.patientType = success;
    })
  }
  
  PatientClick() {
    var patType = this.userForm.get('ddlPatientType')?.value?.VAL;
    var patNo = this.userForm.get('txtPatientNo')?.value;
    
    if (patType != null && patType != undefined) {
    //  this.getPatientInfo();
      this.getPatientInfo(patNo);
      this.getDocInfo();
    } else {
      alert('Please select Patient Type');
    }
  }
  getDocInfo() {
    this.t06201Service.getDoctorInfo().subscribe((success: any) => {
      if (success == null) {
        this.tableShow = false
      } else {       
        this.userForm.controls['txtDoctorId'].setValue(success.T_DOC_CODE);
        this.userForm.controls['txtDoctorName'].setValue(success.D_DESC);
        this.tableShow = true
      }
    })
  }
  onOkClicked() {
    this.tableShowDoctor = false;
    this.tableShowPatient = true;
  }
  onSickLvFlg() {
    this.SickLeaveFor = this.userForm.get('chkSickLeaveFlg')?.value == true ? false : true;
  }
  onLabelLoad(e: string) {
    try {
      return this.labelsNew.filter((a: { T_LABEL_NAME: string; }) => a.T_LABEL_NAME == e)[0].T_LABEL_TEXT;
    } catch (e) {
      return '';
    }
  }
  onPatientTypeChanged() {
    this.userForm.controls['txtAdmDateEng'].setValue('');
    this.userForm.controls['txtAdmDateAr'].setValue('');
    this.userForm.controls['txtDichargeDateEng'].setValue('');
    this.userForm.controls['txtDichargeDateAr'].setValue('');
    this.userForm.controls['txtEpisode'].setValue('');

    var value = this.userForm.get('ddlPatientType')?.value?.VAL;
    var patNo = this.userForm.get('txtPatientNo')?.value;
    this.t06201Service.getPatientTypeInfo(value, patNo).subscribe((success: any) => {
      if (value === '1') {
        if (success == null) { alert('Patient does not have IP Discharge for today! Cant generated Sick Leave'); } else {
          this.userForm.controls['txtAdmDateEng'].setValue(new Date(success.T_ADMIT_DATE).toLocaleDateString("en-GB", { year: 'numeric', month: '2-digit', day: '2-digit' }));
          this.userForm.controls['txtAdmDateAr'].setValue(new Date(success.T_ADMIT_DATE).toLocaleDateString('ar-SA', { year: 'numeric', month: '2-digit', day: '2-digit' }));
          this.userForm.controls['txtDichargeDateEng'].setValue(new Date(success.T_CLN_DISCH_DATE).toLocaleDateString("en-GB", { year: 'numeric', month: '2-digit', day: '2-digit' }));
          this.userForm.controls['txtDichargeDateAr'].setValue(new Date(success.T_CLN_DISCH_DATE).toLocaleDateString('ar-SA', { year: 'numeric', month: '2-digit', day: '2-digit' }));
          this.userForm.controls['txtEpisode'].setValue(success.T_EPISODE_NO);
        }
      } else if (value === '2') {
        if (success == null) { alert('Patient does not have ER Discharge for today! Cant generated Sick Leave'); } else {
          this.userForm.controls['txtAdmDateEng'].setValue(new Date(success.T_EPISODE_START_DATE).toLocaleDateString("en-GB", { year: 'numeric', month: '2-digit', day: '2-digit' }));
          this.userForm.controls['txtAdmDateAr'].setValue(new Date(success.T_EPISODE_START_DATE).toLocaleDateString('ar-SA', { year: 'numeric', month: '2-digit', day: '2-digit' }));
          this.userForm.controls['txtDichargeDateEng'].setValue(new Date(success.T_EPISODE_END_DATE).toLocaleDateString("en-GB", { year: 'numeric', month: '2-digit', day: '2-digit' }));
          this.userForm.controls['txtDichargeDateAr'].setValue(new Date(success.T_EPISODE_END_DATE).toLocaleDateString('ar-SA', { year: 'numeric', month: '2-digit', day: '2-digit' }));
          this.userForm.controls['txtEpisode'].setValue(success.T_EPISODE_NO);
        }
      } else if (value === '3') {
        if (success == null) { alert('Patient does not have ER Discharge for today! Cant generated Sick Leave'); } else {
          this.userForm.controls['txtAdmDateEng'].setValue(new Date(success.T_APPT_DATE).toLocaleDateString("en-GB", { year: 'numeric', month: '2-digit', day: '2-digit' }));
          this.userForm.controls['txtAdmDateAr'].setValue(new Date(success.T_APPT_DATE).toLocaleDateString('ar-SA', { year: 'numeric', month: '2-digit', day: '2-digit' }));
        }
      }
    })
  }
  getPatientInfo(patNo:any) {
    //var patNo = '00794873';
    this.t06201Service.getPatientInfo(patNo).subscribe((success: any) => {
      this.userForm.controls['txtYY'].setValue(success.AGE_Y);
      this.userForm.controls['txtMM'].setValue(success.AGE_M);
      this.userForm.controls['txtDateBirth'].setValue(success.T_BIRTH_DATE);
      this.userForm.controls['txtNationality'].setValue(success.NTNLTY);
      this.userForm.controls['txtPatientNo'].setValue(success.T_PAT_NO);
      this.userForm.controls['txtPatientName'].setValue(success.T_NAME);

      this.userForm.controls['txtPatientNo_1'].setValue(success.T_PAT_NO);
      this.userForm.controls['txtPatientName_1'].setValue(success.T_NAME);
     
      this.userForm.controls['txtSex'].setValue(success.GENDER_DES);
      this.userForm.controls['txtOccupation'].setValue(success.OCCUPATION);
      if (success.PAT_TYPE == '1') {
        this.userForm.controls['ddlPatientType'].setValue({ VAL: '1', NAME: 'IP' });
        this.userForm.controls['txtAdmDateEng'].setValue(new Date(success.T_ADMIT_DATE).toLocaleDateString("en-GB", { year: 'numeric', month: '2-digit', day: '2-digit' }));
        this.userForm.controls['txtAdmDateAr'].setValue(new Date(success.T_ADMIT_DATE).toLocaleDateString('ar-SA', { year: 'numeric', month: '2-digit', day: '2-digit' }));
        this.userForm.controls['txtDichargeDateEng'].setValue(new Date(success.T_CLN_DISCH_DATE).toLocaleDateString("en-GB", { year: 'numeric', month: '2-digit', day: '2-digit' }));
        this.userForm.controls['txtDichargeDateAr'].setValue(new Date(success.T_CLN_DISCH_DATE).toLocaleDateString('ar-SA', { year: 'numeric', month: '2-digit', day: '2-digit' }));
        this.userForm.controls['txtEpisode'].setValue(success.T_EPISODE_NO);
      } else {
        this.userForm.controls['ddlPatientType'].setValue({ VAL: '2', NAME: 'OPD' });
        this.userForm.controls['txtAdmDateEng'].setValue(new Date().toLocaleDateString("en-GB", { year: 'numeric', month: '2-digit', day: '2-digit' }));
        this.userForm.controls['txtAdmDateAr'].setValue(new Date().toLocaleDateString('ar-SA', { year: 'numeric', month: '2-digit', day: '2-digit' }));
        
        this.userForm.controls['txtDichargeDateEng'].setValue('');
        this.userForm.controls['txtDichargeDateAr'].setValue('');
        this.userForm.controls['txtEpisode'].setValue('');
      }
    })
  }
  onNextClicked() {
    var patNo = this.userForm.get('txtPatientNo')?.value;
    var patType = this.userForm.get('ddlPatientType')?.value?.VAL;
    this.t06201Service.getDetails(patNo, patType).subscribe((success: any) => {
      this.userForm.controls['txtDateVisitEng'].setValue(success.T_VISIT_DATE == null ? '' : new Date(success.T_VISIT_DATE).toLocaleDateString("en-GB", { year: 'numeric', month: '2-digit', day: '2-digit' }));
      this.userForm.controls['txtDateVisitAr'].setValue(success.T_VISIT_DATE == null ? '' : new Date(success.T_VISIT_DATE).toLocaleDateString('ar-SA', { year: 'numeric', month: '2-digit', day: '2-digit' }));
      this.userForm.controls['txtStartingEng'].setValue(success.T_START_DATE == null ? '' : new Date(success.T_START_DATE).toLocaleDateString("en-GB", { year: 'numeric', month: '2-digit', day: '2-digit' }));
      this.userForm.controls['txtStartingAr'].setValue(success.T_START_DATE == null ? '' : new Date(success.T_START_DATE).toLocaleDateString('ar-SA', { year: 'numeric', month: '2-digit', day: '2-digit' }));
      this.userForm.controls['txtEndDateEng'].setValue(success.T_END_DATE == null ? '' : new Date(success.T_END_DATE).toLocaleDateString("en-GB", { year: 'numeric', month: '2-digit', day: '2-digit' }));
      this.userForm.controls['txtEndDateAr'].setValue(success.T_END_DATE == null ? '' : new Date(success.T_END_DATE).toLocaleDateString('ar-SA', { year: 'numeric', month: '2-digit', day: '2-digit' }));
      this.userForm.controls['chkSickLeaveFlg'].setValue(success.T_SICK_FLAG);
      this.userForm.controls['txtSickLeaveDays'].setValue(success.T_LEAVE_DAYS);
      this.userForm.controls['txtConsDoc'].setValue(success.T_CONS_DOC);
      this.userForm.controls['chkMedComFlg'].setValue(success.T_MED_COM_FLAG);
      this.userForm.controls['chkFuFlg'].setValue(success.T_FU_FLAG);
      this.userForm.controls['txtMCR'].setValue(success.T_MED_COM_RSN);
      this.userForm.controls['chkApprovalFlg'].setValue(success.T_APPROVAL_FLAG);
      this.userForm.controls['chkConTrearFlg'].setValue(success.T_CNT_TREAT_FLAG);
      this.userForm.controls['chkPrmntParFlg'].setValue(success.T_PRMNT_PAR_FLAG);
      this.userForm.controls['chkOtherFlg'].setValue(success.T_OTHER_FLAG);
      this.userForm.controls['txtOtherRsn'].setValue(success.T_OTHER_RSN);
      this.userForm.controls['txtTreatCode'].setValue(success.T_TREAT_DOC_CODE);
      this.userForm.controls['txtTreatDoc'].setValue(success.T_TREAT_DOC);
      this.userForm.controls['txtPhysicianName'].setValue(success.T_DOC_CODE);
      this.userForm.controls['txtOccupation_2'].setValue(success.T_OCCUPATION);
      this.userForm.controls['txtPlaceofwork'].setValue(success.T_PLACE_WORK);
      this.userForm.controls['txtLetterHead'].setValue(success.T_LETTER_HEAD);
      this.userForm.controls['txtLetterRef'].setValue(success.T_LETTER_REF_NO);
      this.userForm.controls['txtLetterRefDateEng'].setValue(success.T_LETTER_DATE == null ? '' : new Date(success.T_LETTER_DATE).toLocaleDateString('en-GB', { year: 'numeric', month: '2-digit', day: '2-digit' }));
      this.userForm.controls['txtLetterRefDateAr'].setValue(success.T_LETTER_DATE == null ? '' : new Date(success.T_LETTER_DATE).toLocaleDateString('ar-SA', { year: 'numeric', month: '2-digit', day: '2-digit' }));
    })
  }
  onSaveClicked() {
    this.v = 0;
    if (this.userForm.get('txtDateVisitEng')?.value == '' || this.userForm.get('txtDateVisitEng')?.value == undefined) {
      var date = new Date();
      this.visitDate = new Date(date).toLocaleDateString("en-GB", { year: 'numeric', month: '2-digit', day: '2-digit' })
    }
    if (this.userForm.get('txtDateVisitEng')?.value > new Date()) {
      alert('Visit Date should be Todays Date.');
      return;
    }
    if (this.userForm.get('chkSickLeaveFlg')?.value == false || (this.userForm.get('txtSickLeaveDays')?.value == null || this.userForm.get('txtSickLeaveDays')?.value == undefined)) {
      alert('Please enter Leave days and try again');
      return;
    }
    this.obj.T_PAT_TYPE = this.userForm.get('ddlPatientType')?.value?.VAL;
    this.obj.T_PAT_NO = this.userForm.get('txtPatientNo')?.value;
    this.obj.T_VISIT_DATE = this.userForm.get('txtDateVisitEng')?.value;
    this.obj.T_LEAVE_DAYS = this.userForm.get('txtSickLeaveDays')?.value;
    this.obj.T_START_DATE = this.userForm.get('txtStartingEng')?.value;
    this.obj.T_END_DATE = this.userForm.get('txtEndDateEng')?.value;
    this.obj.T_SICK_FLAG = this.userForm.get('chkSickLeaveFlg')?.value == false ? '0' : '1';
    this.obj.T_LEAVE_DAYS = this.userForm.get('txtSickLeaveDays')?.value;
    this.obj.T_CONS_DOC = this.userForm.get('txtConsDoc')?.value;
    this.obj.T_FU_FLAG = this.userForm.get('chkFuFlg')?.value == false ? '0' : '1';
    this.obj.T_MED_COM_RSN = this.validate(this.userForm.get('txtMCR')?.value,'txtMCR');
    this.obj.T_MED_COM_FLAG = this.userForm.get('chkMedComFlg')?.value == false ? '0' : '1';
    this.obj.T_APPROVAL_FLAG = this.userForm.get('chkApprovalFlg')?.value == false ? '0' : '1';
    this.obj.T_CNT_TREAT_FLAG = this.userForm.get('chkConTrearFlg')?.value == false ? '0' : '1';
    this.obj.T_PRMNT_PAR_FLAG = this.userForm.get('chkPrmntParFlg')?.value == false ? '0' : '1';
    this.obj.T_OTHER_FLAG = this.userForm.get('chkOtherFlg')?.value == false ? '0' : '1';
    this.obj.T_OTHER_RSN = this.validate(this.userForm.get('txtOtherRsn')?.value,'txtOtherRsn');
    this.obj.T_TREAT_DOC_CODE = this.userForm.get('txtTreatCode')?.value;
    this.obj.T_TREAT_DOC = this.userForm.get('txtTreatDoc')?.value;
    this.obj.T_DOC_CODE = this.userForm.get('txtPhysicianName')?.value;
    this.obj.T_OCCUPATION = this.validate(this.userForm.get('txtOccupation_2')?.value,'txtOccupation_2');
    this.obj.T_PLACE_WORK = this.validate(this.userForm.get('txtPlaceofwork')?.value, 'txtPlaceofwork');
    this.obj.T_LETTER_HEAD = this.validate(this.userForm.get('txtLetterHead')?.value, 'txtLetterHead');
    this.obj.T_LETTER_REF_NO = this.validate(this.userForm.get('txtLetterRef')?.value,'txtLetterRef');
    this.obj.T_LETTER_DATE = this.validate(this.userForm.get('txtLetterRefDateEng')?.value,'txtLetterRefDateEng');

    if (this.v == 0) {
      this.t06201Service.saveData(this.obj).subscribe((success: any) => {
        this.messageService.add({ severity: 'success', summary: 'Success!', detail: success });
      })
    } else {
      //this.messageService.add({ severity: 'error', summary:'required', detail: 'All are required' });
    }
    
  }
  validate(val: any, textId: any) {
    if (!val) {      
      document.getElementById(textId)?.focus();
      this.messageService.add({ severity: 'error', summary: 'required', detail: 'This is required' });
      this.v = 1;
      return val;
    } else { return val;}
  }
  validateFormFields(formGroup: FormGroup) {
   // if (!formGroup.controls['txtPlaceofwork'].valid) {
    //  formGroup.controls['txtPlaceofwork'].markAsDirty();
    //  this.messageService.add({ severity: 'error', summary: document.getElementById('txtPlaceofwork')?.innerText, detail: 'This is required' });
    //}
    //if (!formGroup.controls['ddlClinic'].valid) {
    //  formGroup.controls['ddlClinic'].markAsDirty();
    //  this.messageService.add({ severity: 'error', summary: document.getElementById('lblClinic')?.innerText, detail: this.messages.find(x => x.CODE === 'S0313').TEXT });
    //}
    //if (!formGroup.controls['txtApptDate'].valid) {
    //  formGroup.controls['txtApptDate'].markAsDirty();
    //  this.messageService.add({ severity: 'error', summary: document.getElementById('lblApptDate')?.innerText, detail: this.messages.find(x => x.CODE === 'S0313').TEXT });
    //}
  }
  onVisitDateBlur() {
    const date = this.userForm.get('txtDateVisitEng')?.value as string;
    let year = 0;
    let month = 0;
    if (date.split('/').length > 1) {
      year = parseInt(date.substr(6, 4));
      month = parseInt(date.substr(3, 2));
    } else {
      year = parseInt(date.substr(4, 4));
      month = parseInt(date.substr(2, 2));
    }
    const day = parseInt(date.substr(0, 2));
    const dob = month + '/' + day + '/' + year;
    if (new Date(dob).toLocaleDateString("en-GB", { year: 'numeric', month: '2-digit', day: '2-digit' }) === 'Invalid Date') {
      this.messageService.add({ severity: 'warn', summary: 'Error!', detail: 'Date Format Should Be Like "dd/MM/yyyy" or "ddMMyyyy"' });
      setTimeout(() => { document.getElementById('txtDateVisitEng')?.focus(); }, 0);
      return;
    }
    this.userForm.get('txtDateVisitEng')?.setValue(new Date(dob).toLocaleDateString("en-GB", { year: 'numeric', month: '2-digit', day: '2-digit' }));
    this.userForm.get('txtDateVisitAr')?.setValue(new Date(dob).toLocaleDateString("ar-SA", { year: 'numeric', month: '2-digit', day: '2-digit' }));
    this.userForm.controls['txtTreatCode'].setValue(this.userForm.get('txtDoctorId')?.value);
    this.userForm.controls['txtTreatDoc'].setValue(this.userForm.get('txtDoctorName')?.value);
    this.userForm.controls['txtPhysicianName'].setValue(this.userForm.get('txtDoctorName')?.value);
    
    
  }
  onSickLeaveBlur() {
    let k = 0;
    let dateSt =  this.userForm.get('txtStartingEng')?.value; 
    var strtD = new Date(this.convertDate(dateSt));    
    let dateAdm = this.userForm.get('txtAdmDateEng')?.value;
    var admD = new Date(this.convertDate(dateAdm));   
    if ( strtD < admD) {
      this.messageService.add({ severity: 'error', summary: 'error!', detail: 'starting date can not be less then the admission date' });
      document.getElementById('txtStartingEng')?.focus();
      k = 1;
    }
    if (k==0) {
     this.onStartDateBlur()
    }
  }
  keyPressSL(e: any) {
   var l= e.key;
    if (/\D/g.test(e.key)) {
      e.key = e.key.replace(/\D/g, '');
    }
  }
  onMcrClick() {
    if (this.userForm.get('chkMedComFlg')?.value == true) {
      this.Mcr = false
    } else {
      this.Mcr = true
      this.userForm.controls['txtMCR'].setValue('');
    }
   
  }
  onOtherClick() {
    if (this.userForm.get('chkOtherFlg')?.value == true) {
      this.other = false
    } else {
      this.other = true
      this.userForm.controls['txtOtherRsn'].setValue('');
    }
    
  }
  convertDate(date:any) {    
    let year = 0;
    let month = 0;
    if (date.split('/').length > 1) {
      year = parseInt(date.substr(6, 4));
      month = parseInt(date.substr(3, 2));
    } else {
      year = parseInt(date.substr(4, 4));
      month = parseInt(date.substr(2, 2));
    }
    const day = parseInt(date.substr(0, 2));
    const dob = year + ',' + month + ',' + day;
    return dob;
  }
  onStartDateBlur() {
    const date = this.userForm.get('txtStartingEng')?.value as string;
    const sickLeaveDays = Number(this.userForm.get('txtSickLeaveDays')?.value) ;
    let year = 0;
    let month = 0;
    if (date.split('/').length > 1) {
      year = parseInt(date.substr(6, 4));
      month = parseInt(date.substr(3, 2));
    } else {
      year = parseInt(date.substr(4, 4));
      month = parseInt(date.substr(2, 2));
    }
    const day = parseInt(date.substr(0, 2));
    const dob = month + '/' + day + '/' + year;
    if (new Date(dob).toLocaleDateString("en-GB", { year: 'numeric', month: '2-digit', day: '2-digit' }) === 'Invalid Date') {
      this.messageService.add({ severity: 'warn', summary: 'Error!', detail: 'Date Format Should Be Like "dd/MM/yyyy" or "ddMMyyyy"' });
      setTimeout(() => { document.getElementById('txtStartingEng')?.focus(); }, 0);
      return;
    }

    //let dateSt = this.userForm.get('txtStartingEng')?.value; //txtAdmDateEng
    var strtD = new Date(this.convertDate(new Date(dob).toLocaleDateString("en-GB", { year: 'numeric', month: '2-digit', day: '2-digit' })));
    let dateAdm = this.userForm.get('txtAdmDateEng')?.value;
    var admD = new Date(this.convertDate(dateAdm));
   
    if ( strtD < admD) {
      this.messageService.add({ severity: 'error', summary: 'error!', detail: 'starting date can not be less then the admission date' });
      document.getElementById('txtStartingEng')?.focus();
    } else {
      this.userForm.get('txtStartingEng')?.setValue(new Date(dob).toLocaleDateString("en-GB", { year: 'numeric', month: '2-digit', day: '2-digit' }));
      this.userForm.get('txtStartingAr')?.setValue(new Date(dob).toLocaleDateString("ar-SA", { year: 'numeric', month: '2-digit', day: '2-digit' }));

      this.userForm.get('txtEndDateEng')?.setValue(this.onDatePastFuture(new Date(dob), sickLeaveDays));
      this.userForm.get('txtEndDateAr')?.setValue(new Date(this.onDatePastFutureArb(new Date(dob), sickLeaveDays)).toLocaleDateString("ar-SA", { year: 'numeric', month: '2-digit', day: '2-digit' }));

    }
    
  }
  onLetterReDate() {
    const date = this.userForm.get('txtLetterRefDateEng')?.value as string;
    let year = 0;
    let month = 0;
    if (date.split('/').length > 1) {
      year = parseInt(date.substr(6, 4));
      month = parseInt(date.substr(3, 2));
    } else {
      year = parseInt(date.substr(4, 4));
      month = parseInt(date.substr(2, 2));
    }
    const day = parseInt(date.substr(0, 2));
    const dob = month + '/' + day + '/' + year;
    if (new Date(dob).toLocaleDateString("en-GB", { year: 'numeric', month: '2-digit', day: '2-digit' }) === 'Invalid Date') {
      this.messageService.add({ severity: 'warn', summary: 'Error!', detail: 'Date Format Should Be Like "dd/MM/yyyy" or "ddMMyyyy"' });
      setTimeout(() => { document.getElementById('txtLetterRefDateEng')?.focus(); }, 0);
      return;
    }
    this.userForm.get('txtLetterRefDateEng')?.setValue(new Date(dob).toLocaleDateString("en-GB", { year: 'numeric', month: '2-digit', day: '2-digit' }));
    this.userForm.get('txtLetterRefDateAr')?.setValue(new Date(dob).toLocaleDateString("ar-SA", { year: 'numeric', month: '2-digit', day: '2-digit' }));
  }
  onDatePastFuture(startingDate: Date, rangeDay: any) {
    let date = startingDate;
    date.setDate(date.getDate() + rangeDay);
    let datePastFuture = date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear();
    return datePastFuture;
  }
  onDatePastFutureArb(startingDate: Date, rangeDay: any) {
    let date = startingDate;
    date.setDate(date.getDate() + rangeDay);
    let datePastFuture = (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();
    return datePastFuture;
  }
  setPermission(permissions: any) {
    if (permissions.canOpen) {
    }
    else {
    }
  }
  onClearClicked() {
  }
  onPrintClicked() {
    //var patNo = '00794873';
    //var visitDate = '11/07/2009';
    //window.open("./api/r06201/getReport?patNo=" + patNo + "&visitDate=" + visitDate, "popup", "location=1, status=1, scrollbars=1");

    //var reqNo = '0011320322';
   // window.open("./api/r13021/getReport?reqNo=" + reqNo, "popup", "location=1, status=1, scrollbars=1");
    //  var reqNo = '0011334813';// '0001807308';
  //  window.open("./api/r13115/getReport?reqNo=" + reqNo, "popup", "location=1, status=1, scrollbars=1");

    var reqNo = '0010770410';
    window.open("./api/r13111/getReport?reqNo=" + reqNo, "popup", "location=1, status=1, scrollbars=1");
  }
}
