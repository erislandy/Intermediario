using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intermediario.ViewModels
{

    public class MainViewModel
    {
        #region Properties

        public CategoryViewModel CategoryViewModel { get; set; }

        #endregion

        #region Constructors

        public MainViewModel()
        {
            CategoryViewModel = new CategoryViewModel();
        }
        #endregion
    }
}
