using DG.Tweening;
using Sudoku.Model;
using System;
using UnityEngine;

namespace Sudoku.UI
{
    public class LevelCompletedPopup : MonoBehaviour
    {
        private GameModel gameModel = null;

        [SerializeField]
        private CanvasGroup popupCanvasGrup;

        public void Initialize(GameModel gameModel)
        {
            this.gameModel = gameModel;
            gameModel.OnLevelComplete += OnLevelCompleted;

        }

        private void OnDestroy()
        {
            gameModel.OnLevelComplete -= OnLevelCompleted;
        }

        private void ShowPopup()
        {
            gameObject.SetActive(true);
            popupCanvasGrup.alpha = 0;
            popupCanvasGrup.DOFade(1, 0.5f).SetDelay(3f);
        }

        private void HidePopup(Action onComplete)
        {
            popupCanvasGrup.DOFade(0, 0.1f).OnComplete(() => onComplete?.Invoke());
        }

        private void OnLevelCompleted()
        {
            ShowPopup();
        }

        public void NextLevelClicked()
        {
            HidePopup(OnHidePopup);
        }

        private void OnHidePopup()
        {
            GameManager.Instance.NextLevel();
        }
    }
}
