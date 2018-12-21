import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class AuthService {
  baseUrl = 'https://localhost:5001/api/auth/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;

  constructor(private https: HttpClient) {}

  login(model: any) {
    return this.https.post(this.baseUrl + 'login', model).pipe(
      map((response: TokenResponse) => {
        if (response) {
          localStorage.setItem('token', response.token);
          this.decodedToken = this.jwtHelper.decodeToken(response.token);
          console.log(this.decodedToken);
        }
      })
    );
  }

  register(model: any) {
    return this.https.post(this.baseUrl + 'register', model);
  }

  changePassword(model: any) {
    return this.https.post(this.baseUrl + 'change_password', model);
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }
}

export interface TokenResponse {
  token: string;
}
