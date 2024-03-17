using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class AutoCellSize : MonoBehaviour
{
    private GridLayoutGroup gridLayoutGroup; 
    void Start()
    {
        // Get the GridLayoutGroup component attached to this GameObject
        gridLayoutGroup = GetComponent<GridLayoutGroup>();

        // Set the cell size based on the parent's width and the horizontal constraint count
        AdjustCellSize();
    }

    void Update()
    {
        // Continuously update the cell size in case the parent's width changes dynamically
        //AdjustCellSize();
        AdjustCellSize();
    }

    private void OnEnable()
    {
        if(gridLayoutGroup != null)
        {
            gridLayoutGroup = GetComponent<GridLayoutGroup>();
        }
        AdjustCellSize();
    }
    public void AdjustCellSize()
    {
        if (gridLayoutGroup == null) return;
        // Ensure the GridLayoutGroup has a horizontal constraint
        if (gridLayoutGroup.constraint != GridLayoutGroup.Constraint.FixedColumnCount)
        {
            Debug.LogWarning("AutoCellSize requires GridLayoutGroup to use Fixed Column Count constraint.");
            return;
        }

        // Calculate the cell width based on the parent's width and the horizontal constraint count
        RectTransform parentRectTransform = (RectTransform)transform.parent;
        float availableWidth = parentRectTransform.rect.width - gridLayoutGroup.padding.horizontal - gridLayoutGroup.spacing.x * (gridLayoutGroup.constraintCount - 1);
        float cellWidth = availableWidth / gridLayoutGroup.constraintCount;

        // Update the cell size in the GridLayoutGroup
        gridLayoutGroup.cellSize = new Vector2(cellWidth, cellWidth);

        //Add Here please
        float heightCount =Mathf.Ceil( (float)transform.childCount/ (float)gridLayoutGroup.constraintCount) * cellWidth; 
        (transform as RectTransform).sizeDelta = 
            new Vector2((transform as RectTransform).sizeDelta.x, heightCount);
    }
}