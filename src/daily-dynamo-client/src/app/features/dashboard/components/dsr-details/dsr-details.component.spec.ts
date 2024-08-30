import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DsrDetailsComponent } from './dsr-details.component';

describe('DsrDetailsComponent', () => {
  let component: DsrDetailsComponent;
  let fixture: ComponentFixture<DsrDetailsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DsrDetailsComponent]
    });
    fixture = TestBed.createComponent(DsrDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
