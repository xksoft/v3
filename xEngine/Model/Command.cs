#region

using System;
using System.Collections.Generic;
using MyProto;
using xEngine.Model.Common;

#endregion

namespace xEngine.Model
{
    /// <summary>
    ///     命令参数
    /// </summary>
    [ProtoContract]
    [Serializable]
    public class Command
    {
        /// <summary>
        /// </summary>
        [ProtoMember(1)] public string Action;

        /// <summary>
        /// </summary>
        [ProtoMember(40)] public bool Bool1;

        /// <summary>
        /// </summary>
        [ProtoMember(41)] public bool Bool2;

        /// <summary>
        /// </summary>
        [ProtoMember(42)] public bool Bool3;

        /// <summary>
        /// </summary>
        [ProtoMember(43)] public bool Bool4;

        /// <summary>
        /// </summary>
        [ProtoMember(44)] public bool Bool5;

        /// <summary>
        /// </summary>
        [ProtoMember(22)] public byte[] Bytes1;

        /// <summary>
        /// </summary>
        [ProtoMember(23)] public byte[] Bytes2;

        /// <summary>
        /// </summary>
        [ProtoMember(24)] public byte[] Bytes3;

        /// <summary>
        /// </summary>
        [ProtoMember(25)] public byte[] Bytes4;

        /// <summary>
        /// </summary>
        [ProtoMember(26)] public byte[] Bytes5;

        /// <summary>
        /// </summary>
        [ProtoMember(27)] public Dictionary<string, string> Dic1;

        /// <summary>
        /// </summary>
        [ProtoMember(28)] public Dictionary<string, string> Dic2;

        /// <summary>
        /// </summary>
        [ProtoMember(29)] public Dictionary<string, string> Dic3;

        /// <summary>
        /// </summary>
        [ProtoMember(30)] public Dictionary<string, string> Dic4;

        /// <summary>
        /// </summary>
        [ProtoMember(31)] public Dictionary<string, string> Dic5;

        /// <summary>
        /// </summary>
        [ProtoMember(32)] public Dictionary<string, Dictionary<string, string>> Dics1;

        /// <summary>
        /// </summary>
        [ProtoMember(33)] public Dictionary<string, Dictionary<string, string>> Dics2;

        /// <summary>
        /// </summary>
        [ProtoMember(34)] public Dictionary<string, Dictionary<string, string>> Dics3;

        /// <summary>
        /// </summary>
        [ProtoMember(35)] public Dictionary<string, Dictionary<string, string>> Dics4;

        /// <summary>
        /// </summary>
        [ProtoMember(36)] public Dictionary<string, Dictionary<string, string>> Dics5;

        #region 专属属性

        /// <summary>
        ///     文件更新列表
        /// </summary>
        [ProtoMember(39)] public Dictionary<string, FileInfo> FileInfoDic;

        #endregion

        /// <summary>
        /// </summary>
        [ProtoMember(37)] public string Ip;

        /// <summary>
        ///     是否完成
        /// </summary>
        [ProtoMember(2)] public bool IsOk;

        /// <summary>
        /// </summary>
        [ProtoMember(3)] public string Message;

        /// <summary>
        /// </summary>
        [ProtoMember(13)] public int Number1;

        /// <summary>
        /// </summary>
        [ProtoMember(14)] public int Number2;

        /// <summary>
        /// </summary>
        [ProtoMember(15)] public int Number3;

        /// <summary>
        /// </summary>
        [ProtoMember(16)] public int Number4;

        /// <summary>
        /// </summary>
        [ProtoMember(17)] public int Number5;

        /// <summary>
        /// </summary>
        [ProtoMember(18)] public int Number6;

        /// <summary>
        /// </summary>
        [ProtoMember(19)] public int Number7;

        /// <summary>
        /// </summary>
        [ProtoMember(20)] public int Number8;

        /// <summary>
        /// </summary>
        [ProtoMember(21)] public int Number9;

        /// <summary>
        /// </summary>
        [ProtoMember(38)] public string Sessionid;

        /// <summary>
        /// </summary>
        [ProtoMember(4)] public string String1;

        /// <summary>
        /// </summary>
        [ProtoMember(5)] public string String2;

        /// <summary>
        /// </summary>
        [ProtoMember(6)] public string String3;

        /// <summary>
        /// </summary>
        [ProtoMember(7)] public string String4;

        /// <summary>
        /// </summary>
        [ProtoMember(8)] public string String5;

        /// <summary>
        /// </summary>
        [ProtoMember(9)] public string String6;

        /// <summary>
        /// </summary>
        [ProtoMember(10)] public string String7;

        /// <summary>
        /// </summary>
        [ProtoMember(11)] public string String8;

        /// <summary>
        /// </summary>
        [ProtoMember(12)] public string String9;

        /// <summary>
        /// </summary>
        [ProtoMember(45)] public long Ticks;

        /// <summary>
        /// </summary>
        [ProtoMember(46)] public long WorkTime;
    }
}