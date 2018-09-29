
namespace Intermediario.Services
{
    using Intermediario.Interfaces;
    using Intermediario.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class CategoriesManager : ICategoriesManager
    {
        #region Services

        IDataService dataService;

        #endregion

        #region Properties
        public IDataService DataService
        {
            get;

        }

        #endregion

        #region Constructors

        public CategoriesManager(IDataService dataService)
        {
            this.dataService = dataService;
        }


        #endregion

        #region Methods
        public Category Add(Category category)
        {
            var cat = dataService.Get<Category>(false)
                                .Where(c => c.Description.ToLower().Equals(category.Description.ToLower()))
                                .FirstOrDefault();
            if (cat == null)
            {
                return dataService.Insert<Category>(category);
            }
            else
            {
                return cat;
            }


        }

        public void Delete(Category category)
        {
            dataService.Delete<Category>(category);
        }

        public IList<Category> GetList()
        {
            return dataService.Get<Category>(true);
        }

        public void Update(Category category)
        {
            dataService.Update<Category>(category);
        }


        #endregion

    }
}
