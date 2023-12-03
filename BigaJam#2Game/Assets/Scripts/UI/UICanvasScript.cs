using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UICanvasScript : MonoBehaviour
{
    [SerializeField] private AudioSource _clickButtonSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        _clickButtonSound.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        _clickButtonSound.Play();
        SceneManager.LoadScene("MainMenuScene");
    }
}
