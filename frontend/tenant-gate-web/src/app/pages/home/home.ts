import { Component, OnInit } from '@angular/core';
import { Api, Tenant } from '../../services/api';

@Component({
  selector: 'app-home',
  templateUrl: './home.html',
  styleUrl: './home.scss'
})
export class Home implements OnInit {

  tenants: Tenant[] = [];
  loading = true;
  error = '';

  constructor(private api: Api) {}

  ngOnInit(): void {

  console.log('Home initialized');

  this.api.getTenants().subscribe({

    next: (response: Tenant[]) => {

      console.log('API RESPONSE:', response);

      this.tenants = response;
      this.loading = false;

      console.log('Tenants:', this.tenants);
      console.log('Loading:', this.loading);
    },

    error: (error: unknown) => {

      console.error('Failed to load tenants', error);

      this.error = 'Failed to load tenants.';
      this.loading = false;
    }

  });
}
}