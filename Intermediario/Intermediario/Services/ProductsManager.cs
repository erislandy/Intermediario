using Intermediario.Interfaces;
using Intermediario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intermediario.Services
{
    public class ProductsManager
    {
        #region Services

        IDataService dataService;

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
            this.dataService = dataService;
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
                productExpected = dataService.Insert<Product>(product);
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
            dataService.Delete<Product>(product);
            Products.Remove(pro);
        }

        public void Update(Category category)
        {
            dataService.Update<Category>(category);
        }

        #endregion

    }
}
