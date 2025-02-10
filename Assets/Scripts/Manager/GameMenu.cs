using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : SingletonA<GameMenu>
{
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private GameObject playerControlsUI;
    [SerializeField] private GameObject customiseGameSettingsUI;
    [HideInInspector] public string currentSceneName;
    public string mainMenuSceneName;

    private void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && !ShopSystem.Instance.isShopOpen)
        {
            SwitchWithKeyTo(pauseMenu);
        }

    }

    public void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            QuestionDialogUI.Instance.Hide();
            _menu.SetActive(false);

            CheckForInGameUI();

            return;
        }
        SwitchTo(_menu);
    }

    private void CheckForInGameUI()
    {

        SwitchTo(inGameUI);
    }

    public void SwitchTo(GameObject _menu)
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }


        if (inGameUI != null)
        {
            inGameUI.SetActive(true);
            
        }   

        if (_menu != null)
        {
            _menu.SetActive(true);
        }

        if (GameManager.Instance != null)
        {
            if (_menu == inGameUI)
            {
                Time.timeScale = 1.0f;
                GameManager.Instance.SetCursor(true);
            }
            else
            {
                Time.timeScale = 0;
                GameManager.Instance.SetCursor(false);
            }

        }
    }

    public void Restart()
    {
        //QuestionDialogUI.Instance.ShowQuestion("You will lose ALL YOUR PROGRESS, are you sure?",
        //    () =>
        //    {
        //        Time.timeScale = 1.0f;
        //        //restartGameUI.SetActive(false);
        //        SceneManager.LoadScene(currentSceneName);
        //    },
        //    () =>
        //    {
        //        SwitchWithKeyTo(inGameUI);
        //    });

        Time.timeScale = 1.0f;
        SceneManager.LoadScene(currentSceneName);

        // Debug.Log("To Main Menu (implementing)");
    }

    public void GoToMainMenu()
    {
        QuestionDialogUI.Instance.ShowQuestion("You will lose ALL YOUR PROGRESS, are you sure?",
            () =>
            {
                Time.timeScale = 1.0f;
                //restartGameUI.SetActive(false);
                SceneManager.LoadScene(mainMenuSceneName);
            },
            () =>
            {
                SwitchWithKeyTo(inGameUI);
            });

        // Debug.Log("To Main Menu (implementing)");
    }
}
