import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './dashboard.component';
import { UsersComponent } from './pages/users/users.component';
import { UserprofileComponent } from './pages/userprofile/userprofile.component';
import { UpdateUserprofileComponent } from './components/update-userprofile/update-userprofile.component';
import { UserAccountSettingsComponent } from './components/user-accountsettings/user-accountsettings.component';
import { MAT_IMPORTS } from './mat.imports';
import { UserCardComponent } from './components/user-card/user-card.component';
import { SharedModule } from '../shared/shared.module';
import { UserCardSkeletonComponent } from './components/skeletons/user-card-skeleton/user-card-skeleton.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HomeComponent } from './pages/home/home.component';
import { UserDSRComponent } from './pages/user-dsr/user-dsr.component';
import { UserDSRFormComponent } from './components/user-dsr-form/user-dsr-form.component';
import { AnnoucementsComponent } from './components/annoucements/annoucements.component';
import { CalenderComponent } from './components/calender/calender.component';
import { EmployeeTableComponent } from './components/employee-table/employee-table.component';
import { DsrDetailsComponent } from './components/dsr-details/dsr-details.component';
import { ViewSelectedDSRModelComponent } from './components/view-dsr-model/view-dsr-model.component';

@NgModule({
  declarations: [
    DashboardComponent,
    UsersComponent,
    UserCardComponent,
    UserprofileComponent,
    UpdateUserprofileComponent,
    UserAccountSettingsComponent,
    UserCardSkeletonComponent,
    AnnoucementsComponent,
    CalenderComponent,
    HomeComponent,
    UserDSRFormComponent,
    EmployeeTableComponent,
    UserDSRComponent,
    DsrDetailsComponent,
    ViewSelectedDSRModelComponent,
  ],
  imports: [
    CommonModule,
    DashboardRoutingModule,
    ...MAT_IMPORTS,
    SharedModule,
    ReactiveFormsModule,
  ],
})
export class DashboardModule {}
