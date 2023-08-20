using Exiled.API.Interfaces;
using Exiled.API.Enums;
using PlayerRoles;
using System.ComponentModel;
using UnityEngine;

namespace RoundMVP
{
     public sealed class Config : IConfig
     {
          public bool IsEnabled { get; set; } = true;
          public bool Debug { get; set; } = false;
          public string HumanKillerText { get; set; } = "%name% got the most kills as a human: %kills%";
          public string ScpKillerText { get; set; } = "%name% got the most kills as SCP: %kills%";
          public string EscapeMessage { get; set; } = "%name% was the first to escape as a %role%!";
          public string FirstScpKill { get; set; } = "%name% was the first to kill an SCP as a %role%";
     }
}