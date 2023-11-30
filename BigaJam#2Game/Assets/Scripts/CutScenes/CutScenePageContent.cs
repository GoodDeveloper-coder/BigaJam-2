using System;

using UnityEngine;



/// <summary>
/// This class stores the content data of a cutscene.
/// </summary>
/// <remarks>
/// To create a new display settings asset, go to ScriptableObjects\CutScenes\PageDisplaySettings in the project. Right click in the project pane and choose "Create->CutScenes->Page Display Settings Asset".
/// This will create a new cut scene content asset. You can then drag that asset into a CutScenePlayer component in your scene. Alternatively, you
/// </remarks>
[Serializable]
public class CutScenePageContent
{
    [Header("Page Content")]

    [Tooltip("The display settings for this page. If this is set to null, then the default display settings of the parent cutscene will be used instead.")]
    public CutScenePageDisplaySettings DisplaySettings = null;

    [Tooltip("The image that will be displayed on this page of the cutscene.")]
    public Sprite Image;

    [Tooltip("The text that will be displayed on this page of the cutscene.")]
    [TextArea(minLines: 5, maxLines: 10)]
    public string Text;

}
