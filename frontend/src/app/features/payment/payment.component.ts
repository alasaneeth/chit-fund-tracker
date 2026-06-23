import { AuthService } from './../../core/services/auth.service';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { ToastService } from '../../core/services/toast.service';
import { ConfirmDialogComponent } from '../../shared/confirm-dialog/confirm-dialog.component';

interface Payment {
  id?: number;
  enrollmentId: number;
  customerName?: string;
  chitGroupName?: string;
  monthNumber: number;
  amountPaid: number;
  paidDate?: string;
  receiptNumber?: string;
  paymentMode: string;
  notes?: string;
  isLate?: boolean;
}

interface Enrollment {
  id: number;
  customerId: number;
  chitGroupId: number;
  slotNumber: number;
}

@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, MatIconModule, ConfirmDialogComponent],
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.scss'
})
export class PaymentComponent implements OnInit {
  payments: Payment[] = [];
  enrollments: Enrollment[] = [];
  showForm = false;
  isEdit = false;
  searchText = '';
  showDeleteConfirm = false;
  deleteId: number | null = null;
  editId: number | null = null;

  paymentModes = ['Cash', 'Online', 'Cheque', 'Bank Transfer'];

  paymentForm: FormGroup;

  constructor(
    private http: HttpClient,
    private toastService: ToastService,
    private cdr: ChangeDetectorRef,
    private authService:AuthService,
    private fb: FormBuilder
  ) {
    this.paymentForm = this.fb.group({
      enrollmentId: [0, [Validators.required, Validators.min(1)]],
      monthNumber: [0, [Validators.required, Validators.min(1)]],
      amountPaid: [0, [Validators.required, Validators.min(1)]],
      paymentMode: ['Cash', Validators.required],
      notes: ['']
    });
  }

get isAdmin(): boolean {
  return this.authService.getRole() === 'Admin';
}

  get enrollmentId() { return this.paymentForm.get('enrollmentId')!; }
  get monthNumber() { return this.paymentForm.get('monthNumber')!; }
  get amountPaid() { return this.paymentForm.get('amountPaid')!; }
  get paymentMode() { return this.paymentForm.get('paymentMode')!; }

  ngOnInit(): void {
    this.loadPayments();
    this.loadEnrollments();
  }

  loadPayments(): void {
    this.http.get<Payment[]>(`${environment.apiUrl}/Payment`).subscribe({
      next: (data) => {
        this.payments = Array.isArray(data) ? data : [];
        this.cdr.detectChanges();
      }
    });
  }

  loadEnrollments(): void {
    this.http.get<Enrollment[]>(`${environment.apiUrl}/Enrollment`).subscribe({
      next: (data) => { this.enrollments = data; this.cdr.detectChanges(); }
    });
  }

  get filteredPayments(): Payment[] {
    return this.payments.filter(p =>
      (p.customerName?.toLowerCase() ?? '').includes(this.searchText.toLowerCase()) ||
      (p.receiptNumber?.toLowerCase() ?? '').includes(this.searchText.toLowerCase()) ||
      (p.chitGroupName?.toLowerCase() ?? '').includes(this.searchText.toLowerCase())
    );
  }

  openAdd(): void {
    this.paymentForm.reset({ enrollmentId: 0, monthNumber: 0, amountPaid: 0, paymentMode: 'Cash', notes: '' });
    this.isEdit = false;
    this.editId = null;
    this.showForm = true;
    this.cdr.detectChanges();
  }

  openEdit(payment: Payment): void {
    this.paymentForm.patchValue(payment);
    this.isEdit = true;
    this.editId = payment.id!;
    this.showForm = true;
    this.cdr.detectChanges();
  }

  save(): void {
    if (this.paymentForm.invalid) {
      this.paymentForm.markAllAsTouched();
      this.toastService.error('Please fill all required fields correctly!');
      return;
    }

    const formData = this.paymentForm.value;

    if (this.isEdit) {
      this.http.put(`${environment.apiUrl}/Payment/${this.editId}`, formData).subscribe({
        next: () => {
          this.showForm = false;
          this.cdr.detectChanges();
          this.toastService.success('Payment updated successfully!');
          this.loadPayments();
        }
      });
    } else {
      this.http.post(`${environment.apiUrl}/Payment`, formData).subscribe({
        next: () => {
          this.showForm = false;
          this.cdr.detectChanges();
          this.toastService.success('Payment added successfully!');
          this.loadPayments();
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
      this.http.delete(`${environment.apiUrl}/Payment/${this.deleteId}`).subscribe({
        next: () => {
          this.showDeleteConfirm = false;
          this.cdr.detectChanges();
          this.toastService.success('Payment deleted successfully!');
          this.loadPayments();
        }
      });
    }
  }

  cancel(): void {
    this.showForm = false;
    this.cdr.detectChanges();
  }
}
