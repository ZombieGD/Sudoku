using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSeparatorPresenter : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private GridSeparatorData separatorData;

    public void ClearHighlight()
    {
        spriteRenderer.material = separatorData.DefaultMaterial;
    }

    public void HighlightHeavy()
    {
        spriteRenderer.material = separatorData.HighlightHeavyMaterial;
    }

    public void HighlightLight()
    {
        spriteRenderer.material = separatorData.HighlightLightMaterial;
    }
}
