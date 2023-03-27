using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridCellData_", menuName = "ScriptableObjects/GridCellData", order = 2)]
public class GridCellData : ScriptableObject
{
    [SerializeField]
    private Vector2 cellSize = new Vector2(1,1);
    public Vector2 CellSize => cellSize;

    [SerializeField]
    private float completedHighlightDelaySeconds = 0.25f;
    public float CompletedHighlightDelaySeconds => completedHighlightDelaySeconds;
    
    [SerializeField]
    private float completedHighlightDurationSeconds = 0.5f;
    public float CompletedHighlightDurationSeconds => completedHighlightDurationSeconds;

    [SerializeField]
    private float correctHighlightDurationSeconds = 0.5f;
    public float CorrectHighlightDurationSeconds => correctHighlightDurationSeconds;

    [SerializeField]
    private float incorrectHighlightDurationSeconds = 0.5f;
    public float IncorrectHighlightDurationSeconds => incorrectHighlightDurationSeconds;

    [SerializeField]
    private Vector3 completedPunchScale = new Vector3(0.1f, 0.1f, 0f);
    public Vector3 CompletedPunchScale => completedPunchScale;

    [SerializeField]
    private Vector3 correctPunchScale = new Vector3(0.1f, 0.1f, 0f);
    public Vector3 CorrectPunchScale => correctPunchScale;

    [SerializeField]
    private int completedPunchVibrato = 10;
    public int CompletedPunchVibrato => completedPunchVibrato;

    [SerializeField]
    private int correctPunchVibrato = 10;
    public int CorrectPunchVibrato => correctPunchVibrato;

    [SerializeField]
    private float incorrectShakeStrenght = 0.1f;
    public float IncorrectShakeStrenght => incorrectShakeStrenght;

    [SerializeField]
    private int incorrectShakeVibrato = 10;
    public int IncorrectShakeVibrato => incorrectShakeVibrato;

    [SerializeField]
    private Color completedHighLightColor = Color.white;
    public Color CompletedHighLightColor => completedHighLightColor;

    [SerializeField]
    private Color correctHighLightColor = Color.white;
    public Color CorrectHighLightColor => correctHighLightColor;

    [SerializeField]
    private Color incorrectHighLightColor = Color.white;
    public Color IncorrectHighLightColor => incorrectHighLightColor;

    [SerializeField]
    private Ease completedHighlightEase = Ease.Flash;
    public Ease CompletedHighlightEase => completedHighlightEase;

    [SerializeField]
    private Ease correctHighlightEase = Ease.Flash;
    public Ease CorrectHighlightEase => correctHighlightEase;

    [SerializeField]
    private Ease incorrectHighlightEase = Ease.Flash;
    public Ease IncorrectHighlightEase => incorrectHighlightEase;

    [SerializeField]
    private Material defaultNumberMaterial;
    public Material DefaultNumberMaterial => defaultNumberMaterial;

    [SerializeField]
    private Material correctNumberMaterial;
    public Material CorrectNumberMaterial => correctNumberMaterial;

    [SerializeField]
    private Material incorrectNumberMaterial;
    public Material IncorrectNumberMaterial => incorrectNumberMaterial;


    [SerializeField]
    private Material correctCellBgMaterial;
    public Material CorrectCellBgMaterial => correctCellBgMaterial;

    [SerializeField]
    private Material incorrectCellBgMaterial;
    public Material IncorrectCellBgMaterial => incorrectCellBgMaterial;
}
