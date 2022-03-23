import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators'

@Injectable()
export class MenuService {
  constructor(private http: HttpClient) {
  }

  getMenu(opt: string, lang: string) {
    return this.http.get('api/menu/get', { params: { option: opt, language: lang } }).pipe(map(response => response));
  }
}
