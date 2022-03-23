import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { MessageService } from 'primeng/api';
import { CommonService } from '../../../services/common.service';
import { T07026Service } from '../../../services/transaction/t07026.service';

@Component({
  selector: 't07026',
  templateUrl: 't07026.component.html',
  providers: [T07026Service]
})
export class T07026Component implements OnInit {
  messages: any[] = [];
  canSave = false;
  canUpdate = false;
  canDelete = false;
  canQuery = false;
  userLang = '';
  assignDoctor = '';
  doctors: any[] = [];
  clinics: any[] = [];
  selectedDoctor: any;
  selectedClinic: any;
  data: any[] = [];
  clinicType = '';
  apptTypes: any[] = [];
  arvlStatus: any[] = [];
  icdList: any[] = [];
  docArvlStatus: any[] = [];
  refType = '';
  appointmentData: any[] = [];
  showAppointment = false;
  index: number = 0;
  showFollowup = false;
  weeks: any[] = [];
  months: any[] = [];

  lblWeeks: any;
  lblMonths: any;
  lblApptDate1: any;
  lblSlot1: any;
  lblApptDate2: any;
  lblSlot2: any;
  lblGridApptDate: any;
  lblGridApptTime: any;
  lblGridApptDay: any;
  lblGridRule: any;

  userForm = new FormGroup({
    'ddlDoctor': new FormControl('', Validators.required),
    'ddlClinic': new FormControl('', Validators.required),
    'txtScheduleRule': new FormControl(),
    'txtApptDate': new FormControl('', Validators.required),
    'txtApptDay': new FormControl(),
    'txtEligibility': new FormControl(),
    'txtEligible': new FormControl(),
    'txtApptEntryUser': new FormControl(),
    'txtApptEntryDate': new FormControl(),
    'txtICD10Desc': new FormControl(),
    'txtRemarks': new FormControl(),
    'txtComment': new FormControl(),
    'ddlWeeks': new FormControl(),
    'ddlMonths': new FormControl(),
    'txtApptDate1': new FormControl(),
    'txtSlot1': new FormControl(),
    'txtApptDate2': new FormControl(),
    'txtSlot2': new FormControl(),
  });

  constructor(private commonService: CommonService, private t07026Service: T07026Service, private messageService: MessageService, private router: Router, private ngxService: NgxUiLoaderService) { }

