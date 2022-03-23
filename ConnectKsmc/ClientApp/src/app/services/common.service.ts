import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';

@Injectable()
export class CommonService {
  constructor(private http: HttpClient) {
  }

  getAllMessage(msgCode: string) {
    return this.http.get('api/common/getAllMessage', { params: { msgCode: msgCode } }).pipe(map(response => response));
  }

  getFormInfo(formCode: string) {
    return this.http.get('api/common/getFormInfo', { params: { formCode: formCode } }).pipe(map(response => response));
  }

  getFormLabel(formCode: string) {
    return this.http.get('api/common/getFormLabel', { params: { formCode: formCode } }).pipe(map(response => response));
  }

  getFormPermission(formCode: string) {
    return this.http.get('api/common/getPermission', { params: { formCode: formCode } }).pipe(map(response => response));
  }

  getFormLabelForEdit(formCode: string) {
    return this.http.get('api/common/getFormLabelForEdit', { params: { formCode: formCode } }).pipe(map(response => response));
  }
  updateFormLabel(t01200: any[]) {
    return this.http.post('api/common/updateFormLabel', t01200).pipe(map(response => response));
  }
  convertToDateString(sDate: any) {
    var dataType = typeof sDate;
    if (dataType == "string") {
      return sDate;
    }
    return (sDate.getDate() < 10 ? '0' + sDate.getDate() : sDate.getDate()) + '/' + ((sDate.getMonth() + 1) < 10 ? '0' + (sDate.getMonth() + 1) : (sDate.getMonth() + 1)) + '/' + sDate.getFullYear();
  }
  convertToTimeString(sDate: any) {
    var dataType = typeof sDate;
    if (dataType == "string") {
      return sDate;
    }
    var date = new Date(sDate),
      hours = ("0" + date.getHours()).slice(-2),
      minutes = ("0" + date.getMinutes()).slice(-2);
    return [hours, minutes].join(":");
  }
  convertToDateTimeString(sDate: any) {
    var dataType = typeof sDate;
    if (dataType == "string") {
      return sDate;
    }
    var time = new Date(sDate),
      //mnth = ("0" + (time.getMonth() + 1)).slice(-2),
      //day = ("0" + time.getDate()).slice(-2),
      hours = ("0" + time.getHours()).slice(-2),
      minutes = ("0" + time.getMinutes()).slice(-2);
    var tm = [hours, minutes].join(":");
    var dt = (sDate.getDate() < 10 ? '0' + sDate.getDate() : sDate.getDate()) + '/' + ((sDate.getMonth() + 1) < 10 ? '0' + (sDate.getMonth() + 1) : (sDate.getMonth() + 1)) + '/' + sDate.getFullYear();
    return dt + ' ' + tm;
  }
  addMin(sDate: any, min: number) {
    var time = new Date(sDate);
    time.setTime(time.getTime() + min * 60 * 1000);
    return time;
  }
  getMin(sDate: any) {
    var time = new Date(sDate);
    return time.getHours() * 60 + time.getMinutes();
  }
  convertDate(date: any) {
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var rDate = date.getDate() + "-" + monthNames[date.getMonth()] + "-" + date.getFullYear()
    return rDate;
  }
  convertToHijriDate(date: any, format: any){
    if (format.toUpperCase() == 'DDMMYYYY') {
      return new Date(date).toLocaleDateString("ar-SA", { day: '2-digit' }) + '/' + new Date(date).toLocaleDateString("ar-SA", { month: '2-digit' }) + '/' + new Date(date).toLocaleDateString("ar-SA", { year: 'numeric' });
    }
    else if (format.toUpperCase() == 'MMDDYYYY') {
      return new Date(date).toLocaleDateString("ar-SA", { month: '2-digit' }) + '/' + new Date(date).toLocaleDateString("ar-SA", { day: '2-digit' }) + '/' + new Date(date).toLocaleDateString("ar-SA", { year: 'numeric' });
    }
    else if (format.toUpperCase() == 'YYYYMMDD') {
      return new Date(date).toLocaleDateString("ar-SA", { year: 'numeric' }) + '/' + new Date(date).toLocaleDateString("ar-SA", { month: '2-digit' }) + '/' + new Date(date).toLocaleDateString("ar-SA", { day: '2-digit' });
    }
    return '';
  }
}
