using RPInventory.Models;

namespace RPInventory.Data;

public static class DbInitializer
{
    public static void Initialize(InventoryContext context)
    {
       if (context.Brands.Any())
        {
            return;  //DB initialized with information
        }

        var brands = new Brand[]
        {
           new Brand {Name = "Rino"},
           new Brand {Name = "Reni"},
           new Brand {Name = "Rocco"},
           new Brand {Name = "Azuri"},
           new Brand {Name = "Bazi"},
           new Brand {Name = "Asis"}
        };

        context.Brands.AddRange(brands);
        context.SaveChanges();

        var departments = new Department[]
        {
            new Department { Name = "General Admin", Description = "General Administration", DateCreation = DateTime.Now},
            new Department { Name = "Human Resources", Description = "Human Resources", DateCreation = DateTime.Now},
            new Department { Name = "Material Resources", Description = "Material Resources", DateCreation = DateTime.Now},
            new Department { Name = "Information Technology", Description = "Information Technology", DateCreation = DateTime.Now},
            new Department { Name = "Sports", Description = "Sports", DateCreation = DateTime.Now}
        };

        context.Departments.AddRange(departments);
        context.SaveChanges();

        var products = new Product[]
        {
            new Product {
                Name = "Secretary Chair", 
                Description = "Skinned Chair imitation", 
                BrandId = context.Brands.First(u => u.Name == "Rino").Id, 
                Price = 2500M
            },
            new Product {
                Name = "Gerencial Chair",
                Description = "Black Chair template",
                BrandId = context.Brands.First(u => u.Name == "Azuri").Id,
                Price = 2500M
            },
            new Product {
                Name = "Coffee Machine",
                Description = "Industrial coffee machine 50 cups",
                BrandId = context.Brands.First(u => u.Name == "Asis").Id,
                Price = 6500M
            },
            new Product {
                Name = "Latop",
                Description = "Lenovo Laptop intel core i7",
                BrandId = context.Brands.First(u => u.Name == "Rino").Id,
                Price = 2500M
            },
             new Product {
                Name = "Projector",
                Description = "Wireless projector",
                BrandId = context.Brands.First(u => u.Name == "Reni").Id,
                Price = 2500M
            },
        };

        context.Products.AddRange(products);
        context.SaveChanges();

    }
}
