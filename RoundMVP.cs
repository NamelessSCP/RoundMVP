namespace RoundMVP
{
     using Exiled.API.Features;
     using Exiled.API.Enums;
     using System.Collections.Generic;

     public class Plugin : Plugin<Config>
     {
          public override string Name => "RoundMVP";
          public override string Prefix => "RoundMVP";
          public override string Author => "@misfiy";
          public override PluginPriority Priority => PluginPriority.Default;
          public static Plugin Instance;
          public Dictionary<string, int> RoundKills = new Dictionary<string, int>();
          private Config config;
          private Handler handler;
          public override void OnEnabled()
          {
               Instance = this;
               config = Instance.Config;
               handler = new Handler();
               Exiled.Events.Handlers.Player.Dying += handler.OnDying;
               Exiled.Events.Handlers.Server.WaitingForPlayers += handler.OnWaiting;
               Exiled.Events.Handlers.Server.RoundEnded += handler.OnRoundEnd;
               base.OnEnabled();
          }

          public override void OnDisabled()
          {
               Exiled.Events.Handlers.Player.Dying -= handler.OnDying;
               Exiled.Events.Handlers.Server.WaitingForPlayers -= handler.OnWaiting;
               Exiled.Events.Handlers.Server.RoundEnded -= handler.OnRoundEnd;

               handler = null;
               Instance = null;
               base.OnDisabled();
          }
     }
}

// player.TryAddCandy(InventorySystem.Items.Usables.Scp330.CandyKindID.Pink);