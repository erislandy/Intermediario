

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

    /*
    
        1- Available state 
        2- AwaitingForPaid state means produts is awaiting for paid
        3- Moved state means product has been moved out stock
        4- AwaitingForReturn state means product has been returned to provider
            because is a faulty product 
    
    */
    public enum StateEnum
    {
        Available, AwaitingForPaid, Moved, AwaitingForReturn
    }

}
