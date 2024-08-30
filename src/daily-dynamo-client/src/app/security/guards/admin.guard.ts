import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { Store } from '@ngrx/store';
import { map, tap } from 'rxjs';
import { UserRole } from 'src/app/features/shared/types/role.enum';
import { logOut } from 'src/app/store/auth.actions';
import { selectUserRole } from 'src/app/store/auth.selector';
import { TAppState } from 'src/app/types/app-state.type';

export const AdminGuard: CanActivateFn = (route, state) => {
  const store = inject(Store<TAppState>);
  const userRole = store.select(selectUserRole);

  return userRole
    .pipe(
      tap(role => {
        if (role != UserRole.Admin) store.dispatch(logOut());
      })
    )
    .pipe(map(role => role == UserRole.Admin));
};
