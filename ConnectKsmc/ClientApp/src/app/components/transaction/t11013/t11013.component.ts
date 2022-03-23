import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { T11013Service } from '../../../services/transaction/t11013.service';
//import { T11013 } from '../../../models/T11013';
import { CommonService } from '../../../services/common.service';
import { Subscription } from 'rxjs';
import { NgxUiLoaderService } from 'ngx-ui-loader';


@Component({
  selector: 't11013',
  templateUrl: 't11013.component.html',
  styles: ['::ng-deep .ui-helper-clearfix::before, ::ng-deep .ui-helper-clearfix::after { display: none !important; }'],
  styleUrls: ['t11013.component.css'],
  providers: [T11013Service]
})
export class T11013Component implements OnInit {
  private subscriptions = new Subscription();
  formCode: string = 'T11013';
  loading: boolean = false;
  userForm: FormGroup;
  userForm1: FormGroup;
  userForm2: FormGroup;
  userFormMAMO: FormGroup;
  userFormMRI: FormGroup;
  selectedData: any[] = [];
  labelsNew: any = [];

  labReq: any[]=[];
  conditions: any[] = [];
  hospitals: any[] = [];
  patientType: any[] = [];
  patientLocationData: any[] = [];
  doctorsData: any[] = [];
  procedureData: any[] = [];
  proceData: any[] = [];
  procData: any[] = [];
  newProcData: any[] = [] = [];
  precautions: any[] = [];
  priorities: any[] = [];
  radiologyType: any[] = [];
  locationSelectedData: any[] = [];
  doctorSelectedData: any[] = [];
  procedureSelectedData: any[] = [];
  procSelectedData: any[] = [];
  selectedPatData: any = [];
  selectedRequestData: any = [];
  radiologyRequestList: any = [];

  display: boolean = false;
  displayMRI: boolean = false;
  displayMAMO: boolean = false;
  displayLocation: boolean = false;
  displayDoctor: boolean = false;
  displayProcedure: boolean = false;
  displayProce: boolean = false;
  displayAdmitted: boolean = false;
  displayRequest: boolean = false;
  ky: string='';  
  tabIndex: number = 0;
  t11013: any;
  rowIndex: number = 0;
  admittedPatients: any[] = [];

  enter: string = 'cancel';
  @ViewChild("orderNo", { static: false }) orderNo!: ElementRef;

  canSave!: boolean;
  canUpdate!: boolean;
  canDelete!: boolean;
  canQuery!: boolean;

  userLang!: string;
  siteCode!: string;
  version!: string;
  constructor(private t11013Service: T11013Service, private router: Router, private formBuilder: FormBuilder, private messageService: MessageService, private commonService: CommonService, private route: ActivatedRoute, private ngxService: NgxUiLoaderService) {
    this.userForm = this.formBuilder.group({
      'T_PAT_NO': new FormControl('', Validators.required),
      'PAT_NAME': new FormControl(),
      'GENDER_DSCRPTN': new FormControl(),
      'T_AGE_YRS': new FormControl(),
      'T_PAT_TYPE': new FormControl(),
      'HOSPITAL': new FormControl(),
      'T_ORDER_DATE': new FormControl(),
      'ORDER_DATE_H': new FormControl(),
      'T_FRM_LOCATION': new FormControl(),
      'WARD_DSCRPTN': new FormControl(),
      'T_ORDER_NO': new FormControl(),
      'T_ORDER_TIME': new FormControl(),
      'T_IV_CONSTRAST': new FormControl(),
      'T_ALERGY_FLAG': new FormControl(),
      'T_ANASTHESIA': new FormControl(),
      'T_LAB_REQ': new FormControl(),
      'T_INF_PREC': new FormControl(),
      'T_REQUEST_DOC': new FormControl('', Validators.required),
      'DOCTOR_NAME_1': new FormControl(),
      'T_URGENT_REP': new FormControl(),
      'T_PREGNANCY_FLAG': new FormControl(),
      'T_LMP_DATE': new FormControl(),
      'status': new FormControl()
    });
    this.userForm1 = this.formBuilder.group({
      'T_TYPE_CODE': new FormControl('', Validators.required),
      'T_PROC_CODE': new FormControl(),
      'PROCEDURE_DSCRPTN': new FormControl(),
      'T_PROC_NOTES': new FormControl(),
      'T_INDICATION': new FormControl(),
      'T_PRIORITY_CODE': new FormControl()
    });
    this.userForm2 = this.formBuilder.group({    });
    this.userFormMRI = this.formBuilder.group({
      'T_I_SUR': new FormControl(),
      'T_I_ART': new FormControl(),
      'T_I_PSUR': new FormControl(),
      'T_I_PRE': new FormControl(),
      'T_I_KIDNY': new FormControl(),
      'T_I_NEURO': new FormControl(),
      'T_I_HRAID': new FormControl(),
      'T_I_CLAUS': new FormControl(),
      'T_I_IVCFIL': new FormControl(),
      'T_I_VASCLIP': new FormControl(),
      'T_I_COMPAT': new FormControl(),
      'T_I_JPF': new FormControl(),
      'T_I_CAR': new FormControl(),
      'T_I_IPCARD': new FormControl(),
      'T_I_INT': new FormControl(),
      'T_I_IMF': new FormControl(),
      'T_I_PRGSTU': new FormControl(),
      'T_I_PENIM': new FormControl(),
      'T_M_PATCH': new FormControl(),
      'T_I_DENT': new FormControl(),
      'T_I_SWANS': new FormControl(),
      'T_I_MRI': new FormControl(),
      'T_MRI_FLAG': new FormControl()
    });
    this.userFormMAMO = this.formBuilder.group({
      'T_M_FEED': new FormControl(),
      'T_M_CON': new FormControl(),
      'T_M_CHL': new FormControl(),
      'T_M_HRT': new FormControl(),
      'T_M_HHL': new FormControl(),
      'T_M_MAM': new FormControl(),
      'T_M_DATE': new FormControl(),
      'T_M_PMAM': new FormControl(),
      'T_M_LES': new FormControl(),
      'T_M_AXI': new FormControl(),
      'T_M_DWM': new FormControl(),
      'T_M_NIP': new FormControl(),
      'T_M_TUM': new FormControl(),
      'T_M_BRE': new FormControl(),
      'T_M_BCAU': new FormControl(),
      'T_M_HCAN': new FormControl(),
      'T_M_FAMB': new FormControl(),
      'T_M_FMSD': new FormControl(),
      'T_M_CLI': new FormControl(),
      'T_M_NCHD': new FormControl(),
      'T_M_LWH': new FormControl(),
      'T_I_KIDNE': new FormControl(),
      'T_I_MEDPAT': new FormControl(),
      'T_M_AGEDIA': new FormControl(),
      'T_M_HCHEM': new FormControl(),
      'T_M_HRAD': new FormControl(),
      'T_M_MSLR': new FormControl(),
      'T_M_AXIR': new FormControl(),
      'T_M_AXIL': new FormControl(),
      'T_M_NIPPR': new FormControl(),
      'T_M_NIPPL': new FormControl(),
      'T_M_SKNR': new FormControl(),
      'T_M_SKNL': new FormControl(),
      'T_M_NIPDSR': new FormControl(),
      'T_M_NIPDSL': new FormControl(),
      'T_M_PAINR': new FormControl(),
      'T_M_PAINL': new FormControl(),
      'T_M_FMHIS': new FormControl(),
      'T_M_MSLL': new FormControl(),
      'T_M_TUMSPY': new FormControl(),
      'T_M_OTHERS': new FormControl(),
      'T_M_COMM': new FormControl(),
      'T_MAM_FLAG': new FormControl()
    });
  }
  ngOnInit(): void {
    this.ngxService.start();
    this.userLang = localStorage.getItem('userLang') as string;
    this.siteCode = localStorage.getItem('siteCode') as string;
    this.labReq = [{ CODE: '1', NAME: 'Available' }, { CODE: '2', NAME: 'Requesting' }];
    this.conditions = [{ CODE: '1', NAME: 'Yes' }, { CODE: '2', NAME: 'No' }];
    this.hospitals = [{ CODE: '1', NAME: 'Central' }, { CODE: '2', NAME: 'Pedia' }, { CODE: '3', NAME: 'Maternity' }];    
    this.onPatientTypeLoad();
    this.onPrecautionsLoad();
    this.onRadionlogyTypeLoad();
    this.onPrioritiesLoad();
    var patNo = this.route.snapshot.paramMap.get('patNo');
    if (patNo != null) {
      this.userForm.get('T_PAT_NO')!.setValue(patNo);
      this.onPatNoBlur();
    }
    this.setOrderDate();
    this.proceData = [{ CODE: '', NAME: '' }, { CODE: '', NAME: '' }, { CODE: '', NAME: '' }];
    this.loading = false;
    this.userForm1.disable();
    this.userForm.get('T_ORDER_NO')!.disable();
    this.ngxService.start();
  }
  ngOnDestroy() {
    this.subscriptions.unsubscribe();
  }
  setOrderDate() {
    var todayG = new Date().toLocaleDateString("en-GB", { year: 'numeric', month: '2-digit', day: '2-digit' });
    var todayH = this.commonService.convertToHijriDate(new Date(),'ddMMyyyy');
    var timeG = new Date().toLocaleTimeString("en-GB", { hour: '2-digit', minute: '2-digit' });

    this.userForm.get('T_ORDER_DATE')!.setValue(todayG);
    this.userForm.get('ORDER_DATE_H')!.setValue(todayH);
    this.userForm.get('T_ORDER_TIME')!.setValue(timeG);
  }
  setVersion(val: string) { this.version = val; }
  onLabelLoad(e: string) {
    try {
      return this.labelsNew.filter((a: { T_LABEL_NAME: string; }) => a.T_LABEL_NAME == e)[0].T_LABEL_TEXT;
    } catch (e) {
      return '';
    }
  }
  setPermission(permissions: any) {
    if (permissions.canOpen) {
      this.canSave = permissions.canSave;
      this.canUpdate = permissions.canUpdate;
      this.canDelete = permissions.canDelete;
      this.canQuery = permissions.canQuery;
    }
    else {
      this.messageService.add({ severity: 'error', summary: 'No permission!', detail: 'You don\'t have permission to open the screen' });
    }
  }
  private makeEmpty() {
    this.userForm.reset();
    this.userForm1.reset();
    this.userFormMAMO.reset();
    this.userFormMRI.reset();
    this.setOrderDate();
  }
  onBtnNewClick() {
    this.makeEmpty();
  }
  onClearClicked() {
    this.makeEmpty();
  }
  onBtnEnterClick() {
    this.enter = this.enter == 'enter' ? 'cancel' : 'enter';
    this.change(this.enter);
  }
  onBtnExecuteClick() {
    this.enter = 'exec';
    this.change(this.enter);
  }
  change(e: string) {
    if (e == 'enter') {
      this.userFormMAMO.reset();
      this.userFormMRI.reset();
      this.proceData = [{ CODE: '', NAME: '' }, { CODE: '', NAME: '' }, { CODE: '', NAME: '' }];
      this.userForm.get('T_ORDER_NO')!.enable();
      this.orderNo.nativeElement.focus();
    }
    if (e == 'cancel') {
      this.userForm.get('T_ORDER_NO')!.disable();
    }
    if (e == 'exec') {
      this.loadRequestDetails();
      this.userForm.get('T_ORDER_NO')!.disable();
    }
  }  

