import {
  BrowserCacheLocation,
  InteractionType,
  IPublicClientApplication,
  PublicClientApplication
} from '@azure/msal-browser';

import {
  MsalGuardConfiguration,
  MsalInterceptorConfiguration
} from '@azure/msal-angular';

export const msalInstance: IPublicClientApplication =
  new PublicClientApplication({
    auth: {
      clientId: 'e3a5eeb7-00e4-4090-a62f-9d4f3cc87abf',

      authority:
        'https://login.microsoftonline.com/organizations',

      redirectUri:
        'http://localhost:4200',

      postLogoutRedirectUri:
        'http://localhost:4200'
    },

    cache: {
      cacheLocation:
        BrowserCacheLocation.LocalStorage
    }
  });


export const msalGuardConfig:
  MsalGuardConfiguration = {

  interactionType:
    InteractionType.Redirect,

  authRequest: {
    scopes: [
      'api://7af1cd49-f408-4e44-8c4f-bbc2f7b2e241/access_as_user'
    ]
  }
};


export const msalInterceptorConfig:
  MsalInterceptorConfiguration = {

  interactionType:
    InteractionType.Redirect,

  protectedResourceMap: new Map([
    [
      'http://localhost:5137/api/',
      [
        'api://7af1cd49-f408-4e44-8c4f-bbc2f7b2e241/access_as_user'
      ]
    ]
  ])
};