  ngOnInit(): void {
    this.ngxService.start();
    this.userLang = localStorage.getItem('userLang') as string;
    this.commonService.getAllMessage(`
        'S0313'/*Generic required*/,
        'S0360'/*Check permission*/,
        'S0614'/*Only Consultant Allowed*/,
        'N0024'/*Data saved*/
      `).subscribe((success: any) => {
      this.messages = success;
    });
    this.t07026Service.getAssignDoctor()
      .subscribe((success: any) => {
        this.assignDoctor = success;
      });
    this.t07026Service.getAllDoctors('', '')
      .subscribe((success: any) => {
        this.doctors = [{ items: success }];
      });
    this.t07026Service.getAllClinics('')
      .subscribe((success: any) => {
        this.clinics = [{ items: success }];
      });
    this.t07026Service.getAllApptTypes()
      .subscribe((success: any) => {
        this.apptTypes = success;
      });
    this.t07026Service.getAllArrivalStatus()
      .subscribe((success: any) => {
        this.arvlStatus = success;
      });
    this.t07026Service.getAllICD10()
      .subscribe((success: any) => {
        this.icdList = [{ items: success }];
      });
    this.t07026Service.getAllDocArrivalStatus()
      .subscribe((success: any) => {
        this.docArvlStatus = success;
      });
    this.weeks = [{ CODE: '14', NAME: '2 Weeks' }, { CODE: '21', NAME: '3 Weeks' }];
    this.months = [{ CODE: '30', NAME: '1 Month' }, { CODE: '60', NAME: '2 Month' }, { CODE: '90', NAME: '3 Month' }, { CODE: '121', NAME: '4 Month' }, { CODE: '152', NAME: '5 Month' }, { CODE: '182', NAME: '6 Month' }, { CODE: '223', NAME: '7 Month' }, { CODE: '242', NAME: '8 Month' }, { CODE: '273', NAME: '9 Month' }];
    this.commonService.getFormLabel('T07026').subscribe((success: any) => {
      for (let i = 0; i < success.length; i++) {
        try {
          switch (success[i].T_LABEL_NAME) {
            case 'lblWeeks': this.lblWeeks = success[i].T_LABEL_TEXT; break;
            case 'lblMonths': this.lblMonths = success[i].T_LABEL_TEXT; break;
            case 'lblApptDate1': this.lblApptDate1 = success[i].T_LABEL_TEXT; break;
            case 'lblSlot1': this.lblSlot1 = success[i].T_LABEL_TEXT; break;
            case 'lblApptDate2': this.lblApptDate2 = success[i].T_LABEL_TEXT; break;
            case 'lblSlot2': this.lblSlot2 = success[i].T_LABEL_TEXT; break;
            case 'lblGridApptDate': this.lblGridApptDate = success[i].T_LABEL_TEXT; break;
            case 'lblGridApptTime': this.lblGridApptTime = success[i].T_LABEL_TEXT; break;
            case 'lblGridApptDay': this.lblGridApptDay = success[i].T_LABEL_TEXT; break;
            case 'lblGridRule': this.lblGridRule = success[i].T_LABEL_TEXT; break;
          }
        } catch (e) {
          continue;
        }
      }
    });
    this.makeEmpty();
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
    this.data = [];
    this.userForm.get('txtApptDate')?.setValue(new Date().toLocaleDateString("en-GB", { year: 'numeric', month: '2-digit', day: '2-digit' }));
    this.userForm.get('txtApptDay')?.setValue(new Date().toLocaleDateString("en-GB", { weekday: 'long' }));
    setTimeout(() => { document.getElementById('txtDoctorNo')?.focus(); }, 0);
  }

  validateFormFields(formGroup: FormGroup) {
    if (!formGroup.controls['ddlDoctor'].valid) {
      formGroup.controls['ddlDoctor'].markAsDirty();
      this.messageService.add({ severity: 'error', summary: document.getElementById('lblDoctor')?.innerText, detail: this.messages.find(x => x.CODE === 'S0313').TEXT });
    }
    if (!formGroup.controls['ddlClinic'].valid) {
      formGroup.controls['ddlClinic'].markAsDirty();
      this.messageService.add({ severity: 'error', summary: document.getElementById('lblClinic')?.innerText, detail: this.messages.find(x => x.CODE === 'S0313').TEXT });
    }
    if (!formGroup.controls['txtApptDate'].valid) {
      formGroup.controls['txtApptDate'].markAsDirty();
      this.messageService.add({ severity: 'error', summary: document.getElementById('lblApptDate')?.innerText, detail: this.messages.find(x => x.CODE === 'S0313').TEXT });
    }
  }

  onDoctorChanged(value: any) {
    if (value !== null && (this.userForm.get('ddlClinic')?.value === null || this.userForm.get('ddlClinic')?.value === undefined)) {
      this.t07026Service.getAllClinics(value.CODE)
        .subscribe((success: any) => {
          this.clinics = [{ items: success }];
        });
    } else {
      this.t07026Service.getAllClinics('')
        .subscribe((success: any) => {
          this.clinics = [{ items: success }];
        });
    }
  }

  onClinicChanged(value: any) {
    if (value !== null && (this.userForm.get('ddlDoctor')?.value === null || this.userForm.get('ddlDoctor')?.value === undefined)) {
      this.t07026Service.getAllDoctors(value.CODE, this.userForm.get('txtApptDate')?.value === null ? '' : this.userForm.get('txtApptDate')?.value)
        .subscribe((success: any) => {
          this.doctors = [{ items: success }];
        });
    } else {
      this.t07026Service.getAllDoctors('', this.userForm.get('txtApptDate')?.value === null ? '' : this.userForm.get('txtApptDate')?.value)
        .subscribe((success: any) => {
          this.doctors = [{ items: success }];
        });
    }
    if (value !== null && value.RULE !== null)
      this.userForm.get('txtScheduleRule')?.setValue(value.RULE);
    else
      this.userForm.get('txtScheduleRule')?.setValue(null);
  }