  onMAMOOkClick() {
    this.displayMAMO = false;
  }
  onMRIOkClick() {
    this.displayMRI = false;
  }
  onMRIShow() {
    var type = this.userForm1.get('T_TYPE_CODE')!.value == null ? "" : this.userForm1.get('T_TYPE_CODE')!.value == undefined ? "" : this.userForm1.get('T_TYPE_CODE')!.value.T_MAIN_PROC_CODE;
    if (type == "") {
      this.messageService.add({ severity: 'error', summary: 'Required!', detail: 'Type code is required.' });
      return;
    }
    else {
      if (type == "0008") { this.displayMAMO = true; }
      else if (type == "0002") { this.displayMRI = true; }
      else { return; }
    }
  }
  onEnableDisableForm1() {
    var elargy = this.userForm.get('T_ALERGY_FLAG')!.value == null ? "" : this.userForm.get('T_ALERGY_FLAG')!.value;
    var renal = this.userForm.get('T_LAB_REQ')!.value == null ? "" : this.userForm.get('T_LAB_REQ')!.value;
    var reqDoc = this.userForm.get('T_REQUEST_DOC')!.value == null ? "" : this.userForm.get('T_REQUEST_DOC')!.value;
    if (elargy != "" && renal != "" && reqDoc != "") {
      this.userForm1.enable();
    }
    else {
      this.userForm1.disable();
    }
  }
  onBtnNextClick() {
    this.displayMRI = false;
  }
  onPatientDblClick() {
    if (this.admittedPatients != null) {
      this.displayAdmitted = true;
      return;
    }
    this.loading = true;
    this.t11013Service.getAllPatient().subscribe((success: any) => {
      if (success.length > 0) {
        this.admittedPatients = success;
        this.displayAdmitted = true;
      }
      this.loading = false;
    },
        (      error: any) => { this.loading = false; console.log(error); });
  }
  onPatNoBlur() {
    var patNo = this.userForm.get('T_PAT_NO')!.value;
    if (patNo != null && patNo != "") {
      this.loading = true;
      this.t11013Service.getPatientInfo(patNo).subscribe((success: any) => {
        if (success == null) {
          this.loading = false;
          return;
        }
        this.userForm.get('PAT_NAME')!.setValue(success.PATIENT_NAME);
        this.userForm.get('GENDER_DSCRPTN')!.setValue(success.GENDER);
        this.userForm.get('T_AGE_YRS')!.setValue(success.AGE_YRS);
        this.t11013Service.getPatientDetails(patNo).subscribe((success: any) => {
          if (success != null) {
            this.userForm.get('T_PAT_TYPE')!.setValue(this.patientType.find(x => x.T_EPISODE_TYPE == success.T_PAT_TYPE));
            this.userForm.get('HOSPITAL')!.setValue(this.hospitals.find(x => x.CODE == success.HOSPITAL));
            this.userForm.get('T_FRM_LOCATION')!.setValue(success.T_FRM_LOCATION);
            this.userForm.get('WARD_DSCRPTN')!.setValue(success.WARD_DSC);
          }
          this.loadDocInfo();
          this.loading = false;
        }, (error: any) => { this.loading = false; console.log(error); });
      }, (error: any) => { this.loading = false; console.log(error); });
    }
  }
  onOkPatPopUpClick() {
    this.ondbClick(this.selectedPatData);
  }
  ondbClick(rowData: any) {
    this.displayAdmitted = false;
    this.userForm.get('T_PAT_NO')!.setValue(rowData.T_PAT_NO);
    this.onPatNoBlur();
    this.selectedPatData = [];
  }
  onRequestOkClick() {
    this.onRequestDblClick(this.selectedRequestData);
  }
  onRequestDblClick(rowData: any) {
    this.displayRequest = false;
    this.userForm.get('T_ORDER_NO')!.setValue(rowData.T_ORDER_NO);
    this.userForm.get('T_ORDER_DATE')!.setValue(rowData.T_ORDER_DATE);
    var newOrderDate = this.userForm.get('T_ORDER_DATE')!.value;
    var dateFormat = typeof newOrderDate;
    if (dateFormat == "string") {
      newOrderDate = String(newOrderDate).split('/')[1] + '/' + String(newOrderDate).split('/')[0] + '/' + String(newOrderDate).split('/')[2];
    }
    //var todayH = new Date(newOrderDate).toLocaleDateString("ar-SA", { year: 'numeric', month: '2-digit', day: '2-digit' });
    var todayH = this.commonService.convertToHijriDate(new Date(newOrderDate), 'ddMMyyyy');

    this.userForm.get('ORDER_DATE_H')!.setValue(todayH);
    this.userForm.get('T_ORDER_TIME')!.setValue(rowData.T_ORDER_TIME);
    this.selectedRequestData = [];
  }
  onOrderRequestDblClick() {
    if (this.userForm.get('T_PAT_NO')!.value != null) {
      this.loading = true;
      this.t11013Service.getRadiologyRequestList(this.userForm.get('T_PAT_NO')!.value).subscribe((success: any) => {
        if (success.length > 0) {
          this.radiologyRequestList = success;
          this.displayRequest = true;
        }
        this.loading = false;
      },
          (        error: any) => { this.loading = false; console.log(error); });
    }
  }
  loadRequestDetails() {
    if (this.userForm.get('T_ORDER_NO')!.value!=null) {
      this.loading = true;
      this.t11013Service.getRadiologyRequestDetails(this.userForm.get('T_ORDER_NO')!.value).subscribe((success: any) => {
        debugger;
        if (success != null) {
          this.userForm.get('T_PAT_NO')!.setValue(success.T_PAT_NO);
          this.userForm.get('PAT_NAME')!.setValue(success.PAT_NAME);
          this.userForm.get('T_AGE_YRS')!.setValue(success.AGE_YRS);
          if (success.T_X_HOSP != null) {
            this.userForm.get('GENDER_DSCRPTN')!.setValue(success.GENDER);
            this.userForm.get('HOSPITAL')!.setValue(this.hospitals.find(x => x.CODE == success.T_X_HOSP));
          }
          if (success.T_IPOP_FLAG != null) {
            this.userForm.get('T_PAT_TYPE')!.setValue(this.patientType.find(x => x.T_EPISODE_TYPE == success.T_IPOP_FLAG));
          }
          if (success.T_IPOP_FLAG != null) {
            this.userForm.get('T_IV_CONSTRAST')!.setValue(this.conditions.find(x => x.CODE == success.T_IV_CONSTRAST));
          }
          if (success.T_ANASTHESIA != null) {
            this.userForm.get('T_ANASTHESIA')!.setValue(this.conditions.find(x => x.CODE == success.T_ANASTHESIA));
          }
          if (success.T_ALERGY_FLAG != null) {
            this.userForm.get('T_ALERGY_FLAG')!.setValue(this.conditions.find(x => x.CODE == success.T_ALERGY_FLAG));
          }
          if (success.T_LAB_REQ != null) {
            this.userForm.get('T_LAB_REQ')!.setValue(this.labReq.find(x => x.CODE == success.T_LAB_REQ));
          }
          if (success.T_INF_PREC != null) {
            this.userForm.get('T_INF_PREC')!.setValue(this.precautions.find(x => x.T_INF_PREC == success.T_INF_PREC));
          }
          if (success.T_PREGNANCY_FLAG != null) {
            this.userForm.get('T_PREGNANCY_FLAG')!.setValue(this.conditions.find(x => x.CODE == success.T_PREGNANCY_FLAG));
          }
          if (success.T_URGENT_REP != null) {
            this.userForm.get('T_URGENT_REP')!.setValue(success.T_PREGNANCY_FLAG == "1" ? true : false);
          }
          this.userForm.get('T_LMP_DATE')!.setValue(success.T_LMP_DATE);
          if (success.T_REQUEST_DOC != null) {
            this.userForm.get('T_REQUEST_DOC')!.setValue(success.T_REQUEST_DOC);
            this.userForm.get('DOCTOR_NAME_1')!.setValue(success.DOC_NAME);
          }
          if (success.T_CLINIC_CODE != null) {
           this.userForm.get('T_FRM_LOCATION')!.setValue(success.T_CLINIC_CODE);
            this.userForm.get('WARD_DSCRPTN')!.setValue(success.LOCATION_NAME);
          }
          if (success.T11012 != null) {
            this.userForm1.get('T_PROC_CODE')!.setValue(success.T11012.T_PROC_CODE);
            this.userForm1.get('PROCEDURE_DSCRPTN')!.setValue(success.T11012.PROCEDURE_DSCRPTN);
            if (success.T11012.T_TYPE_CODE != null) {
              this.userForm1.get('T_TYPE_CODE')!.setValue(this.radiologyType.find(x => x.T_MAIN_PROC_CODE == success.T11012.T_TYPE_CODE));
            }
            if (success.T11012.T_PRIORITY_CODE != null) {
              this.userForm1.get('T_PRIORITY_CODE')!.setValue(this.priorities.find(x => x.T_URGENCY_CODE == success.T11012.T_PRIORITY_CODE));
            }
            this.userForm1.get('T_PROC_NOTES')!.setValue(success.T11012.T_PROC_NOTES);
            this.userForm1.get('T_INDICATION')!.setValue(success.T11012.T_INDICATION);
            this.userForm1.enable();
          }
          if (success.T11012.T_TYPE_CODE != "") {
            if (success.T11019 != null) {
              //MAMO
              if (success.T11012.T_TYPE_CODE == "0008") {
                this.userFormMAMO.get('T_M_FEED')!.setValue(success.T11019.T_M_FEED == "1" ? true : false);
                this.userFormMAMO.get('T_M_CON')!.setValue(success.T11019.T_M_CON == "1" ? true : false);
                this.userFormMAMO.get('T_M_HRT')!.setValue(success.T11019.T_M_HRT == "1" ? true : false);
                this.userFormMAMO.get('T_M_HCAN')!.setValue(success.T11019.T_M_HCAN == "1" ? true : false);
                this.userFormMAMO.get('T_M_TUM')!.setValue(success.T11019.T_M_TUM == "1" ? true : false);
                this.userFormMAMO.get('T_M_HCHEM')!.setValue(success.T11019.T_M_HCHEM == "1" ? true : false);
                this.userFormMAMO.get('T_M_HRAD')!.setValue(success.T11019.T_M_HRAD == "1" ? true : false);
                this.userFormMAMO.get('T_M_FAMB')!.setValue(success.T11019.T_M_FAMB == "1" ? true : false);
                this.userFormMAMO.get('T_M_MAM')!.setValue(success.T11019.T_M_MAM == "1" ? true : false);
                this.userFormMAMO.get('T_M_MAM')!.setValue(success.T11019.T_M_MAM == "1" ? true : false);
                this.userFormMAMO.get('T_M_AGEDIA')!.setValue(success.T11019.T_M_AGEDIA);
                this.userFormMAMO.get('T_M_TUMSPY')!.setValue(success.T11019.T_M_TUMSPY);
                this.userFormMAMO.get('T_M_FMHIS')!.setValue(success.T11019.T_M_TUMSPY);
                this.userFormMAMO.get('T_M_DATE')!.setValue(success.T11019.T_M_DATE);
                this.userFormMAMO.get('T_M_COMM')!.setValue(success.T11019.T_M_COMM);
                this.userFormMAMO.get('T_M_MSLR')!.setValue(success.T11019.T_M_MSLR == "1" ? true : false);
                this.userFormMAMO.get('T_M_AXIR')!.setValue(success.T11019.T_M_AXIR == "1" ? true : false);
                this.userFormMAMO.get('T_M_NIPPR')!.setValue(success.T11019.T_M_NIPPR == "1" ? true : false);
                this.userFormMAMO.get('T_M_SKNR')!.setValue(success.T11019.T_M_SKNR == "1" ? true : false);
                this.userFormMAMO.get('T_M_NIPDSR')!.setValue(success.T11019.T_M_NIPDSR == "1" ? true : false);
                this.userFormMAMO.get('T_M_PAINR')!.setValue(success.T11019.T_M_PAINR == "1" ? true : false);
                this.userFormMAMO.get('T_MAM_FLAG')!.setValue(success.T11019.T_MAM_FLAG == "1" ? true : false);
                this.userFormMAMO.get('T_M_OTHERS')!.setValue(success.T11019.T_M_OTHERS);
              }
              //MRI
              else if (success.T11012.T_TYPE_CODE == "0002") {
                this.userFormMRI.get('T_I_SUR')!.setValue(success.T11019.T_I_SUR == "1" ? true : false);
                this.userFormMRI.get('T_I_ART')!.setValue(success.T11019.T_I_ART == "1" ? true : false);
                this.userFormMRI.get('T_I_PSUR')!.setValue(success.T11019.T_I_PSUR == "1" ? true : false);
                this.userFormMRI.get('T_I_PRE')!.setValue(success.T11019.T_I_PRE == "1" ? true : false);
                this.userFormMRI.get('T_I_KIDNY')!.setValue(success.T11019.T_I_KIDNY == "1" ? true : false);
                this.userFormMRI.get('T_I_NEURO')!.setValue(success.T11019.T_I_NEURO == "1" ? true : false);
                this.userFormMRI.get('T_I_HRAID')!.setValue(success.T11019.T_I_HRAID == "1" ? true : false);
                this.userFormMRI.get('T_I_CLAUS')!.setValue(success.T11019.T_I_CLAUS == "1" ? true : false);
                this.userFormMRI.get('T_I_IVCFIL')!.setValue(success.T11019.T_I_IVCFIL == "1" ? true : false);
                this.userFormMRI.get('T_I_VASCLIP')!.setValue(success.T11019.T_I_VASCLIP == "1" ? true : false);
                this.userFormMRI.get('T_I_COMPAT')!.setValue(success.T11019.T_I_COMPAT);
                this.userFormMRI.get('T_I_JPF')!.setValue(success.T11019.T_I_JPF == "1" ? true : false);
                this.userFormMRI.get('T_I_CAR')!.setValue(success.T11019.T_I_CAR == "1" ? true : false);
                this.userFormMRI.get('T_I_IPCARD')!.setValue(success.T11019.T_I_IPCARD == "1" ? true : false);
                this.userFormMRI.get('T_I_INT')!.setValue(success.T11019.T_I_INT == "1" ? true : false);
                this.userFormMRI.get('T_I_IMF')!.setValue(success.T11019.T_I_IMF == "1" ? true : false);
                this.userFormMRI.get('T_I_PRGSTU')!.setValue(success.T11019.T_I_PRGSTU == "1" ? true : false);
                this.userFormMRI.get('T_I_PENIM')!.setValue(success.T11019.T_I_PENIM == "1" ? true : false);
                this.userFormMRI.get('T_M_PATCH')!.setValue(success.T11019.T_M_PATCH == "1" ? true : false);
                this.userFormMRI.get('T_I_DENT')!.setValue(success.T11019.T_I_DENT == "1" ? true : false);
                this.userFormMRI.get('T_I_SWANS')!.setValue(success.T11019.T_I_SWANS == "1" ? true : false);
                this.userFormMRI.get('T_MRI_FLAG')!.setValue(success.T11019.T_MRI_FLAG == "1" ? true : false);
              }
            }
          }
          if (success.T11013.length > 0) {
            this.proceData = success.T11013;
          }
        }
        this.loading = false;
      },
          (        error: any) => { this.loading = false; console.log(error); });
    }
  }
  loadDocInfo() {
    this.t11013Service.getDoctorInfo().subscribe((success: any) => {
      if (success != null) {
        this.userForm.get('T_REQUEST_DOC')!.setValue(success.T_DOC_CODE);
        this.userForm.get('DOCTOR_NAME_1')!.setValue(success.DOC_NAME);
      }
    }, (error: any) => { this.loading = false; console.log(error); });
  }

