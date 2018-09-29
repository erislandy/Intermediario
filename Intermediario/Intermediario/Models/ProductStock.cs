

namespace Intermediario.Models
{
    using System.Collections.Generic;
    public class ProductStock
    {
        #region Constructors
        public ProductStock()
        {
            ChangeStateList = new List<ChangeState>();
        }
        #endregion

        #region Properties

        public int ProductStockId { get; set; }
        public int ProviderId { get; set; }
        public int ProductId { get; set; }
        public string Code { get; set; }
        public double Amount { get; set; }
        public double PriceIn { get; set; }
        public double PriceOut { get; set; }
        public StateEnum State { get; set; }

        //Navigation Properties

        public virtual IList<ChangeState> ChangeStateList { get; set; }
        public virtual Provider Provider { get; set; }
        public virtual Product Product { get; set; }
        #endregion

        #region Methods
        public override int GetHashCode()
        {
            return ProductStockId;
        }
        #endregion

    }

    public enum StateEnum
    {
        Available, AwaitingForPaid, 
    }

}
