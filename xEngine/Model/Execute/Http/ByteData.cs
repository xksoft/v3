#region

using System;
using MyProto;

#endregion

namespace xEngine.Model.Execute.Http
{
    /// <summary>
    /// </summary>
    [ProtoContract]
    [Serializable]
    public class ByteData
    {
        /// <summary>
        ///     数据1
        /// </summary>
        [ProtoMember(1)] public byte[] Data1;

        /// <summary>
        ///     数据2
        /// </summary>
        [ProtoMember(2)] public byte[] Data2;

        /// <summary>
        ///     数据3
        /// </summary>
        [ProtoMember(3)] public byte[] Data3;

        /// <summary>
        ///     数据4
        /// </summary>
        [ProtoMember(4)] public byte[] Data4;

        /// <summary>
        ///     数据5
        /// </summary>
        [ProtoMember(5)] public byte[] Data5;
    }
}