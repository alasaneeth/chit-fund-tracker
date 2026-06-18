import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  stats = {
    totalGroups: 0,
    totalCustomers: 0,
    totalCollections: 0,
    activeEnrollments: 0
  };

  constructor(
    private http: HttpClient,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadStats();
  }

  loadStats(): void {
    this.http.get<any[]>(`${environment.apiUrl}/ChitGroup`).subscribe({
      next: (data) => {
        this.stats.totalGroups = data.length;
        this.cdr.detectChanges();
      }
    });

    this.http.get<any[]>(`${environment.apiUrl}/Customer`).subscribe({
      next: (data) => {
        this.stats.totalCustomers = data.length;
        this.cdr.detectChanges();
      }
    });

    this.http.get<any[]>(`${environment.apiUrl}/Enrollment`).subscribe({
      next: (data) => {
        this.stats.activeEnrollments = data.length;
        this.cdr.detectChanges();
      }
    });

    this.http.get<any[]>(`${environment.apiUrl}/Payment`).subscribe({
      next: (data) => {
        this.stats.totalCollections = data.length;
        this.cdr.detectChanges();
      }
    });
  }
}
