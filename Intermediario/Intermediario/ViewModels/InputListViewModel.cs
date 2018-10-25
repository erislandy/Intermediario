using Intermediario.Models;
using Intermediario.Services;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intermediario.ViewModels
{
    public class InputListViewModel : BindableBase
    {
        #region Services

        PurchaseManager purchaseManager;

        #endregion

        #region Attributes

        IList<Purchase> _listPurchase;

        #endregion

        #region Properties

        public ObservableCollection<Purchase> InputList { get; set; }
        #endregion

        #region Constructors

        public InputListViewModel()
        {
            var provider = new Provider()
            {
                PersonId = 1,
                Name = "Carlos",
                LastName = "Mambrake",
                PhoneNumber = "52948962",
                Address = "Guantanamo,Cuba"
            };
            var provider1 = new Provider()
            {
                PersonId = 2,
                Name = "Justo",
                LastName = "Lazaro",
                PhoneNumber = "52948962",
                Address = "Habana,Cuba"
            };
            var product1 = new Product()
            {
                ProductId = 1,
                CategoryId = 1,
                Image = "icon3",
                Name = "Leche",
                Remarks = "Leche de vaca",

            };
            var product2 = new Product()
            {
                ProductId = 2,
                CategoryId = 1,
                Image = "icon1",
                Name = "Mantequilla",
                Remarks = "Buena",
            };
            var product3 = new Product()
            {
                ProductId = 3,
                CategoryId = 2,
                Image = "icon3",
                Name = "Mochila",
                Remarks = "Leche de vaca",

            };
            _listPurchase = new List<Purchase>()
            {
                new Purchase()
                {
                    PurchaseId = 1,
                    DatePurchase = DateTime.Now,
                    ProductId = product1.ProductId,
                    Product = product1,
                    Amount = 10,
                    PriceIn = 20,
                    ProviderId = provider.PersonId,
                    Provider = provider,

                },
                new Purchase()
                {
                    PurchaseId = 2,
                    DatePurchase = DateTime.Now,
                    ProductId = product2.ProductId,
                    Product = product2,
                    Amount = 10,
                    PriceIn = 20,
                    ProviderId = provider.PersonId,
                    Provider = provider,

                },
                 new Purchase()
                {
                    PurchaseId = 3,
                    DatePurchase = DateTime.Now,
                    ProductId = product3.ProductId,
                    Product = product3,
                    Amount = 15,
                    PriceIn = 20,
                    ProviderId = provider1.PersonId,
                    Provider = provider1,

                },

        };

            InputList = new ObservableCollection<Purchase>(_listPurchase);
        }
        #endregion
    }
}
