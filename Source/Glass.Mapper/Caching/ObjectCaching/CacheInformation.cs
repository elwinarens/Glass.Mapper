using Glass.Mapper.Caching.CacheKeyResolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Glass.Mapper.Caching.ObjectCaching
{
    public class CacheInformation<TIdType>
    {
        private readonly int _timeout = 10000;
        private readonly ReaderWriterLockSlim _readerWriterLockRelatedKeys = new ReaderWriterLockSlim();
        private readonly ReaderWriterLockSlim _readerWriterLockCacheKey = new ReaderWriterLockSlim();
        
        private volatile Dictionary<string, List<string>> _relatedKeys;

        private volatile List<string> _objectKeys;

        private volatile Dictionary<string, List<string>> _objectKeysRelatedKeys;

        private volatile Dictionary<TIdType, Stack<CacheKey<TIdType>>> _idToCacheKey;

        public CacheInformation()
        {
            _relatedKeys = new Dictionary<string, List<string>>();
            _objectKeysRelatedKeys = new Dictionary<string, List<string>>();
            _objectKeys = new List<string>();
            _idToCacheKey = new Dictionary<TIdType, Stack<CacheKey<TIdType>>>();
        }

        public CacheInformation(int timeout)
            : this()
        {
            _timeout = timeout;
        }

        public Stack<CacheKey<TIdType>> GetCacheKeys(TIdType id)
        {
            using (var rel = new ReaderWriterLock(_readerWriterLockCacheKey, ReaderWriterLockType.Reader, _timeout))
            {
                if (rel.HasTakenLock)
                {
                    return GetCacheKeysKvp(id).Value;
                }
            }
            //TODO: create an exception type
            throw new Exception("Could not take lock");
        }

        public void AddCacheKey(TIdType id, CacheKey<TIdType> cacheKey)
        {
            using (var rel = new ReaderWriterLock(_readerWriterLockCacheKey, ReaderWriterLockType.Writer, _timeout))
            {
                if (!rel.HasTakenLock) return;
                InternalAddCacheKey(id, cacheKey);
            }
        }

        private void InternalAddCacheKey(TIdType id, CacheKey<TIdType> cacheKey)
        {
            if (!ContainsCacheKey(id))
            {
                _idToCacheKey.Add(id, new Stack<CacheKey<TIdType>>());
            }

            if (!_idToCacheKey[id].Contains(cacheKey))
            {
                _idToCacheKey[id].Push(cacheKey);
            }
        }

        public bool ContainsCacheKey(TIdType id, CacheKey<TIdType> cacheKey)
        {
            using (var rel = new ReaderWriterLock(_readerWriterLockCacheKey, ReaderWriterLockType.Reader, _timeout))
            {
                if (rel.HasTakenLock)
                {
                    return ContainsCacheKey(id) && GetCacheKeysKvp(id).Value.Any(x => x == cacheKey);
                }
            }
            //TODO: create an exception type
            throw new Exception("Could not take lock");
        }

        public bool ContainsCacheKey(CacheKey<TIdType> cacheKey, List<CacheKey<TIdType>> cacheKeys)
        {
            using (var rel = new ReaderWriterLock(_readerWriterLockCacheKey, ReaderWriterLockType.Reader, _timeout))
            {
                if (rel.HasTakenLock)
                {
                    return cacheKeys.Contains(cacheKey);
                }
            }
            //TODO: create an exception type
            throw new Exception("Could not take lock");
        }

        private bool ContainsCacheKey(TIdType id)
        {
            return !GetCacheKeysKvp(id).Equals(default(KeyValuePair<TIdType, Stack<CacheKey<TIdType>>>));
        }

        private KeyValuePair<TIdType, Stack<CacheKey<TIdType>>> GetCacheKeysKvp(TIdType id)
        {
            return GetCacheKeys().SingleOrDefault(x => x.Key.Equals(id));
        }

        private Dictionary<TIdType, Stack<CacheKey<TIdType>>> GetCacheKeys()
        {
            return  CloneDictionary(_idToCacheKey);;
        }

        public void AddRelatedKey(string releatedKey)
        {
            using (var rel = new ReaderWriterLock(_readerWriterLockRelatedKeys, ReaderWriterLockType.Writer, _timeout))
            {
                if (rel.HasTakenLock)
                {
                    if (!ContainsRelatedKey(releatedKey))
                    {
                        InternalAddRelatedKey(releatedKey);
                    }
                }
            }
        }

        private void InternalAddRelatedKey(string relatedKey)
        {
            using (var rel = new ReaderWriterLock(_readerWriterLockRelatedKeys, ReaderWriterLockType.Writer, _timeout))
            {
                if (rel.HasTakenLock)
                {
                    _relatedKeys.Add(relatedKey, new List<string>());
                }
            }
        }

        private void AddRelatedKayAndObjectKey(string releatedKey, string objectKey)
        {
            using (var rel = new ReaderWriterLock(_readerWriterLockRelatedKeys, ReaderWriterLockType.Writer, _timeout))
            {
                if (rel.HasTakenLock)
                {
                    if (!_relatedKeys.ContainsKey(releatedKey))
                    {
                        _relatedKeys.Add(releatedKey, new List<string>());
                    }

                    _relatedKeys[releatedKey].Add(objectKey);
                    _objectKeys.Add(objectKey);

                    if (!_objectKeysRelatedKeys.ContainsKey(objectKey))
                    {
                        _objectKeysRelatedKeys.Add(objectKey, new List<string>());
                    }
                    _objectKeysRelatedKeys[objectKey].Add(releatedKey);
                }
            }
        }

        public void AddObjectKey(string releatedKey, string objectKey)
        {
            if (ContainsRelatedKey(releatedKey))
            {
                using (var rel = new ReaderWriterLock(_readerWriterLockRelatedKeys, ReaderWriterLockType.Writer, _timeout))
                {
                    if (rel.HasTakenLock)
                    {
                        if (!_relatedKeys.ContainsKey(releatedKey))
                        {
                            _relatedKeys.Add(releatedKey, new List<string>());
                        }
                        _relatedKeys[releatedKey].Add(objectKey);
                        _objectKeys.Add(objectKey);

                        if (!_objectKeysRelatedKeys.ContainsKey(objectKey))
                        {
                            _objectKeysRelatedKeys.Add(objectKey, new List<string>());
                        }

                        _objectKeysRelatedKeys[objectKey].Add(releatedKey);
                    }
                }
            }
            else
            {
                AddRelatedKayAndObjectKey(releatedKey, objectKey);
            }
        }

        public System.Collections.ICollection GetRelatedKeys()
        {
            var keyList = new List<string>();
            using (var rel = new ReaderWriterLock(_readerWriterLockRelatedKeys, ReaderWriterLockType.Reader, _timeout))
            {
                if (rel.HasTakenLock)
                {
                    keyList = CloneList(this._relatedKeys.Keys.ToList());
                }
            }

            return keyList;
        }

        private List<T> CloneList<T>(IEnumerable<T> list)
        {
            return new List<T>(list);
        }

        private Dictionary<T, K> CloneDictionary<T, K>(IDictionary<T, K> dictionary)
        {
            return new Dictionary<T, K>(dictionary);
        }

        public System.Collections.ICollection Keys
        {
            get
            {
                var returnKeyList = new List<string>();
                using (var rel = new ReaderWriterLock(_readerWriterLockRelatedKeys, ReaderWriterLockType.Writer, _timeout))
                {
                    if (rel.HasTakenLock)
                    {
                        returnKeyList = new List<string>(_objectKeys);
                    }
                }

                return returnKeyList;
            }
        }

        public bool ContainsObjectKey(string objectKey)
        {
            return ((List<string>)Keys).AsParallel().Any(x => x == objectKey);
        }

        public bool ContainsRelatedKey(string releatedKey)
        {
            return ((List<string>)GetRelatedKeys()).AsParallel().Any(x => x == releatedKey);
        }

        public void AddObjectKey(IEnumerable<string> releatedKeys, string objectKey)
        {
            using (var rel = new ReaderWriterLock(_readerWriterLockRelatedKeys, ReaderWriterLockType.Writer, _timeout))
            {
                if (!rel.HasTakenLock) return;

                foreach (var releatedKey in releatedKeys)
                {
                    if (ContainsRelatedKey(releatedKey))
                    {
                        if (!_relatedKeys.ContainsKey(releatedKey))
                        {
                            _relatedKeys.Add(releatedKey, new List<string>());
                        }

                        _relatedKeys[releatedKey].Add(objectKey);

                        _objectKeys.Add(objectKey);

                        if (!_objectKeysRelatedKeys.ContainsKey(objectKey))
                        {
                            _objectKeysRelatedKeys.Add(objectKey, new List<string>());
                        }
                        _objectKeysRelatedKeys[objectKey].Add(releatedKey);
                    }
                    else
                    {
                        AddRelatedKayAndObjectKey(releatedKey, objectKey);
                    }
                }
            }
        }

        public System.Collections.ICollection GetObjectKeys(string releatedKey)
        {
            List<string> returnList;
            using (new ReaderWriterLock(_readerWriterLockRelatedKeys, ReaderWriterLockType.Writer, _timeout))
            {
                returnList = CloneList(_relatedKeys[releatedKey]);
            }

            return returnList;
        }

        /// <summary>
        /// Removes the object key.
        /// </summary>
        /// <param name="objectKey">The object key.</param>
        public void RemoveObjectKey(string objectKey)
        {
            using (var rel = new ReaderWriterLock(_readerWriterLockRelatedKeys, ReaderWriterLockType.Writer, _timeout))
            {
                _objectKeys.Remove(objectKey);

                //get the related keys 
                var relatedKeys = CloneList(_objectKeysRelatedKeys[objectKey]);
                foreach (var relatedKey in relatedKeys)
                {
                    if (_objectKeysRelatedKeys.ContainsKey(objectKey))
                    {
                        _objectKeysRelatedKeys[objectKey].RemoveAll(x => x == relatedKey);
                    }

                    if (_relatedKeys.ContainsKey(relatedKey))
                    {
                        _relatedKeys[relatedKey].RemoveAll(x => x == objectKey);
                    }

                    if (!_relatedKeys[relatedKey].Any())
                        _relatedKeys.Remove(relatedKey);
                }


                _objectKeysRelatedKeys.Remove(objectKey);
            }
        }

        public void RemoveRelatedKey(string relatedKey)
        {
            using (new ReaderWriterLock(_readerWriterLockRelatedKeys, ReaderWriterLockType.Writer, _timeout))
            {
                foreach (var objectKey in _relatedKeys[relatedKey])
                {
                    _objectKeys.RemoveAll(x => x == objectKey);
                }
                _relatedKeys.Remove(relatedKey);
            }
        }
    }
}
