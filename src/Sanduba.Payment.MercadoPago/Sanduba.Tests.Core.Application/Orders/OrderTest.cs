using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sanduba.Tests.Core.Application.Orders
{
    public class OrderTest
    {
        [Fact]
        public void Order_Equals_ReturnsTrue()
        {
            // Arrange
            var mainDishFromCompleteOrder = OrderFixture.CompleteOrder(Guid.NewGuid()).Items
                .FirstOrDefault(item => item.Product.Category == Sanduba.Core.Domain.Orders.Category.MainDish);
            var mainDishFromSimpleOrder = OrderFixture.MainDishOrder(Guid.NewGuid(), mainDishFromCompleteOrder.Product.Id).Items
                .FirstOrDefault(item => item.Product.Category == Sanduba.Core.Domain.Orders.Category.MainDish);

            // Act
            var result = mainDishFromCompleteOrder.Equals(mainDishFromSimpleOrder);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Order_Different_ReturnsFalse()
        {
            // Arrange
            var mainDishFromCompleteOrder = OrderFixture.CompleteOrder(Guid.NewGuid()).Items
                .FirstOrDefault(item => item.Product.Category == Sanduba.Core.Domain.Orders.Category.MainDish);
            var beverageFromCompleteOrder = OrderFixture.CompleteOrder(Guid.NewGuid()).Items
                .FirstOrDefault(item => item.Product.Category == Sanduba.Core.Domain.Orders.Category.Beverage);

            // Act
            var result = mainDishFromCompleteOrder.Equals(beverageFromCompleteOrder);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Order_GetHashCode_ReturnsSameHashCode()
        {
            // Arrange
            var mainDishFromCompleteOrder = OrderFixture.CompleteOrder(Guid.NewGuid()).Items
                .FirstOrDefault(item => item.Product.Category == Sanduba.Core.Domain.Orders.Category.MainDish);
            var mainDishFromSimpleOrder = OrderFixture.MainDishOrder(Guid.NewGuid(), mainDishFromCompleteOrder.Product.Id).Items
                .FirstOrDefault(item => item.Product.Category == Sanduba.Core.Domain.Orders.Category.MainDish);

            // Act
            var hashCode1 = mainDishFromCompleteOrder.GetHashCode();
            var hashCode2 = mainDishFromSimpleOrder.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        [Fact]
        public void Order_GetHashCode_ReturnsDifferentHashCode()
        {
            // Arrange
            var mainDishFromCompleteOrder = OrderFixture.CompleteOrder(Guid.NewGuid()).Items
                .FirstOrDefault(item => item.Product.Category == Sanduba.Core.Domain.Orders.Category.MainDish);
            var beverageFromCompleteOrder = OrderFixture.CompleteOrder(Guid.NewGuid()).Items
                .FirstOrDefault(item => item.Product.Category == Sanduba.Core.Domain.Orders.Category.Beverage);

            // Act
            var hashCode1 = mainDishFromCompleteOrder.GetHashCode();
            var hashCode2 = beverageFromCompleteOrder.GetHashCode();

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }
    }
}
