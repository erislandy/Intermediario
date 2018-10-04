using System;
using System.Collections.Generic;
using System.Linq;

namespace Intermediario.Services
{
    using Intermediario.Interfaces;
    using Intermediario.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ProviderManager
    {
        #region Services

        IDataService _dataService;

        #endregion

        #region Properties
        public IList<Provider> Providers { get; private set; }

        #endregion

        #region Constructors

        public ProviderManager(IDataService dataService)
        {
            _dataService = dataService;
            Providers = _dataService.Get<Provider>(true);
        }


        #endregion

        #region Methods
        public Provider Add(Provider provider)
        {
            Provider providerExpected = new Provider();
            var pro = Providers.Where(p => (p.Name + p.LastName).ToLower().Equals((provider.Name+ provider.LastName).ToLower()))
                               .FirstOrDefault();

            if (pro == null)
            {
                providerExpected = _dataService.Insert<Provider>(provider);
                Providers.Add(providerExpected);
            }
            else
            {
                var message = string.Format("{0} {1} is an existing provider", 
                                                provider.Name,provider.LastName);
                throw new Exception(message);
            }

            return providerExpected;

        }

        public void Delete(Provider provider)
        {
            var pro = Providers.Where(
                                         p => (p.Name + p.LastName).ToLower()
                                               .Equals((provider.Name + provider.LastName).ToLower())
                                      ).FirstOrDefault();
            if (pro.ProductStockList.Count > 0 ||
                pro.PurchaseList.Count > 0 ||
                pro.Pays.Count > 0)
            {
                var message = string.Format("{0} {1} contains elements related", 
                                                provider.Name,provider.LastName);
                throw new Exception(message);
            }
            _dataService.Delete<Provider>(provider);
            Providers.Remove(pro);
        }

        public void Update(Provider provider)
        {
            _dataService.Update<Provider>(provider);
        }

        #endregion
    }
}
