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
         
            foreach (Player p in Player.List)
            {
                p.Broadcast(2, "Friendly Fire Disabled - RoundEnded", global::Broadcast.BroadcastFlags.Normal, true);
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

            foreach (Player p in Player.List)
            {
                p.Broadcast(2, "Friendly Fire Enabled - RoundEnding", global::Broadcast.BroadcastFlags.Normal, true);
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

            base.OnEnabled();
        }

        /// <inheritdoc />
        public override void OnDisabled()
        {
            ServerEvents.RoundStarted -= this.OnRoundStarted;
            ServerEvents.RoundEnded -= this.OnEndingRound;
            base.OnDisabled();
        }

    }
}
