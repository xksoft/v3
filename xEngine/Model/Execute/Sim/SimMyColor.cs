#region

using System;
using MyProto;

#endregion

namespace xEngine.Model.Execute.Sim
{
    /// <summary>
    /// </summary>
    [ProtoContract]
    [Serializable]
    public class SimMyColor
    {
        [ProtoMember(3)] public int B;
        [ProtoMember(2)] public int G;
        [ProtoMember(1)] public int R;

        /// <summary>
        /// </summary>
        public SimMyColor()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public SimMyColor(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }
    }
}