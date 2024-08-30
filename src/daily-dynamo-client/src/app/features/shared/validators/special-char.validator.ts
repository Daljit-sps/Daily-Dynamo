import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
  
export const specialCharacterPasswordValidator: ValidatorFn = (
    control: AbstractControl
  ): ValidationErrors | null => {
  const specialCharRegex = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]+/;

  if (!specialCharRegex.test(control.value)) {
    return { noSpecialCharacter: true };
  }
  return null;
  };