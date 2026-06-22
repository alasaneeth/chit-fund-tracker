import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { ToastService } from '../../core/services/toast.service';
import { ConfirmDialogComponent } from '../../shared/confirm-dialog/confirm-dialog.component';
import { AuthService } from '../../core/services/auth.service';

interface ChitGroup {
  id?: number;
  name: string;
  chitType: number;
  totalAmount: number;
  monthlyContribution: number;
  totalMembers: number;
  durationMonths: number;
  startDate: string;
  status: number;
}

@Component({
  selector: 'app-chit-group',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, MatIconModule, ConfirmDialogComponent],
  templateUrl: './chit-group.component.html',
  styleUrl: './chit-group.component.scss'
})
export class ChitGroupComponent implements OnInit {
  chitGroups: ChitGroup[] = [];
  showForm = false;
  isEdit = false;
  searchText = '';
  showDeleteConfirm = false;
  deleteId: number | null = null;
  editId: number | null = null;

  chitTypes = [
    { value: 0, label: 'Lottery' },
    { value: 1, label: 'Auction' },
    { value: 2, label: 'Fixed' }
  ];

  chitGroupForm: FormGroup;

  constructor(
    private http: HttpClient,
    private toastService: ToastService,
    private cdr: ChangeDetectorRef,
    private authService: AuthService,
    private fb: FormBuilder
  ) {
    this.chitGroupForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      chitType: [0, Validators.required],
      totalAmount: [0, [Validators.required, Validators.min(1)]],
      monthlyContribution: [0, [Validators.required, Validators.min(1)]],
      totalMembers: [0, [Validators.required, Validators.min(2)]],
      durationMonths: [0, [Validators.required, Validators.min(1)]],
      startDate: ['', Validators.required]
    });
  }

  get isAdmin(): boolean {
    return this.authService.getRole() === 'Admin';
  }

  get name() { return this.chitGroupForm.get('name')!; }
  get chitType() { return this.chitGroupForm.get('chitType')!; }
  get totalAmount() { return this.chitGroupForm.get('totalAmount')!; }
  get monthlyContribution() { return this.chitGroupForm.get('monthlyContribution')!; }
  get totalMembers() { return this.chitGroupForm.get('totalMembers')!; }
  get durationMonths() { return this.chitGroupForm.get('durationMonths')!; }
  get startDate() { return this.chitGroupForm.get('startDate')!; }

  ngOnInit(): void {
    this.loadChitGroups();
  }

  loadChitGroups(): void {
    this.http.get<ChitGroup[]>(`${environment.apiUrl}/ChitGroup`).subscribe({
      next: (data) => {
        this.chitGroups = Array.isArray(data) ? data : [];
        this.cdr.detectChanges();
      }
    });
  }

  get filteredChitGroups(): ChitGroup[] {
    return this.chitGroups.filter(c =>
      (c.name?.toLowerCase() ?? '').includes(this.searchText.toLowerCase())
    );
  }

  getChitTypeLabel(type: number): string {
    return this.chitTypes.find(t => t.value === type)?.label ?? 'Unknown';
  }

  openAdd(): void {
    this.chitGroupForm.reset({ chitType: 0 });
    this.isEdit = false;
    this.editId = null;
    this.showForm = true;
    this.cdr.detectChanges();
  }

  openEdit(group: ChitGroup): void {
    this.chitGroupForm.patchValue({
      ...group,
      startDate: group.startDate?.split('T')[0]
    });
    this.isEdit = true;
    this.editId = group.id!;
    this.showForm = true;
    this.cdr.detectChanges();
  }

  save(): void {
    if (this.chitGroupForm.invalid) {
      this.chitGroupForm.markAllAsTouched();
      this.toastService.error('Please fill all required fields correctly!');
      return;
    }

    const formData = this.chitGroupForm.value;

    if (this.isEdit) {
      this.http.put(`${environment.apiUrl}/ChitGroup/${this.editId}`, formData).subscribe({
        next: () => {
          this.showForm = false;
          this.cdr.detectChanges();
          this.toastService.success('Chit Group updated successfully!');
          this.loadChitGroups();
        }
      });
    } else {
      this.http.post(`${environment.apiUrl}/ChitGroup`, formData).subscribe({
        next: () => {
          this.showForm = false;
          this.cdr.detectChanges();
          this.toastService.success('Chit Group added successfully!');
          this.loadChitGroups();
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
      this.http.delete(`${environment.apiUrl}/ChitGroup/${this.deleteId}`).subscribe({
        next: () => {
          this.showDeleteConfirm = false;
          this.cdr.detectChanges();
          this.toastService.success('Chit Group deleted successfully!');
          this.loadChitGroups();
        }
      });
    }
  }

  cancel(): void {
    this.showForm = false;
    this.cdr.detectChanges();
  }
}
