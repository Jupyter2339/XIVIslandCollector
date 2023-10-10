using System;
using System.Collections.Generic;
using System.Numerics;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using ImGuiScene;
using XIVIslandCollector.Helper;
using XIVIslandCollector.Libs;
using XIVIslandCollector.Models;

namespace XIVIslandCollector.Windows;

public class Main : Window, IDisposable
{
    private string innerMachineCode = "";
    private string machineCode = "点击按钮刷新机器码, 卡顿 1~2s 属正常情况";
    private string validateCode = "在这里输入你的验证码";

    public Main() : base(
        "XIC 无人岛矿车青春版", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(480, 400),
            MaximumSize = new Vector2(480, 400)
        };

        // 初始化 GatherNode 池
        GatherNodePool.Nodes = GatherNodeFileManager.LoadNodeJson();
        GatherNodePool.NeedLoots = GatherNodeFileManager.LoadNeedLootJson();
        if (GatherNodePool.NeedLoots.Count == 0)
        {
            GatherNodePool.InitNeedLoots();
        }

        // 初始化采集线程
        ProcessingPool.InitProcessNodes();
        ProcessingPool.InitProcessThread();
    }

    public void Dispose()
    {
        ProcessingPool.IsThreadRunning = false;
    }

    public override void Draw()
    {
        DrawButtonGroup();
        DrawDictionary();
    }

    public void DrawButtonGroup()
    {
        ImGui.Text("请在无人岛地图使用!");
        ImGui.SameLine();
        if (ImGui.Button("开始 / 暂停"))
        {
            ProcessingPool.ToggleProcessing();
        }

        ImGui.SameLine();
        if (ProcessingPool.IsProcessing)
        {
            ImGui.TextColored(ImGuiColors.HealerGreen, "运行进行中");
            ImGui.SameLine();
            ImGui.Text(ProcessingPool.ProcessingNodeIndex + " / " + ProcessingPool.ProcessingNodes.Count);
        }
        else
        {
            ImGui.TextColored(ImGuiColors.DalamudRed, "运行暂停中");
        }
    }

    public void DrawDictionary()
    {
        var needLootList = GatherNodePool.NeedLoots;
        if (ImGui.BeginTable("table1", 3, ImGuiTableFlags.ScrollY))
        {
            ImGui.TableSetupColumn("Column 1", ImGuiTableColumnFlags.WidthFixed, 300.0f);
            ImGui.TableSetupColumn("Column 1", ImGuiTableColumnFlags.WidthFixed, 75f);
            ImGui.TableSetupColumn("Column 1", ImGuiTableColumnFlags.WidthFixed, 75f);
            ImGui.TableNextRow();
            ImGui.TableSetColumnIndex(0);
            ImGui.Separator();
            ImGui.Text("目标资源");
            ImGui.TableSetColumnIndex(1);
            ImGui.Separator();
            ImGui.Text("节点数量");
            ImGui.TableSetColumnIndex(2);
            ImGui.Separator();
            ImGui.Text("是否获取");
            // ImGui.TableSetColumnIndex(3);
            // ImGui.Text("前往目标");

            foreach (var category in GatherNodePool.Nodes)
            {
                var key = category.Key;    // 海岛胡萝卜
                var list = category.Value; // list
                var needLoot = needLootList[key];

                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);
                ImGui.Separator();
                ImGui.Text(key);
                ImGui.TableSetColumnIndex(1);
                ImGui.Separator();
                ImGui.Text(list.Count.ToString());
                ImGui.TableSetColumnIndex(2);
                ImGui.Separator();
                if (ImGui.Checkbox("##" + key, ref needLoot))
                {
                    GatherNodePool.NeedLoots[key] = !GatherNodePool.NeedLoots[key];
                    GatherNodeFileManager.SaveNeedLootJson();
                    ProcessingPool.InitProcessNodes();
                }

                // ImGui.TableSetColumnIndex(3);
                // ImGui.Text("Column 4");
            }

            ImGui.EndTable();
        }
    }

    public void DrawAdmin()
    {
        if (ImGui.Button("刷新列表"))
        {
            GatherNodePool.Nodes = GatherNodeFileManager.LoadNodeJson();
            GatherNodePool.NeedLoots = GatherNodeFileManager.LoadNeedLootJson();
            if (GatherNodePool.NeedLoots.Count == 0)
            {
                GatherNodePool.InitNeedLoots();
            }
        }

        if (ImGui.Button("调试输出周围物体"))
        {
            GatherNodePool.EnumNearNodes();
        }

        if (ImGui.Button("从 nodes.json 读取"))
        {
            GatherNodePool.Nodes = GatherNodeFileManager.LoadNodeJson();
        }

        if (ImGui.Button("输出采集列表"))
        {
            ProcessingPool.GetNeedLootList();
        }

        if (ImGui.Button("输出采集路线列表"))
        {
            ProcessingPool.InitProcessNodes();
        }

        if (ImGui.Button("输出玩家位置"))
        {
            TeleportHelper.Teleport(ProcessingPool.ProcessingNodes[0]);
        }
    }
}
