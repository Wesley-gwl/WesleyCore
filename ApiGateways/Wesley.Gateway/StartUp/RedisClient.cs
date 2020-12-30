using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WesleyPC.Gateway
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
        public static RedisClient redisClient
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
        /// 保存单个key value
        /// </summary>
        /// <param name="key">保存的值</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        public bool SetStringKey(string key, string value, TimeSpan? expiry = default(TimeSpan?))
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
        public bool SetStringKey<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?))
        {
            if (db == null)
            {
                return false;
            }
            string json = JsonConvert.SerializeObject(obj);
            return db.StringSet(key, json, expiry);
        }

        #endregion String
    }
}