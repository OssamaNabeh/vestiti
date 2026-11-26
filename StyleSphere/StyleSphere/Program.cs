using Microsoft.EntityFrameworkCore;
using StyleSphere.Data;
using StyleSphere.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add DbContext - Using SQLite for easy development (no SQL Server needed)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=StyleSphere.db";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();
        
        // Create database if it doesn't exist
        context.Database.EnsureCreated();
        logger.LogInformation("Database created/verified successfully.");
        
        // Seed initial data if database is empty
        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category { Name = "Men", Description = "Clothing and accessories for men" },
                new Category { Name = "Women", Description = "Clothing and accessories for women" },
                new Category { Name = "Shoes", Description = "Footwear for all occasions" },
                new Category { Name = "Accessories", Description = "Fashion accessories and add-ons" }
            );
            context.SaveChanges();
            logger.LogInformation("Initial categories seeded.");
        }

        // Create admin user if doesn't exist
        if (!context.Users.Any(u => u.Email == "admin@stylesphere.com"))
        {
            context.Users.Add(new User
            {
                Name = "Admin",
                Email = "admin@stylesphere.com",
                Password = "admin123", // Change this in production!
                Role = "Admin"
            });
            context.SaveChanges();
            logger.LogInformation("Admin user created.");
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating/seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
