

namespace Intermediario.Models
{
    using System;
    public class Purchase
    {

        #region Properties

        public int PurchaseId { get; set; }
        public int ProviderId { get; set; }
        public int ProductId { get; set; }
        public DateTime DatePurchase { get; set; }
        public double Amount { get; set; }
        public string Remarks { get; set; }

        //Navigation Propeties
        public virtual Provider Provider { get; set; }
        public virtual Product Product { get; set; }

        #endregion

        #region Methods
        public override int GetHashCode()
        {
            return PurchaseId;
        }
        #endregion
    }
}
