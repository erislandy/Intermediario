
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
    public class PurchaseManagerFixure
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
            dataServiceMock.Setup(m => m.Get<Purchase>(true))
                          .Returns(new List<Purchase>());

        }

        /// <summary>
        /// Adding new purchase where product does'nt exist in stock
        /// </summary>
        [TestMethod]
        public void AddNewPurchaseV1()
        {
            //Setup
            var product = new Product()
            {
                ProductId = 4,
                CategoryId = 1,
                Image = "icon3",
                Name = "Mochila Nike",
                Remarks = "Mochila nike azul bajo costo ",
            };
            var purchase = new Purchase()
            {
                DatePurchase = DateTime.Now,
                ProductId = product.ProductId,
                Product = product,
                Amount = 10,
                PriceIn = 20,
                ProviderId = provider.PersonId,
                Provider = provider,

            };
            dataServiceMock.Setup(m => m.Insert<ProductStock>(It.IsAny<ProductStock>()))
                       .Returns(new ProductStock()
                       {
                           ProductStockId = list.Count + 1,
                           Amount = purchase.Amount,
                           Code = "31Available",
                           Product = product,
                           ProductId = product.ProductId,
                           ProviderId = provider.PersonId,
                           Provider = provider,

                       })
                       .Verifiable();
            dataServiceMock.Setup(m => m.Insert<Purchase>(It.IsAny<Purchase>()))
                      .Returns(new Purchase()
                      {
                          PurchaseId = 1,
                          Amount = purchase.Amount,
                          DatePurchase = purchase.DatePurchase,
                          Product = product,
                          ProductId = product.ProductId,
                          ProviderId = provider.PersonId,
                          Provider = provider,
                          PriceIn = purchase.PriceIn,
                      })
                      .Verifiable();
            var purchaseManager = new PurchaseManager(dataServiceMock.Object);

            //Act
            var purchaseExpected = purchaseManager.Add(purchase);

            //Asserts
            dataServiceMock.Verify();

            Assert.AreEqual(1, purchaseManager.PurchaseList.Count);
            Assert.AreEqual(1, purchaseExpected.PurchaseId);
            Assert.AreEqual(4, list.Count);
            Assert.AreEqual(product.Name, list.ElementAt(3).Product.Name);


        }

        /// <summary>
        /// Adding new purchase where product exists in stock
        /// </summary>
        [TestMethod]
        public void AddNewPurchaseV2()
        {
            //Setup
            var product = new Product()
            {
                ProductId = 1,
                CategoryId = 1,
                Image = "icon3",
                Name = "Leche",
                Remarks = "Leche de vaca",
            };
            var purchase = new Purchase()
            {
                DatePurchase = DateTime.Now,
                ProductId = product.ProductId,
                Product = product,
                Amount = 10,
                PriceIn = 20,
                ProviderId = provider.PersonId,
                Provider = provider,

            };
            
            dataServiceMock.Setup(m => m.Insert<Purchase>(It.IsAny<Purchase>()))
                      .Returns(new Purchase()
                      {
                          PurchaseId = 1,
                          Amount = purchase.Amount,
                          DatePurchase = purchase.DatePurchase,
                          Product = product,
                          ProductId = product.ProductId,
                          ProviderId = provider.PersonId,
                          Provider = provider,
                          PriceIn = purchase.PriceIn,
                      })
                      .Verifiable();

            var purchaseManager = new PurchaseManager(dataServiceMock.Object);

            //Act
            var purchaseExpected = purchaseManager.Add(purchase);

            //Asserts
            dataServiceMock.Verify(m => m.Update<ProductStock>(
                                                    It.IsAny<ProductStock>()), 
                                                    Times.AtLeastOnce);
            dataServiceMock.Verify();

            Assert.AreEqual(1, purchaseManager.PurchaseList.Count);
            Assert.AreEqual(1, purchaseExpected.PurchaseId);
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(20, list.ElementAt(0).Amount);


        }
    }
}
