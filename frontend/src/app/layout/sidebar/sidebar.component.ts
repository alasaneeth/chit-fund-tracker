import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive, MatIconModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss'
})
export class SidebarComponent {
  role: string;

  constructor(private authService: AuthService) {
    this.role = this.authService.getRole();
  }

  onLogout(): void {
    this.authService.logout();
  }

  get isAdmin(): boolean {
    return this.role === 'Admin';
  }

  get isCashier(): boolean {
    return this.role === 'Cashier';
  }

  get isAdminOrCashier(): boolean {
    return this.role === 'Admin' || this.role === 'Cashier';
  }
}
