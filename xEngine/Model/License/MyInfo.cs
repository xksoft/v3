#region

using System;
using MyProto;

#endregion

namespace xEngine.Model.License
{
    /// <summary>
    ///     授权用户信息
    /// </summary>
    [ProtoContract]
    [Serializable]
    public class MyInfo
    {
        /// <summary>
        /// </summary>
        [ProtoMember(14)] public string QqOpenid;

        /// <summary>
        /// </summary>
        [ProtoMember(15)] public string WeixinOpenid;

        /// <summary>
        ///     支付宝账号
        /// </summary>
        [ProtoMember(1)]
        public string Alipay { get; internal set; }

        /// <summary>
        ///     邮件地址
        /// </summary>
        [ProtoMember(2)]
        public string Email { get; internal set; }

        /// <summary>
        ///     用户头像
        /// </summary>
        [ProtoMember(3)]
        public byte[] Face { get; internal set; }

        /// <summary>
        ///     ID编号
        /// </summary>
        [ProtoMember(4)]
        public int Id { get; internal set; }

        /// <summary>
        ///     登录地区
        /// </summary>
        [ProtoMember(5)]
        public string LoginIp { get; internal set; }

        /// <summary>
        ///     登录时间
        /// </summary>
        [ProtoMember(6)]
        public DateTime LoginTime { get; internal set; }

        /// <summary>
        ///     余额
        /// </summary>
        [ProtoMember(7)]
        public double Money { get; internal set; }

        /// <summary>
        ///     名字
        /// </summary>
        [ProtoMember(8)]
        public string Name { get; internal set; }

        /// <summary>
        ///     用户密码
        /// </summary>
        [ProtoMember(9)]
        public string Password { get; internal set; }

        /// <summary>
        ///     电话号码
        /// </summary>
        [ProtoMember(10)]
        public string Phone { get; internal set; }

        /// <summary>
        ///     头像地址
        /// </summary>
        [ProtoMember(11)]
        public string Photo { get; internal set; }

        /// <summary>
        ///     权限
        /// </summary>
        [ProtoMember(12)]
        public int Power { get; internal set; }

        /// <summary>
        ///     QQ
        /// </summary>
        [ProtoMember(13)]
        public string Qq { get; internal set; }

        /// <summary>
        ///     未读消息
        /// </summary>
        [ProtoMember(14)]
        public int Newpm { get; internal set; }

        /// <summary>
        ///     我的key
        /// </summary>
        [ProtoMember(16)]
        public string MyKey { get; internal set; }
    }
}