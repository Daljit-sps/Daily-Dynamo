import { Component, OnInit, inject } from '@angular/core';
import { NonNullableFormBuilder, Validators } from '@angular/forms';
import { TUpdateUserProfileForm, TUpdateUserProfileRequest } from '../../types/update-userprofile.type';
import { HotToastService } from '@ngneat/hot-toast';
import { minimumLength } from 'src/app/features/shared/validators/min-length.validator';
import { maximumLength } from 'src/app/features/shared/validators/max-length.validator';
import { emailValidator } from 'src/app/features/shared/validators/email.validator';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { UserProfileService } from '../../services/userprofile.service';
import { Observable, of, take } from 'rxjs';
import { Store } from '@ngrx/store';
import { TAppState } from 'src/app/types/app-state.type';
import { selectUserId, selectUserRole } from 'src/app/store/auth.selector';
import { TDepartments } from '../../types/department.type';
import { TDesignations } from '../../types/Designation.type';
import { TReportingManager } from '../../types/reporting-manager.type';
import * as moment from 'moment';
import { UserRole } from 'src/app/features/shared/types/role.enum';
import { environment } from 'src/environments/environment.development';
import { ModelComponent } from 'src/app/features/shared/components/popup-model/popup-model.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-update-userprofile',
  templateUrl: './update-userprofile.component.html',
  styleUrls: ['./update-userprofile.component.scss'],
})

export class UpdateUserprofileComponent implements OnInit {
    router: Router = inject(Router);
    fb: NonNullableFormBuilder = inject(NonNullableFormBuilder);
    toaster: HotToastService = inject(HotToastService);
    service: UserProfileService = inject(UserProfileService);
    store: Store<TAppState> = inject(Store<TAppState>);
    dialog: MatDialog = inject(MatDialog);
    
    imageUrl: File | undefined;
    user$: TUpdateUserProfileRequest[] | undefined;
    userId: string | undefined;
    userRole: UserRole | undefined;
    idFromForm: string | undefined = undefined;
    showCheck = false;
    selectedImgUrl: string | ArrayBuffer | null | undefined;
    isSelectedImg = false;
    loading = false;
    isAccountActive = false;
    userName: string | undefined;

    departments$: Observable<TDepartments[]> = of();
    designations$: Observable<TDesignations[]> = of();
    reportingManagers$: Observable<TReportingManager[]> = of();

    ngOnInit(): void {
      this.store
          .select(selectUserRole)
          .pipe(take(1))
          .subscribe(role => {
            if (role) {
              this.userRole = role;
            }
          });
    
      this.loadUserProfile();
      this.departments$ = this.service.getDepartments();
      this.designations$ = this.service.getDesignations();
      this.reportingManagers$ = this.service.getReportingManagers();
    }
  
    loadUserProfile(): void {
      this.route.queryParams.subscribe(params => {
        const userId = params['id'];
        if (userId) {
          this.userId = userId;
          this.getUserProfile(userId);
        }
        else{
          this.store
          .select(selectUserId)
          .pipe(take(1))
          .subscribe(uId => {
          if (uId) {
            this.userId = uId;
            this.getUserProfile(this.userId);
          }
          });
        }
      });
    }

    
    getProfileImage(): any {
      if (!this.imageUrl) {
          return 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT4eMoz7DH8l_Q-iCzSc1xyu_C2iryWh2O9_FcDBpY04w&s';
      }
      return environment.API_BASE_URL + '/' + this.imageUrl;
    }

    constructor(private route: ActivatedRoute) {} 

    getUserProfile(userId: string): void {
      console.log(userId)
      if(userId != null){
        this.service.getUserProfile(userId).subscribe((response) => {
          console.log(response);
          if (response) {
            this.idFromForm = response.data.id;
            this.userName = response.data.firstName + ' ' + response.data.lastName;
            console.log(this.idFromForm)
            const dateOfBirthString = response.data.dateOfBirth;
            let dateOfBirth: Date | undefined;
            if (dateOfBirthString) {
              const [day, month, year] = dateOfBirthString.split('-');
              const isoDateString = `${year}-${month}-${day}`; // Reformat to "YYYY-MM-DD" format
              dateOfBirth = new Date(isoDateString);
            } 
            
            this.form.patchValue({
              id: response.data.id,
              firstName: response.data.firstName,
              lastName: response.data.lastName,
              emailId: response.data.emailId,
              mobileNo: response.data.mobileNo,
              dob: dateOfBirth,
              department: response.data.departmentId,
              designation: response.data.designationId,
              reportingManager: response.data.managerId,
              address: response.data.address,
              genderId: response.data.genderId,
            });
            this.imageUrl = response.data.profileImageUrl;
            this.isAccountActive = response.data.isAccountActive;
            console.log(response.data)

            this.addCheckOnDropdown();
          }
          
        });
      }
        
    }

