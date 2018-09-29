

namespace Intermediario.Models
{
    using System.Collections.Generic;
    public class Category
    {
        #region Constructors
        public Category()
        {
            ProductList = new List<Product>();
        }
        #endregion

        #region Properties
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public virtual IList<Product> ProductList { get; set; }

        #endregion

        #region Methods
        public override int GetHashCode()
        {
            return CategoryId;
        }
        #endregion
    }
}
