using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.Networking;

using Random = UnityEngine.Random;


/// <summary>
/// The music manager plays background music in a scene.
/// </summary>
/// <remarks>
/// It expects the music files to be in Assets/StreamingAssets as per Hea7en's request.
/// 
/// There are also a handful of options to change various aspects of how the music plays,
/// such as volume level and random order or not.
/// </remarks>
public class MusicManager : MonoBehaviour
{
    [Header("Music File Search Settings")]
    [SerializeField] private string _MusicFileExtensions = "*.mp3,*.ogg,*.wav,*.m4a";

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _AudioSource1;
    [SerializeField] private AudioSource _AudioSource2;
   
    [Header("Playback Settings")]
    [SerializeField] [Range(0f, 1f)] private float _MusicVolume = 1f;
    [SerializeField] private MusicFadeTypes _FadeType = MusicFadeTypes.FadeOutThenFadeIn;
    [SerializeField] private float _CrossFadeDuration = 2f;

    [SerializeField] private bool _PlayInRandomOrder = true;



    private List<AudioClip> _MusicTracks = new List<AudioClip>();
    private bool _MusicIsLoaded = false;

    private AudioSource _CurrentAudioSource;
    private AudioSource _PrevAudioSource;

    private int _CurrentMusicTrackIndex;

    private MusicManager _Instance;



    private enum MusicFadeTypes { FadeOutThenFadeIn, CrossFade }



    private void Awake()
    {
        if (_Instance != null)
        {
            Debug.LogWarning("A sound manager has already been created in this scene. Self-destructing!");
            Destroy(gameObject);
        }


        _Instance = this;

        Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_MusicIsLoaded || _CurrentAudioSource == null || _AudioSource1 == null || _AudioSource2 == null || _MusicTracks.Count < 1)
            return;
        else if (!IsCrossfading && GetTimeLeft(_CurrentAudioSource) <= _CrossFadeDuration)
            StartCoroutine(StartNextTrack());
    }



    private void Init()
    {
        _MusicFileExtensions = _MusicFileExtensions.ToLower();

        string musicPath = Application.streamingAssetsPath;
        StartCoroutine(LoadMusic(musicPath));


        _CurrentAudioSource = _AudioSource1;
    }

    private IEnumerator StartNextTrack()
    {
        IsCrossfading = true;

        GetNextTrack();
        // Check if GetNextTrack() had an error.
        if (_CurrentMusicTrackIndex < 0)
            yield break;

        Debug.Log("starting next track!");
        if (_FadeType == MusicFadeTypes.FadeOutThenFadeIn)
            yield return StartCoroutine(FadeOutAndFadeInNextTrack());
        else
            yield return StartCoroutine(CrossFade());


        IsCrossfading = false;
    }

    private IEnumerator FadeOutAndFadeInNextTrack()
    {
        // Fade out the current song.
        if (_CurrentAudioSource.clip != null)
        {
            yield return FadeTrack(_CurrentAudioSource,
                                   _CrossFadeDuration,
                                   _CurrentAudioSource.volume,
                                   0f);
        }
        

        SwapAudioSources();

        //  Fade in the next track.
        _CurrentAudioSource.clip = _MusicTracks[_CurrentMusicTrackIndex];
        _CurrentAudioSource.Play();

        yield return StartCoroutine(FadeTrack(_CurrentAudioSource, 
                                              _CrossFadeDuration, 
                                              0f, 
                                              _MusicVolume));
    }

    private IEnumerator CrossFade()
    {
        // Fade out the current song.
        if (_CurrentAudioSource.clip != null)
        {
            StartCoroutine(FadeTrack(_CurrentAudioSource,
                                     _CrossFadeDuration,
                                     _CurrentAudioSource.volume,
                                     0f));
        }


        SwapAudioSources();

        //  Fade in the next track.
        _CurrentAudioSource.clip = _MusicTracks[_CurrentMusicTrackIndex];
        _CurrentAudioSource.Play();

        yield return StartCoroutine(FadeTrack(_CurrentAudioSource,
                                              _CrossFadeDuration,
                                              0f,
                                              _MusicVolume));
    }

    private IEnumerator FadeTrack(AudioSource audioSource, float duration, float startVolume, float endVolume)
    {
        float elapsedTime = 0f;
        

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 
                                            endVolume, 
                                            elapsedTime / duration);

            yield return null;
        }


        yield break;
    }

    private float GetTimeLeft(AudioSource audioSource)
    {
        if (audioSource == null || audioSource.clip == null)
            return 0;

        return audioSource.clip.length - audioSource.time;
    }

    private void SwapAudioSources()
    {
        if (_CurrentAudioSource == _AudioSource1)
        {
            _PrevAudioSource = _AudioSource1;
            _CurrentAudioSource = _AudioSource2;
        }
        else
        {
            _PrevAudioSource = _AudioSource2;
            _CurrentAudioSource = _AudioSource1;
        }
    }

    private void GetNextTrack()
    {
        if (_MusicTracks.Count < 1)
        {
            _CurrentMusicTrackIndex = -1;

            return;
        }


        // If random order is enabled, choose a random track.
        if (_PlayInRandomOrder)
        {
            GetRandomTrack();
            return;
        }

        // Random order is disabled, so advance to the next track.
        // Check whether we are already at the end of the tracks list.
        if (_CurrentMusicTrackIndex < _MusicTracks.Count - 1)
            _CurrentMusicTrackIndex++;
        else
            _CurrentMusicTrackIndex = 0;
    }

    private void GetRandomTrack()
    {
        if (_MusicTracks.Count <= 1)
        {
            // If there is only one track, return 0 so it plays repeatedly. Otherwise we'll get stuck in the loop below forever.
            _CurrentMusicTrackIndex = 0;
            return;
        }


        int newIndex = 0;        

        while (true)
        {
            newIndex = Random.Range(0, _MusicTracks.Count);

            if (newIndex != _CurrentMusicTrackIndex)
            {
                _CurrentMusicTrackIndex = newIndex;
                break;
            }
        }
    }

    private IEnumerator LoadMusic(string musicPath)
    {
        _MusicTracks.Clear();


        if (!Directory.Exists(musicPath))
        {
            Debug.LogError($"Music directory not found!  Path=\"{musicPath}\"");
            yield break;
        }


        List<string> files = Directory.GetFiles(musicPath, "*.*")
                                      .Where(s => _MusicFileExtensions.Contains(Path.GetExtension(s).ToLower())).ToList();

        foreach (string file in files)
        {
            Debug.Log(file);
            string temp = Path.Combine("file://", file);

            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(temp, AudioType.UNKNOWN);
            yield return www.SendWebRequest();


            if (www.result != UnityWebRequest.Result.Success)
                Debug.LogError(www.error);
            else
                _MusicTracks.Add(DownloadHandlerAudioClip.GetContent(www));
        }


        CheckIfMusicWasLoaded();
        SelectFirstTrack();

        _MusicIsLoaded = true;
    }

    private void CheckIfMusicWasLoaded()
    {
        if (_MusicTracks.Count < 1)
        {
            Debug.LogError("MusicManager found no music tracks!");

            _CurrentMusicTrackIndex = -1;
            return;
        }
    }

    private void SelectFirstTrack()
    {
        if (!_PlayInRandomOrder)
            _CurrentMusicTrackIndex = 0;
        else
            GetRandomTrack();
    }


    public bool IsCrossfading { get; private set; }
}
