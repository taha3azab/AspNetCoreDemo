
// import { Injectable } from '@angular/core';
// import { HttpInterceptor, HttpRequest, HttpHandler } from '@angular/common/http';
// import { of } from 'rxjs';

// @Injectable()
// export class CachingInterceptor implements HttpInterceptor {
//   constructor(private cache: RequestCache) {}

//   intercept(req: HttpRequest<any>, next: HttpHandler) {
//     // continue if not cachable.
//     if (!isCachable(req)) { return next.handle(req); }

//     const cachedResponse = this.cache.get(req);
//     return cachedResponse ?
//       of(cachedResponse) : sendRequest(req, next, this.cache);
//   }
// }
/*
Caching
Interceptors can handle requests by themselves, without forwarding to next.handle().

For example, you might decide to cache certain requests and responses to
improve performance. You can delegate caching to an interceptor without
disturbing your existing data services.

The CachingInterceptor demonstrates this approach.
*/
