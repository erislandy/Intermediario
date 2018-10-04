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
    public class ProviderManagerFixure
    {
        
        List<Provider> providers;
        Mock<IDataService> dataServiceMock;

        [TestInitialize]
        public void Setup()
        {
            providers = new List<Provider>()
            {
                new Provider(){PersonId = 1, Name = "Carlos", LastName="Mambrake",
                               PhoneNumber = "52948962", Address="Guantanamo,Cuba"},
                new Provider(){PersonId = 2, Name = "Yorki", LastName="Elias",
                               PhoneNumber = "53670267", Address="Stgo,Cuba"},
                new Provider(){PersonId = 3, Name = "Justo", LastName="Lazaro",
                               PhoneNumber = "52881138", Address="La Habana,Cuba",

                               Pays = new List<Pay>()
                               {
                                   new Pay(){Certificated = true, ProviderId = 3}
                               }
                }

            };

            dataServiceMock = new Mock<IDataService>();
            dataServiceMock.Setup(m => m.Get<Provider>(true))
                          .Returns(providers);
        }

        [TestMethod]
        public void AddingNewProvider()
        {
            var provider = new Provider()
            {
                Name = "Michel",
                LastName = "Piti",
                PhoneNumber = "52529187",
                Address = "La Habana, Cuba",
           
            };

            //Setup

            dataServiceMock.Setup(m => m.Insert<Provider>(provider))
                         .Returns(new Provider()
                                      { PersonId = 4,
                                        Name = provider.Name,
                                        LastName = provider.LastName,
                                        PhoneNumber = provider.PhoneNumber,
                                        Address = provider.Address})
                         .Verifiable();


            //Act

            var providerManager = new ProviderManager(dataServiceMock.Object);
            var providerExpected = providerManager.Add(provider);

            //Assert

            dataServiceMock.Verify();
            Assert.AreEqual(4, providerExpected.PersonId);
            Assert.AreEqual(providerExpected.Name, provider.Name);
            Assert.AreEqual(4, providers.Count);


        }

        [TestMethod]
        public void AddingExistProvider()
        {
            var provider = new Provider()
            {

                Name = "Carlos",
                LastName = "Mambrake",
                PhoneNumber = "52948962",
                Address = "Guantanamo,Cuba"
            };
             

            //Act

            var providerManager = new ProviderManager(dataServiceMock.Object);

            Assert.ThrowsException<Exception>(() => providerManager.Add(provider));
            dataServiceMock.Verify(m => m.Insert(provider), Times.Never);

        }

        [TestMethod]
        public void DeleteAnProviderWithElements()
        {

            var provider = providers.ElementAt(2);

            //Act

            var providerManager = new ProviderManager(dataServiceMock.Object);
            Assert.ThrowsException<Exception>(() => providerManager.Delete(provider));
            dataServiceMock.Verify(m => m.Delete(provider), Times.Never());
        }

        [TestMethod]
        public void DeleteAnProviderWithoutElements()
        {

            var provider = new Provider()
            {
                PersonId = 1,
                Name = "Carlos",
                LastName = "Mambrake",
                PhoneNumber = "52948962",
                Address = "Guantanamo,Cuba"
            };
             
            


            //Act
            var providerManager = new ProviderManager(dataServiceMock.Object);
            providerManager.Delete(provider);

            // Asserts
            var providerExpected = providerManager.Providers.Where(p => (p.Name + p.LastName).ToLower() == (provider.Name + provider.LastName).ToLower())
                                                             .FirstOrDefault();
            Assert.IsNull(providerExpected);
            Assert.AreEqual(2, providerManager.Providers.Count);
            dataServiceMock.Verify(m => m.Delete(provider));

        }
    }
}
