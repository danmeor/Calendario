import { DOCUMENT } from '@angular/common';
import { Injectable, inject } from '@angular/core';
import { Meta, Title } from '@angular/platform-browser';

interface SeoPage {
  title: string;
  description: string;
  canonicalPath?: string;
}

@Injectable({ providedIn: 'root' })
export class SeoService {
  private readonly title = inject(Title);
  private readonly meta = inject(Meta);
  private readonly document = inject(DOCUMENT);
  private readonly siteUrl = 'https://diasfestivoscol.com';

  update(page: SeoPage): void {
    const canonicalUrl = `${this.siteUrl}${page.canonicalPath ?? '/'}`;

    this.title.setTitle(page.title);
    this.meta.updateTag({ name: 'description', content: page.description });
    this.meta.updateTag({ property: 'og:title', content: page.title });
    this.meta.updateTag({ property: 'og:description', content: page.description });
    this.meta.updateTag({ property: 'og:url', content: canonicalUrl });
    this.meta.updateTag({ name: 'twitter:title', content: page.title });
    this.meta.updateTag({ name: 'twitter:description', content: page.description });
    this.setCanonical(canonicalUrl);
  }

  setCalendarSchema(year: number): void {
    const schema = {
      '@context': 'https://schema.org',
      '@type': 'WebPage',
      name: `Calendario ${year} Colombia`,
      description: `Calendario ${year} de Colombia con meses completos, domingos y dias festivos nacionales.`,
      keywords: `calendario ${year} Colombia, dias festivos Colombia, calendario Colombia, festivos Colombia ${year}`,
      inLanguage: 'es-CO',
      url: `${this.siteUrl}/`
    };

    let script = this.document.getElementById('calendar-schema');
    if (!script) {
      script = this.document.createElement('script');
      script.id = 'calendar-schema';
      script.setAttribute('type', 'application/ld+json');
      this.document.head.appendChild(script);
    }

    script.textContent = JSON.stringify(schema);
  }

  private setCanonical(url: string): void {
    let link = this.document.querySelector<HTMLLinkElement>('link[rel="canonical"]');
    if (!link) {
      link = this.document.createElement('link');
      link.rel = 'canonical';
      this.document.head.appendChild(link);
    }

    link.href = url;
  }
}
