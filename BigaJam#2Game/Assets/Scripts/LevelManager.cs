using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.SceneManagement;


public static class LevelManager
{
    private static List<string> _AllScenes;


    public enum LevelTypes { Parkour = 0, Shooting };


    public static void LoadRandomLevel(LevelTypes levelType)
    {
        if (_AllScenes == null)
            Init();
            

    }

    private static void Init()
    {
        _AllScenes = GetAllScenes();

    }

    private static List<string> GetAllScenes()
    {
        List<string> scenes = new List<string>();

        int sceneCount = SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < sceneCount; i++)
        {
            scenes.Add(Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
            Debug.Log("S: " + scenes[scenes.Count - 1]);
        }

        return scenes;
    }
}
