﻿

namespace Intermediario.Models
{
    using System;

    public class Sale
    {
        #region Properties

        public int SaleId { get; set; }
        public int PayId { get; set; }
        public int ProductStockId { get; set; }
        public int CustomerId { get; set; }
        public int MessengerId { get; set; }
        public DateTime SaleDate { get; set; }
        public double Amount { get; set; }
        public double SalePrice { get; set; }
        public SaleState SaleState { get; set; }

        //Navigation Properties
        public virtual Pay Pay { get; set; }
        public virtual ProductStock ProductStock { get; set; }
        public virtual Person Customer { get; set; }
        public virtual Person Messenger { get; set; }

        #endregion

        #region Methods
        public override int GetHashCode()
        {
            return SaleId;
        }
        #endregion
    }

    public enum SaleState
    {
        PendingLiquidate, Liquidated, Certificated
    }
}
