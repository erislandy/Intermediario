using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intermediario.ViewModels
{
    public class PayListViewModel : BindableBase
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

        public PayListViewModel()
        {
            Text = "PayListViewModel ok";
        }
        #endregion
    }
}
