import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { MessageService } from 'primeng/api';
import { CommonService } from '../../../services/common.service';
import { T13115Service } from '../../../services/transaction/t13115.service';
import { Router, ActivatedRoute } from '@angular/router';
import { debug } from 'console';
import { resolveAny } from 'dns';

@Component({
  selector: 't13115',
  templateUrl: 't13115.component.html',
  providers: [T13115Service]
})
export class T13115Component implements OnInit {
  messages: any[] = [];
  canSave = false;
  canUpdate = false;
  canDelete = false;
  canQuery = false;
  userLang = '';
  requestNo: any = '';
  patNo: any;
  statusCode: any = '2';
  docCode: any = '';
  locationCode: any = '';//clinic code 
  episodeNo: any = '1';
  patType: any;


  priorities: any[] = [];
  patientTypes: any[] = [];
  workStation: any[] = [];
  wsSelectedData: any[] = [];
  wsInTables: any[] = [];
  analysis: any[] = [];
  specimen: any[] = [];
  selectedSpecimen: any[] = [];

  labData: any[] = [];
  t13015: any[] = [];
  RequestData: any[] = [];
  selectedRequest: any;
  diagList: any[] = [];
  rowIndex: number = 0;
  showFollowup: boolean = false;
  labelsNew: any[] = [];
  displayWS: boolean = false;
  displayAnalysis: boolean = false;
  analysisSelectedData: any[] = [];
  displaySpecimen: boolean = false;
  specimenSelectedData: any[] = [];

  pretestCode: any;
  groupFlag: any;
  singleFlag: any;
  statReq: any;
  genderDes: any;
  externalFlag: any;

  //cervical pap smear
  labNo: any;
  indication: any;
  lmp: any;
  colpFind: any;
  specExam: any;
  chkPregnancy: any;
  chkChemotherapy: any;
  chkContraceptivePills: any;
  chkICUD: any;
  chkHRT: any;
  chkVaginalDischarge: any;
  chkPostPertum: any;
  chkAbnormalBleeding: any;
  chkPostMenopause: any;

  isUpdate: string = '0';

  ReqListByPat: any[] = [];
  reqListSelectedData: any[] = [];
  displayReqByPat: boolean = false;

  RequiredColor: any = 'WHITE';

  userForm = new FormGroup({
    'txtRequestNo': new FormControl(),
    'txtRequestDate': new FormControl(),
    'txtRequestDateH': new FormControl(),
    'txtRequestTime': new FormControl(),
    'txtPatNo': new FormControl('', Validators.required),
    'txtPatName': new FormControl(),
    'txtYears': new FormControl(),
    'txtMonths': new FormControl(),
    'txtNationalId': new FormControl(),
    'txtNationality': new FormControl(),
    'txtGender': new FormControl(),
    'txtMStatus': new FormControl(),
    'txtLocation': new FormControl(),
    'txtTestToBeDone': new FormControl('', Validators.required),
    'txtTestDay': new FormControl(),
    'ddlWorkStation': new FormControl(),
    'ddlPriority': new FormControl('', Validators.required),
    'ddlPatType': new FormControl(),
    'txtEpisodeNo': new FormControl(),
    'txtClinicData': new FormControl('', Validators.required),
    'txtComments': new FormControl('', Validators.required),
    'txtSearchTestByName': new FormControl(),
    'ddlSpecimen': new FormControl(),
    'txtPapLabNo': new FormControl(),
    'txtIndication': new FormControl(),
    'txtLMP': new FormControl(),
    'txtSpectrumExam': new FormControl(),
    'txtColposcopicFinding': new FormControl(),
    'chkPregnancy': new FormControl(),
    'chkChemotherapy': new FormControl(),
    'chkContraceptivePills': new FormControl(),
    'chkICUD': new FormControl(),
    'chkHRT': new FormControl(),
    'chkVaginalDischarge': new FormControl(),
    'chkPostPertum': new FormControl(),
    'chkAbnormalBleeding': new FormControl(),
    'chkPostMenopause': new FormControl()
  });

