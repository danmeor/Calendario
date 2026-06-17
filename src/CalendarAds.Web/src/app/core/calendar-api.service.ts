import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Advertisement, CalendarEvent } from './api.models';
import { createMockAds, createMockEvents } from './mock-data';

@Injectable({ providedIn: 'root' })
export class CalendarApiService {
  getEvents(year: number): Promise<CalendarEvent[]> {
    if (environment.useMockData) {
      return Promise.resolve(createMockEvents(year));
    }

    return this.getJson<CalendarEvent[]>(`${environment.apiBaseUrl}/calendar-events?year=${year}`);
  }

  getAdvertisements(placement: string, take = 1, randomize = true): Promise<Advertisement[]> {
    if (environment.useMockData) {
      return Promise.resolve(createMockAds(placement).slice(0, take));
    }

    const params = new URLSearchParams({ placement, take: String(take), randomize: String(randomize) });
    return this.getJson<Advertisement[]>(`${environment.apiBaseUrl}/advertisements?${params}`);
  }

  trackAdMetric(adId: string, type: 'Impression' | 'Click'): Promise<void> {
    if (environment.useMockData) {
      return Promise.resolve();
    }

    return fetch(`${environment.apiBaseUrl}/advertisements/${adId}/metrics/${type}`, {
      method: 'POST'
    }).then(() => undefined);
  }

  private async getJson<T>(url: string): Promise<T> {
    const response = await fetch(url);
    if (!response.ok) {
      throw new Error(`Request failed: ${response.status}`);
    }

    return response.json() as Promise<T>;
  }
}
