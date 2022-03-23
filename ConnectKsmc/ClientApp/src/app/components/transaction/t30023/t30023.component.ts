import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { LoginService } from '../../../services/login.service';
import { T30023Service } from '../../../services/transaction/t30023.service';
import { T30023 } from '../../../models/T30023';
import { EndpointService } from '../../../services/endpoint.service';
import { CommonService } from '../../../services/common.service';
import { Subscription } from 'rxjs';
import { LazyLoadEvent } from 'primeng/api';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { ConfirmationService } from 'primeng/api';
import { ConfirmDialogModule } from 'primeng/confirmdialog';

@Component({
  selector: 't30023',
  templateUrl: 't30023.component.html',
  styles: ['::ng-deep .ui-helper-clearfix::before, ::ng-deep .ui-helper-clearfix::after { display: none !important; }'],
  styleUrls: ['t30023.component.css'],
  providers: [T30023Service, EndpointService, ConfirmationService]
})
export class T30023Component implements OnInit {
  private subscriptions = new Subscription();
  messages: any[] = [];
  formCode: string = 'T30023';
  loading: boolean = false;
  userForm: FormGroup;
  insForm: FormGroup;
  daysForm: FormGroup;
  genericForm: FormGroup;
  selectedData: any[] = [];
  t30023: any = [];
  display: boolean = false;
  isSaveEnable: boolean = true;
  ky!: string;
  displayAdmitted: boolean = false;
  admittedPatients!: any[];
  selectedPatData: any = [];
  labelsNew: any = [];
  //t23 = new T30023;
  tabIndex: number = 0;
  medInsBool: boolean = false;
  medInsMsg: string = '';
  docDisplay: boolean = false;
  docList: any = [];
  docSelectedData: any = [];

  spcltyDisplay: boolean = false;
  spcltyList: any = [];
  spcltySelectedData: any = [];

  locationDisplay: boolean = false;
  locationList: any = [];
  locationSelectedData: any = [];

  icd10Display: boolean = false;
  icd10List: any = [];
  icd10SelectedData: any = [];

  slipDisplay: boolean = false;
  slipList: any = [];
  slipSelectedData: any = [];

  medicineDisplay: boolean = false;
  medicineList: any = [];
  medicineSelectedData: any = [];

  medicineTradeDisplay: boolean = false;
  medicineTradeList: any = [];
  medicineTradeSelectedData: any = [];

  medicineSpecialityDisplay: boolean = false;
  medicineSpecialityList: any = [];
  medicineSpecialitySelectedData: any = [];

  frequencyList: any = [];
  frequencySelectedData: any = [];

  durationList: any = [];
  durationSelectedData: any = [];

  umDisplay: boolean = false;
  umList: any = [];
  umSelectedData: any = [];

  insDisplay: boolean = false;
  insList: any = [];
  insSelectedData: any = [];

  daysDisplay: boolean = false;
  daysList: any = [];
  daysSelectedData: any = [];

  genericDisplay: boolean = false;

  drugMasterDisplay: boolean = false;
  drugMasterList: any = [];
  drugMasterSelectedData: any = [];

  medList: any = [];
  morningList: any = [];
  noonList: any = [];
  nightList: any = [];

  medicineListAsOutPatient: any = [];
  medicineListAsInPatient: any = [];
  drugHistoryDisplay: boolean = false;

  isEditable: boolean = true;
  tableIndex: string = '';

  canSave!: boolean;
  canUpdate!: boolean;
  canDelete!: boolean;
  canQuery!: boolean;

  userLang!: string;
  siteCode!: string;
  version!: string;
  pNum: RegExp = /^\d*\.?\d*$/;
  myStyles = { 'display': 'none' }

  totalPatList: number = 0;
  enter: string = 'cancel';
  @ViewChild("slipNo", { static: false }) slipNo!: ElementRef;
  constructor(private loginService: LoginService, private t30023Service: T30023Service, private router: Router, private formBuilder: FormBuilder,
    private messageService: MessageService, private confirmationService: ConfirmationService,
    private endpointService: EndpointService, private commonService: CommonService, private route: ActivatedRoute, private ngxService: NgxUiLoaderService) {
    this.insForm = this.formBuilder.group({
      'txtMorningIns': new FormControl(),
      'txtNoonIns': new FormControl(),
      'txtNightIns': new FormControl(),
      'ddlMorningIns': new FormControl(),
      'ddlNoonIns': new FormControl(),
      'ddlNightIns': new FormControl()
    });
    this.genericForm = this.formBuilder.group({
      'txtGenCode': new FormControl(),
      'txtGenDesc': new FormControl(),
      'txtTradeCode': new FormControl(),
      'txtTradeDesc': new FormControl()
    });
    this.daysForm = this.formBuilder.group({
      chkSaturday: [['sat']],
      chkSunday: [['sun']],
      chkMonday: [['mon']],
      chkTuesday: [['tue']],
      chkWednesday: [['wed']],
      chkThursday: [['thu']],
      chkFriday: [['fri']]
    });
    this.userForm = this.formBuilder.group({
      'txtPatNo': new FormControl('', Validators.required),
      'txtPatName': new FormControl(),
      'txtSex': new FormControl(),
      'txtAge': new FormControl(),
      'txtNationality': new FormControl(),
      'txtHeight': new FormControl(),
      'txtWeight': new FormControl(),
      'txtSlipNo': new FormControl(),
      'txtLocation': new FormControl(''),
      'txtLocationDesc': new FormControl(),
      'txtDocCode': new FormControl('', Validators.required),
      'txtDocName': new FormControl(),
      'txtSpecialityCode': new FormControl(),
      'txtSpecialityDesc': new FormControl(),
      'txtICDCode': new FormControl(),
      'txtICDDEsc': new FormControl(),
      'txtDiagonosis': new FormControl('', Validators.required),
      'txtApptNo': new FormControl(),
      'chkPatType': new FormControl(),
      'txtPregnantWeek': new FormControl(),
      'txtVisitNo': new FormControl(),
      'txtMobileNo': new FormControl(),
      'chkDelType': new FormControl('', Validators.required),
      chkPregnant: [['prg']]
    });
    loginService.checkIdle();
  }

