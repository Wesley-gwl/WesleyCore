using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using WesleyCore.Web.Controllers;
using WesLeyCore.Const;

namespace WesleyCore.Web.Authorizer
{
    /// <summary>
    /// token加密
    /// </summary>
    public static class JWTUtil
    {
        #region JWT加密

        /// <summary>
        /// 密钥
        /// </summary>
        public static string key;

        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static string GenerateToken(AuthModel payload)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder); //加密器

            //var cache = IocManager.Instance.Resolve<ICacheManager>();
            //cache.GetCache(RedisConst.ValidUser).Set(payload.UserId.ToString(), "on", slidingExpireTime: null, absoluteExpireTime: DateTimeOffset.Now.AddHours(3));
            var token = encoder.Encode(payload, key);
            return token;
        }

        /// <summary>
        /// 生成WXtoken
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="sessionKey">缓存key</param>
        /// <returns></returns>
        public static string GenerateWXToken(AuthModel payload, string sessionKey)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder); //加密器

            //var cache = IocManager.Instance.Resolve<ICacheManager>();
            var token = encoder.Encode(payload, key);
            //cache.GetCache(RedisConst.ValidWXUser).Set(sessionKey, token, slidingExpireTime: null, absoluteExpireTime: DateTimeOffset.Now.AddHours(3));
            return token;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static AuthModel DecodeToken(string token)
        {
            IJsonSerializer serializer = new JsonNetSerializer();//序列化和反序列
            IDateTimeProvider provider = new UtcDateTimeProvider();//UTC时间获取
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();//Base64编解码
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);   //解密器

            var dic = decoder.DecodeToObject<AuthModel>(token, key, true);
            return dic;
        }

        #endregion JWT加密
    }
}