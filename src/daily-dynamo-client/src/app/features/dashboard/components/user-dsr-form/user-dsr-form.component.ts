import { Component, Input, OnInit, inject } from '@angular/core';
import { NonNullableFormBuilder, Validators } from '@angular/forms';
import { TAddDSRForm, TAddDSRRequest } from '../../types/add-dsr.type';
import { HotToastService } from '@ngneat/hot-toast';
import { Router } from '@angular/router';
import { DSRService } from '../../services/dsr.service';
import * as moment from 'moment';
import { MatDialog } from '@angular/material/dialog';
import { ModelComponent } from 'src/app/features/shared/components/popup-model/popup-model.component';
import { checkForToken } from 'src/app/store/auth.actions';
import { TAppState } from 'src/app/types/app-state.type';
import { Store } from '@ngrx/store';
import { take } from 'rxjs';
import { Unary } from '@angular/compiler';
import { selectUserName } from 'src/app/store/auth.selector';

@Component({
  selector: 'app-user-dsr-form',
  templateUrl: './user-dsr-form.component.html',
  styleUrls: ['./user-dsr-form.component.scss'],
})

export class UserDSRFormComponent implements OnInit{
    @Input() selectedDSR: TAddDSRRequest | null = null; // Input to receive selected DSR data

    router: Router = inject(Router);
    fb: NonNullableFormBuilder = inject(NonNullableFormBuilder);
    toaster: HotToastService = inject(HotToastService);
    service: DSRService = inject(DSRService);
    store: Store<TAppState> = inject(Store<TAppState>);
    dialog: MatDialog = inject(MatDialog);
    loading = false;
    userName: string | undefined;

    ngOnInit(): void {
      this.store
      .select(selectUserName)
      .pipe(take(1))
      .subscribe(uName => {
        if (uName) {
          this.userName = uName;
        }
      });
    }
    form = this.fb.group<TAddDSRForm>({
      reportDate: this.fb.control<string>('', [
        Validators.required,
      ]),
      taskAccomplished: this.fb.control<string>('', [
        Validators.required,
      ]),
      challengesFaced: this.fb.control<string>(''),
      nextDayPlan: this.fb.control<string>('', [
        Validators.required,
      ]),
    });

    ngOnChanges(): void {
      console.log(this.selectedDSR)
      if (this.selectedDSR) {
        this.form.patchValue({
          reportDate: this.selectedDSR.reportDate,
          taskAccomplished: this.selectedDSR.taskAccomplished,
          challengesFaced: this.selectedDSR.challengesFaced,
          nextDayPlan: this.selectedDSR.nextDayPlan,
        });
        console.log(this.form.value)
      }
      else{
        this.form.reset();
      }
    }
      get controls(): TAddDSRForm {
        return this.form.controls;
      }

      openConfirmationDialog(): void{
        if (this.form.invalid) {
          this.toaster.error('Form is invalid!');
          return;
        }
        const confirmationMsg = "Are you sure you want to submit DSR? No edits or deletes after."; 
        const dialogRef = this.dialog.open(ModelComponent, {
          width: '400px',
          data: { message: confirmationMsg, check: '' }
        });
      
        dialogRef.componentInstance.onYes.subscribe(() => {
         this.addDSR();
        });
      }
  
      addDSR(): void {
        if(!this.loading){
          this.loading = true;
          const dateonly= this.form.value.reportDate as any;
          const request: TAddDSRRequest = {
            reportDate: moment(dateonly).format('YYYY-MM-DD'),
            taskAccomplished: this.form.value.taskAccomplished ?? '', 
            challengesFaced: this.form.value.challengesFaced ?? '', 
            nextDayPlan: this.form.value.nextDayPlan ?? '',
          };

        this.service
          .addDSR(request)
          .subscribe({
            next: (respone) => {this.openSuccessOrErrorModal(respone.message, 'Success');},
            error: (response) => {this.openSuccessOrErrorModal(response.message, 'Error');},
            complete: () => {
              this.loading = false;
            }
          });
        }
        
      }

      openSuccessOrErrorModal(message: string, check: string): void {
        const dialogRef = this.dialog.open(ModelComponent, {
          width: '500px',
          data: { message: message, check: check }
        });
    
        dialogRef.afterClosed().subscribe(() => {
          console.log('The dialog was closed');
        });
      }
}