    addCheckOnDropdown(): void {
      // Get the loged userId from the store
      this.store
        .select(selectUserId)
        .pipe(take(1))
        .subscribe(uId => {
          if (uId) {
            this.userId = uId;
            console.log("loged user", this.userId);
            console.log("form user", this.idFromForm);
    
            if (this.userRole == 1) {
              if(this.userId == this.idFromForm){
                this.form.controls['department'].disable();
                this.form.controls['designation'].disable();
                this.form.controls['reportingManager'].disable();
              }
              else{
                this.form.controls['department'].enable();
                this.form.controls['designation'].enable();
                this.form.controls['reportingManager'].enable();
              }
            } else {
              this.form.controls['department'].enable();
              this.form.controls['designation'].enable();
              this.form.controls['reportingManager'].enable();
            }
          }
        });
    
      // Disable form controls if user role is not 1
      if (this.userRole != 1) {
        this.form.controls['department'].disable();
        this.form.controls['designation'].disable();
        this.form.controls['reportingManager'].disable();
      }
    }
    
    

    form = this.fb.group<TUpdateUserProfileForm>({
      id: this.fb.control<string>(''),
      firstName: this.fb.control<string>('', [
        Validators.required,
        Validators.pattern('^[a-zA-Z]+$'),
      ]),
      lastName: this.fb.control<string>('', [
        Validators.required,
        Validators.pattern('^[a-zA-Z]+$'),
      ]),
      emailId: this.fb.control<string>('', [Validators.required, emailValidator()]),
      mobileNo: this.fb.control<string>('', [
        Validators.required,
        minimumLength(10),
        maximumLength(10),
      ]),
      dob: this.fb.control<Date>(new Date()),
      department: this.fb.control<string>('',[
        Validators.required,
      ]),
      designation: this.fb.control<string>('',[
        Validators.required,
      ]),
      reportingManager: this.fb.control<string>('',[
        Validators.required,
      ]),
      address: this.fb.control<string>(''),
      genderId: this.fb.control<string>('',Validators.required)
    });
    onFileSelected(event: any): void {
      const file = event.target.files[0];
      if (file) {
        const reader = new FileReader();
        reader.onload = (e: any) => {
          this.imageUrl = file;
          this.selectedImgUrl = reader.result;
          this.isSelectedImg = true;
        };
        reader.readAsDataURL(file);
      }
    }

  removeImage() {
    this.imageUrl = undefined;
    this.selectedImgUrl = undefined;
    this.isSelectedImg = false;
  }

  clearForm(){
      this.form.reset(); 
  }
  
    
    
    get controls(): TUpdateUserProfileForm {
      return this.form.controls;
    }


    updateProfile(): void {
      if(this.form.value.genderId == null){
        this.showCheck = true;
      }
      if (this.form.invalid) {
        this.toaster.error('Form is invalid!');
        return;
      }

      this.loading = true;
      let dateonly= this.form.value.dob as any;
      // Check if dob is today's date and set it to null if true
      if (moment(dateonly).isSame(moment(), 'day')) {
        dateonly = null;
      }
      console.log(this.imageUrl);
      if(this.userRole == 1){
        const formData = new FormData();
        formData.append('id', this.form.value.id || '');
        formData.append('firstName', this.form.value.firstName || '');
        formData.append('lastName', this.form.value.lastName || '');
        formData.append('emailId', this.form.value.emailId || '');
        formData.append('mobileNo', this.form.value.mobileNo || '');
        formData.append('genderId', this.form.value.genderId || '');
        formData.append('address', this.form.value.address || '');
        formData.append('dob', dateonly ? moment(dateonly).format('YYYY-MM-DD') : '');
        formData.append('departmentId', this.form.value.department || '');
        formData.append('designationId', this.form.value.designation || '');
        formData.append('managerId', this.form.value.reportingManager || '');
        formData.append('profileImage', this.imageUrl || '');

        this.service
        .updateProfileByAdmin(formData)
        .subscribe({
          next: (respone) => {this.openSuccessOrErrorModal(respone.message, 'Success');},
          error: (response) => {this.openSuccessOrErrorModal(response.message, 'Error');},
          complete: () => {
            this.loading = false;
          }
        });
      }
      
      else{
        const formData = new FormData();
        formData.append('id', this.form.value.id || '');
        formData.append('firstName', this.form.value.firstName || '');
        formData.append('lastName', this.form.value.lastName || '');
        formData.append('emailId', this.form.value.emailId || '');
        formData.append('mobileNo', this.form.value.mobileNo || '');
        formData.append('genderId', this.form.value.genderId || '');
        formData.append('address', this.form.value.address || '');
        formData.append('dob', dateonly ? moment(dateonly).format('YYYY-MM-DD') : '');
        formData.append('profileImage', this.imageUrl || '');

        this.service
        .updateProfile(formData)
        .subscribe({
          next: (respone) => {this.openSuccessOrErrorModal(respone.message, 'Success');},
          error: (response) => {this.openSuccessOrErrorModal(response.message, 'Error');},
          complete: () => {
            this.loading = false;
          }
        });
      }
        
        
        
        
    }

    openSuccessOrErrorModal(message: string, check: string): void {
      const dialogRef = this.dialog.open(ModelComponent, {
        width: '500px',
        data: { message: message, check: check }
      });
  
      dialogRef.afterClosed().subscribe(() => {
        console.log('The dialog was closed');
      });
    }
}

