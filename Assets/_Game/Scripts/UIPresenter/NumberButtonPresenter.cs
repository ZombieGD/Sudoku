using DG.Tweening;
using Sudoku.Model;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Sudoku.UI
{
    public class NumberButtonPresenter : MonoBehaviour
    {
        [SerializeField]
        private int buttonValue;
        [SerializeField]
        private InteractiveButton interactiveButton;
        [SerializeField]
        private RectTransform rectTransform;
        [SerializeField]
        private TextMeshProUGUI textMeshProUGUI = null;

        private GameModel gameModel = null;

        [SerializeField]
        private Vector2 defaultButtonSize = new Vector2(120f, 120f);
        [SerializeField]
        private Vector2 expandedButtonSize = new Vector2(120f, 240f);

        [SerializeField]
        private float buttonResizeDuration = 0.5f;
        [SerializeField]
        private Ease expandButtonEase;
        [SerializeField]
        private Ease shrinkButtonEase;

        private Tween buttonSizeTween;

        private void Awake()
        {
            defaultButtonSize.x = rectTransform.sizeDelta.x;
            expandedButtonSize.x = rectTransform.sizeDelta.x;
        }

        private void OnEnable()
        {
            interactiveButton.OnPointerUpAction += OnClick;
            interactiveButton.OnPointerEnterAction += ExpandButton;
            interactiveButton.OnPointerExitAction += ShrinkButton;
        }

        private void OnDisable()
        {
            interactiveButton.OnPointerUpAction -= OnClick;
            interactiveButton.OnPointerEnterAction -= ExpandButton;
            interactiveButton.OnPointerExitAction -= ShrinkButton;
        }

        public void Initialize(GameModel gameModel)
        {
            this.gameModel = gameModel;

            textMeshProUGUI.SetText(buttonValue.ToString());
        }

        public void OnClick()
        {
            SetCellValueCommand command = new SetCellValueCommand(gameModel, buttonValue);
            command.Execute();
        }

        private void ExpandButton()
        {
            if (buttonSizeTween != null && buttonSizeTween.active)
            {
                buttonSizeTween.Kill();
            }

            buttonSizeTween = rectTransform.DOSizeDelta(expandedButtonSize, buttonResizeDuration)
                .SetEase(expandButtonEase);
        }

        private void ShrinkButton()
        {
            if (buttonSizeTween != null && buttonSizeTween.active)
            {
                buttonSizeTween.Kill();
            }

            buttonSizeTween = rectTransform.DOSizeDelta(defaultButtonSize, buttonResizeDuration)
                .SetEase(shrinkButtonEase);
        }
    }
}