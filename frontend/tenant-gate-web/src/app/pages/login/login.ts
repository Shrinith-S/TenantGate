import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MsalService } from '@azure/msal-angular';

@Component({
  selector: 'app-login',
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login implements OnInit {

  constructor(
    private msalService: MsalService,
    private router: Router
  ) {}

  ngOnInit(): void {

    this.msalService.handleRedirectObservable().subscribe({
      next: (result) => {

        console.log('MSAL redirect result:', result);

        const accounts = this.msalService.instance.getAllAccounts();

        console.log('Accounts:', accounts);

        if (accounts.length > 0) {

          console.log('Logged in user:', accounts[0]);

          // Go to home after successful login
          this.router.navigate(['/home']);
        }
      },

      error: (error) => {
        console.error('MSAL error:', error);
      }
    });
  }

  login(): void {

    this.msalService.loginRedirect({
      scopes: [
        'api://7af1cd49-f408-4e44-8c4f-bbc2f7b2e241/access_as_user'
      ]
    });
  }
}