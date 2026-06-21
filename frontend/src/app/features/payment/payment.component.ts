import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { ToastService } from '../../core/services/toast.service';
import { ConfirmDialogComponent } from '../../shared/confirm-dialog/confirm-dialog.component';
import { AuthService } from '../../core/services/auth.service';

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
  imports: [CommonModule, FormsModule, MatIconModule, ConfirmDialogComponent],
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

  paymentModes = ['Cash', 'Online', 'Cheque', 'Bank Transfer'];

  form: Payment = {
    enrollmentId: 0,
    monthNumber: 0,
    amountPaid: 0,
    paymentMode: 'Cash',
    notes: ''
  };

  constructor(
    private http: HttpClient,
    private toastService: ToastService,
    private cdr: ChangeDetectorRef,
    private authService: AuthService
  ) {}

     get isAdmin(): boolean {
  return this.authService.getRole() === 'Admin';
}


  ngOnInit(): void {
    this.loadPayments();
    this.loadEnrollments();
  }

  loadPayments(): void {
    this.http.get<Payment[]>(`${environment.apiUrl}/Payment`).subscribe({
      next: (data) => {
        this.payments = Array.isArray(data) ? data : [];
        this.cdr.detectChanges();
      },
      error: () => this.toastService.error('Failed to load payments!')
    });
  }

  loadEnrollments(): void {
    this.http.get<Enrollment[]>(`${environment.apiUrl}/Enrollment`).subscribe({
      next: (data) => {
        this.enrollments = Array.isArray(data) ? data : [];
        this.cdr.detectChanges();
      },
      error: () => {}
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
    this.form = {
      enrollmentId: 0,
      monthNumber: 0,
      amountPaid: 0,
      paymentMode: 'Cash',
      notes: ''
    };
    this.isEdit = false;
    this.showForm = true;
    this.cdr.detectChanges();
  }

  openEdit(payment: Payment): void {
    this.form = { ...payment };
    this.isEdit = true;
    this.showForm = true;
    this.cdr.detectChanges();
  }

  save(): void {
    if (this.isEdit) {
      this.http.put(`${environment.apiUrl}/Payment/${this.form.id}`, this.form).subscribe({
        next: () => {
          this.showForm = false;
          this.cdr.detectChanges();
          this.toastService.success('Payment updated successfully!');
          this.loadPayments();
        },
        error: () => this.toastService.error('Failed to update payment!')
      });
    } else {
      this.http.post(`${environment.apiUrl}/Payment`, this.form).subscribe({
        next: () => {
          this.showForm = false;
          this.cdr.detectChanges();
          this.toastService.success('Payment added successfully!');
          this.loadPayments();
        },
        error: () => this.toastService.error('Failed to add payment!')
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
        },
        error: () => this.toastService.error('Failed to delete payment!')
      });
    }
  }

  cancel(): void {
    this.showForm = false;
    this.cdr.detectChanges();
  }
}
