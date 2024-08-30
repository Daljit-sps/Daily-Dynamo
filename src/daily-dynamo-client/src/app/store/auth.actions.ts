import { createAction, props } from '@ngrx/store';
import { TAuth } from '../types/user.type';
import { TSignInRequest } from '../features/auth/types/login.type';

export const login = createAction('[Auth] Login', props<TSignInRequest>());
export const logOut = createAction('[Auth] Logout');
export const checkForToken = createAction('[Auth] CheckForToken');
export const loginSuccess = createAction('[Auth] LoginSuccess', props<TAuth>());
export const setState = createAction('[Auth] setState', props<TAuth>());
export const loginFailure = createAction(
  '[Auth] LoginFaliure',
  props<{ message: string }>()
);
