
using Game;
using UnityEngine;

public class BoxChunkLoader : ChunkLoader
{
    private BoxCollider boxCollider;


    ///Render size(x*x) unit in Chunk(s)
    [Range(1, 16)] public float renderLength = 2;
    ///Render height unit in Chunk(s)
    [Range(1, 16)] public float renderHeight = 4;

    public override void AddColliderComponent()
    {
        boxCollider = chunkLoader.gameObject.AddComponent<BoxCollider>();
        collider = boxCollider;
        Vector3 targetSize = Chunk.UNIT_SIZE * renderLength * Vector3.one;
        targetSize.y = renderHeight * Floor.HEIGHT;
        boxCollider.size = targetSize;
    }
}