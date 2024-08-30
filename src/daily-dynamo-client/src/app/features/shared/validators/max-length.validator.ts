import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const maximumLength = (lengthConstraint: number): ValidatorFn => {
  return (control: AbstractControl): ValidationErrors | null => {
    const length = `${control.value}`.length;
    return length > lengthConstraint
      ? { maxLength: { value: control.value, status: control.status, length } }
      : null;
  };
};
