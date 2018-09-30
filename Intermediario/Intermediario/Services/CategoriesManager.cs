
namespace Intermediario.Services
{
    using Intermediario.Interfaces;
    using Intermediario.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CategoriesManager
    {
        #region Services

        IDataService dataService;

        #endregion

        #region Properties
        public IList<Category> Categories { get; private set; }

        #endregion

        #region Constructors

        public CategoriesManager(IDataService dataService)
        {
            this.dataService = dataService;
            Categories = dataService.Get<Category>(true);
        }


        #endregion

        #region Methods
        public Category Add(Category category)
        {
            Category categoryExpected = new Category();
            var cat = Categories.Where(c => c.Description.ToLower().Equals(category.Description.ToLower()))
                               .FirstOrDefault();
            
            if (cat == null)
            {
                categoryExpected = dataService.Insert<Category>(category);
                Categories.Add(categoryExpected);
            }
            else
            {
                var message = string.Format("{0} is an existing category",category.Description);
                throw new Exception(message);
            }

            return categoryExpected;
  
        }

        public void Delete(Category category)
        {
            var cat = Categories.Where(
                                         c => c.Description.ToLower()
                                               .Equals(category.Description.ToLower())
                                      ).FirstOrDefault();
            if(cat.ProductList.Count > 0)
            {
                var message = string.Format("{0} contains products related", category.Description);
                throw new Exception(message);
            }
            dataService.Delete<Category>(category);
            Categories.Remove(cat);
        }
        
        public void Update(Category category)
        {
            dataService.Update<Category>(category);
        }
        
        #endregion

    }
}
