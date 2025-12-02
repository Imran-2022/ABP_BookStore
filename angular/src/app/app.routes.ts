import { authGuard, permissionGuard } from '@abp/ng.core';
import { Routes } from '@angular/router';

export const APP_ROUTES: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadComponent: () => import('./home/home.component').then(c => c.HomeComponent),
  },
  {
    path: 'books',
    pathMatch: 'full',
    loadComponent: () => import('./book/book').then(c => c.Book),
    //  ADD GUARDS AND POLICY HERE
    canActivate: [authGuard, permissionGuard],
    data: {
      // Use the base permission you defined in the C# backend
      // requiredPolicy: 'BookStore.Books.Default', this not correct. as --
      requiredPolicy: 'BookStore.Books',
      // redirectPath: '/', this redirect not work.
    },
  },
  {
    path: 'account',
    loadChildren: () => import('@abp/ng.account').then(c => c.createRoutes()),
  },
  {
    path: 'identity',
    loadChildren: () => import('@abp/ng.identity').then(c => c.createRoutes()),
  },
  {
    path: 'tenant-management',
    loadChildren: () => import('@abp/ng.tenant-management').then(c => c.createRoutes()),
  },
  {
    path: 'setting-management',
    loadChildren: () => import('@abp/ng.setting-management').then(c => c.createRoutes()),
  },
];
