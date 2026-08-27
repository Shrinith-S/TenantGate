import { Component, OnInit, inject, signal } from '@angular/core';
import { Api, Tenant } from '../../services/api';

@Component({
  selector: 'app-home',
  imports: [],
  templateUrl: './home.html',
  styleUrl: './home.scss'
})
export class Home implements OnInit {

  private readonly api = inject(Api);

  protected readonly tenants = signal<Tenant[]>([]);

  protected readonly loading = signal(true);

  protected readonly error = signal<string | null>(null);

  ngOnInit(): void {

    this.api.getTenants().subscribe({
      next: (response) => {
        console.log('Tenants received:', response);

        this.tenants.set(response);
        this.loading.set(false);
      },

      error: (error) => {
        console.error('Failed to load tenants:', error);

        this.error.set('Unable to load tenants.');
        this.loading.set(false);
      }
    });
  }
}