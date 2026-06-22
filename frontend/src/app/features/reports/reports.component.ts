import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { ToastService } from '../../core/services/toast.service';

interface MonthlyCollection {
  monthNumber: number;
  monthName: string;
  year: number;
  totalCollected: number;
  totalPayments: number;
  latePayments: number;
}

interface ChitGroupSummary {
  chitGroupId: number;
  chitGroupName: string;
  chitType: string;
  totalAmount: number;
  totalMembers: number;
  enrolledMembers: number;
  totalCollected: number;
  totalWinners: number;
  totalCommission: number;
}

interface CommissionSummary {
  chitGroupId: number;
  chitGroupName: string;
  monthNumber: number;
  totalAmount: number;
  commissionRate: number;
  commissionAmount: number;
  recordedDate: string;
}

interface WinnerSummary {
  chitGroupId: number;
  chitGroupName: string;
  customerName: string;
  monthNumber: number;
  prizeAmount: number;
  commissionDeducted: number;
  netAmount: number;
  selectionType: string;
  wonDate: string;
}

interface ChitGroup {
  id: number;
  name: string;
}

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [CommonModule, FormsModule, MatIconModule],
  templateUrl: './reports.component.html',
  styleUrl: './reports.component.scss'
})
export class ReportsComponent implements OnInit {
  activeTab: 'monthly' | 'groups' | 'commission' | 'winners' = 'monthly';

  monthlyCollection: MonthlyCollection[] = [];
  chitGroupSummary: ChitGroupSummary[] = [];
  commissionSummary: CommissionSummary[] = [];
  winnerSummary: WinnerSummary[] = [];
  chitGroups: ChitGroup[] = [];

  fromDate = '';
  toDate = '';
  selectedChitGroupId = '';

  constructor(
    private http: HttpClient,
    private toastService: ToastService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadChitGroups();
    this.loadMonthlyCollection();
    this.loadChitGroupSummary();
  }

setTab(tab: 'monthly' | 'groups' | 'commission' | 'winners'): void {
  this.activeTab = tab;
  this.cdr.detectChanges();
  if (tab === 'monthly') this.loadMonthlyCollection();
  if (tab === 'groups') this.loadChitGroupSummary();
  if (tab === 'commission') this.loadCommissionSummary();
  if (tab === 'winners') this.loadWinnerSummary();
}

  loadChitGroups(): void {
    this.http.get<ChitGroup[]>(`${environment.apiUrl}/ChitGroup`).subscribe({
      next: (data) => { this.chitGroups = data; this.cdr.detectChanges(); },
      error: () => {}
    });
  }

  buildQueryParams(): string {
    const params: string[] = [];
    if (this.fromDate) params.push(`fromDate=${this.fromDate}`);
    if (this.toDate) params.push(`toDate=${this.toDate}`);
    if (this.selectedChitGroupId) params.push(`chitGroupId=${this.selectedChitGroupId}`);
    return params.length ? `?${params.join('&')}` : '';
  }

applyFilter(): void {
  if (this.activeTab === 'monthly') this.loadMonthlyCollection();
  if (this.activeTab === 'groups') this.loadChitGroupSummary();
  if (this.activeTab === 'commission') this.loadCommissionSummary();
  if (this.activeTab === 'winners') this.loadWinnerSummary();
}

  loadMonthlyCollection(): void {
    const query = this.buildQueryParams();
    this.http.get<MonthlyCollection[]>(`${environment.apiUrl}/Report/monthly-collection${query}`)
      .subscribe({
        next: (data) => {
          this.monthlyCollection = data;
          this.cdr.detectChanges();
        },
        error: () => this.toastService.error('Failed to load monthly collection report!')
      });
  }

  loadChitGroupSummary(): void {
    this.http.get<ChitGroupSummary[]>(`${environment.apiUrl}/Report/chit-group-summary`)
      .subscribe({
        next: (data) => {
          this.chitGroupSummary = data;
          this.cdr.detectChanges();
        },
        error: () => this.toastService.error('Failed to load chit group summary!')
      });
  }

  loadCommissionSummary(): void {
    const query = this.buildQueryParams();
    this.http.get<CommissionSummary[]>(`${environment.apiUrl}/Report/commission-summary${query}`)
      .subscribe({
        next: (data) => {
          this.commissionSummary = data;
          this.cdr.detectChanges();
        },
        error: () => this.toastService.error('Failed to load commission summary!')
      });
  }

  loadWinnerSummary(): void {
    const query = this.buildQueryParams();
    this.http.get<WinnerSummary[]>(`${environment.apiUrl}/Report/winner-summary${query}`)
      .subscribe({
        next: (data) => {
          this.winnerSummary = data;
          this.cdr.detectChanges();
        },
        error: () => this.toastService.error('Failed to load winner summary!')
      });
  }

  get totalCollectedSum(): number {
    return this.monthlyCollection.reduce((sum, m) => sum + m.totalCollected, 0);
  }

  get totalCommissionSum(): number {
    return this.commissionSummary.reduce((sum, c) => sum + c.commissionAmount, 0);
  }
}
