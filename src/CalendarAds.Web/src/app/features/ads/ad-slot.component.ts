import { isPlatformBrowser } from '@angular/common';
import { AfterViewInit, ChangeDetectionStrategy, Component, Input, PLATFORM_ID, inject } from '@angular/core';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-ad-slot',
  templateUrl: './ad-slot.component.html',
  styleUrl: './ad-slot.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AdSlotComponent implements AfterViewInit {
  @Input({ required: true }) placement = '';
  @Input() variant: 'leaderboard' | 'rectangle' | 'horizontal' = 'leaderboard';
  @Input() slot = '';

  private readonly platformId = inject(PLATFORM_ID);
  readonly client = environment.adsenseClient;

  ngAfterViewInit(): void {
    if (!this.slot || !isPlatformBrowser(this.platformId)) {
      return;
    }

    const win = window as Window & { adsbygoogle?: unknown[] };
    win.adsbygoogle = win.adsbygoogle ?? [];
    win.adsbygoogle.push({});
  }

  get width(): number {
    return this.variant === 'leaderboard' ? 970 : this.variant === 'horizontal' ? 728 : 300;
  }

  get height(): number {
    return this.variant === 'leaderboard' ? 90 : this.variant === 'horizontal' ? 90 : 250;
  }
}
