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
    public class SimMyPoint
    {
        [ProtoMember(1)] public int X;
        [ProtoMember(2)] public int Y;

        /// <summary>
        /// </summary>
        public SimMyPoint()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public SimMyPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}