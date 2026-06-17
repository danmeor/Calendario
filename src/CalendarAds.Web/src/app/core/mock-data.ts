import { AdPlacement, Advertisement, CalendarEvent } from './api.models';
import { getColombiaHolidays } from './colombia-holidays';

export function createMockEvents(year: number): CalendarEvent[] {
  return [
    ...getColombiaHolidays(year),
    {
      id: 'mock-event-1',
      title: 'Lanzamiento editorial',
      description: 'Fecha destacada de ejemplo para validar el diseno.',
      startDate: `${year}-06-12`,
      endDate: null,
      colorHex: '#f97316',
      type: 'Editorial',
      isPublic: true
    },
    {
      id: 'mock-event-2',
      title: 'Campana patrocinada',
      description: 'Rango de fechas patrocinado.',
      startDate: `${year}-07-13`,
      endDate: `${year}-07-20`,
      colorHex: '#14b8a6',
      type: 'Promotion',
      isPublic: true
    },
    {
      id: 'mock-event-3',
      title: 'Festivo comercial',
      description: 'Evento de muestra para revisar estilos.',
      startDate: `${year}-12-25`,
      endDate: null,
      colorHex: '#0ea5e9',
      type: 'Holiday',
      isPublic: true
    }
  ];
}

export function createMockAds(placement: string): Advertisement[] {
  const ads: Record<string, Array<{ title: string; imageUrl: string; placement: AdPlacement }>> = {
    TopBanner: [
      { title: 'Anuncia aqui - banner superior', imageUrl: 'assets/ad-top.svg', placement: 'TopBanner' }
    ],
    LeftRail: [
      { title: 'Publicidad lateral izquierda A', imageUrl: 'assets/ad-left-a.svg', placement: 'LeftRail' },
      { title: 'Publicidad lateral izquierda B', imageUrl: 'assets/ad-left-b.svg', placement: 'LeftRail' }
    ],
    RightRail: [
      { title: 'Publicidad lateral derecha A', imageUrl: 'assets/ad-right-a.svg', placement: 'RightRail' },
      { title: 'Publicidad lateral derecha B', imageUrl: 'assets/ad-right-b.svg', placement: 'RightRail' }
    ],
    BottomBanner: [
      { title: 'Banner inferior del calendario', imageUrl: 'assets/ad-bottom.svg', placement: 'BottomBanner' }
    ]
  };

  return (ads[placement] ?? []).map((ad, index) => ({
    id: `mock-ad-${placement}-${index}`,
    title: ad.title,
    imageUrl: ad.imageUrl,
    targetUrl: 'mailto:ventas@tu-dominio.com?subject=Quiero%20anunciar',
    placement: ad.placement,
    startsOn: '2026-01-01',
    endsOn: '2030-12-31',
    isActive: true,
    priority: 1
  }));
}
