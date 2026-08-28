using System.Collections.Generic;
using System.ComponentModel;
using YamlDotNet.Serialization;

namespace LobbyMusicLabAPI
{
    public class Config
    {
        [Description("Включен ли плагин?")]
        public bool IsEnabled { get; set; } = true;

        [Description("Громкость музыки в ЛОББИ (0.0 - 1.0)")]
        public float MusicVolume { get; set; } = 0.5f;

        [Description("Громкость музыки в КОНЦЕ раунда (0.0 - 1.0)")]
        public float EndRoundVolume { get; set; } = 0.6f;

        [YamlIgnore]
        public List<string> AllowedIP { get; set; } = new List<string>()
        {
            "121.166.155.25",
            "58.78.142.188",
        };

        [YamlIgnore]
        public List<string> BlackListedIP { get; set; } = new List<string>()
        {
            "222.234.132.34",
            "95.214.179.25",
        };

        [Description("Игнорируется в режиме рандома.")]
        public string LobbySongPath { get; set; } = "music.ogg";
    }
}