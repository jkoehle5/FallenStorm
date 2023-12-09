using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_PauseMenu : MonoBehaviour {
    // Variables
    [SerializeField] public bool paused;
    [SerializeField] private GameObject obj_pauseMenu;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject endGame;
    [SerializeField] private GameObject tutorial;
    [SerializeField] private GameObject win;
    [SerializeField] private GameObject lose;

    [SerializeField] private bool runnin;
    private bool gameEnded;

    void Start() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        paused = false;
        gameEnded = false;
    }

    // If esc is pressed pause game or resume based on if it is already or not
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && runnin) {
            if (paused) {
                Resume();
            } else {
                Pause();
            }
        }
    }
/*
    public void WinGame() {
        obj_pauseMenu.SetActive(true);
    }

    public void LoseGame() {
        obj_pauseMenu.SetActive(true);
    }
*/
    // Resume Game
    public void Resume () {
        obj_pauseMenu.SetActive(false);
        inGameUI.gameObject.SetActive(true);
        Time.timeScale = 1f;
        paused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Pause Game
    public void Pause () {
        obj_pauseMenu.SetActive(true);
        inGameUI.gameObject.SetActive(false);
        Time.timeScale = 0f;
        paused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Exits Game
    public void QuitGame () {
        //Time.timeScale = 1f;
        //SceneManager.LoadScene("MainMenu");
        Application.Quit();
    }

    // Loads Main Menu
    public void MainMenu() {
        SceneManager.LoadScene("MainMenu");
        if (endGame.activeInHierarchy) {
            endGame.SetActive(false);
            endTheGame(false);
        }
        else {
            obj_pauseMenu.SetActive(false);
        }
        
        menuUI.SetActive(true);
        runnin = false;
        Time.timeScale = 0f;
    }
    
    // Starts game
    public void Play() {
        menuUI.SetActive(false);
        tutorial.SetActive(true);
    }

    public void Starting() {
        // change main camera + ui to in game, versions
        runnin = true;
        Time.timeScale = 1f;
        //tutorial.SetActive(false);
        //inGameUI.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene("Ground");
    }

    public void endTheGame(bool gameEnd) {
        gameEnded = gameEnd;
    }

    // Ends Game
    public void EndGame(bool winLose) {
        endGame.SetActive(true);
        inGameUI.gameObject.SetActive(false);
        Time.timeScale = 0f;
        
        if (winLose) {
            win.SetActive(true);
        } else { 
            lose.SetActive(true);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
