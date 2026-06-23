import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { ToastService } from '../services/toast.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const toastService = inject(ToastService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      switch (error.status) {
        case 401:
          toastService.error('Session expired! Please login again.');
          localStorage.clear();
          router.navigate(['/login']);
          break;

        case 403:
          toastService.error('You do not have permission to perform this action!');
          break;

        case 404:
          toastService.error('Requested resource not found!');
          break;

        case 500:
          toastService.error('Server error! Please try again later.');
          break;

        case 0:
          toastService.error('Cannot connect to server! Check your network.');
          break;

        default:
          toastService.error(error.error?.message ?? 'Something went wrong!');
      }

      return throwError(() => error);
    })
  );
};
