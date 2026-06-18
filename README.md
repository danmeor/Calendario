# Dias Festivos Colombia

Aplicacion web para consultar el calendario anual de Colombia con domingos y dias festivos nacionales.

## Arquitectura

- `src/CalendarAds.Api`: ASP.NET Core 9 Web API, Entity Framework Core 9 y PostgreSQL/Supabase para eventos del calendario.
- `src/CalendarAds.Web`: Angular 22 con componentes standalone.
- Base de datos recomendada: PostgreSQL en Supabase.

## Requisitos

- .NET SDK 9.
- Node.js compatible con Angular 22: `^22.22.3`, `^24.15.0` o `^26.0.0`.
- Supabase PostgreSQL para produccion.

## Ejecutar API

```powershell
dotnet run --project src\CalendarAds.Api
```

En Windows tambien puedes ejecutar:

```powershell
.\start-api.cmd
```

La cadena de conexion PostgreSQL/Supabase esta en `src/CalendarAds.Api/appsettings.json`. Para produccion, usa la variable de entorno `ConnectionStrings__DefaultConnection`.

La API no es la pagina visual. Si ves mensajes como `Now listening on: http://localhost:5259`, significa que el backend esta abierto correctamente.

## Ejecutar Angular

```powershell
cd src\CalendarAds.Web
npm install
npm start
```

Si PowerShell muestra que `npm.ps1` esta bloqueado, usa:

```powershell
cd src\CalendarAds.Web
npm.cmd install
npm.cmd start
```

Si `npm.cmd` tampoco aparece porque Windows no refresco el PATH, ejecuta desde la carpeta raiz:

```powershell
.\install-web.cmd
.\start-web.cmd
```

Para medir Lighthouse usa la version de produccion, no `ng serve`:

```powershell
.\serve-web-prod.cmd
```

Luego abre `http://127.0.0.1:4300`.

Si el diagnostico muestra scripts de `kaspersky-labs.com`, eso no viene del proyecto: es el antivirus inyectando JavaScript en la pagina local. Para una medicion mas limpia usa Chrome en modo invitado/incognito o mide en un dominio real con HTTPS.

O usa la ruta completa:

```powershell
& "C:\Program Files\nodejs\npm.cmd" start
```

Si el puerto 4200 ya esta ocupado, cierra la terminal donde corre Angular o ejecuta:

```powershell
netstat -ano | Select-String ':4200'
Stop-Process -Id <PID> -Force
```

El frontend espera la API en `http://localhost:5259/api`. Si tu API levanta en otro puerto, cambia `src/CalendarAds.Web/src/environments/environment.ts`.

Para trabajar primero solo el diseno, `useMockData` esta en `true` dentro de `src/CalendarAds.Web/src/environments/environment.ts`. Asi Angular usa datos de muestra y no necesita base de datos.

Si no tienes Node/NPM instalado, abre `design-preview.html` directamente en el navegador para revisar la maqueta visual sin backend, base de datos ni Angular.

## SEO, AdSense y rendimiento

- El dominio canonico configurado es `https://www.diasfestivoscol.com/`, porque Vercel esta sirviendo la version con `www`.
- El HTML inicial incluye contenido semantico para crawlers y usuarios sin JavaScript.
- La API usa compresion y cache de salida para eventos publicos.
- La publicidad propia fue retirada: no se guardan anuncios en la base de datos.
- Google AdSense queda preparado con el script oficial y espacios manuales opcionales. Mientras no tengas IDs reales de bloques de anuncio, los espacios no renderizan anuncios falsos.
- Para que Google pueda mostrar anuncios automaticos aleatorios, activa Auto Ads desde tu cuenta de AdSense cuando el sitio sea aprobado.

## Endpoints iniciales

- `GET /api/calendar-events?year=2026`
- `POST /api/calendar-events`
- `DELETE /api/calendar-events/{id}`

## Como funciona la publicidad

- El sitio no administra clientes ni campanas propias.
- AdSense decide de forma automatica que anuncio mostrar, segun el usuario, el contenido y la disponibilidad de inventario.
- Para anuncios manuales, crea bloques en AdSense y coloca los IDs en `src/CalendarAds.Web/src/environments/environment.ts`.
- No hagas clic en tus propios anuncios ni uses textos que pidan clics; eso puede afectar la aprobacion y la cuenta.

## Festivos y dias especiales

- Los festivos oficiales de Colombia se calculan por codigo para cada ano.
- Se aplica Ley Emiliani para los festivos trasladables al lunes siguiente.
- Desde 2026 se incluye Nuestra Senora del Rosario de Chiquinquira, creado por Ley 2578 de 2026.
- No necesitas base de datos para calcular los festivos oficiales.
- Si quieres administrar dias comerciales, notas SEO, eventos propios o correcciones manuales, ahi si conviene crear una tabla de dias especiales en la base de datos.
