namespace RoundMVP
{
     using Exiled.API.Features;
     using Exiled.API.Enums;
     using System.Collections.Generic;
     using PlayerRoles;

     public class Plugin : Plugin<Config>
     {
          public override string Name => "RoundMVP";
          public override string Prefix => "RoundMVP";
          public override string Author => "@misfiy";
          public override PluginPriority Priority => PluginPriority.Last;
          public static Plugin Instance;
          public string? FirstEscapeName;
          public RoleTypeId FirstEscapeRole; 
          public Dictionary<string, int> RoundKills = new Dictionary<string, int>();
          private Config config;
          private Handler handler;
          public override void OnEnabled()
          {
               Instance = this;
               config = Instance.Config;
               handler = new Handler();
               Exiled.Events.Handlers.Player.Dying += handler.OnDying;
               Exiled.Events.Handlers.Player.Escaping += handler.OnEscaping;
               Exiled.Events.Handlers.Server.WaitingForPlayers += handler.OnWaiting;
               Exiled.Events.Handlers.Server.RoundEnded += handler.OnRoundEnd;
               base.OnEnabled();
          }

          public override void OnDisabled()
          {
               Exiled.Events.Handlers.Player.Dying -= handler.OnDying;
               Exiled.Events.Handlers.Player.Escaping -= handler.OnEscaping;
               Exiled.Events.Handlers.Server.WaitingForPlayers -= handler.OnWaiting;
               Exiled.Events.Handlers.Server.RoundEnded -= handler.OnRoundEnd;

               handler = null;
               Instance = null;
               base.OnDisabled();
          }
     }
}

// player.TryAddCandy(InventorySystem.Items.Usables.Scp330.CandyKindID.Pink);