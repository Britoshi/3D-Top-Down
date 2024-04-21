using Game;
using UnityEngine;

public class Section
{
    public const short DIMENSION = 10;

    public Domain domain;
    public byte x, y;
    public short count;
    public Group[,] groups;
    public Section(Domain domain, int x, int y)
    {
        this.x = (byte)x;
        this.y = (byte)y;
        this.domain = domain;
        groups = new Group[DIMENSION, DIMENSION];
        count = 0;
    }

    public void Remove(byte x, byte y)
    {
        groups[y, x] = null;
        if (--count == 0) domain.Remove(this.x, this.y);
    }

    public Group GetMakeGroup(int x, int y)
    {
        bool xOutOfBounds = x < 0 || x >= DIMENSION;
        bool yOutOfBounds = y < 0 || y >= DIMENSION;

        if (!xOutOfBounds && !yOutOfBounds)
        {
            var group = groups[y, x];
            if (group == null)
            {
                group = new Group(this, x, y);
                groups[y, x] = group;
            }
            return group;
        }

        int rootIndexX = this.x;
        int targetIndexX = x;
        if (xOutOfBounds)
        {
            int baseX = (int)Mathf.Floor(x / DIMENSION);
            rootIndexX += baseX;
            targetIndexX -= baseX * DIMENSION;
        }
        int rootIndexY = this.y;
        int targetIndexY = y;
        if (yOutOfBounds)
        {
            int baseY = (int)Mathf.Floor(y / DIMENSION);
            rootIndexY += baseY;
            targetIndexY -= baseY * DIMENSION;
        }
        return domain.GetMakeSection(rootIndexX, rootIndexY).GetMakeGroup(targetIndexX, targetIndexY);
    }

}