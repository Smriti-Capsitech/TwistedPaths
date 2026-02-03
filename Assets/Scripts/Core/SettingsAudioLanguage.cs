// // using UnityEngine;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class SettingsAudioLanguage : MonoBehaviour
// {
//     [Header("Audio")]
//     public AudioSource backgroundMusic;
//     public AudioSource buttonSfx;

//     [Header("Music UI")]
//     public Button musicButton;
//     public Sprite musicOnIcon;
//     public Sprite musicOffIcon;

//     [Header("Sound UI")]
//     public Button soundButton;
//     public Sprite soundOnIcon;
//     public Sprite soundOffIcon;

//     [Header("Language")]
//     public TMP_Dropdown languageDropdown;

//     [Header("Settings Panel")]
//     public GameObject settingsPanel;   // 🔥 ADD THIS

//     bool musicOn;
//     bool soundOn;

//     void Start()
//     {
//         musicOn = PlayerPrefs.GetInt("MUSIC_ON", 1) == 1;
//         soundOn = PlayerPrefs.GetInt("SOUND_ON", 1) == 1;

//         ApplyMusicState();
//         ApplySoundState();
//         SetupLanguageDropdown();
//     }

//     // =========================
//     // 🔥 OPEN SETTINGS PANEL
//     // =========================
//    public void OpenSettings()
// {
//     Debug.Log("OPEN SETTINGS CLICKED");

//     if (settingsPanel == null) return;

//     // 🔥 FORCE ENABLE PARENT FIRST
//     Transform parent = settingsPanel.transform.parent;
//     if (parent != null)
//         parent.gameObject.SetActive(true);

//     settingsPanel.SetActive(true);
//     settingsPanel.transform.SetAsLastSibling();
// }


//     // =========================
//     // 🎵 MUSIC
//     // =========================
//     public void ToggleMusic()
//     {
//         musicOn = !musicOn;
//         PlayerPrefs.SetInt("MUSIC_ON", musicOn ? 1 : 0);
//         PlayerPrefs.Save();
//         ApplyMusicState();
//     }

//     void ApplyMusicState()
//     {
//         if (backgroundMusic != null)
//             backgroundMusic.mute = !musicOn;

//         if (musicButton != null)
//             musicButton.image.sprite = musicOn ? musicOnIcon : musicOffIcon;
//     }

//     // =========================
//     // 🔊 SOUND
//     // =========================
//     public void ToggleSound()
//     {
//         soundOn = !soundOn;
//         PlayerPrefs.SetInt("SOUND_ON", soundOn ? 1 : 0);
//         PlayerPrefs.Save();
//         ApplySoundState();
//     }

//     void ApplySoundState()
//     {
//         if (buttonSfx != null)
//             buttonSfx.mute = !soundOn;

//         if (soundButton != null)
//             soundButton.image.sprite = soundOn ? soundOnIcon : soundOffIcon;
//     }

//     // =========================
//     // 🌍 LANGUAGE
//     // =========================
//     void SetupLanguageDropdown()
//     {
//         if (languageDropdown == null) return;

//         languageDropdown.ClearOptions();
//         languageDropdown.AddOptions(new System.Collections.Generic.List<string>
//         {
//             "English",
//             "Spanish",
//             "Portuguese",
//             "French",
//             "Arabic",
//             "Indonesian"
//         });

//         int savedLang = PlayerPrefs.GetInt("LANGUAGE", 0);
//         languageDropdown.value = savedLang;
//         languageDropdown.RefreshShownValue();
//         languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
//     }

//     void OnLanguageChanged(int index)
//     {
//         PlayerPrefs.SetInt("LANGUAGE", index);
//         PlayerPrefs.Save();
//         Debug.Log("Language Changed To: " + languageDropdown.options[index].text);
//     }

