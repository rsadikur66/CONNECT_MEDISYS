import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class T11013Service {
  constructor(private http: HttpClient) {
  }
  getAllPatient() {
    return this.http.get('api/t30014/getAllPatient').pipe(map(response => response));
  }
  getAllPatientType() {
    return this.http.get('api/t11013/getAllPatientType').pipe(map(response => response));
  }
  getAllInfectionPrecaution() {
    return this.http.get('api/t11013/getAllINFPREC').pipe(map(response => response));
  }
  getAllType() {
    return this.http.get('api/t11013/getAllType').pipe(map(response => response));
  }
  getAllPriority() {
    return this.http.get('api/t11013/getAllPriority').pipe(map(response => response));
  }
  getLocationByPatType(pTtype: string, pNo: string) {
    return this.http.get('api/t11013/getLocationByPatType', { params: { patType: pTtype, patNo: pNo } }).pipe(map(response => response));
  }
  getAllProcedure(procType: string) {
    return this.http.get('api/t11013/getAllProcedure', { params: { procType: procType } }).pipe(map(response => response));
  }
  getAllDoctor() {
    return this.http.get('api/t11013/getAllDoctor').pipe(map(response => response));
  }
  getDoctorInfo() {
    return this.http.get('api/t11013/getDoctorInfo').pipe(map(response => response));
  }
  getPatientInfo(patNo: string) {
    return this.http.get('api/t11013/getPatientInfo', { params: { patNo: patNo } }).pipe(map(response => response));
  }
  getPatientDetails(patNo: string) {
    return this.http.get('api/t11013/getPatientDetails', { params: { patNo: patNo } }).pipe(map(response => response));
  }
  getRadiologyRequestList(patientNo: string) {
    return this.http.get('api/t11013/getRadiologyRequestList', { params: { patNo: patientNo } }).pipe(map(response => response));
  }
  getRadiologyRequestDetails(orderNo: string) {
    return this.http.get('api/t11013/getRadiologyRequestDetails', { params: { orderNo: orderNo } }).pipe(map(response => response));
  }
  saveT11013(t11013: any) {
    return this.http.post('api/t11013/saveT11013', t11013);
  }
}
