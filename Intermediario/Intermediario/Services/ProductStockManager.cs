
namespace Intermediario.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Interfaces;
    using Models;

    public class ProductStockManager
    {
        #region Services

        IDataService _dataService;

        #endregion

        #region Properties
        public IList<ProductStock> ProductStockList { get; private set; }

        #endregion

        #region Constructors

        public ProductStockManager(IDataService dataService)
        {
            _dataService = dataService;
            ProductStockList = _dataService.Get<ProductStock>(true);
        }

        #endregion

        #region Methods

        public string GenerateCode(ProductStock productStock)
        {
            var code = productStock.ProductId.ToString() +
                       productStock.ProviderId.ToString() +
                       productStock.State.ToString();
            return code;
        }

        public ProductStock Add(ProductStock productStock)
        {
            var productExpected = new ProductStock();
            var pro = ProductStockList.Where(p => p.Code.Equals(productStock.Code))
                               .FirstOrDefault();

            if (pro == null)
            {
                productExpected = _dataService.Insert<ProductStock>(productStock);
                ProductStockList.Add(productExpected);
                return productExpected;
            }
            else
            {
                pro.Amount = pro.Amount + productStock.Amount;
                pro.State = productStock.State;
                Update(pro);
                return pro;
            }

           

        }

        public void Update(ProductStock productStock)
        {
            _dataService.Update<ProductStock>(productStock);
        }

        public ProductStock ChangeStateProductStock(ProductStock productStock, int amount, StateEnum nextState)
        {
            if(amount > productStock.Amount)
            {
                var message = string.Format("value {0} exceds amount in stock", amount);
                throw new Exception(message);

            }
            if (nextState == productStock.State)
            {
                var message = string.Format("product has {0} state", nextState.ToString());
                throw new Exception(message);

            }

            var nextProductStock = new ProductStock()
            {
                ProductId = productStock.ProductId,
                ProviderId = productStock.ProviderId,
                Amount = amount,
                State = nextState,
                PriceIn = productStock.PriceIn,
                PriceOut = productStock.PriceOut,
                Product = productStock.Product,
                Provider = productStock.Provider,
            };
            var productStockBefore = ProductStockList.Where(p => p.ProductId == productStock.ProductStockId)
                                                 .FirstOrDefault();
            productStockBefore.Amount -= amount;
            Update(productStockBefore);

            ChangeState changeState = new ChangeState()
            {
                ProductStockId = productStock.ProductStockId,
                BeforeState = productStock.State,
                NextState = nextState,
                Amount = amount,
            };
            _dataService.Insert<ChangeState>(changeState);
            nextProductStock.Code = GenerateCode(nextProductStock);
            return Add(nextProductStock);
            
        }
        #endregion
    }
}