  constructor(private commonService: CommonService, private t13115Service: T13115Service, private messageService: MessageService,
    private route: ActivatedRoute, private router: Router, private ngxService: NgxUiLoaderService) { }

  ngOnInit(): void {
    this.ngxService.start();
    this.userLang = localStorage.getItem('userLang') as string;
    this.commonService.getAllMessage(`
        'S0313',/*Generic required*/
        'S0349',/*No record found*/
        'S0360',/*Check permission*/
        'N0024',/*Data saved*/
        'S0566',/*Cannot Generate Request for the Lab Analysis! for this Gender*/
        'S0328',/*No appointmet/arrival found for thos patient..*/
        'S0005',/*Invalid Value Press F9/ Double Click For List*/
        'S0008', /*Entry is required*/
        'S0144',/*Analysis already specified in this Request; Correct entry.*/
        'S0145',/*Cannot Insert record to a closed/cancelled request.*/
        'S0566',/*Cannot Generate Request for this analysis! for this Gender*/
        'S0566',/*Cannot Generate Request for this analysis! for this Gender*/
        'S0328'/*No Appointment/Arrival found for this Patien! Correct Entry*/
      `).subscribe((success: any) => {
      this.messages = success;
    });
    this.onInitialPatData();
    //setTimeout(() => { document.getElementById('txtRequestNo')?.focus(); }, 0);
    this.commonService.getFormLabel('T13115').subscribe((success: any) => {
      this.labelsNew = success;
    });

  }
  onInitialPatData() {
    this.patNo = this.route.snapshot.paramMap.get('patNo');
    this.patType = this.route.snapshot.paramMap.get('patType');
    this.episodeNo = this.route.snapshot.paramMap.get('visitNo');

    //clinic code >locationCode  from main page
    //this.locationCode = this.route.snapshot.paramMap.get('visitNo');
    //this.docCode = this.route.snapshot.paramMap.get('vi5sitNo');

    this.t13115Service.getPatInfo(this.patNo).subscribe((success: any) => {
      this.userForm.get('txtPatNo')?.setValue(success[0].T_PAT_NO);
      this.userForm.get('txtPatName')?.setValue(success[0].PAT_NAME);
      this.userForm.get('txtYears')?.setValue(success[0].YEARS);
      this.userForm.get('txtMonths')?.setValue(success[0].MONTHS);
      this.userForm.get('txtNationality')?.setValue(success[0].NATIONALITY);
      this.userForm.get('txtMStatus')?.setValue(success[0].MRTL_STATUS);
      this.userForm.get('txtGender')?.setValue(success[0].GENDER);
      this.userForm.get('txtEpisodeNo')?.setValue(this.episodeNo);

    });
    this.onPatientTypeLoad();
    this.onPrioritiesLoad();
    this.onWorkStationLoad();
    this.onNatORload();
    this.diagList = [{ value: '1', label: 'Diagnosis' }, { value: '2', label: 'Followup' }];
    this.loadRequestData();
  }
  onInitRequestData16(i: any) {
    this.RequestData.push({
      SL_NO: i, T_WS_CODE: '', WS_NAME: '', T_ANALYSIS_CODE: '', ANALYSIS_NAME: '', T_TB_DIAG: '1', COMMENTS: '', T_SPECIMEN_CODE: '', SPECIMEN_NAME: '', T_GROUP_FLAG: '',
      T_ABNO_BLE_YN: '', T_VAGIN_YN: '', T_IUCD_YN: '', T_CHEM_IRRA_YN: '', T_POST_MENO_YN: '', T_POST_PART_YN: '', T_HRT_YN: '', T_CONT_YN: '', T_PREG_YN: '', IS_UPDATE: '0', WS_O: '', ANA_O: ''
    });
  }
  loadRequestData() {
    this.RequestData = [];
    for (var i = 0; i < 20; i++) {
      this.onInitRequestData16(i);
      this.labData.push({ IS_CHECKED: false, ANALYSIS_NAME: '' });
    }
    this.t13015 = [];
    this.t13015.push({
      T_PAT_NO: this.patNo, T_DOC_CODE: this.docCode, T_LOCATION_CODE: this.locationCode, T_PAT_TYPE: this.patType, T_PRIORITY_CODE: '', T_CLINIC_DATA: '',
      T_EPISODE_NO: this.episodeNo, T_SPECIMEN_TAKEN_DATE: '', T_COMMENT_LINE: '', T_LAB_NO: '', T_INDICATION: '', T_LMP: '', T_SPEC_EXAM: '', T_COLP_FIND: ''
    });
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
  onLabelLoad(e: string) {
    try {
      return this.labelsNew.filter((a: { T_LABEL_NAME: string; }) => a.T_LABEL_NAME == e)[0].T_LABEL_TEXT;
    } catch (e) {
      return '';
    }
  }
  makeEmpty() {
    this.userForm.reset();
    this.requestNo = '';
    setTimeout(() => { document.getElementById('txtRequestNo')?.focus(); }, 0);
    this.RequestData = [];
    this.labData = [];
    this.onInitialPatData();
  }
  validateFormFields(formGroup: FormGroup) {
    if (!formGroup.controls['txtPatNo'].valid) {
      formGroup.controls['txtPatNo'].markAsDirty();
      this.messageService.add({ severity: 'error', summary: document.getElementById('lblPatNo')?.innerText, detail: this.messages.find(x => x.CODE === 'S0313').TEXT });
    }
    if (!formGroup.controls['txtClinicData'].valid) {
      formGroup.controls['txtClinicData'].markAsDirty();
      this.messageService.add({ severity: 'error', summary: document.getElementById('lblClinicData')?.innerText, detail: this.messages.find(x => x.CODE === 'S0313').TEXT });
    }
    if (!formGroup.controls['txtComments'].valid) {
      formGroup.controls['txtComments'].markAsDirty();
      this.messageService.add({ severity: 'error', summary: document.getElementById('lblComments')?.innerText, detail: this.messages.find(x => x.CODE === 'S0313').TEXT });
    }

    if (!formGroup.controls['ddlPriority'].valid) {
      formGroup.controls['ddlPriority'].markAsDirty();
      this.messageService.add({ severity: 'error', summary: document.getElementById('lblPriority')?.innerText, detail: this.messages.find(x => x.CODE === 'S0313').TEXT });
    }
    if (!formGroup.controls['txtTestToBeDone'].valid) {
      formGroup.controls['txtTestToBeDone'].markAsDirty();
      this.messageService.add({ severity: 'error', summary: document.getElementById('lblSpecimenDate')?.innerText, detail: this.messages.find(x => x.CODE === 'S0313').TEXT });
    }
  }
  onRequestNoBlur() {
    this.requestNo = this.userForm.get('txtRequestNo')?.value;
    if (this.requestNo) {
      this.ngxService.start();
      this.requestNo = this.requestNo.padStart(10, '0');
      this.userForm.get('txtRequestNo')?.setValue(this.requestNo);
      this.t13115Service.getRequestInfo15(this.requestNo)
        .subscribe((ss: any) => {
          if (ss) {
            this.showData(ss[0]);
            this.ngxService.stop();
          } else {
            this.ngxService.stop();
            this.makeEmpty();
            this.messageService.add({ severity: 'error', summary: 'No Data Found', detail: this.messages.find(x => x.CODE === 'S0349').TEXT });
          }
        });
    }
  }
  showData(success: any) {
    this.userForm.get('txtRequestDate')?.setValue(success.T_REQUEST_DATE);
    this.userForm.get('txtRequestTime')?.setValue(success.T_REQUEST_TIME);
    this.userForm.get('txtPatNo')?.setValue(success.T_PAT_NO);
    this.userForm.get('txtPatName')?.setValue(success.PAT_NAME);
    this.userForm.get('txtYears')?.setValue(success.YEARS);
    this.userForm.get('txtMonths')?.setValue(success.MONTHS);
    this.userForm.get('txtNationality')?.setValue(success.NATIONALITY);
    this.userForm.get('txtGender')?.setValue(success.GENDER);
    this.userForm.get('txtMStatus')?.setValue(success.MRTL_STATUS);

    this.userForm.get('ddlPriority')?.setValue(this.priorities.find(x => x.PRIORITY_CODE == success.T_PRIORITY_CODE));
    this.userForm.get('ddlPatType')?.setValue(this.patientTypes.find(x => x.T_EPISODE_TYPE == success.T_PAT_TYPE));
    this.userForm.get('txtClinicData')?.setValue(success.T_CLINIC_DATA);
    this.userForm.get('txtTestToBeDone')?.setValue(success.T_SPECIMEN_TAKEN_DATE);
    this.userForm.get('txtTestDay')?.setValue(success.DAY_NAME);

    this.userForm.get('txtComments')?.setValue(success.T_COMMENT_LINE);

    //cervical pap smear  
    this.userForm.get('txtPapLabNo')?.setValue(success.T_LAB_NO);
    this.userForm.get('txtIndication')?.setValue(success.T_INDICATION);
    this.userForm.get('txtLMP')?.setValue(success.T_LMP);
    this.userForm.get('txtSpectrumExam')?.setValue(success.T_SPEC_EXAM);
    this.userForm.get('txtColposcopicFinding')?.setValue(success.T_COLP_FIND);

    this.labNo = this.userForm.get('txtPapLabNo')?.value;
    this.indication = this.userForm.get('txtIndication')?.value;
    this.lmp = this.userForm.get('txtLMP')?.value;
    this.colpFind = this.userForm.get('txtSpectrumExam')?.value;
    this.specExam = this.userForm.get('txtColposcopicFinding')?.value;
    //from 16
    this.t13115Service.getRequestInfo16(this.requestNo)
      .subscribe((success16: any) => {
        if (success16) {
          this.ngxService.stop();
          this.RequestData = success16;
          if (success16.length < 20) {
            let leng = 20 - success16.length;
            for (var i = success16.length; i < leng; i++) {
              this.onInitRequestData16(i);
            }
          }
          this.userForm.controls['chkPregnancy'].setValue(success16.T_PREG_YN);
          this.userForm.controls['chkChemotherapy'].setValue(success16.T_CHEM_IRRA_YN);
          this.userForm.controls['chkContraceptivePills'].setValue(success16.T_CONT_YN);
          this.userForm.controls['chkICUD'].setValue(success16.T_IUCD_YN);
          this.userForm.controls['chkHRT'].setValue(success16.T_HRT_YN);
          this.userForm.controls['chkVaginalDischarge'].setValue(success16.T_VAGIN_YN);
          this.userForm.controls['chkPostPertum'].setValue(success16.T_POST_PART_YN);
          this.userForm.controls['chkAbnormalBleeding'].setValue(success16.T_ABNO_BLE_YN);
          this.userForm.controls['chkPostMenopause'].setValue(success16.T_POST_MENO_YN);
          this.chkPregnancy = this.userForm.get('chkPregnancy')?.value == false ? '0' : '1';
          this.chkChemotherapy = this.userForm.get('chkChemotherapy')?.value == false ? '0' : '1';
          this.chkContraceptivePills = this.userForm.get('chkContraceptivePills')?.value == false ? '0' : '1';
          this.chkICUD = this.userForm.get('chkICUD')?.value == false ? '0' : '1';
          this.chkHRT = this.userForm.get('chkHRT')?.value == false ? '0' : '1';
          this.chkVaginalDischarge = this.userForm.get('chkVaginalDischarge')?.value == false ? '0' : '1';
          this.chkPostPertum = this.userForm.get('chkPostPertum')?.value == false ? '0' : '1';
          this.chkAbnormalBleeding = this.userForm.get('chkAbnormalBleeding')?.value == false ? '0' : '1';
          this.chkPostMenopause = this.userForm.get('chkPostMenopause')?.value == false ? '0' : '1';

        } else {
          this.ngxService.stop();
          this.makeEmpty();
          this.messageService.add({ severity: 'error', summary: 'No Data Found', detail: this.messages.find(x => x.CODE === 'S0349').TEXT });
        }
      },
        error => {
          this.ngxService.stop();
          console.log(error);
        });
  }

  onPatBlur() {

  }
  onSaveClicked() {
    this.t13015[0].T_PRIORITY_CODE = this.userForm.get('ddlPriority')?.value.PRIORITY_CODE;
    this.t13015[0].T_CLINIC_DATA = this.userForm.get('txtClinicData')?.value;
    this.t13015[0].T_SPECIMEN_TAKEN_DATE = this.userForm.get('txtTestToBeDone')?.value;
    this.t13015[0].T_COMMENT_LINE = this.userForm.get('txtComments')?.value;
    //pap smear data for t13015
    this.t13015[0].T_LAB_NO = this.userForm.get('txtPapLabNo')?.value;
    this.t13015[0].T_INDICATION = this.userForm.get('txtIndication')?.value;
    this.t13015[0].T_LMP = this.userForm.get('txtLMP')?.value;
    this.t13015[0].T_SPEC_EXAM = this.userForm.get('txtSpectrumExam')?.value;
    this.t13015[0].T_COLP_FIND = this.userForm.get('txtColposcopicFinding')?.value;

    if (this.canSave) {
      if (this.userForm.valid) {
        this.ngxService.start();
        if (this.RequestData.length > 0) {
          let request16 = this.RequestData.filter(function (r) { return (r.T_WS_CODE != "" && r.IS_UPDATE == "0"); })
          let reqSpAna = "";

          for (var index in request16) {
            if (request16[index].T_ANALYSIS_CODE == "") {
              this.messageService.add({ severity: 'error', summary: 'Required!', detail: 'Analysis is required!' })
              reqSpAna = reqSpAna + " Analysis";
            }
            if (request16[index].T_SPECIMEN_CODE == "") {
              reqSpAna = reqSpAna + " Specimen";
            }
          }
          if (reqSpAna) {
            this.messageService.add({ severity: 'error', summary: 'Required!', detail: reqSpAna + ' is required!' })
            this.ngxService.stop();
            return;
          }
          if (request16.length > 0) {
            this.t13115Service.insertT13115(this.t13015, request16, this.requestNo).subscribe((success: any) => {
              this.userForm.get('txtRequestNo')?.setValue(success.T_REQUEST_NO);
              this.userForm.get('txtRequestDate')?.setValue(success.T_REQUEST_DATE);
              this.userForm.get('txtRequestTime')?.setValue(success.T_REQUEST_TIME);
              this.ngxService.stop();
              this.messageService.add({ severity: 'success', summary: 'Success!', detail: this.messages.find(x => x.CODE === 'N0024').TEXT });
            }, error => {
              this.ngxService.stop();
              console.log(error);
              this.messageService.add({ severity: 'error', summary: 'Error!', detail: 'Data Not Saved' });
            });
          } else {
            this.ngxService.stop();
            this.messageService.add({ severity: 'error', summary: 'Error!', detail: 'No data to save' });
          }

        } else {
        }
      } else {
        this.validateFormFields(this.userForm);
      }
    } else {
      this.messageService.add({ severity: 'error', summary: 'No permission!', detail: this.messages.find(x => x.CODE === 'S0360').TEXT });
    }
  }
  onClearClicked() {
    this.makeEmpty();

  }
  onPatTypeChange() { }
  onPatientTypeLoad() {
    this.t13115Service.getAllPatientType().subscribe((success: any) => {
      this.patientTypes = success;
      this.userForm.get('ddlPatType')?.setValue(this.patientTypes.find(x => x.T_EPISODE_TYPE == this.patType));
    }, error => {
      console.log(error);
      return [];
    });
  }
  onPrioritiesLoad() {
    this.t13115Service.getAllPriority().subscribe((success: any) => {
      this.priorities = success;
    }, error => {
      console.log(error);
      return [];
    });
  }
  onTestToBeDone() {
    if (this.userForm.get('txtTestToBeDone')?.value == null) {
      this.userForm.get('txtTestDay')?.setValue('');
      return;
    }
    if (this.userForm.get('txtTestToBeDone')?.value !== null && this.userForm.get('txtTestToBeDone')?.value !== '') {
      let date = this.userForm.get('txtTestToBeDone')?.value as string;
      let year = parseInt(date.substr(6, 4));
      let month = parseInt(date.substr(3, 2));
      let day = parseInt(date.substr(0, 2));
      const needDate = month + '/' + day + '/' + year;
      if (new Date(needDate).toLocaleDateString("en-GB", { year: 'numeric', month: '2-digit', day: '2-digit' }) === 'Invalid Date') {
        this.messageService.add({ severity: 'warn', summary: 'Error!', detail: 'Date Format Should Be Like "dd/mm/yyyy"' });
        setTimeout(() => { document.getElementById('txtTestToBeDone')?.focus(); }, 0);
        return;
      }
      var d = new Date(needDate);
      var dayName = d.toLocaleDateString('en-US', { weekday: 'long' }).split(',')[0];
      this.userForm.get('txtTestDay')?.setValue(dayName);

    }
  }
  onWorkStationLoad() {
    this.t13115Service.getAllWorkStation().subscribe((success: any) => {
      this.workStation = success;
      //this.wsInTables = [{ items: success }];
    }, error => {
      console.log(error);
      return [];
    });
  }
  onAnalysisLoad(wsCode: any) {
    this.t13115Service.getAnalysisByWs(wsCode).subscribe((success: any) => {
      this.analysis = success;
    }, error => {
      console.log(error);
      return [];
    });
  }
  onWorkStationChange() {
    this.userForm.get('txtSearchTestByName')?.setValue('');
    let wsCode = this.userForm.get('ddlWorkStation')?.value === null ? '' : this.userForm.get('ddlWorkStation')?.value.CODE;
    this.labData = [];
    this.t13115Service.getAnalysisNew(wsCode).subscribe((success: any) => {
      if (success.length > 0) {
        this.labData = success;
        if (success.length < 10) {
          let len = 10 - success.length;
          for (var i = 0; i < len; i++) {
            this.labData.push({ IS_CHECKED: false, ANALYSIS_NAME: '' });
          }
        }
      }
    });
  }
  onSearchTestName() {
    let searchValue = this.userForm.get('txtSearchTestByName')?.value;
    if (searchValue === '')
      return;
    this.labData = [];
    this.t13115Service.getAnalysisNew(this.userForm.get('txtSearchTestByName')?.value).subscribe((success: any) => {
      if (success.length > 0) {
        this.labData = success;
        if (success.length < 10) {
          let len = 10 - success.length;
          for (var i = 0; i < len; i++) {
            this.labData.push({ IS_CHECKED: false, ANALYSIS_NAME: '' });
          }
        }
        this.userForm.get('txtSearchTestByName')?.setValue('');
      }
    });
  }
  //specimen 
  onNatORload() {
    this.t13115Service.getNatOR('').subscribe((success: any) => {
      this.specimen = success;
    }, error => {
      console.log(error);
      return [];
    });
  }
  onRightArrowClick() {
    if (this.labData.length == 0) {
      this.messageService.add({ severity: 'warn', summary: 'Error!', detail: 'No Data left side' });
      return;
    }
    for (var i = 0; i < this.labData.length; i++) {
      if (this.labData[i].IS_CHECKED) {
        var anaCode = this.labData[i].T_ANALYSIS_CODE;
        var isExist = this.RequestData.find(x => x.T_ANALYSIS_CODE === anaCode || x.T_ANALYSIS_CODE === anaCode);
        if (isExist === null || isExist === undefined) {
          var index = this.RequestData.filter(function (h2) { return (h2.T_WS_CODE != ""); });
          this.RequestData[index.length].T_WS_CODE = this.labData[i].T_WS_CODE;
          this.RequestData[index.length].WS_NAME = this.labData[i].WS_NAME;
          this.RequestData[index.length].T_ANALYSIS_CODE = this.labData[i].T_ANALYSIS_CODE;
          this.RequestData[index.length].ANALYSIS_NAME = this.labData[i].ANALYSIS_NAME;
        }
      }
    }
  }
  onLeftArrowClick() {
    if (this.selectedRequest.length === 0 || this.selectedRequest === undefined) {
      this.loadRequestData();
      for (var i = 0; i < this.labData.length; i++) {
        if (this.labData[i].IS_CHECKED) {
          this.labData[i].IS_CHECKED = false;
        }
      }
    }
    else {
      //remove seelcted data 
      this.RequestData = this.RequestData.filter((f) => {
        return f.T_ANALYSIS_CODE != this.selectedRequest.T_ANALYSIS_CODE;
      });
      for (var i = 0; i < this.labData.length; i++) {
        if (this.labData[i].IS_CHECKED && (this.selectedRequest.T_ANALYSIS_CODE === this.labData[i].T_ANALYSIS_CODE)) {
          this.labData[i].IS_CHECKED = false;
        }
      }
    }
    this.selectedRequest = [];
  }
  onRowClick(rowData: any) {
    this.selectedRequest = rowData;
    //console.log(rowData);
  }
  onTextChange(indx: any, value: any) {
    this.RequestData[indx].T_TB_DIAG = value;
    this.rowIndex = indx;
  }
  onSpecimenChange(index: any, value: any) {
    this.RequestData[index].T_SPECIMEN_CODE = value;
  }
  onPrintClicked() {
    if (!this.requestNo) {
      this.messageService.add({ severity: 'warn', summary: 'Error!', detail: 'Please Enter Request No# First' });
      return;
    }
    let ana5001 = this.RequestData.filter(function (a) { return a.T_ANALYSIS_CODE == "5001" })
    if (ana5001)
      window.open("./api/R13051/getData?requestNo=" + this.userForm.get('txtRequestNo')?.value, "popup", "location=1, status=1, scrollbars=1");
    else
      window.open("./api/r13115/getReport?reqNo=" + this.userForm.get('txtRequestNo')?.value, "popup", "location=1, status=1, scrollbars=1");
  }
  getEventValue($event: any): string {
    return $event.target.value;
  }
  //popup
  onWSDblClick(index: any) {
    this.displayWS = true;
    this.rowIndex = index;
  }
  onWSRowDblClick(rowData: any) {
    this.RequestData[this.rowIndex].T_WS_CODE = rowData.CODE;
    this.RequestData[this.rowIndex].WS_NAME = rowData.NAME;
    this.RequestData[this.rowIndex].T_ANALYSIS_CODE = "";
    this.RequestData[this.rowIndex].ANALYSIS_NAME = "";
    this.displayWS = false;
  }
  onWSOkClick() {
    var rowData = this.wsSelectedData;
    if (rowData != null) {
      this.onWSRowDblClick(rowData);
    }
  }
  onAnalysisDblClick(index: any) {
    this.rowIndex = index;
    this.analysis = [];
    var wsCode = this.RequestData[this.rowIndex].T_WS_CODE;
    if (wsCode == '') {
      this.messageService.add({ severity: 'warn', summary: 'Error!', detail: 'Please Enter WorkStation First' });
      return;
    }

    this.displayAnalysis = true;
    if (wsCode == '11') {
      return;
    } else {
      this.onAnalysisLoad(wsCode);
    }
  }
  onAnalysisRowDblClick(rowData: any) {
    var isExistAnalysis = this.RequestData.find(x => x.T_ANALYSIS_CODE === rowData.CODE);
    if (isExistAnalysis) {
      this.messageService.add({ severity: 'warn', summary: 'Error!', detail: 'Duplicate Found! ' + rowData.NAME + ' already exist' });
      this.displayAnalysis = false;
      return;
    }
    //restriction
    this.t13115Service.getAnalysisRestriction(rowData.T_WS_CODE, rowData.CODE)
      .subscribe((success: any) => {
        if (success.length > 0) {
          this.pretestCode = success[0].t_pretest_code;
          this.groupFlag = success[0].t_group_flag;
          this.singleFlag = success[0].t_single_flag;
          this.statReq = success[0].t_stat_req;

          var priority = this.userForm.get('ddlPriority')?.value.CODE;
          if (this.statReq === null && priority == '1') {
            this.messageService.add({ severity: 'warn', summary: 'Error!', detail: 'Cant request Stat for ' + rowData.NAME + ' ! Request as Routine' });
            return;
          }

          if ((rowData.CODE === '0007' || rowData.CODE === '0140') && this.genderDes !== '1') {
            this.messageService.add({ severity: 'warn', summary: 'Error!', detail: this.messages.find(m => m.CODE === 'S0566').TEXT });
            return;
          }
          if (rowData.CODE === '0137' && this.genderDes !== '2') {
            this.messageService.add({ severity: 'error', summary: 'Error!', detail: this.messages.find(m => m.CODE === 'S0566').TEXT });
            return;
          }
          if (rowData.CODE === '22001' && this.externalFlag === null) {
            this.t13115Service.getBloodGroup(this.patNo).subscribe((success: any) => {
              this.messageService.add({ severity: 'error', summary: 'Error!', detail: 'Patient already have Blood Group with previous request# ' || success.T_REQUEST_NO || '  ' || success.bloodGroup });
              return;
            });
          }
        }
      });

    this.RequestData[this.rowIndex].T_ANALYSIS_CODE = rowData.CODE;
    this.RequestData[this.rowIndex].ANALYSIS_NAME = rowData.NAME;

    this.RequestData[this.rowIndex].T_GROUP_FLAG = this.groupFlag;
    this.displayAnalysis = false;

    if (rowData.CODE === '50001')
      this.showFollowup = true;
  }
  onAnalysisOkClick() {
    var rowData = this.analysisSelectedData;
    if (rowData != null) {
      this.onAnalysisRowDblClick(rowData);
    }
  }
  onSpecimenDblClick(index: any) {
    this.displaySpecimen = true;
    this.rowIndex = index;
  }
  onSpecimenRowDblClick(rowData: any) {
    this.RequestData[this.rowIndex].T_SPECIMEN_CODE = rowData.CODE;
    this.RequestData[this.rowIndex].SPECIMEN_NAME = rowData.NAME;
    this.displaySpecimen = false;
  }
  onSpecimenOkClick() {
    var rowData = this.specimenSelectedData;
    if (rowData != null) {
      this.onSpecimenRowDblClick(rowData);
    }
  }

  //pap smear for 16 
  //T_ABNO_BLE_YN,T_VAGIN_YN,T_IUCD_YN,T_CHEM_IRRA_YN,T_POST_MENO_YN,T_POST_PART_YN,T_HRT_YN,T_CONT_YN,
  onChkPregnancy() {
    this.RequestData[this.rowIndex].T_PREG_YN = this.chkPregnancy ? '1' : '';
  }
  onChkChemotherapy() {
    this.RequestData[this.rowIndex].T_CHEM_IRRA_YN = this.chkChemotherapy ? '1' : '';
  }
  onChkContraceptivePills() {
    this.RequestData[this.rowIndex].T_CONT_YN = this.chkContraceptivePills ? '1' : '';
  }
  onChkICUD() {
    this.RequestData[this.rowIndex].T_IUCD_YN = this.chkICUD ? '1' : '';
  }
  onChkHRT() {
    this.RequestData[this.rowIndex].T_HRT_YN = this.chkHRT ? '1' : '';
  }
  onChkVaginalDischarge() {
    this.RequestData[this.rowIndex].T_VAGIN_YN = this.chkVaginalDischarge ? '1' : '';
  }
  onChkPostPertum() {
    this.RequestData[this.rowIndex].T_POST_PART_YN = this.chkPostPertum ? '1' : '';
  }
  onChkAbnormalBleeding() {
    this.RequestData[this.rowIndex].T_ABNO_BLE_YN = this.chkAbnormalBleeding ? '1' : '';
  }
  onChkPostMenopause() {
    this.RequestData[this.rowIndex].T_POST_MENO_YN = this.chkPostMenopause ? '1' : '';
  }

  onReqByPatDblClick() {
    this.t13115Service.getRequestByPatNo(this.patNo).subscribe((success: any) => {
      this.ReqListByPat = success;
      this.displayReqByPat = true;
    })
  }
  onReqByPatOkClick() {
    this.displayReqByPat = false;
    var rowData = this.reqListSelectedData;
    if (rowData != null)
      this.onReqByPatRowDblClick(rowData);

  }
  onReqByPatRowDblClick(data: any) {
    this.displayReqByPat = false;
    this.userForm.get('txtRequestNo')?.setValue(data.T_REQUEST_NO);
    this.onRequestNoBlur();
  }
}
