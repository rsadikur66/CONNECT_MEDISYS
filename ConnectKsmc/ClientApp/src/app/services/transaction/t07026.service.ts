import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable()
export class T07026Service {
  constructor(private http: HttpClient) { }

  getAssignDoctor() {
    return this.http.get('api/t07026/getAssignDoctor').pipe(map(response => response));
  }

  getAllDoctors(clinicCode: string, apptDate: string) {
    return this.http.get('api/t07026/getAllDoctors', { params: { clinicCode: clinicCode, apptDate: apptDate } }).pipe(map(response => response));
  }

  getAllClinics(doctorCode: string) {
    return this.http.get('api/t07026/getAllClinics', { params: { doctorCode: doctorCode } }).pipe(map(response => response));
  }

  getAllApptTypes() {
    return this.http.get('api/t07026/getAllApptTypes').pipe(map(response => response));
  }

  getAllArrivalStatus() {
    return this.http.get('api/t07026/getAllArrivalStatus').pipe(map(response => response));
  }

  getAllICD10() {
    return this.http.get('api/t07026/getAllICD10').pipe(map(response => response));
  }

  getAllDocArrivalStatus() {
    return this.http.get('api/t07026/getAllDocArrivalStatus').pipe(map(response => response));
  }

  isDocOnVacation(doctorCode: string, apptDate: string) {
    return this.http.get('api/t07026/isDocOnVacation', { params: { doctorCode: doctorCode, apptDate: apptDate } }).pipe(map(response => response));
  }

  getAllAppointments(doctorCode: string, clinicCode: string, scheduleRule: string, apptDate: string) {
    return this.http.get('api/t07026/getAllAppointments', { params: { doctorCode: doctorCode, clinicCode: clinicCode, scheduleRule: scheduleRule, apptDate: apptDate } }).pipe(map(response => response));
  }

  getClinicType(clinicCode: string, scheduleRule: string) {
    return this.http.get('api/t07026/getClinicType', { params: { clinicCode: clinicCode, scheduleRule: scheduleRule } }).pipe(map(response => response));
  }

  getFollowupAppointments(doctorCode: string, clinicCode: string) {
    return this.http.get('api/t07026/getFollowupAppointments', { params: { doctorCode: doctorCode, clinicCode: clinicCode } }).pipe(map(response => response));
  }

  checkUserIsConsultant() {
    return this.http.get('api/t07026/checkUserIsConsultant').pipe(map(response => response));
  }

  getFollowupAppointmentsByDays(days: string, clinicCode: string) {
    return this.http.get('api/t07026/getFollowupAppointmentsByDays', { params: { days: days, clinicCode: clinicCode } }).pipe(map(response => response));
  }
}
