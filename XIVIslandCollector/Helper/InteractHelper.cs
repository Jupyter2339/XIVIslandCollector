using System;
using System.Linq;
using Dalamud.Game.ClientState.Objects.Types;
using ECommons.DalamudServices;
using ECommons.Logging;
using FFXIVClientStructs.FFXIV.Client.Game.Control;

namespace XIVIslandCollector.Helper;

public unsafe class InteractHelper
{
    public static void InteractWithNearestObj()
    {
        var nearObjects = Svc.Objects.Where(
            x => (x.ObjectKind == Dalamud.Game.ClientState.Objects.Enums.ObjectKind.GatheringPoint ||
                  x.ObjectKind == Dalamud.Game.ClientState.Objects.Enums.ObjectKind.CardStand) &&
                 GetTargetDistance(x) < 2).ToList();
        if (nearObjects.Count < 1) return;

        var nearestObj = nearObjects.OrderBy(GetTargetDistance).First();
        var baseObj = (FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject*)nearestObj.Address;

        if (GetTargetDistance(nearestObj) > 1) return;
        if (!baseObj->GetIsTargetable()) return;

        TargetSystem.Instance()->InteractWithObject(baseObj, true);
    }

    private static float GetTargetDistance(GameObject gameObject)
    {
        float playerX = Svc.ClientState.LocalPlayer.Position.X;
        float playerY = Svc.ClientState.LocalPlayer.Position.Y;
        float objX = gameObject.Position.X;
        float objY = gameObject.Position.Y;
        float deltaX = playerX - objX;
        float deltaY = playerY - objY;

        return MathF.Sqrt(deltaX * deltaX + deltaY * deltaY);
    }
}
