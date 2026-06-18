import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { CalendarEvent } from './api.models';
import { createMockEvents } from './mock-data';

@Injectable({ providedIn: 'root' })
export class CalendarApiService {
  getEvents(year: number): Promise<CalendarEvent[]> {
    if (environment.useMockData) {
      return Promise.resolve(createMockEvents(year));
    }

    return this.getJson<CalendarEvent[]>(`${environment.apiBaseUrl}/calendar-events?year=${year}`);
  }

  private async getJson<T>(url: string): Promise<T> {
    const response = await fetch(url);
    if (!response.ok) {
      throw new Error(`Request failed: ${response.status}`);
    }

    return response.json() as Promise<T>;
  }
}
