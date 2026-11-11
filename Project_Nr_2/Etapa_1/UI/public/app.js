'use strict';

/* ================= icons ================= */

const FILLED = {
  play: '<path d="M7 4.5v15a1 1 0 0 0 1.5.86l12.5-7.5a1 1 0 0 0 0-1.72L8.5 3.64A1 1 0 0 0 7 4.5z"/>',
  pause: '<rect x="6" y="4" width="4" height="16" rx="1"/><rect x="14" y="4" width="4" height="16" rx="1"/>',
  prev: '<path d="M6 4h2v16H6zM20 5.1v13.8a1 1 0 0 1-1.55.83L9.2 12.83a1 1 0 0 1 0-1.66l9.25-6.9A1 1 0 0 1 20 5.1z"/>',
  next: '<path d="M16 4h2v16h-2zM4 5.1v13.8a1 1 0 0 0 1.55.83l9.25-6.9a1 1 0 0 0 0-1.66L5.55 4.27A1 1 0 0 0 4 5.1z"/>',
  'heart-fill': '<path d="M20.8 4.6a5.5 5.5 0 0 0-7.8 0L12 5.7l-1-1.1a5.5 5.5 0 0 0-7.8 7.8l1 1.1L12 21l7.8-7.5 1-1.1a5.5 5.5 0 0 0 0-7.8z"/>',
};

const STROKED = {
  home: '<path d="M3 10.5 12 3l9 7.5V21h-6v-6H9v6H3z"/>',
  search: '<circle cx="11" cy="11" r="7"/><path d="m20 20-4-4"/>',
  library: '<path d="M4 3v18M9 3v18M14 4l6 16.5"/>',
  server: '<rect x="3" y="4" width="18" height="7" rx="2"/><rect x="3" y="13" width="18" height="7" rx="2"/><path d="M7 7.5h.01M7 16.5h.01"/>',
  plus: '<path d="M12 5v14M5 12h14"/>',
  'chevron-left': '<path d="m15 18-6-6 6-6"/>',
  'chevron-right': '<path d="m9 18 6-6-6-6"/>',
  refresh: '<path d="M21 12a9 9 0 1 1-2.64-6.36"/><path d="M21 3v6h-6"/>',
  edit: '<path d="M12 20h9"/><path d="M16.5 3.5a2.12 2.12 0 0 1 3 3L7 19l-4 1 1-4z"/>',
  trash: '<path d="M3 6h18M8 6V4h8v2M19 6l-1 14H6L5 6M10 11v6M14 11v6"/>',
  heart: '<path d="M20.8 4.6a5.5 5.5 0 0 0-7.8 0L12 5.7l-1-1.1a5.5 5.5 0 0 0-7.8 7.8l1 1.1L12 21l7.8-7.5 1-1.1a5.5 5.5 0 0 0 0-7.8z"/>',
  shuffle: '<path d="M16 3h5v5M4 20 21 3M21 16v5h-5M15 15l6 6M4 4l5 5"/>',
  repeat: '<path d="m17 2 4 4-4 4"/><path d="M3 11V9a3 3 0 0 1 3-3h15"/><path d="m7 22-4-4 4-4"/><path d="M21 13v2a3 3 0 0 1-3 3H3"/>',
  close: '<path d="M18 6 6 18M6 6l12 12"/>',
  globe: '<circle cx="12" cy="12" r="9"/><path d="M3 12h18M12 3a14 14 0 0 1 0 18M12 3a14 14 0 0 0 0 18"/>',
  database: '<ellipse cx="12" cy="5" rx="8" ry="3"/><path d="M4 5v14c0 1.7 3.6 3 8 3s8-1.3 8-3V5M4 12c0 1.7 3.6 3 8 3s8-1.3 8-3"/>',
  sync: '<path d="M3 12a9 9 0 0 1 15-6.7L21 8M21 3v5h-5M21 12a9 9 0 0 1-15 6.7L3 16M3 21v-5h5"/>',
  music: '<path d="M9 18V5l12-2v13"/><circle cx="6" cy="18" r="3"/><circle cx="18" cy="16" r="3"/>',
  alert: '<path d="M12 9v4M12 17h.01"/><path d="M10.3 3.9 1.8 18a2 2 0 0 0 1.7 3h17a2 2 0 0 0 1.7-3L13.7 3.9a2 2 0 0 0-3.4 0z"/>',
  activity: '<path d="M22 12h-4l-3 9L9 3l-3 9H2"/>',
  check: '<path d="M20 6 9 17l-5-5"/>',
};

function svg(name) {
  if (FILLED[name]) return `<svg viewBox="0 0 24 24" fill="currentColor" aria-hidden="true">${FILLED[name]}</svg>`;
  return `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true">${STROKED[name] || ''}</svg>`;
}
const icon = (name) => `<i data-icon="${name}">${svg(name)}</i>`;
const hydrateIcons = (root = document) => root.querySelectorAll('i[data-icon]:empty').forEach((el) => { el.innerHTML = svg(el.dataset.icon); });

/* ================= helpers ================= */

const $ = (sel, root = document) => root.querySelector(sel);
const $$ = (sel, root = document) => [...root.querySelectorAll(sel)];

