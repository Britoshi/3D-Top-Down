using UnityEngine;

public class Domain
{
    public const short DIMENSION = 10;

    public Map map;
    public byte x, y;
    public short count;
    public Section[,] sections;
    public Domain(Map map, byte x, byte y)
    {
        this.x = x;
        this.y = y;
        this.map = map;
        sections = new Section[DIMENSION, DIMENSION];
        count = 0;
    }

    public void Remove(byte x, byte y)
    {
        sections[y, x] = null;
        if (--count == 0) map.Remove(this.x, this.y);
    }

    public Section GetMakeSection(int x, int y)
    {
        bool xOutOfBounds = x < 0 || x >= DIMENSION;
        bool yOutOfBounds = y < 0 || y >= DIMENSION;

        if (!xOutOfBounds && !yOutOfBounds)
        {
            var section = sections[y, x];
            if (section == null)
            {
                section = new Section(this, x, y);
                sections[y, x] = section;
            }
            return section;
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
        return map.GetMakeDomain(rootIndexX, rootIndexY).GetMakeSection(targetIndexX, targetIndexY);
    }

}