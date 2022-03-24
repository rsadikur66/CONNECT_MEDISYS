import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { LoginService } from '../../../services/login.service';
import { CommonService } from '../../../services/common.service';
import { Q13001Service } from '../../../services/query/q13001.service';
import { NgxUiLoaderService } from 'ngx-ui-loader';

@Component({
  selector: 'q13001',
  templateUrl: 'q13001.component.html',
  styleUrls: ['q13001.component.css'],
  providers: [Q13001Service]
})
export class Q13001Component implements OnInit {
  formCode: string = 'Q13001';
  loading: boolean = false;
  userform: FormGroup;
  q13001: any;
  requestList!: any[];
  requestDetailList!: any[];
  lblPatinformation!: string;
  lblListOfPatient!: string;
  lblGender!: string;
  lblYears!: string;
  lblMonth!: string;
  lblNationality!: string;
  lblRoom!: string;
  lblBedNo!: string;
  lblAdmitDate!: string;
  lblProtocol!: string;

  canSave!: boolean;
  canUpdate!: boolean;
  canDelete!: boolean;
  canQuery!: boolean;
  userLang!: string;
  siteCode!: string;
  version!: string;
  nationality_code: string = '';
  displayPatientPopup: boolean = false;
  patientDetails: any = [];
  labels: any = [];
  requestSelectedData: any;
  labList: any[] = [];
  constructor(private loginService: LoginService, private commonService: CommonService, private q13001Service: Q13001Service, private route: ActivatedRoute, private router: Router, private formBuilder: FormBuilder, private messageService: MessageService, private ngxService: NgxUiLoaderService) {
    this.userform = this.formBuilder.group({
      'txtPatientNo': new FormControl(),
      'txtNationality': new FormControl(),
      'txtGender': new FormControl(),
      'txtPatientName': new FormControl(),
      'txtPatientArb': new FormControl(),
      'txtYear': new FormControl(),
      'txtRequestNo': new FormControl(),
      'ddlLab': new FormControl(),
      'txtMonth': new FormControl()
    });
    loginService.checkIdle();
  }
  ngOnInit(): void {
    this.loading = true;
    this.userLang = localStorage.getItem('userLang') as string;
    var patNo = this.route.snapshot.paramMap.get('patNo');
    if (patNo != null) {
      this.userform.get('txtPatientNo')!.setValue(patNo);
      this.onPatNoBlur(patNo);
    }
    this.getLabInfo();
  }
  setVersion(val: string) {
    this.version = val;
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
    this.userform.reset();
  }
  onBtnNextClick() {
    this.requestList = [];
    var patNo = this.userform.get('txtPatientNo')!.value;
    var lab = this.userform.get('ddlLab')!.value == null ? '' : this.userform.get('ddlLab')!.value.CODE;
    if (patNo != '' && patNo != null && patNo != undefined) {
      this.q13001Service.getRequestInfo(patNo, lab).subscribe((success: any) => {
        this.requestList = success;
        if (success.length > 0) {
          this.requestSelectedData = this.requestList[0];
          this.onRequestSelect(this.requestList[0]);
        }
        else {
          this.messageService.add({ severity: 'info', summary: 'Info!', detail: 'No data found' });
        }
        });
    } else {
      this.messageService.add({ severity: 'error', summary: 'Error!', detail: 'Please Select Patient No' });
    }
  }
  onRequestSelect(rowData: any) {
    this.requestDetailList = [];
    if (rowData.T_REQUEST_NO != null && rowData.T_REQUEST_NO != undefined) {
      this.ngxService.start();
      this.q13001Service.getRequestDetail(rowData.T_REQUEST_NO, rowData.T_WS_CODE).subscribe((success: any) => {
        this.ngxService.stop();
        this.requestDetailList = success;
      });
    }
  }
  onLabChange() {
    this.requestSelectedData = [];
    this.requestList = [];
    this.requestDetailList = [];
  }
  getLabInfo() {
    this.labList = [];
    this.q13001Service.getLabInfo().subscribe((success: any) => { this.labList = success; });
  }
  onClearClicked() {
    this.makeEmpty();
    this.requestList = [];
  }
  onPrintClicked() {
    var T_WS_CODE: any; var t_analysis_code: any;
    var REP_ID;
    //if (T_WS_CODE == '02') {      REP_ID = 'R13021';    }
    //else if (T_WS_CODE = '11') { REP_ID = 'R13111' }
    //else if (T_WS_CODE = '10') {
    //  if (t_analysis_code >= '1005' && t_analysis_code <= '1037') { REP_ID = 'R130103'; }
    //  else if (t_analysis_code >= '1038' && t_analysis_code <= '1054') { REP_ID = 'R130104'; }
    //  else if (t_analysis_code >= '10039' && t_analysis_code <= '10077') { REP_ID = 'R13010'; }
    //  else if (t_analysis_code >= '10001' && t_analysis_code <= '10038') { REP_ID = 'R13011'; }
    //}
    //else {
    //  REP_ID = '';
      
    //}
    if (this.requestSelectedData == null || this.requestSelectedData == undefined || this.requestSelectedData == '')
      return;
    window.open("./api/q13001/getReport?reqInfo=" + this.requestSelectedData + "&reqNo=" + this.requestSelectedData.T_REQUEST_NO + "&labNo=" + this.requestSelectedData.T_LAB_NO + "&reportID=" + REP_ID, "popup", "location=1, status=1, scrollbars=1");
  }
  onBtnNewClick() {
    this.makeEmpty();
    this.requestList = [];
  }
  onPatNoBlur(patNo: any) {
    if (patNo != null) {
      this.ngxService.start();
      this.q13001Service.getPatInfo(patNo).subscribe((success: any) => {
        this.ngxService.stop();
        if (success) {
          this.userform.get('txtPatientName')!.setValue(success.T_PAT_NAME);
          this.userform.get('txtPatientArb')!.setValue(success.T_PAT_NAME_ARB);
          this.userform.get('txtYear')!.setValue(success.YEARS);
          this.userform.get('txtMonth')!.setValue(success.MONTHS);
          this.nationality_code = success.T_NTNLTY_CODE;
        } else {
          var patNo: string = this.userform.get('txtPatientNo')!.value;
          this.makeEmpty();
          this.userform.get('txtPatientNo')!.setValue(patNo);
          this.messageService.add({ severity: 'info', summary: 'Info!', detail: 'No data found' });
        }
      },
        error => { this.ngxService.stop(); console.log(error); });
    } else
      this.makeEmpty();
  }

