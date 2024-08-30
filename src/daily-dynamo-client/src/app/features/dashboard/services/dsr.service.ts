import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import {
  Observable,
  catchError,
  delay,
  map,
  of,
  takeUntil,
  tap,
  throwError,
} from 'rxjs';
import { TAddDSRRequest } from '../types/add-dsr.type';
import { environment } from 'src/environments/environment.development';
import { TDSRResponse } from '../types/dsr-types';
import { APIResponse } from 'src/app/types/api-response.type';
import { TPaginatedResponse } from 'src/app/types/paginated-response.type';
import { observableToBeFn } from 'rxjs/internal/testing/TestScheduler';
import { TGetDSRForSelectedDatesRequest } from '../types/getDSR-for-selectedDates.type';

@Injectable({ providedIn: 'root' })
export class DSRService {
  http: HttpClient = inject(HttpClient);

  //to add dsr
  addDSR(data: TAddDSRRequest): Observable<any> {
    console.log(data);
    return this.http.post<any>(`${environment.API_BASE_URL}/work-diary`, data);
  }

  getDsr(id: string): Observable<APIResponse<TDSRResponse>> {
    return this.http.get<APIResponse<TDSRResponse>>(
      `${environment.API_BASE_URL}/work-diary/${id}`
    );
  }

  getDSRs(): Observable<TPaginatedResponse<APIResponse<TDSRResponse[]>>> {
    return this.http.get<TPaginatedResponse<APIResponse<TDSRResponse[]>>>(
      `${environment.API_BASE_URL}/work-diary?page=1&offset=1000`
    );
  }

  //to get all DSR's of user
  getUserDSRs(id: string): Observable<any> {
    return this.http
      .get<any>(`${environment.API_BASE_URL}/work-diary/employee/${id}`)
      .pipe(map(response => response.data.data));
  }

  //to get user DSR for selected dates
  getDSRForSelectedDates(request: any): Observable<any>{
    console.log(request);
    return this.http
      .get<any>(`${environment.API_BASE_URL}/work-diary/employee?fromDate=${request.fromDate}&toDate=${request.toDate}`);
  }
}
