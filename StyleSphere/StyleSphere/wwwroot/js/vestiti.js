/* =========================================================
   VESTITI — Storefront Interactive Behaviour
   Cart, wishlist, promos, live search, quick view, menus, toasts
   ========================================================= */
(function () {
    'use strict';

    document.documentElement.classList.add('js');

    const CART_KEY = 'vestitiCart';
    const WISH_KEY = 'vestitiWishlist';
    const PROMO_KEY = 'vestitiPromo';
    const FREE_SHIP = 150;
    const PROMOS = { VESTITI10: 0.10, WELCOME15: 0.15 };

    const catalog = (window.VESTITI && window.VESTITI.catalog) || [];
    const byId = id => catalog.find(p => p.id === Number(id));
    const money = n => '$' + Number(n || 0).toFixed(2);
    const esc = s => String(s ?? '').replace(/[&<>"']/g, c => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]));
    const img = (id, w) => `https://images.unsplash.com/photo-${id}?auto=format&fit=crop&w=${w}&q=80`;
    const $ = (s, r = document) => r.querySelector(s);
    const $$ = (s, r = document) => Array.from(r.querySelectorAll(s));

    /* ---------- Storage ---------- */
    function read(k) {
        try { return JSON.parse(localStorage.getItem(k)) || []; } catch { return []; }
    }
    function write(k, v) {
        try { localStorage.setItem(k, JSON.stringify(v)); } catch { /* ignore quota */ }
        updateBadges();
        document.dispatchEvent(new CustomEvent('vestiti:change'));
    }

    const Cart = {
        all: () => read(CART_KEY),
        count: () => Cart.all().reduce((s, i) => s + (Number(i.qty) || 0), 0),
        subtotal: () => Cart.all().reduce((s, i) => s + Number(i.price) * Number(i.qty), 0),
        add(id, opts = {}) {
            const p = byId(id);
            if (!p || p.stock <= 0) return false;
            const size = opts.size || (p.sizes && p.sizes.length ? p.sizes[0] : null);
            const color = opts.color || (p.colors && p.colors.length ? p.colors[0].split('|')[0] : null);
            const qty = Math.max(1, Number(opts.qty) || 1);
            const cart = Cart.all();
            const key = [p.id, size || '', color || ''].join('|');
            const ex = cart.find(i => i.key === key);
            if (ex) {
                ex.qty = Math.min(ex.qty + qty, p.stock);
            } else {
                cart.push({
                    key,
                    id: p.id,
                    name: p.name,
                    price: Number(p.price),
                    image: p.images[0],
                    size,
                    color,
                    qty: Math.min(qty, p.stock)
                });
            }
            write(CART_KEY, cart);
            const cb = $('#cart-count');
            if (cb) { cb.classList.remove('bump'); void cb.offsetWidth; cb.classList.add('bump'); }
            return true;
        },
        setQty(key, qty) {
            let cart = Cart.all();
            const it = cart.find(i => i.key === key);
            if (!it) return;
            const p = byId(it.id);
            if (qty <= 0) cart = cart.filter(i => i.key !== key);
            else it.qty = Math.min(qty, p ? p.stock : 99);
            write(CART_KEY, cart);
        },
        remove(key) { write(CART_KEY, Cart.all().filter(i => i.key !== key)); },
        clear() { write(CART_KEY, []); }
    };

    const Wish = {
        all: () => read(WISH_KEY),
        has: id => Wish.all().includes(Number(id)),
        toggle(id) {
            id = Number(id);
            let w = Wish.all();
            const on = !w.includes(id);
            w = on ? [...w, id] : w.filter(x => x !== id);
            write(WISH_KEY, w);
            return on;
        }
    };

    const Promo = {
        get: () => { try { return localStorage.getItem(PROMO_KEY); } catch { return null; } },
        apply(code) {
            code = (code || '').trim().toUpperCase();
            if (!PROMOS[code]) return false;
            try { localStorage.setItem(PROMO_KEY, code); } catch { /* ignore */ }
            document.dispatchEvent(new CustomEvent('vestiti:change'));
            return true;
        },
        clear() {
            try { localStorage.removeItem(PROMO_KEY); } catch { /* ignore */ }
            document.dispatchEvent(new CustomEvent('vestiti:change'));
        }
    };

    function totals(method = 'standard') {
        const subtotal = Cart.subtotal();
        const code = Promo.get();
        const discount = code && PROMOS[code] ? +(subtotal * PROMOS[code]).toFixed(2) : 0;
        const after = Math.max(0, subtotal - discount);
        let shipping = 0;
        if (subtotal > 0) {
            if (method === 'express') shipping = 24;
            else if (method === 'pickup') shipping = 0;
            else shipping = subtotal >= FREE_SHIP ? 0 : 12;
        }
        return { subtotal, discount, code, shipping, total: +(after + shipping).toFixed(2) };
    }

    /* ---------- Toasts ---------- */
    function toast(message, opts = {}) {
        let wrap = $('.toasts');
        if (!wrap) {
            wrap = document.createElement('div');
            wrap.className = 'toasts';
            wrap.setAttribute('role', 'status');
            wrap.setAttribute('aria-live', 'polite');
            document.body.appendChild(wrap);
        }
        const t = document.createElement('div');
        t.className = 'toast' + (opts.type === 'error' ? ' error' : '');
        t.innerHTML = `<i class="fa-solid ${opts.type === 'error' ? 'fa-circle-exclamation' : 'fa-circle-check'}"></i><span>${esc(message)}</span>${opts.href ? `<a href="${opts.href}">${esc(opts.linkText || 'View')}</a>` : ''}`;
        wrap.appendChild(t);
        setTimeout(() => { t.classList.add('out'); setTimeout(() => t.remove(), 260); }, opts.duration || 3400);
    }

    /* ---------- Badges & Wishlist Icons ---------- */
    function updateBadges() {
        const c = Cart.count(), w = Wish.all().length;
        const cb = $('#cart-count'), wb = $('#wish-count');
        if (cb) { cb.textContent = c; cb.hidden = c === 0; }
        if (wb) { wb.textContent = w; wb.hidden = w === 0; }
        $$('[data-wish]').forEach(b => {
            const on = Wish.has(b.dataset.wish);
            b.classList.toggle('active', on);
            b.setAttribute('aria-pressed', String(on));
            const i = b.querySelector('i');
            if (i) i.className = on ? 'fa-solid fa-heart' : 'fa-regular fa-heart';
        });
    }

    const stars = r => {
        const f = Math.max(0, Math.min(5, Math.round(Number(r) || 0)));
        return '<span class="stars" aria-hidden="true">' + '\u2605'.repeat(f) + '\u2606'.repeat(5 - f) + '</span>';
    };

    /* ---------- Header, Mobile Nav & Account Menu ---------- */
    function closeAllOverlays() {
        $('#mobile-nav')?.classList.remove('open');
        $('#search-overlay')?.classList.remove('open');
        $('#quick-modal')?.classList.remove('open');
        $('#account-menu')?.classList.remove('open');
        document.body.style.overflow = '';
    }

    function initHeader() {
        const mnav = $('#mobile-nav');
        const setNav = open => {
            if (!mnav) return;
            mnav.classList.toggle('open', open);
            mnav.setAttribute('aria-hidden', String(!open));
            document.body.style.overflow = open ? 'hidden' : '';
        };
        $('#menu-open')?.addEventListener('click', () => setNav(true));
        $$('[data-close-nav]').forEach(e => e.addEventListener('click', () => setNav(false)));

        // Account button toggle (works on both click and hover)
        const accMenu = $('#account-menu'), accBtn = $('#account-btn');
        if (accMenu && accBtn) {
            accBtn.addEventListener('click', e => {
                e.stopPropagation();
                const open = !accMenu.classList.contains('open');
                accMenu.classList.toggle('open', open);
                accBtn.setAttribute('aria-expanded', String(open));
            });
            document.addEventListener('click', e => {
                if (!accMenu.contains(e.target)) {
                    accMenu.classList.remove('open');
                    accBtn.setAttribute('aria-expanded', 'false');
                }
            });
        }

        // Announcement rotator
        const track = $('.announce-track');
        if (track && track.children.length > 1) {
            let i = 0; const n = track.children.length;
            setInterval(() => { i = (i + 1) % n; track.style.transform = `translateY(-${i * 38}px)`; }, 4200);
        }
    }

    /* ---------- Search Overlay ---------- */
    function initSearch() {
        const ov = $('#search-overlay');
        if (!ov) return;
        const input = $('#search-input'), out = $('#search-results'), sug = $('#search-suggest');
        const open = () => {
            ov.classList.add('open');
            ov.setAttribute('aria-hidden', 'false');
            document.body.style.overflow = 'hidden';
            render(input.value || '');
            setTimeout(() => input.focus(), 120);
        };
        const close = () => {
            ov.classList.remove('open');
            ov.setAttribute('aria-hidden', 'true');
            document.body.style.overflow = '';
        };
        $$('[data-open-search]').forEach(b => b.addEventListener('click', open));
        $('#search-close')?.addEventListener('click', close);
        ov.addEventListener('click', e => { if (e.target === ov) close(); });

        document.addEventListener('keydown', e => {
            if (e.key === 'Escape') closeAllOverlays();
            if ((e.key === '/' || (e.key === 'k' && (e.ctrlKey || e.metaKey))) && !/INPUT|TEXTAREA|SELECT/.test(document.activeElement?.tagName || '')) {
                e.preventDefault();
                open();
            }
        });

        function render(q) {
            q = q.trim().toLowerCase();
            sug.style.display = q ? 'none' : 'flex';
            if (!q) { out.innerHTML = ''; return; }
            const hits = catalog.filter(p => (p.name + ' ' + p.category + ' ' + p.brand + ' ' + (p.tags || []).join(' ')).toLowerCase().includes(q)).slice(0, 8);
            out.innerHTML = hits.length
                ? hits.map(p => `<a class="search-hit" href="/details/${p.id}"><img src="${img(p.images[0], 160)}" alt=""><span><b>${esc(p.name)}</b><small>${esc(p.category)} \u00b7 ${money(p.price)}</small></span></a>`).join('')
                : `<div class="empty" style="grid-column:1/-1;padding:32px 0"><i class="fa-regular fa-face-frown"></i><h3>No results for \u201c${esc(q)}\u201d</h3><p>Try a different keyword, or browse the full collection.</p><a class="btn btn-outline" href="/Products">Browse all</a></div>`;
        }
        input.addEventListener('input', () => render(input.value));
        $('#search-form')?.addEventListener('submit', e => {
            e.preventDefault();
            const q = input.value.trim();
            location.href = q ? '/Products?q=' + encodeURIComponent(q) : '/Products';
        });
        $$('.chip[data-q]', sug).forEach(c => c.addEventListener('click', () => {
            input.value = c.dataset.q;
            render(c.dataset.q);
            input.focus();
        }));
    }

    /* ---------- Quick View Modal ---------- */
    let qvState = {};
    function closeQuick() {
        const m = $('#quick-modal');
        if (m) { m.classList.remove('open'); m.setAttribute('aria-hidden', 'true'); document.body.style.overflow = ''; }
    }
    function openQuick(id) {
        const p = byId(id), m = $('#quick-modal');
        if (!p || !m) return;
        const defaultSize = p.sizes && p.sizes.length ? (p.sizes.includes('M') ? 'M' : p.sizes[0]) : null;
        const defaultColor = p.colors && p.colors.length ? p.colors[0].split('|')[0] : null;
        qvState = { id: p.id, size: defaultSize, color: defaultColor, qty: 1 };
        const off = p.oldPrice ? Math.round((1 - p.price / p.oldPrice) * 100) : 0;
        $('#qv-content').innerHTML = `
          <div class="qv-img"><img src="${img(p.images[0], 800)}" alt="${esc(p.name)}"></div>
          <div class="qv-body">
            <span class="eyebrow">${esc(p.brand)} \u00b7 ${esc(p.category)}</span>
            <h3>${esc(p.name)}</h3>
            <div class="pdp-price"><span class="now">${money(p.price)}</span>${p.oldPrice ? `<s>${money(p.oldPrice)}</s><span class="save">Save ${off}%</span>` : ''}</div>
            <div class="rating">${stars(p.rating)} ${Number(p.rating).toFixed(1)} (${p.reviews} reviews)</div>
            <p style="color:var(--ink-2)">${esc(p.description)}</p>
            ${p.colors.length ? `<div><div class="opt-label">Colour <span id="qv-color-lbl">${esc(defaultColor)}</span></div><div class="color-opts">${p.colors.map((c, i) => { const [n, h] = c.split('|'); return `<button type="button" class="color-opt ${i === 0 ? 'active' : ''}" data-qv-color="${esc(n)}" style="background:${h}" aria-label="${esc(n)}" title="${esc(n)}"></button>`; }).join('')}</div></div>` : ''}
            ${p.sizes.length ? `<div><div class="opt-label">Size</div><div class="size-opts">${p.sizes.map(s => `<button type="button" class="size-opt ${s === defaultSize ? 'active' : ''}" data-qv-size="${esc(s)}">${esc(s)}</button>`).join('')}</div></div>` : ''}
            <div class="buy-row">
              <button type="button" class="btn" id="qv-add" ${p.stock === 0 ? 'disabled' : ''}>${p.stock === 0 ? 'Sold out' : 'Add to bag'}</button>
              <a class="btn btn-outline" href="/details/${p.id}">Full details</a>
            </div>
          </div>`;
        m.classList.add('open');
        m.setAttribute('aria-hidden', 'false');
        document.body.style.overflow = 'hidden';
        $('#qv-add')?.addEventListener('click', () => {
            if (Cart.add(p.id, { size: qvState.size, color: qvState.color })) {
                toast(`${p.name} added to your bag`, { href: '/cart', linkText: 'View bag' });
                closeQuick();
            }
        });
    }

    /* ---------- Global Delegated Clicks & Forms ---------- */
    function initDelegates() {
        document.addEventListener('click', e => {
            const w = e.target.closest('[data-wish]');
            if (w) {
                e.preventDefault();
                const on = Wish.toggle(w.dataset.wish);
                toast(on ? 'Saved to your wishlist' : 'Removed from wishlist', on ? { href: '/Products?wishlist=1', linkText: 'Wishlist' } : {});
                return;
            }
            const q = e.target.closest('[data-quick]');
            if (q) { e.preventDefault(); openQuick(q.dataset.quick); return; }

            if (e.target.closest('[data-close-modal]')) { closeQuick(); return; }

            const col = e.target.closest('[data-qv-color]');
            if (col) {
                qvState.color = col.dataset.qvColor;
                const lbl = $('#qv-color-lbl'); if (lbl) lbl.textContent = qvState.color;
                $$('[data-qv-color]').forEach(b => b.classList.toggle('active', b === col));
                return;
            }
            const sz = e.target.closest('[data-qv-size]');
            if (sz) {
                qvState.size = sz.dataset.qvSize;
                $$('[data-qv-size]').forEach(b => b.classList.toggle('active', b === sz));
                return;
            }
            const quickAdd = e.target.closest('[data-add]');
            if (quickAdd) {
                e.preventDefault();
                const p = byId(quickAdd.dataset.add);
                if (!p || p.stock <= 0) { toast('This item is currently sold out.', { type: 'error' }); return; }
                const defaultSize = p.sizes && p.sizes.length ? (p.sizes.includes('M') ? 'M' : p.sizes[0]) : null;
                const defaultColor = p.colors && p.colors.length ? p.colors[0].split('|')[0] : null;
                Cart.add(p.id, { size: defaultSize, color: defaultColor, qty: 1 });
                toast(`${p.name}${defaultSize ? ' (' + defaultSize + ')' : ''} added to bag`, { href: '/cart', linkText: 'View bag' });
                return;
            }
            const claim = e.target.closest('[data-claim-code]');
            if (claim) {
                e.preventDefault();
                const code = claim.dataset.claimCode;
                Promo.apply(code);
                if (navigator.clipboard && navigator.clipboard.writeText) {
                    navigator.clipboard.writeText(code).catch(() => {});
                }
                toast(`Code ${code} applied \u2014 10% off at checkout!`, { href: '/Products', linkText: 'Shop now' });
            }
        });

        $$('[data-newsletter]').forEach(f => f.addEventListener('submit', e => {
            e.preventDefault();
            const i = f.querySelector('input');
            if (!i || !i.value || !/^\S+@\S+\.\S+$/.test(i.value.trim())) {
                toast('Please enter a valid email address.', { type: 'error' });
                i?.focus();
                return;
            }
            Promo.apply('VESTITI10');
            toast('Welcome to Vestiti \u2014 code VESTITI10 (10% off) is now active!');
            f.reset();
        }));
    }

    /* ---------- Scroll Reveal (with safe fallback) ---------- */
    function initReveal() {
        const els = $$('.reveal');
        if (!els.length) return;
        if (!('IntersectionObserver' in window)) { els.forEach(e => e.classList.add('in')); return; }
        const io = new IntersectionObserver(es => es.forEach(en => {
            if (en.isIntersecting) { en.target.classList.add('in'); io.unobserve(en.target); }
        }), { threshold: 0.05, rootMargin: '0px 0px -20px 0px' });
        els.forEach(e => io.observe(e));
        // Guarantee nothing ever stays hidden if layout shifts or tabs toggle
        setTimeout(() => els.forEach(e => e.classList.add('in')), 900);
    }

    window.Vestiti = { Cart, Wish, Promo, totals, toast, money, img, byId, stars, esc, updateBadges, FREE_SHIP, catalog };

    document.addEventListener('DOMContentLoaded', () => {
        initHeader();
        initSearch();
        initDelegates();
        initReveal();
        updateBadges();
        window.addEventListener('storage', updateBadges);
    });
})();
