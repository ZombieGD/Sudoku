using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NewGameButton : MonoBehaviour
{
    public void OnClick()
    {
        DOVirtual.DelayedCall(.1f, DelayedNewGame);
    }

    private void DelayedNewGame()
    { 
        GameManager.Instance.NewGame();
    }
}
