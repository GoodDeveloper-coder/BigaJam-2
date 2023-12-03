using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.InputSystem.Controls;
using System.Runtime.CompilerServices;



/// <summary>
/// This class plays a cut scene.
/// </summary>
/// <remarks>
/// To create a new cut scene, go to ScriptableObjects\CutScenes in the project. Right click in the project pane and choose "Create->CutScenes->Content Asset".
/// This will create a new cut scene content asset. You can then drag that asset into a CutScenePlayer component in your scene. Alternatively, you
/// can set it in code via the CutSceneContent property.
/// 
/// All cut scene content fields have tooltips in the inspector.
/// 
/// You can play the cutscene by calling the Play() or PlayAfter() methods on the CutScenePlayer component. There is also a Stop() method.
/// 
/// If you should want to know whether a cut scene is playing, you can use the IsPlaying property.
/// </remarks>
[RequireComponent(typeof(AudioSource))] // For the background music.
public class CutScenePlayer : MonoBehaviour
{
    [Header("Cutscene Content")]
    [SerializeField] CutSceneContent _CutSceneContent;

    

    private Image _ImageDisplay;
    private TMP_Text _TextDisplay;
    private RectTransform _TextDisplayRect;
    private RectTransform _TextDisplayParentRect;

    private Image _Background;
    private Image _TopHalfOfScreenBackground;
    private Image _BottomHalfOfScreenBackground;
    private Image _PageFadePanelBackground;

    private bool _IsCutSceneFading; // Whether the cut scene is fading in or out.
    private bool _IsPageFading; // Whether a page is fading in or out.

    private CutScenePageContent _CurrentPage;
    private CutScenePageDisplaySettings _DisplaySettings;

    private float _PageStartTime;

    private AudioSource _MusicSource;
    private AudioSource _TextTypingSource;

    private bool _TextIsTyping;



    private void Awake()
    {      
        _ImageDisplay = GetChildComponent<Image>("Background Panel/TopHalfOfScreen/Image");
        _TextDisplay = GetChildComponent<TMP_Text>("Background Panel/BottomHalfOfScreen/Text (TMP)");
        _TextDisplayRect = _TextDisplay.GetComponent<RectTransform>();
        _TextDisplayParentRect = _TextDisplayRect.parent.GetComponent<RectTransform>();

        _MusicSource = GetComponent<AudioSource>();
        _TextTypingSource = _TextDisplay.GetComponent<AudioSource>();

        _Background = GetChildComponent<Image>("Background Panel");
        _TopHalfOfScreenBackground = GetChildComponent<Image>("Background Panel/TopHalfOfScreen");
        _BottomHalfOfScreenBackground = GetChildComponent<Image>("Background Panel/BottomHalfOfScreen");

        _PageFadePanelBackground = GetChildComponent<Image>("Fade Panel");
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }


    public void Play()
    {
        if (!ValidateCutScene())
            return;


        StartCoroutine(PlayCutScene());
    }

    public void PlayAfter(float delay)
    {
        if (!ValidateCutScene())
            return;


        delay = Mathf.Max(0, delay);

        StartCoroutine(PlayCutScene());

    }

    public void Stop()
    {
        StopCoroutine(PlayCutScene());

        IsPlaying = false;
    }

    private bool ValidateCutScene()
    {
        if (_CutSceneContent == null || _CutSceneContent.Pages == null || _CutSceneContent.Pages.Count < 1)
        {
            Debug.LogError("Cannot play cutscene because no content is specified, or it contains no pages!");
            return false;
        }


        if (_CutSceneContent.DefaultDisplaySettings == null)
        {
            bool result = false;
            foreach (CutScenePageContent page in _CutSceneContent.Pages)
            {
                if (page.DisplaySettings == null)
                { 
                    result = true;
                    break;
                }
            }

            if (result)
            {
                Debug.LogError("Cannot play cutscene because its DefaultDisplaySettings option is null and at least one page does not provide its own display settings!");
                return false;
            }
        }


        if (IsPlaying)
        {
            Debug.LogWarning("Play() was called while the cutscene is already playing!");
            return false;
        }


        return true;
    }

