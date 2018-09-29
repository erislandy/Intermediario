

namespace Intermediario.Interfaces
{
    using System.Collections.Generic;
    public interface IDataService
    {
        void Delete<T>(T model);
        bool DeleteAll<T>() where T : class;
        T DeleteAllAndInsert<T>(T model) where T : class;
        T Find<T>(int pk, bool withChildren) where T : class;
        T First<T>(bool withChildren) where T : class;
        List<T> Get<T>(bool withChildren) where T : class;
        T Insert<T>(T model);
        T InsertOrUpdate<T>(T model) where T : class;
        void Save<T>(List<T> list) where T : class;
        void Update<T>(T model);
    }
}
