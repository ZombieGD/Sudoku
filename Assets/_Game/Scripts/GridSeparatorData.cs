using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridSeparatorData_", menuName = "ScriptableObjects/GridSeparatorData", order = 3)]
public class GridSeparatorData : ScriptableObject
{
    [SerializeField]
    private Material defaultMaterial;
    public Material DefaultMaterial => defaultMaterial;

    [SerializeField]
    private Material highlightHeavyMaterial;
    public Material HighlightHeavyMaterial => highlightHeavyMaterial;

    [SerializeField]
    private Material highlightLightMaterial;
    public Material HighlightLightMaterial => highlightLightMaterial;
}
