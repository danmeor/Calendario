import { Component } from '@angular/core';
import { AnnualCalendarComponent } from './features/calendar/annual-calendar.component';

@Component({
  selector: 'app-root',
  imports: [AnnualCalendarComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {}
