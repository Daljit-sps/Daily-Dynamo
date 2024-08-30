import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const numeralValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {
  const numeralRegex = /\d/;

  if (!numeralRegex.test(control.value)) {
    return { noNumeral: true };
  }

  return null;
};
