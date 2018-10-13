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


        public MenuViewModel()
        {
            _menuList = new List<Menu>()
            {
              new Menu(){ Title = "Menu1"},
               new Menu(){ Title = "Menu2"},
                new Menu(){ Title = "Menu3"}
            };
            MyMenu = new ObservableCollection<Menu>(_menuList);

        }
    }
}
