import { createReadStream, existsSync, statSync } from 'node:fs';
import { createServer } from 'node:http';
import { extname, join, normalize } from 'node:path';

const root = join(process.cwd(), 'dist', 'calendar-ads-web', 'browser');
const port = Number(process.env.PORT ?? 4300);

const contentTypes = {
  '.css': 'text/css; charset=utf-8',
  '.html': 'text/html; charset=utf-8',
  '.ico': 'image/x-icon',
  '.js': 'text/javascript; charset=utf-8',
  '.json': 'application/json; charset=utf-8',
  '.svg': 'image/svg+xml; charset=utf-8',
  '.txt': 'text/plain; charset=utf-8',
  '.webmanifest': 'application/manifest+json; charset=utf-8',
  '.xml': 'application/xml; charset=utf-8'
};

function resolvePath(url) {
  const pathname = decodeURIComponent(new URL(url, `http://127.0.0.1:${port}`).pathname);
  const relative = pathname === '/' ? 'index.html' : pathname.slice(1);
  const candidate = normalize(join(root, relative));
  return candidate.startsWith(root) && existsSync(candidate) ? candidate : join(root, 'index.html');
}

createServer((request, response) => {
  const filePath = resolvePath(request.url ?? '/');
  const extension = extname(filePath);
  const stats = statSync(filePath);
  const isHtml = extension === '.html';

  response.writeHead(200, {
    'Cache-Control': isHtml ? 'public, max-age=0, must-revalidate' : 'public, max-age=31536000, immutable',
    'Content-Length': stats.size,
    'Content-Type': contentTypes[extension] ?? 'application/octet-stream',
    'X-Content-Type-Options': 'nosniff'
  });

  createReadStream(filePath).pipe(response);
}).listen(port, '127.0.0.1', () => {
  console.log(`Production preview: http://127.0.0.1:${port}`);
});
