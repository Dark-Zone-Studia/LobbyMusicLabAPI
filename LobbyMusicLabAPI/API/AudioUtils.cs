using NVorbis;
using System.IO;

namespace LobbyMusicLabAPI.API
{
    public class AudioUtils
    {
        public static float GetOggDurationInSeconds(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("CANNOT FIND AUDIO FILE.", filePath);
            }

            using (var stream = File.OpenRead(filePath))
            using (var vorbis = new VorbisReader(stream, false))
            {
                double duration = vorbis.TotalTime.TotalSeconds;
                return (float)duration;
            }
        }
    }
}
