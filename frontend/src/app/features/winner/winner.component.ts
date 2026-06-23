import { AuthService } from './../../core/services/auth.service';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { ToastService } from '../../core/services/toast.service';
import { ConfirmDialogComponent } from '../../shared/confirm-dialog/confirm-dialog.component';

interface Winner {
  id?: number;
  chitGroupId: number;
  chitGroupName?: string;
  customerId: number;
  customerName?: string;
  enrollmentId: number;
  monthNumber: number;
  prizeAmount: number;
  commissionDeducted: number;
  netAmount: number;
  wonDate?: string;
  selectionType: string;
  notes?: string;
}

interface Customer {
  id: number;
  fullName: string;
}

interface ChitGroup {
  id: number;
  name: string;
}

interface Enrollment {
  id: number;
  slotNumber: number;
  customerId: number;
  chitGroupId: number;
}

@Component({
  selector: 'app-winner',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, MatIconModule, ConfirmDialogComponent],
  templateUrl: './winner.component.html',
  styleUrl: './winner.component.scss'
})
export class WinnerComponent implements OnInit {
  winners: Winner[] = [];
  customers: Customer[] = [];
  chitGroups: ChitGroup[] = [];
  enrollments: Enrollment[] = [];
  showForm = false;
  isEdit = false;
  searchText = '';
  showDeleteConfirm = false;
  deleteId: number | null = null;

  selectionTypes = ['Lottery', 'Auction', 'Fixed'];

  winnerForm: FormGroup;

  constructor(
    private http: HttpClient,
    private toastService: ToastService,
    private cdr: ChangeDetectorRef,
    private authService: AuthService,
    private fb: FormBuilder
  ) {
    this.winnerForm = this.fb.group({
      chitGroupId: [0, [Validators.required, Validators.min(1)]],
      customerId: [0, [Validators.required, Validators.min(1)]],
      enrollmentId: [0, [Validators.required, Validators.min(1)]],
      monthNumber: [0, [Validators.required, Validators.min(1)]],
      prizeAmount: [0, [Validators.required, Validators.min(1)]],
      commissionDeducted: [0],
      netAmount: [0],
      selectionType: ['Lottery', Validators.required],
      notes: ['']
    });
  }

   get isAdmin(): boolean {
  return this.authService.getRole() === 'Admin';
}


  get chitGroupId() { return this.winnerForm.get('chitGroupId')!; }
  get customerId() { return this.winnerForm.get('customerId')!; }
  get enrollmentId() { return this.winnerForm.get('enrollmentId')!; }
  get monthNumber() { return this.winnerForm.get('monthNumber')!; }
  get prizeAmount() { return this.winnerForm.get('prizeAmount')!; }
  get commissionDeducted() { return this.winnerForm.get('commissionDeducted')!; }
  get netAmount() { return this.winnerForm.get('netAmount')!; }

  ngOnInit(): void {
    this.loadWinners();
    this.loadCustomers();
    this.loadChitGroups();
    this.loadEnrollments();
  }

  loadWinners(): void {
    this.http.get<Winner[]>(`${environment.apiUrl}/Winner`).subscribe({
      next: (data) => {
        this.winners = Array.isArray(data) ? data : [];
        this.cdr.detectChanges();
      }
    });
  }

  loadCustomers(): void {
    this.http.get<Customer[]>(`${environment.apiUrl}/Customer`).subscribe({
      next: (data) => { this.customers = data; this.cdr.detectChanges(); }
    });
  }

  loadChitGroups(): void {
    this.http.get<ChitGroup[]>(`${environment.apiUrl}/ChitGroup`).subscribe({
      next: (data) => { this.chitGroups = data; this.cdr.detectChanges(); }
    });
  }

  loadEnrollments(): void {
    this.http.get<Enrollment[]>(`${environment.apiUrl}/Enrollment`).subscribe({
      next: (data) => { this.enrollments = data; this.cdr.detectChanges(); }
    });
  }

  get filteredWinners(): Winner[] {
    return this.winners.filter(w =>
      (w.customerName?.toLowerCase() ?? '').includes(this.searchText.toLowerCase()) ||
      (w.chitGroupName?.toLowerCase() ?? '').includes(this.searchText.toLowerCase()) ||
      (w.selectionType?.toLowerCase() ?? '').includes(this.searchText.toLowerCase())
    );
  }

  openAdd(): void {
    this.winnerForm.reset({
      chitGroupId: 0, customerId: 0, enrollmentId: 0,
      monthNumber: 0, prizeAmount: 0, commissionDeducted: 0,
      netAmount: 0, selectionType: 'Lottery', notes: ''
    });
    this.isEdit = false;
    this.showForm = true;
    this.cdr.detectChanges();
  }

  openEdit(winner: Winner): void {
    this.winnerForm.patchValue(winner);
    this.isEdit = true;
    this.showForm = true;
    this.cdr.detectChanges();
  }

  calculateNet(): void {
    const prize = this.prizeAmount.value || 0;
    const commission = this.commissionDeducted.value || 0;
    this.netAmount.setValue(prize - commission);
  }

  save(): void {
    if (this.isEdit) {
      this.toastService.warning('Winner edit is not supported!');
      return;
    }

    if (this.winnerForm.invalid) {
      this.winnerForm.markAllAsTouched();
      this.toastService.error('Please fill all required fields correctly!');
      return;
    }

    const formValue = this.winnerForm.value;
    const payload = {
      chitGroupId: Number(formValue.chitGroupId),
      customerId: Number(formValue.customerId),
      enrollmentId: Number(formValue.enrollmentId),
      monthNumber: Number(formValue.monthNumber),
      prizeAmount: Number(formValue.prizeAmount),
      selectionType: formValue.selectionType,
      notes: formValue.notes
    };

    this.http.post(`${environment.apiUrl}/Winner/select`, payload).subscribe({
      next: () => {
        this.showForm = false;
        this.cdr.detectChanges();
        this.toastService.success('Winner selected successfully!');
        this.loadWinners();
      }
    });
  }

  delete(id: number): void {
    this.deleteId = id;
    this.showDeleteConfirm = true;
    this.cdr.detectChanges();
  }

  confirmDelete(): void {
    if (this.deleteId) {
      this.http.delete(`${environment.apiUrl}/Winner/${this.deleteId}`).subscribe({
        next: () => {
          this.showDeleteConfirm = false;
          this.cdr.detectChanges();
          this.toastService.success('Winner deleted successfully!');
          this.loadWinners();
        }
      });
    }
  }

  cancel(): void {
    this.showForm = false;
    this.cdr.detectChanges();
  }
}
