
namespace Intermediario.Models
{
    using System;
    using System.Collections.Generic;

    public class Pay
    {
        #region Constructors
        public Pay()
        {
            SaleList = new List<Sale>();
        }
        #endregion

        #region Properties
        public int PayId { get; set; }
        public int ProviderId { get; set; }
        public DateTime Date { get; set; }
        public bool Certificated { get; set; }

        //Navigation Properties
        public virtual Provider Provider { get; set; }
        public virtual IList<Sale> SaleList { get; set; }

        #endregion

        #region Methods
        public override int GetHashCode()
        {
            return PayId;
        }
        #endregion
    }

}
