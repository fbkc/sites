using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoSend
{
    public class CommonToken
    {
        public static string SecretKey = "This is a private key for Server";//这个服务端加密秘钥 属于私钥

        public static string GenToken(TokenInfo M)
        {
            var jwtcreated =
               Math.Round((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds + 5);
            var jwtcreatedOver =
            Math.Round((DateTime.UtcNow.AddHours(2) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds + 5);
            var payload = new Dictionary<string, dynamic>
                {
                    {"iss", M.iss},//非必须。issuer 请求实体，可以是发起请求的用户的信息，也可是jwt的签发者。
                    {"iat", jwtcreated},//非必须。issued at。 token创建时间，unix时间戳格式
                    {"exp", jwtcreatedOver},//非必须。expire 指定token的生命周期。unix时间戳格式
                    {"aud", M.aud},//非必须。接收该JWT的一方。
                    {"sub", M.sub},//非必须。该JWT所面向的用户
                    {"jti", M.jti},//非必须。JWT ID。针对当前token的唯一标识
                    {"UserName", M.UserName},//自定义字段 用于存放当前登录人账户信息
                    {"UserPwd", M.UserPwd},//自定义字段 用于存放当前登录人登录密码信息
                    {"UserRole", M.UserRole},//自定义字段 用于存放当前登录人登录权限信息
                };
            return JWT.JsonWebToken.Encode(payload, SecretKey,
                JWT.JwtHashAlgorithm.HS256);
        }
    }

    public class TokenInfo
    {
        public TokenInfo()
        {
            iss = "签发者信息";
            aud = "http://example.com";
            sub = "HomeCare.VIP";
            jti = DateTime.Now.ToString("yyyyMMddhhmmss");
            UserName = "jack.chen";
            UserPwd = "jack123456";
            UserRole = "HomeCare.Administrator";
        }
        //
        public string iss { get; set; }
        public string aud { get; set; }
        public string sub { get; set; }
        public string jti { get; set; }
        public string UserName { get; set; }
        public string UserPwd { get; set; }
        public string UserRole { get; set; }
    }
}