import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import {
  Observable,
  catchError,
  delay,
  of,
  takeUntil,
  tap,
  throwError,
} from 'rxjs';
import { TUpdateUserProfileRequest } from '../types/update-userprofile.type';
import { TChangePasswordRequest } from '../types/change-password.type';
import { TDepartments } from '../types/department.type';
import { environment } from 'src/environments/environment.development';
import { TDesignations } from '../types/Designation.type';
import { TReportingManager } from '../types/reporting-manager.type';
import { LocalStorageService } from 'src/app/services/local-storage.service';

@Injectable({ providedIn: 'root' })
export class UserProfileService{
  http: HttpClient = inject(HttpClient);


   //to get user profile data
   getUserProfile(userId: string): Observable<any> {
    return this.http.get<any>(`${environment.API_BASE_URL}/profile?id=${userId}`).pipe();
  }

  //to update profile by admin
  updateProfileByAdmin(data: any): Observable<any> {
    console.log(data);
    return this.http.put<any>(`${environment.API_BASE_URL}/profile`, data);
  }

  //to update profile by user itself
  updateProfile(data: any): Observable<any> {
    console.log(data);
    return this.http.patch<any>(`${environment.API_BASE_URL}/profile`, data);
  }

  //to change password
  changePassword(data: TChangePasswordRequest): Observable<any>{
    console.log(data);
    return this.http.put<any>(`${environment.API_BASE_URL}/password`, data);
  }
  
  //to get user isactive state
  getUserActiveState(userId: string): Observable<any>{
    return this.http.get<any>(`${environment.API_BASE_URL}/getUserActiveState?id=${userId}`);
  }

  //to deactivate user account
  deactivateOrActivateAccount(id: string): Observable<any>{
    return this.http.patch<any>(`${environment.API_BASE_URL}/deactivateOrActivate-account?id=${id}`, {});
  }

  //to reset password by admin
  resetPassword(userId: string): Observable<any>{
    console.log(userId);
    return this.http.post<any>(`${environment.API_BASE_URL}/reset-password`, {userId});
  }

  //to get departments
  getDepartments(): Observable<any> {
    return of(dummyDepartments).pipe(delay(1000));
  }

  //to get designations
  getDesignations(): Observable<any> {
    return of(dummyDesignations).pipe(delay(1000));
  }

  //to get reporting managers
  getReportingManagers(): Observable<any> {
    return of(dummyReportingManagers).pipe(delay(1000));
  }
}


const dummyDepartments: TDepartments[] = [
  {
    Id: '9c4bc204-dd96-4d7b-8637-6f4334b2c098',
    DepartmentName: 'HR',
  },
  {
    Id: '52cda349-943d-483c-b0ec-3715b9d8b3db',
    DepartmentName: 'DTX',
  },
  {
    Id: 'd1f8230d-e661-4248-ab3f-cfc419617e19',
    DepartmentName: 'JST',
  }
];

const dummyDesignations: TDesignations[] = [
  {
    Id: 'bc7a64c8-9a53-4298-a057-120a8fb4d4eb',
    Designation: 'Associate Software Engineer',
  },
  {
    Id: '52a77712-7024-4761-aab0-1df1fcc53573',
    Designation: 'Assistant Manager',
  },
  {
    Id: '3f084b0d-e795-47ef-9e49-6f0120710dce',
    Designation: 'Intern',
  }
];

const dummyReportingManagers: TReportingManager[] = [
  {
    Id: 'c71943ac-5f83-48b5-a4a0-c7314162bee9',
    FullName: 'Jules Ambrose',
  },
  {
    Id: '3f743aa6-36a4-410e-99ad-82ed27c1f443',
    FullName: 'Alex Volvo',
  },
];
