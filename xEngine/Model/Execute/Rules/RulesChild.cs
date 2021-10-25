#region

using System;
using MyProto;

#endregion

namespace xEngine.Model.Execute.Rules
{
    /// <summary>
    /// </summary>
    [ProtoContract]
    [Serializable]
    public class RulesChild
    {
        /// <summary>
        ///     要提取的属性名
        /// </summary>
        [ProtoMember(3)] public string AttributeName;

        /// <summary>
        ///     提取模式
        /// </summary>
        [ProtoMember(2)] public GetMethod Method = GetMethod.GetOuter;

        /// <summary>
        ///     规则描述
        /// </summary>
        [ProtoMember(4)] public string Name = "未命名子规则";

        /// <summary>
        ///     提取规则
        /// </summary>
        [ProtoMember(1)] public string Rulesstr;
    }

    public enum GetMethod
    {
        GetInner = 1,
        GetOuter = 2,
        GetAttribute = 3,
        Regex = 4
    }
}