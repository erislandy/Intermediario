
namespace Intermediario.TestProject
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Intermediario.Models;
    using Moq;
    using System.Linq;
    using Intermediario.Interfaces;
    using System.Collections.Generic;
    using Intermediario.Services;

    [TestClass]
    public class ProductsManagerFixure
    {
        Category categorySelected;
        List<Product> products;
        Mock<IDataService> dataServiceMock;

        [TestInitialize]
        public void Setup()
        {
            categorySelected = new Category()
            {
                CategoryId = 1,
                Description = "Lacteos",
            };
            products = new List<Product>()
            {
                new Product() { ProductId = 1, CategoryId = 1, Image = "icon1",
                               Name = "Mantequilla", Remarks = "Buena",
                               Category = categorySelected},
                new Product() { ProductId = 2, CategoryId = 1, Image = "icon2",
                               Name = "Queso", Remarks = "Marca serrano",
                               Category = categorySelected},
                new Product()
                {
                    ProductId = 3,
                    CategoryId = 1,
                    Image = "icon3",
                    Name = "Leche",
                    Remarks = "Leche de vaca",
                    Category = categorySelected,
                    ProductStockList = new List<ProductStock>()
                    {
                        new ProductStock(){ProductId = 1, Code = "0001"}
                    }
                }

            };
            categorySelected.ProductList = products;
            dataServiceMock = new Mock<IDataService>();

            dataServiceMock.Setup(m => m.Get<Product>(true))
                          .Returns(products);
        }

        [TestMethod]
        public void AddingNewProduct()
        {
            var product = new Product()
            {
                Name = "Yogourt",
                CategoryId = 1,
                Category = categorySelected,
                Image = "icon4",
                Remarks = "Hecho en casa"
            };

            //Setup

            dataServiceMock.Setup(m => m.Insert<Product>(product))
                         .Returns(new Product() { ProductId = 4, Name = product.Name })
                         .Verifiable();


            //Act

            var productManager = new ProductsManager(dataServiceMock.Object, categorySelected);
            var productExpected = productManager.Add(product);

            //Assert

            dataServiceMock.Verify();
            Assert.AreEqual(4, productExpected.ProductId);
            Assert.AreEqual(productExpected.Name, product.Name);
            Assert.AreEqual(4, categorySelected.ProductList.Count);


        }

        [TestMethod]
        public void AddingExistProduct()
        {
            var product = new Product()
            {
                ProductId = 1,
                CategoryId = 1,
                Image = "icon1",
                Name = "Mantequilla",
                Remarks = "Buena",
                Category = categorySelected
            };

            //Act

            var productManager = new ProductsManager(dataServiceMock.Object, categorySelected);

            Assert.ThrowsException<Exception>(() => productManager.Add(product));
            dataServiceMock.Verify(m => m.Insert(product), Times.Never);

        }

        [TestMethod]
        public void DeleteAnProductWithElements()
        {
           
            var product = products.ElementAt(2);
           
            //Act

            var productManager = new ProductsManager(dataServiceMock.Object, categorySelected);
            Assert.ThrowsException<Exception>(() => productManager.Delete(product));
            dataServiceMock.Verify(m => m.Delete(product), Times.Never());
        }

        [TestMethod]
        public void DeleteAnProductWithoutProducts()
        {


            var product = new Product()
            {
                Name = "Mantequilla"
            };


            //Act
            var productManager = new ProductsManager(dataServiceMock.Object, categorySelected);
            productManager.Delete(product);

            // Asserts
            var productExpected = productManager.Products.Where(p => p.Name == product.Name)
                                                             .FirstOrDefault();
            Assert.IsNull(productExpected);
            Assert.AreEqual(2, productManager.Products.Count);
            dataServiceMock.Verify(m => m.Delete(product));

        }
    }
}
