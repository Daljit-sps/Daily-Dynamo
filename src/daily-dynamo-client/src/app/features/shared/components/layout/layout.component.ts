import {
  Component,
  NgModule,
  OnDestroy,
  OnInit,
  ViewChild,
  inject,
} from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { BreakpointObserver, BreakpointState } from '@angular/cdk/layout';
import { Observable, Subscription, take } from 'rxjs';
import { UserRole } from '../../types/role.enum';
import { Store } from '@ngrx/store';
import { TAppState } from 'src/app/types/app-state.type';
import { logOut } from 'src/app/store/auth.actions';
import { selectUserRole } from 'src/app/store/auth.selector';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss'],
})
export class LayoutComponent implements OnInit, OnDestroy {
  isMobile: boolean = false;
  subscriptions: Subscription[] = [];
  userRole: UserRole = UserRole.Employee;
  @ViewChild(MatSidenav) sidenav!: MatSidenav;
  screenObserrvable!: Observable<BreakpointState>;
  store: Store<TAppState> = inject(Store<TAppState>);
  observer: BreakpointObserver = inject(BreakpointObserver);

  navLinks = [
    {
      label: 'Dashboard',
      link: 'home',
      permission: [],
      icon: 'apps',
    },
    {
      label: 'Users',
      link: 'users',
      permission: [UserRole.Employee],
      icon: 'group',
    },
    {
      label: 'DSR',
      link: '/dashboard/user-dsr/user-dsr-form',
      permission: [UserRole.Admin],
      icon: 'assignment',
    },
  ];

  ngOnInit(): void {
    this.observer.observe(['(max-width: 500px)']).subscribe(screen => {
      if (screen.matches) this.isMobile = true;
      else this.isMobile = false;
    });
    let role: UserRole | undefined = undefined;
    this.store
      .select(selectUserRole)
      .pipe(take(1))
      .subscribe(rle => (role = rle));
    if (!role) return;
    this.userRole = <UserRole>role;
  }

  signOut(): void {
    this.store.dispatch(logOut());
  }

  ngOnDestroy(): void {
    if (this.subscriptions.length > 0)
      this.subscriptions.map(subscription => {
        subscription.unsubscribe();
      });
  }

  testFunc(arr: any[]): boolean{
    return arr.includes(Number(this.userRole));
  }
}
