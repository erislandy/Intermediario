using Intermediario.Models;
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
    public class OperationsViewModel : BindableBase
    {
        #region Services

        INavigationService _navigationService;
        #endregion

        #region Attributes

        List<Option> _optionsList;

        #endregion

        #region Properties
        public ObservableCollection<Option> OptionsList { get; set; }

        #endregion

        #region Constructors

        public OperationsViewModel(INavigationService navigationService)
        {
            _optionsList = new List<Option>()
            {
                new Option()
                {
                    Name = "Stock",
                    Description ="You can see all products in stock and change its states",
                    ImagePath = "stock_product1",
                    TargetPage="StockView"
                },
                new Option()
                {
                    Name = "Inputs",
                    Description ="Enter and display inputs by different provider",
                    ImagePath = "product_transfer",
                    TargetPage = "InputListView"
                },
                new Option()
                {
                    Name = "Pays",
                    Description ="Edit and display pays",
                    ImagePath = "pay_products",
                    TargetPage="PayListView"
                },
                new Option()
                {
                    Name = "Managers",
                    Description ="You can do CRUD to product, category and provider entity",
                    ImagePath = "product_configuration",
                    TargetPage="ManagerView"
                }
            };

            OptionsList = new ObservableCollection<Option>(_optionsList);

            

            _navigationService = navigationService;
        }

        private void NavigationMethod(Option option)
        {
           _navigationService.NavigateAsync(option.TargetPage);
        }

        #endregion

        #region Commands

        public ICommand NavigationCommand

        {
            get
            {
                return new DelegateCommand<Option>(NavigationMethod);
            }
        }
        #endregion
    }
}
