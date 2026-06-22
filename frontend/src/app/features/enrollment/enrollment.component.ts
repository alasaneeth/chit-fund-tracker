import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { ToastService } from '../../core/services/toast.service';
import { ConfirmDialogComponent } from '../../shared/confirm-dialog/confirm-dialog.component';
import { AuthService } from '../../core/services/auth.service';

interface Enrollment {
  id?: number;
  customerId: number;
  customerName?: string;
  chitGroupId: number;
  chitGroupName?: string;
  slotNumber: number;
  joinDate?: string;
  isActive?: boolean;
  hasWon?: boolean;
}

interface Customer {
  id: number;
  fullName: string;
}

interface ChitGroup {
  id: number;
  name: string;
}

@Component({
  selector: 'app-enrollment',
  standalone: true,
  imports: [CommonModule, FormsModule, MatIconModule, ConfirmDialogComponent],
  templateUrl: './enrollment.component.html',
  styleUrl: './enrollment.component.scss'
})
export class EnrollmentComponent implements OnInit {
  enrollments: Enrollment[] = [];
  customers: Customer[] = [];
  chitGroups: ChitGroup[] = [];
  showForm = false;
  isEdit = false;
  searchText = '';
  showDeleteConfirm = false;
  deleteId: number | null = null;

  form: Enrollment = {
    customerId: 0,
    chitGroupId: 0,
    slotNumber: 0
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
    this.loadEnrollments();
    this.loadCustomers();
    this.loadChitGroups();
  }

  loadEnrollments(): void {
    this.http.get<Enrollment[]>(`${environment.apiUrl}/Enrollment`).subscribe({
      next: (data) => {
        this.enrollments = Array.isArray(data) ? data : [];
        this.cdr.detectChanges();
      },
    });
  }

  loadCustomers(): void {
    this.http.get<Customer[]>(`${environment.apiUrl}/Customer`).subscribe({
      next: (data) => {
        this.customers = Array.isArray(data) ? data : [];
        this.cdr.detectChanges();
      },
      error: () => {}
    });
  }

  loadChitGroups(): void {
    this.http.get<ChitGroup[]>(`${environment.apiUrl}/ChitGroup`).subscribe({
      next: (data) => {
        this.chitGroups = Array.isArray(data) ? data : [];
        this.cdr.detectChanges();
      },
      error: () => {}
    });
  }

  get filteredEnrollments(): Enrollment[] {
    return this.enrollments.filter(e =>
      (e.customerName?.toLowerCase() ?? '').includes(this.searchText.toLowerCase()) ||
      (e.chitGroupName?.toLowerCase() ?? '').includes(this.searchText.toLowerCase())
    );
  }

  openAdd(): void {
    this.form = { customerId: 0, chitGroupId: 0, slotNumber: 0 };
    this.isEdit = false;
    this.showForm = true;
    this.cdr.detectChanges();
  }

  openEdit(enrollment: Enrollment): void {
    this.form = { ...enrollment };
    this.isEdit = true;
    this.showForm = true;
    this.cdr.detectChanges();
  }

  save(): void {
    if (this.isEdit) {
      this.http.put(`${environment.apiUrl}/Enrollment/${this.form.id}`, this.form).subscribe({
        next: () => {
          this.showForm = false;
          this.cdr.detectChanges();
          this.toastService.success('Enrollment updated successfully!');
          this.loadEnrollments();
        },
        error: () => this.toastService.error('Failed to update enrollment!')
      });
    } else {
      this.http.post(`${environment.apiUrl}/Enrollment`, this.form).subscribe({
        next: () => {
          this.showForm = false;
          this.cdr.detectChanges();
          this.toastService.success('Enrollment added successfully!');
          this.loadEnrollments();
        },
        error: () => this.toastService.error('Failed to add enrollment!')
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
      this.http.delete(`${environment.apiUrl}/Enrollment/${this.deleteId}`).subscribe({
        next: () => {
          this.showDeleteConfirm = false;
          this.cdr.detectChanges();
          this.toastService.success('Enrollment deleted successfully!');
          this.loadEnrollments();
        },
        error: () => this.toastService.error('Failed to delete enrollment!')
      });
    }
  }

  cancel(): void {
    this.showForm = false;
    this.cdr.detectChanges();
  }
}