  onApptDateBlur() {
    let date = this.userForm.get('txtApptDate')?.value as string;
    let year = parseInt(date.substr(6, 4));
    let month = parseInt(date.substr(3, 2));
    let day = parseInt(date.substr(0, 2));
    const apptDate = month + '/' + day + '/' + year;
    if (new Date(apptDate).getFullYear() != year || new Date(apptDate).getMonth() != month - 1 || new Date(apptDate).getDate() != day) {
      this.messageService.add({ severity: 'warn', summary: 'Error!', detail: 'Invalid Date. Date Format Should Be Like "dd/mm/yyyy"' });
      setTimeout(() => { document.getElementById('txtApptDate')?.focus(); }, 0);
      return;
    }
    this.userForm.get('txtApptDay')?.setValue(new Date(apptDate).toLocaleDateString("en-GB", { weekday: 'long' }));
  }

  onSaveClicked() {
  }

  onClearClicked() {
    this.makeEmpty();
  }

  onNextClicked() {
    this.ngxService.start();
    this.t07026Service.isDocOnVacation(this.userForm.get('ddlDoctor')?.value.CODE, this.userForm.get('txtApptDate')?.value)
      .subscribe((success: any) => {
        if (success === true) {
          this.ngxService.stop();
          this.messageService.add({ severity: 'error', summary: 'Error!', detail: 'Doctor is on Vacation/Holiday' });
        } else {
          this.t07026Service.getAllAppointments(this.userForm.get('ddlDoctor')?.value.CODE, this.userForm.get('ddlClinic')?.value.CODE, this.userForm.get('txtScheduleRule')?.value, this.userForm.get('txtApptDate')?.value)
            .subscribe((success: any) => {
              if (success.length > 0) {
                for (var i = 0; i < success.length; i++) {
                  success[i].T_APPT_TYPE = this.apptTypes.find(x => x.CODE === success[i].T_APPT_TYPE);
                  success[i].T_ARRIVAL_STATUS = this.arvlStatus.find(x => x.CODE === success[i].T_ARRIVAL_STATUS);
                  if (success[i].T_ICD10_MAIN_CODE !== null)
                    success[i].T_ICD10_MAIN_CODE = this.icdList[0].items.find((x: any) => x.CODE === success[i].T_ICD10_MAIN_CODE);
                  if (success[i].T_DOC_ARRIVAL_STATUS !== null)
                    success[i].T_DOC_ARRIVAL_STATUS = this.docArvlStatus.find(x => x.CODE === success[i].T_DOC_ARRIVAL_STATUS);
                }
                this.data = success;
                this.t07026Service.getClinicType(this.userForm.get('ddlClinic')?.value.CODE, this.userForm.get('txtScheduleRule')?.value)
                  .subscribe((success: any) => {
                    this.clinicType = success;
                  });
                this.t07026Service.getFollowupAppointments(this.userForm.get('ddlDoctor')?.value.CODE, this.userForm.get('ddlClinic')?.value.CODE)
                  .subscribe((success: any) => {
                    this.appointmentData = success;
                  });
                this.ngxService.stop();
              } else {
                this.messageService.add({ severity: 'error', summary: 'No Data Found', detail: 'No Data Found' });
                this.ngxService.stop();
              }
            });
        }
      });
  }

  onRowClick(data: any) {
    switch (data.T_X_PAT_TYPE) {
      case '1':
        this.userForm.get('txtEligibility')?.setValue('Eligible');
        this.userForm.get('txtEligible')?.setValue('NSA');
        break;
      case '2':
        this.userForm.get('txtEligibility')?.setValue('Not Eligible');
        this.userForm.get('txtEligible')?.setValue('NSB');
        break;
      case '3':
        this.userForm.get('txtEligibility')?.setValue('Eligible');
        this.userForm.get('txtEligible')?.setValue('S');
        break;
      default:
        this.userForm.get('txtEligibility')?.setValue('Not Entered');
        this.userForm.get('txtEligible')?.setValue(null);
    }
    this.userForm.get('txtApptEntryUser')?.setValue(data.APPT_ENTRY_USER);
    this.userForm.get('txtApptEntryDate')?.setValue(data.APPT_ENTRY_DATE);
    if (data.T_ICD10_MAIN_CODE !== null)
      this.userForm.get('txtICD10Desc')?.setValue(data.T_ICD10_MAIN_CODE.NAME);
  }

