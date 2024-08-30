import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable, map, tap } from 'rxjs';
import { selectAuth } from 'src/app/store/auth.selector';
import { TAppState } from 'src/app/types/app-state.type';

export const AuthGuard: CanActivateFn = (route, state): Observable<boolean> => {
  const store = inject(Store<TAppState>);
  const auth = store.select(selectAuth);
  const router = inject(Router);

  return auth
    .pipe(
      tap(user => {
        if (user == null) router.navigate(['/auth']);
        return;
      })
    )
    .pipe(map(user => user != null));
};
