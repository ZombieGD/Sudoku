using Sudoku.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;
using Lean.Common;
using TMPro;
using DG.Tweening;

namespace Sudoku.Presenter
{
    public class GridCellPresenter : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer cellBgRenderer;

        [SerializeField]
        private TextMeshPro cellNumberText;

        [SerializeField]
        private LeanSelectableByFinger selectableByFinger;

        private GameCommand selectCellCommand = null;
        /*
        [SerializeField]
        private float completedHighlightDelaySeconds = 0.25f;
        [SerializeField]
        private float completedHighlightDurationSeconds = 0.5f;
        [SerializeField]
        private float correctHighlightDurationSeconds = 0.5f;
        [SerializeField]
        private float incorrectHighlightDurationSeconds = 0.5f;
        [SerializeField]
        private Vector3 completedPunchScale = new Vector3(0.1f, 0.1f, 0f);
        [SerializeField]
        private Vector3 correctPunchScale = new Vector3(0.1f, 0.1f, 0f);
        [SerializeField]
        private int completedPunchVibrato = 10;
        [SerializeField]
        private int correctPunchVibrato = 10;
        [SerializeField]
        private float incorrectShakeStrenght = 0.1f;
        [SerializeField]
        private int incorrectShakeVibrato = 10;
        [SerializeField]
        private Color completedHighLightColor = Color.white;
        [SerializeField]
        private Color correctHighLightColor = Color.white;
        [SerializeField]
        private Color incorrectHighLightColor = Color.white;
        [SerializeField]
        private Ease completedHighlightEase = Ease.Flash;
        [SerializeField]
        private Ease correctHighlightEase = Ease.Flash;
        [SerializeField]
        private Ease incorrectHighlightEase = Ease.Flash;

        [SerializeField]
        private Material defaultNumberMaterial;
        [SerializeField]
        private Material correctNumberMaterial;
        [SerializeField]
        private Material incorrectNumberMaterial;

        [SerializeField]
        private Material correctCellBgMaterial;
        [SerializeField]
        private Material incorrectCellBgMaterial;
        */
        [SerializeField]
        private GridCellData cellData;

        private Vector3 cachedPosition;
        private Material cachedMaterial;
        private Tween highlightTween;

        private void Awake()
        {
            selectableByFinger.OnSelected.AddListener(OnSelectedByFinger);
        }

        private void OnDestroy()
        {
            selectableByFinger.OnSelected.RemoveAllListeners();
        }

        public void Initialize(GameCommand selectCellCommand)
        { 
            this.selectCellCommand = selectCellCommand;
        }

        private void OnSelectedByFinger(LeanSelect leanSelect)
        {
            selectCellCommand?.Execute();
        }

        public void SetCellValue(int value)
        {
            cellNumberText.gameObject.SetActive(value != 0);
            cellNumberText.text = value.ToString();
        }

        public void ClearHighLight()
        {
            cellBgRenderer.gameObject.SetActive(false);
        }

        public void HighlightSelectedCell()
        { 
            cellBgRenderer.gameObject.SetActive(true);
            //cellBgRenderer.material.color = selectedCellColor;
        }

        public void HighlightSelectedRegionCell()
        { 
            //cellMeshRenderer.material.color = selectedRegionCellColor;
        }

        public void HighlightMatchingCell()
        { 
            cellBgRenderer.gameObject.SetActive(true);
            //cellMeshRenderer.material.color = matchingCellColor;
        }

        public void OnDefaultValueSet()
        { 
            cellNumberText.fontMaterial = cellData.DefaultNumberMaterial;
            cachedMaterial = cellNumberText.fontSharedMaterial;
            cellBgRenderer.material = cellData.CorrectCellBgMaterial;
        }

        public void OnCorrectValueSet()
        {
            cellNumberText.fontMaterial = cellData.CorrectNumberMaterial;
            cachedMaterial = cellNumberText.fontSharedMaterial;
            cellBgRenderer.material = cellData.CorrectCellBgMaterial;
            HighlighCorrect();
        }

        public void OnIncorrectValueSet()
        {
            cellNumberText.fontMaterial = cellData.IncorrectNumberMaterial;
            cachedMaterial = cellNumberText.fontSharedMaterial;
            cellBgRenderer.material = cellData.IncorrectCellBgMaterial;
            HighlightIncorrect();
        }

