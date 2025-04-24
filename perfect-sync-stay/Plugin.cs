using UnityEngine;
using Warudo.Core.Attributes;
using Warudo.Core.Plugins;

[PluginType(
    Id = "okakiya.psStay",
    Name = "PerfectSyncStay",
    Description = "A simple plugin that support perfectsync stay on out of tracking.",
    Version = "1.0.0",
    Author = "okakiya",
    SupportUrl = "",
    NodeTypes = new [] { typeof(PerfectSyncStay) })]
public class PerfectSyncStayPlugin : Plugin {

    protected override void OnCreate() {
        base.OnCreate();
        Debug.Log("The PerfectSyncStay plugin is officially enabled! Hooray!");
    }

}

