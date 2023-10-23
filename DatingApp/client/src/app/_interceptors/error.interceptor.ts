import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpResponse,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toast: ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError( (err: HttpErrorResponse) => {
        if (err) {
          switch (err.status) {
            case 400:
              if (err.error.errors) {
                const modelStateErrors = [];
                for (const key in err.error.errors){
                  if (err.error.errors[key]) {
                    modelStateErrors.push(err.error.errors[key]);
                  }
                }
                throw modelStateErrors.flat();
              }
              else{
                this.toast.error(err.error + " Statuscode: " + err.status);
              }
              break;
            case 401:
              this.toast.error('Unauthorized' + " Statuscode: " + err.status);
              break;
            case 404:
              this.router.navigateByUrl('/not-found');
              break;
            case 500:
              const navigationExtras: NavigationExtras = {state: {error: err.error}};
              this.router.navigateByUrl('/server-error', navigationExtras);
              break;          
            default:
              this.toast.error('I didnt see this coming');
              console.log(err);
              break;
          }
        }
        throw err;
      } )
    )
  }
}