  ngOnInit(): void {
    this.ngxService.start();
    this.commonService.getAllMessage(`
        'S0313'/*Generic required*/,
        'S0360'/*Check permission*/,
        'S0614'/*Only Consultant Allowed*/,
        'N0024'/*Data saved*/
      `).subscribe((success: any) => {
      this.messages = success;
    });
    this.userLang = localStorage.getItem('userLang') as string;
    this.siteCode = localStorage.getItem('siteCode') as string;
    var patNo = this.returnValue(this.route.snapshot.paramMap.get('patNo'));
    var docCode = this.returnValue(this.route.snapshot.paramMap.get('docCode'));
    var apptNo = this.returnValue(this.route.snapshot.paramMap.get('apptNo'));
    var clinicCode = this.returnValue(this.route.snapshot.paramMap.get('clinicCode'));
    if (patNo != null) {
      this.onPatBlur(patNo, '1');
    }
    this.loading = true;
    this.endpointService.getData('t30023/getAllData').subscribe((success: any) => {
      //var dt = success;
      var dt = success;
      this.spcltyList = JSON.parse(dt.specialist);
      this.medicineList = JSON.parse(dt.medicine);
      this.medicineTradeList = JSON.parse(dt.trade);
      this.frequencyList = JSON.parse(dt.frequency);
      this.durationList = JSON.parse(dt.duration);
      //dt.ins.splice(0, 0, { T_INSTRUCTION_CODE: '', NAME: 'Select..' });
      var dtIns = JSON.parse(dt.ins);
      dtIns.splice(0, 0, { T_INSTRUCTION_CODE: '', NAME: 'Select..' });
      this.morningList = dtIns;
      this.noonList = dtIns;
      this.nightList = dtIns;
      this.umList = JSON.parse(dt.um);
      this.onLoadICD();
      //this.loading = false;
    }, error => { return []; });
    for (var i = 0; i < 25; i++) {
      this.medList.push({ T_COPY_PRS: false });
    }
    this.commonService.getFormLabel('T30023').subscribe((success: any) => {
      this.labelsNew = success;
    });
    this.userForm.get('txtSlipNo')?.disable();
  }
  ngOnDestroy() {
    this.subscriptions.unsubscribe();
  }
  setVersion(val: string) {
    this.version = val;
  }
  private makeEmpty() {
    this.userForm.reset({ 'txtPatNo': '' });
    this.makeEmpty2();
  }
  private makeEmpty2() {
    this.insForm.reset();
    this.daysForm.reset();
    this.medList = [];
    this.enter = 'cancel';
    this.change(this.enter);
    this.isSaveEnable = true;
  }
  onNewClicked() {
    this.makeEmpty();
  }
  onLoadICD() {
    this.endpointService.getData('t30023/getICD10').subscribe((success: any) => {
      var dt = success;
      this.icd10List = dt;
    }, error => { return []; });
  }
  /*onBtnSaveClick() {*/
  onSaveClicked() {
    if (this.userForm.valid) {
      var dtList = this.medList;
      if (dtList.length > 0) {
        var t23List = [];
        for (var i = 0; i < dtList.length; i++) {
          var t23 = new T30023();
          t23.pt_pat_no = this.returnValue(this.userForm.get('txtPatNo')?.value);
          t23.pt_pat_medicine_seq = this.returnValue(this.userForm.get('txtSlipNo')?.value);
          t23.pt_pat_type = this.returnValue(this.userForm.get('chkPatType')?.value);
          t23.pt_pat_weight = this.returnValue(this.userForm.get('txtWeight')?.value);
          t23.pt_pat_height = this.returnValue(this.userForm.get('txtHeight')?.value);
          t23.pt_orgn_doc = this.returnValue(this.userForm.get('txtDocCode')?.value);
          t23.pt_stk_loc_code = this.returnValue(this.userForm.get('txtSpecialityCode')?.value);
          t23.pt_diagnosis = this.returnValue(this.userForm.get('txtDiagonosis')?.value);
          t23.pt_appt_req_id = this.returnValue(this.userForm.get('txtApptNo')?.value);
          t23.pt_icd10_diag_code = this.returnValue(this.userForm.get('txtICDCode')?.value);
          t23.pt_presc_loc = this.returnValue(this.userForm.get('txtLocation')?.value);
          t23.T_DOC_MOBILE_NO = this.returnValue(this.userForm.get('txtMobileNo')?.value);
          t23.T_DRUG_DLVRY_MTHD_CODE_DCTR = this.returnValue(this.userForm.get('chkDelType')?.value);
          t23.pt_opd_req_item = this.returnValue(dtList[i].T_OPD_REQ_ITEM);
          t23.pt_dose_time_daily = this.returnValue(dtList[i].T_DOSE_TIME_DAILY);
          t23.pt_dose_duration = this.returnValue(dtList[i].T_DOSE_DURATION);
          t23.pt_morning_dose_unit = this.returnValue(dtList[i].T_MORNING_DOSE_UNIT);
          t23.pt_noon_dose_unit = this.returnValue(dtList[i].T_NOON_DOSE_UNIT);
          t23.pt_night_dose_unit = this.returnValue(dtList[i].T_NIGHT_DOSE_UNIT);
          t23.pt_morning_instruction = this.returnValue(dtList[i].T_MORNING_INSTRUCTION);
          t23.pt_noon_instruction = this.returnValue(dtList[i].T_NOON_INSTRUCTION);
          t23.pt_night_instruction = this.returnValue(dtList[i].T_NIGHT_INSTRUCTION);
          t23.pt_issue_um = this.returnValue(dtList[i].T_ISSUE_UM);
          t23.pt_doctor_issue_um = this.returnValue(dtList[i].T_ISSUE_UM);
          t23.pt_qty = this.returnValue(dtList[i].T_QTY);
          t23.pt_entry_date = this.returnValue(dtList[i].T_ENTRY_DATE);
          t23.pt_issue_date = this.returnValue(dtList[i].T_ISSUE_DATE);
          t23.pt_qty_remaining = this.returnValue(dtList[i].T_QTY);
          t23.pt_remarks = this.returnValue(dtList[i].T_REMARKS);
          t23.pt_doc_dose_unit = this.returnValue(dtList[i].T_DOC_DOSE_UNIT);
          t23.pt_dose_unit = this.returnValue(dtList[i].T_DOC_DOSE_UNIT);
          t23.pt_moh_item_code = this.returnValue(dtList[i].T_MOH_ITEM_CODE);
          t23.pt_request_strength = this.returnValue(dtList[i].T_STRENGTH);
          t23.pt_request_route = this.returnValue(dtList[i].T_DRUG_ROUTE_CODE);
          t23.pt_request_mform = this.returnValue(dtList[i].T_DRUG_FORM_CODE);
          t23.pt_request_sform = this.returnValue(dtList[i].T_DRUG_SFORM_CODE);
          t23.pt_request_gcode = this.returnValue(dtList[i].T_GEN_CODE);
          t23.pt_drug_inactive_flag = this.chkInactive(this.returnValue(dtList[i].T_DRUG_INACTIVE_FLAG));
          t23.pt_saturday = this.returnValue(dtList[i].T_SATURDAY);
          t23.pt_sunday = this.returnValue(dtList[i].T_SUNDAY);
          t23.pt_monday = this.returnValue(dtList[i].T_MONDAY);
          t23.pt_tuesday = this.returnValue(dtList[i].T_TUESDAY);
          t23.pt_wednesday = this.returnValue(dtList[i].T_WEDNESDAY);
          t23.pt_thursday = this.returnValue(dtList[i].T_THURSDAY);
          t23.pt_friday = this.returnValue(dtList[i].T_FRIDAY);
          t23.prowid = this.returnValue(dtList[i].ROWID);

          t23List.push(t23);
        }
        this.loading = true;
        var pat = this.returnValue(this.userForm.get('txtPatNo')?.value);
        var slip = this.returnValue(this.userForm.get('txtSlipNo')?.value);
        if (pat != null && slip != null) {
          this.endpointService.setDataParam('t30023/update', t23List).subscribe((success: any) => {
            var dt = success;
            this.onNextClicked('1');
            this.loading = false;
            if (dt.SLIP != '') {
              this.messageService.add({ severity: 'success', summary: 'Success!', detail: dt.MSG });
            } else {
              this.messageService.add({ severity: 'error', summary: 'Error!', detail: dt.MSG });
            }
          }, error => { this.loading = false; this.messageService.add({ severity: 'error', summary: 'Error!', detail: 'Data Not Saved' }); });
        } else if (pat != null && slip == null) {
          this.endpointService.setDataParam('t30023/save', t23List).subscribe((success: any) => {
            var dt = success;
            this.loading = false;
            if (dt.SLIP != '') {
              this.messageService.add({ severity: 'success', summary: 'Success!', detail: dt.MSG });
              this.userForm.get('txtSlipNo')?.setValue(dt.SLIP);
              this.onNextClicked('1');
            } else {
              this.messageService.add({ severity: 'error', summary: 'Error!', detail: dt.MSG });
            }
          }, error => { this.loading = false; this.messageService.add({ severity: 'error', summary: 'Error!', detail: 'Data Not Saved' }); });
        }

      } else {
        this.messageService.add({ severity: 'error', summary: 'Error!', detail: 'Please Add Medication' });
      }
    }
    else {
      this.loading = false;
      this.validateAllFormFields(this.userForm);
    }
  }
  onNextClicked(e: string) {
    if (e == '1') {
      var diag = this.returnValue(this.userForm.get('txtDiagonosis')?.value);
      if (diag == null) {
        this.messageService.add({ severity: 'error', summary: 'Error!', detail: 'Please select Diagonosis' });
        return;
      }
    }
    var pat = this.returnValue(this.userForm.get('txtPatNo')?.value);
    var slip = this.returnValue(this.userForm.get('txtSlipNo')?.value);
    this.loading = true;
    this.endpointService.getDataParam('t30023/getPatData', { pat: pat, slip: slip }).subscribe((success: any) => {
      var data = success;
      this.isSaveEnable = true;
      for (var i = 0; i < data.length; i++) {
        this.medList[i] = data[i];
        this.medList[i].DOSE_TIME_DAILY_DESC = data[i].T_DOSE_TIME_DAILY;
        this.medList[i].DOSE_DURATION_DESC = data[i].T_DOSE_DURATION;
        this.medList[i].T_COPY_PRS = false;
        this.medList[i].T_UM1 = data[i].T_ISSUE_UM;
        this.medList[i].T_UM_DESC1 = data[i].ISSUE_UM_DESC;
        this.medList[i].T_GEN_CODE = data[i].T_REQUEST_GCODE;
        this.medList[i].T_DRUG_ROUTE_CODE = data[i].T_REQUEST_ROUTE;
        this.medList[i].T_DRUG_FORM_CODE = data[i].T_REQUEST_MFORM;
        this.medList[i].T_DRUG_SFORM_CODE = data[i].T_REQUEST_SFORM;
        this.medList[i].T_STRENGTH = data[i].T_REQUEST_STRENGTH;
        this.onFrequencyOkClick(i);
        this.onDurationOkClick(i);

        //var qty = this.returnValue(this.medList[i].QTY) / 30;
        //var dur_qty = this.returnValue(this.medList[i].DUR_QTY);
        //if (this.medList[i].T_DOC_DOSE_UNIT == null || this.medList[i].T_DOC_DOSE_UNIT == undefined || this.medList[i].DOSE_TIME_DAILY_DESC == null || this.medList[i].DOSE_DURATION_DESC == null) {
        //  this.medList[i].T_QTY = '';
        //  return;
        //}
        //var final = Math.ceil(qty * dur_qty * this.medList[i].T_DOC_DOSE_UNIT);


        //if (mor != null || noo != null || nig != null) {
        //  this.medList[i].T_DOC_DOSE_UNIT = '';
        //} else {
        //  this.medList[i].T_QTY = final;
        //}
        //this.medList[i].T_ISSUE_UM = this.medList[i].T_UM1;
        //this.medList[i].ISSUE_UM_DESC = dt.T_UM_DESC1;

      }
      for (var i: number = data.length; i < 25 - data.length; i++) {
        this.medList[i] = {};
      }
      this.loading = false;
    }, error => { this.loading = false; return []; });
  }
  onBtnClearClick() {
    var patNo = this.userForm.get('txtPatNo')?.value;
    this.makeEmpty();
    this.onPatBlur(patNo, "2");
    for (var i = 0; i < 25; i++) {
      this.medList.push({ T_COPY_PRS: false });
    }
  }
  onBtnEnterClick() {
    this.enter = this.enter == 'enter' ? 'cancel' : 'enter';
    this.change(this.enter);
  }
  onBtnExecuteClick() {
    this.enter = 'exec';
    this.change(this.enter);
  }
  onBtnPrintClick() {
    let patNo = this.userForm.get('txtPatNo')?.value;
    let slipNo = this.userForm.get('txtSlipNo')?.value;
    if (slipNo == null || slipNo == undefined) {
      this.messageService.add({ severity: 'error', summary: 'Required!', detail: 'Slip no is required.' });
      return;
    }
    if (slipNo != '') {
      window.open("./api/t30023/getReportMedicineHistoryBySlipNo?patNo=" + patNo + "&slipNo=" + slipNo, "popup", "location=1, status=1, scrollbars=1");
    } else {
      window.open("./api/t30023/getReportMedicineHistoryBySlipNo?patNo=" + patNo + "&slipNo=" + slipNo, "popup", "location=1, status=1, scrollbars=1");
    }
  }
  validateAllFormFields(formGroup: FormGroup) {
    if (!formGroup.controls['txtPatNo'].valid) {
      formGroup.controls['txtPatNo'].markAsDirty();
      this.messageService.add({ severity: 'error', summary: document.getElementById('lblPat')?.innerText, detail: this.messages.find(x => x.CODE === 'S0313').TEXT });
    }
    if (!formGroup.controls['txtDocCode'].valid) {
      formGroup.controls['txtDocCode'].markAsDirty();
      this.messageService.add({ severity: 'error', summary: document.getElementById('lblPhysician')?.innerText, detail: this.messages.find(x => x.CODE === 'S0313').TEXT });
    }
    if (!formGroup.controls['txtDiagonosis'].valid) {
      formGroup.controls['txtDiagonosis'].markAsDirty();
      this.messageService.add({ severity: 'error', summary: document.getElementById('lblDiagonosis')?.innerText, detail: this.messages.find(x => x.CODE === 'S0313').TEXT });
    }
    if (!formGroup.controls['chkDelType'].valid) {
      formGroup.controls['chkDelType'].markAsDirty();
      this.messageService.add({ severity: 'error', summary: document.getElementById('lblMediDelivery')?.innerText, detail: this.messages.find(x => x.CODE === 'S0313').TEXT });
    }
  }
  onDocBlur() {
    var doc = this.userForm.get('txtDocCode')?.value;
    if (!this.docDisplay && (doc != null && doc != '')) {
      var spclty = this.returnValue(this.userForm.get('txtSpecialityCode')?.value);
      if (this.docList == null || this.docList.length == 0) {
        this.loading = true;
        this.endpointService.getDataParam('t30023/getDoc', { spclty: spclty }).subscribe((success: any) => {
          var dt = success;
          this.docList = dt;
          if (dt.length > 0) {
            var arr = dt.filter((a: { HDM_DOC_CODE: any; }) => a.HDM_DOC_CODE == doc.toUpperCase());
            if (arr.length > 0) {
              this.onDoctorRowDblClick(arr[0]);
            }
          }
          this.loading = false;
        }, error => { this.loading = false; return []; });
      } else {
        var x = (this.docList);
        if (x.length > 0) {
          var arr = x.filter((a: { HDM_DOC_CODE: any; }) => a.HDM_DOC_CODE == doc.toUpperCase());
          if (arr.length > 0) {
            this.onDoctorRowDblClick(arr[0]);
          }
        }
      }
    }
  }
  onDocDblClick() {
    if (this.docList == null || this.docList.length == 0) {
      var spclty = this.returnValue(this.userForm.get('txtSpecialityCode')?.value);
      this.loading = true;
      this.endpointService.getDataParam('t30023/getDoc', { spclty: spclty }).subscribe((success: any) => {
        var dt = success;
        this.docList = dt;
        this.docDisplay = true;
        this.loading = false;
      }, error => { this.loading = false; return []; });
    } else {
      this.docDisplay = true;
    }
  }
  onDocOkClick() {
    this.docDisplay = false;
    if (this.docSelectedData == null || this.docSelectedData == undefined || this.docSelectedData == []) {
      return;
    }
    this.onDoctorRowDblClick(this.docSelectedData);
    this.docSelectedData = [];
  }
  onDoctorRowDblClick(rowData: any) {
    this.userForm.get('txtDocCode')?.setValue(rowData.HDM_DOC_CODE);
    this.userForm.get('txtDocName')?.setValue(rowData.DOCTOR);
    this.userForm.get('txtSpecialityCode')?.setValue(rowData.HDM_SPCLTY_CODE);
    this.userForm.get('txtSpecialityDesc')?.setValue(rowData.HDM_SPCLTY_DSCRPTN);
    this.docDisplay = false;
  }

