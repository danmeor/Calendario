import { CalendarEvent } from './api.models';

interface HolidayRule {
  title: string;
  month: number;
  day: number;
  moveToMonday: boolean;
  startYear?: number;
}

const fixedRules: HolidayRule[] = [
  { title: 'Año Nuevo', month: 1, day: 1, moveToMonday: false },
  { title: 'Día del Trabajo', month: 5, day: 1, moveToMonday: false },
  { title: 'Día de la Independencia de Colombia', month: 7, day: 20, moveToMonday: false },
  { title: 'Batalla de Boyacá', month: 8, day: 7, moveToMonday: false },
  { title: 'Inmaculada Concepción', month: 12, day: 8, moveToMonday: false },
  { title: 'Navidad', month: 12, day: 25, moveToMonday: false },
  { title: 'Día de los Reyes Magos', month: 1, day: 6, moveToMonday: true },
  { title: 'Día de San José', month: 3, day: 19, moveToMonday: true },
  { title: 'San Pedro y San Pablo', month: 6, day: 29, moveToMonday: true },
  { title: 'Nuestra Señora del Rosario de Chiquinquirá', month: 7, day: 9, moveToMonday: true, startYear: 2026 },
  { title: 'Asunción de la Virgen', month: 8, day: 15, moveToMonday: true },
  { title: 'Día de la Raza', month: 10, day: 12, moveToMonday: true },
  { title: 'Todos los Santos', month: 11, day: 1, moveToMonday: true },
  { title: 'Independencia de Cartagena', month: 11, day: 11, moveToMonday: true }
];

export function getColombiaHolidays(year: number): CalendarEvent[] {
  const easter = getEasterSunday(year);
  const movable = [
    holidayFromDate('Jueves Santo', addDays(easter, -3), false),
    holidayFromDate('Viernes Santo', addDays(easter, -2), false),
    holidayFromDate('Ascensión del Señor', addDays(easter, 39), true),
    holidayFromDate('Corpus Christi', addDays(easter, 60), true),
    holidayFromDate('Sagrado Corazón de Jesús', addDays(easter, 68), true)
  ];

  const fixed = fixedRules
    .filter((rule) => !rule.startYear || year >= rule.startYear)
    .map((rule) => holidayFromDate(
      rule.title,
      new Date(year, rule.month - 1, rule.day),
      rule.moveToMonday
    ));

  return [...fixed, ...movable]
    .sort((a, b) => a.startDate.localeCompare(b.startDate))
    .map((event, index) => ({ ...event, id: `co-holiday-${year}-${index}` }));
}

function holidayFromDate(title: string, date: Date, moveToMonday: boolean): CalendarEvent {
  const observed = moveToMonday ? nextMonday(date) : date;
  const officialDate = toDateKey(date);
  const observedDate = toDateKey(observed);
  const description = officialDate === observedDate
    ? 'Festivo oficial en Colombia.'
    : `Festivo trasladado por Ley Emiliani desde ${officialDate}.`;

  return {
    id: '',
    title,
    description,
    startDate: observedDate,
    endDate: null,
    colorHex: '#ef4444',
    type: 'Holiday',
    isPublic: true
  };
}

function nextMonday(date: Date): Date {
  const offset = (8 - date.getDay()) % 7;
  return addDays(date, offset);
}

function addDays(date: Date, days: number): Date {
  return new Date(date.getFullYear(), date.getMonth(), date.getDate() + days);
}

function toDateKey(date: Date): string {
  return `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}-${String(date.getDate()).padStart(2, '0')}`;
}

function getEasterSunday(year: number): Date {
  const a = year % 19;
  const b = Math.floor(year / 100);
  const c = year % 100;
  const d = Math.floor(b / 4);
  const e = b % 4;
  const f = Math.floor((b + 8) / 25);
  const g = Math.floor((b - f + 1) / 3);
  const h = (19 * a + b - d - g + 15) % 30;
  const i = Math.floor(c / 4);
  const k = c % 4;
  const l = (32 + 2 * e + 2 * i - h - k) % 7;
  const m = Math.floor((a + 11 * h + 22 * l) / 451);
  const month = Math.floor((h + l - 7 * m + 114) / 31);
  const day = ((h + l - 7 * m + 114) % 31) + 1;
  return new Date(year, month - 1, day);
}
