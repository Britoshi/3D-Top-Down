
using Game;
using UnityEngine;



public abstract class ChunkLoader : BritoBehavior
{
    //Just to make sure, chunk layer should not collide with anything except for ChunkLoaderLayer(1 << 19);
    public const int CHUNK_LOADER_LAYER = 0x80000; // 1 << 19
    public const int CHUNK_LAYER = 0x100000; // 1 << 20
                                             //public const int CHUNK_LAYER = 0x100000; // 1 << 20

    protected Transform chunkLoader;
    protected new Collider collider;
    [Tooltip("If ? then = parent transform")]
    public Transform center;

    ///Simply just add collider component. Nothing additional. Rest will be handled by Awake function.
    public abstract void AddColliderComponent();

    public void Awake()
    {
        if (center == null) center = transform;
        chunkLoader = (new GameObject("Chunk Loader Instance")).transform;
        chunkLoader.parent = transform;
        chunkLoader.position = center.position;
        chunkLoader.gameObject.layer = CHUNK_LOADER_LAYER;

        AddColliderComponent();
        collider.isTrigger = true;
        collider.includeLayers = CHUNK_LAYER;

        chunkLoader.gameObject.AddComponent<ChunkLoaderInstance>();
    }
}
