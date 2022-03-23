import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable()
export class T07027Service {
  constructor(private http: HttpClient) { }

  getPatInfor(patNo: string) {
    return this.http.get('api/t07027/getPatInformation', { params: { patNo: patNo } }).pipe(map(response => response));
  }  
  getClinicSpcltyList() {
    return this.http.get('api/t07027/getClinicSpcltyList').pipe(map(response => response));
  }
  getClinicList(SPCLTY_CODE: string) {
    return this.http.get('api/t07027/getClinicList', { params: { SPCLTY_CODE: SPCLTY_CODE } }).pipe(map(response => response));
  }  
  getClinicDocList() {
    return this.http.get('api/t07027/getClinicDocList').pipe(map(response => response));
  }
  getSpcltyAndDocByClnCode(T_CLINIC_CODE: string, spcltyCode: string) {
    return this.http.get('api/t07027/getSpcltyAndDocByClnCode', { params: { T_CLINIC_CODE: T_CLINIC_CODE, spcltyCode: spcltyCode } }).pipe(map(response => response));
  }
  getAllAppDates() {
    return this.http.get('api/t07027/getAllAppDates').pipe(map(response => response));
  }
  
  getPatReqData(PAT_NUMBER: string) {
    return this.http.get('api/t07027/getPatReqData', { params: { PAT_NUMBER: PAT_NUMBER } }).pipe(map(response => response));
  }
  generateRequestNo(){
    return this.http.get('api/t07027/generateRequestNo').pipe(map(response => response));
  }
  //-----------------------------------------------------------------------------------------------------------------
  getHospitalList() {
    return this.http.get('api/t12207/getRefHospitalData').pipe(map(response => response));
  }  
  validateWeight(unitWeight: string, bagType: string) {
    return this.http.get('api/t12328/validateWeight', { params: { T_UNIT_WEIGHT: unitWeight, T_BAG_TYPE: bagType } }).pipe(map(response => response));
  }
  getReciever() {
    return this.http.get('api/t12328/getReciever').pipe(map(response => response));
  }
  getUnitList(bbCode: string, dateFrom: string, dateTo: string) {
    return this.http.get('api/t12328/getUnitList', { params: { bbCode: bbCode, dateFrom: dateFrom, dateTo: dateTo } }).pipe(map(response => response));
  }
  saveData(patNo: string, apptDate: string, clncSpclty: string, clncCode: string, clncDocCode: string, reqNo: string, reqTime: string) {
    debugger;
    const data = {
      PAT_NO: patNo,
      APPT_DATE: apptDate,
      CLINIC_SPCLTY: clncSpclty,
      CLINIC_CODE: clncCode,
      CLINIC_DOC_CODE: clncDocCode,
      REQ_NO: reqNo,
      REQ_TIME: reqTime
    }
    return this.http.post('api/t07027/saveData', data).pipe(map(response => response));
  }
}
