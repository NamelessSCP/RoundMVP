namespace RoundMVP
{
     using Exiled.API.Features;

     public class Plugin : Plugin<Config>
     {
          public override string Name => "RoundMVP";
          public override string Prefix => "RoundMVP";
          public override string Author => "@misfiy";
          public override Version RequiredExiledVersion => new(8, 2, 1);
          public override Version Version => new(1, 2, 4);

          public static Plugin Instance;
          private Handler handler;

          public override void OnEnabled()
          {
               Instance = this;
               handler = new Handler();
               Exiled.Events.Handlers.Player.Died += handler.OnDied;
               Exiled.Events.Handlers.Player.Spawned += handler.OnSpawned;
               Exiled.Events.Handlers.Server.WaitingForPlayers += handler.OnWaiting;
               Exiled.Events.Handlers.Server.RoundEnded += handler.OnRoundEnd;

               base.OnEnabled();
          }

          public override void OnDisabled()
          {
               Exiled.Events.Handlers.Player.Died -= handler.OnDied;
               Exiled.Events.Handlers.Player.Spawned -= handler.OnSpawned;
               Exiled.Events.Handlers.Server.WaitingForPlayers -= handler.OnWaiting;
               Exiled.Events.Handlers.Server.RoundEnded -= handler.OnRoundEnd;

               handler = null!;
               Instance = null!;
               base.OnDisabled();
          }
     }
}