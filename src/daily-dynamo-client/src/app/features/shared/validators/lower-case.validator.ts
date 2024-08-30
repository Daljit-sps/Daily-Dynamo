import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
  
export const lowerCaseLetterValidator: ValidatorFn = (
    control: AbstractControl
  ): ValidationErrors | null => {
    const lowerCaseRegex = /[a-z]/;

    if (!lowerCaseRegex.test(control.value)) {
      return { noLowerCaseLetter: true };
    }
  
    return null;
  };