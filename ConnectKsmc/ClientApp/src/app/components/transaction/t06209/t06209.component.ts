import { Component, OnInit, Renderer2 } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { MessageService } from 'primeng/api';
import { CommonService } from '../../../services/common.service';
import { T06209Service } from '../../../services/transaction/t06209.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 't06209',
  templateUrl: 't06209.component.html',
  providers: [T06209Service],
  styleUrls: ['t06209.css'],
})
export class T06209Component implements OnInit {
  PatNoPopupDisplay: boolean = false;
  DoctorPopupDisplay: boolean = false;
  RecomPopupDisplay: boolean = false;
  MedHxPopupDisplay: boolean = false;
  txtadmissionHide: boolean = true;
  txtVisitHide: boolean = true;
  disableAllergy: boolean = true;
  disabled: boolean = true;
  pregDisable: boolean = true;
  smokeDisable: boolean = true;
  HeadFlagdisabled: boolean = true;

   checkedAllergy: string = '';
  DM_model: string = '';
  HTN_model: string = '';
  HYPER_model: string = '';
  HD_model: string = '';
  CVA_model: string = '';
  RENAL_model: string = '';
  Tetenus_Flag: string = '';
  HaedFlagCheck: string = '';
  obj: any = new Object();
  messages: any[] = [];

  BmiIndexDropDownList: any[] = [];
  BPIndexDropDownList: any[] = [];
  TempDropDownList: any[] = [];
  PulseDropDownList: any[] = [];
  RRDropDownList: any[] = [];
  GLDropDownList: any[] = [];
  MedHxDropDownList: any[] = [];
  AllergyDietDropDownList: any[] = [];
  AllergyMedDropDownList: any[] = [];
  RecommendationDropDownList: any[] = [];

  SmokingDropDownList: any[] = [
    { CODE: 'Y', NAME: 'YES' },
    { CODE: '', NAME: 'NO' },
  ];
  PregnantDropDownList: any[] = [
    { CODE: '1', NAME: 'YES' },
    { CODE: '', NAME: 'NO' },
  ];
  LocationDropDownlist: any[] = [
    { CODE: 'Y', NAME: 'YES' },
    { CODE: 'N', NAME: 'NO' },
  ];

  PatientPopUpDataList: any[] = [];
  PatientPopUpDataListTemp: any[] = [];
  DoctorPopUpDataList: any[] = [];
  DoctorPopUpDataListTemp: any[] = [];
  DocWardEpiDateByTypeList: any[] = [];
  canSave = false;
  canUpdate = false;
  canDelete = false;
  canQuery = false;
  userLang = '';
  search: any;
  selectedBMIindexCode: string = '';
  selectedBPCode: string = '';
  selected_Temp_Code: string = '';
  selected_PULSE_Code: string = '';
  selected_RESPIRATION_Code: string = '';
  selected_Random_Code: string = '';
  selected_Recommendation_Value: object = {};
  selected_MedEx_Value: object = {};
   selectAllergyDiet: any = '';  
  selectAllergyMed: any = '';
  selectSmoke: object = {};
  selectPregnant: any = '';
  selected_Location_Code: any = '';

  patientType: string = '';
  PopupHeader: string = '';
  bmiValue: any = '';
  sub: any;
  patNo = '';
  userForm = new FormGroup({
    txtPatType: new FormControl(),
    txtPatNo: new FormControl(),
    txtPatName: new FormControl(),
    txtGender: new FormControl(),
    txtStatus: new FormControl(),
    txtAge: new FormControl(),
    txtMonth: new FormControl(),
    txtNationality: new FormControl(),
    txtDate: new FormControl(),
    txtTime: new FormControl(),
    txtDocCode: new FormControl(),
    txtDocName: new FormControl(),
    txtClinic: new FormControl(),
    txtClinicName: new FormControl(),
    txtWeight: new FormControl(),
    txtHeight: new FormControl(),
    txtBmi: new FormControl(),
    txtUpperLimb1: new FormControl(),
    txtUpperLimb2: new FormControl(),
    txtTemperature: new FormControl(),
    txtPulse: new FormControl(),
    txtRespiration: new FormControl(),
    txtSpo2: new FormControl(),
    txtHandCircumference: new FormControl(),
    txtHeadCircum: new FormControl(),
    txtGlucose: new FormControl(),
    txtRandom: new FormControl(),
    txtNotes: new FormControl(),
    txtOtherAllergy: new FormControl(),
    txtSD: new FormControl(),
    txtRecomPopInput: new FormControl(),
    txtWeek: new FormControl(),
    txtLMP: new FormControl(),
    txtGravida: new FormControl(),
    txtPara: new FormControl(),
    txtAbortion: new FormControl(),
    txtLastDose: new FormControl(),
    txtEnteredBy: new FormControl(),
    txtEntryDate: new FormControl(),
    txtEntryTime: new FormControl(),
    txtModifiedBy: new FormControl(),
    txtModifiedDate: new FormControl(),
    txtMedEx: new FormControl(),
    txtEpisode: new FormControl(),
    txtOpArrivalNo: new FormControl(),
    txtOpArrivalDate: new FormControl(),
    txtVisitNo: new FormControl(),
    txtEpisodeNo: new FormControl(),
    txtRecommendation: new FormControl(),
    ddlBmiIndex: new FormControl(),
    ddlBloodPressure: new FormControl(),
    ddlMedicalHx: new FormControl(),
    ddlAllergyDiet: new FormControl(),
    ddlAllergyMed: new FormControl(),
    ddlLocation: new FormControl(),
    ddlPulse: new FormControl(),
    ddlRespiration: new FormControl(),
    ddlRandom: new FormControl(),
    ddlRecommendation: new FormControl(),
    ddlTemp: new FormControl(),
    ddlPregnant: new FormControl(),
    ddlSmoking: new FormControl(),
    chkChangeAllergy: new FormControl(),
    chkChangeTetenus: new FormControl(),
    chkChangeDM: new FormControl(),
    chkChangeHTM: new FormControl(),
    chkChangeHyper: new FormControl(),
    chkChangeHD: new FormControl(),
    chkChangeCVA: new FormControl(),
    chkChangeRenal: new FormControl(),
    chkHeadFlag: new FormControl(),
  });
  constructor(
    public sanitizer: DomSanitizer,
    private commonService: CommonService,
    private t06209Service: T06209Service,
    private messageService: MessageService,
    private router: Router,
    private route: ActivatedRoute,
    private ngxService: NgxUiLoaderService
  ) {}

