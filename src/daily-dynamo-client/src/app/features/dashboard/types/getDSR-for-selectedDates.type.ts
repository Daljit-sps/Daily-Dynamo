import { FormControl } from '@angular/forms';

export type  TGetDSRForSelectedDatesForm= {
  fromDate: FormControl<String>;
  toDate: FormControl<string>;
};

export type  TGetDSRForSelectedDatesRequest= {
    fromDate: String;
    toDate: string;
};
