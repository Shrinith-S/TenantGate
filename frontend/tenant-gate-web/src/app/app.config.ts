import {
  ApplicationConfig,
  importProvidersFrom
} from '@angular/core';

import {
  provideRouter
} from '@angular/router';

import {
  provideHttpClient,
  withInterceptorsFromDi
} from '@angular/common/http';

import {
  routes
} from './app.routes';

import {
  MsalModule,
  MSAL_INSTANCE,
  MSAL_GUARD_CONFIG,
  MSAL_INTERCEPTOR_CONFIG,
  MsalGuard,
  MsalInterceptor,
  MsalService,
  MsalBroadcastService
} from '@azure/msal-angular';

import {
  HTTP_INTERCEPTORS
} from '@angular/common/http';

import {
  msalInstance,
  msalGuardConfig,
  msalInterceptorConfig
} from './auth-config';

export const appConfig: ApplicationConfig = {

  providers: [

    provideRouter(routes),

    provideHttpClient(
      withInterceptorsFromDi()
    ),

    importProvidersFrom(
      MsalModule
    ),

    {
      provide: MSAL_INSTANCE,
      useValue: msalInstance
    },

    {
      provide: MSAL_GUARD_CONFIG,
      useValue: msalGuardConfig
    },

    {
      provide: MSAL_INTERCEPTOR_CONFIG,
      useValue: msalInterceptorConfig
    },

    {
      provide: HTTP_INTERCEPTORS,
      useClass: MsalInterceptor,
      multi: true
    },

    MsalService,

    MsalGuard,

    MsalBroadcastService
  ]
};