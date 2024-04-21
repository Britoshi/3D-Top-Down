using Game;
using UnityEngine;

public class ChunkLoaderInstance : BritoBehavior
{
    public Vector3Int prevIndex;

    Vector3Int? CheckNewFloor()
    {
        var newIndex = transform.position.ToChunkCoordinate();
        if (!newIndex.Equals(prevIndex))
            return newIndex;
        else return null;
    }

    public void Update() //More efficient on tick?
    {
        //Handle Map loading and unloading

        //This means it's a new chunk/floor
        var newIndex = CheckNewFloor();
        if (newIndex != null)
        {
            World.OnRendererMove(newIndex);//This will render/unrender the maps. Or load/unload new scenes.
            prevIndex = newIndex.Value;
        }
    }


    public virtual void OnTriggerEnter(Collider other)
    {
        //name will be (Coordinate)(RenderStack) separated by ':' Example -> 0x0:0
        string[] token = other.gameObject.name.Split(':');
        int renderStack = int.Parse(token[1]) + 1;
        if (renderStack == 1) ChunkRenderer.Render(other.gameObject);
        other.gameObject.name = $"{token[0]}:{renderStack}";
    }

    public virtual void OnTriggerExit(Collider other)
    {
        string[] token = other.gameObject.name.Split(':');
        int renderStack = int.Parse(token[1]) - 1;
        if (renderStack == 0) ChunkRenderer.Unrender(other.gameObject);
        other.gameObject.name = $"{token[0]}:{renderStack}";
    }
}