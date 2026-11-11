// Zero-dependency static server + reverse proxy for the Music UI.
// The browser only talks to this server, so none of the .NET services need CORS changes.
//   /upstream/<target>/<path>  ->  <target url>/<path>
const http = require('http');
const fs = require('fs');
const path = require('path');

const PORT = Number(process.env.PORT) || 8080;
const HOST = process.env.HOST || '127.0.0.1';
const PUBLIC_DIR = path.join(__dirname, 'public');

const TARGETS = {
  gateway: { label: 'Smart Proxy', url: process.env.GATEWAY_URL || 'http://localhost:9000' },
  node1: { label: 'MusicAPI #1', url: process.env.NODE1_URL || 'http://localhost:9001' },
  node2: { label: 'MusicAPI #2', url: process.env.NODE2_URL || 'http://localhost:9002' },
  sync: { label: 'SyncNode', url: process.env.SYNC_URL || 'http://localhost:5000' },
};

const MIME = {
  '.html': 'text/html; charset=utf-8',
  '.css': 'text/css; charset=utf-8',
  '.js': 'text/javascript; charset=utf-8',
  '.json': 'application/json; charset=utf-8',
  '.svg': 'image/svg+xml',
  '.ico': 'image/x-icon',
};

function sendJson(res, status, body, extraHeaders = {}) {
  res.writeHead(status, { 'content-type': MIME['.json'], 'cache-control': 'no-store', ...extraHeaders });
  res.end(JSON.stringify(body));
}

function forward(req, res, target, rest) {
  const upstream = new URL(rest, target.url);
  const headers = { ...req.headers, host: upstream.host };
  delete headers.origin;
  delete headers.referer;

  const proxyReq = http.request(upstream, { method: req.method, headers, timeout: 8000 }, (proxyRes) => {
    res.writeHead(proxyRes.statusCode, { ...proxyRes.headers, 'cache-control': 'no-store' });
    proxyRes.pipe(res);
  });

  proxyReq.on('timeout', () => proxyReq.destroy(new Error('timeout')));
  proxyReq.on('error', (err) => {
    if (res.headersSent) return res.destroy();
    // The marker header lets the UI tell "service is down" apart from an API error.
    sendJson(res, 503, { error: 'unreachable', target: target.url, detail: err.code || err.message },
      { 'x-ui-unreachable': '1' });
  });

  req.pipe(proxyReq);
}

function serveStatic(req, res, pathname) {
  const relative = pathname === '/' ? 'index.html' : decodeURIComponent(pathname).replace(/^\/+/, '');
  const file = path.resolve(PUBLIC_DIR, relative);
  if (!file.startsWith(PUBLIC_DIR + path.sep)) {
    res.writeHead(403);
    return res.end();
  }

  fs.readFile(file, (err, content) => {
    if (err) {
      res.writeHead(404, { 'content-type': 'text/plain; charset=utf-8' });
      return res.end('Not found');
    }
    res.writeHead(200, { 'content-type': MIME[path.extname(file)] || 'application/octet-stream', 'cache-control': 'no-cache' });
    res.end(content);
  });
}

const server = http.createServer((req, res) => {
  const { pathname, search } = new URL(req.url, 'http://ui.local');

  if (pathname === '/config.json') {
    return sendJson(res, 200, { targets: TARGETS });
  }

  const match = pathname.match(/^\/upstream\/([a-z0-9]+)(\/.*)?$/);
  if (match) {
    const target = TARGETS[match[1]];
    if (!target) return sendJson(res, 404, { error: 'unknown target', target: match[1] });
    return forward(req, res, target, (match[2] || '/') + search);
  }

  if (req.method !== 'GET' && req.method !== 'HEAD') {
    res.writeHead(405);
    return res.end();
  }
  serveStatic(req, res, pathname);
});

server.listen(PORT, HOST, () => {
  console.log(`Music UI  ->  http://localhost:${PORT}`);
  for (const [key, t] of Object.entries(TARGETS)) {
    console.log(`  ${key.padEnd(8)} ${t.label.padEnd(12)} ${t.url}`);
  }
});
