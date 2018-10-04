using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Intermediario.Models;
using System.Collections.Generic;
using Intermediario.Interfaces;
using Moq;
using System.Linq;
using Intermediario.Services;

namespace Intermediario.TestProject
{
    [TestClass]
    public class ProductStockManagerFixure
    {
        Provider provider1;
        Provider provider2;
        Product product1;
        Product product2;
        List<ProductStock> list;
        Mock<IDataService> dataServiceMock;

        [TestInitialize]
        public void Setup()
        {

            provider1 = new Provider()
            {
                PersonId = 1,
                Name = "Carlos",
                LastName = "Mambrake",
                PhoneNumber = "52948962",
                Address = "Guantanamo,Cuba"
            };
            provider2 = new Provider()
            {
                PersonId = 2,
                Name = "Yorki",
                LastName = "Elias",
                PhoneNumber = "53670267",
                Address = "Stgo,Cuba"
            };
            product1 = new Product()
            {
                ProductId = 3,
                CategoryId = 1,
                Image = "icon3",
                Name = "Leche",
                Remarks = "Leche de vaca",

            };
            product2 = new Product()
            {
                ProductId = 1,
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
                    PriceIn = 2.00,
                    PriceOut = 3.00,
                    Code = "11" + StateEnum.Available.ToString(),
                    Product = product2,
                    Provider = provider1
                },
                new ProductStock()
                {
                    ProductStockId = 2,
                    ProductId = 1,
                    ProviderId = 1,
                    Amount = 5,
                    State = StateEnum.AwaitingForPaid,
                    PriceIn = 2.00,
                    PriceOut = 3.00,
                    Code = "11" + StateEnum.AwaitingForPaid.ToString(),
                    Product = product2,
                    Provider = provider1
                },
                new ProductStock()
                {
                    ProductStockId = 3,
                    ProductId = 3,
                    ProviderId = 1,
                    Amount = 5,
                    State = StateEnum.Available,
                    PriceIn = 1.80,
                    PriceOut = 2.50,
                    Code = "31" + StateEnum.Available.ToString(),
                    Product = product1,
                    Provider = provider1
                },
                new ProductStock()
                {
                    ProductStockId = 4,
                    ProductId = 3,
                    ProviderId = 2,
                    Amount = 5,
                    State = StateEnum.Available,
                    PriceIn = 1.80,
                    PriceOut = 2.50,
                    Code = "32" + StateEnum.Available.ToString(),
                    Product = product1,
                    Provider = provider2
                },
                new ProductStock()
                {
                    ProductStockId = 5,
                    ProductId = 3,
                    ProviderId = 2,
                    Amount = 1,
                    State = StateEnum.Moved,
                    PriceIn = 1.80,
                    PriceOut = 2.50,
                    Code = "32" + StateEnum.Moved.ToString(),
                    Product = product1,
                    Provider = provider2
                },
                 new ProductStock()
                {
                    ProductStockId = 6,
                    ProductId = 1,
                    ProviderId = 2,
                    Amount = 5,
                    State = StateEnum.AwaitingForPaid,
                    PriceIn = 2.00,
                    PriceOut = 3.00,
                    Code = "11" + StateEnum.AwaitingForPaid.ToString(),
                    Product = product2,
                    Provider = provider2
                },
                  new ProductStock()
                {
                    ProductStockId = 7,
                    ProductId = 1,
                    ProviderId = 2,
                    Amount = 5,
                    State = StateEnum.Available,
                    PriceIn = 2.00,
                    PriceOut = 3.00,
                    Code = "12" + StateEnum.Available.ToString(),
                    Product = product2,
                    Provider = provider2
                },
            };

            dataServiceMock = new Mock<IDataService>();
            dataServiceMock.Setup(m => m.Get<ProductStock>(true))
                          .Returns(list);

        }

        [TestMethod]
        public void GenerateCode()
        {
            var proStock = new ProductStock()
            {
                ProductStockId = 1,
                ProductId = 1,
                ProviderId = 1,
                Amount = 10,
                State = StateEnum.Available,
                PriceIn = 2.00,
                PriceOut = 3.00,
                Product = product2,
                Provider = provider1
            };
            var proStockManager = new ProductStockManager(dataServiceMock.Object);
            var code = proStockManager.GenerateCode(proStock);
            Assert.AreEqual("11Available", code);
        }

