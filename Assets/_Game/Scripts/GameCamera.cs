using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sudoku.Model;
using Sudoku.Presenter;

namespace Sudoku
{
    [RequireComponent(typeof(Camera))]
    public class GameCamera : MonoBehaviour
    {
        [SerializeField]
        private GridCellData cellData;

        [SerializeField]
        private GridCellPresenter gridCellPrefab;

        [SerializeField]
        private Vector2 referenceResolution = new Vector2(1080,1920);
        [SerializeField]
        private float safeAreaTop = 100f;
        [SerializeField]
        private float safeAreaBottom = 660f;
        [SerializeField]
        private float safeAreaLeft = 10f;
        [SerializeField]
        private float safeAreaRight = 10f;

        private Camera cameraComponent = null;

        private GameModel gameModel = null;


        private void Awake()
        {
            cameraComponent = GetComponent<Camera>();
        }

        public void Initialize(GameModel gameModel)
        {
            this.gameModel = gameModel;

            SetupCamera();
        }

        private void SetupCamera()
        {
            float gridWidth = cellData.CellSize.x * gameModel.GridState.ColumnsNumber;
            int rowsNumber = gameModel.GridState.Cells.Length / gameModel.GridState.ColumnsNumber;
            float gridHeight = cellData.CellSize.y * rowsNumber; 

            float realSafeAreaTop = safeAreaTop * Screen.height / referenceResolution.y;
            float realSafeAreaBottom = safeAreaBottom * Screen.height / referenceResolution.y;
            float realSafeAreaLeft = safeAreaLeft * Screen.width / referenceResolution.x;
            float realSafeAreaRight = safeAreaRight * Screen.width / referenceResolution.x;

            float availableHeight = Screen.height - realSafeAreaTop - realSafeAreaBottom;
            float availableWidth = Screen.width - realSafeAreaLeft - realSafeAreaRight;

            float gridSizeRatio = gridWidth / gridHeight;
            float availableAreaRatio = availableWidth / availableHeight;

            float cameraSize = 0;
            if (gridSizeRatio < availableAreaRatio)
            {
                cameraSize = (gridHeight * Screen.height) / (2 * availableHeight);
            }
            else
            {
                cameraSize = (gridWidth / 2) * (Screen.height / availableWidth);
            }

            cameraComponent.orthographicSize = cameraSize;

            Vector3 cameraPosition = transform.position;
            cameraPosition.x = cellData.CellSize.x * (gameModel.GridState.ColumnsNumber - 1) / 2 - cameraSize * Screen.width/Screen.height * (realSafeAreaLeft - realSafeAreaRight)/Screen.width;
            
            cameraPosition.y = -cellData.CellSize.y * (rowsNumber-1) / 2 - 2*cameraSize * (realSafeAreaBottom + gridHeight * Screen.height/(4*cameraSize) - Screen.height /2) / Screen.height;
        
            transform.position = cameraPosition;
        }
    }
}