  ngOnInit(): void {
    this.ngxService.start();
    this.userLang = localStorage.getItem('userLang') as string;
    this.commonService
      .getAllMessage(
        `
        'S0313'/*Generic required*/,
        'S0349'/*No record found*/,
        'S0360'/*Check permission*/,
        'N0024'/*Data saved*/
      `
      )
      .subscribe((success: any) => {
        this.messages = success;
      });
    this.t06209Service.getBMIindex().subscribe((success: any) => {
      this.BmiIndexDropDownList = success;
    });
    this.t06209Service.getBPindex().subscribe((success: any) => {
      this.BPIndexDropDownList = success;
    });
    this.t06209Service.getTempindex().subscribe((success: any) => {
      this.TempDropDownList = success;
    });
    this.t06209Service.getPulseindex().subscribe((success: any) => {
      this.PulseDropDownList = success;
    });
    this.t06209Service.getRRindex().subscribe((success: any) => {
      this.RRDropDownList = success;
    });
    this.t06209Service.getGLindex().subscribe((success: any) => {
      this.GLDropDownList = success;
    });
    this.t06209Service.getMedHxindex().subscribe((success: any) => {
      this.MedHxDropDownList = success;
    });
    this.t06209Service.getAllergyDietindex().subscribe((success: any) => {
      this.AllergyDietDropDownList = success;
    });
    this.t06209Service.getAllergyMedindex().subscribe((success: any) => {
      this.AllergyMedDropDownList = success;
    });
    this.t06209Service
      .getRecommendationDropDownlist()
      .subscribe((success: any) => {
        this.RecommendationDropDownList = success;
      });
    //this.t06209Service.getPatListPopData().subscribe((success: any) => { this.PatientPopUpDataList = success; this.PatientPopUpDataListTemp = success; });
    this.t06209Service.getDoctorListPopData().subscribe((success: any) => {
      this.DoctorPopUpDataList = success;
      this.DoctorPopUpDataListTemp = success;
    });
    var patNo = this.route.snapshot.paramMap.get('patNo');
    if (patNo != null) {
      //this.onPatBlur(patNo, '1');
    }
    this.ngxService.stop();
    this.makeEmpty();
    this.sub = this.route.params.subscribe((params) => {
      if (params) {
        this.userForm.get('txtPatNo')?.setValue(params['patNo']);
        this.OnBlurPatNo();
        this.RiskFactorCheckBoxes();
        
      }
    });
    this.onNextClicked();
  }
  setPermission(permissions: any) {
    if (permissions.canOpen) {
      this.canSave = permissions.canSave;
      this.canUpdate = permissions.canUpdate;
      this.canDelete = permissions.canDelete;
      this.canQuery = permissions.canQuery;
    } else {
      this.messageService.add({
        severity: 'error',
        summary: 'No permission!',
        detail: this.messages.find((x) => x.CODE === 'S0360').TEXT,
      });
      document.getElementById('btnLogout')?.click();
    }
  }
  makeEmpty() {
    this.userForm.reset();
  }
  patListPopup() {
    this.PatNoPopupDisplay = true;
  }
  onSelectPatPopup(y: any) {
    this.userForm.get('txtPatNo')?.setValue(y.T_PAT_NO);
    this.userForm.get('txtPatType')?.setValue(y.TYPE);
    this.userForm.get('txtPatName')?.setValue(y.PAT_NAME);
    this.userForm.get('txtGender')?.setValue(y.GENDER_DSCRPTN);
    this.userForm.get('txtStatus')?.setValue(y.T_MRTL_DESC);
    this.patientType = y.PAT_TYPE;

    if (
      y.GENDER_DSCRPTN.toUpperCase() == 'FEMALE' &&
      y.T_MRTL_DESC.toUpperCase() == 'MARRIED'
    ) {
      this.pregDisable = false;
    }
    if (y.GENDER_DSCRPTN.toUpperCase() == 'FEMALE') {
      this.smokeDisable = true;
      this.selectSmoke = {};
      this.userForm.get('txtSD')?.setValue('');
    } else {
      this.smokeDisable = false;
    }
    this.userForm.get('txtAge')?.setValue(y.AGE_Y);
    this.userForm.get('txtMonth')?.setValue(y.AGE_M);
    this.userForm.get('txtNationality')?.setValue(y.NTNLTY_DSCRPTN);
    this.t06209Service
      .getDocWardEpiDateByType(y.PAT_TYPE, y.T_PAT_NO)
      .subscribe((success: any) => {
        if (success.length != 0) {
          if (y.PAT_TYPE == '1') {
            this.userForm
              .get('txtEpisodeNo')
              ?.setValue(success[0].T_EPISODE_NO);
            this.userForm
              .get('txtDocCode')
              ?.setValue(success[0].T_ADMIT_DOC_CODE);
            this.userForm.get('txtDocName')?.setValue(success[0].DOC_NAME);
            this.userForm
              .get('txtClinic')
              ?.setValue(success[0].T_ADMIT_WARD_NO);
            this.userForm
              .get('txtClinicName')
              ?.setValue(success[0].CLINIC_NAME);
            this.userForm
              .get('txtOpArrivalDate')
              ?.setValue(success[0].T_ADMIT_DATE);
            this.txtadmissionHide = false;
          } else if (y.PAT_TYPE == '2') {
            this.userForm
              .get('txtEpisodeNo')
              ?.setValue(success[0].T_EPISODE_NO);
            this.userForm
              .get('txtDocCode')
              ?.setValue(success[0].T_ADMIT_DOC_CODE);
            this.userForm
              .get('txtClinic')
              ?.setValue(success[0].T_ADMIT_WARD_NO);
            this.userForm
              .get('txtOpArrivalDate')
              ?.setValue(success[0].T_ADMIT_DATE);
          } else if (y.PAT_TYPE == '3') {
            this.userForm
              .get('txtEpisodeNo')
              ?.setValue(success[0].T_EPISODE_NO);
            this.userForm
              .get('txtDocCode')
              ?.setValue(success[0].T_ADMIT_DOC_CODE);
            this.userForm
              .get('txtClinic')
              ?.setValue(success[0].T_ADMIT_WARD_NO);
            this.userForm
              .get('txtOpArrivalDate')
              ?.setValue(success[0].T_ADMIT_DATE);
          }
        }
      });
    this.PatNoPopupDisplay = false;
  }

