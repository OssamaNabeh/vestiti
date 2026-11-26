namespace StyleSphere.Data;

public record CatalogProduct(
    int Id, string Name, string Category, string Brand, decimal Price, decimal? OldPrice,
    double Rating, int Reviews, int Stock, string[] Images, string[] Colors, string[] Sizes,
    string Description, string[] Details, string[] Tags, int DaysOld)
{
    public string Image => Images[0];
    public int DiscountPercent => OldPrice is > 0 ? (int)Math.Round((1 - Price / OldPrice.Value) * 100) : 0;
}

/// <summary>Single source of truth for the Vestiti storefront (shared by every page).</summary>
public static class Catalog
{
    public static string Img(string id, int w) =>
        $"https://images.unsplash.com/photo-{id}?auto=format&fit=crop&w={w}&q=80";

    static readonly string[] Apparel = { "XS", "S", "M", "L", "XL" };
    static readonly string[] Shoe = { "US 7", "US 8", "US 9", "US 10", "US 11", "US 12" };
    static readonly string[] None = Array.Empty<string>();

    static string[] T(params string[] t) => t;

    public static readonly IReadOnlyList<CatalogProduct> All = new List<CatalogProduct>
    {
        // ---------- Women ----------
        new(1, "Sole Wrap Dress", "Women", "Vestiti Atelier", 189m, 249m, 4.8, 214, 14,
            T("1572804013309-59a88b7e92f1", "1595777457583-95e059d581b8"), T("Poppy|#c8382e", "Ink|#1b1b1b"), Apparel,
            "A fluid wrap silhouette cut from a crisp viscose blend. Defined waist, gentle sleeve, and a hem that moves.",
            T("Fabric|Viscose / linen blend", "Fit|Regular, true to size", "Length|Midi, 118 cm", "Care|Machine wash cold", "Origin|Made in Portugal"), T("best", "trending"), 40),
        new(2, "Velluto Off-Shoulder Gown", "Women", "Vestiti Atelier", 329m, null, 4.9, 87, 6,
            T("1566174053879-31528523f8ae"), T("Plum|#5a1f45"), Apparel,
            "Evening in a single piece. Structured bodice, draped neckline, and a column skirt in a heavy satin-back crepe.",
            T("Fabric|Satin-back crepe", "Fit|Slim through the bodice", "Length|Floor", "Care|Dry clean only", "Origin|Made in Italy"), T("new"), 9),
        new(3, "Cammello Belted Coat", "Women", "Maison Nord", 389m, 460m, 4.9, 302, 11,
            T("1539533018447-63fcce2678e3", "1539109136881-3be0616acf4b"), T("Camel|#b07a43", "Charcoal|#3a3a3a"), Apparel,
            "The coat you keep for ten winters. Double-faced wool, a clean wrap front, and a belt that cinches without bulk.",
            T("Fabric|80% wool, 20% cashmere", "Fit|Relaxed", "Length|Knee, 104 cm", "Care|Dry clean only", "Origin|Made in Italy"), T("best", "trending"), 70),
        new(4, "Giardino Floral Maxi", "Women", "Vestiti Atelier", 149m, null, 4.6, 129, 22,
            T("1496747611176-843222e1e57c"), T("Garden|#d9a7a0"), Apparel,
            "Lightweight cotton voile with a hand-drawn floral print. Tiered skirt, adjustable tie waist, pockets.",
            T("Fabric|100% organic cotton", "Fit|Relaxed", "Length|Maxi, 138 cm", "Care|Machine wash cold", "Origin|Made in Portugal"), T("new", "trending"), 14),
        new(5, "Denim Shirt Dress", "Women", "Studio Blu", 119m, 159m, 4.5, 176, 30,
            T("1617137968427-85924c800a22"), T("Washed Blue|#7c9cc2"), Apparel,
            "A pressed-pearl-button classic in soft, mid-weight chambray. Easy at the desk, easier on weekends.",
            T("Fabric|100% cotton chambray", "Fit|Regular", "Length|Knee, 96 cm", "Care|Machine wash cold", "Origin|Made in Turkey"), T("best"), 95),
        new(6, "Frangia Knit Poncho", "Women", "Maison Nord", 139m, null, 4.4, 58, 18,
            T("1445205170230-053b83016050"), T("Cream|#efe6d2"), T("One size"),
            "Hand-finished fringe on an open-knit merino blend. Layer it over anything.",
            T("Fabric|Merino / cotton blend", "Fit|One size", "Care|Hand wash", "Origin|Made in Peru"), T(), 120),
        new(7, "Rigato Wide-Leg Trousers", "Women", "Studio Blu", 129m, null, 4.7, 96, 25,
            T("1509631179647-0177331693ae"), T("Ink Stripe|#222222"), Apparel,
            "High-rise, fluid, and pleated at the front. A pinstripe that reads sharp, not corporate.",
            T("Fabric|Viscose / polyester twill", "Fit|Wide leg", "Rise|High", "Care|Machine wash cold", "Origin|Made in Portugal"), T("new"), 5),
        new(8, "Righe Striped Blouse", "Women", "Studio Blu", 89m, 119m, 4.3, 71, 0,
            T("1525507119028-ed4c629a60a3"), T("Stripe|#e8e3d8"), Apparel,
            "Relaxed boxy cut in a washed linen-cotton. Soft hand-feel from the very first wear.",
            T("Fabric|Linen / cotton", "Fit|Relaxed boxy", "Care|Machine wash cold", "Origin|Made in India"), T(), 150),

        // ---------- Men ----------
        new(9, "Blu Three-Piece Suit", "Men", "Sartoria Nove", 599m, 720m, 4.9, 143, 9,
            T("1594938298603-c8148c4dae35", "1507679799987-c73779587ccf"), T("Royal Check|#2f4a8a", "Navy|#14213d"), T("46", "48", "50", "52", "54"),
            "Half-canvassed jacket, matching waistcoat, flat-front trousers. Cut from a wool-blend check with real structure.",
            T("Fabric|Super 110s wool blend", "Fit|Slim", "Lining|Bemberg", "Care|Dry clean only", "Origin|Made in Italy"), T("best", "trending"), 60),
        new(10, "Nero Leather Biker Jacket", "Men", "Maison Nord", 449m, null, 4.8, 211, 7,
            T("1551028719-00167b16eac5", "1614252369475-531eba835eb1"), T("Black|#111111", "Tan|#9a6234"), Apparel,
            "Full-grain lambskin, brushed hardware, and a cut that breaks in rather than wears out.",
            T("Fabric|Full-grain lambskin", "Fit|Slim", "Lining|Cupro", "Care|Leather specialist", "Origin|Made in Italy"), T("best", "trending"), 100),
        new(11, "Terra Bomber Jacket", "Men", "Studio Blu", 169m, 210m, 4.6, 118, 20,
            T("1591047139829-d91aecb6caea"), T("Terracotta|#b8623e"), Apparel,
            "Lightweight, water-repellent shell with ribbed trims and a quiet matte finish.",
            T("Fabric|Recycled nylon", "Fit|Regular", "Care|Machine wash cold", "Origin|Made in Vietnam"), T("new"), 12),
        new(12, "Essential Crew Tee", "Men", "Vestiti Basics", 39m, null, 4.7, 842, 120,
            T("1521572163474-6864f9cf17ab", "1622445275463-afa2ab738c34"), T("White|#f4f4f2", "Black|#141414", "Sage|#8a9a86"), Apparel,
            "The everyday tee, perfected. 220gsm Supima cotton that holds its shape wash after wash.",
            T("Fabric|100% Supima cotton, 220gsm", "Fit|Regular", "Care|Machine wash cold", "Origin|Made in Peru"), T("best"), 200),
        new(13, "Original Graphic Tee", "Men", "Vestiti Basics", 49m, 65m, 4.4, 97, 45,
            T("1576566588028-4147f3842f27"), T("Stone|#d9d1c0"), Apparel,
            "Screen-printed in water-based inks on a washed, garment-dyed cotton jersey.",
            T("Fabric|100% cotton jersey", "Fit|Boxy", "Care|Machine wash cold", "Origin|Made in Portugal"), T(), 80),
        new(14, "Selvedge Slim Denim", "Men", "Studio Blu", 139m, null, 4.8, 387, 33,
            T("1542272604-787c3835535d"), T("Raw Indigo|#1d2535"), T("28", "30", "32", "34", "36"),
            "14oz Japanese selvedge denim, slim through the thigh, with a clean taper. Fades beautifully.",
            T("Fabric|14oz selvedge denim", "Fit|Slim taper", "Care|Wash rarely, cold", "Origin|Made in Japan"), T("best", "trending"), 160),
        new(15, "Heavyweight Hoodie", "Men", "Vestiti Basics", 109m, null, 4.9, 456, 40,
            T("1556821840-3a63f95609a7"), T("Heather|#9aa0a2", "Black|#141414"), Apparel,
            "450gsm brushed-back fleece with a double-lined hood and a clean kangaroo pocket.",
            T("Fabric|100% organic cotton, 450gsm", "Fit|Relaxed", "Care|Machine wash cold", "Origin|Made in Portugal"), T("new", "trending"), 7),
        new(16, "Notte Black Tee", "Men", "Vestiti Basics", 45m, null, 4.5, 133, 60,
            T("1583743814966-8936f5b7be1a"), T("Black|#141414"), Apparel,
            "Garment-dyed black with a soft vintage finish and discreet chest lettering.",
            T("Fabric|100% cotton", "Fit|Regular", "Care|Machine wash cold", "Origin|Made in Portugal"), T(), 130),

        // ---------- Shoes ----------
        new(17, "Aero Runner", "Shoes", "Corsa", 139m, 165m, 4.7, 521, 28,
            T("1542291026-7eec264c27ff"), T("Crimson|#a21a24"), Shoe,
            "Engineered knit upper, responsive foam midsole, and a locked-in heel for daily miles.",
            T("Upper|Engineered knit", "Midsole|Responsive foam", "Drop|8 mm", "Weight|255 g"), T("best", "trending"), 55),
        new(18, "Court Low", "Shoes", "Corsa", 119m, null, 4.8, 689, 52,
            T("1608231387042-66d1773070a5"), T("White|#f6f6f4"), Shoe,
            "Minimal court sneaker in smooth full-grain leather with a perforated toe and a gum-free sole.",
            T("Upper|Full-grain leather", "Sole|Cupsole rubber", "Lining|Leather"), T("best"), 180),
        new(19, "Cuoio Low Sneaker", "Shoes", "Corsa", 149m, null, 4.6, 164, 19,
            T("1549298916-b41d501d3772"), T("Tan|#a9763b"), Shoe,
            "Waxed nubuck in warm tan with contrast ripstop panels and a cushioned insole.",
            T("Upper|Waxed nubuck", "Sole|Vulcanised rubber", "Lining|Textile"), T("new"), 10),
        new(20, "Pastello Court", "Shoes", "Corsa", 159m, 189m, 4.5, 102, 15,
            T("1595950653106-6c9ebd614d3a"), T("Pastel|#d8c9ea"), Shoe,
            "Soft pastel overlays on a chunky layered sole. A playful, still-polished statement.",
            T("Upper|Leather / synthetic", "Sole|Rubber cupsole"), T("trending"), 30),
        new(21, "Volt Trainer", "Shoes", "Corsa", 129m, null, 4.6, 249, 24,
            T("1606107557195-0e29a4b5b4aa"), T("Volt|#bfef2f"), Shoe,
            "Gym-ready trainer with a stable heel, flexible forefoot, and a breathable mesh upper.",
            T("Upper|Breathable mesh", "Midsole|Dual-density foam", "Drop|6 mm"), T(), 110),
        new(22, "Ember Air Sneaker", "Shoes", "Corsa", 179m, 215m, 4.8, 310, 12,
            T("1600185365926-3a2ce3cdb9eb"), T("Cloud / Ember|#f1ece4"), Shoe,
            "Visible air unit, suede overlays, and a pop of ember orange. Lightweight and loud, in a good way.",
            T("Upper|Suede / mesh", "Midsole|Air-cushioned foam"), T("best"), 85),

        // ---------- Accessories ----------
        new(23, "Matelassé Camera Bag", "Accessories", "Maison Nord", 259m, null, 4.9, 175, 10,
            T("1548036328-c9fa89d128fa"), T("Black|#111111"), None,
            "Quilted calf leather with a signature clasp and a detachable chain strap. Sized for the essentials.",
            T("Material|Quilted calf leather", "Size|24 × 14 × 7 cm", "Closure|Magnetic clasp", "Strap|Detachable, 120 cm"), T("best", "trending"), 45),
        new(24, "Rosso Top-Handle Bag", "Accessories", "Maison Nord", 319m, 389m, 4.8, 94, 5,
            T("1584917865442-de89df76afd3"), T("Red|#b3201a"), None,
            "Structured silhouette in smooth leather with a polished turn-lock. Carry it by hand or on the shoulder.",
            T("Material|Smooth calf leather", "Size|26 × 20 × 11 cm", "Closure|Turn-lock", "Strap|Detachable"), T("new"), 8),
        new(25, "Wayfarer Sunglasses", "Accessories", "Lumen", 95m, null, 4.7, 408, 70,
            T("1572635196237-14b3f281503f"), T("Black|#111111"), None,
            "The icon, refined: UV400 lenses in a lightweight acetate frame with metal hinges.",
            T("Frame|Italian acetate", "Lens|UV400 polarised", "Width|145 mm"), T("best"), 220),
        new(26, "Chrono Steel Watch", "Accessories", "Lumen", 395m, 460m, 4.9, 233, 8,
            T("1523170335258-f5ed11844a49"), T("Steel / Blue|#27406b"), None,
            "Swiss-movement chronograph, sapphire crystal, and a brushed steel bracelet. 100 m water resistant.",
            T("Case|42 mm stainless steel", "Movement|Swiss quartz chronograph", "Crystal|Sapphire", "Water resistance|100 m"), T("trending"), 90),
        new(27, "Everyday Backpack", "Accessories", "Lumen", 89m, null, 4.6, 312, 36,
            T("1553062407-98eeb64c6a62"), T("Navy|#1c2740"), None,
            "A clean daily carry with a padded 15″ laptop sleeve, hidden zip pocket, and water-repellent shell.",
            T("Material|Recycled polyester", "Capacity|18 L", "Laptop|Up to 15\""), T("new"), 6),
        new(28, "Billfold Wallet", "Accessories", "Maison Nord", 79m, null, 4.8, 267, 48,
            T("1627123424574-724758594e93"), T("Cognac|#7b4a28"), None,
            "Vegetable-tanned leather that darkens with age. Six card slots and a full-length note sleeve.",
            T("Material|Vegetable-tanned leather", "Slots|6 cards", "Size|11 × 9 cm"), T(), 140),
    };

