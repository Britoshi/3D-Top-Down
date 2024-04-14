using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
  public class EntityChunk
  {
    public Vec2 position;
    public EntityChunk(Vec2 position)
    {
      this.position = position;
    }
    public SortedSet<Entity> entities;
  }

  public class World : BritoBehavior
  {
    public static World Instance { set; get; }
    public static SortedDictionary<string, Map> LoadedMaps { set; get; }
    public static SortedDictionary<int, Entity> Entities { set; get; }

    void Awake()
    {
      Instance = this;
      Entities = new();
      LoadedMaps = new();
    }

    void Start()
    {

    }

    /// <summary>
    /// THis  ca  only work with tag "MAP"
    /// </summary>
    /// <param name="mapObject"></param>
    public static void RegisterMap(GameObject mapObject)
    {
      print("take this out later");
      LoadedMaps = new();
      if (!mapObject.CompareTag("MAP")) throw new System.Exception("Not a map or wrong tag");

      LoadedMaps.Add(mapObject.name, new(mapObject));

      print($"Successfully registered {mapObject.name}");
    }
    public static void Move(Entity entity, Vec2 prevCoord, Vec2 newCoord)
    {

    }
    public static void Dump()
    {
      string output = "";
      foreach (Map map in LoadedMaps.Values)
      {
        output += map.name + "\n";
        foreach (Chunk chunk in map.Chunks.Values)
        {
          output += "    " + chunk.position + ": Floors: ";
          foreach (Floor floor in chunk.floors)
          {
            output += " " + floor.height + ",";
          }
          output += "\n";
        }
        output += "\n";
      }
      print(output);
    }
  }
}
