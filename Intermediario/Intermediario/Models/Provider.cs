
namespace Intermediario.Models
{
    using System.Collections.Generic;
    public class Provider : Person
    {

        #region Costurctors
        public Provider()
        {
            PurchaseList = new List<Purchase>();
            Pays = new List<Pay>();
            ProductStockList = new List<ProductStock>();
        }

        #endregion

        #region Properties
        public virtual IList<Purchase> PurchaseList { get; set; }
        public virtual IList<Pay> Pays { get; set; }
        public virtual IList<ProductStock> ProductStockList { get; set; }

        #endregion
    }
}
