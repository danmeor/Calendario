create table if not exists public.calendar_events (
  id uuid primary key,
  title varchar(140) not null,
  description varchar(800),
  start_date date not null,
  end_date date,
  color_hex varchar(7) not null default '#14b8a6',
  type integer not null default 1,
  is_public boolean not null default true,
  created_at timestamptz not null default now()
);

create index if not exists ix_calendar_events_start_date
  on public.calendar_events (start_date);

create table if not exists public.advertisements (
  id uuid primary key,
  title varchar(160) not null,
  image_url varchar(1000) not null,
  target_url varchar(1000) not null,
  placement integer not null default 1,
  starts_on date not null,
  ends_on date not null,
  is_active boolean not null default true,
  priority integer not null default 1,
  created_at timestamptz not null default now()
);

create index if not exists ix_advertisements_placement_active_dates
  on public.advertisements (placement, is_active, starts_on, ends_on);

create table if not exists public.ad_metrics (
  id uuid primary key,
  advertisement_id uuid not null references public.advertisements(id) on delete cascade,
  type integer not null,
  user_agent varchar(600),
  ip_hash varchar(128),
  occurred_at timestamptz not null default now()
);

create index if not exists ix_ad_metrics_ad_type_time
  on public.ad_metrics (advertisement_id, type, occurred_at);

insert into public.advertisements (
  id, title, image_url, target_url, placement, starts_on, ends_on, is_active, priority
) values
  ('00000000-0000-0000-0000-000000000101', 'Anuncia aqui - superior', '/assets/ad-top.svg', 'mailto:ventas@diasfestivoscol.com?subject=Quiero%20anunciar', 1, '2026-01-01', '2030-12-31', true, 1),
  ('00000000-0000-0000-0000-000000000102', 'Anuncia aqui - lateral izquierdo', '/assets/ad-left-a.svg', 'mailto:ventas@diasfestivoscol.com?subject=Quiero%20anunciar', 4, '2026-01-01', '2030-12-31', true, 1),
  ('00000000-0000-0000-0000-000000000103', 'Anuncia aqui - lateral derecho', '/assets/ad-right-a.svg', 'mailto:ventas@diasfestivoscol.com?subject=Quiero%20anunciar', 2, '2026-01-01', '2030-12-31', true, 1),
  ('00000000-0000-0000-0000-000000000104', 'Anuncia aqui - inferior', '/assets/ad-bottom.svg', 'mailto:ventas@diasfestivoscol.com?subject=Quiero%20anunciar', 5, '2026-01-01', '2030-12-31', true, 1)
on conflict (id) do nothing;