        public void HighlightCompleted(int delay)
        {
            if (highlightTween != null && highlightTween.active)
            {
                highlightTween.Kill();
                RestorePosition();
                RestoreMaterial();
            }

            float delaySeconds = cellData.CompletedHighlightDelaySeconds * delay;
            cachedMaterial = cellNumberText.fontSharedMaterial;
            Color color = cellNumberText.fontMaterial.GetColor("_FaceColor");
            Sequence colorSequence = DOTween.Sequence();
            colorSequence.Append(DOVirtual.Color(color, cellData.CompletedHighLightColor, cellData.CompletedHighlightDurationSeconds, UpdateColor)
                .SetDelay(delaySeconds)
                .SetEase(cellData.CompletedHighlightEase));
            colorSequence.Join(cellNumberText.transform.DOPunchScale(cellData.CompletedPunchScale, cellData.CompletedHighlightDurationSeconds * 2, cellData.CompletedPunchVibrato));
            colorSequence.Append(DOVirtual.Color(cellData.CompletedHighLightColor, color, cellData.CompletedHighlightDurationSeconds, UpdateColor)
                .SetEase(cellData.CompletedHighlightEase));
            colorSequence.Append(DOVirtual.DelayedCall(0.1f, RestoreMaterial));
            highlightTween = colorSequence;
        }

        public void HighlighCorrect()
        {
            if (highlightTween != null && highlightTween.active)
            {
                highlightTween.Kill();
                RestorePosition();
                RestoreMaterial();
            }

            cachedMaterial = cellNumberText.fontSharedMaterial;
            Color color = cellNumberText.fontMaterial.GetColor("_FaceColor");
            Sequence colorSequence = DOTween.Sequence();
            colorSequence.Append(DOVirtual.Color(color, cellData.CorrectHighLightColor, cellData.CorrectHighlightDurationSeconds, UpdateColor)
                .SetEase(cellData.CorrectHighlightEase));
            colorSequence.Join(cellNumberText.transform.DOPunchScale(cellData.CorrectPunchScale, cellData.CorrectHighlightDurationSeconds * 2, cellData.CorrectPunchVibrato));
            colorSequence.Append(DOVirtual.Color(cellData.CorrectHighLightColor, color, cellData.CorrectHighlightDurationSeconds, UpdateColor)
                .SetEase(cellData.CorrectHighlightEase));
            colorSequence.Append(DOVirtual.DelayedCall(0.1f, RestoreMaterial));
            highlightTween = colorSequence;
        }

        public void HighlightIncorrect()
        {
            if (highlightTween != null && highlightTween.active)
            {
                highlightTween.Kill();
                RestorePosition();
                RestoreMaterial();
            }

            cachedPosition = cellNumberText.transform.localPosition;
            cachedMaterial = cellNumberText.fontSharedMaterial;
            Color color = cellNumberText.fontMaterial.GetColor("_FaceColor");
            Sequence colorSequence = DOTween.Sequence();
            colorSequence.Append(DOVirtual.Color(color, cellData.IncorrectHighLightColor, cellData.IncorrectHighlightDurationSeconds, UpdateColor)
                .SetEase(cellData.IncorrectHighlightEase));
            colorSequence.Join(cellNumberText.transform.DOShakePosition(cellData.IncorrectHighlightDurationSeconds, cellData.IncorrectShakeStrenght * 2, cellData.IncorrectShakeVibrato));
            colorSequence.Append(DOVirtual.Color(cellData.IncorrectHighLightColor, color, cellData.IncorrectHighlightDurationSeconds, UpdateColor)
                 .SetEase(cellData.IncorrectHighlightEase));
            colorSequence.Append(DOVirtual.DelayedCall(0.1f, () => { RestorePosition(); RestoreMaterial(); }));
            highlightTween = colorSequence;
        }

        private void UpdateColor(Color color)
        { 
            cellNumberText.fontMaterial.SetColor("_FaceColor", color);
        }

        private void RestorePosition()
        {
            cellNumberText.transform.localPosition = cachedPosition;
        }
        private void RestoreMaterial()
        {
            cellNumberText.fontSharedMaterial = cachedMaterial;
        }
    }
}