  dblPatNo() {
    // this.PatNoPopupDisplay = true;
    debugger;
    const pageHistory = localStorage.getItem('pageHistory');
    if (pageHistory !== null) {
      const history = JSON.parse(pageHistory);
      const basePath = localStorage.getItem('basePath') as string;
      let currentLocation = location.pathname.replace(basePath, '');
      if (!currentLocation.endsWith('Transaction/T06209'))
        currentLocation = currentLocation.replace(
          currentLocation.substr(
            currentLocation.indexOf('Transaction/T06209') + 18
          ),
          ''
        );
      history.push(currentLocation);
      localStorage.setItem('pageHistory', JSON.stringify(history));
      this.router.navigate(['Transaction/T12214']);
    }
  }

  OnBlurPatNo() {
    if (this.patNo === this.userForm.get('txtPatNo')?.value) return;
    if (this.userForm.get('txtPatNo')?.value !== 'undefined' && this.userForm.get('txtPatNo')?.value !== null &&  this.userForm.get('txtPatNo')?.value !== '') {
      this.patNo = this.userForm.get('txtPatNo')?.value;
      this.patNo = this.patNo.padStart(8, '0');
      this.userForm.get('txtPatNo')?.setValue(this.patNo);
      this.t06209Service.getPatListPopData(this.userForm.get('txtPatNo')?.value).subscribe((success: any) => {
        this.PatientPopUpDataList = success;
        this.PatientPopUpDataListTemp = success;
        this.userForm.get('txtPatType')?.setValue(this.PatientPopUpDataList[0].TYPE);
        this.userForm.get('txtPatName')?.setValue(this.PatientPopUpDataList[0].PAT_NAME);
        this.userForm.get('txtGender')?.setValue(this.PatientPopUpDataList[0].GENDER_DSCRPTN);
        this.userForm.get('txtStatus')?.setValue(this.PatientPopUpDataList[0].T_MRTL_DESC);
        this.patientType = this.PatientPopUpDataList[0].PAT_TYPE;
        if (this.PatientPopUpDataList[0].GENDER_DSCRPTN.toUpperCase() == 'FEMALE' && this.PatientPopUpDataList[0].T_MRTL_DESC.toUpperCase() == 'MARRIED') {
          this.pregDisable = false;
        }
        if (this.PatientPopUpDataList[0].GENDER_DSCRPTN.toUpperCase() == 'FEMALE') {
          this.smokeDisable = true;
          this.selectSmoke = {};
          this.userForm.get('txtSD')?.setValue('');
        } else {
          this.smokeDisable = false;
        }
        this.userForm
          .get('txtAge')
          ?.setValue(this.PatientPopUpDataList[0].AGE_Y);
        this.userForm
          .get('txtMonth')
          ?.setValue(this.PatientPopUpDataList[0].AGE_M);
        this.userForm
          .get('txtNationality')
          ?.setValue(this.PatientPopUpDataList[0].NTNLTY_DSCRPTN);
        this.t06209Service
          .getDocWardEpiDateByType(
            this.PatientPopUpDataList[0].PAT_TYPE,
            this.PatientPopUpDataList[0].T_PAT_NO
          )
          .subscribe((success: any) => {
            this.DocWardEpiDateByTypeList = success;
            //console.log(success);
            if (success.length != 0) {
              if (this.PatientPopUpDataList[0].PAT_TYPE == '1') {
                this.userForm.get('txtEpisodeNo')?.setValue(success[0].T_EPISODE_NO);
                this.userForm.get('txtDocCode')?.setValue(success[0].T_ADMIT_DOC_CODE);
                this.userForm.get('txtDocName')?.setValue(success[0].DOC_NAME);
                this.userForm.get('txtClinic')?.setValue(success[0].T_ADMIT_WARD_NO);
                this.userForm.get('txtClinicName')?.setValue(success[0].CLINIC_NAME);
                this.userForm.get('txtOpArrivalDate')?.setValue(success[0].T_ADMIT_DATE);
                this.txtadmissionHide = false;
              } else if (this.PatientPopUpDataList[0].PAT_TYPE == '2') {
                this.userForm.get('txtEpisodeNo')?.setValue(success[0].T_EPISODE_NO);
                this.userForm.get('txtDocCode')?.setValue(success[0].T_CLINIC_DOC_CODE);
                this.userForm.get('txtDocName')?.setValue(success[0].DOC_NAME);
                this.userForm.get('txtClinic')?.setValue(success[0].T_CLINIC_CODE);
                this.userForm.get('txtClinicName')?.setValue(success[0].CLINIC_NAME);
                this.userForm.get('txtOpArrivalNo')?.setValue(success[0].T_ARRIVAL_NO);              
                this.userForm.get('txtOpArrivalDate')?.setValue(success[0].T_ARRIVAL_DATE);
                this.userForm.get('txtVisitNo')?.setValue(success[0].T_VISIT_NO);
                this.txtadmissionHide = false;
                this.txtVisitHide = false;
              } else if (this.PatientPopUpDataList[0].PAT_TYPE == '3') {
                this.userForm.get('txtEpisodeNo')?.setValue(success[0].T_EPISODE_NO);
                this.userForm.get('txtDocCode')?.setValue(success[0].T_ADMIT_DOC_CODE);
                this.userForm.get('txtClinic')?.setValue(success[0].T_ADMIT_WARD_NO);
                this.userForm.get('txtOpArrivalDate')?.setValue(success[0].T_ADMIT_DATE);
              }
            }
          });
        // this.PatNoPopupDisplay = false;
      });
    }
    // this.userForm.get("txtPatNo")?.setValue(this.PatientPopUpDataList[0].T_PAT_NO);
  }