const ESC = { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' };
const esc = (value) => String(value ?? '').replace(/[&<>"']/g, (c) => ESC[c]);
const enc = encodeURIComponent;
const norm = (s) => String(s ?? '').normalize('NFD').replace(/[̀-ͯ]/g, '').toLowerCase().trim();

function hash(str) {
  let h = 2166136261;
  for (const ch of String(str)) h = Math.imul(h ^ ch.charCodeAt(0), 16777619);
  return h >>> 0;
}
const hueOf = (str) => hash(str) % 360;

function initials(text) {
  const words = String(text || '?').trim().split(/\s+/).filter(Boolean);
  const letters = words.slice(0, 2).map((w) => [...w][0]).join('');
  return (letters || '?').toUpperCase();
}

function parseDate(value) {
  if (!value) return null;
  // .NET emits 7 fractional digits; trim to milliseconds so every browser parses it.
  const d = typeof value === 'number' ? new Date(value) : new Date(String(value).replace(/(\.\d{3})\d+/, '$1'));
  return Number.isNaN(d.getTime()) ? null : d;
}

const rtf = new Intl.RelativeTimeFormat('ro', { numeric: 'auto' });
function relTime(value) {
  const d = parseDate(value);
  if (!d) return '—';
  const sec = Math.round((d.getTime() - Date.now()) / 1000);
  const abs = Math.abs(sec);
  if (abs < 45) return 'chiar acum';
  const units = [['minute', 60], ['hour', 3600], ['day', 86400], ['week', 604800], ['month', 2629800], ['year', 31557600]];
  let [unit, size] = units[0];
  for (const [u, s] of units) if (abs >= s) [unit, size] = [u, s];
  return rtf.format(Math.round(sec / size), unit);
}
function fullDate(value) {
  const d = parseDate(value);
  return d ? d.toLocaleString('ro-RO', { dateStyle: 'medium', timeStyle: 'medium' }) : '';
}

const nf = new Intl.NumberFormat('ro-RO', { maximumFractionDigits: 2 });
const fmtBudget = (n) => nf.format(Number(n) || 0);
const fmtTime = (sec) => `${Math.floor(sec / 60)}:${String(Math.floor(sec % 60)).padStart(2, '0')}`;
const fmtMs = (ms) => (ms == null ? '—' : `${Math.round(ms)} ms`);

function songsLabel(n) {
  if (n === 1) return '1 melodie';
  const r = n % 100;
  return n === 0 || (r >= 1 && r <= 19) ? `${n} melodii` : `${n} de melodii`;
}

function uuid() {
  if (crypto.randomUUID) return crypto.randomUUID();
  const b = crypto.getRandomValues(new Uint8Array(16));
  b[6] = (b[6] & 0x0f) | 0x40;
  b[8] = (b[8] & 0x3f) | 0x80;
  const h = [...b].map((x) => x.toString(16).padStart(2, '0')).join('');
  return `${h.slice(0, 8)}-${h.slice(8, 12)}-${h.slice(12, 16)}-${h.slice(16, 20)}-${h.slice(20)}`;
}

function highlight(text, query) {
  const raw = String(text ?? '');
  const i = query ? raw.toLowerCase().indexOf(query.toLowerCase()) : -1;
  if (i < 0) return esc(raw);
  return `${esc(raw.slice(0, i))}<mark class="mark">${esc(raw.slice(i, i + query.length))}</mark>${esc(raw.slice(i + query.length))}`;
}

const store = {
  get(key, fallback) {
    try {
      const v = localStorage.getItem(`dm-music:${key}`);
      return v == null ? fallback : JSON.parse(v);
    } catch { return fallback; }
  },
  set(key, value) {
    try { localStorage.setItem(`dm-music:${key}`, JSON.stringify(value)); } catch { /* storage unavailable */ }
  },
};

/* ================= state ================= */

const EMPTY_GUID = '00000000-0000-0000-0000-000000000000';
const SOURCES = [
  { key: 'gateway', label: 'Gateway' },
  { key: 'node1', label: 'Nod #1' },
  { key: 'node2', label: 'Nod #2' },
];
const SERVICES = ['gateway', 'node1', 'node2', 'sync'];
const STATUS_LABEL = { online: 'Online', offline: 'Offline', error: 'Eroare', degraded: 'Parțial', unknown: 'Se verifică…' };

const state = {
  targets: {
    gateway: { label: 'Smart Proxy', url: 'http://localhost:9000' },
    node1: { label: 'MusicAPI #1', url: 'http://localhost:9001' },
    node2: { label: 'MusicAPI #2', url: 'http://localhost:9002' },
    sync: { label: 'SyncNode', url: 'http://localhost:5000' },
  },
  source: SOURCES.some((s) => s.key === store.get('source')) ? store.get('source') : 'gateway',
  songs: [],
  signature: '',
  loaded: false,
  loading: false,
  loadError: null,
  libFilter: store.get('libFilter', 'all'),
  libQuery: '',
  sort: { key: 'recent', dir: 1 },
  selectedId: null,
  liked: new Set(store.get('liked', [])),
  cluster: Object.fromEntries(SERVICES.map((k) => [k, { status: 'unknown' }])),
  clusterCheckedAt: null,
  activity: [],
  route: { name: 'home', arg: '' },
};

const player = { queue: [], index: -1, listKey: null, playing: false, pos: 0, shuffle: false, repeat: false };
let lists = new Map();

const hostOf = (url) => { try { return new URL(url).host; } catch { return url; } };
const portOf = (key) => { try { return new URL(state.targets[key].url).port; } catch { return ''; } };
const targetLabel = (key) => state.targets[key]?.label || key;
const sourceLabel = () => SOURCES.find((s) => s.key === state.source)?.label || state.source;

/* ================= api ================= */

function logActivity(entry) {
  state.activity.unshift({ at: Date.now(), ...entry });
  state.activity.length = Math.min(state.activity.length, 40);
}

async function request(target, method, path, body) {
  const started = performance.now();
  let res;
  let retries = 0;
  try {
    for (;;) {
      res = await fetch(`/upstream/${target}${path}`, {
        method,
        cache: 'no-store',
        headers: body === undefined ? {} : { 'Content-Type': 'application/json' },
        body: body === undefined ? undefined : JSON.stringify(body),
      });
      // Ocelot answers 502 when round-robin picks a node that is down. The request never
      // reached a MusicAPI, so retrying moves on to the next node in the pool.
      if (res.status === 502 && target === 'gateway' && retries < 2) { retries++; continue; }
      break;
    }
  } catch {
    logActivity({ method, target, path, status: 0, ms: performance.now() - started, retries });
    throw Object.assign(new Error('network'), { status: 0, network: true });
  }

  const text = await res.text();
  let data = text;
  try { data = text ? JSON.parse(text) : null; } catch { /* plain-text body */ }
  logActivity({ method, target, path, status: res.status, ms: performance.now() - started, retries });

  if (!res.ok) {
    throw Object.assign(new Error(`HTTP ${res.status}`), {
      status: res.status, unreachable: res.headers.has('x-ui-unreachable'), data,
    });
  }
  return data;
}

function describeError(err, target = state.source) {
  const label = `${targetLabel(target)} (${hostOf(state.targets[target]?.url)})`;
  if (err.network) return 'Serverul UI nu răspunde. Pornește-l cu „node server.js” din folderul UI.';
  if (err.unreachable) return `${label} nu răspunde — serviciul nu rulează?`;
  if (err.status === 502) return `${label}: niciun nod MusicAPI disponibil (502).`;
  const d = err.data;
  let detail = typeof d === 'string' ? d : d && (d.title || d.detail || d.error);
  if (d && d.errors) detail = Object.values(d.errors).flat().join(' ');
  detail = detail ? String(detail).split('\n')[0].slice(0, 160) : '';
  if (err.status === 400) return `Cerere respinsă (400)${detail ? `: ${detail}` : '.'}`;
  if (err.status === 404) return `${label} a răspuns 404 — endpoint inexistent pe acest port.`;
  return `${label} a răspuns ${err.status}${detail ? `: ${detail}` : '.'}`;
}

/* ================= data ================= */

function normalizeSong(s) {
  const artists = s.artists ?? s.Artists ?? [];
  return {
    id: String(s.id ?? s.Id ?? ''),
    name: String(s.name ?? s.Name ?? ''),
    artists: (Array.isArray(artists) ? artists : []).filter(Boolean).map(String),
    budget: Number(s.budget ?? s.Budget) || 0,
    description: String(s.description ?? s.Description ?? ''),
    lastChangedAt: s.lastChangedAt ?? s.LastChangedAt ?? null,
  };
}

const songById = (id) => state.songs.find((s) => s.id === id);

function artistIndex(songs = state.songs) {
  const map = new Map();
  for (const s of songs) {
    for (const a of new Set(s.artists.map((x) => x.trim()).filter(Boolean))) {
      if (!map.has(a)) map.set(a, { name: a, songs: [] });
      map.get(a).songs.push(s);
    }
  }
  return [...map.values()].sort((a, b) => b.songs.length - a.songs.length || a.name.localeCompare(b.name, 'ro'));
}

const likedSongs = () => state.songs.filter((s) => state.liked.has(s.id));
const pendingIds = () => new Set((state.cluster.sync.data || []).map((e) => String(e.id).toLowerCase()));

const SORTS = {
  recent: (a, b) => (parseDate(b.lastChangedAt)?.getTime() || 0) - (parseDate(a.lastChangedAt)?.getTime() || 0),
  name: (a, b) => a.name.localeCompare(b.name, 'ro'),
  artist: (a, b) => (a.artists[0] || '').localeCompare(b.artists[0] || '', 'ro'),
  budget: (a, b) => b.budget - a.budget,
  desc: (a, b) => a.description.localeCompare(b.description, 'ro'),
};
const SORT_LABEL = { recent: 'Recente', name: 'Titlu', artist: 'Artist', budget: 'Buget' };

function sortSongs(list) {
  const sorted = [...list].sort(SORTS[state.sort.key] || SORTS.recent);
  return state.sort.dir < 0 ? sorted.reverse() : sorted;
}

async function loadSongs({ quiet = false } = {}) {
  state.loading = true;
  if (!quiet) $('[data-action="refresh"]')?.classList.add('spinning');
  if (!state.loaded) renderView();

  const previousError = state.loadError;
  try {
    const data = await request(state.source, 'GET', '/api/music');
    state.songs = (Array.isArray(data) ? data : []).filter(Boolean).map(normalizeSong);
    state.loadError = null;
  } catch (err) {
    state.songs = [];
    state.loadError = describeError(err);
    if (!quiet) toast(state.loadError, 'err');
  } finally {
    state.loading = false;
    $('[data-action="refresh"]')?.classList.remove('spinning');
  }

  const signature = JSON.stringify(state.songs);
  const changed = !state.loaded || signature !== state.signature || previousError !== state.loadError;
  state.loaded = true;
  state.signature = signature;
  pruneQueue();
  if (changed) renderAll();
}

function upsertLocal(song) {
  const i = state.songs.findIndex((s) => s.id === song.id);
  if (i >= 0) state.songs[i] = song; else state.songs.push(song);
  renderAll();
}

function toggleLike(id) {
  if (state.liked.has(id)) state.liked.delete(id); else state.liked.add(id);
  store.set('liked', [...state.liked]);
  const liked = state.liked.has(id);
  $$(`[data-action="like"][data-id="${CSS.escape(id)}"]`).forEach((btn) => {
    btn.classList.toggle('liked', liked);
    btn.innerHTML = icon(liked ? 'heart-fill' : 'heart');
    btn.title = liked ? 'Elimină din Melodii apreciate' : 'Salvează în Melodii apreciate';
  });
  renderSidebar();
  if (state.route.name === 'liked') renderView();
  toast(liked ? 'Adăugată în Melodii apreciate' : 'Eliminată din Melodii apreciate');
}

/* ================= player (simulated: songs carry no audio) ================= */

const durationOf = (song) => 140 + (hash(song.id || song.name) % 150);
const currentSong = () => songById(player.queue[player.index]);

function playFrom(listKey, id) {
  const ids = lists.get(listKey) || [];
  if (!ids.length) return;
  const cur = currentSong();
  if (cur && player.listKey === listKey && (id == null || id === cur.id)) {
    togglePlay();
    return;
  }
  player.queue = [...ids];
  player.listKey = listKey;
  player.index = id == null ? (player.shuffle ? Math.floor(Math.random() * ids.length) : 0) : Math.max(0, ids.indexOf(id));
  player.pos = 0;
  player.playing = true;
  renderPlayer();
  markPlaying();
}

function togglePlay() {
  if (!currentSong()) {
    const all = sortSongs(state.songs);
    lists.set('player:all', all.map((s) => s.id));
    if (all.length) playFrom('player:all');
    return;
  }
  player.playing = !player.playing;
  renderPlayer();
  markPlaying();
}

function step(dir, auto = false) {
  if (!player.queue.length) return;
  if (dir < 0 && player.pos > 3) {
    player.pos = 0;
  } else if (player.shuffle && player.queue.length > 1) {
    let next = player.index;
    while (next === player.index) next = Math.floor(Math.random() * player.queue.length);
    player.index = next;
    player.pos = 0;
  } else {
    const next = player.index + dir;
    if (next >= player.queue.length || next < 0) {
      if (player.repeat || !auto) player.index = (next + player.queue.length) % player.queue.length;
      else player.playing = false;
    } else {
      player.index = next;
    }
    player.pos = 0;
  }
  renderPlayer();
  markPlaying();
}

function pruneQueue() {
  const curId = player.queue[player.index];
  player.queue = player.queue.filter((id) => songById(id));
  player.index = player.queue.indexOf(curId);
  if (player.index < 0) {
    player.playing = false;
    player.pos = 0;
  }
}

function tick() {
  const song = currentSong();
  if (!song || !player.playing) return;
  player.pos += 0.25;
  if (player.pos >= durationOf(song)) step(1, true);
  else updateProgress();
}

/* ================= cluster probing ================= */

async function probe(key) {
  const path = key === 'sync' ? '/sync' : '/api/music';
  const started = performance.now();
  let saw502 = false;
  try {
    for (let attempt = 0; attempt < 3; attempt++) {
      const res = await fetch(`/upstream/${key}${path}`, { cache: 'no-store' });
      const ms = performance.now() - started;
      if (res.headers.has('x-ui-unreachable')) return { status: 'offline', ms, note: 'Nu rulează sau portul e închis.' };
      if (res.status === 502 && key === 'gateway') { saw502 = true; continue; }
      if (!res.ok) {
        const note = res.status === 404
          ? `Portul răspunde, dar nu ca ${targetLabel(key)} (HTTP 404). Alt proces ocupă portul?`
          : `HTTP ${res.status}`;
        return { status: 'error', ms, note };
      }
      let data;
      try { data = await res.json(); } catch { return { status: 'error', ms, note: 'Răspunsul nu este JSON.' }; }
      if (!Array.isArray(data)) return { status: 'error', ms, note: 'Răspuns neașteptat.' };
      if (saw502) return { status: 'degraded', ms, data, note: 'Un nod din pool nu răspunde: Ocelot a dat 502, apoi a reușit pe următorul.' };
      return { status: 'online', ms, data };
    }
    return { status: 'error', ms: performance.now() - started, note: 'Niciun nod MusicAPI disponibil (502).' };
  } catch {
    return { status: 'offline', ms: null, note: 'Serverul UI nu răspunde.' };
  }
}

let probing = false;
async function probeCluster() {
  if (probing) return;
  probing = true;
  const before = [...pendingIds()].sort().join();
  const results = await Promise.all(SERVICES.map(probe));
  SERVICES.forEach((k, i) => { state.cluster[k] = results[i]; });
  state.clusterCheckedAt = Date.now();
  probing = false;

  renderClusterMini();
  renderSourceSwitch();
  if (state.route.name === 'cluster') renderView();
  else if ([...pendingIds()].sort().join() !== before && $('#track-rows')) refreshRows();
}

/* ================= covers ================= */

function coverHtml(song, { size, round = false, cls = '' } = {}) {
  const style = `--h:${hueOf(song.id || song.name)};${size ? `width:${size}px;height:${size}px;font-size:${Math.round(size * 0.36)}px;` : ''}`;
  return `<div class="cover ${round ? 'round' : ''} ${cls}" style="${style}"><span>${esc(initials(song.name))}</span></div>`;
}
const artistCover = (name, opts = {}) => coverHtml({ id: `artist:${name}`, name }, { round: true, ...opts });

function specialCover(kind, size, cls = '') {
  const style = size ? `width:${size}px;height:${size}px;font-size:${Math.round(size * 0.42)}px;` : '';
  return `<div class="cover ${kind} ${cls}" style="${style}">${icon(kind === 'liked' ? 'heart-fill' : 'music')}</div>`;
}

function mosaicCover(songs, { cls = '', size } = {}) {
  if (songs.length < 4) return songs.length ? coverHtml(songs[0], { cls, size }) : specialCover('all', size, cls);
  const style = size ? ` style="width:${size}px;height:${size}px;font-size:${Math.round(size * 0.18)}px"` : '';
  return `<div class="cover mosaic ${cls}"${style}>${songs.slice(0, 4).map((s) => coverHtml(s)).join('')}</div>`;
}

const artistLinks = (song, q = '') => song.artists.length
  ? song.artists.map((a) => `<a href="#/artist/${enc(a)}">${highlight(a, q)}</a>`).join(', ')
  : '<span>Artist necunoscut</span>';

/* ================= rendering: chrome ================= */

function renderAll() {
  renderSidebar();
  renderSourceSwitch();
  renderView();
  renderPlayer();
}

function renderSidebar() {
  const r = state.route;
  $$('.nav-link').forEach((a) => a.classList.toggle('active', a.dataset.nav === r.name));
  $$('#library-chips .chip').forEach((c) => c.classList.toggle('active', c.dataset.filter === state.libFilter));

  const item = ({ href, active, cover, title, sub }) => `
    <li><a class="lib-item ${active ? 'active' : ''}" href="${href}" title="${esc(title)}">${cover}
      <div class="meta"><div class="title">${esc(title)}</div><div class="sub">${sub}</div></div></a></li>`;

  const items = [];
  if (state.libFilter !== 'artists') {
    items.push(item({ href: '#/liked', active: r.name === 'liked', cover: specialCover('liked', 48), title: 'Melodii apreciate', sub: `Playlist · ${songsLabel(likedSongs().length)}` }));
    items.push(item({ href: '#/library', active: r.name === 'library', cover: mosaicCover(sortSongs(state.songs), { size: 48 }), title: 'Toate melodiile', sub: `Playlist · ${songsLabel(state.songs.length)}` }));
  }
  if (state.libFilter !== 'playlists') {
    for (const a of artistIndex()) {
      items.push(item({ href: `#/artist/${enc(a.name)}`, active: r.name === 'artist' && r.arg === a.name, cover: artistCover(a.name, { size: 48 }), title: a.name, sub: `Artist · ${songsLabel(a.songs.length)}` }));
    }
  }
  $('#library-list').innerHTML = items.join('');
}

function renderSourceSwitch() {
  $('#source-switch').innerHTML = SOURCES.map((s) => {
    const t = state.targets[s.key];
    const status = state.cluster[s.key].status;
    return `<button role="radio" aria-checked="${s.key === state.source}" class="${s.key === state.source ? 'active' : ''}"
      data-action="source" data-key="${s.key}" title="Citește și scrie prin ${esc(t.label)} · ${esc(hostOf(t.url))} · ${STATUS_LABEL[status]}">
      <span class="dot-led ${status}"></span><span>${s.label}</span></button>`;
  }).join('');
}

function renderPlayer() {
  const song = currentSong();
  const liked = song && state.liked.has(song.id);
  const np = song
    ? `<div class="np">${coverHtml(song, { size: 56 })}
        <div class="np-text"><div class="np-title">${esc(song.name)}</div><div class="np-sub">${artistLinks(song)}</div></div>
        <button class="icon-btn ${liked ? 'liked' : ''}" data-action="like" data-id="${esc(song.id)}" title="${liked ? 'Elimină din Melodii apreciate' : 'Salvează în Melodii apreciate'}">${icon(liked ? 'heart-fill' : 'heart')}</button></div>`
    : `<div class="np empty-np">${specialCover('all', 56)}
        <div class="np-text"><div class="np-title">Alege o melodie</div><div class="np-sub">Redare simulată — melodiile nu au fișier audio</div></div></div>`;

  $('#player').innerHTML = `${np}
    <div class="controls">
      <div class="ctrl-row">
        <button class="ctrl ${player.shuffle ? 'on' : ''}" data-action="shuffle" title="Amestecă">${icon('shuffle')}</button>
        <button class="ctrl" data-action="prev" title="Anterioara">${icon('prev')}</button>
        <button class="ctrl-main" data-action="toggle" ${song || state.songs.length ? '' : 'disabled'} title="${player.playing ? 'Pauză' : 'Redă'} (Space)">${icon(player.playing ? 'pause' : 'play')}</button>
        <button class="ctrl" data-action="next" title="Următoarea">${icon('next')}</button>
        <button class="ctrl ${player.repeat ? 'on' : ''}" data-action="repeat" title="Repetă">${icon('repeat')}</button>
      </div>
      <div class="progress">
        <span class="t" id="p-pos">0:00</span>
        <div class="bar" data-action="seek"><div class="rail"><div class="fill" id="p-fill"></div></div><div class="knob" id="p-knob"></div></div>
        <span class="t">${song ? fmtTime(durationOf(song)) : '0:00'}</span>
      </div>
    </div>
    <div class="cluster-mini" id="cluster-mini"></div>`;
  updateProgress();
  renderClusterMini();
}

function updateProgress() {
  const song = currentSong();
  const pct = song ? Math.min(100, (player.pos / durationOf(song)) * 100) : 0;
  const fill = $('#p-fill');
  if (!fill) return;
  fill.style.width = `${pct}%`;
  $('#p-knob').style.left = `${pct}%`;
  $('#p-pos').textContent = fmtTime(player.pos);
}

function renderClusterMini() {
  const el = $('#cluster-mini');
  if (!el) return;
  const minis = [['gateway', 'Proxy'], ['node1', '#1'], ['node2', '#2'], ['sync', 'Sync']];
  el.innerHTML = minis.map(([k, label]) => {
    const s = state.cluster[k];
    return `<a class="mini" href="#/cluster" title="${esc(`${targetLabel(k)} · ${hostOf(state.targets[k].url)} · ${STATUS_LABEL[s.status]}${s.note ? ` — ${s.note}` : ''}`)}"><span class="dot-led ${s.status}"></span>${label}</a>`;
  }).join('');
}

function markPlaying() {
  const cur = currentSong();
  const curId = cur?.id;
  $$('#view .track[data-id]').forEach((row) => {
    const on = row.dataset.id === curId;
    row.classList.toggle('playing', on);
    const idx = row.querySelector('.idx');
    if (idx) idx.innerHTML = on && player.playing ? '<span class="eq"><b></b><b></b><b></b><b></b></span>' : row.dataset.n;
    const mini = row.querySelector('.play-mini');
    if (mini) mini.innerHTML = icon(on && player.playing ? 'pause' : 'play');
  });
  $$('#view [data-action="play-list"]').forEach((btn) => {
    const on = player.playing && player.listKey === btn.dataset.list;
    btn.innerHTML = icon(on ? 'pause' : 'play');
    btn.classList.toggle('on', on);
  });
  $$('#view .play-fab[data-action="play-card"]').forEach((btn) => {
    const on = player.playing && curId === btn.dataset.id;
    btn.innerHTML = icon(on ? 'pause' : 'play');
    btn.classList.toggle('on', on);
  });
}

/* ================= rendering: views ================= */

function renderView() {
  lists = new Map();
  const r = state.route;
  const { html, hue } = (VIEWS[r.name] || VIEWS.home)(r.arg);
  $('#view').innerHTML = html;
  $('#topbar').style.setProperty('--topbar-bg', hue == null ? '#101010' : `hsl(${hue} 36% 16%)`);
  const input = $('#search-input');
  if (document.activeElement !== input) input.value = r.name === 'search' ? r.arg : '';
  onScroll();
  markPlaying();
}

function registerList(key, songs) {
  lists.set(key, songs.map((s) => s.id));
  return esc(key);
}

function emptyState({ iconName = 'music', title, text, actions = '' }) {
  return `<div class="empty"><div><div class="big-icon" style="margin:0 auto 16px">${icon(iconName)}</div>
    <h3>${esc(title)}</h3><p>${text}</p><div style="display:flex;gap:10px;justify-content:center;flex-wrap:wrap">${actions}</div></div></div>`;
}

function errorState() {
  return emptyState({
    iconName: 'alert',
    title: 'Nu pot încărca melodiile',
    text: esc(state.loadError),
    actions: `<button class="pill-btn light" data-action="refresh">Reîncearcă</button><a class="pill-btn outline" href="#/cluster">Vezi clusterul</a>`,
  });
}

function skeletonRows(n = 6) {
  return Array.from({ length: n }, () => `
    <div class="track"><div></div><div class="t-main"><div class="skeleton" style="width:40px;height:40px"></div>
      <div style="flex:1"><div class="skeleton" style="height:12px;width:45%;margin-bottom:8px"></div><div class="skeleton" style="height:10px;width:30%"></div></div></div>
      <div class="t-desc"><div class="skeleton" style="height:10px;width:60%"></div></div><div></div><div></div><div></div></div>`).join('');
}

function trackHead() {
  const col = (key, label, cls = '') => {
    const on = state.sort.key === key;
    return `<button class="${cls} ${on ? 'sorted' : ''}" data-action="sort" data-key="${key}">${label}${on ? (state.sort.dir > 0 ? ' ▾' : ' ▴') : ''}</button>`;
  };
  return `<div class="track-head"><span class="num">#</span>${col('name', 'Titlu')}${col('desc', 'Descriere', 'c-desc')}
    ${col('budget', 'Buget', 'c-budget right')}${col('recent', 'Modificat', 'c-date')}<span></span></div>`;
}

function trackRow(song, n, q = '') {
  const pending = pendingIds().has(song.id.toLowerCase());
  const liked = state.liked.has(song.id);
  const badges = [
    pending ? '<span class="badge warn" title="În coada SyncNode — urmează să fie propagată pe celălalt nod">sync</span>' : '',
    song.id === EMPTY_GUID ? '<span class="badge red" title="Id = Guid.Empty: API-ul respinge PUT pentru această înregistrare">id gol</span>' : '',
  ].join(' ');
  return `
    <div class="track ${state.selectedId === song.id ? 'selected' : ''}" data-action="select" data-id="${esc(song.id)}" data-n="${n}">
      <div class="num"><span class="idx">${n}</span><button class="play-mini" data-action="play-row" data-id="${esc(song.id)}" title="Redă ${esc(song.name)}">${icon('play')}</button></div>
      <div class="t-main">${coverHtml(song, { size: 40 })}
        <div class="t-text"><div class="t-name">${highlight(song.name, q)} ${badges}</div><div class="t-artists">${artistLinks(song, q)}</div></div></div>
      <div class="t-desc" title="${esc(song.description)}">${highlight(song.description, q) || '—'}</div>
      <div class="t-budget right">${fmtBudget(song.budget)}</div>
      <div class="t-date" title="${esc(fullDate(song.lastChangedAt))}">${relTime(song.lastChangedAt)}</div>
      <div class="t-actions">
        <button class="icon-btn hover-only ${liked ? 'liked' : ''}" data-action="like" data-id="${esc(song.id)}" title="${liked ? 'Elimină din Melodii apreciate' : 'Salvează în Melodii apreciate'}">${icon(liked ? 'heart-fill' : 'heart')}</button>
        <button class="icon-btn hover-only" data-action="edit" data-id="${esc(song.id)}" title="Editează">${icon('edit')}</button>
        <button class="icon-btn hover-only danger" data-action="delete" data-id="${esc(song.id)}" title="Șterge">${icon('trash')}</button>
      </div>
    </div>`;
}

// The playlist currently on screen, so the inline filter / sort can redraw just the rows.
let currentPlaylist = null;

function playlistRows() {
  const p = currentPlaylist;
  if (!p) return '';
  const q = p.filterable ? state.libQuery.trim() : '';
  const nq = norm(q);
  const filtered = nq
    ? p.songs.filter((s) => norm(s.name).includes(nq) || s.artists.some((a) => norm(a).includes(nq)) || norm(s.description).includes(nq))
    : p.songs;
  const list = sortSongs(filtered);
  registerList(p.key, list);

  if (state.loadError) return errorState();
  if (!state.loaded) return skeletonRows();
  if (!list.length) {
    return q
      ? emptyState({ iconName: 'search', title: `Nimic pentru „${q}”`, text: 'Încearcă alt cuvânt sau verifică ortografia.' })
      : p.emptyHtml;
  }
  return list.map((s, i) => trackRow(s, i + 1, q)).join('');
}

function refreshRows() {
  const rows = $('#track-rows');
  if (!rows || !currentPlaylist) return;
  rows.innerHTML = playlistRows();
  markPlaying();
}

function playlistView(p) {
  currentPlaylist = p;
  const sortOptions = Object.entries(SORT_LABEL)
    .map(([k, label]) => `<option value="${k}" ${state.sort.key === k ? 'selected' : ''}>${label}</option>`).join('');
  const html = `
    <section class="hero" style="--hue:${p.hue}">
      <div class="hero-cover ${p.round ? 'round' : ''}">${p.cover}</div>
      <div class="hero-info">
        <div class="hero-kicker">${esc(p.kicker)}</div>
        <h1 class="hero-title ${p.title.length > 16 ? 'long' : ''}">${esc(p.title)}</h1>
        <div class="hero-meta">${p.meta}</div>
      </div>
    </section>
    <div class="action-row">
      <button class="play-xl" data-action="play-list" data-list="${esc(p.key)}" title="Redă">${icon('play')}</button>
      <button class="pill-btn outline" data-action="create" ${p.prefillArtist ? `data-artist="${esc(p.prefillArtist)}"` : ''}>${icon('plus')}<span>Adaugă melodie</span></button>
      <span class="spacer"></span>
      ${p.filterable ? `<label class="inline-filter">${icon('search')}<input id="lib-filter" type="search" placeholder="Caută în playlist" value="${esc(state.libQuery)}" autocomplete="off"></label>` : ''}
      <select class="select" id="sort-select" aria-label="Sortează">${sortOptions}</select>
    </div>
    <div class="tracks">${trackHead()}<div id="track-rows" data-list="${esc(p.key)}">${playlistRows()}</div></div>`;
  return { html, hue: p.hue };
}

function metaLine(songs, extra = '') {
  const total = songs.reduce((sum, s) => sum + s.budget, 0);
  const via = `<span class="badge blue" title="${esc(hostOf(state.targets[state.source].url))}">via ${esc(sourceLabel())}</span>`;
  return `${extra}<span>${songsLabel(songs.length)}</span><span class="dot"></span><span>buget total <b>${fmtBudget(total)}</b></span><span class="dot"></span>${via}`;
}

const VIEWS = {
  home() {
    currentPlaylist = null;
    const recent = [...state.songs].sort(SORTS.recent);
    const hue = recent[0] ? hueOf(recent[0].id) : 140;
    const h = new Date().getHours();
    const greeting = h < 5 ? 'Noapte bună' : h < 12 ? 'Bună dimineața' : h < 18 ? 'Bună ziua' : 'Bună seara';

    if (state.loadError) return { hue, html: `<div class="home-hero"><h1 class="greeting">${greeting}</h1></div>${errorState()}` };
    if (!state.loaded) {
      const sk = Array.from({ length: 6 }, () => '<div class="quick"><div class="skeleton" style="width:64px;height:64px;border-radius:0"></div><div class="skeleton" style="height:12px;width:50%"></div></div>').join('');
      return { hue, html: `<div class="home-hero"><h1 class="greeting">${greeting}</h1><div class="quick-grid">${sk}</div></div>` };
    }
    if (!state.songs.length) {
      return {
        hue,
        html: `<div class="home-hero"><h1 class="greeting">${greeting}</h1></div>${emptyState({
          title: 'Biblioteca e goală', text: `${esc(sourceLabel())} nu a returnat nicio melodie. Adaugă prima melodie și urmărește cum se sincronizează între noduri.`,
          actions: `<button class="pill-btn light" data-action="create">${icon('plus')}<span>Adaugă melodie</span></button>`,
        })}`,
      };
    }

    const quickKey = registerList('home:quick', recent.slice(0, 6));
    const quick = [
      `<a class="quick" href="#/liked">${specialCover('liked', 64)}<span class="q-title">Melodii apreciate</span>
        <button class="play-fab" data-action="play-list" data-list="${registerList('liked', sortSongs(likedSongs()))}" title="Redă">${icon('play')}</button></a>`,
      `<a class="quick" href="#/library">${specialCover('all', 64)}<span class="q-title">Toate melodiile</span>
        <button class="play-fab" data-action="play-list" data-list="${registerList('library', sortSongs(state.songs))}" title="Redă">${icon('play')}</button></a>`,
      ...recent.slice(0, 6).map((s) => `
        <div class="quick" data-action="play-card" data-id="${esc(s.id)}" data-list="${quickKey}">${coverHtml(s, { size: 64 })}
          <span class="q-title">${esc(s.name)}</span>
          <button class="play-fab" data-action="play-card" data-id="${esc(s.id)}" title="Redă">${icon('play')}</button></div>`),
    ].join('');

    const songCard = (s, key) => `
      <div class="card" data-action="play-card" data-id="${esc(s.id)}" data-list="${key}">
        <div class="cover-wrap">${coverHtml(s)}<button class="play-fab" data-action="play-card" data-id="${esc(s.id)}" title="Redă">${icon('play')}</button></div>
        <div class="c-title">${esc(s.name)}</div><div class="c-sub">${artistLinks(s)}</div></div>`;

    const recentList = recent.slice(0, 12);
    const recentKey = registerList('home:recent', recentList);
    const byBudget = [...state.songs].sort(SORTS.budget).slice(0, 6);
    const budgetKey = registerList('home:budget', byBudget);
    const artists = artistIndex().slice(0, 12);
    const artistCards = artists.map((a) => `
      <a class="card" href="#/artist/${enc(a.name)}">
        <div class="cover-wrap">${artistCover(a.name)}<button class="play-fab" data-action="play-list" data-list="${registerList(`artist:${a.name}`, sortSongs(a.songs))}" title="Redă">${icon('play')}</button></div>
        <div class="c-title">${esc(a.name)}</div><div class="c-sub">Artist · ${songsLabel(a.songs.length)}</div></a>`).join('');

    return {
      hue,
      html: `
        <div class="home-hero" style="--hue:${hue}"><h1 class="greeting">${greeting}</h1><div class="quick-grid">${quick}</div></div>
        <div class="view-pad">
          <section class="section"><div class="section-head"><h2><a href="#/library">Modificate recent</a></h2><a class="more" href="#/library">Afișează tot</a></div>
            <div class="shelf">${recentList.map((s) => songCard(s, recentKey)).join('')}</div></section>
          ${artists.length ? `<section class="section"><div class="section-head"><h2><a href="#/search">Artiștii tăi</a></h2><a class="more" href="#/search">Afișează tot</a></div>
            <div class="shelf">${artistCards}</div></section>` : ''}
          <section class="section"><div class="section-head"><h2>Cele mai mari bugete</h2></div>
            <div class="shelf">${byBudget.map((s) => songCard(s, budgetKey)).join('')}</div></section>
        </div>`,
    };
  },

  library() {
    const songs = state.songs;
    return playlistView({
      key: 'library', kicker: 'Playlist', title: 'Toate melodiile', hue: 140, filterable: true,
      cover: mosaicCover(sortSongs(songs), { cls: 'fill' }), songs,
      meta: metaLine(songs, '<b>DistributedMinds</b><span class="dot"></span>'),
      emptyHtml: emptyState({ title: 'Nicio melodie încă', text: 'Adaugă prima melodie în colecție.', actions: `<button class="pill-btn light" data-action="create">${icon('plus')}<span>Adaugă melodie</span></button>` }),
    });
  },

  liked() {
    const songs = likedSongs();
    return playlistView({
      key: 'liked', kicker: 'Playlist', title: 'Melodii apreciate', hue: 252,
      cover: specialCover('liked', null, 'fill'), songs,
      meta: metaLine(songs, '<span>Salvate în acest browser</span><span class="dot"></span>'),
      emptyHtml: emptyState({ iconName: 'heart', title: 'Melodiile apreciate apar aici', text: 'Apasă pe inimă lângă o melodie ca s-o salvezi în acest playlist.', actions: '<a class="pill-btn light" href="#/library">Vezi toate melodiile</a>' }),
    });
  },

  artist(name) {
    const songs = state.songs.filter((s) => s.artists.some((a) => a.trim() === name));
    const collaborators = artistIndex(songs).filter((a) => a.name !== name).map((a) => `<a href="#/artist/${enc(a.name)}"><b>${esc(a.name)}</b></a>`);
    return playlistView({
      key: `artist:${name}`, kicker: 'Artist', title: name, hue: hueOf(`artist:${name}`), round: true, prefillArtist: name,
      cover: artistCover(name, { cls: 'fill' }), songs,
      meta: metaLine(songs, collaborators.length ? `<span>cu ${collaborators.slice(0, 3).join(', ')}${collaborators.length > 3 ? ' și alții' : ''}</span><span class="dot"></span>` : ''),
      emptyHtml: emptyState({ iconName: 'search', title: 'Niciun rezultat', text: `Nu există melodii pentru „${esc(name)}”.`, actions: '<a class="pill-btn light" href="#/library">Toate melodiile</a>' }),
    });
  },

  search(q) {
    currentPlaylist = null;
    const query = (q || '').trim();
    const pad = (inner) => `<div class="view-pad" style="padding-top:84px">${inner}</div>`;
    if (state.loadError) return { hue: null, html: pad(errorState()) };

    if (!query) {
      const tile = (href, label, cover, h) => `<a class="tile" href="${href}" style="--h:${h}"><span>${esc(label)}</span>${cover}</a>`;
      const tiles = [
        tile('#/library', 'Toate melodiile', mosaicCover(sortSongs(state.songs)), 140),
        tile('#/liked', 'Melodii apreciate', specialCover('liked'), 252),
        ...artistIndex().map((a) => tile(`#/artist/${enc(a.name)}`, a.name, artistCover(a.name), (hueOf(`artist:${a.name}`) + 30) % 360)),
      ].join('');
      return { hue: null, html: pad(`<section class="section" style="margin-top:0"><div class="section-head"><h2>Răsfoiește tot</h2></div><div class="browse-grid">${tiles}</div></section>`) };
    }

    const nq = norm(query);
    const score = (s) => {
      const n = norm(s.name);
      if (n === nq) return 5;
      if (n.startsWith(nq)) return 4;
      if (n.includes(nq)) return 3;
      if (s.artists.some((a) => norm(a).includes(nq))) return 2;
      return norm(s.description).includes(nq) ? 1 : 0;
    };
    const found = state.songs.map((s) => ({ s, sc: score(s) })).filter((x) => x.sc > 0)
      .sort((a, b) => b.sc - a.sc || SORTS.recent(a.s, b.s)).map((x) => x.s);
    const artists = artistIndex().filter((a) => norm(a.name).includes(nq));

    if (!found.length && !artists.length) {
      return { hue: null, html: pad(emptyState({ iconName: 'search', title: `Nu am găsit nimic pentru „${query}”`, text: 'Verifică ortografia sau caută după artist ori descriere.' })) };
    }

    const key = registerList(`search:${query}`, found);
    const topArtist = artists.find((a) => norm(a.name).startsWith(nq));
    const topIsArtist = topArtist && (!found.length || score(found[0]) < 4);
    let top;
    if (topIsArtist) {
      top = `<a class="top-card" href="#/artist/${enc(topArtist.name)}">${artistCover(topArtist.name, { size: 96 })}
        <div class="tc-title">${highlight(topArtist.name, query)}</div><div class="tc-sub"><span class="badge">Artist</span>${songsLabel(topArtist.songs.length)}</div>
        <button class="play-fab" data-action="play-list" data-list="${registerList(`artist:${topArtist.name}`, sortSongs(topArtist.songs))}" title="Redă">${icon('play')}</button></a>`;
    } else {
      const s = found[0];
      top = `<div class="top-card" data-action="play-card" data-id="${esc(s.id)}" data-list="${key}">${coverHtml(s, { size: 96 })}
        <div class="tc-title">${highlight(s.name, query)}</div><div class="tc-sub"><span class="badge">Melodie</span><span>${artistLinks(s, query)}</span></div>
        <button class="play-fab" data-action="play-card" data-id="${esc(s.id)}" title="Redă">${icon('play')}</button></div>`;
    }

    const compact = found.slice(0, 5).map((s) => {
      const liked = state.liked.has(s.id);
      return `<div class="track" data-action="play-row" data-id="${esc(s.id)}" data-n="">
        <div class="t-main">${coverHtml(s, { size: 40 })}<div class="t-text"><div class="t-name">${highlight(s.name, query)}</div><div class="t-artists">${artistLinks(s, query)}</div></div></div>
        <div class="t-actions">
          <button class="icon-btn hover-only ${liked ? 'liked' : ''}" data-action="like" data-id="${esc(s.id)}" title="Apreciază">${icon(liked ? 'heart-fill' : 'heart')}</button>
          <button class="icon-btn hover-only" data-action="edit" data-id="${esc(s.id)}" title="Editează">${icon('edit')}</button></div></div>`;
    }).join('');

    const artistShelf = artists.length ? `
      <section class="section"><div class="section-head"><h2>Artiști</h2></div><div class="shelf">${artists.slice(0, 12).map((a) => `
        <a class="card" href="#/artist/${enc(a.name)}"><div class="cover-wrap">${artistCover(a.name)}
          <button class="play-fab" data-action="play-list" data-list="${registerList(`artist:${a.name}`, sortSongs(a.songs))}" title="Redă">${icon('play')}</button></div>
          <div class="c-title">${highlight(a.name, query)}</div><div class="c-sub">Artist</div></a>`).join('')}</div></section>` : '';

    const allRows = found.length > 5 ? `
      <section class="section"><div class="section-head"><h2>Toate rezultatele · ${found.length}</h2></div>
        <div class="tracks" style="padding:0">${trackHead()}<div data-list="${key}">${sortSongs(found).map((s, i) => trackRow(s, i + 1, query)).join('')}</div></div></section>` : '';

    return {
      hue: null,
      html: pad(`
        <div class="top-results">
          <section><div class="section-head"><h2>Rezultatul principal</h2></div>${top}</section>
          ${found.length ? `<section><div class="section-head"><h2>Melodii</h2></div><div class="compact-list" data-list="${key}">${compact}</div></section>` : '<section></section>'}
        </div>${artistShelf}${allRows}`),
    };
  },

  cluster() {
    currentPlaylist = null;
    const c = state.cluster;
    const card = (key, iconName, role, stats) => {
      const s = c[key];
      const t = state.targets[key];
      return `<article class="svc ${s.status}">
        <div class="svc-head"><div class="svc-icon">${icon(iconName)}</div>
          <div><div class="svc-name">${esc(t.label)}</div><div class="svc-url">${esc(hostOf(t.url))}</div></div>
          <span class="status-pill"><span class="dot-led"></span>${STATUS_LABEL[s.status]}</span></div>
        <div class="svc-stats">${stats.map(([k, v]) => `<div class="stat"><div class="k">${k}</div><div class="v">${v}</div></div>`).join('')}</div>
        <div class="svc-note">${esc(s.note || role)}</div></article>`;
    };
    const count = (k) => (c[k].data ? c[k].data.length : '—');

    const flow = `<div class="flow">
      <span class="node">Browser</span><span class="arrow">→</span><span class="node">UI :${location.port || 80}</span><span class="arrow">→</span>
      <span class="node">Smart Proxy :${portOf('gateway')}</span><span class="arrow">round-robin →</span>
      <span class="node">MusicAPI :${portOf('node1')}</span><span class="node">MusicAPI :${portOf('node2')}</span>
      <span class="arrow">⇄</span><span class="node">SyncNode :${portOf('sync')}</span><span class="arrow">· fiecare nod are MongoDB propriu</span></div>`;

    // Per-node comparison: what each MusicAPI returns (Mongo + pending overlay from SyncNode).
    const n1 = c.node1.data;
    const n2 = c.node2.data;
    const rows = new Map();
    for (const [side, data] of [['a', n1], ['b', n2]]) {
      for (const raw of data || []) {
        const s = normalizeSong(raw);
        if (!rows.has(s.id)) rows.set(s.id, { id: s.id });
        rows.get(s.id)[side] = s;
      }
    }
    const same = (a, b) => a.name === b.name && a.description === b.description && a.budget === b.budget
      && a.artists.join('|') === b.artists.join('|')
      && Math.abs((parseDate(a.lastChangedAt)?.getTime() || 0) - (parseDate(b.lastChangedAt)?.getTime() || 0)) < 2;
    const bothUp = n1 && n2;
    const cmp = [...rows.values()].map((r) => {
      let status;
      if (!bothUp) status = ['Nod indisponibil', ''];
      else if (r.a && r.b) status = same(r.a, r.b) ? ['Sincronizat', 'green'] : ['Diferă', 'warn'];
      else status = [r.a ? 'Doar pe #1' : 'Doar pe #2', 'red'];
      return { ...r, status, order: { Diferă: 0, 'Doar pe #1': 1, 'Doar pe #2': 1, 'Nod indisponibil': 2, Sincronizat: 3 }[status[0]] };
    }).sort((x, y) => x.order - y.order || (x.a || x.b).name.localeCompare((y.a || y.b).name, 'ro'));
    const synced = cmp.filter((r) => r.status[0] === 'Sincronizat').length;
    const cell = (s) => (s ? `<span class="check">${icon('check')}</span> <span class="ago">${relTime(s.lastChangedAt)}</span>` : '<span class="cross">—</span>');
    const cmpRows = cmp.length
      ? cmp.map((r) => `<tr><td class="main-cell">${esc((r.a || r.b).name)}</td><td>${cell(r.a)}</td><td>${cell(r.b)}</td>
          <td><span class="badge ${r.status[1]}">${r.status[0]}</span></td></tr>`).join('')
      : `<tr class="empty-row"><td colspan="4">${n1 || n2 ? 'Niciun nod nu are melodii.' : 'Niciun nod MusicAPI nu răspunde.'}</td></tr>`;

    const pending = c.sync.data;
    const queueRows = pending && pending.length
      ? pending.map((e) => {
        let name = '—';
        try { const d = JSON.parse(e.jsonData); name = d.Name ?? d.name ?? '—'; } catch { /* keep placeholder */ }
        const del = String(e.syncType).toUpperCase() === 'DELETE';
        return `<tr><td class="main-cell">${esc(name)}</td><td><span class="badge ${del ? 'red' : 'green'}">${esc(e.syncType)}</span></td>
          <td class="mono">${esc(e.origin)}</td><td><span class="ago" title="${esc(fullDate(e.lastChangedAt))}">${relTime(e.lastChangedAt)}</span></td></tr>`;
      }).join('')
      : `<tr class="empty-row"><td colspan="4">${pending ? 'Coada e goală — SyncNode a trimis toate modificările.' : esc(c.sync.note || 'SyncNode indisponibil.')}</td></tr>`;

    const activity = state.activity.length
      ? state.activity.slice(0, 25).map((a) => {
        const cls = a.status === 0 || a.status >= 500 ? 'red' : a.status >= 400 ? 'warn' : 'green';
        return `<tr><td class="ago">${new Date(a.at).toLocaleTimeString('ro-RO')}</td><td><b>${a.method}</b></td><td>${esc(targetLabel(a.target))}</td>
          <td class="mono">${esc(a.path)}</td><td><span class="badge ${cls}">${a.status || 'eșec'}</span>${a.retries ? ` <span class="ago">${a.retries}× retry</span>` : ''}</td><td class="ago">${fmtMs(a.ms)}</td></tr>`;
      }).join('')
      : '<tr class="empty-row"><td colspan="6">Încă nicio cerere. Adaugă, editează sau șterge o melodie.</td></tr>';

    const checked = state.clusterCheckedAt ? `verificat ${relTime(state.clusterCheckedAt)} · se actualizează la 5 s` : 'se verifică…';
    return {
      hue: 165,
      html: `
        <section class="hero" style="--hue:165;min-height:260px">
          <div class="hero-cover" style="width:160px;height:160px"><div class="cover fill" style="--h:150;font-size:64px">${icon('server')}</div></div>
          <div class="hero-info"><div class="hero-kicker">Sistem distribuit</div><h1 class="hero-title">Cluster</h1>
            <div class="hero-meta"><span>${SERVICES.filter((k) => c[k].status === 'online').length}/${SERVICES.length} servicii online</span><span class="dot"></span><span>${checked}</span></div></div>
        </section>
        <div class="view-pad" style="position:relative;z-index:1;padding-top:24px">
          ${flow}
          <div class="topology">
            ${card('gateway', 'globe', 'Ocelot · round-robin între noduri', [['Melodii', count('gateway')], ['Latență', fmtMs(c.gateway.ms)]])}
            ${card('node1', 'database', 'MusicAPI + MongoDB', [['Melodii', count('node1')], ['Latență', fmtMs(c.node1.ms)]])}
            ${card('node2', 'database', 'MusicAPI + MongoDB', [['Melodii', count('node2')], ['Latență', fmtMs(c.node2.ms)]])}
            ${card('sync', 'sync', 'Propagă modificările la fiecare 20 s', [['În coadă', count('sync')], ['Latență', fmtMs(c.sync.ms)]])}
          </div>
          <div class="two-col">
            <section class="panel-card">
              <div class="pc-head"><h3>Comparare noduri</h3><p>Ce returnează fiecare MusicAPI</p>
                <div class="right">${bothUp ? `<span class="badge ${synced === cmp.length ? 'green' : 'warn'}">${synced}/${cmp.length} sincronizate</span>` : ''}
                  <button class="icon-btn" data-action="probe" title="Verifică acum">${icon('refresh')}</button></div></div>
              <div class="table-wrap"><table class="grid"><thead><tr><th>Melodie</th><th>Nod #1</th><th>Nod #2</th><th>Stare</th></tr></thead><tbody>${cmpRows}</tbody></table></div>
            </section>
            <section class="panel-card">
              <div class="pc-head"><h3>Coada SyncNode</h3><p>Modificări care așteaptă propagarea</p></div>
              <div class="table-wrap"><table class="grid"><thead><tr><th>Melodie</th><th>Operație</th><th>Origine</th><th>Modificat</th></tr></thead><tbody>${queueRows}</tbody></table></div>
            </section>
          </div>
          <section class="panel-card" style="margin-top:16px">
            <div class="pc-head"><h3>Activitate</h3><p>Cererile trimise de acest UI</p></div>
            <div class="table-wrap"><table class="grid"><thead><tr><th>Ora</th><th>Metodă</th><th>Țintă</th><th>Cale</th><th>Status</th><th>Durată</th></tr></thead><tbody>${activity}</tbody></table></div>
          </section>
        </div>`,
    };
  },
};

/* ================= modals ================= */

function openModal(html) {
  const root = $('#modal-root');
  root.innerHTML = html;
  const first = root.querySelector('[autofocus]') || root.querySelector('input, textarea, button');
  setTimeout(() => first?.focus(), 30);
}
const closeModal = () => { $('#modal-root').innerHTML = ''; };
const modalOpen = () => !!$('#modal-root').firstElementChild;

function openEditor(song, prefill = {}) {
  const isNew = !song;
  const draft = {
    id: song ? song.id : uuid(),
    name: song?.name ?? '',
    artists: [...(song?.artists ?? prefill.artists ?? [])],
    budget: song ? song.budget : '',
    description: song?.description ?? '',
  };
  const method = isNew ? 'POST' : 'PUT';

  openModal(`
    <div class="modal-backdrop" data-action="modal-close-bg">
      <form class="modal wide" id="song-form" novalidate>
        <div class="modal-head"><h2>${isNew ? 'Melodie nouă' : 'Editează detaliile'}</h2>
          <button type="button" class="icon-btn" data-action="modal-close" title="Închide">${icon('close')}</button></div>
        <div class="edit-grid">
          <div id="preview-cover">${coverHtml(draft, { size: 180 })}</div>
          <div class="fields">
            <div class="field"><label for="f-name">Nume</label><input class="input" id="f-name" name="name" placeholder="Numele melodiei" maxlength="120" value="${esc(draft.name)}" autofocus></div>
            <div class="field"><label>Artiști</label><div class="input artist-input" id="artist-input"><input id="f-artist" placeholder="Adaugă artist și apasă Enter" autocomplete="off"></div></div>
            <div class="field"><label for="f-budget">Buget</label><input class="input" id="f-budget" name="budget" type="number" min="0" step="any" placeholder="Buget" value="${esc(draft.budget)}"></div>
            <div class="field"><label for="f-desc">Descriere</label><textarea class="input" id="f-desc" name="description" placeholder="Descriere">${esc(draft.description)}</textarea></div>
          </div>
        </div>
        <div class="form-error" id="form-error"></div>
        <div class="modal-foot">
          <span class="note">${method} prin <b>${esc(sourceLabel())}</b>. SyncNode propagă modificarea pe celălalt nod în cel mult 20 s.</span>
          <button type="submit" class="pill-btn light" id="save-btn">Salvează</button>
        </div>
      </form>
    </div>`);

  const form = $('#song-form');
  const tagBox = $('#artist-input');
  const tagInput = $('#f-artist');
  const refreshFilled = () => {
    $$('.field', form).forEach((f) => {
      const input = f.querySelector('input:not(#f-artist), textarea');
      f.classList.toggle('filled', input ? input.value !== '' : draft.artists.length > 0 || tagInput.value !== '');
    });
  };
  const renderTags = () => {
    $$('.tag', tagBox).forEach((t) => t.remove());
    tagInput.insertAdjacentHTML('beforebegin', draft.artists.map((a, i) =>
      `<span class="tag">${esc(a)}<button type="button" data-remove="${i}" title="Elimină">${icon('close')}</button></span>`).join(''));
    refreshFilled();
  };
  const addTags = (raw) => {
    for (const part of raw.split(',')) {
      const v = part.trim();
      if (v && !draft.artists.some((a) => a.toLowerCase() === v.toLowerCase())) draft.artists.push(v);
    }
    tagInput.value = '';
    renderTags();
  };

  tagInput.addEventListener('keydown', (e) => {
    if (e.key === 'Enter' || e.key === ',') {
      e.preventDefault();
      addTags(tagInput.value);
    } else if (e.key === 'Backspace' && !tagInput.value && draft.artists.length) {
      draft.artists.pop();
      renderTags();
    }
  });
  tagInput.addEventListener('blur', () => { if (tagInput.value.trim()) addTags(tagInput.value); });
  tagBox.addEventListener('click', (e) => {
    const rm = e.target.closest('[data-remove]');
    if (rm) {
      draft.artists.splice(Number(rm.dataset.remove), 1);
      renderTags();
    }
    tagInput.focus();
  });
  form.addEventListener('input', (e) => {
    refreshFilled();
    if (e.target.id === 'f-name') $('#preview-cover').innerHTML = coverHtml({ id: draft.id, name: e.target.value }, { size: 180 });
  });
  renderTags();

  form.addEventListener('submit', async (e) => {
    e.preventDefault();
    if (tagInput.value.trim()) addTags(tagInput.value);
    const errEl = $('#form-error');
    const name = $('#f-name').value.trim();
    const description = $('#f-desc').value.trim();
    const budgetRaw = $('#f-budget').value.trim();
    const budget = budgetRaw === '' ? 0 : Number(budgetRaw);

    if (!name) { errEl.textContent = 'Numele este obligatoriu.'; $('#f-name').focus(); return; }
    if (!Number.isFinite(budget) || budget < 0) { errEl.textContent = 'Bugetul trebuie să fie un număr pozitiv.'; $('#f-budget').focus(); return; }

    const payload = { id: draft.id, name, artists: draft.artists, budget, description };
    const btn = $('#save-btn');
    btn.disabled = true;
    btn.textContent = 'Se salvează…';
    errEl.textContent = '';
    try {
      const saved = await request(state.source, method, '/api/music', payload);
      closeModal();
      upsertLocal(normalizeSong(saved && typeof saved === 'object' ? saved : { ...payload, lastChangedAt: new Date().toISOString() }));
      toast(`„${name}” ${isNew ? 'a fost adăugată' : 'a fost actualizată'}`, 'ok', `${method} prin ${sourceLabel()} · SyncNode o propagă în ≤ 20 s`);
      loadSongs({ quiet: true });
      probeCluster();
    } catch (err) {
      errEl.textContent = describeError(err);
      btn.disabled = false;
      btn.textContent = 'Salvează';
    }
  });
}

function confirmDelete(song) {
  openModal(`
    <div class="modal-backdrop" data-action="modal-close-bg">
      <div class="modal" role="dialog" aria-modal="true" aria-labelledby="del-title">
        <div class="modal-head"><h2 id="del-title">Ștergi melodia?</h2>
          <button class="icon-btn" data-action="modal-close" title="Închide">${icon('close')}</button></div>
        <p class="confirm-body">„<b>${esc(song.name)}</b>” va fi ștearsă de pe nodul care primește cererea (prin ${esc(sourceLabel())}). SyncNode propagă apoi ștergerea pe celălalt nod.</p>
        <div class="form-error" id="form-error"></div>
        <div class="modal-foot">
          <button class="pill-btn ghost" data-action="modal-close" autofocus>Anulează</button>
          <button class="pill-btn danger" id="confirm-delete">Șterge</button>
        </div>
      </div>
    </div>`);

  $('#confirm-delete').addEventListener('click', async (e) => {
    const btn = e.currentTarget;
    btn.disabled = true;
    btn.textContent = 'Se șterge…';
    try {
      await request(state.source, 'DELETE', `/api/music/${enc(song.id)}`);
      closeModal();
      state.songs = state.songs.filter((s) => s.id !== song.id);
      if (state.liked.delete(song.id)) store.set('liked', [...state.liked]);
      pruneQueue();
      renderAll();
      toast(`„${song.name}” a fost ștearsă`, 'ok', `DELETE prin ${sourceLabel()} · SyncNode propagă ștergerea în ≤ 20 s`);
      loadSongs({ quiet: true });
      probeCluster();
    } catch (err) {
      $('#form-error').textContent = err.status === 400
        ? `Nodul care a primit cererea nu are melodia în baza lui de date (${describeError(err)}). Încearcă din nou sau alege direct nodul.`
        : describeError(err);
      btn.disabled = false;
      btn.textContent = 'Șterge';
    }
  });
}

/* ================= toasts ================= */

function toast(message, kind = 'info', sub = '') {
  const root = $('#toast-root');
  while (root.children.length >= 3) root.firstElementChild.remove();
  const el = document.createElement('div');
  el.className = `toast ${kind}`;
  el.setAttribute('role', kind === 'err' ? 'alert' : 'status');
  el.innerHTML = `<div>${esc(message)}${sub ? `<small>${esc(sub)}</small>` : ''}</div>`;
  root.append(el);
  setTimeout(() => {
    el.classList.add('out');
    el.addEventListener('animationend', () => el.remove(), { once: true });
  }, kind === 'err' ? 6500 : 3800);
}

/* ================= routing & events ================= */

function parseRoute() {
  const [name = 'home', ...rest] = location.hash.replace(/^#\/?/, '').split('/');
  const arg = rest.length ? decodeURIComponent(rest.join('/')) : '';
  return { name: VIEWS[name] ? name : 'home', arg };
}

window.addEventListener('hashchange', () => {
  const prev = state.route;
  state.route = parseRoute();
  if (prev.name !== state.route.name || prev.arg !== state.route.arg) {
    if (!(prev.name === 'search' && state.route.name === 'search')) $('#main').scrollTop = 0;
    if (prev.name !== state.route.name) state.libQuery = '';
    renderSidebar();
    renderView();
  }
});

function onScroll() {
  $('#topbar').classList.toggle('solid', $('#main').scrollTop > 40);
}
$('#main').addEventListener('scroll', onScroll, { passive: true });

const ACTIONS = {
  back: () => history.back(),
  forward: () => history.forward(),
  refresh: () => { loadSongs(); probeCluster(); },
  probe: () => probeCluster(),
  create: (el) => openEditor(null, el.dataset.artist ? { artists: [el.dataset.artist] } : {}),
  edit: (el) => { const s = songById(el.dataset.id); if (s) openEditor(s); },
  delete: (el) => { const s = songById(el.dataset.id); if (s) confirmDelete(s); },
  like: (el) => toggleLike(el.dataset.id),
  source: (el) => {
    if (state.source === el.dataset.key) return;
    state.source = el.dataset.key;
    store.set('source', state.source);
    state.loaded = false;
    renderSourceSwitch();
    loadSongs();
  },
  sort: (el) => {
    const key = el.dataset.key;
    state.sort = { key, dir: state.sort.key === key ? -state.sort.dir : 1 };
    renderView();
  },
  select: (el) => {
    state.selectedId = el.dataset.id;
    $$('#view .track.selected').forEach((r) => r.classList.remove('selected'));
    el.classList.add('selected');
  },
  'play-row': (el) => playFrom(el.closest('[data-list]')?.dataset.list, el.dataset.id),
  'play-card': (el) => playFrom(el.closest('[data-list]')?.dataset.list || el.closest('.card,.quick,.top-card')?.dataset.list, el.dataset.id),
  'play-list': (el) => playFrom(el.dataset.list),
  toggle: () => togglePlay(),
  next: () => step(1),
  prev: () => step(-1),
  shuffle: () => { player.shuffle = !player.shuffle; renderPlayer(); },
  repeat: () => { player.repeat = !player.repeat; renderPlayer(); },
  seek: (el, e) => {
    const song = currentSong();
    if (!song) return;
    const r = el.getBoundingClientRect();
    player.pos = Math.max(0, Math.min(1, (e.clientX - r.left) / r.width)) * durationOf(song);
    updateProgress();
  },
  'modal-close': () => closeModal(),
  'modal-close-bg': (el, e) => { if (e.target === el) closeModal(); },
};

document.addEventListener('click', (e) => {
  const chip = e.target.closest('#library-chips .chip');
  if (chip) {
    state.libFilter = state.libFilter === chip.dataset.filter ? 'all' : chip.dataset.filter;
    store.set('libFilter', state.libFilter);
    renderSidebar();
    return;
  }
  const el = e.target.closest('[data-action]');
  if (!el) return;
  // Let plain links inside an actionable card (e.g. artist names) navigate normally.
  const link = e.target.closest('a[href]');
  if (link && el.contains(link) && link !== el) return;
  const handler = ACTIONS[el.dataset.action];
  if (!handler) return;
  if (el.tagName === 'BUTTON' || el.closest('a[href]')) e.preventDefault();
  handler(el, e);
});

document.addEventListener('dblclick', (e) => {
  const row = e.target.closest('#view .track[data-action="select"]');
  if (row && !e.target.closest('button, a')) playFrom(row.closest('[data-list]')?.dataset.list, row.dataset.id);
});

document.addEventListener('input', (e) => {
  if (e.target.id === 'lib-filter') {
    state.libQuery = e.target.value;
    refreshRows();
  } else if (e.target.id === 'search-input') {
    const hash = `#/search/${enc(e.target.value)}`;
    if (state.route.name === 'search') {
      history.replaceState(null, '', hash);
      state.route = parseRoute();
      renderView();
    } else {
      location.hash = hash;
    }
  }
});

document.addEventListener('change', (e) => {
  if (e.target.id === 'sort-select') {
    state.sort = { key: e.target.value, dir: 1 };
    renderView();
  }
});

$('#search-input').addEventListener('focus', () => {
  if (state.route.name !== 'search') location.hash = `#/search/${enc($('#search-input').value)}`;
});

document.addEventListener('keydown', (e) => {
  const typing = e.target.closest('input, textarea, select, [contenteditable]');
  if (e.key === 'Escape' && modalOpen()) { closeModal(); return; }
  if (typing || modalOpen() || e.ctrlKey || e.metaKey || e.altKey) return;
  if (e.key === '/') {
    e.preventDefault();
    $('#search-input').focus();
  } else if (e.code === 'Space' && !e.target.closest('button, a')) {
    e.preventDefault();
    togglePlay();
  }
});

document.addEventListener('visibilitychange', () => {
  if (!document.hidden) { probeCluster(); loadSongs({ quiet: true }); }
});

/* ================= boot ================= */

(async function init() {
  hydrateIcons();
  state.route = parseRoute();
  try {
    const cfg = await fetch('/config.json', { cache: 'no-store' }).then((r) => r.json());
    if (cfg && cfg.targets) state.targets = cfg.targets;
  } catch { /* keep defaults */ }
  renderAll();
  await Promise.all([loadSongs({ quiet: true }), probeCluster()]);

  setInterval(tick, 250);
  setInterval(() => { if (!document.hidden) probeCluster(); }, 5000);
  setInterval(() => { if (!document.hidden && !modalOpen()) loadSongs({ quiet: true }); }, 15000);
}());
