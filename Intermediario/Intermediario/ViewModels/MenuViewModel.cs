using Intermediario.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intermediario.ViewModels
{
    public class MenuViewModel : BindableBase
    {

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

        public MenuViewModel()
        {
            _menuList = new List<Menu>()
            {
              new Menu(){ Title = "Stock",Icon="stock_menu"},
              new Menu(){ Title = "Inputs", Icon="inputs"},
              new Menu(){ Title = "Pays", Icon="pays"},
               new Menu(){ Title = "Managers", Icon="manager_menu"}
            };
            MyMenu = new ObservableCollection<Menu>(_menuList);

        }

        #endregion


    }
}
