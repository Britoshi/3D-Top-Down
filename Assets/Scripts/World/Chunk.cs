using Game;
using System.Collections.Generic;

public class Chunk
{
    public const string HOLDER_TRANSFORM_NAME = "CHUNKS";
    public const int UNIT_SIZE = 32;

    public Group group;
    public byte x, y;
    public SortedDictionary<short, Floor> floors;
    public Chunk(Group group, Vec2 localIndex) : this(group, localIndex.x, localIndex.y)
    {

    }
    public Chunk(Group group, int x, int y)
    {
        this.x = (byte)x;
        this.y = (byte)y;
        this.group = group;
        floors = new(); 
    }

    public void Remove(short height)
    {
        floors.Remove(height);
        if (floors.Count == 0) group.Remove(x, y);
    }

    public Floor GetMakeFloor(int height)
    {
        if (floors.TryGetValue((short)height, out var floor)) return floor;
        floor = new(this, (short)height);
        return floor;
    }

}