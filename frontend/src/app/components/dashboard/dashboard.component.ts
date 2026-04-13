import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Employee, UserRole } from '../../models/employee.model';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
})
export class DashboardComponent implements OnInit {
  employees: Employee[] = [];
  search = '';
  role = (localStorage.getItem('role') as UserRole) || 'User';

  employeeForm = this.fb.nonNullable.group({
    employeeName: ['', Validators.required],
    age: [18, [Validators.required, Validators.min(18)]],
    dateOfBirth: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    salary: [null as number | null],
  });

  constructor(private readonly api: ApiService, private readonly fb: FormBuilder) {}

  ngOnInit(): void {
    this.loadEmployees();
  }

  loadEmployees(): void {
    this.api.listEmployees(this.search).subscribe((data) => (this.employees = data));
  }

  onSearch(value: string): void {
    this.search = value;
    this.loadEmployees();
  }

  submit(): void {
    if (this.employeeForm.invalid) {
      return;
    }

    const value = this.employeeForm.getRawValue();
    this.api
      .createEmployee({
        ...value,
        role: this.role,
        salary: this.role === 'Admin' ? value.salary : null,
      })
      .subscribe(() => {
        this.employeeForm.reset({
          employeeName: '',
          age: 18,
          dateOfBirth: '',
          email: '',
          salary: null,
        });
        this.loadEmployees();
      });
  }

  downloadPdf(id: number): void {
    this.api.exportCertificatePdf(id).subscribe((blob) => this.saveFile(blob, `certificate-${id}.pdf`));
  }

  downloadExcel(id: number): void {
    this.api.exportCertificateExcel(id).subscribe((blob) => this.saveFile(blob, `certificate-${id}.xlsx`));
  }

  private saveFile(blob: Blob, fileName: string): void {
    const url = window.URL.createObjectURL(blob);
    const anchor = document.createElement('a');
    anchor.href = url;
    anchor.download = fileName;
    anchor.click();
    URL.revokeObjectURL(url);
  }
}
