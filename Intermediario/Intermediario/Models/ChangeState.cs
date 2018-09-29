
namespace Intermediario.Models
{
    public class ChangeState
    {
        #region Properties
        public int ChangeStateId { get; set; }
        public int ProductStockId { get; set; }
        public double Amount { get; set; }

        public StateEnum BeforeState { get; set; }
        public StateEnum NextState { get; set; }

        //Navigation Properties
        public virtual ProductStock ProductStock { get; set; }

        #endregion
        #region Methods
        public override int GetHashCode()
        {
            return ChangeStateId;
        }
        #endregion
    }
}
