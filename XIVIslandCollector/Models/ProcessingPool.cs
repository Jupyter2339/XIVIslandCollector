using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Dalamud.Logging;
using XIVIslandCollector.Helper;
using PluginLog = ECommons.Logging.PluginLog;

namespace XIVIslandCollector.Models;

public class ProcessingPool
{
    private static readonly int IntervalLoadMap = 500;
    private static readonly int IntervalGather = 3000;

    public static bool IsThreadRunning = true;
    public static bool IsProcessing;
    public static Thread ThreadProcessing;
    public static int ProcessingNodeIndex = 0;
    public static List<GatherNode> ProcessingNodes = new List<GatherNode>();

    public static void InitProcessThread()
    {
        ThreadProcessing = new Thread(ThreadGathering);
        ThreadProcessing.Start();
    }

    private static void ThreadGathering()
    {
        while (IsThreadRunning)
        {
            if (IsProcessing)
            {
                GatherPoint();
                ProcessingNodeIndex++;
            }
        }
    }

    private static void GatherPoint()
    {
        if (ProcessingNodeIndex >= ProcessingNodes.Count)
        {
            ProcessingNodeIndex = 0;
        }

        GatherNode gatherNode = ProcessingNodes[ProcessingNodeIndex];
        TeleportHelper.Teleport(gatherNode);
        Thread.Sleep(IntervalLoadMap);
        InteractHelper.InteractWithNearestObj();
        Thread.Sleep(IntervalGather);
    }

    /// <summary>
    /// 获取要前往的节点
    /// </summary>
    public static void InitProcessNodes()
    {
        ProcessingNodes = new List<GatherNode>();

        var needLootList = GetNeedLootList();
        foreach (var pair in GatherNodePool.Nodes)
        {
            var key = pair.Key;
            var list = pair.Value;
            if (needLootList.Contains(pair.Key))
            {
                foreach (var node in list)
                {
                    ProcessingNodes.Add(node);
                }
            }
        }
    }

    public static void ToggleProcessing()
    {
        if (!TeleportHelper.IsInIsland()) return;
        IsProcessing = !IsProcessing;
    }

    public static List<string> GetNeedLootList()
    {
        var list = new List<string>();
        foreach (var needLoot in GatherNodePool.NeedLoots)
        {
            if (needLoot.Value)
            {
                list.Add(needLoot.Key);
            }
        }

        return list;
    }
}
