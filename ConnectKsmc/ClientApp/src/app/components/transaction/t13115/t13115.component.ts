import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { MessageService } from 'primeng/api';
import { CommonService } from '../../../services/common.service';
import { T13115Service } from '../../../services/transaction/t13115.service';


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
  requestNo = '';

  priorities: any[] = [];
  patientTypes: any[] = [];
  workStation: any[] = [];

  labData: any[] = [];
  RequestData: any[] = [];
  diagList: any[] = [];
  rowIndex: number = 0;
  displaySite: boolean = false;
  canEdit = true;
  result: string = '';

  userForm = new FormGroup({
    'txtRequestNo': new FormControl('', Validators.required),
    'txtRequestDate': new FormControl(),
    'txtRequestDateH': new FormControl(),
    'txtRequestTime': new FormControl(),
    'txtPatNo': new FormControl(),
    'txtPatName': new FormControl(),
    'txtYears': new FormControl(),
    'txtMonths': new FormControl(),
    'txtNationalId': new FormControl(),
    'txtNationality': new FormControl(),
    'txtGender': new FormControl(),
    'txtMStatus': new FormControl(),
    'txtLocation': new FormControl(),
    'txtTestToBeDone': new FormControl(),
    'txtTestDay': new FormControl(),
    'ddlWorkStation': new FormControl('', Validators.required),
    'ddlPriority': new FormControl(),
    'ddlPatType': new FormControl(),
    'txtEpisodeNo': new FormControl(),
    'txtClinicData': new FormControl(),
    'txtComments': new FormControl(),
    'txtSearchTestByName': new FormControl()
  });

  constructor(private commonService: CommonService, private t13115Service: T13115Service, private messageService: MessageService, private ngxService: NgxUiLoaderService) { }

  ngOnInit(): void {
    this.ngxService.start();
    this.userLang = localStorage.getItem('userLang') as string;
    this.commonService.getAllMessage(`
        'S0313'/*Generic required*/,
        'S0349'/*No record found*/,
        'S0360'/*Check permission*/,
        'N0024'/*Data saved*/,
        'S0566'/*Cannot Generate Request for the Lab Analysis! for this Gender*/,
        'S0328'/*No appointmet/arrival found for thos patient..*/
      `).subscribe((success: any) => {
      this.messages = success;
    });
    this.makeEmpty();
    this.onPatientTypeLoad();
    this.onPrioritiesLoad();
    this.onWorkStationLoad();
    this.diagList = [{ value: 'Diagnosis', label: 'Diagnosis' }, { value: 'Followup', label: 'Followup' }];
    setTimeout(() => { document.getElementById('txtRequestNo')?.focus(); }, 0);
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
  makeEmpty() {
    this.userForm.reset();
    this.requestNo = '';
    setTimeout(() => { document.getElementById('txtRequestNo')?.focus(); }, 0);
  }
  validateFormFields(formGroup: FormGroup) {
    if (!formGroup.controls['txtRequestNo'].valid) {
      formGroup.controls['txtRequestNo'].markAsDirty();
      this.messageService.add({ severity: 'error', summary: document.getElementById('lblRequestNo')?.innerText, detail: this.messages.find(x => x.CODE === 'S0313').TEXT });
    }
  }
  showData(success: any) {
    this.userForm.get('txtRequestDate')?.setValue(success.T_REQUEST_DATE);
    this.userForm.get('txtRequestTime')?.setValue(success.T_REQUEST_TIME);
    this.userForm.get('txtPatNo')?.setValue(success.T_PAT_NO);
    this.userForm.get('txtPatName')?.setValue(success.T_PAT_NAME);
    this.userForm.get('txtYears')?.setValue(success.T_YEARS);
    this.userForm.get('txtMonths')?.setValue(success.T_MONTHS);
    this.userForm.get('txtNationalId')?.setValue(success.T_NTNLTY_ID);
    this.userForm.get('txtNationality')?.setValue(success.T_NTNLTY);
    this.userForm.get('txtGender')?.setValue(success.T_GENDER);
    this.userForm.get('txtMStatus')?.setValue(success.T_MRTL_STATUS);
    this.userForm.get('txtLocation')?.setValue(success.LOCATION_DSCRPTN);
    this.userForm.get('ddlWorkStation')?.setValue(success.LOCATION_DSCRPTN);
  }
  onRequestNoBlur() {
    if (this.requestNo === this.userForm.get('txtRequestNo')?.value) return;
    if (this.userForm.get('txtRequestNo')?.value !== null && this.userForm.get('txtRequestNo')?.value !== '') {
      this.ngxService.start();
      this.requestNo = this.userForm.get('txtRequestNo')?.value;
      this.requestNo = this.requestNo.padStart(8, '0');
      this.userForm.get('txtRequestNo')?.setValue(this.requestNo);
      this.t13115Service.getDetailInfo(this.userForm.get('txtRequestNo')?.value)
        .subscribe((success: any) => {
          if (success) {
            this.showData(success);
            this.ngxService.stop();
            setTimeout(() => { document.getElementById('txtBloodBring')?.focus(); }, 0);
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
  }
  onSaveClicked() {
    if (this.canSave) {
      if (this.userForm.valid) {
        this.ngxService.start();
        //this.t13115Service.updateT12012(this.userForm.get('txtRequestNo')?.value, this.userForm.get('txtBloodBring')?.value, this.userForm.get('ddlAssignTech')?.value.CODE)
        //  .subscribe(() => {
        //    this.isOldRecord = true;
        //    this.ngxService.stop();
        //    this.messageService.add({ severity: 'success', summary: 'Success!', detail: this.messages.find(x => x.CODE === 'N0024').TEXT });
        //    this.t13115Service.getDetailInfo(this.userForm.get('txtRequestNo')?.value)
        //      .subscribe((success: any) => {
        //        if (success) {
        //          this.showData(success);
        //        }
        //      });
        //  },
        //    error => {
        //      this.ngxService.stop();
        //      if (error.status === 400)
        //        this.messageService.add({ severity: 'error', summary: 'Error!', detail: error.error.msg });
        //      else
        //        console.log(error);
        //    });
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
    }, error => {
      console.log(error);
      return [];
    });
  }
  onWorkStationChange() {
    this.labData = [];
    this.RequestData = [];
    this.t13115Service.getAnalysisNew(this.userForm.get('ddlWorkStation')?.value.CODE).subscribe((success: any) => {
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
    console.log(this.labData);
  }
  onRightArrowClick() {
    if (this.labData.length == 0)
      return;
    var workStation = this.userForm.get('ddlWorkStation')?.value.CODE;
    var wsName = this.userForm.get('ddlWorkStation')?.value.NAME;
    if (workStation == null) {
      return;
    }
    for (var i = 0; i < this.labData.length; i++) {
      var reqIndex = 0;
      if (this.labData[i].IS_CHECKED) {
        reqIndex++;
        var anaCode = this.labData[i].T_ANALYSIS_CODE;
        var anaName = this.labData[i].ANALYSIS_NAME;
        var isExist = this.RequestData.find(x => x.T_ANALYSIS_CODE == anaCode);
        if (isExist == null) {
          this.RequestData.push({
            SL_NO: (this.rowIndex), T_WS_CODE: workStation, WS_NAME: wsName, T_ANALYSIS_CODE: anaCode, ANALYSIS_NAME: anaName, T_TB_DIAG: '', COMMENT: '', T_SPECIMEN_CODE: '', SPECIMEN_NAME: ''
          });
        }
      }
    }
    console.log(this.RequestData);
    console.log('zzzzzzzzzzzzzzzz');
  }
  onLeftArrowClick() {
  }
  onTextChange(indx: any) {
    this.rowIndex = indx;
  }
  onDoctorChanged(indx: any) {
    this.rowIndex = indx;
    this.displaySite = true;
  }
  onSiteDblClick(indx: any) {
  }
  onPrintClicked() {
    window.open("./api/rXXXX/getData?requestNo=" + this.userForm.get('txtRequestNo')?.value, "popup", "location=1, status=1, scrollbars=1");
  }
}
