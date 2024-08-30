import { FormControl } from '@angular/forms';

export type TAddDSRForm = {
  reportDate: FormControl<String>;
  taskAccomplished: FormControl<string>;
  challengesFaced: FormControl<string>;
  nextDayPlan: FormControl<string>;
};

export type  TAddDSRRequest= {
    reportDate: String;
    taskAccomplished: string;
    challengesFaced: string;
    nextDayPlan: string;
    id?: string;
};
