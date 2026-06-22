import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
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
  imports: [CommonModule, FormsModule, ReactiveFormsModule, MatIconModule, ConfirmDialogComponent],
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
  editId: number | null = null;

  enrollmentForm: FormGroup;

  constructor(
    private http: HttpClient,
    private toastService: ToastService,
    private cdr: ChangeDetectorRef,
    private authService: AuthService,
    private fb: FormBuilder
  ) {
    this.enrollmentForm = this.fb.group({
      customerId: [0, [Validators.required, Validators.min(1)]],
      chitGroupId: [0, [Validators.required, Validators.min(1)]],
      slotNumber: [0, [Validators.required, Validators.min(1)]]
    });
  }

  get isAdmin(): boolean {
  return this.authService.getRole() === 'Admin';
}

  get customerId() { return this.enrollmentForm.get('customerId')!; }
  get chitGroupId() { return this.enrollmentForm.get('chitGroupId')!; }
  get slotNumber() { return this.enrollmentForm.get('slotNumber')!; }

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

  get filteredEnrollments(): Enrollment[] {
    return this.enrollments.filter(e =>
      (e.customerName?.toLowerCase() ?? '').includes(this.searchText.toLowerCase()) ||
      (e.chitGroupName?.toLowerCase() ?? '').includes(this.searchText.toLowerCase())
    );
  }

  openAdd(): void {
    this.enrollmentForm.reset({ customerId: 0, chitGroupId: 0, slotNumber: 0 });
    this.isEdit = false;
    this.editId = null;
    this.showForm = true;
    this.cdr.detectChanges();
  }

  openEdit(enrollment: Enrollment): void {
    this.enrollmentForm.patchValue(enrollment);
    this.isEdit = true;
    this.editId = enrollment.id!;
    this.showForm = true;
    this.cdr.detectChanges();
  }

  save(): void {
    if (this.enrollmentForm.invalid) {
      this.enrollmentForm.markAllAsTouched();
      this.toastService.error('Please fill all required fields correctly!');
      return;
    }

    const formData = this.enrollmentForm.value;

    if (this.isEdit) {
      this.http.put(`${environment.apiUrl}/Enrollment/${this.editId}`, formData).subscribe({
        next: () => {
          this.showForm = false;
          this.cdr.detectChanges();
          this.toastService.success('Enrollment updated successfully!');
          this.loadEnrollments();
        }
      });
    } else {
      this.http.post(`${environment.apiUrl}/Enrollment`, formData).subscribe({
        next: () => {
          this.showForm = false;
          this.cdr.detectChanges();
          this.toastService.success('Enrollment added successfully!');
          this.loadEnrollments();
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
      this.http.delete(`${environment.apiUrl}/Enrollment/${this.deleteId}`).subscribe({
        next: () => {
          this.showDeleteConfirm = false;
          this.cdr.detectChanges();
          this.toastService.success('Enrollment deleted successfully!');
          this.loadEnrollments();
        }
      });
    }
  }

  cancel(): void {
    this.showForm = false;
    this.cdr.detectChanges();
  }
}
