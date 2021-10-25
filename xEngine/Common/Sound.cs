#region

using System.IO;
using System.Media;

#endregion

namespace xEngine.Common
{
    /// <summary>
    ///     声音帮助类
    /// </summary>
    public class Sound
    {
        /// <summary>
        /// </summary>
        /// <param name="stream"></param>
        public static void PlaySound(Stream stream)
        {
            try
            {
                var sndPlayer = new SoundPlayer(stream);
                sndPlayer.Play();
                sndPlayer.Dispose();
            }
            catch
            {
            }
        }
    }
}