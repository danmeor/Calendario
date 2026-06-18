import { CalendarEvent } from './api.models';
import { getColombiaHolidays } from './colombia-holidays';

export function createMockEvents(year: number): CalendarEvent[] {
  return getColombiaHolidays(year);
}
