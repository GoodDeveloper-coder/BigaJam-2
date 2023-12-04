using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void OnPlayAgainClicked()
    {
        ButtonClickPlayer.Play();

        if (SceneManager.GetActiveScene().name.StartsWith("ParkourLevel"))
            LevelManager.LoadRandomLevel(LevelTypes.Parkour);
        else if (SceneManager.GetActiveScene().name.StartsWith("ShootingLevel"))
            LevelManager.LoadRandomLevel(LevelTypes.Shooting);
    }

    public void OnReturnToMainMenuClicked()
    {
        ButtonClickPlayer.Play();

        SceneManager.LoadScene("MainMenuScene");
    }

}
