#region

using System.Collections.Generic;
using MyProto;
using xEngine.Common;

#endregion

namespace xEngine.Model.Common
{
    [ProtoContract]
    public class KeywordDb
    {
        [ProtoMember(1)] public Dictionary<string, string> AllDictionary = new Dictionary<string, string>();
        [ProtoMember(2)] public string[] AllKeyword;
        public XiakeLruCache<string, int[]> HotKeywordCache = new XiakeLruCache<string, int[]>(5000);
        public Dictionary<string, int> IndexDic = new Dictionary<string, int>();
    }
}