  OnAllergyCheckBox(event: any) {
    //this.checkedAllergy;
    if (event.checked == true) {
      this.disableAllergy = false;
      this.disabled = false;
    } else {
      // this.AllergyDietDropDownList;
      this.selectAllergyDiet = '';
      this.disableAllergy = true;
      this.disabled = true;
    }
  }

  OnHeadFlagChange(event: any) {
    if (event.checked == true) {
      this.HeadFlagdisabled = false;
    } else {
      this.HeadFlagdisabled = true;
      this.userForm.get('txtHeadCircum')?.setValue('');
    }
  }

  OnSmokingChange(event: any) {
    let smokingVal = event.value;
    if (smokingVal == 'Y') {
      this.smokeDisable = false;
    }
  }
  OnBlurSetBMIindex(e: any) {
    let bmiValue = e.target.value;
    if (bmiValue != '' && bmiValue != null) {
      if (bmiValue < 18.5) {
        this.selectedBMIindexCode = '0501';
      } else if (bmiValue >= 18.5 && bmiValue <= 24.9) {
        this.selectedBMIindexCode = '0502';
      } else if (bmiValue >= 25 && bmiValue <= 29.9) {
        this.selectedBMIindexCode = '0503';
      } else if (bmiValue >= 30 && bmiValue <= 34.9) {
        this.selectedBMIindexCode = '0504';
      } else if (bmiValue >= 35 && bmiValue <= 39.9) {
        this.selectedBMIindexCode = '0505';
      } else {
        this.selectedBMIindexCode = '';
      }
    } else {
      this.selectedBMIindexCode = '';
    }
  }
  bpSystolicValidate(event: any) {
    let bpSystolic = event.target.value;
    let bpDiastolic = this.userForm.get('txtUpperLimb2')?.value;
    if (bpSystolic != null && bpSystolic != undefined && bpSystolic != '') {
      if (bpSystolic < 120 && bpDiastolic < 80) {
        this.selectedBPCode = '0102';
      } else if (bpSystolic >= 120 && bpSystolic <= 129 && bpDiastolic < 80) {
        this.selectedBPCode = '0101';
      } else if (
        bpSystolic >= 130 &&
        bpSystolic <= 139 &&
        bpDiastolic >= 80 &&
        bpDiastolic <= 89
      ) {
        this.selectedBPCode = '0103';
      } else if (bpSystolic > 139 && bpDiastolic > 89) {
        this.selectedBPCode = '0104';
      } else {
        this.selectedBPCode = '';
      }
    } else {
      this.selectedBPCode = '';
    }
  }
  bpDiastolicValidate(event: any) {
    let bpDiastolic = event.target.value;
    let bpSystolic = this.userForm.get('txtUpperLimb1')?.value;
    if (bpDiastolic != null && bpDiastolic != undefined && bpDiastolic != '') {
      if (bpSystolic < 120 && bpDiastolic < 80) {
        this.selectedBPCode = '0102';
      } else if (bpSystolic >= 120 && bpSystolic <= 129 && bpDiastolic < 80) {
        this.selectedBPCode = '0101';
      } else if (
        bpSystolic >= 130 &&
        bpSystolic <= 139 &&
        bpDiastolic >= 80 &&
        bpDiastolic <= 89
      ) {
        this.selectedBPCode = '0103';
      } else if (bpSystolic > 139 && bpDiastolic > 89) {
        this.selectedBPCode = '0104';
      } else {
        this.selectedBPCode = '';
      }
    } else {
      this.selectedBPCode = '';
    } //else part of first if
  } //Blood Pressure Diastolic function END---

