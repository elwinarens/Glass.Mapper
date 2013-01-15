using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Glass.Mapper.Caching.ObjectCaching
{
    public class CacheInformation
    {
        private readonly int _timeout = 10000;
        private readonly ReaderWriterLockSlim _readerWriterLock = new ReaderWriterLockSlim();
        
        private volatile Dictionary<string, List<string>> _relatedKeys;

        private volatile List<string> _objectKeys;

        private volatile Dictionary<string, List<string>> _objectKeysRelatedKeys;

        public CacheInformation()
        {
            _relatedKeys = new Dictionary<string, List<string>>();
            _objectKeysRelatedKeys = new Dictionary<string, List<string>>();
            _objectKeys = new List<string>();
        }

        public CacheInformation(int timeout)
            : this()
        {
            _timeout = timeout;
        }


        public void AddRelatedKey(string releatedKey)
        {
            if (!ContainsRepeatedKey(releatedKey))
            {
                InternalAddRepeatedKey(releatedKey);
            }
        }

        private void InternalAddRepeatedKey(string relatedKey)
        {
            using (var rel = new ReaderWriterLock(_readerWriterLock, ReaderWriterLockType.Writer, _timeout))
            {
                if (rel.HasTakenLock)
                {
                    _relatedKeys.Add(relatedKey, new List<string>());
                }
            }
        }

        private bool ContainsRepeatedKey(string releatedKey)
        {
            using (var rel = new ReaderWriterLock(_readerWriterLock, ReaderWriterLockType.Reader, _timeout))
            {
                if (rel.HasTakenLock)
                {
                    return _relatedKeys.ContainsKey(releatedKey);
                }
            }
            return false;
        }

        private void AddRelatedKayAndObjectKey(string releatedKey, string objectKey)
        {
            using (var rel = new ReaderWriterLock(_readerWriterLock, ReaderWriterLockType.Writer, _timeout))
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
            if (ContainsRepeatedKey(releatedKey))
            {
                using (var rel = new ReaderWriterLock(_readerWriterLock, ReaderWriterLockType.Writer, _timeout))
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
            using (var rel = new ReaderWriterLock(_readerWriterLock, ReaderWriterLockType.Writer, _timeout))
            {
                if (rel.HasTakenLock)
                {
                    keyList = CloneList(this._relatedKeys.Keys.ToList());
                }
            }

            return keyList;
        }

        private List<string> CloneList(IEnumerable<string> list)
        {
            return new List<string>(list);
        }

        public System.Collections.ICollection Keys
        {
            get
            {
                var returnKeyList = new List<string>();
                using (var rel = new ReaderWriterLock(_readerWriterLock, ReaderWriterLockType.Writer, _timeout))
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
            using (var rel = new ReaderWriterLock(_readerWriterLock, ReaderWriterLockType.Writer, _timeout))
            {
                if (!rel.HasTakenLock) return;

                foreach (var releatedKey in releatedKeys)
                {
                    if (ContainsRepeatedKey(releatedKey))
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
            using (new ReaderWriterLock(_readerWriterLock, ReaderWriterLockType.Writer, _timeout))
            {
                returnList = new List<string>(_relatedKeys[releatedKey]);
            }

            return returnList;
        }

        /// <summary>
        /// Removes the object key.
        /// </summary>
        /// <param name="objectKey">The object key.</param>
        public void RemoveObjectKey(string objectKey)
        {
            using (var rel = new ReaderWriterLock(_readerWriterLock, ReaderWriterLockType.Writer, _timeout))
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

                    if (this._relatedKeys.ContainsKey(relatedKey))
                    {
                        this._relatedKeys[relatedKey].RemoveAll(x => x == objectKey);
                    }

                    if (!this._relatedKeys[relatedKey].Any())
                        this._relatedKeys.Remove(relatedKey);
                }


                _objectKeysRelatedKeys.Remove(objectKey);
            }
        }

        public void RemoveRelatedKey(string relatedKey)
        {
            using (new ReaderWriterLock(_readerWriterLock, ReaderWriterLockType.Writer, _timeout))
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
