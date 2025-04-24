using UnityEngine;
using Warudo.Core.Attributes;
using Warudo.Core.Plugins;

[PluginType(
    Id = "okakiya.gachamod",
    Name = "GachaMOD",
    Description = "A simple plugin that support gachagacha.",
    Version = "1.1.0",
    Author = "okakiya",
    SupportUrl = "",
    NodeTypes = new [] { typeof(GachaTable), typeof(MakeGachaItem), typeof(DoGacha), typeof(GachaTable20) })]
public class GachaPlugin : Plugin {

    protected override void OnCreate() {
        base.OnCreate();
        Debug.Log("The Gacha plugin is officially enabled! Hooray!");
    }

}