  onSpecialityBlur() {
    var spe = this.userForm.get('txtSpecialityCode')?.value;
    if (!this.spcltyDisplay && (spe != null && spe != '')) {
      if (this.spcltyList == null || this.spcltyList.length == 0) {
        this.loading = true;
        this.endpointService.getData('t30023/getSpeciality').subscribe((success: any) => {
          var dt = success;
          this.spcltyList = JSON.parse(dt);
          if (dt.length > 0) {
            var arr = dt.filter((a: { HDM_SPCLTY_CODE: any; }) => a.HDM_SPCLTY_CODE == spe.toUpperCase());
            if (arr.length > 0) {
              this.onSpcltyRowDblClick(arr[0]);
            }
          }
          this.loading = false;
        }, error => { this.loading = false; return []; });
      } else {
        var x = this.spcltyList;
        if (x.length > 0) {
          var arr = x.filter((a: { HDM_SPCLTY_CODE: any; }) => a.HDM_SPCLTY_CODE == spe.toUpperCase());
          if (arr.length > 0) {
            this.onSpcltyRowDblClick(arr[0]);
          }
        }
      }
    }
  }
  onSpcltyDblClick() {
    if (this.spcltyList == null || this.spcltyList.length == 0) {
      this.loading = true;
      this.endpointService.getData('t30023/getSpeciality').subscribe((success: any) => {
        var dt = success;
        this.spcltyList = JSON.parse(dt);
        this.spcltyDisplay = true;
        this.loading = false;
      }, error => { this.loading = false; return []; });
    } else {
      this.spcltyDisplay = true;
    }

  }
  onSpcltyOkClick() {
    this.spcltyDisplay = false;
    if (this.spcltySelectedData == null || this.spcltySelectedData == undefined || this.spcltySelectedData == []) {
      return;
    }
    this.onSpcltyRowDblClick(this.spcltySelectedData);
    this.spcltySelectedData = [];
  }
  onSpcltyRowDblClick(rowData: any) {
    this.userForm.get('txtSpecialityCode')?.setValue(rowData.HDM_SPCLTY_CODE);
    this.userForm.get('txtSpecialityDesc')?.setValue(rowData.HDM_SPCLTY_DSCRPTN);
    this.spcltyDisplay = false;
  }

  onLocationDblClick() {
    var type = this.returnValue(this.userForm.get('chkPatType')?.value);
    var doc = this.returnValue(this.userForm.get('txtDocCode')?.value);
    this.loading = true;
    this.endpointService.getDataParam('t30023/getLocation', { type: type, doc: doc }).subscribe((success: any) => {
      var dt = success;
      this.locationList = dt;
      this.locationDisplay = true;
      this.loading = false;
    }, error => { this.loading = false; return []; });
  }
  onLocationOkClick() {
    this.locationDisplay = false;
    if (this.locationSelectedData == null || this.locationSelectedData == undefined || this.locationSelectedData == []) {
      return;
    }
    this.onLocationRowDblClick(this.locationSelectedData);
    this.locationSelectedData = [];
  }
  onLocationRowDblClick(rowData: any) {
    this.userForm.get('txtLocation')?.setValue(rowData.STK_LOC);
    this.userForm.get('txtLocationDesc')?.setValue(rowData.STK_LOC_DESC);
    this.locationDisplay = false;
  }

