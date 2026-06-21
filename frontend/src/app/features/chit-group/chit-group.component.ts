import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
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
  monthlyAmount: number;
  maxMembers: number;
  durationMonths: number;
  startDate: string;
  status: number;
}

@Component({
  selector: 'app-chit-group',
  standalone: true,
  imports: [CommonModule, FormsModule, MatIconModule, ConfirmDialogComponent],
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

  chitTypes = [
    { value: 1, label: 'Lottery' },
    { value: 2, label: 'Auction' },
    { value: 3, label: 'Fixed' }
  ];

  form: ChitGroup = {
    name: '', chitType: 0, totalAmount: 0,
    monthlyAmount: 0, maxMembers: 0,
    durationMonths: 0, startDate: '', status: 0
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
    this.loadChitGroups();
  }

  loadChitGroups(): void {
    this.http.get<ChitGroup[]>(`${environment.apiUrl}/ChitGroup`).subscribe({
      next: (data) => {
        this.chitGroups = Array.isArray(data) ? data : [];
        this.cdr.detectChanges();
      },
      error: () => this.toastService.error('Failed to load chit groups!')
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
    this.form = {
      name: '', chitType: 0, totalAmount: 0,
      monthlyAmount: 0, maxMembers: 0,
      durationMonths: 0, startDate: '', status: 0
    };
    this.isEdit = false;
    this.showForm = true;
    this.cdr.detectChanges();
  }

  openEdit(group: ChitGroup): void {
    this.form = { ...group };
    this.isEdit = true;
    this.showForm = true;
    this.cdr.detectChanges();
  }

save(): void {
  if (this.isEdit) {
    this.http.put(`${environment.apiUrl}/ChitGroup/${this.form.id}`, this.form).subscribe({
      next: () => {
        this.showForm = false;
        this.cdr.detectChanges();
        this.toastService.success('Chit Group updated successfully!');
        this.loadChitGroups();
      },
      error: () => this.toastService.error('Failed to update chit group!')
    });
  } else {
    this.http.post(`${environment.apiUrl}/ChitGroup`, this.form).subscribe({
      next: () => {
        this.showForm = false;
        this.cdr.detectChanges();
        this.toastService.success('Chit Group added successfully!');
        this.loadChitGroups();
      },
      error: () => this.toastService.error('Failed to add chit group!')
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
          this.toastService.success('Chit Group deleted successfully!');
          this.loadChitGroups();
          this.showDeleteConfirm = false;
          this.cdr.detectChanges();
        },
        error: () => this.toastService.error('Failed to delete chit group!')
      });
    }
  }

  cancel(): void {
    this.showForm = false;
    this.cdr.detectChanges();
  }
}
