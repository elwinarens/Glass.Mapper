﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace Glass.Mapper.Sc
{
    public class SitecoreService : AbstractService, ISitecoreService
    {
        public  Database Database { get; private set; }

        public SitecoreService(Database database, string contextName = "Default")
            :base(contextName)
        {
            Database = database;
        }

        public SitecoreService(string databaseName, string contextName = "Default")
            : base(contextName)
        {
            Database = Sitecore.Configuration.Factory.GetDatabase(databaseName);
        }

        public SitecoreService(string databaseName, Context context)
            : base(context ?? Context.Default )
        {
            Database = Sitecore.Configuration.Factory.GetDatabase(databaseName);
        }

        public SitecoreService(Database database, Context context)
            : base(context ?? Context.Default)
        {
            Database = database;
        }


        public T GetItem<T>(Guid id, bool isLazy = false, bool inferType = false) where T:class 
        {
            var item = Database.GetItem(new ID(id));
            return CreateType(typeof (T), item, isLazy, inferType) as T;

        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>        
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T>(Guid id, Language language, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(new ID(id), language);
            return CreateType<T>(item, isLazy, inferType);
        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>        
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, K>(Guid id, Language language, K param1, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(new ID(id), language);
            return CreateType<T, K>(item, param1, isLazy, inferType);
        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>        
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, K, L>(Guid id, Language language, K param1, L param2, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(new ID(id), language);
            return CreateType<T, K, L>(item, param1, param2, isLazy, inferType);

        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>        
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, K, L, M>(Guid id, Language language, K param1, L param2, M param3, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(new ID(id), language);
            return CreateType<T, K, L, M>(item, param1, param2, param3, isLazy, inferType);

        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <typeparam name="N">The type of the fourth constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>        
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, K, L, M, N>(Guid id, Language language, K param1, L param2, M param3, N param4, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(new ID(id), language);
            return CreateType<T, K, L, M, N>(item, param1, param2, param3, param4, isLazy, inferType);
        }

                /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>        
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T>(string path, Language language, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(path, language);
            return CreateType<T>(item, isLazy, inferType);
        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>        
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, K>(string path, Language language, K param1, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(path, language);
            return CreateType<T, K>(item, param1, isLazy, inferType);
        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>        
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, K, L>(string path, Language language, K param1, L param2, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(path, language);
            return CreateType<T, K, L>(item, param1, param2, isLazy, inferType);

        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>        
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, K, L, M>(string path, Language language, K param1, L param2, M param3, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(path, language);
            return CreateType<T, K, L, M>(item, param1, param2, param3, isLazy, inferType);

        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <typeparam name="N">The type of the fourth constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>        
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, K, L, M, N>(string path, Language language, K param1, L param2, M param3, N param4, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(path, language);
            return CreateType<T, K, L, M, N>(item, param1, param2, param3, param4, isLazy, inferType);
        }



        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T>(Guid id, Language language, global::Sitecore.Data.Version version, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(new ID(id), language, version);
            return CreateType<T>(item, isLazy, inferType);
        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, K>(Guid id, Language language, global::Sitecore.Data.Version version, K param1, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(new ID(id), language, version);
            return CreateType<T, K>(item, param1, isLazy, inferType);
        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, K, L>(Guid id, Language language, global::Sitecore.Data.Version version, K param1, L param2, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(new ID(id), language, version);
            return CreateType<T, K, L>(item, param1, param2, isLazy, inferType);
        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, K, L, M>(Guid id, Language language, global::Sitecore.Data.Version version, K param1, L param2, M param3, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(new ID(id), language, version);
            return CreateType<T, K, L, M>(item, param1, param2, param3, isLazy, inferType);
        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <typeparam name="N">The type of the fourth constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, K, L, M, N>(Guid id, Language language, global::Sitecore.Data.Version version, K param1, L param2, M param3, N param4, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(new ID(id), language, version);
            return CreateType<T, K, L, M, N>(item, param1, param2, param3, param4, isLazy, inferType);
        }

        public T GetItem<T>(string path, bool isLazy = false, bool inferType = false) where T : class
        {
            var item = Database.GetItem(path);
            return CreateType(typeof(T), item, isLazy, inferType) as T;
        }

        public void Save<T>(T target)
        {
            //TODO: should this be a separate context
          //  SitecoreTypeContext context = new SitecoreTypeContext();

            //TODO: ME - this may not work with a proxy
            var config = GlassContext.GetTypeConfiguration(target) as SitecoreTypeConfiguration;
            
            if(config == null) 
                throw new NullReferenceException("Can not save class, could not find configuration for {0}".Formatted(typeof(T).FullName));

            var item = config.ResolveItem(target, Database);
            if(item == null)
                throw new MapperException("Could not save class, item not found");
            
            WriteToItem(target, item, config);
           
        }

        /// <summary>
        /// Query Sitecore for a set of items. Proxy classes are created.
        /// </summary>
        /// <typeparam name="T">The type to return the items as</typeparam>
        /// <param name="query">The query to execute</param>
        /// <returns>Sitecore items as proxy classes of the specified type</returns>
        public IEnumerable<T> Query<T>(string query, bool isLazy = false, bool inferType = false) where T : class
        {
            return CreateTypes(isLazy, inferType, typeof(T), () => { return Database.SelectItems(query); }) as IEnumerable<T>;
        }

        /// <summary>
        /// Query Sitecore for a single item. 
        /// </summary>
        /// <typeparam name="T">The type to return the items as</typeparam>
        /// <param name="query">The query to execute</param>
        /// <returns>Sitecore item as the specified type</returns>
        public T QuerySingle<T>(string query, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.SelectSingleItem(query);
            return CreateType<T>(item, isLazy, inferType);
        }


        public void WriteToItem<T>(T target, Item item, SitecoreTypeConfiguration config = null)
        {
            if(config == null)
                config = GlassContext.GetTypeConfiguration(target) as SitecoreTypeConfiguration;

            SitecoreTypeSavingContext savingContext = new SitecoreTypeSavingContext();
            savingContext.Config = config;

            //ME-an item with no versions should be null

            savingContext.Item = item;
            savingContext.Object = target;

            item.Editing.BeginEdit();

            SaveObject(savingContext);

            item.Editing.EndEdit();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="item"></param>
        /// <param name="isLazy"></param>
        /// <param name="inferType"></param>
        /// <param name="constructorParameters">Parameters to pass to the constructor of the new class. Must be in the order specified on the consturctor.</param>
        /// <returns></returns>
        public object CreateType(Type type, Item item, bool isLazy , bool inferType, params object[] constructorParameters)
        {
            if (item == null || item.Versions.Count == 0) return null;


            if (constructorParameters != null && constructorParameters.Length > 4)
                throw new NotSupportedException("Maximum number of constructor parameters is 4");

            SitecoreTypeCreationContext creationContext = new SitecoreTypeCreationContext();
            creationContext.SitecoreService = this;
            creationContext.RequestedType = type;
            creationContext.ConstructorParameters = constructorParameters;
            creationContext.Item = item;
            creationContext.InferType = inferType;
            creationContext.IsLazy = isLazy;
            var obj = InstantiateObject(creationContext);

            return obj;
        }

        /// <summary>
        /// Create a collection of classes from the specified type
        /// </summary>
        /// <param name="isLazy">If true creates a proxy for each class</param>
        /// <param name="type">The type to return</param>
        /// <param name="getItems">A function that returns the list of items to load</param>
        /// <param name="inferType">Infer the type to be loaded from the item template</param>
        /// <returns>An enumerable of the items as the specified type</returns>
        public IEnumerable CreateTypes(bool isLazy, bool inferType, Type type, Func<IEnumerable<Item>> getItems)
        {
            return Utilities.CreateGenericType(typeof(LazyItemEnumerable<>), new Type[] { type }, getItems, isLazy, inferType, this) as IEnumerable;
        }
        
        public override AbstractDataMappingContext CreateDataMappingContext(AbstractTypeCreationContext abstractTypeCreationContext, Object obj)
        {
            var scTypeContext =  abstractTypeCreationContext as SitecoreTypeCreationContext;
            return new SitecoreDataMappingContext(obj, scTypeContext.Item, this);
        }

        public override AbstractDataMappingContext CreateDataMappingContext(AbstractTypeSavingContext creationContext)
        {
            var scContext = creationContext as SitecoreTypeSavingContext;
            return new SitecoreDataMappingContext(scContext.Object, scContext.Item, this);
        }


        /// <summary>
        /// Creates a class from the specified item
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="item">The item to load data from</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>The item as the specified type</returns>
        public T CreateType<T>(Item item, bool isLazy = false, bool inferType = false) where T : class
        {
            return (T)CreateType(typeof(T), item, isLazy, inferType);
        }

        /// <summary>
        /// Creates a class from the specified item with a single constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="item">The item to load data from</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <returns>The item as the specified type</returns>
        public T CreateType<T, K>(Item item, K param1, bool isLazy = false, bool inferType = false)
        {
            return (T)CreateType(typeof(T), item, isLazy, inferType, param1);

        }

        /// <summary>
        /// Creates a class from the specified item with a two constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="item">The item to load data from</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <returns>The item as the specified type</returns>
        public T CreateType<T, K, L>(Item item, K param1, L param2, bool isLazy = false, bool inferType = false)
        {
            return (T)CreateType(typeof(T), item, isLazy, inferType, param1, param2);
        }

        /// <summary>
        /// Creates a class from the specified item with a two constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="item">The item to load data from</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <returns>The item as the specified type</returns>
        public T CreateType<T, K, L, M>(Item item, K param1, L param2, M param3, bool isLazy = false, bool inferType = false)
        {
            return (T)CreateType(typeof(T), item, isLazy, inferType, param1, param2, param3);
        }

        /// <summary>
        /// Creates a class from the specified item with a two constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <typeparam name="N">The type of the fourth constructor parameter</typeparam>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="item">The item to load data from</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <returns>The item as the specified type</returns>
        public T CreateType<T, K, L, M, N>(Item item,  K param1, L param2, M param3, N param4, bool isLazy = false, bool inferType = false)
        {
            return (T)CreateType(typeof(T), item, isLazy, inferType, param1, param2, param3, param4);
        }
        
    }
}
