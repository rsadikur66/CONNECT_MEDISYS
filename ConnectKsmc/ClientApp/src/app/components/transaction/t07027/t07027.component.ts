import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { debug } from 'console';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { MessageService } from 'primeng/api';
import { retry } from 'rxjs/operators';
import { CommonService } from '../../../services/common.service';
import { T07027Service } from '../../../services/transaction/t07027.service';
import {ActivatedRoute } from '@angular/router';


@Component({
  selector: 't07027',
  templateUrl: 't07027.component.html',
  providers: [T07027Service]
})
export class T07027Component implements OnInit {
  ClinicSpcltypopupDisplay: boolean = false;
  ClinicPopupDisplay: boolean = false;
  ClinicDocPopupDisplay: boolean = false;
  obj: any = new Object();
  messages: any[] = [];
  
  clinicSpecialityList: any[] = [];
  clinicList: any[] = [];
  clinicDocList: any[] = [];
  
  SPCLTYListTemp: any[] = [];
  ClinicListTemp: any[] = [];
  ClinicDocListTemp: any[] = [];
  
  AppDateDropDownList: any[] = [];
  canSave = false;
  canUpdate = false;
  canDelete = false;
  canQuery = false;
  userLang = '';
  search: any;
  

  userForm = new FormGroup({
    'txtPatNo': new FormControl(),
    'txtPatName': new FormControl(),
    'txtAge': new FormControl(),
    'txtAgeM': new FormControl(),
    'txtGenderDesc': new FormControl(),
    'txtPatNameArb': new FormControl(),
    'txtMrtlSt': new FormControl(),
    'txtId': new FormControl(),
    'txtNationality': new FormControl(),
    'txtMobileNo': new FormControl(),
    'txtReqNo': new FormControl(),
    'txtReqTime': new FormControl(),
    'txtReqDate': new FormControl(),
    
    'txtApptDateH': new FormControl(),
    'txtDay': new FormControl(),
    'txtClncSpclty': new FormControl(),
    'txtClinicSpcltyName': new FormControl(),
    'txtClinicCode': new FormControl(),
    'txtClinicName': new FormControl(),
    'txtClinicDoc': new FormControl(),
    'txtClinicDocName': new FormControl(),
    'ddlApptDate': new FormControl()
  });

  constructor(public sanitizer: DomSanitizer, private commonService: CommonService, private route: ActivatedRoute,private t07027Service: T07027Service, private messageService: MessageService, private ngxService: NgxUiLoaderService) { }