//     // =========================
//     // 🔊 CLICK SOUND
//     // =========================
//     public void PlayClickSound()
//     {
//         if (buttonSfx != null && soundOn)
//             buttonSfx.Play();
//     }
// }
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsAudioLanguage : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource backgroundMusic;
    public AudioSource buttonSfx;

    [Header("Music UI")]
    public Button musicButton;
    public Sprite musicOnIcon;
    public Sprite musicOffIcon;

    [Header("Sound UI")]
    public Button soundButton;
    public Sprite soundOnIcon;
    public Sprite soundOffIcon;

    [Header("Language")]
    public TMP_Dropdown languageDropdown;

    [Header("Settings Panel")]
    public GameObject settingsPanel;

    bool musicOn;
    bool soundOn;

    void Awake()
    {
        // 🔥 Make AudioManager persistent
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        musicOn = PlayerPrefs.GetInt("MUSIC_ON", 1) == 1;
        soundOn = PlayerPrefs.GetInt("SOUND_ON", 1) == 1;

        ApplyMusicState();
        ApplySoundState();
        SetupLanguageDropdown();
    }

    // =========================
    // 🔥 OPEN SETTINGS PANEL
    // =========================
    public void OpenSettings()
    {
        if (settingsPanel == null) return;

        settingsPanel.SetActive(true);
        settingsPanel.transform.SetAsLastSibling();
    }

    // =========================
    // 🎵 MUSIC (BACKGROUND)
    // =========================
    public void ToggleMusic()
    {
        musicOn = !musicOn;
        PlayerPrefs.SetInt("MUSIC_ON", musicOn ? 1 : 0);
        PlayerPrefs.Save();

        ApplyMusicState();
    }

    void ApplyMusicState()
    {
        if (backgroundMusic == null) return;

        backgroundMusic.mute = !musicOn;

        if (musicOn && !backgroundMusic.isPlaying)
            backgroundMusic.Play();

        UpdateButtonIcon(musicButton, musicOnIcon, musicOffIcon, musicOn);
    }

    // =========================
    // 🔊 SOUND (BUTTON SFX)
    // =========================
    public void ToggleSound()
    {
        soundOn = !soundOn;
        PlayerPrefs.SetInt("SOUND_ON", soundOn ? 1 : 0);
        PlayerPrefs.Save();

        ApplySoundState();
    }

    void ApplySoundState()
    {
        if (buttonSfx != null)
            buttonSfx.mute = !soundOn;

        UpdateButtonIcon(soundButton, soundOnIcon, soundOffIcon, soundOn);
    }

    // =========================
    // 🔊 BUTTON CLICK SOUND
    // =========================
    public void PlayClickSound()
    {
        if (buttonSfx != null && soundOn)
            buttonSfx.Play();
    }

    // =========================
    // 🌍 LANGUAGE
    // =========================
    void SetupLanguageDropdown()
    {
        if (languageDropdown == null) return;

        languageDropdown.ClearOptions();
        languageDropdown.AddOptions(new System.Collections.Generic.List<string>
        {
            "English",
            "Spanish",
            "Portuguese",
            "French",
            "Arabic",
            "Indonesian"
        });

        int savedLang = PlayerPrefs.GetInt("LANGUAGE", 0);
        languageDropdown.value = savedLang;
        languageDropdown.RefreshShownValue();
        languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
    }

    void OnLanguageChanged(int index)
    {
        PlayerPrefs.SetInt("LANGUAGE", index);
        PlayerPrefs.Save();
        Debug.Log("Language Changed To: " + languageDropdown.options[index].text);
    }

    // =========================
    // 🧠 ICON HELPER (IMPORTANT)
    // =========================
    void UpdateButtonIcon(Button btn, Sprite onIcon, Sprite offIcon, bool isOn)
    {
        if (btn == null) return;

        Image icon = btn.GetComponentInChildren<Image>();
        if (icon == null) return;

        icon.sprite = isOn ? onIcon : offIcon;
    }
}
