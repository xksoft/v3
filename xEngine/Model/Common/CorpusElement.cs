#region

using MyProto;

#endregion

namespace xEngine.Model.Common
{
    [ProtoContract]
    public class CorpusElement
    {
        [ProtoMember(1)]
        public string content { get; set; }

        [ProtoMember(2)]
        public CEType type { get; set; }

        [ProtoMember(3)]
        public int len { get; set; }
    }

    [ProtoContract]
    public enum CEType
    {
        juhao,
        douhao,
        wenhao,
        gantanhao,
        qita
    }
}