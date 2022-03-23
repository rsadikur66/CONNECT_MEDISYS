import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class T06201Service {
  constructor(private http: HttpClient) { }
  getPatientType() {
    return this.http.get('api/t06201/getPatientType').pipe(map(response => response));
  }
  getPatientTypeInfo(value: string, patNo: string) {
    return this.http.get('api/t06201/getPatientTypeInfo', { params: { value: value, patNo: patNo } }).pipe(map(response => response));
  }
  getPatientInfo(patNo: string) {
    return this.http.get('api/t06201/getPatientInfo', { params: { patNo: patNo } }).pipe(map(response => response));
  }
  getDetails(patNo: string, patType: string) {
    return this.http.get('api/t06201/getDetails', { params: { patNo: patNo, patType: patType } }).pipe(map(response => response));
  }
  saveData(data: any) {
    const datas = { DataObject: new Object(data) };
    return this.http.post('api/t06201/saveData', datas).pipe(map(response => response));
  }
  getDoctorInfo() {
    return this.http.get('api/t06201/getDoctorInfo').pipe(map(response => response));
  }
}
