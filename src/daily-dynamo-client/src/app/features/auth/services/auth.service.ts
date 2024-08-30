import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, delay, of, tap } from 'rxjs';
import { TSignupRequest } from '../types/signup.types';
import { TForgotPasswordRequest } from '../types/forgot-password.types';
import { TSignInRequest } from '../types/login.type';
import { EAuthStatus, TAuth } from 'src/app/types/user.type';
import { UserRole } from '../../shared/types/role.enum';
import { TAddPasswordRequest } from '../types/addpassword.type';
import { APIResponse } from 'src/app/types/api-response.type';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  http: HttpClient = inject(HttpClient);
  signup(data: TSignupRequest): Observable<APIResponse<boolean>> {
    return this.http.post<APIResponse<boolean>>(
      `${environment.API_BASE_URL}/adduser`,
      data
    );
  }
  sendResetLink(data: TForgotPasswordRequest): Observable<TForgotPasswordRequest> {
    return this.http.post<TForgotPasswordRequest>(`${environment.API_BASE_URL}/forgot-password`, data);
  }

  login(data: TSignInRequest): Observable<TAuth> {
    return this.http.post<TAuth>(`${environment.API_BASE_URL}/login`, data);
  }

  addpassword(data: TAddPasswordRequest): Observable<APIResponse<boolean>> {
    return this.http.patch<APIResponse<boolean>>(
      `${environment.API_BASE_URL}/password`,
      data
    );
  }
  constructor() {}
}
