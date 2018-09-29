
namespace Intermediario.Interfaces
{
    using Models;
    using System.Collections.Generic;
    public interface ICategoriesManager
    {
        IDataService DataService { get; }
        Category Add(Category category);
        void Update(Category category);
        void Delete(Category category);
        IList<Category> GetList();
    }
}
