import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable()
export class T06209Service {
  constructor(private http: HttpClient) { }

  getBMIindex() {
    return this.http.get('api/t06209/getBMIindex').pipe(map(response => response));
  }
  getBPindex() {
    return this.http.get('api/t06209/getBPindex').pipe(map(response => response));
  }
  getTempindex() {
    return this.http.get('api/t06209/getTempindex').pipe(map(response => response));
  }
  getPulseindex() {
    return this.http.get('api/t06209/getPulseindex').pipe(map(response => response));
  }
  getRRindex() {
    return this.http.get('api/t06209/getRRindex').pipe(map(response => response));
  }
  getGLindex() {
    return this.http.get('api/t06209/getGLindex').pipe(map(response => response));
  }
  getMedHxindex() {
    return this.http.get('api/t06209/getMedHxindex').pipe(map(response => response));
  }
  getAllergyDietindex() {
    return this.http.get('api/t06209/getAllergyDietindex').pipe(map(response => response));
  }
  getAllergyMedindex() {
    return this.http.get('api/t06209/getAllergyMedindex').pipe(map(response => response));
  }
  getRecommendationDropDownlist() {
    return this.http.get('api/t06209/getRecommendationDropDownlist').pipe(map(response => response));
  }
  getLocationDropDownlist() {
    return this.http.get('api/t06209/getLocationDropDownlist').pipe(map(response => response));
  }
  getPatListPopData(PatNo: string) {
    return this.http.get('api/t06209/getPatListPopData', { params: { PatNo:PatNo } }).pipe(map(response => response));
  }
  getDoctorListPopData() {
    return this.http.get('api/t06209/getDoctorListPopData').pipe(map(response => response));
  }
  getDocWardEpiDateByType(PAT_TYPE: string, PatNo: string) {
    return this.http.get('api/t06209/getDocWardEpiDateByType', { params: { PAT_TYPE: PAT_TYPE, PatNo: PatNo } }).pipe(map(response => response));
  }
  getPatientVitalDetails(PAT_NUMBER: string, PAT_TYPE: string) {
    return this.http.get('api/t06209/getPatientVitalDetails', { params: { PAT_NUMBER: PAT_NUMBER, PAT_TYPE: PAT_TYPE } }).pipe(map(response => response));
  }
  getPatRiskFactor(PAT_NUMBER: string) {
    return this.http.get('api/t06209/getPatRiskFactor', { params: { PAT_NUMBER: PAT_NUMBER } }).pipe(map(response => response));
  }
  saveData(data: any) {
    return this.http.post('api/t06209/saveData', data).pipe(map(response => response));
  }
}