  onICD10Changed(value: any) {
    if (value != null)
      this.userForm.get('txtICD10Desc')?.setValue(value.NAME);
    else
      this.userForm.get('txtICD10Desc')?.setValue(null);
  }

  onDocArvlStatusChanged(value: any, index: number) {
    if (value === null)
      this.data[index].T_SEEN_BY_DOC_TIME = null;
    else {
      this.data[index].T_SEEN_BY_DOC_TIME = new Date().toLocaleString('en-GB', { hour: '2-digit', minute: '2-digit' }).replace(':', '');
      if (value.CODE === '5' || value.CODE === '6' || value.CODE === '8') {
        //Open Appointment Data1Canvas/BT1Block
      }
    }
  }

  onPrescriptionClick(rowData: any) {
    //if (rowData.T_ICD10_MAIN_CODE === null) {
    //  this.messageService.add({ severity: 'error', summary: document.getElementById('lblGridICD10')?.innerText, detail: 'ICD Code Cant be null! Please Ack' });
    //  return;
    //}
    //if (rowData.T_DOC_ARRIVAL_STATUS === null) {
    //  this.messageService.add({ severity: 'error', summary: document.getElementById('lblGridDocArrival')?.innerText, detail: 'Doctor arrive status is null, Please arrive, save and continue' });
    //  return;
    //}
    this.router.navigate(['Transaction/T30023', rowData.T_PAT_NO, this.assignDoctor, rowData.T_APPT_NO, this.userForm.get('ddlClinic')?.value.CODE]);
  }

  onLabRequestClick(rowData: any) {
    //if (rowData.T_ICD10_MAIN_CODE === null) {
    //  this.messageService.add({ severity: 'error', summary: document.getElementById('lblGridICD10')?.innerText, detail: 'ICD Code Cant be null! Please Ack' });
    //  return;
    //}
    //if (rowData.T_DOC_ARRIVAL_STATUS === null) {
    //  this.messageService.add({ severity: 'error', summary: document.getElementById('lblGridDocArrival')?.innerText, detail: 'Doctor arrive status is null, Please arrive, save and continue' });
    //  return;
    //}
    this.router.navigate(['Transaction/T13115', rowData.T_PAT_NO, '2', rowData.T_VISIT_NO, '0']);
  }

  onLabInvReportClick(rowData: any) {
    this.router.navigate(['Query/Q13001', rowData.T_PAT_NO ]);
  }

  onRadRequestClick(rowData: any) {
    //if (rowData.T_ICD10_MAIN_CODE === null) {
    //  this.messageService.add({ severity: 'error', summary: document.getElementById('lblGridICD10')?.innerText, detail: 'ICD Code Cant be null! Please Ack' });
    //  return;
    //}
    //if (rowData.T_DOC_ARRIVAL_STATUS === null) {
    //  this.messageService.add({ severity: 'error', summary: document.getElementById('lblGridDocArrival')?.innerText, detail: 'Doctor arrive status is null, Please arrive, save and continue' });
    //  return;
    //}
    this.router.navigate(['Transaction/T11013', rowData.T_PAT_NO, '2', this.userForm.get('ddlClinic')?.value.CODE, ((this.userForm.get('ddlClinic')?.value.T_CLINIC_HOSP_CODE === '2' || this.userForm.get('ddlClinic')?.value.T_CLINIC_HOSP_CODE === '3') ? this.userForm.get('ddlClinic')?.value.T_CLINIC_HOSP_CODE : '1')]);
  }

