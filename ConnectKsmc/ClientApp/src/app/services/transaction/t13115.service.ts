import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable()
export class T13115Service {
  constructor(private http: HttpClient) { }

  getAllPriority() {
    return this.http.get('api/t13115/getPriorities').pipe(map(response => response));
  }
  getAllPatientType() {
    return this.http.get('api/t13115/getAllPatientType').pipe(map(response => response));
  }
  getAllWorkStation() {
    return this.http.get('api/t13115/getAllWorkStation').pipe(map(response => response));
  }
  getDetailInfo(requestNo: string) {
    return this.http.get('api/t13115/getDetailInfo', { params: { requestNo: requestNo } }).pipe(map(response => response));
  }
  getWorkStations() {
    return this.http.get('api/t13115/getWorkStations').pipe(map(response => response));
  }
  getPatientInformation(patNo: string) {
    return this.http.get('api/t13115/getPatientInformation', { params: { patNo: patNo } }).pipe(map(response => response));
  }
  getAnalysisGroup(code: string) {
    return this.http.get('api/t13115/getAnalysisGroup', { params: { wsCode: code } }).pipe(map(response => response));
  }
  getAnalysis(code: string) {
    return this.http.get('api/t13115/getAnalysis', { params: { aCode: code } }).pipe(map(response => response));
  }
  getAnalysisNew(code: string) {
    return this.http.get('api/t13115/getAnalysisNew', { params: { wsCode: code } }).pipe(map(response => response));
  }
  getSites(code: string) {
    return this.http.get('api/t13115/getSites', { params: { code: code } }).pipe(map(response => response));
  }
  getRequestDetails(reqNo: string) {
    return this.http.get('api/t13115/getRequestDetails', { params: { reqNo: reqNo } }).pipe(map(response => response));
  }
}
