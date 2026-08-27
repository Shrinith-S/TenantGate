import {
  Component,
  OnInit,
  inject,
  signal
} from '@angular/core';

import {
  RouterOutlet
} from '@angular/router';

import {
  MsalService
} from '@azure/msal-angular';

@Component({
  imports: [RouterOutlet],
  selector: 'app-root',
  styleUrl: './app.scss',
  templateUrl: './app.html'
})
export class App implements OnInit {

  private readonly msal = inject(MsalService);

  protected readonly title = signal('tenant-gate-web');

  ngOnInit(): void {

    this.msal
      .handleRedirectObservable()
      .subscribe({

        next: (result) => {

          console.log(
            'MSAL redirect result:',
            result
          );

          if (result?.account) {

            this.msal.instance.setActiveAccount(
              result.account
            );

          }

        },

        error: (error) => {

          console.error(
            'MSAL redirect error:',
            error
          );

        }

      });
  }
}