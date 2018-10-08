
namespace Intermediario.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Interfaces;
    using Models;
    public class SaleManager
    {
        #region Services

        IDataService _dataService;

        #endregion

        #region Properties
        public IList<Sale> SaleList { get; private set; }

        #endregion

        #region Constructors
        public SaleManager(IDataService dataService)
        {
            _dataService = dataService;
            SaleList = dataService.Get<Sale>(true);
        }
        #endregion

        #region Methods

        public Sale Add(Sale sale)
        {
            if(sale.ProductStock.State != StateEnum.Available)
            {
                var message = string.Format("This element is not available");
                throw new Exception(message);
            }

            if (sale.Amount > sale.ProductStock.Amount)
            {
                var message = string.Format("Amount must not excess {0} units",sale.ProductStock.Amount);
                throw new Exception(message);
            }

            Sale saleExpected = _dataService.Insert<Sale>(sale);
            SaleList.Add(saleExpected);

            var productStock = sale.ProductStock;
            productStock.Amount -= sale.Amount;
            _dataService.Update<ProductStock>(productStock);
            return saleExpected;

        }

        #endregion

    }
}
