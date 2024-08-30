import { Injectable, inject } from '@angular/core';
import { TUser } from '../types/user.type';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, delay, of, shareReplay } from 'rxjs';
import { APIResponse } from 'src/app/types/api-response.type';
import { environment } from 'src/environments/environment.development';

@Injectable({ providedIn: 'root' })
export class UserService {
  http: HttpClient = inject(HttpClient);

  getUsers(): Observable<APIResponse<TUser[]>> {
    return this.http.get<APIResponse<TUser[]>>(
      `${environment.API_BASE_URL}/employees?page=1&offset=100`
    );
  }
}
