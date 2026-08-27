import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Tenant {
  id: string;
  entraTenantId: string;
  name: string;
  isActive: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class Api {

  private readonly http = inject(HttpClient);

  private readonly apiUrl = 'http://localhost:5137/api';

  getTenants(): Observable<Tenant[]> {
    return this.http.get<Tenant[]>(
      `${this.apiUrl}/tenants`
    );
  }
}