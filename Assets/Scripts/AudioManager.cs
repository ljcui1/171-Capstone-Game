using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource ambienceSource;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource walkingSfxSource;
    [SerializeField] private AudioSource npcWalkingSource;

    [Header("Ambience")]
    [SerializeField] private AudioClip dayAmbience;
    [SerializeField] private AudioClip nightAmbience;

    [Header("Background Music")]
    [SerializeField] private AudioClip dayMusic;
    [SerializeField] private AudioClip nightMusic;

    [Header("Minigame Music")]
    [SerializeField] private AudioClip minigame1Music;
    [SerializeField] private AudioClip minigame2Music;
    [SerializeField] private AudioClip minigame3Music;

    [Header("Main Sound Effects")]
    [SerializeField] private AudioClip walkingSfx;
    [SerializeField] private AudioClip npcWalkingSfx;

    [Header("Multi Sound Effects")]
    [SerializeField] private AudioClip[] talkingSfxList;
    [SerializeField] private AudioClip[] pawThumpSfxList;
    [SerializeField] private AudioClip[] mouseSqueakSfxList;
    [SerializeField] private AudioClip[] musicalMeowSfxList;

    [Header("Game SFX")]
    [SerializeField] private AudioClip beanCatchSfx;
    [SerializeField] private AudioClip customerSpawnSfx;

    public AudioClip BeanCatchSfx => beanCatchSfx;


    private bool isDaytime = true;
    private bool isMinigameActive = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    /// <summary>
    /// Plays the current background music based on the time of day.
    /// </summary>
    public void PlayBackgroundMusic()
    {
        if (isMinigameActive)
        {
            return;
        }

        bgmSource.clip = isDaytime ? dayMusic : nightMusic;
        bgmSource.loop = true;
        bgmSource.Play();

        // Play Ambience
        ambienceSource.clip = isDaytime ? dayAmbience : nightAmbience;
        ambienceSource.loop = true;
        ambienceSource.Play();
    }

    /// <summary>
    /// Fades music and switches between Day/Night themes.
    /// </summary>
    public void SwitchDayNight(bool isDay)
    {
        if (isDay == isDaytime)
        {
            return;
        }
        npcWalkingSource.Stop();
        isDaytime = isDay;
        StartCoroutine(FadeMusic(isDaytime ? dayMusic : nightMusic));

        ambienceSource.clip = isDaytime ? dayAmbience : nightAmbience;
        ambienceSource.Play();
    }

    private IEnumerator FadeMusic(AudioClip newClip)
    {
        float fadeTime = 1.5f;
        float startVolume = bgmSource.volume;

        // Fade out
        while (bgmSource.volume > 0)
        {
            bgmSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        bgmSource.Stop();
        bgmSource.clip = newClip;
        bgmSource.Play();

        // Fade in
        while (bgmSource.volume < startVolume)
        {
            bgmSource.volume += startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
    }

    /// <summary>
    /// Starts the selected minigame's music, pausing all other sounds.
    /// </summary>
    public void StartMinigame(int minigameIndex)
    {
        isMinigameActive = true;
        bgmSource.Pause();
        ambienceSource.Pause();
        sfxSource.Stop();
        walkingSfxSource.Stop();
        npcWalkingSource.Stop();

        // Select the correct minigame music
        AudioClip selectedMinigameMusic = null;
        switch (minigameIndex)
        {
            case 1:
                selectedMinigameMusic = minigame1Music;
                break;
            case 2:
                selectedMinigameMusic = minigame2Music;
                break;
            case 3:
                selectedMinigameMusic = minigame3Music;
                break;
            default:
                Debug.LogWarning("Invalid minigame index! No music will play.");
                return;
        }

        bgmSource.clip = selectedMinigameMusic;
        bgmSource.Play();
    }

    /// <summary>
    /// Ends the minigame and resumes background music.
    /// </summary>
    public void EndMinigame()
    {
        isMinigameActive = false;
        bgmSource.Stop();
        PlayBackgroundMusic();
    }

    /// <summary>
    /// Plays a single sound effect.
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    /// <summary>
    /// Plays the walking sound if the player or NPC is moving.
    /// </summary>
    public void PlayWalkingSound(bool isPlayer)
    {
        if (isPlayer)
        {
            if (!walkingSfxSource.isPlaying)
            {
                walkingSfxSource.clip = walkingSfx;
                walkingSfxSource.loop = true;
                walkingSfxSource.Play();
            }
        }
        else
        {
            if (!npcWalkingSource.isPlaying)
            {
                npcWalkingSource.clip = npcWalkingSfx;
                npcWalkingSource.loop = true;
                npcWalkingSource.Play();
            }
        }
    }

    /// <summary>
    /// Stops the walking sound if the player or NPC stops moving.
    /// </summary>
    public void StopWalkingSound(bool isPlayer)
    {
        if (isPlayer)
        {
            walkingSfxSource.Stop();
        }
        else
        {
            npcWalkingSource.Stop();
        }
    }

    /// <summary>
    /// Plays a random talking sound effect.
    /// </summary>
    public void PlayTalkingSound()
    {
        if (talkingSfxList.Length > 0)
        {
            int randomIndex = Random.Range(0, talkingSfxList.Length);
            sfxSource.PlayOneShot(talkingSfxList[randomIndex]);
        }
        else
        {
            Debug.LogWarning("No talking sound effects assigned in AudioManager!");
        }
    }

    public void PlayThumpSound()
    {
        if (pawThumpSfxList.Length > 0)
        {
            int randomIndex = Random.Range(0, pawThumpSfxList.Length);
            sfxSource.PlayOneShot(pawThumpSfxList[randomIndex]);
        }
        else
        {
            Debug.LogWarning("No talking sound effects assigned in AudioManager!");
        }
    }

    public void PlaySqueakSound()
    {
        if (mouseSqueakSfxList.Length > 0)
        {
            int randomIndex = Random.Range(0, mouseSqueakSfxList.Length);
            sfxSource.PlayOneShot(mouseSqueakSfxList[randomIndex]);
        }
        else
        {
            Debug.LogWarning("No talking sound effects assigned in AudioManager!");
        }
    }

    public void PlayEnterChime()
    {
        if (customerSpawnSfx != null)
        {
            sfxSource.PlayOneShot(customerSpawnSfx);
        }
        else
        {
            Debug.LogWarning("Customer spawn sound effect not assigned in AudioManager!");
        }
    }

    public void PlayMusicalMeow()
    {
        if (musicalMeowSfxList.Length > 0)
        {
            int randomIndex = Random.Range(0, musicalMeowSfxList.Length);
            sfxSource.PlayOneShot(musicalMeowSfxList[randomIndex]);
        }
        else
        {
            Debug.LogWarning("No musical meow sound effects assigned in AudioManager!");
        }
    }

}