import { Injectable } from '@angular/core';
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
  private readonly baseUrl = 'http://localhost:5137/api';

  constructor(private http: HttpClient) {}

  getTenants(): Observable<Tenant[]> {
    return this.http.get<Tenant[]>(`${this.baseUrl}/tenants`);
  }
}