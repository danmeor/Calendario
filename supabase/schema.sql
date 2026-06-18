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
