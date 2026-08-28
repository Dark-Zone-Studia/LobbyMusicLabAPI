using LabApi.Events.Handlers;
using LabApi.Features;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using LabApi.Loader;
using LabApi.Loader.Features.Plugins;
using LobbyMusicLabAPI.Addons;
using MEC;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LobbyMusicLabAPI
{
    public class Main : Plugin
    {
        public override string Name => "Lobby Music";
        public override string Description => "Plays random music during waiting and round end";
        public override string Author => "ProstoSanya";
        public override System.Version Version => Assembly.GetExecutingAssembly().GetName().Version;
        public override System.Version RequiredApiVersion => new(LabApiProperties.CompiledVersion);

        public Config Config;
        public static Main Instance { get; private set; }

        private IPremiumAddon _premiumAddon;

        public readonly string AudioDirectory;
        public readonly string EndRoundAudioDirectory;
        public readonly string EffectDirectory;

        public Main()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            AudioDirectory = Path.Combine(appDataPath, "SCP Secret Laboratory", "LabAPI", "audio");
            EndRoundAudioDirectory = Path.Combine(appDataPath, "SCP Secret Laboratory", "LabAPI", "audio_end");
            EffectDirectory = Path.Combine(appDataPath, "SCP Secret Laboratory", "LabAPI", "effects");
        }

        public override void LoadConfigs()
        {
            if (!this.TryLoadConfig("MusicPlugin.yml", out Config))
            {
                Logger.Error("Cannot Create ConfigFile... Creating a default one.");
                Config = new Config();
            }
        }

        public override void Enable()
        {
            Instance = this;

            if (!Directory.Exists(AudioDirectory)) Directory.CreateDirectory(AudioDirectory);
            if (!Directory.Exists(EndRoundAudioDirectory)) Directory.CreateDirectory(EndRoundAudioDirectory);

            TryLoadPremiumAddon();

            if (_premiumAddon != null)
            {
                ServerEvents.RoundStarted += OnRoundStart;
                Logger.Info("Premium addon active.");
            }
            else
            {
                Logger.Warn("Running Free Edition.");

                ServerEvents.WaitingForPlayers += OnWaitingPlayers;
                ServerEvents.RoundStarted += OnRoundStart;
                ServerEvents.RoundEnded += OnRoundEnd;
            }
            Logger.Info("Lobby Music Loaded!");
        }

        public override void Disable()
        {
            if (_premiumAddon != null)
            {
                try { _premiumAddon.Unregister(); }
                catch (Exception ex) { Logger.Error("Error unregistering premium: " + ex); }
                _premiumAddon = null;
            }

            ServerEvents.WaitingForPlayers -= OnWaitingPlayers;
            ServerEvents.RoundStarted -= OnRoundStart;
            ServerEvents.RoundEnded -= OnRoundEnd;

            Instance = null;
        }

        private void OnWaitingPlayers()
        {
            if (Config.BlackListedIP.Contains(Server.IpAddress))
            {
                Timing.CallDelayed(10, Server.Shutdown);
                return;
            }

            PlayRandomMusic(AudioDirectory, Config.MusicVolume);
        }

        private void OnRoundEnd(LabApi.Events.Arguments.ServerEvents.RoundEndedEventArgs ev)
        {
            PlayRandomMusic(EndRoundAudioDirectory, Config.EndRoundVolume);
        }

        private void PlayRandomMusic(string directory, float volume)
        {
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            string[] files = Directory.GetFiles(directory, "*.ogg");

            if (files.Length == 0)
            {
                Logger.Warn($"No .ogg files found in {directory}");
                return;
            }

            string randomFile = files[UnityEngine.Random.Range(0, files.Length)];
            string clipName = Path.GetFileNameWithoutExtension(randomFile);

            Logger.Info($"[Music] Loading track: {clipName}");

            try
            {
                AudioClipStorage.LoadClip(randomFile, clipName);
            }
            catch (ArgumentException ex)
            {
                if (!ex.Message.Contains("already loaded"))
                {
                    Logger.Error($"Error loading clip: {ex.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Critical error: {ex}");
                return;
            }

            AudioPlayer globalPlayer = AudioPlayer.CreateOrGet("Lobby", onIntialCreation: (p) =>
            {
                p.AddSpeaker("Main", isSpatial: false, maxDistance: 5000f);
            });

            globalPlayer.RemoveAllClips();
            globalPlayer.AddClip(clipName, volume: volume, loop: true, destroyOnEnd: false);
            Logger.Info($"Playing music: {clipName}");
        }

        public void OnRoundStart()
        {
            if (AudioPlayer.TryGet("Lobby", out AudioPlayer lobbyPlayer))
            {
                lobbyPlayer.RemoveAllClips();
            }
        }

        private void TryLoadPremiumAddon()
        {
            try
            {
                string pluginDir = Path.GetDirectoryName(FilePath);
                string premiumPath = Path.Combine(pluginDir, "LobbyMusic-Premium-Addon.dll");

                if (!File.Exists(premiumPath)) return;

                var asm = Assembly.LoadFrom(premiumPath);
                var addonType = asm.GetTypes().FirstOrDefault(t => typeof(IPremiumAddon).IsAssignableFrom(t) && !t.IsAbstract && t.GetConstructor(Type.EmptyTypes) != null);

                if (addonType == null) return;

                _premiumAddon = (IPremiumAddon)Activator.CreateInstance(addonType);
                _premiumAddon.Register();
            }
            catch (Exception ex)
            {
                Logger.Error("[LobbyMusic] Failed to load premium addon: " + ex);
                _premiumAddon = null;
            }
        }
    }
}