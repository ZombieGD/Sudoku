using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    const string CURRENT_LEVEL_ID = "Level";

    public static GameManager Instance;

    [SerializeField]
    private string gameplaySceneName = "Gameplay";

    public int CurrentLevelId { get; private set; }


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void NewGame()
    {
        CurrentLevelId = GetCurrentLevel();
        ++CurrentLevelId;
        CurrentLevelId %= 2;

        SetCurrentLevel(CurrentLevelId);

        SceneManager.LoadScene(gameplaySceneName);
    }

    public void NextLevel()
    {
        ++CurrentLevelId;
        CurrentLevelId %= 2;

        SetCurrentLevel(CurrentLevelId);

        SceneManager.LoadScene(gameplaySceneName);
    }

    private int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt(CURRENT_LEVEL_ID, -1);
    }

    private void SetCurrentLevel(int levelID)
    { 
        PlayerPrefs.SetInt(CURRENT_LEVEL_ID, levelID);
    }

}
