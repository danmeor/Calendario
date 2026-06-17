export type CalendarEventType = 'General' | 'Holiday' | 'Promotion' | 'Editorial';
export type AdPlacement = 'TopBanner' | 'RightRail' | 'BetweenMonths' | 'LeftRail' | 'BottomBanner';

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

export interface Advertisement {
  id: string;
  title: string;
  imageUrl: string;
  targetUrl: string;
  placement: AdPlacement | number;
  startsOn: string;
  endsOn: string;
  isActive: boolean;
  priority: number;
}
