import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable()
export class Q13001Service {
  constructor(private http: HttpClient) {
  }

  getPatInfo(patNo: string) {
    return this.http.get('api/q13001/getPatInfo', { params: { patNo: patNo } }).pipe(map(response => response));
  }
  getLabInfo() {
    return this.http.get('api/t13115/getAllWorkStation').pipe(map(response => response));
  }
  getRequestInfo(patNo: string, lab: string) {
    return this.http.get('api/q13001/getRequestInfo', { params: { patNo: patNo, lab: lab } }).pipe(map(response => response));
  }
  getRequestDetail(reqNo: string,wsCode: string) {
    return this.http.get('api/q13001/getRequestDetail', { params: { reqNo: reqNo, wsCode: wsCode} }).pipe(map(response => response));
  }
  getPatientInfromation() {
    return this.http.get('api/t28011/getPatients').pipe(map(response => response));
  }
}
