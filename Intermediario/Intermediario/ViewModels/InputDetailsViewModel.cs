using Intermediario.Models;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intermediario.ViewModels
{
    public class InputDetailsViewModel : BindableBase, INavigationAware
    {

        #region Attributes

        Purchase _purchase;

        #endregion

        #region Properties

        public Purchase SelectedPurchase
        {
            get => _purchase;
            set => SetProperty(ref _purchase, value);
        }

        #endregion

        #region Navigation


        public void OnNavigatedFrom(NavigationParameters parameters)
        {
           
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("purchaseSelected"))
            {
                SelectedPurchase = parameters.GetValue<Purchase>("purchaseSelected");
            }
        }


        #endregion


    }
}
