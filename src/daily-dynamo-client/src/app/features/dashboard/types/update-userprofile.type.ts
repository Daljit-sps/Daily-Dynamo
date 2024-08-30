import { FormControl } from '@angular/forms';

export type TUpdateUserProfileForm = {
  id: FormControl<string>;
  firstName: FormControl<string>;
  lastName: FormControl<string>;
  emailId: FormControl<string>;
  mobileNo: FormControl<string>;
  dob: FormControl<Date>;
  department: FormControl<string>;
  designation: FormControl<string>;
  reportingManager: FormControl<string>;
  address: FormControl<string>;
  genderId: FormControl<string>;
};

export type  TUpdateUserProfileRequest= {
    id: string;
    firstName: string;
    lastName: string;
    emailId: string;
    mobileNo: string;
    dob: Date;
    departmentId: string;
    designationId: string;
    managerId: string;
    address: string;
    genderId: string;
    profileImage?: File; // Optional image file
};
