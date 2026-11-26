# Vestiti. — Modern Fashion E-Commerce Platform

**Vestiti** (*Italian for clothing*) is a responsive, editorial-grade fashion e-commerce web application built with **ASP.NET Core 8 Razor Pages**, **Entity Framework Core**, **SQLite**, and custom **Vanilla CSS & JavaScript**.

---

## Key Features

- **Editorial Storefront & Design System** — Minimal luxury UI with custom typography (`Fraunces` & `Inter`), fluid responsive layouts across Desktop, Tablet, and Mobile, and zero framework bloat.
- **Interactive Shop & Filtering** — Filter in real time by category, price range, brand, star rating, stock availability, and sale status, with multi-criteria sorting, active filter chips, and progressive pagination.
- **Live Instant Search & Quick View** — Full-screen instant search overlay (`/` or `Ctrl+K`) with popular suggestions, plus a Quick View modal with color/size selection.
- **Rich Product Details (PDP)** — Multi-angle image gallery with click-to-zoom, variant selectors (color & size), stock indicators, specification accordions, verified customer reviews, and related recommendations.
- **Persistent Bag & Promo Engine** — Client-side shopping bag and wishlist with live header badges, free-shipping progress meter, and promo codes (`VESTITI10`, `WELCOME15`).
- **Streamlined Checkout** — Multi-step contact, shipping, delivery method (`Standard`, `Express`, `Store Pickup`), and payment flow with real-time validation and order confirmation.
- **Authentication & Admin Dashboard** — Session-based customer registration/sign-in and an Admin CRUD panel for managing users, categories, and products.

---

## Tech Stack

- **Backend:** C#, ASP.NET Core 8.0 (Razor Pages), Session State
- **Database:** SQLite + Entity Framework Core 8
- **Frontend:** Semantic HTML5, Custom CSS Grid/Flexbox Design System, Vanilla ES6+ JavaScript

---

## Getting Started

```bash
cd "StyleSphere/StyleSphere"
dotnet restore
dotnet run --launch-profile http
```

Then open `http://localhost:5245` in your browser.

### Default Admin Credentials (Development)
- **Email:** `admin@stylesphere.com`
- **Password:** `admin123`
