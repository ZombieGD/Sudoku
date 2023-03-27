using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelLayoutData_", menuName = "ScriptableObjects/LevelLayoutData", order = 1)]
public class LevelLayoutData : ScriptableObject
{
    [SerializeField]
    public string LevelDefinition = "";
}
