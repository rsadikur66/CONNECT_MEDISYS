import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { DomSanitizer, Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { LoginService } from '../../services/login.service';
import { CommonService } from '../../services/common.service';

@Component({
  selector: 'common',
  templateUrl: 'common.component.html'
})
export class CommonComponent implements OnInit {
  @Input() formCode = '';
  @Input() printTitle = 'Print';
  @Input() print1Title = '';
  @Input() print2Title = '';
  @Output() languageChanged = new EventEmitter();
  @Output() permissionLoaded = new EventEmitter();

  //data: any[] = [];
  //labelMenu: any[] = [];
  //display = false;
  @Input() isTransactionVisible = true;
  @Output() transactionClicked = new EventEmitter();
  @Input() isQueryVisible = true;
  @Output() queryClicked = new EventEmitter();
  @Input() isReportVisible = true;
  @Output() reportClicked = new EventEmitter();
  @Input() isSetupVisible = true;
  @Output() setupClicked = new EventEmitter();
  @Input() isSecurityVisible = true;
  @Output() securityClicked = new EventEmitter();

  @Input() isNewVisible = false;
  @Output() newClicked = new EventEmitter();
  @Input() isSaveVisible = true;
  @Output() saveClicked = new EventEmitter();
  @Input() isNextVisible = false;
  @Output() nextClicked = new EventEmitter();
  @Input() isDeleteVisible = false;
  @Output() deleteClicked = new EventEmitter();
  @Input() isClearVisible = true;
  @Output() clearClicked = new EventEmitter();
  @Input() isEnterVisible = false;
  @Output() enterClicked = new EventEmitter();
  @Input() isExecuteVisible = false;
  @Output() executeClicked = new EventEmitter();
  @Input() isCancelVisible = false;
  @Output() cancelClicked = new EventEmitter();
  @Input() isPrintVisible = false;
  @Output() printClicked = new EventEmitter();
  @Input() isPrint1Visible = false;
  @Output() print1Clicked = new EventEmitter();
  @Input() isPrint2Visible = false;
  @Output() print2Clicked = new EventEmitter();

  css = '';
  basePath = '';
  language = '';
  userName = '';
  userRole = '';
  userLang = '';
  formInfo = '';
  title = '';
  siteNameArb = '';
  siteNameEng = '';
  transaction = '';
  query = '';
  report = '';
  setup = '';
  security = '';
  logout = '';

  todayG: string = new Date().toLocaleDateString("en-GB", { year: 'numeric', month: '2-digit', day: '2-digit' });
  todayH: string = new Date().toLocaleDateString("ar-SA", { year: 'numeric', month: '2-digit', day: '2-digit' });

  constructor(public sanitizer: DomSanitizer, private commonService: CommonService, public loginService: LoginService, private titleService: Title, private router: Router, private ngxService: NgxUiLoaderService) {
    loginService.checkIdle();
  }

  ngOnInit(): void {
    //this.labelMenu = [{
    //  label: 'Change Label', command: () => {
    //    this.commonService.getFormLabelForEdit(this.formCode)
    //      .subscribe((success: any) => {
    //        this.data = success;
    //        this.display = true;
    //      },
    //        error => {
    //          console.log(error.error.msg);
    //        });
    //  }
    //}];
    this.basePath = localStorage.getItem('basePath') as string;
    this.userName = localStorage.getItem('userName') as string;
    this.userRole = localStorage.getItem('userRole') as string;
    this.userLang = localStorage.getItem('userLang') as string;
    this.siteNameArb = localStorage.getItem('siteNameArb') as string;
    this.siteNameEng = localStorage.getItem('siteNameEng') as string;
    this.changeLanguage();

    document.querySelector('body')?.addEventListener('keydown', function (event: KeyboardEvent) {
      if (event.keyCode === 13) {
        const element = event.target as HTMLInputElement;
        if (element.id.startsWith('ddl')) return;
        const form = element.form;
        const length = form?.elements.length;
        if (length) {
          for (let i = 0; i < length; i++) {
            if (element === form?.elements[i])
              if (i + 1 < length)
                document.getElementById(form.elements[i + 1].id)?.focus();
          }
        }
      }
    });

    //function getElementIndex(element: HTMLInputElement, length: number, form: HTMLFormElement) {
    //  for (let i = 0; i < length; i++) {
    //    if (element === form?.elements[i])
    //      return i;
    //  }
    //  return -1;
    //}

    //document.querySelector('body')?.addEventListener('keydown', function (event: KeyboardEvent) {
    //  if (event.keyCode === 13) {
    //    const element = event.target as HTMLInputElement;
    //    const form = element.form;
    //    if (form) {
    //      const length = form.elements.length;
    //      if (length) {
    //        const index = getElementIndex(element, length, form);

    //      }
    //    }
    //  }
    //});
  }

  changeLanguage() {
    this.ngxService.start();

    this.css = this.userLang === '1' ? 'css/app.ar.css' : 'css/app.en.css';
    //this.titleService.setTitle(this.userLang === '1' ? this.siteNameArb : this.siteNameEng);
    this.titleService.setTitle('ConnectKsmc');
    this.language = this.userLang === "1" ? "English EN" : "AR العربية";
    this.title = this.userLang === "1" ? "Hospital Information System" : "Hospital Information System";

    this.commonService.getFormInfo(this.formCode).subscribe((success: any) => {
      this.formInfo = this.userLang === "1" ? success.T_FORM_TITLE + " - " + success.T_FORM_CODE : success.T_FORM_CODE + " - " + success.T_FORM_TITLE;
      this.ngxService.stop();
    },
      error => {
        this.ngxService.stop();
        console.log(error.error.msg);
      });

    this.transaction = this.userLang === '1' ? 'ادخال البيانات' : 'Transaction';
    this.query = this.userLang === '1' ? 'الاستعلام' : 'Query';
    this.report = this.userLang === '1' ? 'تقارير' : 'Report';
    this.setup = this.userLang === '1' ? 'إعداد البيانات' : 'Setup';
    this.security = this.userLang === '1' ? 'الأمان' : 'Security';
    this.logout = this.userLang === '1' ? 'تسجيل خروج' : 'Logout';

    if (!this.formCode.startsWith('M')) {
      this.ngxService.start();
      this.commonService.getFormPermission(this.formCode)
        .subscribe((success: any) => {
          this.permissionLoaded.emit(success);
        },
          error => {
            console.log(error.error.msg);
          });
      this.commonService.getFormLabel(this.formCode)
        .subscribe((success: any) => {
          for (let i = 0; i < success.length; i++) {
            try {
              const label = document.getElementById(success[i].T_LABEL_NAME);
              if (label !== null) {
                label.innerHTML = success[i].T_LABEL_TEXT;
                const parent = label.parentElement;
                if (parent !== null) {
                  if (!parent.classList.contains('center-label') && !parent.classList.contains('dummy'))
                    if (parent.tagName !== 'TH')
                      parent.classList.add('align-label');
                }
              }
            } catch (e) {
              continue;
            }
          }
          this.ngxService.stop();
        },
          error => {
            console.log(error.error.msg);
            this.ngxService.stop();
          });
    }
    this.languageChanged.emit();
  }

  onLanguageClick() {
    this.userLang = this.userLang === '1' ? '2' : '1';
    localStorage.setItem('userLang', this.userLang);
    this.changeLanguage();
  }

  onBtnTransactionClick() {
    this.transactionClicked.emit();
  }

  onBtnQueryClick() {
    this.queryClicked.emit();
  }

  onBtnReportClick() {
    this.reportClicked.emit();
  }

  onBtnSetupClick() {
    this.setupClicked.emit();
  }

  onBtnSecurityClick() {
    this.securityClicked.emit();
  }

  onBtnNewClick() {
    this.newClicked.emit();
  }

  onBtnSaveClick() {
    this.saveClicked.emit();
  }

  onBtnNextClick() {
    this.nextClicked.emit();
  }

  onBtnDeleteClick() {
    this.deleteClicked.emit();
  }

  onBtnClearClick() {
    this.clearClicked.emit();
  }

  onBtnEnterClick() {
    this.isEnterVisible = false;
    this.isExecuteVisible = true;
    this.isCancelVisible = true;
    this.enterClicked.emit();
  }

  onBtnExecuteClick() {
    this.executeClicked.emit();
  }

  onBtnCancelClick() {
    this.isEnterVisible = true;
    this.isExecuteVisible = false;
    this.isCancelVisible = false;
    this.cancelClicked.emit();
  }

  onBtnPrintClick() {
    this.printClicked.emit();
  }

  onBtnPrint1Click() {
    this.print1Clicked.emit();
  }

  onBtnPrint2Click() {
    this.print2Clicked.emit();
  }

  onBtnBackClick() {
    const history = JSON.parse(localStorage.getItem('pageHistory') as string);
    const url = history.pop();
    let urlParam = '';
    if (location.pathname === this.basePath + '/Transaction/T12214' && url === '/Transaction/T12201') {
      const txtDonarId = document.getElementById('txtDonarId') as HTMLInputElement;
      if (txtDonarId) urlParam = txtDonarId.value;
    }
    localStorage.setItem('pageHistory', JSON.stringify(history));
    if (urlParam !== '')
      this.router.navigate([url, urlParam]);
    else
      this.router.navigate([url]);
  }

  onBtnLogoutClick() {
    this.loginService.logoutUser()
      .subscribe(() => {
        localStorage.clear();
        this.titleService.setTitle('ConnectKsmc');
        this.router.navigate(['Login']);
      },
        error => {
          if (error.status === 400) {
            console.log(error.error.msg);
            localStorage.clear();
            this.titleService.setTitle('ConnectKsmc');
            this.router.navigate(['Login']);
          }
          else
            console.log('Logout failed');
        });
  }
}
