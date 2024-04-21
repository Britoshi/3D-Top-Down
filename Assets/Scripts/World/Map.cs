using Game;
using System.Collections.Generic;

public class Map
{
    public const string TAG = "MAP";

    public string name;
    public SortedDictionary<Vec2, Domain> domains;

    public void Remove(int x, int y)
    {
        domains.Remove(new Vec2(x, y));
        if (domains.Count == 0) World.Remove(name);
    }

    public Domain GetMakeDomain(int x, int y)
    {
        var index = new Vec2(x, y);
        if (domains.TryGetValue(index, out var domain)) return domain;
        domain = new(this, (byte)x, (byte)y);
        return domain;
    }
}