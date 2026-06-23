import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import {FormsModule, FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
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
}

@Component({
  selector: 'app-customer',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, MatIconModule, ConfirmDialogComponent],
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
  editId: number | null = null;

  customerForm: FormGroup;

  constructor(
    private http: HttpClient,
    private toastService: ToastService,
    private cdr: ChangeDetectorRef,
    private fb: FormBuilder
  ) {
    this.customerForm = this.fb.group({
      fullName: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern(/^[0-9]{10}$/)]],
      address: ['', Validators.required],
      aadharNumber: ['']
    });
  }

  ngOnInit(): void {
    this.loadCustomers();
  }

  loadCustomers(): void {
    this.http.get<Customer[]>(`${environment.apiUrl}/Customer`).subscribe({
      next: (data) => {
        this.customers = Array.isArray(data) ? data : [];
        this.cdr.detectChanges();
      }
    });
  }

  get filteredCustomers(): Customer[] {
    return this.customers.filter(c =>
      (c.fullName?.toLowerCase() ?? '').includes(this.searchText.toLowerCase()) ||
      (c.phone ?? '').includes(this.searchText)
    );
  }

  // Getter shortcuts for template
get fullName() { return this.customerForm.get('fullName')!; }
get email() { return this.customerForm.get('email')!; }
get phone() { return this.customerForm.get('phone')!; }
get address() { return this.customerForm.get('address')!; }
get aadharNumber() { return this.customerForm.get('aadharNumber')!; }

  openAdd(): void {
    this.customerForm.reset();
    this.isEdit = false;
    this.editId = null;
    this.showForm = true;
    this.cdr.detectChanges();
  }

  openEdit(customer: Customer): void {
    this.customerForm.patchValue(customer);
    this.isEdit = true;
    this.editId = customer.id!;
    this.showForm = true;
    this.cdr.detectChanges();
  }

  save(): void {
    if (this.customerForm.invalid) {
      this.customerForm.markAllAsTouched();
      this.toastService.error('Please fill all required fields correctly!');
      return;
    }

    const formData = this.customerForm.value;

    if (this.isEdit) {
      this.http.put(`${environment.apiUrl}/Customer/${this.editId}`, formData).subscribe({
        next: () => {
          this.showForm = false;
          this.cdr.detectChanges();
          this.toastService.success('Customer updated successfully!');
          this.loadCustomers();
        }
      });
    } else {
      this.http.post(`${environment.apiUrl}/Customer`, formData).subscribe({
        next: () => {
          this.showForm = false;
          this.cdr.detectChanges();
          this.toastService.success('Customer added successfully!');
          this.loadCustomers();
        }
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
          this.showDeleteConfirm = false;
          this.cdr.detectChanges();
          this.toastService.success('Customer deleted successfully!');
          this.loadCustomers();
        }
      });
    }
  }

  cancel(): void {
    this.showForm = false;
    this.cdr.detectChanges();
  }
}
