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
  getAnalysisByWs(wsCode: string) {
    return this.http.get('api/t13115/getAnalysisByWs', { params: { wsCode: wsCode } }).pipe(map(response => response));
  }

  getDetailInfo(requestNo: string) {
    return this.http.get('api/t13115/getDetailInfo', { params: { requestNo: requestNo } }).pipe(map(response => response));
  }
  getWorkStations() {
    return this.http.get('api/t13115/getWorkStations').pipe(map(response => response));
  }
  getAnalysis() {
    return this.http.get('api/t13115/getAnalysis').pipe(map(response => response));
  }

  getPatientInformation(patNo: string) {
    return this.http.get('api/t13115/getPatientInformation', { params: { patNo: patNo } }).pipe(map(response => response));
  }

  getAnalysisGroup(code: string) {
    return this.http.get('api/t13115/getAnalysisGroup', { params: { wsCode: code } }).pipe(map(response => response));
  }

  getAnalysisNew(code: string) {
    return this.http.get('api/t13115/getAnalysisNew', { params: { wsCode: code } }).pipe(map(response => response));
  }
  //getAnalysisRestriction
  getAnalysisRestriction(wsCode: string, anaCode: string) {
    return this.http.get('api/t13115/getAnalysisRestriction', { params: { wsCode: wsCode, anaCode: anaCode } }).pipe(map(response => response));
  }
  //specimen
  getNatOR(code: string) {
    return this.http.get('api/t13115/getNatOR', { params: { code: code } }).pipe(map(response => response));
  }
  getPatInfo(patNo: any) {
    return this.http.get('api/t13115/getPatInfo', { params: { patNo: patNo } }).pipe(map(response => response));
  }
  getRequestInfo15(requestNo: string) {
    return this.http.get('api/t13115/getRequestInfo15', { params: { requestNo: requestNo } }).pipe(map(response => response));
  }
  getRequestInfo16(requestNo: string) {
    return this.http.get('api/t13115/getRequestInfo16', { params: { requestNo: requestNo } }).pipe(map(response => response));
  }
  
  getBloodGroup(patNo: any) {
    return this.http.get('api/t13115/getBloodGroup', { params: { patNo: patNo } }).pipe(map(response => response));
  }
  insertT13115(t13015: any, t13016: any,requestNo: any) {
    const request = {
      t13015: t13015,
      requestList: new Object(t13016),
      requestNo: requestNo
    };
    return this.http.post('api/t13115/insert', request).pipe(map(response => response));
  }
  getRequestByPatNo(patNo: string) {
    return this.http.get('api/t13115/getRequestByPatNo', { params: { patNo: patNo } }).pipe(map(response => response));
  }
}