  onICD10Blur() {
    var icd = this.userForm.get('txtICDCode')?.value;
    if (!this.icd10Display && (icd != null && icd != '')) {
      if (this.icd10List == null || this.icd10List.length == 0) {
        this.loading = true;
        this.endpointService.getData('t30023/getICD10').subscribe((success: any) => {
          var dt = success;
          this.icd10List = dt;
          if (dt.length > 0) {
            var arr = dt.filter((a: { CODE: any; }) => a.CODE == icd.toUpperCase());
            if (arr.length > 0) {
              this.onICD10RowDblClick(arr[0]);
            }
          }
          this.loading = false;
        }, error => { this.loading = false; return []; });
      } else {
        var x = this.icd10List;
        if (x.length > 0) {
          var arr = x.filter((a: { CODE: any; }) => a.CODE == icd.toUpperCase());
          if (arr.length > 0) {
            this.onICD10RowDblClick(arr[0]);
          }
        }
      }
    }
  }
  onICD10DblClick() {
    if (this.icd10List == null || this.icd10List.length == 0) {
      this.loading = true;
      this.endpointService.getData('t30023/getICD10').subscribe((success: any) => {
        var dt = success;
        this.icd10List = dt;
        this.icd10Display = true;
        this.loading = false;
      }, error => { this.loading = false; return []; });
    } else {
      this.icd10Display = true;
    }
  }
  onICD10OkClick() {
    this.icd10Display = false;
    if (this.icd10SelectedData == null || this.icd10SelectedData == undefined || this.icd10SelectedData == []) {
      return;
    }
    this.onICD10RowDblClick(this.icd10SelectedData);
    this.icd10SelectedData = [];
  }
  onICD10RowDblClick(rowData: any) {
    this.userForm.get('txtICDCode')?.setValue(rowData.CODE);
    this.userForm.get('txtICDDEsc')?.setValue(rowData.NAME);
    this.icd10Display = false;
  }
  onSlipBlur() {
    var doc = this.userForm.get('txtDocCode')?.value;
    var pat = this.userForm.get('txtPatNo')?.value;
    var slip = this.userForm.get('txtSlipNo')?.value;
    this.loading = true;
    this.endpointService.getDataParam('t30023/getSlipValidation', { doc: doc, pat: pat, slip: slip }).subscribe((success: any) => {
      var dt = JSON.parse(success);
      if (dt.length > 0) {
        this.loading = false;
        this.userForm.get('txtDocCode')?.setValue(dt[0].DOCCODE);
        this.userForm.get('txtDocName')?.setValue(dt[0].DOCNAME);
        this.userForm.get('txtSpecialityCode')?.setValue(dt[0].SPCLTYCODE);
        this.userForm.get('txtSpecialityDesc')?.setValue(dt[0].SPCLTYDESC);
        this.userForm.get('txtICDCode')?.setValue(dt[0].ICD10_CODE);
        this.userForm.get('txtICDDEsc')?.setValue(dt[0].ICD10_DESC);
        this.userForm.get('txtDiagonosis')?.setValue(dt[0].DIAGNOSIS);
        this.userForm.get('txtApptNo')?.setValue(dt[0].APPTNO);
      }
    }, error => { this.loading = false; return []; });
  }
  onSlipDblClick() {
    var slip = this.userForm.get('txtSlipNo')?.value;
    var pat = this.userForm.get('txtPatNo')?.value;
    var type = this.userForm.get('chkPatType')?.value;
    var clinic = this.userForm.get('txtLocation')?.value;
    var tempPatNo = '';
    this.loading = true;
    this.endpointService.getDataParam('t30023/getSlipList', { type: type, clinic: clinic, patNo: pat, tempPatNo: tempPatNo, slip: slip }).subscribe((success: any) => {
      var dt = success;
      this.slipList = dt;
      this.slipDisplay = true;
      this.loading = false;
    }, error => { this.loading = false; return []; });
  }
  onSlipOkClick() {
    this.slipDisplay = false;
    if (this.slipSelectedData == null || this.slipSelectedData == undefined || this.slipSelectedData == []) {
      return;
    }
    this.onSlipRowDblClick(this.slipSelectedData, '2');
    this.slipSelectedData = [];
  }
  onSlipRowDblClick(rowData: any, d: string) {
    this.slipDisplay = false;
    var pat = this.userForm.get('txtPatNo')?.value;
    var doc = rowData.PHM_ORGN_DOC;
    var slip = rowData.PHM_PAT_MEDICINE_SEQ;
    this.userForm.get('txtSlipNo')?.setValue(slip);
    this.userForm.get('txtVisitNo')?.setValue(rowData.T_VISIT_NO);
    this.userForm.get('chkDelType')?.setValue(rowData.T_DRUG_DLVRY_MTHD_CODE_DCTR);
    this.userForm.get('txtMobileNo')?.setValue(rowData.T_DOC_MOBILE_NO);
    this.onBtnEnterClick();
    this.endpointService.getDataParam('t30023/getSlipValidation', { doc: doc, pat: pat, slip: slip }).subscribe((success: any) => {
      var dt = success;
      if (dt.length > 0) {
        this.userForm.get('txtDocCode')?.setValue(dt[0].DOCCODE);
        this.userForm.get('txtDocName')?.setValue(dt[0].DOCNAME);
        this.userForm.get('txtSpecialityCode')?.setValue(dt[0].SPCLTYCODE);
        this.userForm.get('txtSpecialityDesc')?.setValue(dt[0].SPCLTYDESC);
        this.userForm.get('txtICDCode')?.setValue(dt[0].ICD10_CODE);
        this.userForm.get('txtICDDEsc')?.setValue(dt[0].ICD10_DESC);
        this.userForm.get('txtDiagonosis')?.setValue(dt[0].DIAGNOSIS);
        this.userForm.get('txtApptNo')?.setValue(dt[0].APPTNO);

        this.onNextClicked('2');
      } else {
        this.userForm.get('txtDocCode')?.setValue('');
        this.userForm.get('txtDocName')?.setValue('');
        this.userForm.get('txtSpecialityCode')?.setValue('');
        this.userForm.get('txtSpecialityDesc')?.setValue('');
        this.userForm.get('txtICDCode')?.setValue('');
        this.userForm.get('txtICDDEsc')?.setValue('');
        this.userForm.get('txtDiagonosis')?.setValue('');
        this.userForm.get('txtApptNo')?.setValue('');
        this.userForm.get('txtVisitNo')?.setValue('');
        this.loading = false;
      }
    }, error => { this.loading = false; return []; });
  }

  onMedicineDblClick(index: any) {
    if (this.medicineList == null || this.medicineList.length == 0) {
      this.tableIndex = '';
      this.loading = true;
      this.endpointService.getData('t30023/getMedicineList').subscribe((success: any) => {
        var dt = success;
        this.medicineList = JSON.parse(dt);
        this.medicineDisplay = true;
        this.loading = false;
        if (dt.length > 0) {
          this.tableIndex = index;
        }
      }, error => { this.loading = false; return []; });
    } else {
      this.medicineDisplay = true;
      if (this.medicineList.length > 0) {
        this.tableIndex = index;
      }
    }
  }
  onMedicineOkClick() {
    this.medicineDisplay = false;
    if (this.medicineSelectedData == null || this.medicineSelectedData == undefined || this.medicineSelectedData == []) {
      return;
    }
    this.onMedicineRowDblClick(this.medicineSelectedData);
    this.medicineSelectedData = [];
    this.tableIndex = '';
  }
  onMedicineRowDblClick(rowData: any) {
    this.medList[this.tableIndex].T_OPD_REQ_ITEM = rowData.DRUG_NAME;
    this.medList[this.tableIndex].T_DRUG_FORM_CODE = rowData.T_DRUG_FORM_CODE;
    this.medList[this.tableIndex].T_DRUG_ROUTE_CODE = rowData.T_DRUG_ROUTE_CODE;
    this.medList[this.tableIndex].T_DRUG_SFORM_CODE = rowData.T_DRUG_SFORM_CODE;
    this.medList[this.tableIndex].T_DRUG_SPCLTY_CODE = rowData.T_DRUG_SPCLTY_CODE;
    this.medList[this.tableIndex].T_GEN_CODE = rowData.T_GEN_CODE;
    this.medList[this.tableIndex].T_MOH_ITEM_CODE = rowData.T_MOH_ITEM_CODE;
    this.medList[this.tableIndex].T_STRENGTH = rowData.T_STRENGTH;
    this.medList[this.tableIndex].T_UM1 = rowData.T_UM;
    this.medList[this.tableIndex].T_UM_DESC1 = rowData.T_UM_DESC;
    this.medicineDisplay = false;

    this.onMedicineSelect(this.medList[this.tableIndex]);
    this.checkMedicineValidation(this.medList[this.tableIndex]);
  }

  onMedicineTradeDblClick(index: any) {
    if (this.medicineTradeList == null || this.medicineTradeList.length == 0) {
      this.tableIndex = '';
      this.loading = true;
      this.endpointService.getData('t30023/getMedicineListbyTrade').subscribe((success: any) => {
        var dt = success;
        this.medicineTradeList = dt;
        this.medicineTradeDisplay = true;
        this.loading = false;
        if (dt.length > 0) {
          this.tableIndex = index;
        }
      }, error => { this.loading = false; return []; });
    } else {
      this.medicineTradeDisplay = true;
      if (this.medicineTradeList.length > 0) {
        this.tableIndex = index;
      }
    }

  }
  onMedicineTradeOkClick() {
    this.medicineTradeDisplay = false;
    if (this.medicineTradeSelectedData == null || this.medicineTradeSelectedData == undefined || this.medicineTradeSelectedData == []) {
      return;
    }
    this.onMedicineTradeRowDblClick(this.medicineTradeSelectedData);
    this.medicineTradeSelectedData = [];
    this.tableIndex = '';
  }
  onMedicineTradeRowDblClick(rowData: any) {
    this.medList[this.tableIndex].T_OPD_REQ_ITEM = rowData.ITEM_DESC;
    this.medList[this.tableIndex].T_DRUG_FORM_CODE = rowData.T_DRUG_FORM_CODE;
    this.medList[this.tableIndex].T_DRUG_ROUTE_CODE = rowData.T_DRUG_ROUTE_CODE;
    this.medList[this.tableIndex].T_DRUG_SFORM_CODE = rowData.T_DRUG_SFORM_CODE;
    this.medList[this.tableIndex].T_DRUG_SPCLTY_CODE = rowData.T_DRUG_SPCLTY_CODE;
    this.medList[this.tableIndex].T_GEN_CODE = rowData.T_GEN_CODE;
    this.medList[this.tableIndex].T_MOH_ITEM_CODE = rowData.T_MOH_ITEM_CODE;
    this.medList[this.tableIndex].T_STRENGTH = rowData.T_STRENGTH;
    this.medList[this.tableIndex].T_UM1 = rowData.T_UM;
    this.medList[this.tableIndex].T_UM_DESC1 = rowData.T_UM_DESC;
    this.medicineTradeDisplay = false;

    this.onMedicineSelect(rowData.T_GEN_CODE);
  }

