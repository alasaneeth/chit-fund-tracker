import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { ToastService } from '../../core/services/toast.service';
import { ConfirmDialogComponent } from '../../shared/confirm-dialog/confirm-dialog.component';

interface Customer {
  id?: number;
  fullName: string;
  email: string;
  phone: string;
  address: string;
  aadharNumber?: string;
  dateOfBirth?: string;
}

@Component({
  selector: 'app-customer',
  standalone: true,
  imports: [CommonModule, FormsModule, MatIconModule, ConfirmDialogComponent],
  templateUrl: './customer.component.html',
  styleUrl: './customer.component.scss'
})
export class CustomerComponent implements OnInit {
  customers: Customer[] = [];
  showForm = false;
  isEdit = false;
  searchText = '';
  showDeleteConfirm = false;
  deleteId: number | null = null;

  form: Customer = {
    fullName: '', email: '', phone: '', address: '', aadharNumber: ''
  };

  constructor(
    private http: HttpClient,
    private toastService: ToastService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadCustomers();
  }

  loadCustomers(): void {
    this.http.get<Customer[]>(`${environment.apiUrl}/Customer`).subscribe({
      next: (data) => {
        this.customers = Array.isArray(data) ? data : [];
        this.cdr.detectChanges();
      },
      error: () => this.toastService.error('Failed to load customers!')
    });
  }

  get filteredCustomers(): Customer[] {
    return this.customers.filter(c =>
      (c.fullName?.toLowerCase() ?? '').includes(this.searchText.toLowerCase()) ||
      (c.phone ?? '').includes(this.searchText)
    );
  }

  openAdd(): void {
    this.form = { fullName: '', email: '', phone: '', address: '', aadharNumber: '' };
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
        next: () => {
          this.toastService.success('Customer updated successfully!');
          this.loadCustomers();
          this.showForm = false;
          this.cdr.detectChanges();
        },
        error: () => this.toastService.error('Failed to update customer!')
      });
    } else {
      this.http.post(`${environment.apiUrl}/Customer`, this.form).subscribe({
        next: () => {
          this.toastService.success('Customer added successfully!');
          this.loadCustomers();
          this.showForm = false;
          this.cdr.detectChanges();
        },
        error: () => this.toastService.error('Failed to add customer!')
      });
    }
  }

  delete(id: number): void {
    this.deleteId = id;
    this.showDeleteConfirm = true;
    this.cdr.detectChanges();
  }

  confirmDelete(): void {
    if (this.deleteId) {
      this.http.delete(`${environment.apiUrl}/Customer/${this.deleteId}`).subscribe({
        next: () => {
          this.toastService.success('Customer deleted successfully!');
          this.loadCustomers();
          this.showDeleteConfirm = false;
          this.cdr.detectChanges();
        },
        error: () => this.toastService.error('Failed to delete customer!')
      });
    }
  }

  cancel(): void {
    this.showForm = false;
    this.cdr.detectChanges();
  }
}
