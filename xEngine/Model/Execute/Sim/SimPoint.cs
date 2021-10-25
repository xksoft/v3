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
    public class SimPoint
    {
        /// <summary>
        /// </summary>
        [ProtoMember(4)] public SimMyColor Color1 = new SimMyColor();

        /// <summary>
        /// </summary>
        [ProtoMember(5)] public SimMyColor Color2 = new SimMyColor();

        /// <summary>
        /// </summary>
        [ProtoMember(6)] public SimMyColor Color3 = new SimMyColor();

        /// <summary>
        /// </summary>
        [ProtoMember(1)] public SimMyPoint Position1 = new SimMyPoint();

        /// <summary>
        /// </summary>
        [ProtoMember(2)] public SimMyPoint Position2 = new SimMyPoint();

        /// <summary>
        /// </summary>
        [ProtoMember(3)] public SimMyPoint Position3 = new SimMyPoint();

        /// <summary>
        /// </summary>
        [ProtoMember(12)] public SimMyPoint ReturnPoint1 = new SimMyPoint();

        /// <summary>
        /// </summary>
        [ProtoMember(13)] public SimMyPoint ReturnPoint2 = new SimMyPoint();

        /// <summary>
        /// </summary>
        [ProtoMember(14)] public SimMyPoint ReturnPoint3 = new SimMyPoint();

        /// <summary>
        /// </summary>
        [ProtoMember(15)] public SimMyPoint ReturnPoint4 = new SimMyPoint();

        /// <summary>
        /// </summary>
        [ProtoMember(16)] public SimMyPoint ReturnPoint5 = new SimMyPoint();

        /// <summary>
        /// </summary>
        [ProtoMember(7)] public SimMyPoint SelectPoint1 = new SimMyPoint();

        /// <summary>
        /// </summary>
        [ProtoMember(8)] public SimMyPoint SelectPoint2 = new SimMyPoint();

        /// <summary>
        /// </summary>
        [ProtoMember(9)] public SimMyPoint SelectPoint3 = new SimMyPoint();

        /// <summary>
        /// </summary>
        [ProtoMember(10)] public SimMyPoint SelectPoint4 = new SimMyPoint();

        /// <summary>
        /// </summary>
        [ProtoMember(11)] public SimMyPoint SelectPoint5 = new SimMyPoint();
    }
}