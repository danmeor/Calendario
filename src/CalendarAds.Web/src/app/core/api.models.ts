export type CalendarEventType = 'General' | 'Holiday' | 'Promotion' | 'Editorial';

export interface CalendarEvent {
  id: string;
  title: string;
  description?: string | null;
  startDate: string;
  endDate?: string | null;
  colorHex: string;
  type: CalendarEventType | number;
  isPublic: boolean;
}
