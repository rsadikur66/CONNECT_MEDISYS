import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { MessageService } from 'primeng/api';
import { LoginService } from '../../services/login.service';
import { NgxUiLoaderService } from 'ngx-ui-loader';

@Component({
  selector: 'login',
  templateUrl: 'login.component.html',
  styleUrls: ['login.component.css']
})
export class LoginComponent implements OnInit {
  @ViewChild("txtUserName") txtUserName!: ElementRef;
  @ViewChild("txtPassword") txtPassword!: ElementRef;

  focusedItem = 'UserName';
  userName = '';
  password = '';
  showImage = false;

  constructor(public loginService: LoginService, private router: Router, private titleService: Title, private messageService: MessageService, private ngxService: NgxUiLoaderService) {
  }

  ngOnInit(): void {
    this.titleService.setTitle('ConnectKsmc');
  }

  private checkUser() {
    this.ngxService.start();
    this.loginService.loginUser(this.userName, this.password)
      .subscribe((success: any) => {
        localStorage.setItem('basePath', success.basePath);
        localStorage.setItem('empCode', success.empCode);
        localStorage.setItem('userName', success.userName);
        localStorage.setItem('userRole', success.userRole);
        localStorage.setItem('userLang', success.userLang);
        //localStorage.setItem('siteCode', success.siteCode);
        //localStorage.setItem('siteNameArb', success.siteNameArb);
        //localStorage.setItem('siteNameEng', success.siteNameEng);
        this.ngxService.stop();
        this.showImage = true;
        //this.router.navigate(['M35001']);
      },
        error => {
          this.ngxService.stop();
          this.messageService.add({ severity: 'error', summary: 'Error!', detail: error.error.msg });
        });
  }

  onDoctorImageClick() {
    this.router.navigate(['M35001']);
  }

  onUserNameFocus() {
    this.focusedItem = 'UserName';
  }

  onUserNameKeyUp() {
    this.txtPassword.nativeElement.focus();
  }

  onPasswordFocus() {
    this.focusedItem = 'Password';
  }

  onPasswordKeyUp() {
    this.checkUser();
  }

  onBtn1Click() {
    if (this.focusedItem === 'UserName') {
      this.userName += '1';
      this.txtUserName.nativeElement.focus();
    }
    else {
      this.password += '1';
      this.txtPassword.nativeElement.focus();
    }
  }

  onBtn2Click() {
    if (this.focusedItem === 'UserName') {
      this.userName += '2';
      this.txtUserName.nativeElement.focus();
    }
    else {
      this.password += '2';
      this.txtPassword.nativeElement.focus();
    }
  }

  onBtn3Click() {
    if (this.focusedItem === 'UserName') {
      this.userName += '3';
      this.txtUserName.nativeElement.focus();
    }
    else {
      this.password += '3';
      this.txtPassword.nativeElement.focus();
    }
  }

  onBtn4Click() {
    if (this.focusedItem === 'UserName') {
      this.userName += '4';
      this.txtUserName.nativeElement.focus();
    }
    else {
      this.password += '4';
      this.txtPassword.nativeElement.focus();
    }
  }

  onBtn5Click() {
    if (this.focusedItem === 'UserName') {
      this.userName += '5';
      this.txtUserName.nativeElement.focus();
    }
    else {
      this.password += '5';
      this.txtPassword.nativeElement.focus();
    }
  }

  onBtn6Click() {
    if (this.focusedItem === 'UserName') {
      this.userName += '6';
      this.txtUserName.nativeElement.focus();
    }
    else {
      this.password += '6';
      this.txtPassword.nativeElement.focus();
    }
  }

  onBtn7Click() {
    if (this.focusedItem === 'UserName') {
      this.userName += '7';
      this.txtUserName.nativeElement.focus();
    }
    else {
      this.password += '7';
      this.txtPassword.nativeElement.focus();
    }
  }

  onBtn8Click() {
    if (this.focusedItem === 'UserName') {
      this.userName += '8';
      this.txtUserName.nativeElement.focus();
    }
    else {
      this.password += '8';
      this.txtPassword.nativeElement.focus();
    }
  }

  onBtn9Click() {
    if (this.focusedItem === 'UserName') {
      this.userName += '9';
      this.txtUserName.nativeElement.focus();
    }
    else {
      this.password += '9';
      this.txtPassword.nativeElement.focus();
    }
  }

  onBtn0Click() {
    if (this.focusedItem === 'UserName') {
      this.userName += '0';
      this.txtUserName.nativeElement.focus();
    }
    else {
      this.password += '0';
      this.txtPassword.nativeElement.focus();
    }
  }

  onBtnDotClick() {
    if (this.focusedItem === 'UserName') {
      this.userName += '.';
      this.txtUserName.nativeElement.focus();
    }
    else {
      this.password += '.';
      this.txtPassword.nativeElement.focus();
    }
  }

  onBtnACClick() {
    if (this.focusedItem === 'UserName') {
      this.userName = '';
      this.txtUserName.nativeElement.focus();
    }
    else {
      this.password = '';
      this.txtPassword.nativeElement.focus();
    }
  }

  onBtnBSClick() {
    if (this.focusedItem === 'UserName') {
      this.userName = this.userName.substring(0, this.userName.length - 1);
      this.txtUserName.nativeElement.focus();
    }
    else {
      this.password = this.password.substring(0, this.password.length - 1);
      this.txtPassword.nativeElement.focus();
    }
  }

  onBtnEnterClick() {
    if (this.focusedItem === 'UserName') {
      this.txtPassword.nativeElement.focus();
    }
    else {
      this.checkUser();
    }
  }

  onBtnLoginClick() {
    this.checkUser();
  }

  onBtnLogoutClick() {
    this.userName = '';
    this.password = '';
    this.txtUserName.nativeElement.focus();
  }
}
