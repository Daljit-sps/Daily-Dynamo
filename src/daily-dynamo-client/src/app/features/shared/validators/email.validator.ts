import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const emailValidator = (): ValidatorFn => {
  return (control: AbstractControl): ValidationErrors | null => {
    return /^[a-zA-Z0-9]+?[+a-zA-z0-9]+@[a-zA-Z]+.[a-zA-Z]{1,}$/.test(
      control.value
    )
      ? null
      : { email: { value: control.value, status: control.status } };
  };
};
