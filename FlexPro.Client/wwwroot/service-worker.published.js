self.importScripts('./service-worker-assets.js');
self.addEventListener('install', event => event.waitUntil(onInstall(event)));
self.addEventListener('activate', event => event.waitUntil(onActivate(event)));
self.addEventListener('fetch', event => event.respondWith(onFetch(event)));

const cacheNamePrefix = 'offline-cache-';
const cacheName = `${cacheNamePrefix}${self.assetsManifest.version}`;
const offlineAssetsInclude = [ /\.dll$/, /\.pdb$/, /\.wasm/, /\.html$/, /\.js$/, /\.json$/, /\.css$/, /\.woff$/, /\.png$/, /\.jpe?g$/, /\.gif$/, /\.ico$/, /\.blat$/, /\.dat$/ ];
const offlineAssetsExclude = [ /^service-worker\.js$/ ];

// Subdiretório base no GitHub Pages
const base = "/flexpro/";
const baseUrl = new URL(base, self.origin);
const manifestPaths = self.assetsManifest.assets.map(asset => new URL(asset.url, baseUrl).pathname);

async function onInstall(event) {
    console.info('Service worker: Install');
    const assetsRequests = self.assetsManifest.assets
        .filter(asset => offlineAssetsInclude.some(pattern => pattern.test(asset.url)))
        .filter(asset => !offlineAssetsExclude.some(pattern => pattern.test(asset.url)))
        .map(asset => new Request(asset.url, { integrity: asset.hash, cache: 'no-cache' }));
    await caches.open(cacheName).then(cache => cache.addAll(assetsRequests));
}

async function onActivate(event) {
    console.info('Service worker: Activate');
    const cacheKeys = await caches.keys();
    await Promise.all(cacheKeys
        .filter(key => key.startsWith(cacheNamePrefix) && key !== cacheName)
        .map(key => caches.delete(key)));
}

async function onFetch(event) {
    let cachedResponse = null;

    if (event.request.method === 'GET') {
        const requestPath = new URL(event.request.url).pathname;
        const shouldServeIndexHtml = event.request.mode === 'navigate'
            && !manifestPaths.includes(requestPath);

        const request = shouldServeIndexHtml ? new Request(`${base}index.html`) : event.request;
        const cache = await caches.open(cacheName);
        cachedResponse = await cache.match(request);
    }

    try {
        return cachedResponse || await fetch(event.request);
    } catch {
        if (event.request.mode === 'navigate') {
            const cache = await caches.open(cacheName);
            return await cache.match(`${base}index.html`);
        }
        return cachedResponse;
    }
}
