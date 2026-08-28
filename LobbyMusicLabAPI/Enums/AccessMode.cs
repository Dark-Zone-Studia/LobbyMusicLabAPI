//using LabApi.Features.Console;
//using LabApi.Features.Wrappers;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LobbyMusicLabAPI.Enums
//{
//    public enum RunMode
//    {
//        Full,
//        Limited,
//        Blocked
//    }
//    public static class AccessMode
//    {
//        public static RunMode Resolve()
//        {
//            var ip = Server.IpAddress;
//            var cfg = Main.Instance.Config;

//            if (cfg == null) return RunMode.Limited;

//            if (string.IsNullOrWhiteSpace(ip))
//            {
//                Logger.Warn("IP not resolved yet; defaulting to Limited for now.");
//                return RunMode.Limited;
//            }

//            if (cfg.BlackListedIP.Contains(ip)) return RunMode.Blocked;
//            if (cfg.AllowedIP.Contains(ip))     return RunMode.Full;

//            return RunMode.Limited;
//        }
//    }
//}