  ngOnInit(): void {
    this.ngxService.start();
    this.userLang = localStorage.getItem('userLang') as string;
    this.commonService.getAllMessage(`
        'S0313'/*Generic required*/,
        'S0349'/*No record found*/,
        'S0360'/*Check permission*/,
        'N0024'/*Data saved*/
      `).subscribe((success: any) => {
      this.messages = success;
      });
    let patNo: any = this.route.snapshot.paramMap.get('patNo');
    this.t07027Service.getPatInfor(patNo).subscribe((success: any) => {
      this.obj.T_PAT_NO = success[0].T_PAT_NO;
      this.obj.T_PAT_ENG_NAME = success[0].ENG_NAME;
      this.obj.T_PAT_ARB_NAME = success[0].ARB_NAME;
      this.obj.T_PAT_GENDER = success[0].GENDER_DSCRPTN;
      this.obj.T_PAT_MRTLST = success[0].MRTL_STATUS_DSCRPTN;
      this.obj.T_PAT_NATIONALITY = success[0].NTNLTY_DSCRPTN;
      this.obj.T_PAT_NTNLTY_ID = success[0].T_NTNLTY_ID;
      this.obj.T_PAT_MOBILE = success[0].T_MOBILE_NO;
      this.obj.T_AGE_MONTHS = success[0].AGE_M;
      this.obj.T_AGE_YEARS = success[0].AGE_Y;
    });
    this.t07027Service.getClinicSpcltyList().subscribe((success: any) => { this.clinicSpecialityList = success; this.SPCLTYListTemp = success; });
    this.t07027Service.getClinicDocList().subscribe((success: any) => { this.clinicDocList = success; this.ClinicDocListTemp = success; });
    this.t07027Service.getAllAppDates().subscribe((success: any) => { this.AppDateDropDownList = success; });
    this.ngxService.stop();
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
  }
  onNewClicked(){
    this.onClearClicked();
  }
  onPrintClicked(){
    let T_REQUEST_NO = this.userForm.get("txtReqNo")?.value;
    if (T_REQUEST_NO == null || T_REQUEST_NO == undefined) {
      T_REQUEST_NO = "";
    }
     window.open("./api/t07027/getReport?T_REQUEST_NO=" + T_REQUEST_NO, "popup", "location=1, status=1, scrollbars=1");
  }
  onNextClicked() {
    let PatNumber = this.userForm.get("txtPatNo")?.value;
    this.t07027Service.getPatReqData(PatNumber).subscribe((success: any) => { 
      
      if (success.length > 0) {
        this.userForm.get("txtReqNo")?.setValue(success[0].T_REQUEST_NO);
        this.userForm.get("txtReqTime")?.setValue(success[0].T_REQUEST_TIME);
        this.userForm.get("txtReqDate")?.setValue(success[0].T_REQUEST_DATE);

        this.userForm.get('ddlApptDate')?.setValue(this.AppDateDropDownList.find(x => x.CODE === success.T_APPT_DATE));
        this.userForm.get("txtApptDateH")?.setValue(success[0].DOCNAME);
        this.userForm.get("txtDay")?.setValue(success[0].T_CLINIC_NAME_LANG2);

        this.userForm.get("txtClncSpclty")?.setValue(success[0].T_CLINIC_SPCLTY);
        this.userForm.get("txtClinicSpcltyName")?.setValue(success[0].SPCLTY_DESC);

        this.userForm.get("txtClinicCode")?.setValue(success[0].T_CLINIC_CODE);
        this.userForm.get("txtClinicName")?.setValue(success[0].CLINIC_DESC);

        this.userForm.get("txtClinicDoc")?.setValue(success[0].T_CLINIC_DOC_CODE);
        this.userForm.get("txtClinicDocName")?.setValue(success[0].CLINIC_DOC_NAME);
        console.log(this.canSave);
      } else {
        this.messageService.add({ severity: 'error', summary: 'Alert!!!', detail: 'No Data Found.' });
      }
      
    });
  }

  dblClinicSpclty() {    
    this.search = '';
    this.filterGlobal();
    this.ClinicSpcltypopupDisplay = true;
  }
  dblClinicDoc() {
    this.search = '';
    this.filterGlobalClinicDoc();
    this.ClinicDocPopupDisplay = true;
  }
  dblClinic() {
    let spcltyCode = this.userForm.get("txtClncSpclty")?.value;
    this.t07027Service.getClinicList(spcltyCode).subscribe((success: any) => { this.clinicList = success; this.ClinicListTemp = success; });
    this.search = '';
    this.filterGlobalClinic();
    this.ClinicPopupDisplay = true;
  }

  onSelectSPCLTYPopup(y: any) {
    let requestNo = this.userForm.get("txtReqNo")?.value;
    if (requestNo == null) {
      this.userForm.get("txtClncSpclty")?.setValue(y.T_SPCLTY_CODE);
    this.userForm.get("txtClinicSpcltyName")?.setValue(y.T_SPCLTY_NAME);
    this.ClinicSpcltypopupDisplay = false;
    } else {      
    this.ClinicSpcltypopupDisplay = false;
    return;
    }
    
  }
  onSelectDocPopup(y: any) {
    if (this.userForm.get("txtReqNo")?.value == null) {
      this.userForm.get("txtClinicDoc")?.setValue(y.T_DOC_CODE);
    this.userForm.get("txtClinicDocName")?.setValue(y.DOC_NAME);
    this.ClinicDocPopupDisplay = false; 
    } else {
    this.ClinicDocPopupDisplay = false; 
    return;
    }
       
  }
  onSelectClinicPopup(y: any) {
    if (this.userForm.get("txtReqNo")?.value == null) {
      if (y.T_CLINIC_CODE != null) {
        let spcltyCode = this.userForm.get("txtClncSpclty")?.value;
        this.userForm.get("txtClinicCode")?.setValue(y.T_CLINIC_CODE);
        this.t07027Service.getSpcltyAndDocByClnCode(y.T_CLINIC_CODE, spcltyCode).subscribe((success: any) => {
          this.userForm.get("txtClncSpclty")?.setValue(success[0].T_CLINIC_SPCLTY_CODE);
          this.userForm.get("txtClinicSpcltyName")?.setValue(success[0].CLINIC_SPCLTY_DESC);
          this.userForm.get("txtClinicName")?.setValue(success[0].T_CLINIC_NAME_LANG2);
          this.userForm.get("txtClinicDoc")?.setValue(success[0].T_CLINIC_DOC_CODE);
          this.userForm.get("txtClinicDocName")?.setValue(success[0].DOCNAME);
        });
      }    
      this.ClinicPopupDisplay = false;
    } else {
      this.ClinicPopupDisplay = false;
      return;
    }
    
  }

