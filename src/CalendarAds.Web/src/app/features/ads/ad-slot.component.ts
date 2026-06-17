import { ChangeDetectionStrategy, Component, Input, OnChanges, inject } from '@angular/core';
import { Advertisement } from '../../core/api.models';
import { CalendarApiService } from '../../core/calendar-api.service';

@Component({
  selector: 'app-ad-slot',
  templateUrl: './ad-slot.component.html',
  styleUrl: './ad-slot.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AdSlotComponent implements OnChanges {
  @Input({ required: true }) placement = '';
  @Input() variant: 'leaderboard' | 'rectangle' | 'horizontal' = 'leaderboard';
  @Input() ads: Advertisement[] = [];

  private readonly api = inject(CalendarApiService);
  private trackedIds = new Set<string>();

  get primaryAd(): Advertisement | undefined {
    return this.ads[0];
  }

  get imageUrl(): string {
    const url = this.primaryAd?.imageUrl ?? '';
    return url.startsWith('/') ? url.slice(1) : url;
  }

  ngOnChanges(): void {
    const ad = this.primaryAd;
    if (!ad || this.trackedIds.has(ad.id)) {
      return;
    }

    this.trackedIds.add(ad.id);
    void this.api.trackAdMetric(ad.id, 'Impression');
  }

  onAdClick(ad: Advertisement): void {
    void this.api.trackAdMetric(ad.id, 'Click');
  }

  get width(): number {
    return this.variant === 'leaderboard' ? 970 : this.variant === 'horizontal' ? 728 : 300;
  }

  get height(): number {
    return this.variant === 'leaderboard' ? 90 : this.variant === 'horizontal' ? 90 : 250;
  }

  get isLazy(): boolean {
    return this.variant !== 'leaderboard';
  }
}
