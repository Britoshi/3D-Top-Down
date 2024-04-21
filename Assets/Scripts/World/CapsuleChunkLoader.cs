

using Game;
using Unity.VisualScripting;
using UnityEngine;

public class CapsuleChunkLoader : ChunkLoader
{
    private CapsuleCollider capsuleCollider;
    ///Render radius unit in Chunk(s)
    [Range(1, 16)] public float renderRadius = 2;
    ///Render height unit in Chunk(s)
    [Range(1, 16)] public float renderHeight = 4;

    public override void AddColliderComponent()
    {
        capsuleCollider = chunkLoader.AddComponent<CapsuleCollider>();
        collider = capsuleCollider;
        capsuleCollider.radius = renderRadius * Chunk.UNIT_SIZE;
        capsuleCollider.height = renderHeight * Floor.HEIGHT;
    }
}


