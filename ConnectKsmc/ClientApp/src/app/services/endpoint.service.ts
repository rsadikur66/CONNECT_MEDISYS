import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class EndpointService {
  private baseURL = 'api/';
  constructor(private http: HttpClient) {
  }
  getData(action:string) {
    return this.http.get(this.baseURL + action).pipe(map(response => response));
  }
  getDataParam(action: string, paramList: any) {
    return this.http.get(this.baseURL + action, { params: paramList}).pipe(map(response => response));
  }
  setDataParam(action: string, obj: any) {
    return this.http.post(this.baseURL + action, obj).pipe(map(response => response));
  }
  deleteDataParam(action: string, obj: any) {
    return this.http.delete(this.baseURL + action, obj).pipe(map(response => response));
  }
}
