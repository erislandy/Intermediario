
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
    public class PayManagerFixure
    {
        Mock<IDataService> dataServiceMock;
        List<Sale> sales;

        [TestInitialize]
        public void Setup()
        {
            dataServiceMock = new Mock<IDataService>();
            sales = new List<Sale>()
            {
                new Sale()
                {
                    SaleId = 1,
                    SaleDate = DateTime.Now,
                    CustomerId  = 1,
                    Amount = 4,
                    ProductStockId = 1,
                    SalePrice = 15.0,
                    MessengerId = 1,
                    ProductStock = new ProductStock()
                    {
                        ProductStockId = 1,
                        ProviderId = 1,
                        Provider = new Provider(){PersonId = 1, Name= "Carlos", LastName = "Mambrake"}
                    },
                    SaleState = SaleState.PendingLiquidate

                },
                new Sale()
                {
                    SaleId = 2,
                    SaleDate = DateTime.Now,
                    CustomerId  = 1,
                    Amount = 5,
                    ProductStockId = 2,
                    SalePrice = 7.0,
                    MessengerId = 1,
                    ProductStock = new ProductStock()
                    {
                        ProductStockId = 2,
                        ProviderId = 1,
                        Provider = new Provider(){PersonId = 1, Name= "Carlos", LastName = "Mambrake"}
                    },
                    SaleState = SaleState.PendingLiquidate

                },
                new Sale()
                {
                    SaleId = 3,
                    SaleDate = DateTime.Now,
                    CustomerId  = 2,
                    Amount = 3,
                    ProductStockId = 1,
                    SalePrice = 11.0,
                    MessengerId = 1,
                    ProductStock = new ProductStock()
                    {
                        ProductStockId = 3,
                        ProviderId = 1,
                        Provider = new Provider(){PersonId = 1, Name= "Carlos", LastName = "Mambrake"}
                    },
                    SaleState = SaleState.Liquidated

                },
                new Sale()
                {
                    SaleId = 4,
                    SaleDate = DateTime.Now,
                    CustomerId  = 2,
                    Amount = 3,
                    ProductStockId = 1,
                    SalePrice = 11.0,
                    MessengerId = 1,
                    ProductStock = new ProductStock()
                    {
                        ProductStockId = 3,
                        ProviderId = 2,
                        Provider = new Provider(){PersonId = 2, Name= "Justo", LastName = "Llerena"}
                    },
                    SaleState = SaleState.PendingLiquidate

                },
                new Sale()
                {
                    SaleId = 5,
                    SaleDate = DateTime.Now,
                    CustomerId  = 2,
                    Amount = 3,
                    ProductStockId = 1,
                    SalePrice = 11.0,
                    MessengerId = 1,
                    ProductStock = new ProductStock()
                    {
                        ProductStockId = 4,
                        ProviderId = 1,
                        Provider = new Provider(){PersonId = 1, Name= "Justo", LastName = "Llerena"}
                    },
                    SaleState = SaleState.Certificated

                }
            };
            dataServiceMock.Setup(m => m.Get<Sale>(true))
                           .Returns(sales);
            dataServiceMock.Setup(m => m.Get<Pay>(true))
                           .Returns(new List<Pay>() { });
        }

        [TestMethod]
        public void GetSalesPendingToLiquidate()
        {
            //Setup
            var provider = new Provider()
            {
                PersonId = 1,
                Name = "Carlos",
                LastName = "Mambrake",
            };
            var payManager = new PayManager(dataServiceMock.Object);

            //Act
            var list = payManager.GetPendingToLiquidateSales(provider);

            //Asserts
            Assert.AreEqual(2, list.Count);

        }

        /// <summary>
        /// property CertificatedState will be true and
        /// all sales will have SaleState = Certificated
        /// </summary>
        [TestMethod]
        public void CertificateState()
        {
            //Setup
            var provider = new Provider()
            {
                PersonId = 1,
                Name = "Carlos",
                LastName = "Mambrake",
            };

            var pay = new Pay()
            {
                Date = DateTime.Now,
                Provider = provider,
                ProviderId = 1,
                Certificated = false,

            };

            var payManager = new PayManager(dataServiceMock.Object);
            var salesList = payManager.GetPendingToLiquidateSales(provider);
            pay.SaleList = salesList;

            //Act
            payManager.CertificatePay(pay);

            //Asserts
            dataServiceMock.Verify(m => m.Update<Pay>(pay), Times.Exactly(1));
            dataServiceMock.Verify(m => m.Update<Sale>(It.IsAny<Sale>()), Times.Exactly(2));

            Assert.IsTrue(pay.Certificated);
            Assert.AreEqual(SaleState.Certificated, sales.ElementAt(0).SaleState);
            Assert.AreEqual(SaleState.Certificated, sales.ElementAt(1).SaleState);


        }

        [TestMethod]
        public void AddNewPayWithSaleInvalid()
        {
            //setup
            var sale = sales.ElementAt(4);
            var pay = new Pay()
            {
                Date = DateTime.Now,
                Provider = new Provider() { PersonId = 1, Name = "Carlos", LastName = "Mambrake" },
                ProviderId = 1,

            };
            pay.SaleList.Add(sale);
            PayManager payManager = new PayManager(dataServiceMock.Object);

            //Act and Assert
            Assert.ThrowsException<Exception>(() => payManager.Add(pay));

        }

        /// <summary>
        /// property Certificated == true throw a new exception
        /// </summary>
        [TestMethod]
        public void AddNewPayInvalid()
        {
            //setup
            var sale = sales.ElementAt(4);
            var pay = new Pay()
            {
                Date = DateTime.Now,
                Provider = new Provider() { PersonId = 1, Name = "Carlos", LastName = "Mambrake" },
                ProviderId = 1,
                Certificated = true

            };
            pay.SaleList.Add(sale);
            PayManager payManager = new PayManager(dataServiceMock.Object);

            //Act and Assert
            Assert.ThrowsException<Exception>(() => payManager.Add(pay));

        }

        [TestMethod]
        public void AddNewPay()
        {
            //Setup

            var provider = new Provider()
            {
                PersonId = 1,
                Name = "Carlos",
                LastName = "Mambrake",
            };

            var pay = new Pay()
            {
                Date = DateTime.Now,
                Provider = provider,
                ProviderId = 1,
                Certificated = false,

            };

            dataServiceMock.Setup(m => m.Insert<Pay>(pay))
                            .Returns(new Pay()
                            {
                                PayId = 1,
                                Date = pay.Date,
                                Provider = pay.Provider,
                                ProviderId = 1,
                                Certificated = false,
                                SaleList = pay.SaleList,
                            }).Verifiable();

            var payManager = new PayManager(dataServiceMock.Object);
            var salesList = payManager.GetPendingToLiquidateSales(provider);
            pay.SaleList = salesList;

            //Act
            var payExpected = payManager.Add(pay);

            //Asserts
            dataServiceMock.Verify(m => m.Update<Sale>(It.IsAny<Sale>()), Times.Exactly(2));
            dataServiceMock.Verify();

            Assert.AreEqual(1, payExpected.PayId);
            Assert.AreEqual(SaleState.Liquidated, sales.ElementAt(0).SaleState);
            Assert.AreEqual(SaleState.Liquidated, sales.ElementAt(1).SaleState);

        }



    }
}
