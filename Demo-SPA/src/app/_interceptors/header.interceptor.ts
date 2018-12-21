// The HttpInterceptor is an interface and used to implement the intercept method.
import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class HttpHeaderInterceptor implements HttpInterceptor {
  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const token = localStorage.getItem('token');

    // if (!req.url.includes('/api/auth/')) {
    //   const reqHeader = req.clone({
    //     headers: req.headers.set('Authorization', 'Bearer ' + token)
    //   });
    //   return next.handle(reqHeader);
    // }
    console.log(req);
    return next.handle(req);
  }
}
