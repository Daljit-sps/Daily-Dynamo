import { ApplicationInitStatus, inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { Store } from '@ngrx/store';
import { checkForToken } from 'src/app/store/auth.actions';
import { TAppState } from 'src/app/types/app-state.type';

export const authCheckGuard: CanActivateFn = (route, state) => {
  const store = inject(Store<TAppState>);
  store.dispatch(checkForToken());
  return true;
};
