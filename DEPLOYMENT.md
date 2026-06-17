# Despliegue: GitHub + Vercel + Supabase

Este proyecto queda dividido asi:

- **Angular**: `src/CalendarAds.Web`, se publica en Vercel.
- **ASP.NET Core API**: `src/CalendarAds.Api`, queda lista para PostgreSQL/Supabase, pero no corre como API completa dentro de Vercel.
- **Supabase**: base de datos PostgreSQL.

## 1. GitHub

Repositorio objetivo:

```text
https://github.com/danmeor/Calendario
```

Comandos desde la raiz del proyecto:

```powershell
git remote add origin https://github.com/danmeor/Calendario.git
git add .
git commit -m "Prepare calendar deployment with PostgreSQL and Vercel"
git push -u origin master
```

Si GitHub pide usuario/token, usa un Personal Access Token con permiso para ese repositorio.

## 2. Supabase PostgreSQL

Proyecto:

```text
https://fhkqmdfappxoznahwmqy.supabase.co
```

En Supabase:

1. Entra al panel del proyecto.
2. Ve a **SQL Editor**.
3. Abre el archivo `supabase/schema.sql`.
4. Copia y ejecuta todo el SQL.

La API ya usa PostgreSQL con Npgsql. La cadena base esta en:

```text
src/CalendarAds.Api/appsettings.json
```

Formato esperado:

```text
Host=db.fhkqmdfappxoznahwmqy.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=TU_PASSWORD;SSL Mode=Require;Trust Server Certificate=true
```

Para produccion no subas la clave al repositorio. Configurala como variable de entorno:

```text
ConnectionStrings__DefaultConnection
```

## 3. Vercel

Cuenta/proyecto:

```text
https://vercel.com/ositos
```

En Vercel:

1. Importa el repositorio `danmeor/Calendario`.
2. Usa la raiz del repositorio.
3. Vercel leera `vercel.json`.
4. Build command:

```text
cd src/CalendarAds.Web && npm ci && npm run build
```

5. Output directory:

```text
src/CalendarAds.Web/dist/calendar-ads-web/browser
```

La version actual de Angular esta en modo demo:

```text
src/CalendarAds.Web/src/environments/environment.ts
```

```ts
useMockData: true
```

Esto permite que el sitio funcione en Vercel sin API desplegada.

## 4. Dominio diasfestivoscol.com

En Vercel, abre el proyecto y ve a:

```text
Settings -> Domains
```

Agrega:

```text
diasfestivoscol.com
www.diasfestivoscol.com
```

En tu DNS actual, cambia los registros de la imagen:

```text
A      @      76.76.21.21
CNAME  www    cname.vercel-dns.com
```

TTL recomendado: `300` mientras haces pruebas.

La propagacion DNS puede tardar desde minutos hasta 24-48 horas.

## 5. API .NET

Vercel no es el lugar ideal para hospedar una API ASP.NET Core completa.

Opciones recomendadas para la API:

- Azure App Service
- Render
- Railway
- Fly.io
- VPS

Cuando la API este publicada:

1. Cambia `useMockData` a `false`.
2. Cambia `apiBaseUrl` a la URL real de la API.
3. Vuelve a desplegar Vercel.

## 6. Validacion local

Frontend:

```powershell
.\serve-web-prod.cmd
```

Abrir:

```text
http://127.0.0.1:4300
```

Backend:

```powershell
.\start-api.cmd
```
