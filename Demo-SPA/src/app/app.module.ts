import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule} from '@angular/http';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ValuesComponent } from './values/values.component';
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';

import { ErrorInterceptorProvider } from './services/error.interceptor';
import { WebService } from './services/web.service';
import { AuthService } from './services/auth.service';

@NgModule({
  declarations: [AppComponent, ValuesComponent, NavComponent, HomeComponent, RegisterComponent],
  imports: [BrowserModule, HttpModule, FormsModule, AppRoutingModule],
  providers: [ErrorInterceptorProvider, WebService, AuthService],
  bootstrap: [AppComponent]
})
export class AppModule {}
