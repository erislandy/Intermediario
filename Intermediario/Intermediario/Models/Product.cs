using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intermediario.Models
{
    public class Product
    {
        #region Consturctors
        public Product()
        {
        }
        #endregion

        #region Properties
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Remarks { get; set; }

        //Navigation Properties
        public virtual Category Category { get; set; }

        #endregion

        #region Methods
        public override int GetHashCode()
        {
            return ProductId;
        }
        #endregion
    }
}
