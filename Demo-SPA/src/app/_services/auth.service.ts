import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { JwtHelperService } from '@auth0/angular-jwt';
import 'rxjs/add/operator/delay';
import { map } from 'rxjs/operators/map';
import { UserForRegister } from '../shared/models/user-for-register.model';

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

  signup(model: UserForRegister) {
    return this.https.post<UserForRegister>(this.baseUrl + 'register', model);
  }

  changePassword(model: any) {
    return this.https.post(this.baseUrl + 'change_password', model);
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

  userIsExist(username: string) {
    return this.https.get<Boolean>(this.baseUrl + 'user_is_exist', {
      params: { username: username }
    });
  }
}

export interface TokenResponse {
  token: string;
}
