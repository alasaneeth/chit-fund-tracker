import { Component, OnInit } from '@angular/core';
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

  ngOnInit(): void {
    this.fullName = localStorage.getItem('fullName') ?? '';
    this.role = localStorage.getItem('role') ?? '';
  }
}