    private IEnumerator PlayCutScene()
    {
        IsPlaying = true;
        InitCutscenePlayback();


        // Fade in the entire cutscene GUI before we begin playing the cutscene itself.
        yield return StartCoroutine(Fade(0,
                                         255,
                                         _CutSceneContent.CutsceneFadeInTime,
                                         true));


        // Loop through all pages of the cutscene and display each.
        for (int i = 0; i < _CutSceneContent.Pages.Count; i++)
        {
            _PageStartTime = Time.time;

            _CurrentPage = _CutSceneContent.Pages[i];
            _DisplaySettings = GetPageDisplaySettings(_CurrentPage);

            SetPageColors();
            SetPageContent();


            yield return StartCoroutine(Fade(255, 
                                             0, 
                                             GetPageFadeInTime()));

            yield return StartCoroutine(DisplayPage());

            yield return StartCoroutine(Fade(0, 
                                             255, 
                                             GetPageFadeOutTime()));

            // If the cutscene was interupted, then break out of this loop.
            if (!IsPlaying)
                break;
        }


        ClearPage(); // So no text or image is visible as the cutscene UI fades out.

        // The cutscene is done, so fade out the entire cutscene UI.
        yield return StartCoroutine(Fade(255,
                                         0,
                                         _CutSceneContent.CutsceneFadeOutTime,
                                         true));


        IsPlaying = false;

        StopBackgroundMusic();

        _Background.gameObject.SetActive(false);
        _PageFadePanelBackground.gameObject.SetActive(false);
    }

    private void InitCutscenePlayback()
    {
        StartCoroutine(PlayBackgroundMusic());

        _Background.gameObject.SetActive(true);
        _PageFadePanelBackground.gameObject.SetActive(true);


        _CurrentPage = _CutSceneContent.Pages[0];
        _DisplaySettings = GetPageDisplaySettings(_CurrentPage);

        SetPageColors();
        ClearPage();
    }

    private IEnumerator DisplayPage()
    {
        if (GetTextDisplayMode() == CutScenePageDisplaySettings.TextDisplayModes.TypeOutLetterByLetter)
            yield return StartCoroutine(TypeTextOnPage());
        // NOTE: We don't have code here for if it is set to TextDisplayModes.DisplayAllAtOnce, because the SetPageContent() function already did that.


        // Wait until it is time for the next page in the cutscene.
        while (Time.time - _PageStartTime <= GetPageDisplayTime())
        {

            // Bail out if the player decides to skip the cutscene.
            if (_CutSceneContent.CanSkipCutScene && IsSkipCutSceneKeyPressed())
            {
                IsPlaying = false;
                yield break;
            }


            if (CanSkipPage() && IsAnyKeyOrButtonPressed())
                yield break;


            yield return null;
        }

    }

    private bool IsSkipCutSceneKeyPressed()
    {
        return _CutSceneContent.Player1_SkipCutSceneInputAction != null && _CutSceneContent.Player1_SkipCutSceneInputAction.action.IsPressed() ||
               _CutSceneContent.Player2_SkipCutSceneInputAction != null && _CutSceneContent.Player2_SkipCutSceneInputAction.action.IsPressed();

    }
    private IEnumerator Fade(byte startAlpha, byte endAlpha, float fadeDuration, bool isIntroOrOutroFade = false)
    {
        _IsPageFading = true;
        if (isIntroOrOutroFade)
            _IsCutSceneFading = true;


        float elapsedTime = 0;


        Color32 color = _PageFadePanelBackground.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            float percent = elapsedTime / fadeDuration;
            byte alpha = (byte) Mathf.Lerp(startAlpha, endAlpha, percent);

            if (!isIntroOrOutroFade)
            {
                SetFadeColor(_PageFadePanelBackground, alpha);
            }
            else
            {
                SetFadeColor(_PageFadePanelBackground, alpha);

                SetFadeColor(_Background, alpha);
                SetFadeColor(_TopHalfOfScreenBackground, alpha);
                SetFadeColor(_BottomHalfOfScreenBackground, alpha);

                // If we are fading out the entire cutscene, make the image and text have min alpha already. Otherwise the image in particular tends to lag behind the backgrounds. 
                if (isIntroOrOutroFade)
                {
                    byte newAlpha = startAlpha < endAlpha ? startAlpha : endAlpha;
                    SetFadeColor(_ImageDisplay, newAlpha);
                    SetFadeColor(_TextDisplay, newAlpha);
                }
                else
                {
                    SetFadeColor(_ImageDisplay, alpha);
                    SetFadeColor(_TextDisplay, alpha);
                }

                float volumePercent = percent;
                if (endAlpha < startAlpha)
                    volumePercent = 1.0f - volumePercent;
                
                _MusicSource.volume = _CutSceneContent.BackgroundMusicVolume * volumePercent;
                _TextTypingSource.volume = GetTextTypingVolume() * volumePercent;
            }

            yield return null;
        }

