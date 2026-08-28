using CommandSystem;
using System;

namespace LobbyMusicLabAPI.Commands
{
    public class MusicList : ICommand
    {
        public string Command => throw new NotImplementedException();

        public string[] Aliases => throw new NotImplementedException();

        public string Description => throw new NotImplementedException();

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            throw new NotImplementedException();
        }
    }
}