    public static CatalogProduct? Find(int id) => All.FirstOrDefault(p => p.Id == id);

    public static IEnumerable<CatalogProduct> Related(CatalogProduct p, int take = 4) =>
        All.Where(x => x.Id != p.Id).OrderByDescending(x => x.Category == p.Category).ThenByDescending(x => x.Rating).Take(take);

    public static IEnumerable<CatalogProduct> WithTag(string tag, int take) =>
        All.Where(p => p.Tags.Contains(tag)).Take(take);

    public static readonly (string Name, string Cover, string Blurb)[] Categories =
    {
        ("Women", "1566174053879-31528523f8ae", "Dresses, coats & tailoring"),
        ("Men", "1507679799987-c73779587ccf", "Suiting, denim & essentials"),
        ("Shoes", "1595950653106-6c9ebd614d3a", "Sneakers & trainers"),
        ("Accessories", "1548036328-c9fa89d128fa", "Bags, watches & eyewear"),
    };

    /// <summary>Compact JSON for client-side search, quick view and cart.</summary>
    public static string ToJson()
    {
        var opts = new System.Text.Json.JsonSerializerOptions { PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase };
        return System.Text.Json.JsonSerializer.Serialize(All.Select(p => new
        {
            id = p.Id, name = p.Name, category = p.Category, brand = p.Brand, price = p.Price, oldPrice = p.OldPrice,
            rating = p.Rating, reviews = p.Reviews, stock = p.Stock, images = p.Images, colors = p.Colors, sizes = p.Sizes,
            description = p.Description, tags = p.Tags
        }), opts).Replace("</", "<\\/");
    }
}
