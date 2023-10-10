using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using Dalamud.Logging;
using ECommons.DalamudServices;
using XIVIslandCollector.Helper;

namespace XIVIslandCollector.Models;

public class GatherNodeFileManager
{
    /// <summary>
    /// 保存采集列表
    /// </summary>
    public static void SaveNeedLootJson()
    {
        string fileName = "needLoots.json";
        var serializer = new DataContractJsonSerializer(typeof(Dictionary<string, bool>));
        var memoryStream = new MemoryStream();
        serializer.WriteObject(memoryStream, GatherNodePool.NeedLoots);
        byte[] jsonBytes = memoryStream.ToArray();
        string jsonString = Encoding.UTF8.GetString(jsonBytes);
        File.WriteAllText(Svc.PluginInterface.AssemblyLocation.DirectoryName + "/" + fileName, jsonString);
    }

    /// <summary>
    /// 读取采集列表
    /// </summary>
    /// <returns></returns>
    public static Dictionary<string, bool> LoadNeedLootJson()
    {
        string fileName = "needLoots.json";
        string geneFilePath = Svc.PluginInterface.AssemblyLocation.DirectoryName + "/" + fileName;
        if (!File.Exists(geneFilePath))
        {
            return new Dictionary<string, bool>();
        }

        string jsonString = File.ReadAllText(geneFilePath);
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonString);
        var memoryStream = new MemoryStream(jsonBytes);
        var serializer = new DataContractJsonSerializer(typeof(Dictionary<string, bool>));
        var deserializedData = (Dictionary<string, bool>)serializer.ReadObject(memoryStream);
        return deserializedData;
    }

    /// <summary>
    /// 保存池子
    /// </summary>
    public static void SaveNodeJson()
    {
        string fileName = "nodes.json";
        var serializer = new DataContractJsonSerializer(typeof(Dictionary<string, HashSet<GatherNode>>));
        var memoryStream = new MemoryStream();
        serializer.WriteObject(memoryStream, GatherNodePool.Nodes);
        byte[] jsonBytes = memoryStream.ToArray();
        string jsonString = Encoding.UTF8.GetString(jsonBytes);
        File.WriteAllText(Svc.PluginInterface.AssemblyLocation.DirectoryName + "/" + fileName, jsonString);
    }

    /// <summary>
    /// 读取池子
    /// </summary>
    /// <returns></returns>
    public static Dictionary<string, HashSet<GatherNode>> LoadNodeJson()
    {
        string fileName = "nodes.json";
        string geneFilePath = Svc.PluginInterface.AssemblyLocation.DirectoryName + "/" + fileName;
        if (!File.Exists(geneFilePath))
        {
            return new Dictionary<string, HashSet<GatherNode>>();
        }

        string jsonString = File.ReadAllText(geneFilePath);
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonString);
        var memoryStream = new MemoryStream(jsonBytes);
        var serializer = new DataContractJsonSerializer(typeof(Dictionary<string, HashSet<GatherNode>>));
        var deserializedData = (Dictionary<string, HashSet<GatherNode>>)serializer.ReadObject(memoryStream);
        return deserializedData;
    }
}
