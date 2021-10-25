#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

#endregion

namespace xEngine.Common
{
    /// <summary>
    ///     侠客LRU缓存
    /// </summary>
    /// <typeparam name="TK"></typeparam>
    /// <typeparam name="TV"></typeparam>
    public class XiakeLruCache<TK, TV>
    {
        private readonly Dictionary<TK, TV> _dict;
        private readonly Dictionary<TK, long> _queue;
        private readonly int _removeTime;
        private readonly object _syncRoot = new object();
        private readonly Dictionary<TK, long> _timerDirectory;
        private long _tick;

        /// <summary>
        ///     获取最大缓存长度
        /// </summary>
        public int CacheLength;

        /// <summary>
        ///     初始化缓存器
        /// </summary>
        /// <param name="max">最大大小</param>
        /// <param name="removeTime">自动移除缓存时间(单位：秒）</param>
        public XiakeLruCache(int max, int removeTime = 0)
        {
            _dict = new Dictionary<TK, TV>();
            _queue = new Dictionary<TK, long>();
            _timerDirectory = new Dictionary<TK, long>();
            CacheLength = max;
            _removeTime = removeTime;
            if (_removeTime > 0)
            {
                var autoRemoveTask = new Thread(Autoremove) {IsBackground = true};
                autoRemoveTask.Start();
            }
        }

        public Dictionary<TK, TV> GetDic()
        {
            lock (_syncRoot)
            {
                return _dict;
            }
        }

        private void Autoremove()
        {
            while (_removeTime > 0)
            {
                Thread.Sleep(_removeTime/2*1000);

                var nowtick = DateTime.Now.Ticks;
                lock (_syncRoot)
                {
                    var removelist = (from l in _timerDirectory
                        where nowtick - l.Value > _removeTime*10000000
                        select l).ToList();

                    foreach (var keyValuePair in removelist.Where(keyValuePair => _dict.ContainsKey(keyValuePair.Key)))
                    {
                        _dict.Remove(keyValuePair.Key);
                        _queue.Remove(keyValuePair.Key);
                        _timerDirectory.Remove(keyValuePair.Key);
                    }
                }
            }
        }

        /// <summary>
        ///     判断是否包含键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TK key)
        {
            return _dict.ContainsKey(key);
        }

        /// <summary>
        ///     获取缓存数量
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return _dict.Count;
        }

        /// <summary>
        ///     清空缓存
        /// </summary>
        public void ClaerAll()
        {
            lock (_syncRoot)
            {
                _dict.Clear();
                _queue.Clear();
                _timerDirectory.Clear();
            }
        }

        /// <summary>
        ///     添加缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TK key, TV value)
        {
            lock (_syncRoot)
            {
                if (key == null)
                    return;
                CheckAndTruncate();
                if (_dict.ContainsKey(key))
                {
                    _dict[key] = value;
                    _queue.Remove(key);
                    _tick++;
                    _queue.Add(key, _tick);
                }
                else
                {
                    _tick++;
                    _queue.Add(key, _tick);
                    _dict[key] = value;
                }

                if (_removeTime > 0)
                {
                    if (_timerDirectory.ContainsKey(key))
                    {
                        _timerDirectory[key] = DateTime.Now.Ticks;
                    }
                    else
                    {
                        _timerDirectory.Add(key, DateTime.Now.Ticks);
                    }
                }
            }
        }

        private void CheckAndTruncate()
        {
            lock (_syncRoot)
            {
                var count = _dict.Count;
                if (count >= CacheLength)
                {
                    var needremovekv = (from d in _queue
                        where d.Value < _tick - count*0.9
                        select d).ToArray();

                    foreach (var keyValuePair in needremovekv)
                    {
                        _dict.Remove(keyValuePair.Key);
                        _queue.Remove(keyValuePair.Key);
                        if (_removeTime > 0)
                        {
                            _timerDirectory.Remove(keyValuePair.Key);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     删除缓存
        /// </summary>
        /// <param name="key"></param>
        public void Delete(TK key)
        {
            lock (_syncRoot)
            {
                if (_dict.ContainsKey(key))
                {
                    _dict.Remove(key);
                    _queue.Remove(key);
                    if (_removeTime > 0)
                    {
                        _timerDirectory.Remove(key);
                    }
                }
            }
        }

        /// <summary>
        ///     获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TV Get(TK key)
        {
            lock (_syncRoot)
            {
                if (key == null)
                    return default(TV);
                TV ret;
                if (!_dict.TryGetValue(key, out ret)) return ret;
                _tick++;
                _queue[key] = _tick;
                return ret;
            }
        }

        /// <summary>
        ///     移除缓存
        /// </summary>
        /// <param name="key"></param>
        public void Remove(TK key)
        {
            lock (_syncRoot)
            {
                if (_dict.ContainsKey(key))
                    _dict.Remove(key);
                if (_queue.ContainsKey(key))
                    _queue.Remove(key);
                if (_timerDirectory.ContainsKey(key))
                    _timerDirectory.Remove(key);
            }
        }
    }
}