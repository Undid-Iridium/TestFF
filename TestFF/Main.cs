using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.DamageHandlers;
using Exiled.Events.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerEvents = Exiled.Events.Handlers.Server;
namespace TestFF
{
    public class Main : Plugin<Config>
    {
        public Main Instance { get; private set; }

        public override PluginPriority Priority { get; } = PluginPriority.Last;

        /// <inheritdoc />
        public override string Author => "Undid-Iridium";

        /// <inheritdoc />
        public override string Name => "TestFF";

        /// <inheritdoc />
        public override Version RequiredExiledVersion { get; } = new Version(5, 1, 3);

        /// <inheritdoc />
        public override Version Version { get; } = new Version(1, 0, 1);

        public void RefreshBools()
        {
            ServerConfigSynchronizer serversync = new ServerConfigSynchronizer();
            serversync.RefreshMainBools();
        }

        public void OnShooting(ShootingEventArgs ev)
        {
            ev.Shooter.SendConsoleMessage($"FriendlyFire Enabled : {Server.FriendlyFire}", "red");
        }

        public void OnRoundStarted()
        {

            if (Instance.Config.broadcast)
            {
                foreach (Player p in Player.List)
                {
                    p.Broadcast(2, "Friendly Fire Disabled - RoundEnded", global::Broadcast.BroadcastFlags.Normal, true);
                }
            }
            Server.FriendlyFire = false;
   
            RefreshBools();
            PlayerStatsSystem.AttackerDamageHandler.RefreshConfigs();
        }


        public void OnEndingRound(RoundEndedEventArgs ev)
        {
            if (!Round.IsEnded)
            {
                return;
            }

            if (Instance.Config.broadcast)
            {
                foreach (Player p in Player.List)
                {
                    p.Broadcast(2, "Friendly Fire Enabled - RoundEnding", global::Broadcast.BroadcastFlags.Normal, true);
                }
            }
            FriendlyFireConfig.PauseDetector = true;
            Server.FriendlyFire = true;
            RefreshBools();
            PlayerStatsSystem.AttackerDamageHandler.RefreshConfigs();
        }


        /// <inheritdoc />
        public override void OnEnabled()
        {
         
            ServerEvents.RoundStarted += this.OnRoundStarted;
            ServerEvents.RoundEnded += this.OnEndingRound;
            Instance = this;

            base.OnEnabled();
        }

        /// <inheritdoc />
        public override void OnDisabled()
        {
            ServerEvents.RoundStarted -= this.OnRoundStarted;
            ServerEvents.RoundEnded -= this.OnEndingRound;
            Instance = null;
            base.OnDisabled();
        }

    }
}
