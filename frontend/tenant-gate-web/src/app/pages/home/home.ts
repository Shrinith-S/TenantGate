import {
  Component,
  OnInit,
  ChangeDetectorRef
} from '@angular/core';

import { Api, Tenant } from '../../services/api';

@Component({
  selector: 'app-home',
  templateUrl: './home.html',
  styleUrl: './home.scss'
})
export class Home implements OnInit {

  tenants: Tenant[] = [];

  selectedTenant: Tenant | null = null;

  loading = true;
  error = '';

  constructor(
    private api: Api,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {

    console.log('1. Home initialized');

    this.api.getTenants().subscribe({

      next: (response: Tenant[]) => {

        console.log('2. SUCCESS');
        console.log('3. Response:', response);

        this.tenants = response;

        console.log(
          '4. tenants AFTER assignment:',
          this.tenants
        );

        console.log(
          '5. tenant count:',
          this.tenants.length
        );

        this.loading = false;

        // TEMPORARY TEST
        this.cdr.detectChanges();

        console.log('6. Change detection triggered');
      },

      error: (error: unknown) => {

        console.error('ERROR:', error);

        this.error = 'Failed to load tenants.';
        this.loading = false;

        this.cdr.detectChanges();
      }

    });
  }

  selectTenant(tenant: Tenant): void {

    this.selectedTenant = tenant;

    localStorage.setItem(
      'selectedTenantId',
      tenant.id
    );

    console.log('Selected tenant:', tenant);

    this.cdr.detectChanges();
  }
}