using Game;
using UnityEngine;

public class Group
{
    public const short DIMENSION = 4;

    public Section section;
    public byte x, y;
    public short count;
    public Chunk[,] chunks;
    public Group(Section section, int x, int y)
    {
        this.x = (byte)x;
        this.y = (byte)y;
        this.section = section;
        chunks = new Chunk[DIMENSION, DIMENSION];
        count = 0;
    }

    public void Remove(byte x, byte y)
    {
        chunks[y, x] = null;
        if (--count == 0) section.Remove(this.x, this.y);
    }

    public Chunk GetMakeChunk(int x, int y)
    {
        bool xOutOfBounds = x < 0 || x >= DIMENSION;
        bool yOutOfBounds = y < 0 || y >= DIMENSION;

        if (!xOutOfBounds && !yOutOfBounds)
        {
            var chunk = chunks[y, x];
            if (chunk == null)
            {
                chunk = new Chunk(this, x, y);
                chunks[y, x] = chunk;
            }
            return chunk;
        }

        int groupIndexX = this.x;
        int chunkIndexX = x;
        if (xOutOfBounds)
        {
            int baseX = (int)Mathf.Floor(x / DIMENSION);
            groupIndexX += baseX;
            chunkIndexX -= baseX * DIMENSION;
        }
        int groupIndexY = this.y;
        int chunkIndexY = y;
        if (yOutOfBounds)
        {
            int baseY = (int)Mathf.Floor(y / DIMENSION);
            groupIndexY += baseY;
            chunkIndexY -= baseY * DIMENSION;
        }
        return section.GetMakeGroup(groupIndexX, groupIndexY).GetMakeChunk(chunkIndexX, chunkIndexY);
    }

}