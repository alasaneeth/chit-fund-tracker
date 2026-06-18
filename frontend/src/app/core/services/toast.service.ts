import { Injectable, signal } from '@angular/core';

export interface ToastMessage {
  message: string;
  type: 'success' | 'error' | 'warning';
}

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  toast = signal<ToastMessage | null>(null);

  success(message: string): void {
    this.toast.set({ message, type: 'success' });
    setTimeout(() => this.toast.set(null), 3000);
  }

  error(message: string): void {
    this.toast.set({ message, type: 'error' });
    setTimeout(() => this.toast.set(null), 3000);
  }

  warning(message: string): void {
    this.toast.set({ message, type: 'warning' });
    setTimeout(() => this.toast.set(null), 3000);
  }
}
