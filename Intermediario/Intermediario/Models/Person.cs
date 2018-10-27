
namespace Intermediario.Models
{
 
    public class Person
    {
        #region Properties
        public int PersonId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string ImagePath { get; set; }


        #endregion

        #region Methods
        public override int GetHashCode()
        {
            return PersonId;
        }
        #endregion

    }
}
