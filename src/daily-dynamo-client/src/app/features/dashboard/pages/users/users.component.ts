import { Component, OnInit, inject } from '@angular/core';
import { TUser } from '../../types/user.type';
import { UserService } from '../../services/user.service';
import { Observable, catchError, ignoreElements, map, of, tap } from 'rxjs';
import { Router } from '@angular/router';


@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss'],
})
export class UsersComponent implements OnInit {
  service: UserService = inject(UserService);
  router: Router = inject(Router);

  user$: Observable<TUser[] | null> = of(null);
  userError$: Observable<any> = this.user$.pipe(
    ignoreElements(),
    catchError(e => of(e))
  );

  ngOnInit(): void {
    this.user$ = this.service.getUsers().pipe(map(response => response.data));
  }

  redirectToUserProfile(userId: string): void {
    this.router.navigate(['/dashboard/userprofile'], { queryParams: { id: userId } });
  }
  
}
