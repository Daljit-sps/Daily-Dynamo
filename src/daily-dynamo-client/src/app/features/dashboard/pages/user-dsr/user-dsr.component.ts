import { Component, OnInit, inject } from '@angular/core';
import { DSRService } from '../../services/dsr.service';
import { TAddDSRRequest } from '../../types/add-dsr.type';
import { Store } from '@ngrx/store';
import { TAppState } from 'src/app/types/app-state.type';
import { selectUserId } from 'src/app/store/auth.selector';
import { take } from 'rxjs';
import * as moment from 'moment';
import { NonNullableFormBuilder, Validators } from '@angular/forms';
import { HotToastService } from '@ngneat/hot-toast';
import { TGetDSRForSelectedDatesForm, TGetDSRForSelectedDatesRequest } from '../../types/getDSR-for-selectedDates.type';
import { MatDialog } from '@angular/material/dialog';
import { ModelComponent } from 'src/app/features/shared/components/popup-model/popup-model.component';

@Component({
  selector: 'app-user-dsr',
  templateUrl: './user-dsr.component.html',
  styleUrls: ['./user-dsr.component.scss'],
})

export class UserDSRComponent implements OnInit{
    fb: NonNullableFormBuilder = inject(NonNullableFormBuilder);
    toaster: HotToastService = inject(HotToastService);
    service: DSRService = inject(DSRService);
    userDSRs: TAddDSRRequest[] = [];
    selectedDSR: TAddDSRRequest | null = null;
    store: Store<TAppState> = inject(Store<TAppState>);
    dialog: MatDialog = inject(MatDialog);
    
    userId: string | undefined;
    hideAddButton = false;
    loading = false;

    ngOnInit(): void {
        this.store
      .select(selectUserId)
      .pipe(take(1))
      .subscribe(uId => {
        if (uId) {
          this.userId = uId;
          this.getUserDSRs(this.userId);
        }
      });
      }

    getUserDSRs(userId: string): void {
      console.log(userId)
      if(userId != null){
        this.service.getUserDSRs(userId).subscribe((data) => {
          console.log(data)
          this.userDSRs = data;
          // Check if today's date exists in the list of dates
      const today = moment().format('YYYY-MM-DD');
      const isTodayInList = this.userDSRs.some(dsr => dsr.reportDate === today);
      if (isTodayInList) {
        this.hideAddButton = true; // Set a flag to hide the button
      }
          // Set the first DSR as selected by default
          if (this.userDSRs.length > 0) {
            this.selectedDSR = this.userDSRs[0];
          }
          console.log(this.selectedDSR)
        });
      }
        
    }

    form = this.fb.group<TGetDSRForSelectedDatesForm>({
      fromDate: this.fb.control<string>('', [
        Validators.required,
      ]),
      toDate: this.fb.control<string>('', [
        Validators.required,
      ]),
    });

    get controls():  TGetDSRForSelectedDatesForm{
      return this.form.controls;
    }

    getDSRForSelectedDates(): void{
      if (this.form.invalid) {
        this.toaster.error('Form is invalid!');
        return;
      }
      this.loading = true;
      const fromdateonly= this.form.value.fromDate as any;
      const todateonly= this.form.value.toDate as any;
      const request: TGetDSRForSelectedDatesRequest = {
        fromDate: moment(fromdateonly).format('YYYY-MM-DD'),
        toDate: moment(todateonly).format('YYYY-MM-DD'),
      };

    this.service
      this.service.getDSRForSelectedDates(request)
      .subscribe({
        next: (respone) => {
          this.userDSRs = respone.data;
          
          // Check if today's date exists in the list of dates
          const today = moment().format('YYYY-MM-DD');
          const isTodayInList = this.userDSRs.some(dsr => dsr.reportDate === today);
          if (isTodayInList) {
            this.hideAddButton = true; 
          }
        
          if (this.userDSRs.length > 0) {
            this.selectedDSR = this.userDSRs[0];
          }
        },
        error: (response) => {this.openSuccessOrErrorModal(response.message, 'Error');},
        complete: () => {
              this.loading = false;
            }
      });

    }


    truncateText(text: string): string {
        const words = text.split(' ');
        if (words.length > 4) {
          return words.slice(0, 4).join(' ') + '...';
        }
        return text;
    }

    selectDSR(id: string): void {
      this.selectedDSR = this.userDSRs.find(dsr => dsr.id === id)|| null;
      console.log(this.selectedDSR)
    }

    resetFormAndShow(): void {
      this.selectedDSR = null;
      console.log(this.selectedDSR)
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
