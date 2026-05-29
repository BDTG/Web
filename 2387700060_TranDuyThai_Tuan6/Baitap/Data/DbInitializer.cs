using Baitap.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Baitap.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var roles = new[] { "Admin", "Member" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        var adminEmail = "admin@shop.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser is null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "Administrator",
                Address = "123 Admin St",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(adminUser, "Admin@123");
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }

        var memberEmail = "member@shop.com";
        var memberUser = await userManager.FindByEmailAsync(memberEmail);
        if (memberUser is null)
        {
            memberUser = new ApplicationUser
            {
                UserName = memberEmail,
                Email = memberEmail,
                FullName = "Sample Member",
                Address = "456 Member Ave",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(memberUser, "Member@123");
            await userManager.AddToRoleAsync(memberUser, "Member");
        }

        if (await context.Categories.AnyAsync()) return;

        var categories = new[]
        {
            new Category { Name = "\u0110i\u1ec7n tho\u1ea1i", Description = "\u0110i\u1ec7n tho\u1ea1i th\u00f4ng minh, smartphone c\u00e1c lo\u1ea1i" },
            new Category { Name = "Laptop", Description = "M\u00e1y t\u00ednh x\u00e1ch tay c\u00e1c h\u00e3ng" },
            new Category { Name = "Ph\u1ee5 ki\u1ec7n", Description = "Tai nghe, s\u1ea1c, c\u00e1p, \u1ed1p l\u01b0ng..." },
            new Category { Name = "Tablet", Description = "M\u00e1y t\u00ednh b\u1ea3ng iPad, Samsung, Xiaomi..." }
        };

        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();

        var products = new[]
        {
            new Product
            {
                Name = "iPhone 15 Pro Max",
                Price = 34990000,
                Description = "Apple iPhone 15 Pro Max 256GB - H\u00e0ng ch\u00ednh h\u00e3ng VN/A. Chip A17 Pro, Camera 48MP.",
                CategoryId = categories[0].Id,
                CreatedAt = DateTime.UtcNow,
                Images = new List<ProductImage>
                {
                    new() { Url = "/images/iphone15.webp", IsPrimary = true }
                }
            },
            new Product
            {
                Name = "Samsung Galaxy S24 Ultra",
                Price = 33990000,
                Description = "Samsung Galaxy S24 Ultra 256GB ch\u00ednh h\u00e3ng. Snapdragon 8 Gen 3, S Pen, Camera 200MP.",
                CategoryId = categories[0].Id,
                CreatedAt = DateTime.UtcNow,
                Images = new List<ProductImage>
                {
                    new() { Url = "/images/samsung-s24.jpg", IsPrimary = true }
                }
            },
            new Product
            {
                Name = "MacBook Pro 14 M4 Pro",
                Price = 49990000,
                Description = "Apple MacBook Pro 14 inch M4 Pro \u2013 24GB RAM \u2013 512GB SSD.",
                CategoryId = categories[1].Id,
                CreatedAt = DateTime.UtcNow,
                Images = new List<ProductImage>
                {
                    new() { Url = "/images/macbook.png", IsPrimary = true }
                }
            },
            new Product
            {
                Name = "Dell XPS 15",
                Price = 42990000,
                Description = "Dell XPS 15 9530 - Intel Core Ultra 7, 16GB RAM, 512GB SSD, OLED 3.5K.",
                CategoryId = categories[1].Id,
                CreatedAt = DateTime.UtcNow,
                Images = new List<ProductImage>
                {
                    new() { Url = "/images/dellxps.webp", IsPrimary = true }
                }
            },
            new Product
            {
                Name = "AirPods Pro 2",
                Price = 5890000,
                Description = "Apple AirPods Pro 2nd Gen v\u1edbi chip H2, ch\u1ed1ng \u1ed3n ch\u1ee7 \u0111\u1ed9ng 2x, USB-C.",
                CategoryId = categories[2].Id,
                CreatedAt = DateTime.UtcNow,
                Images = new List<ProductImage>
                {
                    new() { Url = "/images/airpods.jpg", IsPrimary = true }
                }
            },
            new Product
            {
                Name = "iPad Pro M4 11 inch",
                Price = 29990000,
                Description = "Apple iPad Pro M4 11 inch \u2013 256GB WiFi, m\u00e0n h\u00ecnh Ultra Retina XDR.",
                CategoryId = categories[3].Id,
                CreatedAt = DateTime.UtcNow,
                Images = new List<ProductImage>
                {
                    new() { Url = "/images/ipad-pro-m4.webp", IsPrimary = true }
                }
            }
        };

        context.Products.AddRange(products);
        await context.SaveChangesAsync();
    }
}
