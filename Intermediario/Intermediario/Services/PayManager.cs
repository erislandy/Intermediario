namespace Intermediario.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Interfaces;
    using Models;
    public class PayManager
    {
        #region Services

        IDataService _dataService;

        #endregion

        #region Properties
        public IList<Pay> PayList { get; private set; }

        #endregion

        #region Constructors
        public PayManager(IDataService dataService)
        {
            _dataService = dataService;
            PayList = dataService.Get<Pay>(true);
        }
        #endregion

        #region Methods

        public Pay Add(Pay pay)
        {
            if (pay.Certificated)
            {
                throw new Exception("This pay must not be certificated");
            }
            if (pay.SaleList == null || pay.SaleList.Count == 0)
            {
                throw new Exception("There was not selected any sale");
            }

            for (var i = 0; i < pay.SaleList.Count; i++)
            {
                var sale = pay.SaleList.ElementAt(i);
                if(sale.SaleState != SaleState.PendingLiquidate)
                {
                    var message = string.Format("sale number {0} is not pending to liquidate state",i + 1);
                    throw new Exception(message);
                }
            }



            Pay payExpected = _dataService.Insert<Pay>(pay);
            PayList.Add(payExpected);

            foreach (var sale in pay.SaleList)
            {
                sale.SaleState = SaleState.Liquidated;
                sale.PayId = payExpected.PayId;
                sale.Pay = payExpected;
                _dataService.Update<Sale>(sale);
                payExpected.SaleList.Add(sale);
            }
            
            return payExpected;

        }
        public void UpdatePay(Pay pay)
        {
            if (pay.SaleList == null || pay.SaleList.Count == 0)
            {
                throw new Exception("There was not selected any sale");
            }
            if (!pay.Certificated)
            {
                throw new Exception("This pay can not be edit 'cause is certificated ");
            }
            _dataService.Update<Pay>(pay);

            foreach (var sale in pay.SaleList)
            {
                sale.PayId = pay.PayId;
                sale.Pay = pay;
                _dataService.Update<Sale>(sale);
            }

        }
        public void CertificatePay(Pay pay)
        {
            if(pay.Certificated == true)
            {
                throw new Exception("This pay is certificated..");
            }
            pay.Certificated = true;
            _dataService.Update<Pay>(pay);
            foreach (var sale in pay.SaleList)
            {
                sale.SaleState = SaleState.Certificated;
                _dataService.Update<Sale>(sale);
            }
        }
        public IList<Sale> GetPendingToLiquidateSales(Provider provider)
        {
            return _dataService.Get<Sale>(true)
                                .Where(s => s.SaleState == SaleState.PendingLiquidate)
                                .Where(s => s.ProductStock.Provider.PersonId == provider.PersonId)
                                .ToList<Sale>();
        }

        #endregion
    }
}