  onMedicineSpecialityDblClick(index: any) {
    if (this.medicineSpecialityList == null || this.medicineSpecialityList.length == 0) {
      var location = this.userForm.get('txtLocation')?.value;
      var speciality = this.userForm.get('txtSpecialityCode')?.value;
      this.tableIndex = '';
      this.loading = true;
      this.endpointService.getDataParam('t30023/getMedicineListbySpeciality', { speciality: speciality, location: location }).subscribe((success: any) => {
        var dt = success;
        this.medicineSpecialityList = dt;
        this.medicineSpecialityDisplay = true;
        if (dt.length > 0) {
          this.tableIndex = index;
        }
        this.loading = false;
      }, error => { this.loading = false; return []; });
    } else {
      this.medicineSpecialityDisplay = true;
      if (this.medicineSpecialityList.length > 0) {
        this.tableIndex = index;
      }
    }
  }
  onMedicineSpecialityOkClick() {
    this.medicineSpecialityDisplay = false;
    if (this.medicineSpecialitySelectedData == null || this.medicineSpecialitySelectedData == undefined || this.medicineSpecialitySelectedData == []) {
      return;
    }
    this.onMedicineSpecialityRowDblClick(this.medicineSpecialitySelectedData);
    this.medicineSpecialitySelectedData = [];
    this.tableIndex = '';
  }
  onMedicineSpecialityRowDblClick(rowData: any) {
    this.medList[this.tableIndex].T_OPD_REQ_ITEM = rowData.ITEM_DESC;
    this.medList[this.tableIndex].T_DRUG_FORM_CODE = rowData.T_DRUG_FORM_CODE;
    this.medList[this.tableIndex].T_DRUG_ROUTE_CODE = rowData.T_DRUG_ROUTE_CODE;
    this.medList[this.tableIndex].T_DRUG_SFORM_CODE = rowData.T_DRUG_SFORM_CODE;
    this.medList[this.tableIndex].T_DRUG_SPCLTY_CODE = rowData.T_DRUG_SPCLTY_CODE;
    this.medList[this.tableIndex].T_GEN_CODE = rowData.T_GEN_CODE;
    this.medList[this.tableIndex].T_MOH_ITEM_CODE = rowData.T_MOH_ITEM_CODE;
    this.medList[this.tableIndex].T_STRENGTH = rowData.T_STRENGTH;
    this.medList[this.tableIndex].T_UM1 = rowData.T_UM;
    this.medList[this.tableIndex].T_UM_DESC1 = rowData.T_UM_DESC;
    this.medicineSpecialityDisplay = false;

    this.onMedicineSelect(this.medList[this.tableIndex]);
    this.checkMedicineValidation(this.medList[this.tableIndex]);
  }

  onMedicineSelect(rowData: any) {
    this.medInsMsg = '';
    var docCode = this.userForm.get('txtDocCode')?.value;
    var specCode = this.userForm.get('txtSpecialityCode')?.value;
    var paramList = { docCode: docCode, specCode: specCode, genCode: rowData.T_GEN_CODE, routeCode: rowData.T_DRUG_ROUTE_CODE, mFormCode: rowData.T_DRUG_FORM_CODE }
    this.endpointService.getDataParam('t30023/getSpecialityIns', paramList).subscribe((success: any) => {
      var dt = success;
      if (dt != null) {
        if (dt.length > 0) {
          this.medInsBool = true;
          this.medInsMsg = dt;
        }
      } else {
        this.medInsBool = false;
        this.medInsMsg = '';
      }
    }, error => { this.loading = false; return []; });
  }

  onFrequencyOkClick(rowIndex: any) {
    this.tableIndex = rowIndex;
    if (this.medList[rowIndex].DOSE_TIME_DAILY_DESC != null) {
      var frequency = this.frequencyList.find((x: { T_FREQUENCY_CODE: any; }) => x.T_FREQUENCY_CODE == this.medList[rowIndex].DOSE_TIME_DAILY_DESC);
      this.onFrequencyRowDblClick(frequency);
      this.onDoseBlur(rowIndex);
    }
    else {
      this.medList[this.tableIndex].T_DOSE_DURATION = '';
      this.medList[this.tableIndex].DUR_QTY = '';
      this.medList[this.tableIndex].T_DOC_DOSE_UNIT = '';
      this.medList[this.tableIndex].T_QTY = '';
    }
  }
  onFrequencyRowDblClick(rowData: any) {
    this.medList[this.tableIndex].T_DOSE_TIME_DAILY = rowData.T_FREQUENCY_CODE;
    this.medList[this.tableIndex].QTY = rowData.QTY;
    var con = ['18', '19', '20', '21'];
    var x = con.indexOf(rowData.T_FREQUENCY_CODE) > -1;
    if (x) {
      this.daysDisplay = true;
      var sat = this.medList[this.tableIndex].T_SATURDAY;
      var sun = this.medList[this.tableIndex].T_SUNDAY;
      var mon = this.medList[this.tableIndex].T_MONDAY;
      var tue = this.medList[this.tableIndex].T_TUESDAY;
      var wed = this.medList[this.tableIndex].T_WEDNESDAY;
      var thu = this.medList[this.tableIndex].T_THURSDAY;
      var fri = this.medList[this.tableIndex].T_FRIDAY;
      this.daysForm.get('chkSaturday')?.setValue(sat == null ? ['sat'] : ['sat', sat]);
      this.daysForm.get('chkSunday')?.setValue(sun == null ? ['sun'] : ['sun', sun]);
      this.daysForm.get('chkMonday')?.setValue(mon == null ? ['mon'] : ['mon', mon]);
      this.daysForm.get('chkTuesday')?.setValue(tue == null ? ['tue'] : ['tue', tue]);
      this.daysForm.get('chkWednesday')?.setValue(wed == null ? ['wed'] : ['wed', wed]);
      this.daysForm.get('chkThursday')?.setValue(thu == null ? ['thu'] : ['thu', thu]);
      this.daysForm.get('chkFriday')?.setValue(fri == null ? ['fri'] : ['fri', fri]);
    }
  }

  onDurationOkClick(rowIndex: any) {
    this.tableIndex = rowIndex;
    if (this.medList[rowIndex].DOSE_DURATION_DESC != null) {
      var duration = this.durationList.find((x: { T_DOSE_DURATION_CODE: any; }) => x.T_DOSE_DURATION_CODE == this.medList[rowIndex].DOSE_DURATION_DESC);
      this.onDurationRowDblClick(duration);
      this.onDoseBlur(rowIndex);
    }
    else {
      this.medList[this.tableIndex].T_DOSE_DURATION = '';
      this.medList[this.tableIndex].DUR_QTY = '';
      this.medList[this.tableIndex].T_DOC_DOSE_UNIT = '';
      this.medList[this.tableIndex].T_QTY = '';
    }
  }
  onDurationRowDblClick(rowData: any) {
    this.medList[this.tableIndex].T_DOSE_DURATION = rowData.T_DOSE_DURATION_CODE;
    this.medList[this.tableIndex].DUR_QTY = rowData.DUR_QTY;
  }
  onUMDblClick(rowIndex: any) {
    this.tableIndex = rowIndex;
    if (this.umList == null || this.umList.length == 0) {
      this.loading = true;
      this.endpointService.getData('t30023/getUMList').subscribe((success: any) => {
        var dt = success;
        this.umList = dt;
        this.umDisplay = true;
        this.loading = false;
      }, error => { this.loading = false; return []; });
    } else {
      this.umDisplay = true;
    }
  }
  onUMOkClick() {
    this.umDisplay = false;
    if (this.umSelectedData == null || this.umSelectedData == undefined || this.umSelectedData == []) {
      return;
    }
    this.onUMRowDblClick(this.umSelectedData);
    this.umSelectedData = [];
    this.tableIndex = '';
  }
  onUMRowDblClick(rowData: any) {
    this.umDisplay = false;
    this.medList[this.tableIndex].ISSUE_UM_DESC = rowData.UM_SHORT_DESC;
    this.medList[this.tableIndex].T_ISSUE_UM = rowData.T_UM;
  }

  onDrugMasterDblClick() {
    if (this.drugMasterList != null && this.drugMasterList.length > 0) {
      this.drugMasterDisplay = true;
      return;
    }
    this.loading = true;
    this.endpointService.getData('t30023/getTradeGen').subscribe((success: any) => {
      var dt = success;
      this.drugMasterList = dt;
      this.drugMasterDisplay = true;
      this.loading = false;
    }, error => {

      this.loading = false;
      return [];
    });
  }
  onDrugMasterOkClick() {
    this.drugMasterDisplay = false;
    if (this.drugMasterSelectedData == null || this.drugMasterSelectedData == undefined || this.drugMasterSelectedData == []) {
      return;
    }
    this.onDrugMasterRowDblClick(this.drugMasterSelectedData);
    this.drugMasterSelectedData = [];
  }
  onDrugMasterRowDblClick(rowData: any) {
    this.drugMasterDisplay = false;
    this.genericForm.get('txtTradeCode')?.setValue(rowData.PHM_DRUG_MASTER_CODE);
    this.genericForm.get('txtTradeDesc')?.setValue(rowData.PHM_ITEM_DSCRPTN);
    this.genericForm.get('txtGenCode')?.setValue(rowData.T_GEN_CODE);
    this.genericForm.get('txtGenDesc')?.setValue(rowData.GENERIC_DESC);
  }

