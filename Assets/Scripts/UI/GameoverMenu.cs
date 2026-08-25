using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverMenu : MonoBehaviour
{
    public GameObject gameOverMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowGameOver() //spawn the gameover screen and pause the game
    {
        gameOverMenu.SetActive(false);
        Time.timeScale = 0;
    }

    public void RestartGame() //load the game scene to restart
    {
        SceneManager.LoadScene("Dungeon");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode(); //close in editor
        #else
            Application.Quit();  //close game
        #endif
    }
}
