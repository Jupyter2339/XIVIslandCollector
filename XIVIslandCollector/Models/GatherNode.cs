using System.Runtime.Serialization;
using FFXIVClientStructs.FFXIV.Common.Math;

namespace XIVIslandCollector.Models;

[DataContract]
public class GatherNode
{
    [DataMember]
    public string Name = "";

    [DataMember]
    public float X;

    [DataMember]
    public float Y;

    [DataMember]
    public float Z;

    public GatherNode(string name, float x, float y, float z)
    {
        Name = name;
        X = x;
        Y = y;
        Z = z;
    }

    public GatherNode(string name, Vector3 position)
    {
        Name = name;
        X = position.X;
        Y = position.Y;
        Z = position.Z;
    }

    public override string ToString()
    {
        return Name + " x:" + X + " y:" + Y + " z:" + Z;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + X.GetHashCode();
        hash = hash * 23 + Y.GetHashCode();
        hash = hash * 23 + Z.GetHashCode();
        return hash;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        GatherNode otherNode = (GatherNode)obj;
        return Name == otherNode.Name && X == otherNode.X && Y == otherNode.Y && Z == otherNode.Z;
    }
}
