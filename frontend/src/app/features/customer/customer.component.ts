import { Component, OnInit,ChangeDetectorRef  } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

interface Customer {
  id?: number;
  fullName: string;
  email: string;
  phone: string;
  address: string;
}

@Component({
  selector: 'app-customer',
  standalone: true,
  imports: [CommonModule, FormsModule, MatIconModule, MatButtonModule],
  templateUrl: './customer.component.html',
  styleUrl: './customer.component.scss'
})
export class CustomerComponent implements OnInit {
  customers: Customer[] = [];
  showForm = false;
  isEdit = false;
  searchText = '';

  form: Customer = {
    fullName: '', email: '', phone: '', address: ''
  };

  constructor(
      private http: HttpClient,
      private cdr: ChangeDetectorRef

  ) {}

  ngOnInit(): void {
    this.loadCustomers();
  }

 loadCustomers(): void {
  this.http.get<any>(`${environment.apiUrl}/Customer`).subscribe({
    next: (data) => {
      this.customers = Array.isArray(data) ? data : data.data ?? [];
      this.cdr.detectChanges();
    },
    error: () => {}
  });
}

 get filteredCustomers(): Customer[] {
  return this.customers.filter(c =>
    (c.fullName?.toLowerCase() ?? '').includes(this.searchText.toLowerCase()) ||
    (c.phone ?? '').includes(this.searchText)
  );
}


  openAdd(): void {
  this.form = { fullName: '', email: '', phone: '', address: '' };
  this.isEdit = false;
  this.showForm = true;
  this.cdr.detectChanges();
}

openEdit(customer: Customer): void {
  this.form = { ...customer };
  this.isEdit = true;
  this.showForm = true;
  this.cdr.detectChanges();
}

  save(): void {
    if (this.isEdit) {
      this.http.put(`${environment.apiUrl}/Customer/${this.form.id}`, this.form).subscribe({
        next: () => { this.loadCustomers(); this.showForm = false; },
        error: () => {}
      });
    } else {
      this.http.post(`${environment.apiUrl}/Customer`, this.form).subscribe({
        next: () => { this.loadCustomers(); this.showForm = false; },
        error: () => {}
      });
    }
  }

  delete(id: number): void {
    if (confirm('Delete this customer?')) {
      this.http.delete(`${environment.apiUrl}/Customer/${id}`).subscribe({
        next: () => this.loadCustomers(),
        error: () => {}
      });
    }
  }

  cancel(): void {
    this.showForm = false;
  }
}
