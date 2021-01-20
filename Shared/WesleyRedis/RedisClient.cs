using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading;

namespace WesleyRedis
{
    /// <summary>
    /// 缓存
    /// </summary>
    public class RedisClient
    {
        private static readonly object Locker = new object();

        private ConnectionMultiplexer redisMultiplexer;

        private IDatabase db = null;

        private static RedisClient _redisClient = null;

        /// <summary>
        /// 链接
        /// </summary>
        public static RedisClient RedisCt
        {
            get
            {
                if (_redisClient == null)
                {
                    lock (Locker)
                    {
                        if (_redisClient == null)
                        {
                            _redisClient = new RedisClient();
                        }
                    }
                }
                return _redisClient;
            }
        }

        /// <summary>
        /// 读取配置
        /// </summary>
        /// <param name="Configuration"></param>
        public void InitConnect(IConfiguration Configuration)
        {
            try
            {
                var RedisConnection = Configuration.GetSection("RedisCache");
                redisMultiplexer = ConnectionMultiplexer.Connect(RedisConnection["ConnectionStrings"]);
                //redis集群
                //redisMultiplexer = ConnectionMultiplexer.Connect("localhost:6000,localhost:6001,localhost:6002,localhost:6003,localhost:6004,localhost:6005");
                db = redisMultiplexer.GetDatabase();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                redisMultiplexer = null;
                db = null;
            }
        }

        #region String

        /// <summary>
        /// 移除单个key value
        /// </summary>
        /// <param name="key">保存的值</param>
        public bool KeyDelete(string key)
        {
            return db.KeyDelete(key);
        }

        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="key">保存的值</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        public bool SetStringKey(string key, string value, TimeSpan? expiry = default)
        {
            return db.StringSet(key, value, expiry);
        }

        /// <summary>
        /// 获取单个key的值
        /// </summary>
        public RedisValue GetStringKey(string key)
        {
            return db.StringGet(key);
        }

        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetStringKey<T>(string key)
        {
            if (db == null)
            {
                return default;
            }
            var value = db.StringGet(key);
            if (value.IsNullOrEmpty)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool SetStringKey<T>(string key, T obj, TimeSpan? expiry = default)
        {
            if (db == null)
            {
                return false;
            }
            string json = JsonConvert.SerializeObject(obj);
            return db.StringSet(key, json, expiry);
        }

        #endregion String

        #region 分布式锁

        /// <summary>
        /// 加锁
        /// </summary>
        /// <param name="key">锁名称</param>
        /// <param name="value">产品id 订单id等关键唯一主键</param>
        public void Lock(string key, string value)
        {
            try
            {
                while (true)
                {
                    //var flag = db.LockTake("redis_lock", Thread.CurrentThread.ManagedThreadId, TimeSpan.FromMinutes(30));
                    var flag = db.LockTake(key, value, TimeSpan.FromMinutes(30));
                    //如果枷锁成功
                    if (flag)
                    {
                        break;
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 解锁
        /// </summary>
        /// <param name="key">锁名称</param>
        /// <param name="value">产品id 订单id等关键唯一主键</param>
        public void UnLock(string key, string value)
        {
            try
            {
                //解锁
                //使枷锁和解锁的线程是同一个
                while (true)
                {
                    //var flag = db.LockRelease("redis_lock", Thread.CurrentThread.ManagedThreadId);
                    var flag = db.LockRelease(key, value);
                    if (flag)
                    {
                        break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion 分布式锁
    }
}