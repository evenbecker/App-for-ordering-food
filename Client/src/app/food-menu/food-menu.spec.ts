import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FoodMenu } from './food-menu';

describe('FoodMenu', () => {
  let component: FoodMenu;
  let fixture: ComponentFixture<FoodMenu>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FoodMenu]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FoodMenu);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
