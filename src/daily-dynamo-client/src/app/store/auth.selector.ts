import { createSelector } from '@ngrx/store';
import { TAppState } from '../types/app-state.type';

const selectFeature = (state: TAppState) => state.auth;

export const selectAuth = createSelector(selectFeature, state => state.user);
export const selectAuthStatus = createSelector(
  selectFeature,
  state => state.status
);

export const selectUserRole = createSelector(
  selectFeature,
  state => state.user?.userRole
);

export const selectUserId = createSelector(
  selectFeature,
  state => state.user?.id
);

export const selectUserName = createSelector(
  selectFeature,
  state => state.user?.userName
);

