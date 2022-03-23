import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class T30023Service {
  private formCode = 't30023/';
  constructor(private http: HttpClient) {
  }
  getInsList() {
    return this.http.get('api/t25523/getWorkStations').pipe(map(response => response));
  }
}
