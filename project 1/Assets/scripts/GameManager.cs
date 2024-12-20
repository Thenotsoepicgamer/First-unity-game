using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public bool isPaused = false;

    public GameObject pauseMenu;
    public PlayerController playerData;

    public Image healthBar;
    public TextMeshProUGUI clipCounter;
    public TextMeshProUGUI ammoCounter;

    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.Find("Player").GetComponent<PlayerController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
       if (playerData !=null)
        {

            healthBar.fillAmount = Mathf.Clamp(((float)playerData.health / (float)playerData.maxHealth), 0, 1);

            if (playerData.weaponID < 0)
            {
                clipCounter.gameObject.SetActive(false);
                ammoCounter.gameObject.SetActive(false);
            }

            else
            {
                clipCounter.gameObject.SetActive(true);
                clipCounter.text = "Clip: " + playerData.currentMag + "/" + playerData.magSize;

                ammoCounter.gameObject.SetActive(true);
                ammoCounter.text = "Ammo: " + playerData.currentAmmo;
            }

            if (!isPaused && Input.GetKeyDown(KeyCode.Escape))
            {
                isPaused = true;

                pauseMenu.SetActive(true);

                Time.timeScale = 0;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            else if (isPaused && Input.GetKeyDown(KeyCode.Escape))
                Resume();
        }
    }

    public void Resume()
    {
        isPaused = false;

        pauseMenu.SetActive(false);

        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex);
        
    }

    public void LoadLevel(int sceneID)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneID);
    }
}