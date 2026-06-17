import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/login/login.component')
        .then(m => m.LoginComponent)
  },
  {
    path: '',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./layout/shell/shell.component')
        .then(m => m.ShellComponent),
    children: [
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./features/dashboard/dashboard.component')
            .then(m => m.DashboardComponent)
      },
      {
        path: 'customer',
        loadComponent: () =>
          import('./features/customer/customer.component')
            .then(m => m.CustomerComponent)
      },
      {
        path: 'chit-group',
        loadComponent: () =>
          import('./features/chit-group/chit-group.component')
            .then(m => m.ChitGroupComponent)
      },
      {
        path: 'payment',
        loadComponent: () =>
          import('./features/payment/payment.component')
            .then(m => m.PaymentComponent)
      },
      {
        path: 'enrollment',
        loadComponent: () =>
          import('./features/enrollment/enrollment.component')
            .then(m => m.EnrollmentComponent)
      },
      // {
      //   path: 'winner',
      //   loadComponent: () =>
      //     import('./features/winner/winner.component')
      //       .then(m => m.WinnerComponent)
      // }
    ]
  },
  {
    path: '**',
    redirectTo: 'login'
  }
];
