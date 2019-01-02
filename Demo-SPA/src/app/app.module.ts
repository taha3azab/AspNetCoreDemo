import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import {
  AngularPerfModule,
  RoutingService
} from '@microsoft/mezzurite-angular';
import { BsDropdownModule } from 'ngx-bootstrap';

import { AppComponent } from './app.component';
import { ValuesComponent } from './values/values.component';
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { MemberListComponent } from './member-list/member-list.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';

import { ErrorInterceptorProvider } from './_interceptors/error.interceptor';
import { AuthService } from './_services/auth.service';
import { AlertifyService } from './_services/alertify.service';
import { AuthGuard } from './_guards/auth.guard';
import { JwtModule } from '@auth0/angular-jwt';
import { ContactFormComponent } from './contact-form/contact-form.component';
import { SignupFormComponent } from './signup-form/signup-form.component';
import { LoggingInterceptor, LoggingInterceptorProvider } from './_interceptors/logging-interceptor';
import { AppErrorHandler, AppErrorHandlerProvider } from './common/app-error-handler';
import { ValueService } from './_services/value.service';
import { UsersComponent } from './users/users.component';
import { UsersService } from './_services/users.service';

@NgModule({
  declarations: [
    AppComponent,
    ValuesComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MemberListComponent,
    ListsComponent,
    MessagesComponent,
    ContactFormComponent,
    SignupFormComponent,
    UsersComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    AngularPerfModule.forRoot(),
    BsDropdownModule.forRoot(),
    JwtModule.forRoot({
      // https://github.com/auth0/angular2-jwt
      config: {
        tokenGetter: () => {
          return localStorage.getItem('token');
        },
        blacklistedRoutes: [new RegExp('\/api\/auth')],
        whitelistedDomains: ['localhost:5001']
      }
    })
  ],
  providers: [
    ErrorInterceptorProvider,
    LoggingInterceptorProvider,
    ValueService,
    UsersService,
    AuthService,
    AlertifyService,
    AuthGuard,
    AppErrorHandlerProvider
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(private router: RoutingService) {
    router.start();
  }
}
