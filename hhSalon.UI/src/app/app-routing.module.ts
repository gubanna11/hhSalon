import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateGroupComponent } from './components/groups/create-group/create-group.component';
import { CreateServiceComponent } from './components/services/create-service/create-service.component';
import { GroupsListComponent } from './components/groups/groups-list/groups-list.component';
import { LoginComponent } from './components/login/login.component';
import { ServicesListComponent } from './components/services/services-list/services-list.component';
import { SignUpComponent } from './components/sign-up/sign-up.component';
import { UpdateGroupComponent } from './components/groups/update-group/update-group.component';
import { AuthGuard } from './guards/auth.guard';
import { CreateAttendanceComponent } from './components/attendances/create-attendance/create-attendance.component';

import { AdminCreateComponent } from './components/admin-create/admin-create.component';
import { WorkerCreateComponent } from './components/workers/worker-create/worker-create.component';
import { MyNotRenderedAttendancesComponent } from './components/attendances/my-not-rendered-attendances/my-not-rendered-attendances.component';
import { MyHistoryComponent } from './components/attendances/my-history/my-history.component';
import { WorkerHistoryComponent } from './components/attendances/worker-history/worker-history.component';
import { WorkerNotRenderedAttendancesComponent } from './components/attendances/worker-not-rendered-attendances/worker-not-rendered-attendances.component';
import { WorkerScheduleComponent } from './components/workers/worker-schedule/worker-schedule.component';
import { WorkersListComponent } from './components/workers/workers-list/workers-list.component';
import { WorkerEditComponent } from './components/workers/worker-edit/worker-edit.component';
import { ChatComponent } from './components/chat/chat.component';
//import { UsersListComponent } from './components/users/users-list.component';
import { UsersListComponent } from './components/users/users-list/users-list.component';
import { UserEditComponent } from './components/users/user-edit/user-edit.component';

const routes: Routes = [];

@NgModule({
  imports: [RouterModule.forRoot([
      {path: 'groups', component: GroupsListComponent},
      {path: 'groups/create', component: CreateGroupComponent},
      {path: 'groups/edit/:groupId', component: UpdateGroupComponent},
      {path: 'services/:groupId/:groupName', component: ServicesListComponent},
      {path: 'services/create', component: CreateServiceComponent},
      {path: 'attendances/create', component: CreateAttendanceComponent, canActivate:[AuthGuard]},
      //{path: 'attendances/create', component: CreateAttendanceComponent},
      {path: 'my-not-rendered-attendances', component: MyNotRenderedAttendancesComponent},
      {path: 'my-history', component: MyHistoryComponent},
      {path: 'worker-history', component: WorkerHistoryComponent},
      {path: 'worker-not-rendered', component: WorkerNotRenderedAttendancesComponent},

      {path: 'login', component: LoginComponent},
      {path: 'sign-up', component: SignUpComponent},

      {path: 'users', component: UsersListComponent, canActivate:[AuthGuard]},

      {path: 'admin-create', component: AdminCreateComponent},
      {path: 'worker-create', component: WorkerCreateComponent},
      {path:'worker-schedule-create/:workerId', component:WorkerScheduleComponent},
      {path: 'workers', component: WorkersListComponent},
      {path: 'workers/edit/:workerId', component: WorkerEditComponent},

      {path: 'users/edit/:userId', component: UserEditComponent},

      {path: 'chat/:toUser', component: ChatComponent},
    ])
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
