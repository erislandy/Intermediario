
namespace Intermediario.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Interfaces;
    using Models;
    
    public class ProductsManager
    {
        #region Services

        IDataService _dataService;

        #endregion

        #region Attributes

        Category _category;

        #endregion

        #region Properties
        public IList<Product> Products { get; private set; }

        #endregion

        #region Constructors
        public ProductsManager(IDataService dataService, Category category)
        {
            _dataService = dataService;
            _category = category;
            Products = category.ProductList;
           
        }

        #endregion

        #region Methods

       
        public Product Add(Product product)
        {
            Product productExpected = new Product();
            var pro = Products.Where(p => p.Name.ToLower().Equals(product.Name.ToLower()))
                               .FirstOrDefault();

            if (pro == null)
            {
                productExpected = _dataService.Insert<Product>(product);
                Products.Add(productExpected);
                
            }
            else
            {
                var message = string.Format("{0} is an existing product", product.Name);
                throw new Exception(message);
            }

            return productExpected;

        }

        public void Delete(Product product)
        {
            var pro = Products.Where(
                                         p => p.Name.ToLower()
                                               .Equals(product.Name.ToLower())
                                      ).FirstOrDefault();
            if (pro.ProductStockList.Count > 0 ||
                pro.Sales.Count > 0 ||
                pro.PurchaseList.Count > 0)
            {
                var message = string.Format("{0} contains elements related", product.Name);
                throw new Exception(message);
            }
            _dataService.Delete<Product>(product);
            Products.Remove(pro);
        }

        public void Update(Category category)
        {
            _dataService.Update<Category>(category);
        }

        #endregion

    }
}
