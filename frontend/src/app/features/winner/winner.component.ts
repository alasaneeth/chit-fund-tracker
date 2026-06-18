import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
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
  imports: [CommonModule, FormsModule, MatIconModule, ConfirmDialogComponent],
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

  form: Winner = {
    chitGroupId: 0,
    customerId: 0,
    enrollmentId: 0,
    monthNumber: 0,
    prizeAmount: 0,
    commissionDeducted: 0,
    netAmount: 0,
    selectionType: 'Lottery',
    notes: ''
  };

  constructor(
    private http: HttpClient,
    private toastService: ToastService,
    private cdr: ChangeDetectorRef
  ) {}

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
      },
      error: () => this.toastService.error('Failed to load winners!')
    });
  }

  loadCustomers(): void {
    this.http.get<Customer[]>(`${environment.apiUrl}/Customer`).subscribe({
      next: (data) => { this.customers = data; this.cdr.detectChanges(); },
      error: () => {}
    });
  }

  loadChitGroups(): void {
    this.http.get<ChitGroup[]>(`${environment.apiUrl}/ChitGroup`).subscribe({
      next: (data) => { this.chitGroups = data; this.cdr.detectChanges(); },
      error: () => {}
    });
  }

  loadEnrollments(): void {
    this.http.get<Enrollment[]>(`${environment.apiUrl}/Enrollment`).subscribe({
      next: (data) => { this.enrollments = data; this.cdr.detectChanges(); },
      error: () => {}
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
    this.form = {
      chitGroupId: 0, customerId: 0, enrollmentId: 0,
      monthNumber: 0, prizeAmount: 0, commissionDeducted: 0,
      netAmount: 0, selectionType: 'Lottery', notes: ''
    };
    this.isEdit = false;
    this.showForm = true;
    this.cdr.detectChanges();
  }

  openEdit(winner: Winner): void {
    this.form = { ...winner };
    this.isEdit = true;
    this.showForm = true;
    this.cdr.detectChanges();
  }

  calculateNet(): void {
    this.form.netAmount = this.form.prizeAmount - this.form.commissionDeducted;
  }

save(): void {
  const payload = {
    chitGroupId: Number(this.form.chitGroupId),
    customerId: Number(this.form.customerId),
    enrollmentId: Number(this.form.enrollmentId),
    monthNumber: Number(this.form.monthNumber),
    prizeAmount: Number(this.form.prizeAmount),
    selectionType: this.form.selectionType,
    notes: this.form.notes
  };

  if (this.isEdit) {
    this.toastService.warning('Winner edit is not supported!');
    return;
  }

  this.http.post(`${environment.apiUrl}/Winner/select`, payload).subscribe({
    next: () => {
      this.showForm = false;
      this.cdr.detectChanges();
      this.toastService.success('Winner selected successfully!');
      this.loadWinners();
    },
    error: () => this.toastService.error('Failed to select winner!')
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
        },
        error: () => this.toastService.error('Failed to delete winner!')
      });
    }
  }

  cancel(): void {
    this.showForm = false;
    this.cdr.detectChanges();
  }
}
