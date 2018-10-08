
namespace Intermediario.TestProject
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Intermediario.Interfaces;
    using Intermediario.Models;
    using System.Collections.Generic;
    using System;
    using Intermediario.Services;
    using System.Linq;

    [TestClass]
    public class SaleManagerFixure
    {
        Mock<IDataService> dataServiceMock;
        Provider provider;
        List<ProductStock> list;
        Product product1;
        Product product2;
        Product product3;

        [TestInitialize]
        public void Setup()
        {
            provider = new Provider()
            {
                PersonId = 1,
                Name = "Carlos",
                LastName = "Mambrake",
                PhoneNumber = "52948962",
                Address = "Guantanamo,Cuba"
            };
            product1 = new Product()
            {
                ProductId = 1,
                CategoryId = 1,
                Image = "icon3",
                Name = "Leche",
                Remarks = "Leche de vaca",

            };
            product2 = new Product()
            {
                ProductId = 2,
                CategoryId = 1,
                Image = "icon1",
                Name = "Mantequilla",
                Remarks = "Buena",
            };
            list = new List<ProductStock>()
            {
                new ProductStock()
                {
                    ProductStockId = 1,
                    ProductId = 1,
                    ProviderId = 1,
                    Amount = 10,
                    State = StateEnum.Available,
                    Code = "11" + StateEnum.Available.ToString(),
                    Provider = provider,
                    Product = product1
                },
                new ProductStock()
                {
                    ProductStockId = 2,
                    ProductId = 1,
                    ProviderId = 1,
                    Amount = 5,
                    State = StateEnum.AwaitingForPaid,
                    Code = "11" + StateEnum.AwaitingForPaid.ToString(),
                    Product = product1,
                    Provider = provider
                },
                new ProductStock()
                {
                    ProductStockId = 3,
                    ProductId = 2,
                    ProviderId = 1,
                    Amount = 5,
                    State = StateEnum.Available,
                    Code = "31" + StateEnum.Available.ToString(),
                    Product = product2,
                    Provider = provider
                },

            };
            dataServiceMock = new Mock<IDataService>();
            dataServiceMock.Setup(m => m.Get<ProductStock>(true))
                          .Returns(list);
            dataServiceMock.Setup(m => m.Get<Sale>(true))
                          .Returns(new List<Sale>());

        }

        /// <summary>
        /// Adding new sale where product in stock has'nt an available state
        /// </summary>
        [TestMethod]
        public void AddNewSaleV1()
        {
            //Setup
            var sale = new Sale()
            {
                SaleDate= DateTime.Now,
                ProductStockId = list.ElementAt(1).ProductStockId,
                ProductStock = list.ElementAt(1),
                Amount = 10,
                SaleState = SaleState.PendingLiquidate
            };
            
           
            var saleManager = new SaleManager(dataServiceMock.Object);


            //Asserts
            Assert.ThrowsException<Exception>(() => saleManager.Add(sale));

        }

        /// <summary>
        /// Adding new sale where amount excess amount of product in stock
        /// </summary>
        [TestMethod]
        public void AddNewSaleV2()
        {
            //Setup
            var sale = new Sale()
            {
                SaleDate = DateTime.Now,
                ProductStockId = list.ElementAt(0).ProductStockId,
                ProductStock = list.ElementAt(0),
                Amount = 11,
                SaleState = SaleState.PendingLiquidate
            };


            var saleManager = new SaleManager(dataServiceMock.Object);


            //Asserts
            Assert.ThrowsException<Exception>(() => saleManager.Add(sale));

        }

        /// <summary>
        /// Add new sale and take products in stock
        /// </summary>
        [TestMethod]
        public void AddNewSale()
        {
            //Setup
            var sale = new Sale()
            {
                SaleDate = DateTime.Now,
                ProductStockId = list.ElementAt(0).ProductStockId,
                ProductStock = list.ElementAt(0),
                Amount = 8,
                SalePrice = 1.00,
                SaleState = SaleState.PendingLiquidate
            };
            dataServiceMock.Setup(m => m.Insert<Sale>(It.IsAny<Sale>()))
                      .Returns(new Sale()
                      {
                          SaleId = 1,
                          Amount = sale.Amount,
                          SaleDate = sale.SaleDate,
                          SalePrice = sale.SalePrice,
                          SaleState = SaleState.PendingLiquidate,
                          ProductStockId = list.ElementAt(0).ProductStockId,
                          ProductStock = list.ElementAt(0)
                      })
                      .Verifiable();

            var saleManager = new SaleManager(dataServiceMock.Object);
            //Act

            var saleExpected = saleManager.Add(sale);


             //Asserts
             dataServiceMock.Verify();
            dataServiceMock.Verify(m => m.Update<ProductStock>(
                                                    It.IsAny<ProductStock>()),
                                                    Times.AtLeastOnce);
            Assert.AreEqual(1, saleExpected.SaleId);
            Assert.AreEqual(1, saleManager.SaleList.Count);
            Assert.AreEqual(2, list.ElementAt(0).Amount);

        }

    }
}
