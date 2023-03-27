using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sudoku.Model;
using Sudoku.Presenter;
using Sudoku.UI;

namespace Sudoku
{
    public class Boot : MonoBehaviour
    {
        [SerializeField]
        private GameCamera gameCamera = null;
        [SerializeField]
        private GridRenderer gridRenderer = null;
        [SerializeField]
        private UIRenderer uiRenderer = null;

        [SerializeField]
        private LevelLayoutData[] levels;

        void Start()
        {
            BootLevel();
        }

        private void BootLevel()

        {
            int levelId = GameManager.Instance.CurrentLevelId;


            string levelDefinition = levels[levelId].LevelDefinition;

            GameModel gameModel = new GameModel(in levelDefinition);

            gameCamera.Initialize(gameModel);
            gridRenderer.Initialize(gameModel);
            uiRenderer.Initialize(gameModel);
        }
    }
}
