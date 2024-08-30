import { createReducer, on } from '@ngrx/store';
import { EAuthStatus, TAuth, TUser } from '../types/user.type';
import * as AuthActions from './auth.actions';

export const initialState: TAuth = {
  user: null,
  status: EAuthStatus.Pending,
  message: '',
};

export const authReducer = createReducer(
  initialState,
  on(AuthActions.login, state => ({ ...state, status: EAuthStatus.Loading })),
  on(AuthActions.loginSuccess, (_state, { user }) => ({
    user: user,
    status: EAuthStatus.Pending,
    message: 'Login successful',
  })),
  on(AuthActions.setState, (_state, { user }) => ({
    user: user,
    status: EAuthStatus.Pending,
    message: 'Login successful',
  })),
  on(AuthActions.loginFailure, (_state, { message }) => ({
    user: null,
    status: EAuthStatus.Error,
    message: message,
  })),
  on(AuthActions.logOut, () => ({
    user: null,
    message: '',
    status: EAuthStatus.Pending,
  }))
);