  getEventValue($event: any): string {
    return $event.target.value;
  }

  //sadik
  onPrintMarriage() {
    /*let T_REQUEST_NO = '0010783840';*/
    var T_REQUEST_NO = this.userform.get('txtRequestNo')!.value;
    this.q13001Service.getCountX(T_REQUEST_NO).subscribe((success: any) => {      
      if (success.CntX >= 2) {
        if (this.nationality_code == '01') {
          window.open('./api/t06029/getReportT13166?T_REQUEST_NO=' + T_REQUEST_NO,'popup','location=1, status=1, scrollbars=1');
        } else {
          window.open('./api/q13001/getReportT13166A?T_REQUEST_NO=' + T_REQUEST_NO, 'popup', 'location=1, status=1, scrollbars=1');
        }        
      }
    });    
  }
  onPrintMarriageNS() {
    /*let T_REQUEST_NO = '0010783840';*/
    var T_REQUEST_NO = this.userform.get('txtRequestNo')!.value;
    this.q13001Service.getCountX(T_REQUEST_NO).subscribe((success: any) => {      
      if (success.CntX >= 2) {
          window.open('./api/q13001/getReportT13166A?T_REQUEST_NO=' + T_REQUEST_NO, 'popup', 'location=1, status=1, scrollbars=1');
      }
    });    
  }

  //sadik end
}
