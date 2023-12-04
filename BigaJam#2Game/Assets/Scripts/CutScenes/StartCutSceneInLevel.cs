using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;


public class StartCutSceneInLevel : MonoBehaviour
{
    private CutScenePlayer _CutScenePlayer;
    private GameObject _Player1;
    private GameObject _Player2;


    private void Awake()
    {
        GameObject obj = GameObject.Find("Story CutScene");
        if (obj == null || obj.GetComponent<CutScenePlayer>() == null)
            Debug.LogError("CutScenePlayer not found in this scene!");
        else
            _CutScenePlayer = obj.GetComponent<CutScenePlayer>();


        _Player1 = GameObject.Find("Player1");
        if (_Player1 == null)
            Debug.LogError("Player 1 not found in this scene!");

        _Player2 = GameObject.Find("Player2");
        if (_Player1 == null)
            Debug.LogError("Player 2 not found in this scene!");
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCutScene();
    }

    private void StartCutScene()
    {
        EnablePlayers(false);

        _CutScenePlayer.Play();

        StartCoroutine(WaitForCutSceneToEnd());
    }

    private void EnablePlayers(bool state)
    {
        _Player1.SetActive(state);
        _Player2.SetActive(state);
    }

    private IEnumerator WaitForCutSceneToEnd()
    {
        while (_CutScenePlayer.IsPlaying)
            yield return null;


        EnablePlayers(true);
    }
}
