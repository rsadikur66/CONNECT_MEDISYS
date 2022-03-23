import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { MessageService } from 'primeng/api';
import { CommonService } from '../../services/common.service';
import { LoginService } from '../../services/login.service';
import { MenuService } from '../../services/menu.service';

@Component({
  selector: 'm35001',
  templateUrl: 'm35001.component.html'
})
export class M35001Component implements OnInit {
  messages: any[] = [];
  basePath = '';
  navdir = '';
  userLang = '';
  menuOption = '';
  option = '';
  transaction = '';
  query = '';
  report = '';
  setup = '';
  security = '';
  links: any[] = [];

  constructor(public sanitizer: DomSanitizer, private menuService: MenuService, private commonService: CommonService, public loginService: LoginService, private router: Router, private messageService: MessageService, private ngxService: NgxUiLoaderService) {
    loginService.checkIdle();
  }

  ngOnInit(): void {
    this.ngxService.start();
    this.basePath = localStorage.getItem('basePath') as string;
    this.commonService.getAllMessage(`
        'S0360'/*Check permission*/
      `).subscribe((success: any) => {
      this.messages = success;
    });
    this.changeLanguage();
  }

  onLanguageChanged() {
    this.userLang = localStorage.getItem('userLang') as string;
    this.changeLanguage();
  }

  changeLanguage() {
    this.navdir = this.userLang === '1' ? 'images/navdirar.png' : 'images/navdiren.png';
    this.transaction = this.userLang === '1' ? 'ادخال البيانات' : 'Transaction';
    this.query = this.userLang === '1' ? 'الاستعلام' : 'Query';
    this.report = this.userLang === '1' ? 'تقارير' : 'Report';
    this.setup = this.userLang === '1' ? 'إعداد البيانات' : 'Setup';
    this.security = this.userLang === '1' ? 'الأمان' : 'Security';

    this.option = localStorage.getItem('option') as string;
    if (this.option === null) this.option = this.transaction;
    this.menuOption = localStorage.getItem('menuOption') as string;
    if (this.menuOption === null) this.menuOption = 'Transaction';

    switch (this.menuOption) {
      case 'Transaction': this.option = this.transaction; break;
      case 'Query': this.option = this.query; break;
      case 'Report': this.option = this.report; break;
      case 'Setup': this.option = this.setup; break;
      case 'Security': this.option = this.security; break;
    }

    this.refreshMenu();
  }

  refreshMenu() {
    this.menuService.getMenu(this.menuOption, this.userLang).subscribe((success: any) => {
      this.links = success;
      this.ngxService.stop();
    },
      error => {
        console.log(error.error.msg);
        this.ngxService.stop();
      });
  }

  onTransactionClicked() {
    this.ngxService.start();
    this.menuOption = 'Transaction';
    localStorage.setItem('menuOption', this.menuOption);
    this.option = this.transaction;
    localStorage.setItem('option', this.option);
    this.refreshMenu();
  }

  onQueryClicked() {
    this.ngxService.start();
    this.menuOption = 'Query';
    localStorage.setItem('menuOption', this.menuOption);
    this.option = this.query;
    localStorage.setItem('option', this.option);
    this.refreshMenu();
  }

  onReportClicked() {
    this.ngxService.start();
    this.menuOption = 'Report';
    localStorage.setItem('menuOption', this.menuOption);
    this.option = this.report;
    localStorage.setItem('option', this.option);
    this.refreshMenu();
  }

  onSetupClicked() {
    this.ngxService.start();
    this.menuOption = 'Setup';
    localStorage.setItem('menuOption', this.menuOption);
    this.option = this.setup;
    localStorage.setItem('option', this.option);
    this.refreshMenu();
  }

  onSecurityClicked() {
    this.ngxService.start();
    this.menuOption = 'Security';
    localStorage.setItem('menuOption', this.menuOption);
    this.option = this.security;
    localStorage.setItem('option', this.option);
    this.refreshMenu();
  }

  onLinkClick(obj: any) {
    obj.preventDefault();
    const route = obj.target.attributes.href.value.replace('..' + this.basePath + '/', '');
    this.commonService.getFormPermission(route.substring(route.indexOf('/') + 1))
      .subscribe((success: any) => {
        if (success.canOpen) {
          const pageHistory = localStorage.getItem('pageHistory');
          if (pageHistory !== null) {
            const history = JSON.parse(pageHistory);
            history.push(location.pathname.replace(this.basePath, ''));
            localStorage.setItem('pageHistory', JSON.stringify(history))
          }
          else {
            const history = Array();
            history.push(location.pathname.replace(this.basePath, ''));
            localStorage.setItem('pageHistory', JSON.stringify(history))
          }
          this.router.navigate([route])
            .catch(() => {
              this.messageService.add({ severity: 'error', summary: 'Under Construction!', detail: 'This link is under construction' });
            });
        }
        else
          this.messageService.add({ severity: 'error', summary: 'No Permission!', detail: this.messages.find(x => x.CODE === 'S0360').TEXT });
      },
        error => {
          if (error.status === 403)
            this.messageService.add({ severity: 'error', summary: 'No Permission!', detail: this.messages.find(x => x.CODE === 'S0360').TEXT });
          else
            console.log(error);
        });
  }
}
