using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.SceneManagement;

using Random = UnityEngine.Random;


public enum LevelTypes { Parkour = 0, Shooting = 1};

public static class LevelManager
{
    private static List<string> _AllSceneNames;
    private static List<string> _ParkourLevelSceneNames;
    private static List<string> _ShootingLevelSceneNames;


    public static void LoadRandomLevel(LevelTypes levelType)
    {
        if (_AllSceneNames == null)
            Init();

        int index = -1;
        string selectedSceneName = "";
        if (levelType == LevelTypes.Parkour)
        {
            index = Random.Range(0, _ParkourLevelSceneNames.Count);
            selectedSceneName = _ParkourLevelSceneNames[index];
        }
        else if (levelType == LevelTypes.Shooting)
        {
            index = Random.Range(0, _ShootingLevelSceneNames.Count);
            selectedSceneName = _ShootingLevelSceneNames[index];
        }

        SceneManager.LoadScene(selectedSceneName);
    }

    private static void Init()
    {
        _AllSceneNames = GetAllScenes();

        GetLevels();
    }

    private static void GetLevels()
    {
        _ParkourLevelSceneNames = new List<string>();
        _ShootingLevelSceneNames = new List<string>();

        foreach (string sceneName in _AllSceneNames)
        {
            if (sceneName.StartsWith("ParkourLevel"))
            {
                _ParkourLevelSceneNames.Add(sceneName);
                Debug.Log("P: " + sceneName);
            }
            else if (sceneName.StartsWith("ShootingLevel"))
            {
                _ShootingLevelSceneNames.Add(sceneName);
                Debug.Log("Sh: " + sceneName);
            }              
        }
    }

    private static List<string> GetAllScenes()
    {
        List<string> scenes = new List<string>();

        int sceneCount = SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < sceneCount; i++)
        {
            scenes.Add(Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
            //Debug.Log("S: " + scenes[scenes.Count - 1]);
        }

        return scenes;
    }
}

