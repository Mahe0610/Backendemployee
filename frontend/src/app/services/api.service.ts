import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Employee, LoginResponse, UserRole } from '../models/employee.model';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private readonly apiUrl = 'https://localhost:7040/api';

  constructor(private readonly http: HttpClient) {}

  login(username: string, password: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/auth/login`, {
      username,
      password,
    });
  }

  createEmployee(payload: {
    employeeName: string;
    age: number;
    dateOfBirth: string;
    email: string;
    salary?: number | null;
    role: UserRole;
  }): Observable<Employee> {
    return this.http.post<Employee>(`${this.apiUrl}/employees`, payload);
  }

  listEmployees(search?: string): Observable<Employee[]> {
    const params = search
      ? new HttpParams().set('search', search)
      : new HttpParams();

    return this.http.get<Employee[]>(`${this.apiUrl}/employees`, { params });
  }

  exportCertificatePdf(employeeId: number): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/certificate/${employeeId}/pdf`, {
      responseType: 'blob',
    });
  }

  exportCertificateExcel(employeeId: number): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/certificate/${employeeId}/excel`, {
      responseType: 'blob',
    });
  }
}
