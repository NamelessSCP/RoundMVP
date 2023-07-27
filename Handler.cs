namespace RoundMVP
{
    using Exiled.Events.EventArgs.Player;
    using Exiled.Events.EventArgs.Server;
    using Exiled.API.Features;
    public sealed class Handler
    {
        Config config = Plugin.Instance.Config;
        public void OnWaiting()
        {
            Plugin.Instance.RoundKills.Clear();
            Plugin.Instance.FirstEscape = new KeyValuePair<string, PlayerRoles.RoleTypeId>();
        }
        public void OnEscaping(EscapingEventArgs ev)
        {
            if(!ev.IsAllowed) return;
            if(!Plugin.Instance.FirstEscape.Key.IsEmpty()) return;
            Plugin.Instance.FirstEscape = new KeyValuePair<string, PlayerRoles.RoleTypeId>(ev.Player.Nickname, ev.Player.Role.Type);
        }
        public void OnDying(DyingEventArgs ev)
        {
            if (ev.Attacker == null) return;
            string killer = ev.Attacker.Nickname;
            if (!Plugin.Instance.RoundKills.ContainsKey(killer))
            {
                Plugin.Instance.RoundKills.Add(killer, 1);
            }
            else
            {
                Plugin.Instance.RoundKills[killer] += 1;
            }
        }
        public void OnRoundEnd(RoundEndedEventArgs ev)
        {
            string message = GetEndMessage();
            Map.Broadcast(10, message);
        }
        private void GetHighestKillCount(out string? topKiller, out int kills)
        {
            int highestScore = 0;
            string? playerWithHighestScore = null;
            foreach (var entry in Plugin.Instance.RoundKills)
            {
                if (entry.Value > highestScore)
                {
                    highestScore = entry.Value;
                    playerWithHighestScore = entry.Key;
                }
            }
            topKiller = playerWithHighestScore;
            kills = highestScore;
        }
        public string GetEndMessage()
        {
            string? topKiller;
            int topKillerKills;
            GetHighestKillCount(out topKiller, out topKillerKills);
            string message = "";
            if(topKiller == null) 
            {
                message += config.noKillsText;
            }
            else 
            {
                string killsMessage = config.killsText.Replace("%killer%", topKiller).Replace("%kills%", topKillerKills.ToString());
                message += killsMessage;
            }
            if(Plugin.Instance.FirstEscape.Key.IsEmpty()) message += $"\n{config.escapeMessage}";
            else
            {
                string escapeMessage = "\n" + config.escapeMessage.Replace("%escapee%", Plugin.Instance.FirstEscape.Key).Replace("%escaperole%", Plugin.Instance.FirstEscape.Value.ToString());
                message += escapeMessage;
            }
            // message += $"\n{config.escapeMessage}";
            return message;
        }
    }
}