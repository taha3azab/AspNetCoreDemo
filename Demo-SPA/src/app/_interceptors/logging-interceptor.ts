import { finalize, tap } from 'rxjs/operators';
// import { MessageService } from '../message.service';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpResponse, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable()
export class LoggingInterceptor implements HttpInterceptor {
  // constructor(private messenger: MessageService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    const started = Date.now();
    let ok: string;

    // extend server response observable with logging
    return next.handle(req)
      .pipe(
        tap(
          // Succeeds when there is a response; ignore other events
          event => ok = event instanceof HttpResponse ? 'succeeded' : '',
          // Operation failed; error is an HttpErrorResponse
          error => ok = 'failed'
        ),
        // Log when response observable either completes or errors
        finalize(() => {
          const elapsed = Date.now() - started;
          const msg = `${req.method} "${req.urlWithParams}"
             ${ok} in ${elapsed} ms.`;
          // this.messenger.add(msg);
          console.log(msg);
        })
      );
  }
}
export const LoggingInterceptorProvider = {
  provide: HTTP_INTERCEPTORS,
  useClass: LoggingInterceptor,
  multi: true
};

/*
Logging
Because interceptors can process the request
and response together, they can do things
like time and log an entire HTTP operation.

Consider the following LoggingInterceptor,
which captures the time of the request,
the time of the response, and logs the outcome with
the elapsed time with the injected MessageService.
*/
