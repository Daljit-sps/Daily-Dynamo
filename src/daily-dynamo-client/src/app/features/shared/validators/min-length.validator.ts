import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const minimumLength = (lengthConstraint: number): ValidatorFn => {
  return (control: AbstractControl): ValidationErrors | null => {
    const length = `${control.value}`.length;
    return length < lengthConstraint
      ? { minLength: { value: control.value, status: control.status, length } }
      : null;
  };
};
