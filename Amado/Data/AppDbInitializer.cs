using Amado.Data.Enums;
using Amado.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Amado.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                context.Database.EnsureCreated();
                if (!context.Item.Any())
                {
                    context.Item.AddRange(new List<Item>()
                    {
                        new Item()
                        {
                            Name = "Modern Chair",
                            Price = 180,
                            Rate = 5,
                            Category = Category.Chairs,
                            Brand = Brand.Amado,
                            Color = Color.White,
                            Quantity = 50,
                        },
                         new Item()
                        {
                            Name = "Plant Pot",
                            Price = 140,
                            Rate = 4,
                            Category = Category.Accessories,
                            Brand = Brand.Amado,
                            Color = Color.White,
                            Quantity = 50,
                        },
                         new Item()
                        {
                            Name = "Modern Rocking Chair",
                            Price = 380,
                            Rate = 5,
                            Category = Category.Chairs,
                            Brand = Brand.FurnitureInc,
                            Color = Color.White,
                            Quantity = 50,
                        },
                         new Item()
                        {
                            Name = "Minimalistic Plant Pot",
                            Price = 190,
                            Rate = 5,
                            Category = Category.Accessories,
                            Brand = Brand.Artdeco,
                            Color = Color.White,
                            Quantity = 50,
                        },
                         new Item()
                        {
                            Name = "Small Table",
                            Price = 450,
                            Rate = 5,
                            Category = Category.Furniture,
                            Brand = Brand.Ikea,
                            Color = Color.White,
                            Quantity = 50,
                        },
                         new Item()
                        {
                            Name = "Home Deco",
                            Price = 270,
                            Rate = 3,
                            Category = Category.HomeDeco,
                            Brand = Brand.TheFactory,
                            Color = Color.White,
                            Quantity = 50,
                        }, 
                        new Item()
                        {
                            Name = "Modern Chair+",
                            Price = 370,
                            Rate = 4,
                            Category = Category.Chairs,
                            Brand = Brand.Amado,
                            Color = Color.White,
                            Quantity = 50,
                        },
                        new Item()
                        {
                            Name = "Night Stand",
                            Price = 110,
                            Rate = 5,
                            Category = Category.Accessories,
                            Brand = Brand.Amado,
                            Color = Color.White,
                            Quantity = 50,
                        },
                        new Item()
                        {
                            Name = "Metalic Chair",
                            Price = 250,
                            Rate = 5,
                            Category = Category.Chairs,
                            Brand = Brand.Ikea,
                            Color = Color.White,
                            Quantity = 50,
                        },
                    });
                    context.SaveChanges();
                }
                if (!context.Image.Any())
                {
                    context.Image.AddRange(new List<Models.Image>()
                    {
                        new Models.Image()
                        {
                            ItemID = 1,
                            ImageURL = "/assets/1.jpg.webp",
                        },
                        new Models.Image()
                        {
                            ItemID = 2,
                            ImageURL = "/assets/5.jpg.webp",
                        },
                        new Models.Image()
                        {
                            ItemID = 3,
                            ImageURL = "/assets/8.jpg.webp",
                        },
                        new Models.Image()
                        {
                            ItemID = 4,
                            ImageURL = "/assets/2.jpg.webp",
                        },
                        new Models.Image()
                        {
                            ItemID = 5,
                            ImageURL = "/assets/6.jpg.webp",
                        },
                        new Models.Image()
                        {
                            ItemID = 6,
                            ImageURL = "/assets/9.jpg.webp",
                        },
                        new Models.Image()
                        {
                            ItemID = 7,
                            ImageURL = "/assets/3.jpg.webp",
                        },
                        new Models.Image()
                        {
                            ItemID = 8,
                            ImageURL = "/assets/4.jpg.webp",
                        },
                        new Models.Image()
                        {
                            ItemID = 9,
                            ImageURL = "/assets/7.jpg.webp",
                        },
                    });
                    context.SaveChanges();
                }
                if (!context.Cart.Any())
                {
                    context.Cart.AddRange(new List<Cart>()
                    {
                        new Cart()
                        {
                            TotalPrice = 0,
                        },
                        new Cart()
                        {
                            TotalPrice = 0,
                        },
                    });

                    context.SaveChanges();
                }
                if (!context.User.Any())
                {
                    context.User.AddRange(new List<User>()
                    {
                        new User()
                        {
                            CartID = 1,
                            FirstName = "test",
                            SecondName = "user",
                            CompanyName = "Google",
                            Email = "test@google.com",
                            Country = Country.Egypt,
                            Address = "20 test street",
                            Town = "test town",
                            ZIPCode = "123",
                            PhoneNo = "0123",
                            Password = "testpass",
                            IsAdmin = false,
                        },
                        new User()
                        {
                            CartID = 2,
                            FirstName = "test",
                            SecondName = "admin",
                            Email = "admin@amado.com",
                            Password = "adminpass",
                            IsAdmin = true,
                        },
                    });

                    context.SaveChanges();
                }

            }
        }
    }
}
