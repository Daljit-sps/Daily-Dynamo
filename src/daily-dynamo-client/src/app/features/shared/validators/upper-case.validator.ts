import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const upperCaseLetterValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {
  const upperCaseRegex = /[A-Z]/;

  if (!upperCaseRegex.test(control.value)) {
    return { noUpperCaseLetter: true };
  }

  return null;
};
