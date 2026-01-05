import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Bill } from './bill';

describe('Bill', () => {
  let component: Bill;
  let fixture: ComponentFixture<Bill>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [Bill]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Bill);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
