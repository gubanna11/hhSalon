import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CreateGroupComponent } from './components/groups/create-group/create-group.component';
import { GroupsListComponent } from './components/groups/groups-list/groups-list.component';
import { GroupsMenuComponent } from './components/groups-menu/groups-menu.component';
import { SharedService } from './services/shared.service';
import { UpdateGroupComponent } from './components/groups/update-group/update-group.component';
import { ServicesListComponent } from './components/services/services-list/services-list.component';
import { CreateServiceComponent } from './components/services/create-service/create-service.component';
import { UpdateServiceComponent } from './components/services/update-service/update-service.component';
import { LoginComponent } from './components/login/login.component';
import { SignUpComponent } from './components/sign-up/sign-up.component';
import { CreateAttendanceComponent } from './components/attendances/create-attendance/create-attendance.component';
import { TokenInterceptor } from './interceptors/token.interceptor';
import { UsersListComponent } from './components/users/users-list/users-list.component';
import { HeaderComponent } from './components/header/header.component';
import { AdminCreateComponent } from './components/admin-create/admin-create.component';
import { MyNotRenderedAttendancesComponent } from './components/attendances/my-not-rendered-attendances/my-not-rendered-attendances.component';
import { MyHistoryComponent } from './components/attendances/my-history/my-history.component';
import { WorkerHistoryComponent } from './components/attendances/worker-history/worker-history.component';
import { WorkerNotRenderedAttendancesComponent } from './components/attendances/worker-not-rendered-attendances/worker-not-rendered-attendances.component';
import { WorkerScheduleComponent } from './components/workers/worker-schedule/worker-schedule.component';
import { WorkerCreateComponent } from './components/workers/worker-create/worker-create.component';


import { EnumToArrayPipe } from './pipes/enum-to-array.pipe';
import { TimeFromDatePipe } from './pipes/time.pipe';

import { WorkersListComponent } from './components/workers/workers-list/workers-list.component';
import { WorkerEditComponent } from './components/workers/worker-edit/worker-edit.component';
import { ChatComponent } from './components/chat/chat.component';
import { MessagesComponent } from './components/messages/messages.component';
import { ChatInputComponent } from './components/chat-input/chat-input.component';
import { UpdateAttendanceComponent } from './components/attendances/update-attendance/update-attendance.component';
import { TimeStringPipe } from './pipes/time-string.pipe';
import { NgToastComponent, NgToastModule } from 'ng-angular-popup';
//import * as toastr from 'toastr';
import { UserEditComponent } from './components/users/user-edit/user-edit.component';
import { AllAttendancesComponent } from './components/attendances/all-attendances/all-attendances.component';



@NgModule({
  declarations: [
    AppComponent,
    CreateGroupComponent,
    GroupsListComponent,
    GroupsMenuComponent,
    UpdateGroupComponent,
    ServicesListComponent,
    CreateServiceComponent,
    UpdateServiceComponent,
    LoginComponent,
    SignUpComponent,
    CreateAttendanceComponent,
    UsersListComponent,
    HeaderComponent,
    AdminCreateComponent,
    WorkerCreateComponent,
    MyNotRenderedAttendancesComponent,
    MyHistoryComponent,
    WorkerHistoryComponent,
    WorkerNotRenderedAttendancesComponent,
    WorkerScheduleComponent,

    EnumToArrayPipe,
    TimeFromDatePipe,

    WorkersListComponent,
    WorkerEditComponent,
    ChatComponent,
    MessagesComponent,
    ChatInputComponent,
    UpdateAttendanceComponent,
    TimeStringPipe,
    UserEditComponent,
    AllAttendancesComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,    
    FormsModule,    
    ReactiveFormsModule,    
    NgToastModule
  ],
  providers: [SharedService, 
  {
    provide: HTTP_INTERCEPTORS,
    useClass: TokenInterceptor,
    multi: true
  }],
  bootstrap: [AppComponent]

})
export class AppModule { }
