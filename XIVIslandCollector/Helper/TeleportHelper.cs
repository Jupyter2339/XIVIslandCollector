using System;
using System.Numerics;
using Dalamud.Memory;
using ECommons.DalamudServices;
using ECommons.Logging;
using XIVIslandCollector.Models;
using Vector3 = FFXIVClientStructs.FFXIV.Common.Math.Vector3;

namespace XIVIslandCollector.Helper;

public class TeleportHelper
{
    public static void Teleport(GatherNode node)
    {
        if (!IsInIsland()) return;
        if (Svc.ClientState.LocalPlayer == null) return;
        Vector3 position = Svc.ClientState.LocalPlayer.Position;
        IntPtr playerAddress = Svc.ClientState.LocalPlayer.Address;
        MemoryHelper.Write(playerAddress + 0xB0, node.X);
        MemoryHelper.Write(playerAddress + 0xB4, node.Y);
        MemoryHelper.Write(playerAddress + 0xB8, node.Z);
    }

    public static bool IsInIsland()
    {
        return Svc.ClientState.TerritoryType == 1055;
    }
}
