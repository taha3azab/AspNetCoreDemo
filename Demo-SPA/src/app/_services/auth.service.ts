import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { JwtHelperService } from '@auth0/angular-jwt';
import 'rxjs/add/operator/delay';
import 'rxjs/add/observable/throw';
import { map } from 'rxjs/operators/map';
import { UserForRegister } from '../shared/models/user-for-register.model';
import { Observable } from 'rxjs';
import { BadInput } from '../common/bad-input';
import { AppError } from '../common/app-error';
import { LocalStorageService } from 'ngx-webstorage';

@Injectable()
export class AuthService {
  baseUrl = 'https://localhost:5001/api/auth/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;

  constructor(private https: HttpClient, private storage: LocalStorageService) {}

  login(model: any) {
    return this.https.post(this.baseUrl + 'login', model).pipe(
      map((response: TokenResponse) => {
        if (response) {
          this.storage.store('token', response.token);
          // this.decodedToken = this.jwtHelper.decodeToken(response.token);
          // console.log(this.decodedToken);
        }
      })
    );
  }

  signup(model: UserForRegister) {
    return this.https
      .post<UserForRegister>(this.baseUrl + 'register', model)
      .catch((error: Response) => {
        if (error.status === 400) {
          return Observable.throw(new BadInput(error.json()));
        }
        return Observable.throw(new AppError(error.json()));
      });
  }

  changePassword(model: any) {
    return this.https.post(this.baseUrl + 'change_password', model);
  }

  loggedIn() {
    const token = this.storage.retrieve('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

  userIsExist(username: string) {
    return this.https.get<Boolean>(this.baseUrl + 'user_is_exist', {
      params: { username: username },
      headers: new HttpHeaders({
        // 'Cache-control': 'no-cache',
        'Cache-control': 'no-store',
        Expires: '0',
        Pragma: 'no-cache'
      })
    });
  }
}

export interface TokenResponse {
  token: string;
}