  onInsDblClick(rowData: any, rowIndex: any) {
    this.tableIndex = rowIndex;
    this.insDisplay = true;
    this.insForm.get('txtMorningIns')?.setValue(null);
    this.insForm.get('txtNoonIns')?.setValue(null);
    this.insForm.get('txtNightIns')?.setValue(null);
    this.insForm.get('txtMorningIns')?.setValue(this.medList[rowIndex].T_MORNING_DOSE_UNIT);
    this.insForm.get('txtNoonIns')?.setValue(this.medList[rowIndex].T_NOON_DOSE_UNIT);
    this.insForm.get('txtNightIns')?.setValue(this.medList[rowIndex].T_NIGHT_DOSE_UNIT);
    this.insForm.get('ddlMorningIns')?.setValue({ T_INSTRUCTION_CODE: this.medList[rowIndex].T_MORNING_INSTRUCTION, NAME: this.medList[rowIndex].T_MORNING_INS_DESC });
    this.insForm.get('ddlNoonIns')?.setValue({ T_INSTRUCTION_CODE: this.medList[rowIndex].T_NOON_INSTRUCTION, NAME: this.medList[rowIndex].T_NOON_INS_DESC });
    this.insForm.get('ddlNightIns')?.setValue({ T_INSTRUCTION_CODE: this.medList[rowIndex].T_NIGHT_INSTRUCTION, NAME: this.medList[rowIndex].T_NIGHT_INS_DESC });

  }
  onInsOkClick() {
    this.medList[this.tableIndex].T_MORNING_DOSE_UNIT = this.insForm.get('txtMorningIns')?.value;
    this.medList[this.tableIndex].T_NOON_DOSE_UNIT = this.insForm.get('txtNoonIns')?.value;
    this.medList[this.tableIndex].T_NIGHT_DOSE_UNIT = this.insForm.get('txtNightIns')?.value;
    this.medList[this.tableIndex].T_MORNING_INSTRUCTION = this.insForm.get('ddlMorningIns')?.value.T_INSTRUCTION_CODE;
    this.medList[this.tableIndex].T_NOON_INSTRUCTION = this.insForm.get('ddlNoonIns')?.value.T_INSTRUCTION_CODE;
    this.medList[this.tableIndex].T_NIGHT_INSTRUCTION = this.insForm.get('ddlNightIns')?.value.T_INSTRUCTION_CODE;
    this.insDisplay = false;
  }
  onDaysOkClick() {
    var sat = this.daysForm.get('chkSaturday')?.value;
    var sun = this.daysForm.get('chkSunday')?.value;
    var mon = this.daysForm.get('chkMonday')?.value;
    var tue = this.daysForm.get('chkTuesday')?.value;
    var wed = this.daysForm.get('chkWednesday')?.value;
    var thu = this.daysForm.get('chkThursday')?.value;
    var fri = this.daysForm.get('chkFriday')?.value;
    this.medList[this.tableIndex].T_SATURDAY = sat.length > 0 ? sat[1] : null;
    this.medList[this.tableIndex].T_SUNDAY = sun.length > 0 ? sun[1] : null;
    this.medList[this.tableIndex].T_MONDAY = mon.length > 0 ? mon[1] : null;
    this.medList[this.tableIndex].T_TUESDAY = tue.length > 0 ? tue[1] : null;
    this.medList[this.tableIndex].T_WEDNESDAY = wed.length > 0 ? wed[1] : null;
    this.medList[this.tableIndex].T_THURSDAY = thu.length > 0 ? thu[1] : null;
    this.medList[this.tableIndex].T_FRIDAY = fri.length > 0 ? fri[1] : null;

    this.daysDisplay = false;
  }
  onDoseBlur(i: any) {
    var mor = this.returnValue(this.medList[i].T_MORNING_DOSE_UNIT);
    var noo = this.returnValue(this.medList[i].T_NOON_DOSE_UNIT);
    var nig = this.returnValue(this.medList[i].T_NIGHT_DOSE_UNIT);

    var qty = this.returnValue(this.medList[i].QTY) / 30;
    var dur_qty = this.returnValue(this.medList[i].DUR_QTY);
    if (this.medList[i].T_DOC_DOSE_UNIT == null || this.medList[i].T_DOC_DOSE_UNIT == undefined || this.medList[i].DOSE_TIME_DAILY_DESC == null || this.medList[i].DOSE_DURATION_DESC == null) {
      this.medList[i].T_QTY = '';
      return;
    }
    var final = Math.ceil(qty * dur_qty * this.medList[i].T_DOC_DOSE_UNIT);

    if (mor != null || noo != null || nig != null) {
      this.medList[i].T_DOC_DOSE_UNIT = '';
    } else {
      this.medList[i].T_QTY = final;
    }
    this.medList[i].T_ISSUE_UM = this.medList[i].T_UM1;
    this.medList[i].ISSUE_UM_DESC = this.medList[i].T_UM_DESC1;
  }
  change(e: string) {
    if (e == 'enter') {
      this.userForm.get('txtSlipNo')?.enable();
      this.slipNo.nativeElement.focus();

      this.userForm.get('txtSlipNo')?.setValue('');
      this.userForm.get('txtDiagonosis')?.setValue('');
      this.insForm.reset();
      this.daysForm.reset();
      //this.medList = [];
    }
    if (e == 'cancel') {
      this.userForm.get('txtSlipNo')?.disable();
    }
    if (e == 'exec') {
      this.userForm.get('txtSlipNo')?.disable();

      var slip = this.userForm.get('txtSlipNo')?.value;
      var pat = this.userForm.get('txtPatNo')?.value;
      var type = this.userForm.get('chkPatType')?.value;
      var clinic = this.userForm.get('txtLocation')?.value;
      var tempPatNo = '';
      this.loading = true;
      //get doctor info
      this.endpointService.getDataParam('t30023/getSlipList', { type: type, clinic: clinic, patNo: pat, tempPatNo: tempPatNo, slip: slip }).subscribe((success: any) => {
          var dt = success;
          var slip = this.userForm.get('txtSlipNo')?.value;
          if (dt.length > 0) {
            if (slip != null && slip != '') {
              var xt = dt.filter((a: { PHM_PAT_MEDICINE_SEQ: any; }) => a.PHM_PAT_MEDICINE_SEQ == slip);
              if (xt.length > 0) {
                //  this.onSlipRowDblClick(xt[0], '1');
                //  var pat = this.userForm.get('txtPatNo')?.value;
                var doc = xt[0].PHM_ORGN_DOC;
                pat = xt[0].T_PAT_NO
                this.endpointService.getDataParam('t30023/getSlipValidation', { doc: doc, pat: pat, slip: slip }).subscribe((success: any) => {
                  var dt = success;
                  if (dt.length > 0) {
                    // this.loading = false;
                    this.userForm.get('txtDocCode')?.setValue(dt[0].DOCCODE);
                    this.userForm.get('txtDocName')?.setValue(dt[0].DOCNAME);
                    this.userForm.get('txtSpecialityCode')?.setValue(dt[0].SPCLTYCODE);
                    this.userForm.get('txtSpecialityDesc')?.setValue(dt[0].SPCLTYDESC);
                    this.userForm.get('txtICDCode')?.setValue(dt[0].ICD10_CODE);
                    this.userForm.get('txtICDDEsc')?.setValue(dt[0].ICD10_DESC);
                    this.userForm.get('txtDiagonosis')?.setValue(dt[0].DIAGNOSIS);
                    this.userForm.get('txtApptNo')?.setValue(dt[0].APPTNO);

                    this.endpointService.getDataParam('t30023/GetPatInfoT03001', { patNo: pat }).subscribe((success: any) => {
                      var dp = success;
                      if (dp.length > 0) {
                        this.userForm.get('txtPatNo')?.setValue(dp[0].T_PAT_NO);
                        this.userForm.get('txtPatName')?.setValue(dp[0].T_PAT_NAME);
                        this.userForm.get('txtSex')?.setValue(dp[0].T_GENDER);
                        this.userForm.get('txtAge')?.setValue(dp[0].YEARS);
                        this.userForm.get('txtNationality')?.setValue(dp[0].NATIONALITY);
                      } else {
                        this.messageService.add({ severity: 'error', summary: 'Error!', detail: 'No Data found...' });
                        this.loading = false;
                      }
                    });
                      this.onNextClicked('2');
                    //this.endpointService.getDataParam('t30023/getPatData', { pat: pat, slip: slip }).subscribe((success: any) => {
                    //  var dt = success;
                    //  debugger;
                    //  this.medList = dt;
                    //  this.loading = false;
                    //}, error => { this.loading = false; return []; });
                  }
                }, error => { this.loading = false; return []; });
                ////////////             
                //  this.loading = false;
              } else {
                this.messageService.add({ severity: 'error', summary: 'Error!', detail: 'No Data found..' });
              }
            } else {
              this.messageService.add({ severity: 'error', summary: 'Error!', detail: 'Slip No can not be empty' });
            }
          } else {
            this.messageService.add({ severity: 'error', summary: 'Error!', detail: 'No Data found...' });
          }
          //  this.loading = false;
        }, error => { this.loading = false; return []; });
    }
  }
  onPatBlur1() {
    var pat = this.userForm.get('txtPatNo')?.value;
    if (!this.displayAdmitted && (pat != null && pat != '')) {
      this.onPatBlur(pat, '1');
    }
  }
  onPatBlur(pat: any, e: string) {
    this.loading = true;
    this.endpointService.getDataParam('t30023/GetPatInfoT03001', { patNo: pat }).subscribe((success: any) => {
      if (success.validity != "0") {
        this.loading = false;
        this.messageService.add({ severity: 'error', summary: 'Error!', detail: success.msg });
        return;
      }
      var dp = success.data.value;
      if (dp.length > 0) {

        this.loading = false;
        this.userForm.get('txtPatNo')?.setValue(dp[0].T_PAT_NO);
        this.userForm.get('txtPatName')?.setValue(dp[0].T_PAT_NAME);
        this.userForm.get('txtSex')?.setValue(dp[0].T_GENDER);
        this.userForm.get('txtAge')?.setValue(dp[0].YEARS);
        this.userForm.get('txtNationality')?.setValue(dp[0].NATIONALITY);
        this.userForm.get('chkPatType')?.setValue(dp[0].T_TYPE_CODE);
        if (dp[0].T_WEIGHT != null || dp[0].T_HEIGHT != null) {
          this.userForm.get('txtHeight')?.setValue(dp[0].T_HEIGHT);
          this.userForm.get('txtWeight')?.setValue(dp[0].T_WEIGHT);
        } else {
          this.userForm.get('txtHeight')?.setValue(dp[0].T_HEIGHT);
          this.userForm.get('txtWeight')?.setValue(dp[0].T_WEIGHT);
        }

        this.userForm.get('txtDocCode')?.setValue(dp[0].T_CLINIC_DOC_CODE);
        var docName = this.userLang == '2' ? dp[0].DOC_NAME : dp[0].DOC_NAME_A;
        this.userForm.get('txtDocName')?.setValue(docName);
        this.userForm.get('txtSpecialityCode')?.setValue(dp[0].T_SPCLTY_CODE);
        this.userForm.get('txtSpecialityDesc')?.setValue(dp[0].SPCLTY);
        this.userForm.get('txtLocation')?.setValue(dp[0].T_CLINIC_CODE);
        this.userForm.get('txtLocationDesc')?.setValue(dp[0].CLINIC_NAME);
        this.userForm.get('txtVisitNo')?.setValue(dp[0].T_VISIT_NO);
        this.userForm.get('txtApptNo')?.setValue(dp[0].T_APPT_NO);

      } else {
        this.loading = false;
        this.messageService.add({ severity: 'error', summary: 'Error!', detail: 'No Data found..' });
      }
    }, error => { this.loading = false; return []; });
    if (e == '1') {
      this.makeEmpty2();
      this.userForm.get('txtSlipNo')?.enable();
      this.userForm.get('txtSlipNo')?.setValue('');
      this.userForm.get('txtSlipNo')?.disable();
    }
  }
  onAddClick() {
    var diag = this.returnValue(this.userForm.get('txtDiagonosis')?.value);
    if (diag != null) {
      this.medList.push({});
    } else {
      this.messageService.add({ severity: 'error', summary: 'Error!', detail: 'Please Enter Diagonosis' });
    }
  }
  onCloseClick(index: any) {
    this.medList.splice(index, 1);
  }
  returnValue(val: any) {
    if (val) {
      return val;
    } else {
      return null;
    }
  }
  getGenericInfo() {
    this.genericDisplay = true;
  }
  getDrugHistory() {
    var pat = this.returnValue(this.userForm.get('txtPatNo')?.value);
    //var app = this.returnValue(this.route.snapshot.queryParamMap.get('app'));
    //var fromDate = this.route.snapshot.queryParamMap.get('fromDate');
    //var toDate = this.route.snapshot.queryParamMap.get('toDate');
    if (pat == null) {
      this.messageService.add({ severity: 'error', summary: 'Error!', detail: 'Please select a Patient' });
      return;
    }
    this.ngxService.start();
    this.endpointService.getDataParam('t30023/getDrugHistory', { patNo: pat }).subscribe((success: any) => {
      var dt = success;
      if (dt != null) {
        this.medicineListAsOutPatient = dt.drug_out_pat;
        this.medicineListAsInPatient = dt.drug_in_pat;
        this.drugHistoryDisplay = true;
      } else {
        this.messageService.add({ severity: 'error', summary: 'Error!', detail: 'No Data found...' });
      }
      this.ngxService.stop();
    }, error => {
      this.loading = false;
      return [];
    });
  }
  chkInactive(par: any) {
    if (par === null) {
      return null;
    }
    else if (par.length == 0) {
      return null;
    }
    else if (par.length == 1) {
      return par[0];
    }
  }
  ondbClick(rowData: any) {
    this.onPatBlur(rowData.T_PAT_NO, '1');
    this.displayAdmitted = false;
    this.selectedPatData = [];
  }
  onOkPatPopUpClick() {
    this.onPatBlur(this.selectedPatData.T_PAT_NO, '1');
    this.displayAdmitted = false;
    this.selectedPatData = [];
  }
  onLabelLoad(e: string) {
    try {
      return this.labelsNew.filter((a: { T_LABEL_NAME: string; }) => a.T_LABEL_NAME == e)[0].T_LABEL_TEXT;
    } catch (e) {
      return '';
    }
  }
  loadPatients(event: LazyLoadEvent) {
    console.log('first' + event.first);
    console.log('rows' + event.rows);
    //this.onPatDblClick();
  }
  changeButtonStatus() {
    this.isEditable = false;
  }
  setPermission(permissions: any) {
    if (permissions.canOpen) {
      this.canSave = permissions.canSave;
      this.canUpdate = permissions.canUpdate;
      this.canDelete = permissions.canDelete;
      this.canQuery = permissions.canQuery;
    }
    else {
      this.messageService.add({ severity: 'error', summary: 'No permission!', detail: this.messages.find(x => x.CODE === 'S0360').TEXT });
      document.getElementById('btnLogout')?.click();
    }
  }
  getEventValue($event: any): string {
    return $event.target.value;
  }
  checkMedicineValidation(rowData: any) {
    if (rowData.T_OPD_REQ_ITEM != null) {
      if (rowData.T_DRUG_SPCLTY_CODE != null) {
        if (rowData.T_DRUG_SPCLTY_CODE == '0115') {
          this.messageService.add({ severity: 'error', summary: 'Attention!', detail: 'Attention Please !!!!!!!!!!! Cardiology Medication' });
          return;
        }
        else if (rowData.T_DRUG_SPCLTY_CODE == '0113') {
          this.messageService.add({ severity: 'error', summary: 'Attention!', detail: 'Attention Please !!!!!!!!!!! Oncology Medication' });
          return;
        }
        else if (rowData.T_DRUG_SPCLTY_CODE == '0106') {
          this.messageService.add({ severity: 'error', summary: 'Attention!', detail: 'Attention Please !!!!!!!!!!! Psychiatric Medication' });
          return;
        }
      }
      this.endpointService.getDataParam('t30023/getMedicineStatus', { itemCode: rowData.T_MOH_ITEM_CODE, strength: rowData.T_STRENGTH, routeCode: rowData.T_DRUG_ROUTE_CODE, formCode: rowData.T_DRUG_FORM_CODE, genCode: rowData.T_GEN_CODE }).subscribe((success: any) => {
        var dp = success;
        if (dp.IS_RISK == '1') {
          this.messageService.add({ severity: 'error', summary: 'HIGH RISK!', detail: 'HIGH RISK Medicine...........Please Re-Check!!' });
        }
        else if (dp.IS_ANTIOBIOTIC == '1') {
          this.messageService.add({ severity: 'error', summary: 'Caution!', detail: 'Caution...........You are prescribing an Antibiotic Drug!!' });
        }
      }, error => { console.log(error); return []; });
    }
  }
  onCheckDuplicate(rowIndex: number) {
    var one_day = 1000 * 60 * 60 * 24
    var present_date = new Date();
    var entry_day = new Date(this.medList[rowIndex].T_ENTRY_DATE);
    if (present_date.getMonth() == 11 && present_date.getDate() > 25)
      entry_day.setFullYear(entry_day.getFullYear() + 1)
    var Result = Math.round(entry_day.getTime() - present_date.getTime()) / (one_day);
    const Final_Result = Number(Result.toFixed(0)) + Number(this.medList[rowIndex].DUR_QTY);
    return Final_Result > 0 ? true : false;
  }
  onCopyChange($event: any, rowIndex: number) {
    if ($event.checked === true) {
      if (this.onCheckDuplicate(rowIndex)) {
       var slp= this.returnValue(this.userForm.get('txtSlipNo')?.value);
        var msg = "Duplicate Drug found for: " + this.medList[rowIndex].T_OPD_REQ_ITEM + ".<br/> Please check slip#" + slp +".Duration not yet completed.<br/>Please stop/inactive the old medication first.<br/>Do you want to continue?";
        this.confirmationService.confirm({
          message: msg,
          header: 'Duplicate drug found!!',
          icon: 'pi pi-exclamation-triangle',
          accept: () => {
            this.isSaveEnable = false;
            this.medList[rowIndex].IS_PERMITTED = true;
          },
          reject: () => {
            this.medList[rowIndex].T_COPY_PRS = false;
          }
        });        
      }
      else {
        this.isSaveEnable = false;
        this.medList[rowIndex].IS_PERMITTED = true;
      }
    }
    else {
      this.isSaveEnable = true;
      this.medList[rowIndex].IS_PERMITTED = false;
      for (var i = 0; i < this.medList.length; i++) {
        if (rowIndex != i) {
          if (this.medList[i].T_OPD_REQ_ITEM != null && this.medList[i].T_COPY_PRS != false) {
            if (this.medList[i].T_COPY_PRS) {
              this.isSaveEnable = false;
              return;
            }
          }
        }
      }
    }
  }
  onCopyAllChange($event: any) {
    if ($event.checked) {
      this.isSaveEnable = false;
      for (var i = 0; i < this.medList.length; i++) {
        if (this.medList[i].T_OPD_REQ_ITEM != null && this.medList[i].IS_PERMITTED == false && this.medList[i].T_PAT_MEDICINE_SEQ != undefined) {
          this.medList[i].IS_PERMITTED = true;
          this.medList[i].T_COPY_PRS = true;
        }        
      }
    }
    else {
      for (var i = 0; i < this.medList.length; i++) {
        if (this.medList[i].T_OPD_REQ_ITEM != null && this.medList[i].IS_PERMITTED == true && this.medList[i].T_PAT_MEDICINE_SEQ != undefined) {
          this.medList[i].IS_PERMITTED = false;
          this.medList[i].T_COPY_PRS = false;
        }
      }
      this.isSaveEnable = true;
    }
  }
  onCopyClicked() {
    debugger;
    var slipNo = this.returnValue(this.userForm.get('txtSlipNo')?.value);
    var isCopied = this.medList.filter((a: { T_COPY_PRS: any; }) => a.T_COPY_PRS == true);
    if (isCopied.length == 0 || slipNo == null) {
      this.messageService.add({ severity: 'error', summary: 'Error!', detail: 'Please select the drugs to be copied first!!' });
      return;
    }
    this.confirmationService.confirm({
      message: 'You are going to prescribe same Prescription. Are you sure?',
      header: 'Confirmation',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.onCopyDataSave();
      },
      reject: () => {
        return;
      }
    });
  }
  onCopyDataSave() {
    if (this.userForm.valid) {
      var dtList = this.medList;
      if (dtList.length > 0) {
        const t23List: T30023[] = [];
        var isCopied = dtList.filter((a: { T_COPY_PRS: any; }) => a.T_COPY_PRS == true);
        if (isCopied == null) {
          this.messageService.add({ severity: 'error', summary: 'Error!', detail: 'Please select the drugs to be copied first!!' });
          return;
        }       
        this.ngxService.start();
        this.endpointService.getDataParam('t30023/GetPatInfoT03001', { patNo: this.returnValue(this.userForm.get('txtPatNo')?.value) }).subscribe((success: any) => {
          var dp = success.data.value;
          this.userForm.get('txtDocCode')?.setValue(dp[0].T_CLINIC_DOC_CODE);
          var docName = this.userLang == '2' ? dp[0].DOC_NAME : dp[0].DOC_NAME_A;
          this.userForm.get('txtDocName')?.setValue(docName);
          this.userForm.get('txtSpecialityCode')?.setValue(dp[0].T_SPCLTY_CODE);
          this.userForm.get('txtSpecialityDesc')?.setValue(dp[0].SPCLTY);
          this.userForm.get('txtLocation')?.setValue(dp[0].T_CLINIC_CODE);
          this.userForm.get('txtLocationDesc')?.setValue(dp[0].CLINIC_NAME);
          this.userForm.get('txtVisitNo')?.setValue(dp[0].T_VISIT_NO);
          this.userForm.get('txtApptNo')?.setValue(dp[0].T_APPT_NO);
          for (var i = 0; i < dtList.length; i++) {
            var t23 = new T30023();
            t23.pt_pat_no = this.returnValue(this.userForm.get('txtPatNo')?.value);
            t23.pt_pat_medicine_seq = this.returnValue(this.userForm.get('txtSlipNo')?.value);
            t23.pt_pat_type = this.returnValue(this.userForm.get('chkPatType')?.value);
            t23.pt_pat_weight = this.returnValue(this.userForm.get('txtWeight')?.value);
            t23.pt_pat_height = this.returnValue(this.userForm.get('txtHeight')?.value);
            t23.pt_orgn_doc = this.returnValue(this.userForm.get('txtDocCode')?.value);
            t23.pt_stk_loc_code = this.returnValue(this.userForm.get('txtSpecialityCode')?.value);
            t23.pt_diagnosis = this.returnValue(this.userForm.get('txtDiagonosis')?.value);
            t23.pt_appt_req_id = this.returnValue(this.userForm.get('txtApptNo')?.value);
            t23.pt_icd10_diag_code = this.returnValue(this.userForm.get('txtICDCode')?.value);
            t23.pt_presc_loc = this.returnValue(this.userForm.get('txtLocation')?.value);
            t23.T_DOC_MOBILE_NO = this.returnValue(this.userForm.get('txtMobileNo')?.value);
            t23.T_DRUG_DLVRY_MTHD_CODE_DCTR = this.returnValue(this.userForm.get('chkDelType')?.value);
            t23.pt_opd_req_item = this.returnValue(dtList[i].T_OPD_REQ_ITEM);
            t23.pt_dose_time_daily = this.returnValue(dtList[i].T_DOSE_TIME_DAILY);
            t23.pt_dose_duration = this.returnValue(dtList[i].T_DOSE_DURATION);
            t23.pt_morning_dose_unit = this.returnValue(dtList[i].T_MORNING_DOSE_UNIT);
            t23.pt_noon_dose_unit = this.returnValue(dtList[i].T_NOON_DOSE_UNIT);
            t23.pt_night_dose_unit = this.returnValue(dtList[i].T_NIGHT_DOSE_UNIT);
            t23.pt_morning_instruction = this.returnValue(dtList[i].T_MORNING_INSTRUCTION);
            t23.pt_noon_instruction = this.returnValue(dtList[i].T_NOON_INSTRUCTION);
            t23.pt_night_instruction = this.returnValue(dtList[i].T_NIGHT_INSTRUCTION);
            t23.pt_issue_um = this.returnValue(dtList[i].T_ISSUE_UM);
            t23.pt_issue_date = this.returnValue(dtList[i].T_ISSUE_DATE);
            t23.pt_doctor_issue_um = this.returnValue(dtList[i].T_ISSUE_UM);
            t23.pt_qty = this.returnValue(dtList[i].T_QTY);
            t23.pt_qty_remaining = this.returnValue(dtList[i].T_QTY);
            t23.pt_remarks = this.returnValue(dtList[i].T_REMARKS);
            t23.pt_doc_dose_unit = this.returnValue(dtList[i].T_DOC_DOSE_UNIT);
            t23.pt_dose_unit = this.returnValue(dtList[i].T_DOC_DOSE_UNIT);
            t23.pt_moh_item_code = this.returnValue(dtList[i].T_MOH_ITEM_CODE);
            t23.pt_request_strength = this.returnValue(dtList[i].T_STRENGTH);
            t23.pt_request_route = this.returnValue(dtList[i].T_DRUG_ROUTE_CODE);
            t23.pt_request_mform = this.returnValue(dtList[i].T_DRUG_FORM_CODE);
            t23.pt_request_sform = this.returnValue(dtList[i].T_DRUG_SFORM_CODE);
            t23.pt_request_gcode = this.returnValue(dtList[i].T_GEN_CODE);
            t23.pt_drug_inactive_flag = this.chkInactive(this.returnValue(dtList[i].T_DRUG_INACTIVE_FLAG));
            t23.pt_saturday = this.returnValue(dtList[i].T_SATURDAY);
            t23.pt_sunday = this.returnValue(dtList[i].T_SUNDAY);
            t23.pt_monday = this.returnValue(dtList[i].T_MONDAY);
            t23.pt_tuesday = this.returnValue(dtList[i].T_TUESDAY);
            t23.pt_wednesday = this.returnValue(dtList[i].T_WEDNESDAY);
            t23.pt_thursday = this.returnValue(dtList[i].T_THURSDAY);
            t23.pt_friday = this.returnValue(dtList[i].T_FRIDAY);
            t23.prowid = this.returnValue(dtList[i].ROWID);
            if (dtList[i].T_COPY_PRS) {
              t23List.push(t23);
            }
            else if (dtList[i].T_PAT_MEDICINE_SEQ == null && dtList[i].T_OPD_REQ_ITEM != null) {
              t23List.push(t23);
            }
          }
          if (t23List.length==0) {
            this.messageService.add({ severity: 'error', summary: 'Error!', detail: 'Please Add Medication' });
            return;
          }
          this.endpointService.setDataParam('t30023/save', t23List).subscribe((success: any) => {
            var dt = success;
            this.ngxService.stop();
            if (dt.SLIP != '') {
              this.messageService.add({ severity: 'success', summary: 'Success!', detail: dt.MSG });
              this.userForm.get('txtSlipNo')?.setValue(dt.SLIP);
              this.isSaveEnable = true;
              this.onNextClicked('1');
            } else {
              this.messageService.add({ severity: 'error', summary: 'Error!', detail: dt.MSG });
            }
          }, error => { this.ngxService.stop(); this.messageService.add({ severity: 'error', summary: 'Error!', detail: 'Data Not Saved' }); });
        }, error => { this.loading = false; return []; });       
      } else {
        this.messageService.add({ severity: 'error', summary: 'Error!', detail: 'Please Add Medication' });
      }
    }
    else {
      this.loading = false;
      this.validateAllFormFields(this.userForm);
    }
  }

}
