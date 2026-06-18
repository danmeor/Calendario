import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { CalendarEvent } from '../../core/api.models';
import { CalendarApiService } from '../../core/calendar-api.service';
import { SeoService } from '../../core/seo.service';
import { AdSlotComponent } from '../ads/ad-slot.component';

interface CalendarDay {
  date: Date;
  day: number;
  events: CalendarEvent[];
}

interface CalendarWeek {
  id: string;
  label: string;
  days: Array<CalendarDay | null>;
}

interface CalendarMonth {
  index: number;
  name: string;
  weeks: CalendarWeek[];
}

@Component({
  selector: 'app-annual-calendar',
  imports: [AdSlotComponent],
  templateUrl: './annual-calendar.component.html',
  styleUrl: './annual-calendar.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AnnualCalendarComponent {
  private readonly api = inject(CalendarApiService);
  private readonly seo = inject(SeoService);

  readonly selectedYear = signal(new Date().getFullYear());
  readonly events = signal<CalendarEvent[]>([]);
  readonly eventsByDate = computed(() => this.indexEventsByDate(this.events()));
  readonly adSlots = environment.adsenseSlots;
  readonly loading = signal(true);

  readonly months = computed(() => this.buildYear(this.selectedYear(), this.eventsByDate()));
  readonly holidays = computed(() => this.events()
    .filter((event) => event.type === 'Holiday' || event.type === 2)
    .sort((a, b) => a.startDate.localeCompare(b.startDate)));

  readonly weekdays = ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'];

  constructor() {
    this.updateSeo();
    this.loadYear();
  }

  changeYear(offset: number): void {
    this.selectedYear.update((year) => year + offset);
    this.updateSeo();
    this.loadYear();
  }

  private async loadYear(): Promise<void> {
    this.loading.set(true);
    try {
      const events = await this.api.getEvents(this.selectedYear());
      this.events.set(events);
    } finally {
      this.loading.set(false);
    }
  }

  dayLabel(day: CalendarDay): string {
    const date = day.date.toLocaleDateString('es-CO', {
      weekday: 'long',
      day: 'numeric',
      month: 'long',
      year: 'numeric'
    });
    const eventText = day.events.length > 0 ? `, ${day.events.map((event) => event.title).join(', ')}` : '';
    return `${date}${eventText}`;
  }

  isHoliday(day: CalendarDay): boolean {
    return day.events.some((event) => event.type === 'Holiday' || event.type === 2);
  }

  dayColor(day: CalendarDay): string {
    const holiday = day.events.find((event) => event.type === 'Holiday' || event.type === 2);
    return holiday?.colorHex ?? day.events[0]?.colorHex ?? '#14b8a6';
  }

  dateKey(date: Date): string {
    return this.toDateKey(date);
  }

  formatEventDate(value: string): string {
    return this.parseDateKey(value).toLocaleDateString('es-CO', {
      day: 'numeric',
      month: 'long'
    });
  }

  private updateSeo(): void {
    const year = this.selectedYear();
    this.seo.update({
      title: `Calendario ${year} Colombia | Dias festivos Colombia`,
      description: `Consulta el calendario ${year} de Colombia con meses completos, domingos y dias festivos nacionales calculados segun las reglas vigentes.`,
      canonicalPath: '/'
    });
    this.seo.setCalendarSchema(year);
  }

  private buildYear(year: number, eventsByDate: Map<string, CalendarEvent[]>): CalendarMonth[] {
    return Array.from({ length: 12 }, (_, monthIndex) => {
      const firstDay = new Date(year, monthIndex, 1);
      const daysInMonth = new Date(year, monthIndex + 1, 0).getDate();

      return {
        index: monthIndex,
        name: firstDay.toLocaleDateString('es-CO', { month: 'long', year: 'numeric' }),
        weeks: this.buildMonthWeeks(year, monthIndex, firstDay.getDay(), daysInMonth, eventsByDate)
      };
    });
  }

  private buildMonthWeeks(
    year: number,
    monthIndex: number,
    leadingBlanks: number,
    daysInMonth: number,
    eventsByDate: Map<string, CalendarEvent[]>
  ): CalendarWeek[] {
    const slots: Array<CalendarDay | null> = [
      ...Array.from({ length: leadingBlanks }, () => null),
      ...Array.from({ length: daysInMonth }, (_, dayIndex) => {
        const date = new Date(year, monthIndex, dayIndex + 1);
        return {
          date,
          day: dayIndex + 1,
          events: eventsByDate.get(this.toDateKey(date)) ?? []
        };
      })
    ];

    while (slots.length % 7 !== 0) {
      slots.push(null);
    }

    return Array.from({ length: slots.length / 7 }, (_, weekIndex) => {
      const days = slots.slice(weekIndex * 7, weekIndex * 7 + 7);
      const labelDate = days.find((day) => day !== null)?.date ?? new Date(year, monthIndex, 1);
      return {
        id: `${year}-${monthIndex}-${weekIndex}`,
        label: `S${this.getIsoWeek(labelDate)}`,
        days
      };
    });
  }

  private indexEventsByDate(events: CalendarEvent[]): Map<string, CalendarEvent[]> {
    const index = new Map<string, CalendarEvent[]>();

    for (const event of events) {
      let current = this.parseDateKey(event.startDate);
      const end = this.parseDateKey(event.endDate ?? event.startDate);

      while (current <= end) {
        const key = this.toDateKey(current);
        index.set(key, [...(index.get(key) ?? []), event]);
        current = new Date(current.getFullYear(), current.getMonth(), current.getDate() + 1);
      }
    }

    return index;
  }

  private parseDateKey(value: string): Date {
    const [year, month, day] = value.split('-').map(Number);
    return new Date(year, month - 1, day);
  }

  private toDateKey(date: Date): string {
    const month = `${date.getMonth() + 1}`.padStart(2, '0');
    const day = `${date.getDate()}`.padStart(2, '0');
    return `${date.getFullYear()}-${month}-${day}`;
  }

  private getIsoWeek(date: Date): number {
    const utcDate = new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate()));
    const dayNumber = utcDate.getUTCDay() || 7;
    utcDate.setUTCDate(utcDate.getUTCDate() + 4 - dayNumber);
    const yearStart = new Date(Date.UTC(utcDate.getUTCFullYear(), 0, 1));
    return Math.ceil((((utcDate.getTime() - yearStart.getTime()) / 86400000) + 1) / 7);
  }
}