  temperatureValidation(event: any) {
    let bodyTemp = event.target.value;
    if (bodyTemp != null && bodyTemp != undefined && bodyTemp != '') {
      if (bodyTemp >= 96.8 && bodyTemp <= 100.4) {
        this.selected_Temp_Code = '0302';
      } else if (bodyTemp > 100.4) {
        this.selected_Temp_Code = '0303';
      } else if (bodyTemp < 96.8) {
        this.selected_Temp_Code = '0301';
      } else {
        this.selected_Temp_Code = '';
      }
    } else {
      this.selected_Temp_Code = '';
    }
  } //Temerature Field Validation End

  PulseValidation(event: any) {
    let PULSE_RATE = event.target.value;
    if (PULSE_RATE != null && PULSE_RATE != undefined && PULSE_RATE != '') {
      if (PULSE_RATE >= 60 && PULSE_RATE <= 100) {
        this.selected_PULSE_Code = '0202';
      } else if (PULSE_RATE > 100) {
        this.selected_PULSE_Code = '0203';
      } else if (PULSE_RATE < 60) {
        this.selected_PULSE_Code = '0201';
      } else {
        this.selected_PULSE_Code = '';
      }
    } else {
      this.selected_PULSE_Code = '';
    }
  } //PULSE Field Validation End