  onHospitalChange() {
    var age = this.userForm.get('T_AGE_YRS')!.value == null ? "" : this.userForm.get('T_AGE_YRS')!.value == undefined ? "" : this.userForm.get('T_AGE_YRS')!.value;
    var hospital = this.userForm.get('HOSPITAL')!.value == null ? "" : this.userForm.get('HOSPITAL')!.value == undefined ? "" : this.userForm.get('HOSPITAL')!.value.CODE;
    var gender = this.userForm.get('GENDER_DSCRPTN')!.value == null ? "" : this.userForm.get('GENDER_DSCRPTN')!.value == undefined ? "" : this.userForm.get('GENDER_DSCRPTN')!.value;
    var userLang = localStorage.getItem('userLang') as string;
    if (age == "") {
      this.messageService.add({ severity: 'error', summary: 'Required!', detail: 'Type code is required.' });
      return;
    }
    else {
    }
    if (age > 13 && hospital == "2") {
      //USER_MESSAGE('S0005');
      this.messageService.add({ severity: 'error', summary: 'Required!', detail: 'Type code is required.' });
      return;
    }
    else if (age > 8 && hospital == "3" && gender.toUpperCase() == "FEMALE") {
      //user_message('N0010');
      this.messageService.add({ severity: 'error', summary: 'Required!', detail: 'Type code is required.' });
      return;
    }
  }
  onPatTypeChange() {
    this.userForm.get('T_FRM_LOCATION')!.setValue('');
    this.userForm.get('WARD_DSCRPTN')!.setValue('');
    if (this.userForm.get('T_PAT_TYPE')!.value == null) {
      return;
    }
    this.loading = true;
    this.t11013Service.getLocationByPatType(this.userForm.get('T_PAT_TYPE')!.value.T_EPISODE_TYPE, this.userForm.get('T_PAT_NO')!.value).subscribe((success: any) => {
      this.patientLocationData = success;
      this.loading = false;
    }, (error: any) => { this.loading = false; console.log(error); return []; });
  }
  onLocationRowDblClick(rowData: any) {
    this.userForm.get('T_FRM_LOCATION')!.setValue(rowData.LOC_CODE);
    this.userForm.get('WARD_DSCRPTN')!.setValue(rowData.LOC_NAME);
    this.displayLocation = false;
  }
  onLocationOkClick() {
    var rowData = this.locationSelectedData;
    if (rowData != null) { this.onLocationRowDblClick(rowData); }
  }
  onLocationDblClick() {
    var patNo = this.userForm.get('T_PAT_NO')!.value == null ? "" : this.userForm.get('T_PAT_NO')!.value == undefined ? "" : this.userForm.get('T_PAT_NO')!.value;
    if (patNo == "") {
      this.messageService.add({ severity: 'error', summary: 'Required!', detail: 'Patient no is required.' });
      return;
    }
    var patType = this.userForm.get('T_PAT_TYPE')!.value == null ? "" : this.userForm.get('T_PAT_TYPE')!.value == undefined ? "" : this.userForm.get('T_PAT_TYPE')!.value.T_EPISODE_TYPE;
    if (patType == "") {
      this.messageService.add({ severity: 'error', summary: 'Required!', detail: 'Patient type is required.' });
      return;
    }
    else {
      this.loading = true;
      this.t11013Service.getLocationByPatType(this.userForm.get('T_PAT_TYPE')!.value.T_EPISODE_TYPE, this.userForm.get('T_PAT_NO')!.value).subscribe((success: any) => {
        this.patientLocationData = success;
        if (success.length > 0) {
          this.displayLocation = true;
        }
        else {
          this.messageService.add({ severity: 'info', summary: 'Information!', detail: 'No location found.' });
        }
        this.loading = false;
      }, (error: any) => { this.loading = false; console.log(error); return []; });
    }
    //shw_lov('LOCATION');
  }
  onDoctorRowDblClick(rowData: any) {
    this.userForm.get('T_REQUEST_DOC')!.setValue(rowData.T_DOC_CODE);
    this.userForm.get('DOCTOR_NAME_1')!.setValue(rowData.DOC_NAME);
    this.displayDoctor = false;
    this.onEnableDisableForm1();
  }
  onReqDocBlur() {
    var dCode = this.userForm.get('T_REQUEST_DOC')!.value;
    if (dCode != null) {
      if (this.doctorsData.length > 0) {
        var rowData = this.doctorsData.find(x => x.T_DOC_CODE == dCode.toUpperCase());
        if (rowData != null) {
          this.userForm.get('DOCTOR_NAME_1')!.setValue(rowData.DOC_NAME);
        }
        else {
          this.userForm.get('DOCTOR_NAME_1')!.setValue('');
        }
      }
    }
  }
  onProcBlur() {
    var pCode = this.userForm1.get('T_PROC_CODE')!.value;
    if (pCode != null) {
      if (this.procedureData.length > 0) {
        var rowData = this.procedureData.find(x => x.T_PROC_CODE == pCode);
        if (rowData != null) {
          this.userForm1.get('PROCEDURE_DSCRPTN')!.setValue(rowData.PROC_DSCRPTN);
        } else {
          this.userForm1.get('PROCEDURE_DSCRPTN')!.setValue('');
        }
      }
    }
  }
  onLocationBlur() {
    var patType = this.userForm.get('T_PAT_TYPE')!.value == null ? "" : this.userForm.get('T_PAT_TYPE')!.value == undefined ? "" : this.userForm.get('T_PAT_TYPE')!.value.T_EPISODE_TYPE;
    if (patType == "") {
      this.messageService.add({ severity: 'error', summary: 'Required!', detail: 'Patient type is required.' });
      return;
    }
    var lCode = this.userForm.get('T_FRM_LOCATION')!.value;
    if (lCode != null) {
      if (this.patientLocationData.length > 0) {
        var rowData = this.patientLocationData.find(x => x.LOC_CODE == lCode);
        if (rowData != null) {
          this.userForm.get('WARD_DSCRPTN')!.setValue(rowData.LOC_NAME);
        }
        else {
          this.userForm.get('WARD_DSCRPTN')!.setValue('');
        }
      }
    }
  }
  onDoctorOkClick() {
    var rowData = this.doctorSelectedData;
    if (rowData != null) {
      this.onDoctorRowDblClick(rowData);
    }
  }
  onReqDocDblClick() {
    if (this.doctorsData.length > 0) {
      this.displayDoctor = true;
    }
    else {
      this.messageService.add({ severity: 'info', summary: 'Information!', detail: 'No Doctor found.' });
    }
  }
  onPatientLoad() {
    this.t11013Service.getAllPatient().subscribe((success: any) => {
      this.admittedPatients = success;
    }, (error: any) => { console.log(error); });
  }  
  onDoctorLoad() {
    this.t11013Service.getAllDoctor().subscribe((success: any) => {
      this.doctorsData = success;
    }, error => { console.log(error); });
  }  
  onPatientTypeLoad() {
    this.t11013Service.getAllPatientType().subscribe((success: any) => {
      this.patientType = success;
    }, error => { this.loading = false; console.log(error); return []; });
  }
  onPrecautionsLoad() {
    this.t11013Service.getAllInfectionPrecaution().subscribe((success: any) => {
      this.precautions = success;
    }, error => { this.loading = false; console.log(error); return []; });
  }
  onRadionlogyTypeLoad() {
    this.t11013Service.getAllType().subscribe((success: any) => {
      this.radiologyType = success;
    }, error => { this.loading = false; console.log(error); return []; });
  }
  onPrioritiesLoad() {
    this.t11013Service.getAllPriority().subscribe((success: any) => {
      this.priorities = success;
    }, error => { this.loading = false; console.log(error); return []; });
  }
  onRadioTypeChange() {
    this.userForm1.get('T_PROC_CODE')!.setValue('');
    this.userForm1.get('PROCEDURE_DSCRPTN')!.setValue('');
    this.userFormMAMO.reset();
    this.userFormMRI.reset();
    var typ = this.userForm1.get('T_TYPE_CODE')!.value == null ? "" : this.userForm1.get('T_TYPE_CODE')!.value.TYPE_NAME;
    if (typ != "") {
      this.userForm1.get('T_PRIORITY_CODE')!.setValue(this.priorities.find(x => x.PRIORITY_CODE == "Routine"));
      this.loading = true;
      this.t11013Service.getAllProcedure(this.userForm1.get('T_TYPE_CODE')!.value.T_MAIN_PROC_CODE).subscribe((success: any) => {
        this.procedureData = success;
        this.loading = false;
      }, error => { this.loading = false; console.log(error); return []; });
    }    
  }
  onProcedureRowDblClick(indx: any) {
    var type = this.userForm1.get('T_TYPE_CODE')!.value == null ? "" : this.userForm1.get('T_TYPE_CODE')!.value == undefined ? "" : this.userForm1.get('T_TYPE_CODE')!.value.T_MAIN_PROC_CODE;
    if (type == "") {
      this.messageService.add({ severity: 'error', summary: 'Required!', detail: 'Type code is required.' });
      return;
    }
    else {
      this.loading = true;
      this.t11013Service.getAllProcedure(this.userForm1.get('T_TYPE_CODE')!.value.T_MAIN_PROC_CODE).subscribe((success: any) => {
        this.procData = success;
        this.rowIndex = indx;
        if (success.length > 0) {
          this.displayProce = true;
        }
        else {
          this.messageService.add({ severity: 'info', summary: 'Information!', detail: 'No Procedure found.' });
        }
        this.loading = false;
      }, error => { this.loading = false; console.log(error); return []; });
    }
    //shw_lov('LOCATION');
  }
  onProcedure1RowDblClick(rowData: any) {
    this.proceData[this.rowIndex].CODE = rowData.T_PROC_CODE;
    this.proceData[this.rowIndex].NAME = rowData.PROC_DSCRPTN;
    this.displayProce = false;
  }
  onProcedureOkClick() {
    var rowData = this.procSelectedData;
    if (rowData != null) { this.onProcedure1RowDblClick(rowData); }
  }
  onProcDblClick() {
    var type = this.userForm1.get('T_TYPE_CODE')!.value == null ? "" : this.userForm1.get('T_TYPE_CODE')!.value == undefined ? "" : this.userForm1.get('T_TYPE_CODE')!.value.T_MAIN_PROC_CODE;
    if (type == "") {
      this.messageService.add({ severity: 'error', summary: 'Required!', detail: 'Type code is required.' });
      return;
    }
    else {
      this.loading = true;
      this.t11013Service.getAllProcedure(this.userForm1.get('T_TYPE_CODE')!.value.T_MAIN_PROC_CODE).subscribe((success: any) => {
        this.procedureData = success;
        if (success.length > 0) {
          this.displayProcedure = true;
        }
        else {
          this.messageService.add({ severity: 'info', summary: 'Information!', detail: 'No Procedure found.' });
        }
        this.loading = false;
      }, error => { this.loading = false; console.log(error); return []; });
    }
    //shw_lov('LOCATION');
  }
  onProcRowDblClick(rowData: any) {
    this.userForm1.get('T_PROC_CODE')!.setValue(rowData.T_PROC_CODE);
    this.userForm1.get('PROCEDURE_DSCRPTN')!.setValue(rowData.PROC_DSCRPTN);
    this.displayProcedure = false;
  }
  onProcOkClick() {
    var rowData = this.procedureSelectedData;
    if (rowData != null) { this.onProcRowDblClick(rowData); }
  }
  onOrderDateChange() {
    //var todayH = new Date(this.userForm.get('T_ORDER_DATE')!.value).toLocaleDateString("ar-SA", { year: 'numeric', month: '2-digit', day: '2-digit' });
    var todayH = this.commonService.convertToHijriDate(new Date(this.userForm.get('T_ORDER_DATE')!.value), 'ddMMyyyy');
    this.userForm.get('ORDER_DATE_H')!.setValue(todayH);
  }
  onSaveClicked() {
    if (this.userForm.valid) {
      var typ = this.userForm1.get('T_TYPE_CODE')!.value == null ? "" : this.userForm1.get('T_TYPE_CODE')!.value == undefined ? "" : this.userForm1.get('T_TYPE_CODE')!.value == "" ? "" : this.userForm1.get('T_TYPE_CODE')!.value.TYPE_NAME;
      if (typ == "") {
        this.messageService.add({ severity: 'error', summary: 'Type Code', detail: 'This field required' });
        this.userForm1.get('T_TYPE_CODE')!.markAsDirty();
        return;
      }   
      //this.t11013 = this.userForm.value;
      this.t11013 = new Object();
      //this.t11013 = new T11013();
      this.t11013.T_FRM_LOCATION = this.formCode;
      //this.t11013.T_X_HOSP = "1";
      this.t11013.T_PAT_NO = this.userForm.value.T_PAT_NO == null ? "" : this.userForm.value.T_PAT_NO;
      this.userForm.get('T_ORDER_NO')!.enable();
      this.t11013.T_ORDER_NO = this.userForm.value.T_ORDER_NO == null ? "" : this.userForm.value.T_ORDER_NO;
      this.userForm.get('T_ORDER_NO')!.disable();
      this.t11013.T_ORDER_TIME = this.userForm.value.T_ORDER_TIME == null ? "" : this.userForm.value.T_ORDER_TIME;
      this.t11013.HOSPITAL = this.userForm.value.HOSPITAL == null ? "" : this.userForm.value.HOSPITAL.CODE;
      this.t11013.T_IPOP_FLAG = this.userForm.value.T_PAT_TYPE == null ? "" : this.userForm.value.T_PAT_TYPE.T_EPISODE_TYPE;
      this.t11013.T_IV_CONSTRAST = this.userForm.value.T_IV_CONSTRAST == null ? "" : this.userForm.value.T_IV_CONSTRAST.CODE;
      this.t11013.T_ANASTHESIA = this.userForm.value.T_ANASTHESIA == null ? "" : this.userForm.value.T_ANASTHESIA.CODE;
      this.t11013.T_ALERGY_FLAG = this.userForm.value.T_ALERGY_FLAG == null ? "" : this.userForm.value.T_ALERGY_FLAG.CODE;
      this.t11013.T_LAB_REQ = this.userForm.value.T_LAB_REQ == null ? "" : this.userForm.value.T_LAB_REQ.CODE;
      this.t11013.T_INF_PREC = this.userForm.value.T_INF_PREC == null ? "" : this.userForm.value.T_INF_PREC.CODE;
      this.t11013.T_PREGNANCY_FLAG = this.userForm.value.T_PREGNANCY_FLAG == null ? "" : this.userForm.value.T_PREGNANCY_FLAG.CODE;
      this.t11013.T_URGENT_REP = this.userForm.value.T_URGENT_REP == null ? "" : this.userForm.value.T_URGENT_REP == true ? "1" : "0";
      this.t11013.T_ORDER_DATE = this.userForm.value.T_ORDER_DATE == null ? "" : this.commonService.convertToDateString(this.userForm.value.T_ORDER_DATE);
      this.t11013.T_LMP_DATE = this.userForm.value.T_LMP_DATE == null ? "" : this.commonService.convertToDateString(this.userForm.value.T_LMP_DATE);
      this.t11013.T_CLINICAL_DATA = this.userForm1.value.T_INDICATION == null ? "" : this.userForm1.value.T_INDICATION;
      this.t11013.T_CLINIC_CODE = this.userForm.value.T_FRM_LOCATION == null ? "" : this.userForm.value.T_FRM_LOCATION;
      this.t11013.T_REQUEST_DOC = this.userForm.value.T_REQUEST_DOC == null ? "" : this.userForm.value.T_REQUEST_DOC;
      this.t11013.T11012.T_TYPE_CODE = this.userForm1.value.T_TYPE_CODE == null ? "" : this.userForm1.value.T_TYPE_CODE.T_MAIN_PROC_CODE;
      this.t11013.T11012.T_PROC_CODE = this.userForm1.value.T_PROC_CODE == null ? "" : this.userForm1.value.T_PROC_CODE;
      this.t11013.T11012.T_PROC_NOTES = this.userForm1.value.T_PROC_NOTES == null ? "" : this.userForm1.value.T_PROC_NOTES;
      this.t11013.T11012.T_PRIORITY_CODE = this.userForm1.value.T_PRIORITY_CODE == null ? "" : this.userForm1.value.T_PRIORITY_CODE.T_URGENCY_CODE;
      this.t11013.T11012.T_INDICATION = this.userForm1.value.T_INDICATION == null ? "" : this.userForm1.value.T_INDICATION;
      //if (this.userFormT1.get('T_ID_TYPE')!.value != null) {
      //  this.t03001.T_ID_TYPE = this.userFormT1.get('T_ID_TYPE')!.value.CODE;
      //}
      //this.t11013.T11019 = null;
      if (this.proceData.length > 0) {
        for (var i = 0; i < this.proceData.length; i++) {
          if (this.proceData[i].CODE != null && this.proceData[i].CODE != "") {
            this.newProcData.push(this.proceData[i]);
          }
        }
        this.t11013.T11013 = this.newProcData.length > 0 ? this.newProcData : null;
      }
      if (this.t11013.T11012.T_TYPE_CODE != "") {
        if (this.t11013.T11012.T_TYPE_CODE == "0002" || this.t11013.T11012.T_TYPE_CODE == "0008") {
          //MAMO
          this.t11013.T11019.T_M_FEED = this.userFormMAMO.value.T_M_FEED == null ? "" : this.userFormMAMO.value.T_M_FEED == true ? "1" : "0";
          this.t11013.T11019.T_M_CON = this.userFormMAMO.value.T_M_CON == null ? "" : this.userFormMAMO.value.T_M_CON == true ? "1" : "0";
          this.t11013.T11019.T_M_HRT = this.userFormMAMO.value.T_M_HRT == null ? "" : this.userFormMAMO.value.T_M_HRT == true ? "1" : "0";
          this.t11013.T11019.T_M_HCAN = this.userFormMAMO.value.T_M_HCAN == null ? "" : this.userFormMAMO.value.T_M_HCAN == true ? "1" : "0";
          this.t11013.T11019.T_M_TUM = this.userFormMAMO.value.T_M_TUM == null ? "" : this.userFormMAMO.value.T_M_TUM == true ? "1" : "0";
          this.t11013.T11019.T_M_HCHEM = this.userFormMAMO.value.T_M_HCHEM == null ? "" : this.userFormMAMO.value.T_M_HCHEM == true ? "1" : "0";
          this.t11013.T11019.T_M_HRAD = this.userFormMAMO.value.T_M_HRAD == null ? "" : this.userFormMAMO.value.T_M_HRAD == true ? "1" : "0";
          this.t11013.T11019.T_M_FAMB = this.userFormMAMO.value.T_M_FAMB == null ? "" : this.userFormMAMO.value.T_M_FAMB == true ? "1" : "0";
          this.t11013.T11019.T_M_MAM = this.userFormMAMO.value.T_M_MAM == null ? "" : this.userFormMAMO.value.T_M_MAM == true ? "1" : "0";
          this.t11013.T11019.T_M_AGEDIA = this.userFormMAMO.value.T_M_AGEDIA == null ? "" : this.userFormMAMO.value.T_M_AGEDIA;
          this.t11013.T11019.T_M_TUMSPY = this.userFormMAMO.value.T_M_TUMSPY == null ? "" : this.userFormMAMO.value.T_M_TUMSPY;
          this.t11013.T11019.T_M_FMHIS = this.userFormMAMO.value.T_M_FMHIS == null ? "" : this.userFormMAMO.value.T_M_FMHIS;
          this.t11013.T11019.T_M_DATE = this.userFormMAMO.value.T_M_DATE == null ? "" : this.commonService.convertToDateString(this.userFormMAMO.value.T_M_DATE);
          this.t11013.T11019.T_M_COMM = this.userFormMAMO.value.T_M_COMM == null ? "" : this.userFormMAMO.value.T_M_COMM;
          this.t11013.T11019.T_M_MSLR = this.userFormMAMO.value.T_M_MSLR == null ? "" : this.userFormMAMO.value.T_M_MSLR == true ? "1" : "0";
          this.t11013.T11019.T_M_AXIR = this.userFormMAMO.value.T_M_AXIR == null ? "" : this.userFormMAMO.value.T_M_AXIR == true ? "1" : "0";
          this.t11013.T11019.T_M_NIPPR = this.userFormMAMO.value.T_M_NIPPR == null ? "" : this.userFormMAMO.value.T_M_NIPPR == true ? "1" : "0";
          this.t11013.T11019.T_M_SKNR = this.userFormMAMO.value.T_M_SKNR == null ? "" : this.userFormMAMO.value.T_M_SKNR == true ? "1" : "0";
          this.t11013.T11019.T_M_NIPDSR = this.userFormMAMO.value.T_M_NIPDSR == null ? "" : this.userFormMAMO.value.T_M_NIPDSR == true ? "1" : "0";
          this.t11013.T11019.T_M_PAINR = this.userFormMAMO.value.T_M_PAINR == null ? "" : this.userFormMAMO.value.T_M_PAINR == true ? "1" : "0";
          this.t11013.T11019.T_M_OTHERS = this.userFormMAMO.value.T_M_OTHERS == null ? "" : this.userFormMAMO.value.T_M_OTHERS;
          this.t11013.T11019.T_MAM_FLAG = this.userFormMAMO.value.T_MAM_FLAG == null ? "" : this.userFormMAMO.value.T_MAM_FLAG == true ? "1" : "0";
          //MRI
          this.t11013.T11019.T_I_SUR = this.userFormMRI.value.T_I_SUR == null ? "" : this.userFormMRI.value.T_I_SUR == true ? "1" : "0";
          this.t11013.T11019.T_I_ART = this.userFormMRI.value.T_I_ART == null ? "" : this.userFormMRI.value.T_I_ART == true ? "1" : "0";
          this.t11013.T11019.T_I_PSUR = this.userFormMRI.value.T_I_PSUR == null ? "" : this.userFormMRI.value.T_I_PSUR == true ? "1" : "0";
          this.t11013.T11019.T_I_PRE = this.userFormMRI.value.T_I_PRE == null ? "" : this.userFormMRI.value.T_I_PRE == true ? "1" : "0";
          this.t11013.T11019.T_I_KIDNY = this.userFormMRI.value.T_I_KIDNY == null ? "" : this.userFormMRI.value.T_I_KIDNY == true ? "1" : "0";
          this.t11013.T11019.T_I_NEURO = this.userFormMRI.value.T_I_NEURO == null ? "" : this.userFormMRI.value.T_I_NEURO == true ? "1" : "0";
          this.t11013.T11019.T_I_HRAID = this.userFormMRI.value.T_I_HRAID == null ? "" : this.userFormMRI.value.T_I_HRAID == true ? "1" : "0";
          this.t11013.T11019.T_I_CLAUS = this.userFormMRI.value.T_I_CLAUS == null ? "" : this.userFormMRI.value.T_I_CLAUS == true ? "1" : "0";
          this.t11013.T11019.T_I_IVCFIL = this.userFormMRI.value.T_I_IVCFIL == null ? "" : this.userFormMRI.value.T_I_IVCFIL == true ? "1" : "0";
          this.t11013.T11019.T_I_VASCLIP = this.userFormMRI.value.T_I_VASCLIP == null ? "" : this.userFormMRI.value.T_I_VASCLIP == true ? "1" : "0";
          this.t11013.T11019.T_I_COMPAT = this.userFormMRI.value.T_I_COMPAT == null ? "" : this.userFormMRI.value.T_I_COMPAT;
          this.t11013.T11019.T_I_JPF = this.userFormMRI.value.T_I_JPF == null ? "" : this.userFormMRI.value.T_I_JPF == true ? "1" : "0";
          this.t11013.T11019.T_I_CAR = this.userFormMRI.value.T_I_CAR == null ? "" : this.userFormMRI.value.T_I_CAR == true ? "1" : "0";
          this.t11013.T11019.T_I_IPCARD = this.userFormMRI.value.T_I_IPCARD == null ? "" : this.userFormMRI.value.T_I_IPCARD == true ? "1" : "0";
          this.t11013.T11019.T_I_INT = this.userFormMRI.value.T_I_INT == null ? "" : this.userFormMRI.value.T_I_INT == true ? "1" : "0";
          this.t11013.T11019.T_I_IMF = this.userFormMRI.value.T_I_IMF == null ? "" : this.userFormMRI.value.T_I_IMF == true ? "1" : "0";
          this.t11013.T11019.T_I_PRGSTU = this.userFormMRI.value.T_I_PRGSTU == null ? "" : this.userFormMRI.value.T_I_PRGSTU == true ? "1" : "0";
          this.t11013.T11019.T_I_PENIM = this.userFormMRI.value.T_I_PENIM == null ? "" : this.userFormMRI.value.T_I_PENIM == true ? "1" : "0";
          this.t11013.T11019.T_M_PATCH = this.userFormMRI.value.T_M_PATCH == null ? "" : this.userFormMRI.value.T_M_PATCH == true ? "1" : "0";
          this.t11013.T11019.T_I_DENT = this.userFormMRI.value.T_I_DENT == null ? "" : this.userFormMRI.value.T_I_DENT == true ? "1" : "0";
          this.t11013.T11019.T_I_SWANS = this.userFormMRI.value.T_I_SWANS == null ? "" : this.userFormMRI.value.T_I_SWANS == true ? "1" : "0";
          this.t11013.T11019.T_MRI_FLAG = this.userFormMRI.value.T_MRI_FLAG == null ? "" : this.userFormMRI.value.T_MRI_FLAG == true ? "1" : "0";

          this.t11013.T11019.T_M_CHL = "";
          this.t11013.T11019.T_M_HHL = "";
          this.t11013.T11019.T_M_PMAM = "";
          this.t11013.T11019.T_M_LES = "";
          this.t11013.T11019.T_M_AXI = "";
          this.t11013.T11019.T_M_DWM = "";
          this.t11013.T11019.T_M_NIP = "";
          this.t11013.T11019.T_I_MRI = "";
          this.t11013.T11019.T_M_NCHD = "";
          this.t11013.T11019.T_M_LWH = "";
          this.t11013.T11019.T_I_KIDNE = "";
          this.t11013.T11019.T_I_MEDPAT = "";
          this.t11013.T11019.T_M_AXIL = "";
          this.t11013.T11019.T_M_NIPPL = "";
          this.t11013.T11019.T_M_SKNL = "";
          this.t11013.T11019.T_M_NIPDSL = "";
          this.t11013.T11019.T_M_PAINL = "";
          this.t11013.T11019.T_M_MSLL = "";
          this.t11013.T11019.T_M_BRE = "";
          this.t11013.T11019.T_M_BCAU = "";
          this.t11013.T11019.T_M_FMSD = "";
          this.t11013.T11019.T_M_CLI = "";
        }
      }
      this.loading = true;
      this.t11013Service.saveT11013(this.t11013)
        .subscribe((success: any) => {
          if (success) {
            this.loading = false;
            this.userForm.get('T_ORDER_NO')!.setValue(success);
            this.messageService.add({ severity: 'success', summary: 'Success!', detail: 'Data Save successfully' });
          }
          else {
            this.loading = false;
            this.messageService.add({ severity: 'error', summary: 'Error!', detail: 'Data not Save' });
          }
          this.newProcData = [];
        },
          error => {
            this.loading = false;
            if (error.status == 400) {
              this.messageService.add({ severity: 'error', summary: 'Error!', detail: 'Data not Save' });
              console.log(error.error.msg);
            }
            else { console.log(error); }
          });
    }
    else {
      this.validateAllFormFields(this.userForm);
      //this.validateAllFormFields(this.userForm1);
    }
  }

