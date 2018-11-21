import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { map } from 'rxjs/operators';
@Injectable()
export class AuthService {
  baseUrl = 'https://localhost:5001/api/auth/';

  constructor(private https: Http) {}

  login(model: any) {
    return this.https.post(this.baseUrl + 'login', model).pipe(
      map((response: any) => {
        const user = response.json();
        if (user) {
          localStorage.setItem('token', user.token);
        }
      })
    );
  }

  register(model: any) {
    return this.https.post(this.baseUrl + 'register', model);
  }
}
