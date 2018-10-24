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
    public class MenuViewModel : BindableBase
    {
        #region Services
        INavigationService _navigationService;
        #endregion

        #region Attributes

        List<Menu> _menuList;

        #endregion

        #region Properties
        public ObservableCollection<Menu> MyMenu
        {
            get;
            set;
        }
        #endregion

        #region Constructors

        public MenuViewModel(INavigationService navigationService)
        {
            _menuList = new List<Menu>()
            {
              new Menu(){ Title = "Stock",Icon="stock_menu", TargetPage="StockView"},
              new Menu(){ Title = "Inputs", Icon="inputs", TargetPage="InputListView"},
              new Menu(){ Title = "Pays", Icon="pays", TargetPage="PayListView"},
               new Menu(){ Title = "Managers", Icon="manager_menu", TargetPage="ManagerView"}
            };
            MyMenu = new ObservableCollection<Menu>(_menuList);
            _navigationService = navigationService;
            
        }

        #endregion

        #region Commands

        public ICommand ItemTappedCommand
        {
            get
            {
                return new DelegateCommand<Menu>(ItemTappedMethod);
            }
           
        }

        private async void ItemTappedMethod(Menu menu)
        {
            await _navigationService.NavigateAsync(menu.TargetPage);
        }

        #endregion
    }
}
