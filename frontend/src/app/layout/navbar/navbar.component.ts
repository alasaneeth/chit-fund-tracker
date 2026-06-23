import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [MatIconModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent implements OnInit {
  fullName = '';
  role = '';

  @Output() menuToggle = new EventEmitter<void>();

  ngOnInit(): void {
    this.fullName = localStorage.getItem('fullName') ?? '';
    this.role = localStorage.getItem('role') ?? '';
  }

  onMenuToggle(): void {
    this.menuToggle.emit();
  }
}
