#region

using System;
using System.Collections.Generic;
using MyProto;

#endregion

namespace xEngine.Model.Execute.Rules
{
    /// <summary>
    /// </summary>
    [ProtoContract]
    [Serializable]
    public class RulesMain
    {
        /// <summary>
        ///     插入尾部
        /// </summary>
        [ProtoMember(10)] public string InsertFootStr;

        /// <summary>
        ///     插入头部
        /// </summary>
        [ProtoMember(9)] public string InsertHeadStr;

        /// <summary>
        ///     是不是提取文本，否则是提取链接
        /// </summary>
        [ProtoMember(3)] public bool IsGetText;

        /// <summary>
        ///     合并间隔符
        /// </summary>
        [ProtoMember(5)] public string MergerStr = "<BR>";

        /// <summary>
        ///     结果必须包含的字符
        /// </summary>
        [ProtoMember(7)] public string MustHaveStr;

        /// <summary>
        ///     结果必须不包含
        /// </summary>
        [ProtoMember(8)] public string MustNoHaveStr;

        /// <summary>
        ///     提取方案名
        /// </summary>
        [ProtoMember(1)] public string Name = "未命名提取方案";

        /// <summary>
        ///     输出模式 1，合并所有结果 2，多结果输出 3，取大 4，包含特征字符的
        /// </summary>
        [ProtoMember(4)] public int OutModel = 1;

        /// <summary>
        ///     规则
        /// </summary>
        [ProtoMember(11)] public List<RulesChild> Rules = new List<RulesChild>();

        /// <summary>
        ///     规则名
        /// </summary>
        [ProtoMember(2)] public string RulesName = "未命名规则";

        /// <summary>
        ///     测试地址
        /// </summary>
        [ProtoMember(6)] public string TestUrl = "http://www.baidu.com";
    }
}