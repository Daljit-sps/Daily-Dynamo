import { Component, EventEmitter, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  templateUrl: './popup-model.component.html',
  styleUrls: ['./popup-model.component.scss'],
})
export class ModelComponent {
    public onYes: EventEmitter<boolean> = new EventEmitter<boolean>();
    icon: string;
    title: string; 
    message: string;
    isConfirmation: boolean = false;

    constructor(
      public dialogRef: MatDialogRef<ModelComponent>,
      @Inject(MAT_DIALOG_DATA) public data: { message: string, check: string }
    ) {
      if (data.check == 'Success') {
        this.icon = 'done'; 
        this.title = 'Success';
      } 
      else if(data.check == 'Error') {
        this.icon = 'highlight_off'; 
        this.title = 'Error';
      }
      else{
        this.icon = 'help'; 
        this.title = 'Confirm Action';
        this.isConfirmation = true;
      }
      this.message = data.message;
    }
  
    closeModal(): void {
      this.dialogRef.close();
      this.refreshPage();
    }
  
    refreshPage(): void {
      window.location.reload();
    }

    onNoClick(): void {
      this.dialogRef.close(false);
    }
  
    onYesClick(): void {
      this.dialogRef.close(true);
      this.onYes.emit(true);
    }
}
