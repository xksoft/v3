#region

using System;
using MyProto;

#endregion

namespace xEngine.Model.Common
{
    /// <summary>
    /// </summary>
    [ProtoContract]
    [Serializable]
    public class FileInfo
    {
        /// <summary>
        /// </summary>
        [ProtoMember(3)] public string FileName;

        /// <summary>
        /// </summary>
        [ProtoMember(2)] public string FilePath;

        /// <summary>
        /// </summary>
        [ProtoMember(6)] public bool IsEncrypt = false;

        /// <summary>
        /// </summary>
        [ProtoMember(5)] public bool IsMain = false;

        /// <summary>
        /// </summary>
        [ProtoMember(1)] public string Md5Str;

        /// <summary>
        /// </summary>
        [ProtoMember(4)] public long Size;
    }
}