  RespirationValidation(event: any) {
    let RESPIRATION_RATE = event.target.value;
    if (
      RESPIRATION_RATE != null &&
      RESPIRATION_RATE != undefined &&
      RESPIRATION_RATE != ''
    ) {
      if (RESPIRATION_RATE >= 12 && RESPIRATION_RATE <= 24) {
        this.selected_RESPIRATION_Code = '0402';
      } else if (RESPIRATION_RATE > 24) {
        this.selected_RESPIRATION_Code = '0403';
      } else if (RESPIRATION_RATE < 12) {
        this.selected_RESPIRATION_Code = '0401';
      } else {
        this.selected_RESPIRATION_Code = '';
      }
    } else {
      this.selected_RESPIRATION_Code = '';
    }
  }
  RandomValidation(event: any) {
    let glucoseVal = this.userForm.get('txtGlucose')?.value;
    let randomVal = event.target.value;
    if (
      glucoseVal != null &&
      glucoseVal != undefined &&
      glucoseVal != '' &&
      randomVal != null &&
      randomVal != undefined &&
      randomVal != ''
    ) {
      if (glucoseVal < 99 && randomVal < 139) {
        this.selected_Random_Code = '0602';
      } else if (
        glucoseVal >= 100 &&
        glucoseVal <= 125 &&
        randomVal >= 140 &&
        randomVal <= 199
      ) {
        this.selected_Random_Code = '0601';
      } else if (glucoseVal > 126 && randomVal > 200) {
        this.selected_Random_Code = '0603';
      } else {
        this.selected_Random_Code = '';
      }
    } else {
      this.selected_Random_Code = '';
    }
  }
  HeightValidate(event: any) {
    let heightValue = event.target.value;
    if (heightValue != null && heightValue.indexOf('.') != -1) {
      null;
    } else {
      let newHeightVal = heightValue / 100;
      this.userForm.get('txtHeight')?.setValue(newHeightVal);
    }
    let weight = this.userForm.get('txtWeight')?.value;
    let height = event.target.value; //this.userForm.get("txtHeight")?.value;
    if (
      weight != null &&
      height != null &&
      weight != undefined &&
      height != undefined
    ) {
      this.userForm.get('txtBmi')?.setValue(weight / (height * height));
    } else {
      null;
    }
  }
  OnTetenusCheckBox(event: any) {}
  onSelectDoctorPopup(y: any) {
    this.userForm.get('txtDocCode')?.setValue(y.T_DOC_CODE);
    this.userForm.get('txtDocName')?.setValue(y.DOCTOR_NAME);
    this.DoctorPopupDisplay = false;
  }
  onChangeRecommendation(event: any) {
    let selectedItemName = event.value.T_LANG2_NAME;
    let textRecommendationValue = this.userForm.get('txtRecommendation')?.value;
    if (textRecommendationValue == null) {
      this.userForm.get('txtRecommendation')?.setValue(selectedItemName);
      this.selected_Recommendation_Value = {};
    } else {
      this.userForm
        .get('txtRecommendation')
        ?.setValue(textRecommendationValue + ',' + selectedItemName);
      this.selected_Recommendation_Value = {};
    }
  }
  popUpForRecommText() {
    this.RecomPopupDisplay = true;
    let textRecommenVal = this.userForm.get('txtRecommendation')?.value;
    this.obj.Recom = textRecommenVal;
  }
  clickRecomOK() {
    this.userForm.get('txtRecommendation')?.setValue(this.obj.Recom);
    this.RecomPopupDisplay = false;
  }
  clickPopUpCancel(event: any, id: any) {
    debugger;
    if (id == 'med') {
      this.MedHxPopupDisplay = false;
    } else if (id == 'recom') {
      this.RecomPopupDisplay = false;
    }
  }
  clickPopupSearch(event: any, id: any) {}
  OnChangeMedicalHx(event: any) {
    let selectedItemName = event.value.T_LANG2_NAME;
    let textMedExValue = this.userForm.get('txtMedEx')?.value;
    if (textMedExValue == null) {
      this.userForm.get('txtMedEx')?.setValue(selectedItemName);
      this.selected_MedEx_Value = {};
    } else {
      this.userForm
        .get('txtMedEx')
        ?.setValue(textMedExValue + ',' + selectedItemName);
      this.selected_MedEx_Value = {};
    }
  }
  PopUpMedEx(event: any) {
    this.MedHxPopupDisplay = true;
    this.PopupHeader = event.target.id == 'txtNotes' ? 'Notes' : 'Medical Hx';
    this.obj.MedHx = event.target.value;
  }
  clickMedHxOK(event: any) {
    if (this.PopupHeader == 'Notes') {
      this.userForm.get('txtNotes')?.setValue(this.obj.MedHx);
    } else {
      this.userForm.get('txtMedEx')?.setValue(this.obj.MedHx);
    }
    this.MedHxPopupDisplay = false;
  }
  DocCodePopUp() {
    this.DoctorPopupDisplay = true;
  }
  filterGlobalPatList() {
    this.PatientPopUpDataList = this.PatientPopUpDataListTemp.filter(
      (i: { T_PAT_NO: string; PAT_NAME: string }) =>
        i.T_PAT_NO.indexOf(this.search) >= 0 ||
        i.PAT_NAME.indexOf(this.search.toUpperCase()) >= 0
    );
  }
  filterGlobalDoctorList() {
    this.DoctorPopUpDataList = this.DoctorPopUpDataListTemp.filter(
      (i: { T_DOC_CODE: string; DOCTOR_NAME: string }) =>
        i.T_DOC_CODE.indexOf(this.search) >= 0 ||
        i.DOCTOR_NAME.indexOf(this.search.toUpperCase()) >= 0
    );
  }
  OnPregnancyChange(event: any) {
    let pregSel = event.value.CODE;
    let genderDesc = this.userForm.get('txtGender')?.value;
    let mrtlStatus = this.userForm.get('txtStatus')?.value;
    if (pregSel == '1') {
      if (genderDesc != 'FEMALE') {
        this.messageService.add({
          severity: 'error',
          summary: 'No permission!',
          detail: 'Patient is Not Female',
        });
        return;
      } else if (genderDesc == 'FEMALE') {
        if (mrtlStatus == 'Single') {
          this.messageService.add({
            severity: 'error',
            summary: 'No permission!',
            detail: 'Patient is Single, Correct Pregnancy Status',
          });
          return;
        } else {
          this.pregDisable = false;
        }
      }
    }
  }
  onNewClicked() {
    this.onClearClicked();
  }
  RiskFactorCheckBoxes(){
    let patNumber = this.userForm.get('txtPatNo')?.value;
    if (patNumber != undefined) {
      this.t06209Service.getPatRiskFactor(patNumber).subscribe((success: any) => {
        this.DM_model = success[0].T_MED_DM;
        this.HD_model = success[0].T_RF_IHD;
        this.CVA_model = success[0].T_RF_CVA;
        this.HTN_model = success[0].T_MED_HTN;
        this.RENAL_model = success[0].T_RF_RENAL_FAILURE;
        this.HYPER_model = success[0].T_RF_HYPERLIPIDEMIA;
      });
  }
}
  onNextClicked() {
    let patNumber = this.userForm.get('txtPatNo')?.value;
    if (patNumber != undefined) {      
      this.t06209Service.getPatientVitalDetails(patNumber, this.patientType).subscribe((success: any) => {
          console.log(success);
          if(success.length > 0){
            this.userForm.get('txtWeight')?.setValue(success[0].T_WEIGHT);
            this.userForm.get('txtHeight')?.setValue(success[0].T_HEIGHT);
            this.userForm.get('txtBmi')?.setValue(success[0].T_BMI);
            this.selectedBMIindexCode = success[0].T_BMI_INDEX;
            this.userForm.get('txtUpperLimb1')?.setValue(success[0].T_BP_SYSTOLIC);
            this.userForm.get('txtUpperLimb2')?.setValue(success[0].T_BP_DIASTOLIC);
            this.selectedBPCode = success[0].T_BP_INDEX;          
            this.userForm.get('txtTemperature')?.setValue(success[0].T_BODY_TEMP);
            this.selected_Temp_Code = success[0].T_BODY_TEMP_INDEX;
            this.userForm.get('txtPulse')?.setValue(success[0].T_PULSE);
            this.selected_PULSE_Code = success[0].T_PULSE_INDEX;
            this.userForm.get('txtRespiration')?.setValue(success[0].T_RESPIRATION_RATE);
            this.selected_RESPIRATION_Code = success[0].T_RR_INDEX;
            this.userForm.get('txtSpo2')?.setValue(success[0].T_SPO);
            this.userForm.get('txtHandCircumference')?.setValue(success[0].T_CURCUM_HAND);
            this.userForm.get('txtHeadCircum')?.setValue(success[0].T_HEAD_CIRCUMFERENCE);
             this.obj.HaedFlagCheck = success[0].T_HEAD_CIRCUMFERENCE_FLAG == null ? '' : success[0].T_HEAD_CIRCUMFERENCE_FLAG;
             
            //success[0].T_HEAD_CIRCUMFERENCE_FLAG == null ? this.userForm.get('chkHeadFlag')?.setValue('false') : this.userForm.get('chkHeadFlag')?.setValue('true') ;
            this.userForm.get('txtGlucose')?.setValue(success[0].T_GL_FASTING);
            this.userForm.get('txtRandom')?.setValue(success[0].T_GL_RANDOM);
            this.selected_Random_Code = success[0].T_GL_INDEX;
            this.userForm.get('txtMedEx')?.setValue(success[0].T_MEDICAL_HISTORY);
            this.userForm.get('txtNotes')?.setValue(success[0].T_NOTE);
            //this.checkedAllergy = success[0].T_ALLERGY_FLAG;
            this.selectAllergyDiet = success[0].T_ALLERGY_DIET;
            if(success[0].T_ALLERGY_FLAG == '1'){
              this.userForm.get('chkChangeAllergy')?.setValue('true');
                this.disableAllergy = false;
            }
            this.selectAllergyMed = success[0].T_ALLERGY_MEDICATION != null ? success[0].T_ALLERGY_MEDICATION : '';
            this.userForm.get('txtOtherAllergy')?.setValue(success[0].T_ALLERGY_OTHERS);
            this.userForm.get('txtRecommendation')?.setValue(success[0].T_RECOMMENDATIONS);
            this.selectSmoke = success[0].T_SMOKING_YN;
            this.selected_Location_Code = success[0].T_PREGNENCY_LACTATION != null ? success[0].T_PREGNENCY_LACTATION : '';
            this.userForm.get('txtSD')?.setValue(success[0].T_STICK_PER_DAY);
            this.userForm.get('txtEnteredBy')?.setValue(success[0].ENTRY_USER);
            this.userForm.get('txtEntryDate')?.setValue(success[0].ENTRY_DATE);
            this.userForm.get('txtEntryTime')?.setValue(success[0].T_ENTRY_TIME);
            this.userForm.get('txtModifiedBy')?.setValue(success[0].UPDATE_USER);
            this.userForm.get('txtModifiedDate')?.setValue(success[0].UPDATE_DATE);            
          }   
        });
    } else {
      this.messageService.add({
        severity: 'error',
        summary: 'Field Blank',
        detail: 'Must input Patient number.',
      });
    }
  }
  onSaveClicked() {
    this.obj.T_PAT_NO = this.userForm.get('txtPatNo')?.value;
    this.obj.DM_FLAG = this.DM_model ? 'Y' : '';
    this.obj.HD_FLAG = this.HD_model ? 'Y' : '';
    this.obj.CVA_FLAG = this.CVA_model ? 'Y' : '';
    this.obj.HTN_FLAG = this.HTN_model ? 'Y' : '';
    this.obj.HYPER_FLAG = this.HYPER_model ? 'Y' : '';
    this.obj.RENAL_FLAG = this.RENAL_model ? 'Y' : '';
    this.obj.WEIGHT = this.userForm.get('txtWeight')?.value;
    this.obj.HEIGHT = this.userForm.get('txtHeight')?.value;
    let bmivalue = this.userForm.get('txtBmi')?.value ;
    this.obj.BMI = bmivalue == null ? bmivalue :  bmivalue.toPrecision(3)  ;
    this.obj.BMI_INDEX = this.selectedBMIindexCode;
    this.obj.BP_SYS = this.userForm.get('txtUpperLimb1')?.value;
    this.obj.BP_DIA = this.userForm.get('txtUpperLimb2')?.value;
    this.obj.BP_INDEX = this.selectedBPCode;
    this.obj.BODY_TEMP = this.userForm.get('txtTemperature')?.value;
    this.obj.BODY_TEMP_INDEX = this.selected_Temp_Code;
    this.obj.PULSE = this.userForm.get('txtPulse')?.value;
    this.obj.PULSE_INDEX = this.selected_PULSE_Code;
    this.obj.RESPIRATION = this.userForm.get('txtRespiration')?.value;
    this.obj.RR_INDEX = this.selected_RESPIRATION_Code;
    this.obj.HAND_CIR = this.userForm.get('txtHandCircumference')?.value;
    this.obj.HEAD_CIR = this.userForm.get('txtHeadCircum')?.value;
    this.obj.HEAD_FLAG = this.obj.HaedFlagCheck == true? 'Y':'';
    this.obj.GL_FAST = this.userForm.get('txtGlucose')?.value;
    this.obj.GL_RANDOM = this.userForm.get('txtRandom')?.value;
    this.obj.GL_INDEX = this.selected_Random_Code;
    this.obj.MED_HIS = this.userForm.get('txtPulse')?.value;
    this.obj.NOTE = this.userForm.get('txtNotes')?.value;
    // this.obj.ALLERGY_FLAG = this.checkedAllergy ? '1' : '';
     this.obj.ALLERGY_FLAG =this.userForm.get('chkChangeAllergy')?.value ? '1' : '';
    this.obj.ALLERGY_DIET = this.selectAllergyDiet ;
    this.obj.ALLERGY_MED = this.selectAllergyMed != null ? this.selectAllergyMed : '';
    this.obj.ALLERGY_OTHERS = this.userForm.get('txtOtherAllergy')?.value;
    this.obj.RECOM = this.userForm.get('txtRecommendation')?.value;
    this.obj.SMOKE = this.selectSmoke;
    this.obj.STICK_DAY = this.userForm.get('txtSD')?.value;
    if (this.userForm.get('txtGender')?.value.toUpperCase() == 'FEMALE') {
      this.obj.PREG_STATUS =this.selectPregnant != null ? this.selectPregnant : '';
      this.obj.PREG_WEEK = this.userForm.get('txtWeek')?.value;
      this.obj.LMP_DATE = this.userForm.get('txtLMP')?.value;
      this.obj.GRAVIDA = this.userForm.get('txtGravida')?.value;
      this.obj.PARA = this.userForm.get('txtPara')?.value;
      this.obj.ABORTION = this.userForm.get('txtAbortion')?.value;
      this.obj.TETENUS_TOX = this.Tetenus_Flag;
      this.obj.LAST_DOSE = this.userForm.get('txtLastDose')?.value;
    }
    this.obj.LOCATION =this.selected_Location_Code != null ? this.selected_Location_Code : '';
    this.obj.SPO2 = this.userForm.get('txtSpo2')?.value;
    this.obj.PAT_NO = this.userForm.get('txtPatNo')?.value;
    this.obj.EPISODE_TYPE = this.userForm.get('txtPatType')?.value;
    this.obj.EPISODE_TYPE == 'IP' ? '1' : 'OP' ? '2' : 'ER' ? '3' : '';
    this.obj.EPISODE_NO = this.userForm.get('txtEpisodeNo')?.value == 'undefined' ? '' : this.userForm.get('txtEpisodeNo')?.value;
    this.obj.CLINIC_DOC_CODE = this.userForm.get('txtDocCode')?.value;
    this.obj.CLINIC_CODE = this.userForm.get('txtClinic')?.value;
    console.log(this.obj);
    // this.t06209Service.saveData(this.obj).subscribe((success: any) => {
    //   if (success != null) {
    //     this.messageService.add({
    //       severity: 'success',
    //       summary: 'Saved Successsfully',
    //       detail: this.messages.find((x) => x.CODE === 'N0024').TEXT,
    //     });
    //   }
    // });
  }
  onClearClicked() {
    this.userForm.get('txtClncSpclty')?.reset();
    this.userForm.get('txtClinicSpcltyName')?.reset();
    this.userForm.get('txtClinicCode')?.reset();
    this.userForm.get('txtClinicName')?.reset();
    this.userForm.get('txtClinicDoc')?.reset();
    this.userForm.get('txtClinicDocName')?.reset();
    this.userForm.get('ddlApptDate')?.reset();
  }
  onPrintClicked() {
    let T_REQUEST_NO = '0008796512';
    window.open(
      './api/t06029/getReport?T_REQUEST_NO=' + T_REQUEST_NO,
      'popup',
      'location=1, status=1, scrollbars=1'
    );
  }
}