        [TestMethod]
        public void AddNewStockProduct()
        {
            var proStock = new ProductStock()
            {
                ProductId = 1,
                ProviderId = 1,
                Amount = 10,
                State = StateEnum.AwaitingForReturn,
                PriceIn = 2.00,
                PriceOut = 3.00,
                Product = product2,
                Provider = provider1
            };
            dataServiceMock.Setup(m => m.Insert<ProductStock>(proStock))
                        .Returns(new ProductStock()
                        {
                            ProductStockId = list.Count + 1,
                            Amount = proStock.Amount,
                            Code = proStock.Code,
                            PriceIn = proStock.PriceIn,
                            PriceOut = proStock.PriceOut,
                            Product = proStock.Product,
                            ProductId = proStock.ProductId,
                            ProviderId = proStock.ProviderId,
                            Provider = proStock.Provider,
                            
                                    })
                        .Verifiable();

            //Act
            var proStockManager = new ProductStockManager(dataServiceMock.Object);
            proStock.Code = proStockManager.GenerateCode(proStock);
            var proStockExpected = proStockManager.Add(proStock);

            //Asserts
            dataServiceMock.Verify();
            Assert.AreEqual(8, proStockExpected.ProductStockId);
            Assert.AreEqual(8, list.Count);


        }

        [TestMethod]
        public void AddAnExistingStockProduct()
        {
            //Setup
            var proStock = new ProductStock()
            {
                ProductId = 3,
                ProviderId = 1,
                Amount = 3,
                State = StateEnum.Available,
                PriceIn = 1.80,
                PriceOut = 2.50,
                Code = "31" + StateEnum.Available.ToString(),
                
            };
            
           
            //Act
            var proStockManager = new ProductStockManager(dataServiceMock.Object);
            proStock.Code = proStockManager.GenerateCode(proStock);
            var proStockExpected = proStockManager.Add(proStock);

            //Asserts
            dataServiceMock.Verify(
                    m => m.Update<ProductStock>(It.IsAny<ProductStock>()),
                                                Times.AtLeastOnce);
            Assert.AreEqual(7, list.Count);
            Assert.AreEqual(8, proStockExpected.Amount);


        }

        [TestMethod]
        public void ChangeStateWidhLongerAmount()
        {
            //Setup
            var proStock = list.ElementAt(6);

            var proStockManager = new ProductStockManager(dataServiceMock.Object);

            //Act
            
            //Asserts

            Assert.ThrowsException<Exception>(
                () => proStockManager.ChangeStateProductStock(
                    proStock, 
                    8, 
                    StateEnum.AwaitingForPaid));
            
        }

        [TestMethod]
        public void ChangeStateWidhSameState()
        {
            //Setup
            var proStock = list.ElementAt(6);

            var proStockManager = new ProductStockManager(dataServiceMock.Object);

            //Act

            //Asserts

            Assert.ThrowsException<Exception>(
                () => proStockManager.ChangeStateProductStock(
                    proStock,
                    3,
                    StateEnum.Available));

        }

        [TestMethod]
        public void ChangeToNextStateProductStockNotExist()
        {
            //Setup

            var productStock = list.ElementAt(2);
            dataServiceMock.Setup(m => m.Insert(It.IsAny<ProductStock>()))
                            .Returns(new ProductStock()
                            {
                                ProductStockId = 8,
                                ProductId = 3,
                                ProviderId = 1,
                                Amount = 2,
                                State = StateEnum.AwaitingForPaid,
                                PriceIn = 1.80,
                                PriceOut = 2.50,
                                Code = "31" + StateEnum.AwaitingForPaid.ToString()})
                                .Verifiable();
            var proStockManager = new ProductStockManager(dataServiceMock.Object);
           
            //Act
            var productStockNewState = proStockManager.ChangeStateProductStock(
                                        productStock,
                                        2,
                                        StateEnum.AwaitingForPaid);
            //Asserts
            dataServiceMock.Verify(m => m.Update<ProductStock>(It.IsAny<ProductStock>()),
                                        Times.Exactly(1));
            dataServiceMock.Verify();

            Assert.IsNotNull(productStockNewState);
            Assert.AreEqual(8,productStockNewState.ProductStockId);
            Assert.AreEqual(2, productStockNewState.Amount);
            Assert.AreEqual(StateEnum.AwaitingForPaid, productStockNewState.State);
            Assert.AreEqual(3, productStock.Amount);


        }

        [TestMethod]
        public void ChangeToNextStateProductStockExist()
        {
            //Setup

            var productStock = list.ElementAt(0);
            
            var proStockManager = new ProductStockManager(dataServiceMock.Object);

            //Act
            var productStockNewState = proStockManager.ChangeStateProductStock(
                                        productStock,
                                        2,
                                        StateEnum.AwaitingForPaid);
            //Asserts
            dataServiceMock.Verify(m => m.Update<ProductStock>(It.IsAny<ProductStock>()),
                                        Times.Exactly(2));
            
            Assert.IsNotNull(productStockNewState);
            Assert.AreEqual(2, productStockNewState.ProductStockId);
            Assert.AreEqual(7, productStockNewState.Amount);
            Assert.AreEqual(StateEnum.AwaitingForPaid, productStockNewState.State);
            Assert.AreEqual(8, productStock.Amount);


        }

    }
}
