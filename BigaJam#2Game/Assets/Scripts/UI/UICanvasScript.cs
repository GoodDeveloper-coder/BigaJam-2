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
        int _randomNumber = Random.Range(1, 4);
        if (_randomNumber == 1)
        {
            SceneManager.LoadScene("ShootingLevel_01");
        }
        else if (_randomNumber == 2)
        {
            SceneManager.LoadScene("ShootingLevel_02");
        }
        else if (_randomNumber == 3)
        {
            SceneManager.LoadScene("ShootingLevel_03");
        }
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        _clickButtonSound.Play();
        SceneManager.LoadScene("MainMenuScene");
    }
}
