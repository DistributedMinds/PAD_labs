# Music UI

Interfață web pentru clusterul MusicAPI (Smart Proxy → MusicAPI ×2 ⇄ SyncNode).
Nu are dependențe: doar Node.js (≥ 18).

```bash
node UI/server.js
```

Apoi deschide http://localhost:8080.

`server.js` servește fișierele din `public/` și face proxy către servicii prin
`/upstream/<țintă>/...`, așa că browserul nu are nevoie de CORS pe serviciile .NET.

| Variabilă     | Implicit                | Ce este                 |
|---------------|-------------------------|-------------------------|
| `PORT`        | `8080`                  | portul UI-ului          |
| `GATEWAY_URL` | `http://localhost:9000` | Smart Proxy (Ocelot)    |
| `NODE1_URL`   | `http://localhost:9001` | MusicAPI #1             |
| `NODE2_URL`   | `http://localhost:9002` | MusicAPI #2             |
| `SYNC_URL`    | `http://localhost:5000` | SyncNode                |

## Ce conține

- **Acasă / Toate melodiile / Artiști / Căutare** — CRUD pe `/api/music` (adaugă, editează, șterge).
- **Sursa datelor** (sus-dreapta): citește și scrie prin Gateway sau direct pe un nod.
  Prin Gateway, un 502 (nod căzut în round-robin) e reîncercat automat.
- **Cluster** — starea fiecărui serviciu, comparația melodiilor între noduri,
  coada SyncNode și jurnalul cererilor trimise de UI.
- Badge-ul **sync** marchează melodiile aflate încă în coada SyncNode.
- „Melodii apreciate” și player-ul sunt doar în browser (redare simulată, melodiile nu au audio).
