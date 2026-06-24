import { TestBed } from '@angular/core/testing';
import { ToastService } from './toast.service';

describe('ToastService', () => {
  let service: ToastService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ToastService);
  });

  afterEach(() => {
    jasmine.clock().uninstall();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should have null toast initially', () => {
    expect(service.toast()).toBeNull();
  });

  it('should set success toast message', () => {
    service.success('Customer added successfully!');

    expect(service.toast()).toEqual({
      message: 'Customer added successfully!',
      type: 'success'
    });
  });

  it('should set error toast message', () => {
    service.error('Failed to load customers!');

    expect(service.toast()).toEqual({
      message: 'Failed to load customers!',
      type: 'error'
    });
  });

  it('should set warning toast message', () => {
    service.warning('Winner edit is not supported!');

    expect(service.toast()).toEqual({
      message: 'Winner edit is not supported!',
      type: 'warning'
    });
  });

  it('should clear toast after 3 seconds', () => {
    jasmine.clock().install();

    service.success('Test message');
    expect(service.toast()).not.toBeNull();

    jasmine.clock().tick(3001);
    expect(service.toast()).toBeNull();
  });
});
