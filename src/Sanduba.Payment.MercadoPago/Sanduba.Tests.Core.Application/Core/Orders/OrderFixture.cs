using Sanduba.Core.Domain.Orders;
using System;
using System.Collections.Generic;

namespace Sanduba.Test.Unit.Core.Orders
{
    internal static class OrderFixture
    {
        internal static Order CompleteOrder(Guid id)
        {
            return new Order(id)
            {
                ClientId = Guid.NewGuid(),
                Code = "123",
                Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Product = new Product(Guid.NewGuid())
                        {
                            Category = Category.Beverage,
                            Name = "Product 2",
                            Description = "Product 2",
                            UnitPrice = 3.59,
                        },
                        Amount = 3.59,
                        Code = 1,
                        Currency = Currency.BRL
                    },
                    new OrderItem
                    {
                        Product = new Product(Guid.NewGuid())
                        {
                            Category = Category.MainDish,
                            Name = "Product 1",
                            Description = "Product 1",
                            UnitPrice = 19.99
                        },
                        Amount = 19.99,
                        Code = 2,
                        Currency = Currency.BRL
                    },
                    new OrderItem
                    {
                        Product = new Product(Guid.NewGuid())
                        {
                            Category = Category.SideDish,
                            Name = "Product 3",
                            Description = "Product 3",
                            UnitPrice = 11.00,
                        },
                        Amount = 11.00,
                        Code = 3,
                        Currency = Currency.BRL
                    }
                }
            };
        }

        internal static Order MainDishOrder(Guid id, Guid productId)
        {
            return new Order(id)
            {
                ClientId = Guid.NewGuid(),
                Code = "123",
                Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Product = new Product(productId)
                        {
                            Category = Category.MainDish,
                            Name = "Product 1",
                            Description = "Product 1",
                            UnitPrice = 19.99
                        },
                        Amount = 19.99,
                        Code = 1,
                        Currency = Currency.BRL
                    }
                }
            };
        }

    }
}
