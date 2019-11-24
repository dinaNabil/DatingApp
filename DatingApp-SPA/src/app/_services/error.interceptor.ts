import { Injectable } from "@angular/core";
import {
  HttpInterceptor,
  HttpRequest,
  HttpErrorResponse,
  HTTP_INTERCEPTORS,
  HttpHandler,
  HttpEvent
} from '@angular/common/http';
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError(error => {
        if (error instanceof HttpErrorResponse) {
            if(error.status === 401) {
                return throwError(error.statusText);
            }
           const applicationError = error.headers.get("Application-Error");
           if (applicationError) {
              console.log(applicationError);
              return throwError(applicationError);
           }
          const serverError = error.error;
          let modalStateErrors = '';
          if (serverError && typeof serverError === 'object') {
            for(const key in serverError) {
              
              if (serverError[key] && key==='errors') {
               // console.log(serverError[key]);
                for (const value in serverError[key]) {
                    if (value) {
                        for (const message in serverError[key][value]) {
                       // console.log(serverError[key][value][message]);
                        modalStateErrors += serverError[key][value][message]+ '\n';
                        }
                    }
                }
               
                
              }
            }
          }
          return throwError(modalStateErrors || serverError || 'serverError');
        }
      })
    );
  }
}
export const ErrorInterceptorProvider = {
  provide: HTTP_INTERCEPTORS,
  useClass: ErrorInterceptor,
  multi: true
};
