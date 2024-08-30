import { Component, EventEmitter, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { TDSRResponse } from '../../types/dsr-types';

@Component({
  templateUrl: './view-dsr-model.component.html',
  styleUrls: ['./view-dsr-model.component.scss'],
})
export class ViewSelectedDSRModelComponent {
    selectedDSR: TDSRResponse;

    constructor(
      public dialogRef: MatDialogRef<ViewSelectedDSRModelComponent>,
      @Inject(MAT_DIALOG_DATA) public data: { selectedDSR: TDSRResponse }
    ) {
      this.selectedDSR = data.selectedDSR;
    }
}
