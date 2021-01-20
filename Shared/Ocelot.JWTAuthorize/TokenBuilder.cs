using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Ocelot.JwtAuthorize
{
    /// <summary>
    /// TokenBuilder
    /// </summary>
    public class TokenBuilder : ITokenBuilder
    {
        /// <summary>
        /// JwtAuthorizationRequirement
        /// </summary>
        private readonly JwtAuthorizationRequirement _jwtAuthorizationRequirement;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="jwtAuthorizationRequirement"></param>
        public TokenBuilder(JwtAuthorizationRequirement jwtAuthorizationRequirement)
        {
            _jwtAuthorizationRequirement = jwtAuthorizationRequirement;
        }

        /// <summary>
        /// generate token
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="notBefore"></param>
        /// <param name="expires"></param>
        /// <param name="ip"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Token BuildJwtToken(Claim[] claims, DateTime? notBefore = null, DateTime? expires = null, TokenType type = TokenType.PC)
        {
            var now = notBefore ?? DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: _jwtAuthorizationRequirement.Issuer,
                audience: _jwtAuthorizationRequirement.Audience,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: _jwtAuthorizationRequirement.SigningCredentials
            );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var responseJson = new Token
            {
                TokenValue = encodedJwt,
                Expires = expires,
                TokenType = type
            };
            return responseJson;
        }

        /// <summary>
        /// 令牌类型
        /// </summary>
        public enum TokenType
        {
            PC,
            App,
            WX
        }

        /// <summary>
        /// back token
        /// </summary>
        public class Token
        {
            /// <summary>
            /// Token Value
            /// </summary>
            public string TokenValue { get; set; }

            /// <summary>
            /// Expires (unit second)
            /// </summary>
            public DateTime? Expires { get; set; }

            /// <summary>
            /// token type
            /// </summary>
            public TokenType TokenType { get; set; }

            /// <summary>
            /// type Name
            /// </summary>
            public string TokenTypeName { get { return TokenType.ToString(); } }
        }
    }
}