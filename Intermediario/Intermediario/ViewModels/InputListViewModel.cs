using Intermediario.Models;
using Intermediario.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Intermediario.ViewModels
{
    public class InputListViewModel : BindableBase
    {
        #region Services

        PurchaseManager purchaseManager;
        INavigationService _navigationService;

        #endregion

        #region Attributes

        IList<Purchase> _listPurchase;

        #endregion

        #region Properties

        public ObservableCollection<Purchase> InputList { get; set; }
        #endregion

        #region Constructors

        public InputListViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            var provider = new Provider()
            {
                PersonId = 1,
                Name = "Carlos",
                LastName = "Mambrake",
                PhoneNumber = "52948962",
                Address = "Guantanamo,Cuba",
                ImagePath = "duenno.jpg" 
            };
            var provider1 = new Provider()
            {
                PersonId = 2,
                Name = "Rosmeri",
                LastName = "Escobar",
                PhoneNumber = "52948962",
                Address = "Habana,Cuba",
                ImagePath="encargada.jpg"
            };
            var product1 = new Product()
            {
                ProductId = 1,
                CategoryId = 1,
                ImagePath = "leche_de_vaca.jpg",
                Name = "Leche",
                Remarks = "Leche de vaca",

            };
            var product2 = new Product()
            {
                ProductId = 2,
                CategoryId = 1,
                ImagePath = "mantequilla.jpg",
                Name = "Mantequilla",
                Remarks = "Buena",
            };
            var product3 = new Product()
            {
                ProductId = 3,
                CategoryId = 2,
                ImagePath = "mochila.jpg",
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
                    Remarks="Carlos trajo elementos son poner precio"

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
                    Remarks="Carlos trajo elementos son poner precio"

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
                    Remarks="Carlos trajo elementos son poner precio"

                },

        };

            InputList = new ObservableCollection<Purchase>(_listPurchase);
        }
        #endregion

        #region Commands

        public ICommand ItemTappedCommand

        {
            get
            {
                return new DelegateCommand<Purchase>(ItemTappedMethod);
            }
        }

        private async void ItemTappedMethod(Purchase purchase)
        {
            var p = new NavigationParameters
            {
                { "purchaseSelected", purchase }
            };

            await _navigationService.NavigateAsync("InputDetailsView", p);
            
        }
        #endregion
    }
}
