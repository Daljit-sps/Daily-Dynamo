import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  template: `<mat-dialog-content class="h-full">
    <div class="flex flex-col items-center h-full gap-5 p-5 text-center">
      <div class="w-52 md:w-72 aspect-video">
        <img src="/assets/images/email_notif.svg" alt="" />
      </div>
      <div class="flex flex-col flex-1 gap-4">
        <span>
          <p class="text-lg font-bold">
            Please check you inbox we have sent a confimation mail to
            <span class="text-primary">{{ email }}</span>
          </p>
        </span>
        <p class="text-sm">
          Please use the link received in the email to set a new password <br />
        </p>
      </div>
    </div>
  </mat-dialog-content>`,
  styleUrls: ['./signup-confirmation.component.scss'],
})
export class SignupConfirmationComponent {
  constructor(
    public dialogRef: MatDialogRef<SignupConfirmationComponent>,
    @Inject(MAT_DIALOG_DATA) public email: string
  ) {}
}
