import { Injectable, inject } from '@angular/core';
import { Actions, act, createEffect, ofType } from '@ngrx/effects';
import * as AuthActions from './auth.actions';
import {
  catchError,
  map,
  mergeMap,
  of,
  switchMap,
  tap,
  withLatestFrom,
} from 'rxjs';
import { AuthService } from '../features/auth/services/auth.service';
import { HotToastService } from '@ngneat/hot-toast';
import { Router } from '@angular/router';
import { Store, select } from '@ngrx/store';
import { TAppState } from '../types/app-state.type';
import { HttpErrorResponse } from '@angular/common/http';
import { LocalStorageService } from '../services/local-storage.service';
import { ELocalStorage } from '../features/auth/types/local-storage.enum';
import { EAuthStatus, TAuth, TUser } from '../types/user.type';
// import { selectAuthToken, selectDecryptedToken } from './auth.selector';
import { decryptToken } from '../utilities/jwt-utils';

@Injectable({ providedIn: 'root' })
export class AuthEffectsService {
  actions$: Actions = inject(Actions);
  authService: AuthService = inject(AuthService);
  toaster: HotToastService = inject(HotToastService);
  router: Router = inject(Router);
  store: Store = inject(Store<TAppState>);
  localStorage: LocalStorageService = inject(LocalStorageService);

  readonly loginSuccessEffect$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.loginSuccess),
        mergeMap(action =>
          of([]).pipe(
            tap(async () => {
              this.toaster.success('Welcome to DD');
              this.localStorage.setItem(ELocalStorage.User, action.user);
              this.router.navigate(['/dashboard']);
            })
          )
        )
      ),
    { dispatch: false }
  );

  readonly loginFaliureEffect$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.loginFailure),
        mergeMap(action =>
          of([]).pipe(
            tap(() => {
              this.localStorage.removeItem(ELocalStorage.User);
              this.toaster.error(action.message);
            })
          )
        )
      ),
    { dispatch: false }
  );

  readonly checkForTokenEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.checkForToken),
      switchMap(() => {
        const token: string | null = this.localStorage.getItem(
          ELocalStorage.User
        );
        console.log(token);
        if (!token) return [];
        const authState: TAuth = {
          message: '',
          status: EAuthStatus.Pending,
          user: token as any as TUser,
        };
        return of(AuthActions.setState(authState));
      })
    )
  );

  readonly onLogoutEffect$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.logOut),
        mergeMap(() =>
          of([]).pipe(
            tap(() => {
              this.localStorage.removeItem(ELocalStorage.User);
              this.router.navigate(['/auth']);
            })
          )
        )
      ),
    { dispatch: false }
  );

  loginEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.login),
      switchMap(login => {
        this.localStorage.removeItem(ELocalStorage.User);
        return this.authService.login(login).pipe(
          map(response => {
            const user: TUser | null = decryptToken(response ?? '');
            if (user) {
              this.localStorage.setItem(
                ELocalStorage.User,
                JSON.stringify(user)
              );
              const authState: TAuth = {
                message: '',
                status: EAuthStatus.Pending,
                user: user,
              };
              return AuthActions.loginSuccess(authState);
            } else {
              return AuthActions.loginFailure({ message: response.message });
            }
          }),
          catchError((error: HttpErrorResponse) => {
            return of(AuthActions.loginFailure({ message: error.statusText }));
          })
        );
      })
    )
  );
}
