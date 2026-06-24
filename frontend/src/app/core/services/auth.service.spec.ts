import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';
import { environment } from '../../../environments/environment';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(() => {
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        AuthService,
        { provide: Router, useValue: routerSpy }
      ]
    });

    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
    localStorage.clear();
  });

  afterEach(() => {
    httpMock.verify();
    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should login and store token in localStorage', () => {
    const mockResponse = {
      token: 'fake-jwt-token',
      fullName: 'Admin User',
      email: 'admin@test.com',
      role: 'Admin'
    };

    service.login({ email: 'admin@test.com', password: '123456' })
      .subscribe(response => {
        expect(response.token).toBe('fake-jwt-token');
      });

    const req = httpMock.expectOne(`${environment.apiUrl}/Auth/login`);
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse);

    expect(localStorage.getItem('token')).toBe('fake-jwt-token');
    expect(localStorage.getItem('fullName')).toBe('Admin User');
    expect(localStorage.getItem('role')).toBe('Admin');
  });

  it('should return true when token exists (isLoggedIn)', () => {
    localStorage.setItem('token', 'sample-token');
    expect(service.isLoggedIn()).toBeTrue();
  });

  it('should return false when token does not exist (isLoggedIn)', () => {
    expect(service.isLoggedIn()).toBeFalse();
  });

  it('should return stored role (getRole)', () => {
    localStorage.setItem('role', 'Cashier');
    expect(service.getRole()).toBe('Cashier');
  });

  it('should return empty string when role not set (getRole)', () => {
    expect(service.getRole()).toBe('');
  });

  it('should clear localStorage and navigate to login on logout', () => {
    localStorage.setItem('token', 'sample-token');

    service.logout();

    expect(localStorage.getItem('token')).toBeNull();
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/login']);
  });
});
