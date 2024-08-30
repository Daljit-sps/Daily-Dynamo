import { inject, Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { ELocalStorage } from 'src/app/features/auth/types/local-storage.enum';
import { Router, RouterPreloader } from '@angular/router';
import { HotToastService } from '@ngneat/hot-toast';
import { TUser } from 'src/app/types/user.type';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor() {}
  ls: LocalStorageService = inject(LocalStorageService);
  router: Router = inject(Router);
  toaster: HotToastService = inject(HotToastService);

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    const user: TUser = this.ls.getItem(ELocalStorage.User);
    console.log(user);
    if (user) {
      if (new Date(user?.tokenValidity).getTime() < new Date().getTime()) {
        this.handleExpiredSession();
        return of();
      }

      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${user?.token}`,
        },
      });
    }
    return next.handle(request);
  }

  private handleExpiredSession() {
    this.toaster.error('Your session has expired! Please login again.', {
      duration: 5000,
    });
    this.ls.removeItem(ELocalStorage.User);
    this.router.navigate(['/auth']);
  }
}
