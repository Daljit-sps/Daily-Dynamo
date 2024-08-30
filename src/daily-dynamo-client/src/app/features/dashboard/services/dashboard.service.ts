import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, delay, of, shareReplay } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { TDSRStatusTable } from '../types/dashboard-table.type';
import { TPaginatedResponse } from 'src/app/types/paginated-response.type';
import { APIResponse } from 'src/app/types/api-response.type';

@Injectable({ providedIn: 'root' })
export class DashboardService {
  http: HttpClient = inject(HttpClient);

  getData(page: number, offset: number): Observable<TPaginatedResponse<APIResponse<TDSRStatusTable[]>>> {
    return this.http.get<TPaginatedResponse<APIResponse<TDSRStatusTable[]>>>(
      `${environment.API_BASE_URL}/work-diary/dashboard?page=${page}&offset=${offset}`
    );
  }
}
