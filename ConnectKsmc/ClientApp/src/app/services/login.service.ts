import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Idle, DEFAULT_INTERRUPTSOURCES } from '@ng-idle/core';
import { Keepalive } from '@ng-idle/keepalive';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { map } from 'rxjs/operators'

@Injectable()
export class LoginService {
  constructor(private http: HttpClient, private idle: Idle, private keepalive: Keepalive, private titleService: Title, private router: Router) {
  }

  loginUser(userName: string, password: string) {
    const login = {
      T_LOGIN_NAME: userName,
      T_PWD: password
    };
    return this.http.post('api/login', login).pipe(map(response => response));
  }

  logoutUser() {
    this.idle.stop();
    this.keepalive.stop();
    return this.http.post('api/logout', null);
  }

  checkIdle() {
    this.http.get('api/checkStatus').pipe(map(response => response))
      .subscribe(() => {
        this.idle.setIdle(900);
        this.idle.setTimeout(1);
        this.idle.setInterrupts(DEFAULT_INTERRUPTSOURCES);
        this.idle.onTimeout.subscribe(() => {
          this.logoutUser();
          localStorage.clear();
          this.titleService.setTitle('ConnectKsmc');
          this.router.navigate(['Login']);
        });
        this.keepalive.interval(60);
        this.keepalive.request('api/checkStatus');
        this.idle.watch();
      },
        error => {
          this.router.navigate(['Login']);
          console.log(error);
        });
  }
}
