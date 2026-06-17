using CalendarAds.Api.Contracts;
using CalendarAds.Api.Domain;

namespace CalendarAds.Api.Services;

public static class ColombiaHolidayProvider
{
    public static IReadOnlyList<CalendarEventDto> GetHolidays(int year)
    {
        var holidays = new List<HolidayDefinition>
        {
            new("Año Nuevo", new DateOnly(year, 1, 1), false),
            new("Día del Trabajo", new DateOnly(year, 5, 1), false),
            new("Día de la Independencia de Colombia", new DateOnly(year, 7, 20), false),
            new("Batalla de Boyacá", new DateOnly(year, 8, 7), false),
            new("Inmaculada Concepción", new DateOnly(year, 12, 8), false),
            new("Navidad", new DateOnly(year, 12, 25), false),
            new("Día de los Reyes Magos", new DateOnly(year, 1, 6), true),
            new("Día de San José", new DateOnly(year, 3, 19), true),
            new("San Pedro y San Pablo", new DateOnly(year, 6, 29), true),
            new("Asunción de la Virgen", new DateOnly(year, 8, 15), true),
            new("Día de la Raza", new DateOnly(year, 10, 12), true),
            new("Todos los Santos", new DateOnly(year, 11, 1), true),
            new("Independencia de Cartagena", new DateOnly(year, 11, 11), true)
        };

        if (year >= 2026)
        {
            holidays.Add(new HolidayDefinition("Nuestra Señora del Rosario de Chiquinquirá", new DateOnly(year, 7, 9), true));
        }

        var easter = GetEasterSunday(year);
        holidays.AddRange([
            new("Jueves Santo", easter.AddDays(-3), false),
            new("Viernes Santo", easter.AddDays(-2), false),
            new("Ascensión del Señor", easter.AddDays(39), true),
            new("Corpus Christi", easter.AddDays(60), true),
            new("Sagrado Corazón de Jesús", easter.AddDays(68), true)
        ]);

        return holidays
            .Select((holiday, index) => ToDto(holiday, year, index))
            .OrderBy(holiday => holiday.StartDate)
            .ToList();
    }

    private static CalendarEventDto ToDto(HolidayDefinition holiday, int year, int index)
    {
        var observed = holiday.MoveToMonday ? NextMonday(holiday.Date) : holiday.Date;
        var description = observed == holiday.Date
            ? "Festivo oficial en Colombia."
            : $"Festivo trasladado por Ley Emiliani desde {holiday.Date:yyyy-MM-dd}.";

        return new CalendarEventDto(
            CreateStableId(year, index),
            holiday.Title,
            description,
            observed,
            null,
            "#ef4444",
            CalendarEventType.Holiday,
            true);
    }

    private static DateOnly NextMonday(DateOnly date)
    {
        var offset = ((int)DayOfWeek.Monday - (int)date.DayOfWeek + 7) % 7;
        return date.AddDays(offset);
    }

    private static DateOnly GetEasterSunday(int year)
    {
        var a = year % 19;
        var b = year / 100;
        var c = year % 100;
        var d = b / 4;
        var e = b % 4;
        var f = (b + 8) / 25;
        var g = (b - f + 1) / 3;
        var h = (19 * a + b - d - g + 15) % 30;
        var i = c / 4;
        var k = c % 4;
        var l = (32 + 2 * e + 2 * i - h - k) % 7;
        var m = (a + 11 * h + 22 * l) / 451;
        var month = (h + l - 7 * m + 114) / 31;
        var day = ((h + l - 7 * m + 114) % 31) + 1;
        return new DateOnly(year, month, day);
    }

    private static Guid CreateStableId(int year, int index)
    {
        var bytes = new byte[16];
        BitConverter.GetBytes(year).CopyTo(bytes, 0);
        BitConverter.GetBytes(index).CopyTo(bytes, 4);
        return new Guid(bytes);
    }

    private sealed record HolidayDefinition(string Title, DateOnly Date, bool MoveToMonday);
}