        color.a = endAlpha;
        _PageFadePanelBackground.color = color;


        _IsPageFading = false;
        if (isIntroOrOutroFade)
            _IsCutSceneFading = false;
    }

    private void SetFadeColor(Image image, byte alpha)
    {
        Color32 color = image.color;
        color.a = alpha;

        image.color = color;
    }

    private void SetFadeColor(TMP_Text text, byte alpha)
    {
        Color32 color = text.color;
        color.a = alpha;

        text.color = color;
    }

    private void SetPageColors()
    {        
        _Background.color = _DisplaySettings.BackgroundColors.BackgroundColor;
        _TopHalfOfScreenBackground.color = _DisplaySettings.BackgroundColors.TopHalfOfScreenBackgroundColor;
        _BottomHalfOfScreenBackground.color = _DisplaySettings.BackgroundColors.BottomHalfOfScreenBackgroundColor;
        
        _ImageDisplay.color = _DisplaySettings.Image.TintColor;

        _TextDisplay.color = _DisplaySettings.Text.Color;
        _TextDisplay.colorGradientPreset = _DisplaySettings.Text.ColorGradient;
        _TextDisplay.enableVertexGradient = _DisplaySettings.Text.ColorGradient != null;
    }

    private void SetPageContent()
    {
        // Setup the image display.
        _ImageDisplay.sprite = _CurrentPage.Image;


        _TextDisplay.alignment = _DisplaySettings.Text.Alignment;


        // Setup the text display.
        UpdateTextAreaBounds();

        if (_DisplaySettings.Text.Font != null)
            _TextDisplay.font = _DisplaySettings.Text.Font;

        _TextDisplay.fontSize = _DisplaySettings.Text.FontSize;
        _TextDisplay.fontStyle = _DisplaySettings.Text.FontStyle;

        
        // Show the text depending on display mode.
        if (GetTextDisplayMode() != CutScenePageDisplaySettings.TextDisplayModes.TypeOutLetterByLetter)
            _TextDisplay.text = _CurrentPage.Text;
        else
            _TextDisplay.text = ""; // Clear the text so there is nothing already there when text starts typing on the screen.
    }

    private void UpdateTextAreaBounds()
    {
        float widthPercentage = GetTextAreaWidthPercentage();
        float remainingWidth = (_TextDisplayParentRect.rect.width * widthPercentage);
        

        _TextDisplayRect.offsetMin = new Vector2(remainingWidth / 2f,
                                                 0f);                                                 
        _TextDisplayRect.offsetMax = new Vector2(-remainingWidth / 2f,
                                                 -GetTextAreaPaddingAbove());
     
    }

    private IEnumerator TypeTextOnPage()
    {
        if (_CurrentPage.Text.Length == 0)
            yield break;
      

        GetTextTypingCharDelays(out float normalDelay, out float fastDelay);
        WaitForSeconds waitNormalDelay = new WaitForSeconds(normalDelay);
        WaitForSeconds waitFastDelay = new WaitForSeconds(fastDelay);


        _TextIsTyping = true;

        StartCoroutine(PlayTextTypingSound());
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < _CurrentPage.Text.Length; i++) 
        {
            // This lets us exit early in case the cutscene was stopped.
            if (!_TextIsTyping || !IsPlaying)
                break;


            builder.Append(_CurrentPage.Text[i]);

            _TextDisplay.text = builder.ToString();


            if (CanSpeedUpText() && IsAnyKeyOrButtonPressed())
                yield return waitFastDelay;
            else
                yield return waitNormalDelay;


            // Bail out if the player decides to skip the cutscene.
            if (_CutSceneContent.CanSkipCutScene && IsSkipCutSceneKeyPressed())
            {
                IsPlaying = false;
                yield break;
            }


            yield return null;
        }


        _TextIsTyping = false;
    }

    private IEnumerator PlayBackgroundMusic()
    {
        if (_CutSceneContent.BackgroundMusic.Count < 1)
        {
            Debug.LogWarning("Cannot play background music because this cutscene does not specify any!");
            yield break;
        }


        int musicIndex = 0;
        _MusicSource.volume = _CutSceneContent.BackgroundMusicVolume;
        while (IsPlaying)
        {
            if (!_MusicSource.isPlaying)
            {
                _MusicSource.clip = _CutSceneContent.BackgroundMusic[musicIndex];
                _MusicSource.Play();

                musicIndex++;
                if (musicIndex >= _CutSceneContent.BackgroundMusic.Count)
                    musicIndex = 0;
            }


            yield return null;

        } // end while


        _MusicSource.Stop();
    }

    private IEnumerator PlayTextTypingSound()
    {
        if ((_CurrentPage.DisplaySettings == null || (_CurrentPage != null && _CurrentPage.DisplaySettings != null && _CurrentPage.DisplaySettings.Text.TypingAnimationSound == null)) && 
            _CutSceneContent.DefaultDisplaySettings.Text.TypingAnimationSound == null)         
        {
            Debug.LogWarning("Cannot play text typing sound because this cutscene does not specify any!");
            yield break;
        }


        // Get the text typing sound clip
        AudioClip clip = _CutSceneContent.DefaultDisplaySettings.Text.TypingAnimationSound;
        if (_CurrentPage != null && _CurrentPage.DisplaySettings != null &&
            _CurrentPage.DisplaySettings.Text.TypingAnimationSound != null)
        {
            clip = _CurrentPage.DisplaySettings.Text.TypingAnimationSound;
        }


        _TextTypingSource.clip = clip;
        _TextTypingSource.volume = GetTextTypingVolume();

        while (_TextIsTyping)
        {
            if (!_TextTypingSource.isPlaying)
                _TextTypingSource.Play();


            yield return null;

        } // end while


        _TextTypingSource.Stop();
    }

    private CutScenePageDisplaySettings.TextDisplayModes GetTextDisplayMode()
    {
        CutScenePageDisplaySettings.TextDisplayModes displayMode = _CutSceneContent.DefaultDisplaySettings.Text.DisplayMode;

        if (_CurrentPage.DisplaySettings != null && _CurrentPage.DisplaySettings.Text.DisplayMode != CutScenePageDisplaySettings.TextDisplayModes.UseDefault)
            displayMode = _CurrentPage.DisplaySettings.Text.DisplayMode;

        return displayMode;
    }

    private void GetTextTypingCharDelays(out float normalDelay, out float fastDelay)
    {
        normalDelay = 0f;
        if (_CurrentPage.DisplaySettings != null && _CurrentPage.DisplaySettings.Text.TypingAnimationNormalDelayPerLetter > 0f)
            normalDelay = _DisplaySettings.Text.TypingAnimationNormalDelayPerLetter;
        else
            normalDelay = _CutSceneContent.DefaultDisplaySettings.Text.TypingAnimationNormalDelayPerLetter;


        fastDelay = 0f;
        if (_CurrentPage.DisplaySettings != null && _CurrentPage.DisplaySettings.Text.TypingAnimationFastDelayPerLetter > 0f)
            fastDelay = _DisplaySettings.Text.TypingAnimationFastDelayPerLetter;
        else
            fastDelay = _CutSceneContent.DefaultDisplaySettings.Text.TypingAnimationFastDelayPerLetter;
    }

    private float GetTextTypingVolume()
    {
        float volume = _CutSceneContent.DefaultDisplaySettings.Text.TypingAnimationSoundVolume;

        if (_CurrentPage != null && _CurrentPage.DisplaySettings != null && _CurrentPage.DisplaySettings.Text.TypingAnimationSoundVolume > 0f)
            volume = _CurrentPage.DisplaySettings.Text.TypingAnimationSoundVolume;

        return volume;
    }

    private int GetTextAreaPaddingAbove()
    {
        int padding = _CutSceneContent.DefaultDisplaySettings.Text.PaddingAbove;

        if (_CurrentPage != null && _CurrentPage.DisplaySettings != null && _CurrentPage.DisplaySettings.Text.PaddingAbove > 0)
            padding = _CurrentPage.DisplaySettings.Text.PaddingAbove;

        return padding;
    }

    private float GetTextAreaWidthPercentage()
    {
        float width = _CutSceneContent.DefaultDisplaySettings.Text.WidthPercentage;

        if (_CurrentPage != null && _CurrentPage.DisplaySettings != null && _CurrentPage.DisplaySettings.Text.WidthPercentage > 0)
            width = _CurrentPage.DisplaySettings.Text.WidthPercentage;

        return width;
    }

    private float GetPageFadeInTime()
    {
        float fadeTime = _CutSceneContent.DefaultPageFadeInDuration;

        if (_CurrentPage != null && _CurrentPage.DisplaySettings != null && _CurrentPage.DisplaySettings.Page.FadeInDuration > 0f)
            fadeTime = _CurrentPage.DisplaySettings.Page.FadeInDuration;

        return fadeTime;
    }

    private float GetPageFadeOutTime()
    {
        float fadeTime = _CutSceneContent.DefaultPageFadeOutDuration;

        if (_CurrentPage != null && _CurrentPage.DisplaySettings != null && _CurrentPage.DisplaySettings.Page.FadeOutDuration > 0f)
            fadeTime = _CurrentPage.DisplaySettings.Page.FadeOutDuration;

        return fadeTime;
    }

    private float GetPageDisplayTime()
    {
        float displayTime = _CutSceneContent.DefaultPageDisplayTime;

        if (_CurrentPage != null && _CurrentPage.DisplaySettings != null && _CurrentPage.DisplaySettings.Page.DisplayTime > 0f)
            displayTime = _CurrentPage.DisplaySettings.Page.DisplayTime;

        return displayTime;
    }

    private bool IsAnyKeyOrButtonPressed()
    {
        bool isKeyboardKeyPressed = Keyboard.current != null && Keyboard.current.anyKey.isPressed;
        bool isGamepadButtonPressed = Gamepad.current != null && Gamepad.current.allControls.Any(x => x is ButtonControl button && x.IsPressed() && !x.synthetic);

        return isKeyboardKeyPressed || isGamepadButtonPressed;
    }

    private bool CanSpeedUpText()
    {
        bool result = _CutSceneContent.DefaultDisplaySettings.Text.CanSpeedUpText;

        if (_CurrentPage != null && _CurrentPage.DisplaySettings != null)
            result = _CurrentPage.DisplaySettings.Text.CanSpeedUpText;

        return result;
    }

    private bool CanSkipPage()
    {
        bool result = _CutSceneContent.DefaultDisplaySettings.Text.CanSkipPage;

        if (_CurrentPage != null && _CurrentPage.DisplaySettings != null)
            result = _CurrentPage.DisplaySettings.Text.CanSkipPage;

        return result;
    }

    private void StopBackgroundMusic()
    {
        StopCoroutine(PlayBackgroundMusic());
        _MusicSource.Stop();
    }

    private void ClearPage()
    {
        _ImageDisplay.sprite = null;
        _ImageDisplay.color = _TopHalfOfScreenBackground.color;
        _TextDisplay.text = "";
    }

    private CutScenePageDisplaySettings GetPageDisplaySettings(CutScenePageContent page)
    {
        CutScenePageDisplaySettings displaySettings = null;

        if (page.DisplaySettings != null)
            displaySettings = page.DisplaySettings;
        else
            displaySettings = _CutSceneContent.DefaultDisplaySettings;

        return displaySettings;
    }

    private T GetChildComponent<T>(string path)
    {
        Transform t = transform.Find(path);
        if (t == null)
            Debug.LogError($"Cutscene component failed to find child object \"{path}\"");

        T component = t.GetComponent<T>();
        if (component == null)
            Debug.LogError($"Cutscene component's child object \"{path}\" is missing its {typeof(T).Name} component!");


        return component;
    }



    public CutSceneContent CutScene
    {
        get { return _CutSceneContent; }
        set { _CutSceneContent = value; }
    }



    public bool IsFading { get { return _IsCutSceneFading || _IsPageFading; } }
    public bool IsPlaying { get; private set; }

}
