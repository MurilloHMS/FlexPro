self.addEventListener('install', (event) => {
    event.waitUntil(
        caches.open('flexpro-cache').then((cache) => {
            return cache.addAll([
                '/', 
                '/index.html',
                '/css/app.css',
                '/_framework/blazor.webassembly.js',
                '/_content/MudBlazor/MudBlazor.min.css',
                '/icon-512.png',
                '/icon-192.png',
                '/manifest.webmanifest'
            ]);
        })
    );
});

self.addEventListener('fetch', (event) => {
    event.respondWith(
        caches.match(event.request).then((cachedResponse) => {
            return cachedResponse || fetch(event.request);
        })
    );
});

self.addEventListener('activate', (event) => {
    const cacheWhitelist = ['flexpro-cache'];
    event.waitUntil(
        caches.keys().then((cacheNames) => {
            return Promise.all(
                cacheNames.map((cacheName) => {
                    if (!cacheWhitelist.includes(cacheName)) {
                        return caches.delete(cacheName);
                    }
                })
            );
        })
    );
});
