import { HttpErrorResponse } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { HotToastService } from '@ngneat/hot-toast';

@Injectable({
  providedIn: 'root',
})
export class ErrorHandlingService {
  toaster: HotToastService = inject(HotToastService);

  handleHttpError(error: HttpErrorResponse) {
    // we can use this to log http errors
    console.log(error);
  }
}
