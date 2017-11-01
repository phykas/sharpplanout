using System;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace SharpPlanOut.Logger.MongoDb
{
    public class MongoDbGenericRepository<TModel> where TModel : Entity
    {
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;
        private readonly string _collectionName = typeof(TModel).Name;
        private IMongoCollection<TModel> _mongoCollection;

        public MongoDbGenericRepository()
        {
            _client = new MongoClient();
            GetCollection();
        }

        private void GetCollection()
        {
            _database = _client.GetDatabase("planout");
            _mongoCollection = _database.GetCollection<TModel>(_collectionName);
        }

        public IQueryable<TModel> Get()
        {
            GetCollection();
            return _mongoCollection.AsQueryable();
        }

        public TModel GetById(object id)
        {
            throw new NotImplementedException();
        }

        public void Insert(TModel entity)
        {
            GetCollection();
            _mongoCollection.InsertOne(entity);
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public void Delete(TModel entityToDelete)
        {
            GetCollection();
            _mongoCollection
                .DeleteOne(model => model.Id == entityToDelete.Id);
        }

        public void Update(TModel entityToUpdate)
        {
            GetCollection();
            _mongoCollection
                .ReplaceOne(model => model.Id == entityToUpdate.Id, entityToUpdate);
        }

        public IQueryable<TModel> GetMany(Func<TModel, bool> @where)
        {
            GetCollection();
            return
                _mongoCollection.AsQueryable().Where(where).AsQueryable();
        }

        public IQueryable<TModel> GetManyQueryable(Func<TModel, bool> @where)
        {
            GetCollection();
            return
                _mongoCollection.AsQueryable().Where(where).AsQueryable();
        }

        public TModel Get(Func<TModel, bool> @where)
        {
            GetCollection();
            return _mongoCollection.AsQueryable().FirstOrDefault(where);
        }

        public void Delete(Func<TModel, bool> @where)
        {
            GetCollection();
            _mongoCollection
                .DeleteOne(Expression.Lambda<Func<TModel, bool>>(Expression.Call(where.Method)));
        }

        public IQueryable<TModel> GetAll()
        {
            GetCollection();
            return _mongoCollection.AsQueryable();
        }

        public IQueryable<TModel> GetWithInclude(Expression<Func<TModel, bool>> predicate, params string[] include)
        {
            throw new NotImplementedException();
        }

        public bool Exists(object primaryKey)
        {
            GetCollection();
            return _mongoCollection.AsQueryable().Any(e => e.Id == (string)primaryKey);
        }

        public TModel GetSingle(Func<TModel, bool> predicate)
        {
            GetCollection();
            return _mongoCollection.AsQueryable().Single(predicate);
        }

        public TModel GetFirst(Func<TModel, bool> predicate)
        {
            GetCollection();
            return _mongoCollection.AsQueryable().FirstOrDefault(predicate);
        }

        public TModel GetLast(Func<TModel, bool> predicate)
        {
            GetCollection();
            return _mongoCollection.AsQueryable().LastOrDefault(predicate);
        }

        public IQueryable<T> IncludeAll<T>(IQueryable<T> queryable) where T : class
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            //does nothing in mongo db
        }

        public void Update(Expression<Func<TModel, object>> expression, TModel entityToUpdate)
        {
            throw new NotImplementedException();
        }

        public void Update(Expression<Func<TModel, bool>> expression, TModel entityToUpdate)
        {
            GetCollection();
            _mongoCollection
                .ReplaceOne(expression, entityToUpdate);
        }

        public void AddOrUpdate(Expression<Func<TModel, object>> expression, TModel[] entitiesToUpdate)
        {
            throw new NotImplementedException();
        }

        public void LazyLoading(bool enable)
        {
            throw new NotImplementedException();
        }

        public void Reload(TModel entity)
        {
            throw new NotImplementedException();
        }
    }
}