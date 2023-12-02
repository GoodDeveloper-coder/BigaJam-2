using System;

using UnityEngine;
using TMPro;



[CreateAssetMenu(fileName = "CutScenePageDisplaySettings", menuName = "CutScenes/Page Display Settings Asset")]
[Serializable]
public class CutScenePageDisplaySettings : ScriptableObject
{
    public PageOptions Page;
    public ImageOptions Image;
    public TextOptions Text;
    public BackgroundColorsOptions BackgroundColors;



    public enum TextDisplayModes
    {
        UseDefault = 0,
        DisplayAllAtOnce = 1,
        TypeOutLetterByLetter = 2
    }



    public void Reset()
    {
        // Default Display Settings
        Page = new PageOptions()
        {
            FadeInDuration = -1f,
            FadeOutDuration = -1f,
            DisplayTime = -1f,
        };

        // Default Image Settings
        Image = new ImageOptions()
        {
            TintColor = Color.white,
        };

        // Default Text Display Settings
        Text = new TextOptions()
        {
            Font = null,
            FontSize = 30f,
            FontStyle = FontStyles.Normal,
            Alignment = TextAlignmentOptions.TopLeft,

            Color = Color.white,
            ColorGradient = null,

            WidthPercentage = 0.5f,
            PaddingAbove = 30,
            DisplayMode = TextDisplayModes.UseDefault,
            TypingAnimationNormalDelayPerLetter = 0.1f,
            TypingAnimationFastDelayPerLetter = 0.02f,
            TypingAnimationSound = null,
            TypingAnimationSoundVolume = 1f,
        };

        // Default Background Colors
        BackgroundColors = new BackgroundColorsOptions()
        {
            BackgroundColor = Color.black,
            TopHalfOfScreenBackgroundColor = new Color32(0, 0, 0, 0),
            BottomHalfOfScreenBackgroundColor = new Color32(0, 0, 0, 0),
            FadeColor = Color.black,
        };

    }



    [Serializable]
    public class PageOptions
    {
        [Tooltip("How long it takes (in seconds) this page to fade in. Leave this set to 0 to have it use the default value in the parent CutSceneContent scriptable object.")]
        [Min(-1f)]
        public float FadeInDuration;

        [Tooltip("How long it takes (in seconds) this page to fade out. Leave this set to 0 to have it use the default value in the parent CutSceneContent scriptable object.")]
        [Min(-1f)]
        public float FadeOutDuration;

        [Tooltip("How long this page will be displayed for (in seconds) before it starts to fade out. Leave this set to 0 to have it use the default value in the parent CutSceneContent scriptable object.")]
        [Min(-1f)]
        public float DisplayTime;
    }


    [Serializable]
    public class TextOptions
    {
        [Tooltip("The font to display the text in. Leave this null to use the TextMeshPro default font.")]
        public TMP_FontAsset Font;

        [Tooltip("The size the text should be displayed in.")]
        [Min(0f)]
        public float FontSize;

        [Tooltip("The style to display the text in. Note that you can select more than one style in this list, too.")]
        public FontStyles FontStyle;

        [Tooltip("How the text is aligned on the page. Note that you can select one option in each row for this setting.")]
        public TextAlignmentOptions Alignment;

        [Tooltip("The color of the text on this page.")]
        public Color32 Color;

        [Tooltip("Lets you set a color gradient for the text. If this is set to null, then the TextColor setting above will be used instead.")]
        public TMP_ColorGradient ColorGradient;

        [Tooltip("Lets you set the width of the text area as a percentage of screen width.")]
        [Range(0f, 1f)]
        public float WidthPercentage;

        [Tooltip("This sets how much space there is between the text area and the bottom of the image (in pixels).")]
        [Min(0)]
        public int PaddingAbove;

        [Tooltip("Specifies how text will be enter the screen after the page fades in.")]
        public TextDisplayModes DisplayMode;

        [Tooltip("The sound that plays while text is typing out onto the screen.")]
        public AudioClip TypingAnimationSound;

        [Tooltip("The volume level of the text typing sound.")]
        [Range(0f, 1f)]
        public float TypingAnimationSoundVolume;

        [Tooltip("Whether or not the player can speed up the cutscene text typing animation on this page by pressing any key or button.")]
        public bool CanSpeedUpText = true;

        [Tooltip("Whether or not the player can skip to the next cutscene page by pressing any key or button after the text typing animation is done.")]
        public bool CanSkipPage = true;

        [Tooltip("The amount of time (in seconds) that will elapse before another letter appears on the screen.")]
        [Min(0f)]
        public float TypingAnimationNormalDelayPerLetter;

        [Tooltip("The amount of time (in seconds) that will elapse before another letter appears on the screen when you hold down any key or gamepad button.")]
        [Min(0f)]
        public float TypingAnimationFastDelayPerLetter;
    }

    [Serializable]
    public class ImageOptions
    {
        [Tooltip("This setting allows you to easily tint the image on this page if you want. This should be set to white with full alpha if you want no tint. You can also change the alpha channel value to make the image look faded out.")]
        public Color32 TintColor;
    }

    [Serializable]
    public class BackgroundColorsOptions
    {
        [Tooltip("The background color of this page. If you only the entire screen this color, then set the alpha to 0 on the Top and Bottom half of screen colors below.")]
        public Color32 BackgroundColor;

        [Tooltip("The background color of the top half of the screen. This can have alpha set to 0 to let BackgroundColor show through instead.")]
        public Color32 TopHalfOfScreenBackgroundColor;

        [Tooltip("The background color of the bottom half of the screen. This can have alpha set to 0 to let BackgroundColor show through instead.")]
        public Color32 BottomHalfOfScreenBackgroundColor;

        [Tooltip("The color that will be used when this page transitions out.")]
        public Color32 FadeColor;
    }

}


