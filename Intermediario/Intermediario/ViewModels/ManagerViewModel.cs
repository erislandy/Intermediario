using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intermediario.ViewModels
{
    public class ManagerViewModel : BindableBase
    {
        #region Attributes

        string _text;

        #endregion

        #region Properties

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                SetProperty(ref _text, value);
            }

        }
        #endregion

        #region Constructors

        public ManagerViewModel()
        {
            Text = "ManagerViewModel ok";
        }
        #endregion
    }
}
