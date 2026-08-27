import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login {

  constructor(private readonly router: Router) {}

  login(): void {
    // Temporary navigation.
    // Later this will start the OAuth 2.0
    // Authorization Code + PKCE flow.
    this.router.navigate(['/home']);
  }
}