  private validateAllFormFields(formGroup: FormGroup) {
    Object.keys(formGroup.controls).forEach((key: string) => {
      const abstractControl = formGroup.get(key);
      if (abstractControl instanceof FormGroup) {
        this.validateAllFormFields(abstractControl);
      } else {
        if (!formGroup.controls[key].valid) {
          formGroup.controls[key].markAsDirty();
          //this.ky = $("label[for]").filter('[for="' + key + '"]').text();
          //this.messageService.add({ severity: 'error', summary: this.ky, detail: 'This field required' });
        }
      }
    });
  }
  onPrintClicked() {
    var type = this.userForm1.get('T_TYPE_CODE')!.value == null ? "" : this.userForm1.get('T_TYPE_CODE')!.value == undefined ? "" : this.userForm1.get('T_TYPE_CODE')!.value.T_MAIN_PROC_CODE;
    if (type == "") {
      this.messageService.add({ severity: 'error', summary: 'Required!', detail: 'Type code is required.' });
      return;
    }
    var orderNo = this.userForm.get('T_ORDER_NO')!.value == null ? "" : this.userForm.get('T_ORDER_NO')!.value == undefined ? "" : this.userForm.get('T_ORDER_NO')!.value;
    if (orderNo == "") {
      this.messageService.add({ severity: 'error', summary: 'Required!', detail: 'Order no is required.' });
      return;
    }
    else {
      if (type == "0008") {
        window.open("./api/t11013/getRadiologyReport?orderNo=" + orderNo + "&radiologyType=" + "mamo", "popup", "location=1, status=1, scrollbars=1");
      }
      else if (type == "0002") {
        window.open("./api/t11013/getRadiologyReport?orderNo=" + orderNo + "&radiologyType=" + "MRI", "popup", "location=1, status=1, scrollbars=1");
      }
      else {
        window.open("./api/t11013/getRadiologyReport?orderNo=" + orderNo + "&radiologyType=" + "", "popup", "location=1, status=1, scrollbars=1");
      }
    }
  }
  getEventValue($event: any): string {
    return $event.target.value;
  }
}