  filterGlobal() {
    this.clinicSpecialityList = this.SPCLTYListTemp.filter((i: { T_SPCLTY_CODE: string; T_SPCLTY_NAME: string; }) => i.T_SPCLTY_CODE.indexOf(this.search) >= 0 || i.T_SPCLTY_NAME.indexOf(this.search.toUpperCase()) >= 0);
  }
  filterGlobalClinicDoc() {
    this.clinicDocList = this.ClinicDocListTemp.filter((i: { T_DOC_CODE: string; DOC_NAME: string; }) => i.T_DOC_CODE.indexOf(this.search) >= 0 || i.DOC_NAME.indexOf(this.search.toUpperCase()) >= 0);
  }
  filterGlobalClinic() {
    this.clinicList = this.ClinicListTemp.filter((i: { T_CLINIC_CODE: string; CLINIC_NAME: string; }) => i.T_CLINIC_CODE.indexOf(this.search) >= 0 || i.CLINIC_NAME.indexOf(this.search.toUpperCase()) >= 0);
  }


  onSaveClicked() {
    if (this.canSave) {
      debugger;
      let patientNo = this.userForm.get("txtPatNo")?.value;
      let clinicCode = this.userForm.get("txtClinicCode")?.value;
      let appointmentDate = this.userForm.get("ddlApptDate")?.value;
      if (patientNo != "" && clinicCode != null && appointmentDate != "") {
        this.t07027Service.generateRequestNo().subscribe((success:any) => {
          this.userForm.get("txtReqNo")?.setValue(success.REQ_NO);
        this.userForm.get("txtReqDate")?.setValue(success.REQUEST_DATE);
        this.userForm.get("txtReqTime")?.setValue(success.REQUEST_TIMEE);

        let spcltyCode = this.userForm.get("txtClncSpclty")?.value;        
        let clncDocCode = this.userForm.get("txtClinicDoc")?.value;
        this.t07027Service.saveData(patientNo,appointmentDate.T_TIME_CODE, spcltyCode,clinicCode,clncDocCode,this.userForm.get("txtReqNo")?.value,this.userForm.get("txtReqTime")?.value)
        .subscribe(() => {         
          this.ngxService.stop();
          this.messageService.add({ severity: 'success', summary: 'Success!', detail: this.messages.find(x => x.CODE === 'N0024').TEXT });          
        },
          error => {
            this.ngxService.stop();
            if (error.status === 400)
              this.messageService.add({ severity: 'error', summary: 'Error!', detail: error.error.msg });
            else
              console.log(error);
          });
        })
      } else {
        this.messageService.add({ severity: 'error', summary: 'Alert!!!', detail: 'Patient No or Clinic or Appointment date cant be empty' });
      }
    } else {
      this.messageService.add({ severity: 'error', summary: 'No permission!', detail: this.messages.find(x => x.CODE === 'N0024').TEXT });
    }
  }

  //sadik
  onClearClicked() {
    this.userForm.get("txtClncSpclty")?.reset();
    this.userForm.get("txtClinicSpcltyName")?.reset();
    this.userForm.get("txtClinicCode")?.reset();
    this.userForm.get("txtClinicName")?.reset();
    this.userForm.get("txtClinicDoc")?.reset();
    this.userForm.get("txtClinicDocName")?.reset();
    this.userForm.get("ddlApptDate")?.reset();
  }
  //sadik code
}
