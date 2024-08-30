import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard.component';
import { UsersComponent } from './pages/users/users.component';
import { LayoutComponent } from '../shared/components/layout/layout.component';
import { AdminGuard } from 'src/app/security/guards/admin.guard';
import { UserprofileComponent } from './pages/userprofile/userprofile.component';
import { UpdateUserprofileComponent } from './components/update-userprofile/update-userprofile.component';
import { UserAccountSettingsComponent } from './components/user-accountsettings/user-accountsettings.component';
import { HomeComponent } from './pages/home/home.component';
import { UserDSRComponent } from './pages/user-dsr/user-dsr.component';
import { UserDSRFormComponent } from './components/user-dsr-form/user-dsr-form.component';
import { DsrDetailsComponent } from './components/dsr-details/dsr-details.component';

const routes: Routes = [
  {
    path: '',
    component: DashboardComponent,
    children: [
      {
        path: '',
        component: LayoutComponent,
        children: [
          {
            // canActivate: [AdminGuard],
            path: 'users',
            component: UsersComponent,
          },
          { path: '', redirectTo: 'home', pathMatch: 'full' },
          { path: 'home', component: HomeComponent },
          {
            path: 'userprofile',
            component: UserprofileComponent,
            children: [
              {
                path: 'update-userprofile',
                component: UpdateUserprofileComponent,
              },
              {
                path: 'user-accountsettings',
                component: UserAccountSettingsComponent,
              },
            ],
          },
          {
            path: 'user-dsr',
            component: UserDSRComponent,
            children: [
              { path: 'user-dsr-form', component: UserDSRFormComponent },
              { path: ':id', component: DsrDetailsComponent },
            ],
          },
        ],
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DashboardRoutingModule {}