  onSickLeaveClick(rowData: any) {
    //if (rowData.T_ICD10_MAIN_CODE === null) {
    //  this.messageService.add({ severity: 'error', summary: document.getElementById('lblGridICD10')?.innerText, detail: 'ICD Code Cant be null! Please Ack' });
    //  return;
    //}
    //if (rowData.T_DOC_ARRIVAL_STATUS === null) {
    //  this.messageService.add({ severity: 'error', summary: document.getElementById('lblGridDocArrival')?.innerText, detail: 'Doctor arrive status is null, Please arrive, save and continue' });
    //  return;
    //}
    //this.router.navigate(['Transaction/T06201', { patNo: rowData.T_PAT_NO }]);
    this.router.navigate(['Transaction/T06201', rowData.T_PAT_NO]);
  }

  onApptRequestClick(rowData: any) {
    this.t07026Service.checkUserIsConsultant().subscribe((success: any) => {
        if (success === true) {
          this.router.navigate(['Transaction/T07027', rowData.T_PAT_NO]);
        } else {
          this.messageService.add({ severity: 'error', summary: 'Error!', detail: this.messages.find(x => x.CODE === 'S0614').TEXT });
        }
      });
  }
  onVitalSignClick(rowData: any) {
    this.router.navigate(['Transaction/T06209', rowData.T_PAT_NO ]);
  }
  onAdmissionRequestClick(rowData: any) {

  }

  onAppointmentByDays(value: any) {
    this.t07026Service.getFollowupAppointmentsByDays(value.CODE, this.userForm.get('ddlClinic')?.value.CODE)
      .subscribe((success: any) => {
        if (success.length > 0) {
          this.userForm.get('txtApptDate1')?.setValue(success[0].T_APPT_DATE);
          this.userForm.get('txtSlot1')?.setValue(success[0].ADD_SLOT);
        }
        if (success.length > 1) {
          this.userForm.get('txtApptDate2')?.setValue(success[1].T_APPT_DATE);
          this.userForm.get('txtSlot2')?.setValue(success[1].ADD_SLOT);
        }
        if (success.length === 1)
          this.messageService.add({ severity: 'warn', summary: 'Warning!', detail: 'OPD Diary is not schedule! Please cordinate with OPD Cordination office' });
      });
  }

  onNextFollowupDblClick(rowData: any, rowIndex: any) {
    if (rowData.T_DOC_ARRIVAL_STATUS === null) return;
    this.index = rowIndex;
    if (rowData.T_ARRIVAL_STATUS.CODE === '1' && (this.clinicType === '2' || this.clinicType === '3') && (rowData.T_DOC_ARRIVAL_STATUS.CODE === '4' || rowData.T_DOC_ARRIVAL_STATUS.CODE === '6' || rowData.T_DOC_ARRIVAL_STATUS.CODE === '8'))
      this.showAppointment = true;
  }

  onAppointmentSelect(rowData: any) {
    this.data[this.index].T_NXT_FLWUP = rowData.APPT_DATE_G;
    this.data[this.index].T_FLWUP_TIME = rowData.T_APPT_TIME;
    this.showAppointment = false;
  }

  onWeeksChanged(value: any) {
    this.userForm.get('txtApptDate1')?.setValue(null);
    this.userForm.get('txtSlot1')?.setValue(null);
    this.userForm.get('txtApptDate2')?.setValue(null);
    this.userForm.get('txtSlot2')?.setValue(null);
    if (value !== null) {
      this.userForm.get('ddlMonths')?.setValue(null);
      this.onAppointmentByDays(value);
    }
  }

  onMonthsChanged(value: any) {
    this.userForm.get('txtApptDate1')?.setValue(null);
    this.userForm.get('txtSlot1')?.setValue(null);
    this.userForm.get('txtApptDate2')?.setValue(null);
    this.userForm.get('txtSlot2')?.setValue(null);
    if (value !== null) {
      this.userForm.get('ddlWeeks')?.setValue(null);
      this.onAppointmentByDays(value);
    }
  }

  onAddSlot1Click() {

  }

  onAddSlot2Click() {

  }

  onCClicked() {

  }
}
