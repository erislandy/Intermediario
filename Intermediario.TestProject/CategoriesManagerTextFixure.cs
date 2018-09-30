

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
    public class CategoriesManagerTextFixure
    {
        List<Category> categories;
        Mock<IDataService> dataServiceMock;

        [TestInitialize]
        public void Setup()
        {
            categories = new List<Category>()
            {
                new Category(){ CategoryId = 1, Description = "Lacteos"},
                new Category(){ CategoryId = 2, Description = "Carnicos"},
                new Category(){ CategoryId = 3, Description = "Calzados"},
                new Category(){ CategoryId = 4, Description = "Ropas"},
                new Category(){ CategoryId = 5, Description = "Aseos Personal"},
            };
            dataServiceMock = new Mock<IDataService>();

            dataServiceMock.Setup(m => m.Get<Category>(true))
                          .Returns(categories);
        }

        [TestMethod]
        public void AddingNewCategory()
        {
            var category = new Category()
            {
                Description = "Electrodomésticos"
            };

            //Setup
            
            dataServiceMock.Setup(m => m.Insert<Category>(category))
                         .Returns(new Category() { CategoryId = 6, Description = category.Description })
                         .Verifiable();
            

            //Act

            var categoryManager = new CategoriesManager(dataServiceMock.Object);
            var categoryExpected = categoryManager.Add(category);

            //Assert

            dataServiceMock.Verify();
            Assert.AreEqual(6, categoryExpected.CategoryId);
            Assert.AreEqual(categoryExpected.Description, category.Description);

            
        }

        [TestMethod]
        public void AddingExistCategory()
        {
            var category = new Category()
            {
                Description = "Lacteos"
            };
            
            //Act

            var categoryManager = new CategoriesManager(dataServiceMock.Object);

            Assert.ThrowsException<Exception>(() => categoryManager.Add(category));
           


        }

        [TestMethod]
        public void DeleteAnCategoryWithProducts()
        {
            Product product1 = new Product()
            {
                ProductId = 1,
                CategoryId = 1,
                Category = categories.ElementAt(1),
                Image = "icon.png",
                Name = "Mantequilla",
                Remarks = "Muy buena"
            };
            Product product2 = new Product()
            {
                ProductId = 2,
                CategoryId = 1,
                Category = categories.ElementAt(2),
                Image = "icon2.png",
                Name = "Mantequilla",
                Remarks = "Muy buena"
            };

            var category = categories.ElementAt(1);
            category.ProductList.Add(product1);
            category.ProductList.Add(product2);

            
            //Act

            var categoryManager = new CategoriesManager(dataServiceMock.Object);
            Assert.ThrowsException<Exception>(() => categoryManager.Delete(category));
            dataServiceMock.Verify(m => m.Delete(category), Times.Never());
        }

        [TestMethod]
        public void DeleteAnCategoryWithoutProducts()
        {


            var category = new Category()
            {
                Description = "Lacteos"
            };

                           
            //Act
            var categoryManager = new CategoriesManager(dataServiceMock.Object);
            categoryManager.Delete(category);

            // Asserts
            var categoryExpected = categoryManager.Categories.Where(c => c.Description == category.Description)
                                                             .FirstOrDefault();
            Assert.IsNull(categoryExpected);
            Assert.AreEqual(4, categoryManager.Categories.Count);
            dataServiceMock.Verify(m => m.Delete(category));

        }
    }
}
