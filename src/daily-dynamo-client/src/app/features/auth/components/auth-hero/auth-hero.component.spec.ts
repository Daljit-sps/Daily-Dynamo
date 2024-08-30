import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthHeroComponent } from './auth-hero.component';

describe('AuthHeroComponent', () => {
  let component: AuthHeroComponent;
  let fixture: ComponentFixture<AuthHeroComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AuthHeroComponent]
    });
    fixture = TestBed.createComponent(AuthHeroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
