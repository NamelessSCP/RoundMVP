namespace RoundMVP
{
     using Exiled.Events.EventArgs.Player;
     using Exiled.Events.EventArgs.Server;
     using Exiled.API.Features;
     using Exiled.API.Enums;
     using PlayerRoles;

     public sealed class Handler
     {
          private readonly Config config = Plugin.Instance.Config;

          private Dictionary<Player, int> humanKills = new();
          private Dictionary<Player, int> scpKills = new();
          private Dictionary<Player, RoleTypeId> escapes = new();
          private Dictionary<Player, RoleTypeId> scpKillers = new();

          public void OnWaiting()
          {
               humanKills.Clear();
               scpKills.Clear();
               escapes.Clear();
               scpKillers.Clear();
          }
          public void OnSpawned(SpawnedEventArgs ev)
          {
               if (ev.Reason != SpawnReason.Escaped || escapes.ContainsKey(ev.Player)) return;
               escapes.Add(ev.Player, ev.OldRole);
          }
          public void OnDied(DiedEventArgs ev)
          {
               if (ev.DamageHandler.Type == DamageType.PocketDimension)
               {
                    foreach(Player scp in Player.List.Where(p => p.Role.Type == RoleTypeId.Scp106))
                    {
                         if(!scpKills.ContainsKey(scp)) scpKills.Add(scp, 0);
                         else scpKills[scp]++;
                    }
               }
               if (ev.Attacker == null) return;
               if(ev.Attacker.IsScp)
               {
                    if(!scpKills.ContainsKey(ev.Attacker)) scpKills.Add(ev.Attacker, 0);
                    else scpKills[ev.Attacker]++;


                    if(!scpKillers.ContainsKey(ev.Attacker)) scpKillers.Add(ev.Attacker, ev.Attacker.Role.Type);
               }
               else
               {
                    if(!humanKills.ContainsKey(ev.Attacker)) humanKills.Add(ev.Attacker, 0);
                    else humanKills[ev.Attacker]++;
               }
          }
          public void OnRoundEnd(RoundEndedEventArgs ev)
          {
               string text = "";

               Player? humanKiller = GetTopHumanKills();
               Player? scpKiller = GetTopScpKills();
               
               if(humanKiller != null) text += $"{config.HumanKillerText.Replace("%name%", humanKiller.Nickname).Replace("%kills%", humanKills[humanKiller].ToString())}\n";
               if(scpKiller != null) text += $"{config.ScpKillerText.Replace("%name%", scpKiller.Nickname).Replace("%kills%", scpKills[scpKiller].ToString())}\n";
               if(!escapes.IsEmpty()) text += $"{config.EscapeMessage.Replace("%name%", escapes.First().Key.Nickname).Replace("%role%", escapes.First().Value.ToString())}\n";
               if(!scpKillers.IsEmpty()) text += $"{config.FirstScpKill.Replace("%name%", scpKillers.First().Key.Nickname).Replace("%role%", scpKillers.First().Value.ToString())}\n";

               if(!text.IsEmpty()) Map.Broadcast(10, text, default, true);
          }
          private Player? GetTopHumanKills()
          {
               Player? player = null;
               int max = 0;

               foreach(KeyValuePair<Player, int> kvp in humanKills)
               {
                    if(kvp.Value > max) { max = kvp.Value; player = kvp.Key; }
               }

               return player;
          }
          private Player? GetTopScpKills()
          {
               Player? player = null;
               int max = 0;

               foreach(KeyValuePair<Player, int> kvp in scpKills)
               {
                    if(kvp.Value > max) { max = kvp.Value; player = kvp.Key; }
               }

               return player;
          }


          //private void GetHighestKillCount(out string? topKiller, out int kills)
          //{
          //     int highestScore = 0;
          //     string? playerWithHighestScore = null;
          //     foreach (var entry in Plugin.Instance.RoundKills)
          //     {
          //          if (entry.Value > highestScore)
          //          {
          //               highestScore = entry.Value;
          //               playerWithHighestScore = entry.Key;
          //          }
          //     }
          //     topKiller = playerWithHighestScore;
          //     kills = highestScore;
          //}
          //public string GetEndMessage()
          //{
          //     string? topKiller;
          //     int topKillerKills;
          //     GetHighestKillCount(out topKiller, out topKillerKills);
          //     string message = "";
          //     if (topKiller == null)
          //     {
          //          message += config.noKillsText;
          //     }
          //     else
          //     {
          //          string killsMessage = config.killsText.Replace("%killer%", topKiller).Replace("%kills%", topKillerKills.ToString());
          //          message += killsMessage;
          //     }
          //     if (Plugin.Instance.FirstEscapeName == null || Plugin.Instance.FirstEscapeName.IsEmpty())
          //     {
          //          message += "\n" + config.noEscapesMessage;
          //     }
          //     else
          //     {
          //          string escapeMessage = "\n" + config.escapeMessage.Replace("%escapee%", Plugin.Instance.FirstEscapeName).Replace("%escaperole%", Plugin.Instance.FirstEscapeRole.ToString());
          //          message += escapeMessage;
          //     }
          //     return message;
          //}
     }
}