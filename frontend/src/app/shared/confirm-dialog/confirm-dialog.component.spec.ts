import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ConfirmDialogComponent } from './confirm-dialog.component';

describe('ConfirmDialogComponent', () => {
  let component: ConfirmDialogComponent;
  let fixture: ComponentFixture<ConfirmDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConfirmDialogComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(ConfirmDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should have default title and message', () => {
    expect(component.title).toBe('Confirm');
    expect(component.message).toBe('Are you sure?');
  });

it('should accept custom title and message via input', () => {
  const newFixture = TestBed.createComponent(ConfirmDialogComponent);
  const newComponent = newFixture.componentInstance;

  newComponent.title = 'Delete Customer';
  newComponent.message = 'Are you sure you want to delete this customer?';
  newFixture.detectChanges();

  expect(newComponent.title).toBe('Delete Customer');
  expect(newComponent.message).toBe('Are you sure you want to delete this customer?');
});

  it('should emit confirmed event when confirm is triggered', () => {
    spyOn(component.confirmed, 'emit');

    component.confirmed.emit();

    expect(component.confirmed.emit).toHaveBeenCalled();
  });

  it('should emit cancelled event when cancel is triggered', () => {
    spyOn(component.cancelled, 'emit');

    component.cancelled.emit();

    expect(component.cancelled.emit).toHaveBeenCalled();
  });
});
