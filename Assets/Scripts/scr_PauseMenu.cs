using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_PauseMenu : MonoBehaviour {
    // Variables
    [SerializeField] static bool paused = false;
    [SerializeField] GameObject obj_pauseMenu;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject menuUI;

    private bool runnin;

    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // If esc is pressed pause game or resume based on if it is already or not
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (paused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    // Resume Game
    public void Resume () {
        obj_pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Pause Game
    public void Pause () {
        obj_pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Return to Main Menu
    public void QuitGame () {
        Time.timeScale = 1f;
        //SceneManager.LoadScene("scn_MainMenu");
    }

    // Loads Main Menu
    public void MainMenu() {
        /*if (obj_endGame.activeInHierarchy) {
            obj_endGame.SetActive(false);
        }
        else {
            obj_pauseMenu.SetActive(false);
        }
        
        runnin = false;*/
        Time.timeScale = 0f;
    }
    
    // Starts game
    public void Play() {
        // change main camera + ui to in game, versions
        //runnin = true;
        Time.timeScale = 1f;
        menuUI.SetActive(false);
        //*/
        //SceneManager.LoadScene("Store");
    }
}
