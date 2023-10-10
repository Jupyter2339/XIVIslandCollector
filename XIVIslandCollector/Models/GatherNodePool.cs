using System.Collections.Generic;
using Dalamud.Game.ClientState.Objects.Enums;
using ECommons.DalamudServices;
using ECommons.Logging;
using XIVIslandCollector.Models;

namespace XIVIslandCollector.Helper;

public class GatherNodePool
{
    public static Dictionary<string, HashSet<GatherNode>>
        Nodes = new();

    public static Dictionary<string, bool> NeedLoots = new();

    public static void EnumNearNodes()
    {
        PluginLog.Log("开始遍历数据");
        foreach (var obj in Svc.Objects)
        {
            if (obj.ObjectKind == ObjectKind.CardStand)
            {
                GatherNode node = new GatherNode(obj.Name.ToString(), obj.Position);
                AddNode(node);
            }
        }
    }

    public static void AddNode(GatherNode node)
    {
        string name = node.Name;
        if (!Nodes.ContainsKey(name))
        {
            Nodes.Add(name, new HashSet<GatherNode>());
        }

        Nodes[name].Add(node);
    }

    public static void InitNeedLoots()
    {
        foreach (var pair in Nodes)
        {
            var key = pair.Key;
            NeedLoots.Add(key, false);
        }
    }
}
