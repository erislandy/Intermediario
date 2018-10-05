
namespace Intermediario.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Interfaces;
    using Models;
    public class PurchaseManager
    {
        #region Services

        IDataService _dataService;
        #endregion

        #region Properties
        public IList<Purchase> PurchaseList { get; private set; }

        #endregion

        #region Constructors
        public PurchaseManager(IDataService dataService)
        {
            _dataService = dataService;
            PurchaseList = dataService.Get<Purchase>(true);
        }
        #endregion

        #region Methods

        public Purchase Add(Purchase purchase)
        {
            Purchase purchaseExpected = _dataService.Insert<Purchase>(purchase);
            PurchaseList.Add(purchaseExpected);
            ProductStockManager productStockManager = new ProductStockManager(_dataService);
            ProductStock productStock = new ProductStock()
            {
                ProductId = purchaseExpected.ProductId,
                ProviderId = purchaseExpected.ProviderId,
                Amount = purchaseExpected.Amount,
                Provider = purchaseExpected.Provider,
                Product = purchaseExpected.Product,
                State = StateEnum.Available
            };
            productStock.Code = productStockManager.GenerateCode(productStock);
            productStockManager.Add(productStock);

            return purchaseExpected;

        }

        #endregion

    }
}
