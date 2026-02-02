
// using UnityEngine;
// using GoogleMobileAds.Api;
// using UnityEngine.SceneManagement;
// using System;

// public class AdManager : MonoBehaviour
// {
//     public static AdManager Instance;

//     const string BANNER_ID = "ca-app-pub-3940256099942544/6300978111";
//     const string INTERSTITIAL_ID = "ca-app-pub-3940256099942544/1033173712";
//     const string REWARDED_ID = "ca-app-pub-3940256099942544/5224354917";

//     BannerView banner;
//     InterstitialAd interstitial;
//     RewardedAd rewarded;

//     int levelCompleteCount = 0;
//     int lastCountedLevel = -1;
//     int lastChapter = -1;

//     void Awake()
//     {
//         if (Instance != null)
//         {
//             Destroy(gameObject);
//             return;
//         }

//         Instance = this;
//         DontDestroyOnLoad(gameObject);

//         MobileAds.Initialize(_ => { });

//         LoadInterstitial();
//         LoadRewarded();

//         SceneManager.sceneLoaded += OnSceneLoaded;
//     }

//     void OnDestroy()
//     {
//         SceneManager.sceneLoaded -= OnSceneLoaded;
//     }

//     // =========================
//     // SCENE HANDLER
//     // =========================
//     void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//     {
//         HandleBanner(scene.name);
//         ResetIfChapterChanged();
//     }

//     void HandleBanner(string sceneName)
//     {
//         if (sceneName == "SampleScene" || sceneName == "Chapter_2")
//             ShowBanner();
//         else
//             HideBanner();
//     }

//     void ResetIfChapterChanged()
//     {
//         int chapter = PlayerPrefs.GetInt("ACTIVE_CHAPTER", 1);

//         if (chapter != lastChapter)
//         {
//             levelCompleteCount = 0;
//             lastCountedLevel = -1;
//             lastChapter = chapter;
//         }
//     }

//     // =========================
//     // BANNER
//     // =========================
//     public void ShowBanner()
//     {
//         if (banner != null) return;

//         banner = new BannerView(BANNER_ID, AdSize.Banner, AdPosition.Bottom);
//         banner.LoadAd(new AdRequest());
//         banner.Show();
//     }

//     public void HideBanner()
//     {
//         if (banner != null)
//         {
//             banner.Destroy();
//             banner = null;
//         }
//     }

//     // =========================
//     // INTERSTITIAL
//     // =========================
//     void LoadInterstitial()
//     {
//         InterstitialAd.Load(
//             INTERSTITIAL_ID,
//             new AdRequest(),
//             (ad, error) =>
//             {
//                 if (error == null && ad != null)
//                     interstitial = ad;
//             });
//     }

//     void ShowInterstitial()
//     {
//         if (interstitial != null && interstitial.CanShowAd())
//         {
//             interstitial.Show();
//             interstitial = null;
//             LoadInterstitial();
//         }
//     }

//     // =========================
//     // REWARDED
//     // =========================
//     void LoadRewarded()
//     {
//         RewardedAd.Load(
//             REWARDED_ID,
//             new AdRequest(),
//             (ad, error) =>
//             {
//                 if (error == null && ad != null)
//                     rewarded = ad;
//             });
//     }

//     void ShowRewarded(Action rewardAction)
//     {
//         if (rewarded != null && rewarded.CanShowAd())
//         {
//             rewarded.Show(_ => rewardAction?.Invoke());
//             rewarded = null;
//             LoadRewarded();
//         }
//         else
//         {
//             rewardAction?.Invoke();
//         }
//     }

//     // =========================
//     // GAME EVENTS
//     // =========================

//     // 🔹 Interstitial every 4 level completes
//     public void OnLevelComplete()
//     {
//         int currentLevel = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);

//         if (currentLevel == lastCountedLevel)
//             return;

//         lastCountedLevel = currentLevel;
//         levelCompleteCount++;

//         Debug.Log($"✅ Level Complete Count = {levelCompleteCount}");

//         if (levelCompleteCount % 4 == 0)
//             ShowInterstitial();
//     }

//     // 🔹 Game over = interstitial
//     public void OnGameOver()
//     {
//         ShowInterstitial();
//     }

//     // 🔹 REQUIRED by HintButton.cs (THIS FIXES YOUR ERROR)
//     public void OnReplay(Action replayAction)
//     {
//         ShowRewarded(replayAction);
//     }
// }
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;
using System;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance;

    const string BANNER_ID = "ca-app-pub-3940256099942544/6300978111";
    const string INTERSTITIAL_ID = "ca-app-pub-3940256099942544/1033173712";
    const string REWARDED_ID = "ca-app-pub-3940256099942544/5224354917";

    BannerView banner;
    InterstitialAd interstitial;
    RewardedAd rewarded;

    int levelCompleteCount = 0;
    int lastCountedLevel = -1;
    int lastChapter = -1;

    bool pendingInterstitial = false; // 🔥 KEY FIX

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        MobileAds.Initialize(_ => { });

        LoadInterstitial();
        LoadRewarded();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // =========================
    // SCENE HANDLER
    // =========================
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HandleBanner(scene.name);
        ResetIfChapterChanged();
    }

    void HandleBanner(string sceneName)
    {
        if (sceneName == "SampleScene" || sceneName == "Chapter_2")
            ShowBanner();
        else
            HideBanner();
    }

    void ResetIfChapterChanged()
    {
        int chapter = PlayerPrefs.GetInt("ACTIVE_CHAPTER", 1);

        if (chapter != lastChapter)
        {
            levelCompleteCount = 0;
            lastCountedLevel = -1;
            pendingInterstitial = false;
            lastChapter = chapter;
        }
    }

    // =========================
    // BANNER
    // =========================
    public void ShowBanner()
    {
        if (banner != null) return;

        banner = new BannerView(BANNER_ID, AdSize.Banner, AdPosition.Bottom);
        banner.LoadAd(new AdRequest());
        banner.Show();
    }

    public void HideBanner()
    {
        if (banner != null)
        {
            banner.Destroy();
            banner = null;
        }
    }

    // =========================
    // INTERSTITIAL
    // =========================
    void LoadInterstitial()
    {
        InterstitialAd.Load(
            INTERSTITIAL_ID,
            new AdRequest(),
            (ad, error) =>
            {
                if (error != null || ad == null)
                {
                    interstitial = null;
                    return;
                }

                interstitial = ad;

                // 🔥 SHOW IF IT WAS WAITING
                if (pendingInterstitial)
                {
                    pendingInterstitial = false;
                    ShowInterstitial();
                }
            });
    }

    void ShowInterstitial()
    {
        if (interstitial != null && interstitial.CanShowAd())
        {
            interstitial.Show();
            interstitial = null;
            LoadInterstitial();
        }
        else
        {
            // 🔥 NOT READY → WAIT
            pendingInterstitial = true;
        }
    }

    // =========================
    // REWARDED
    // =========================
    void LoadRewarded()
    {
        RewardedAd.Load(
            REWARDED_ID,
            new AdRequest(),
            (ad, error) =>
            {
                if (error == null && ad != null)
                    rewarded = ad;
            });
    }

    void ShowRewarded(Action rewardAction)
    {
        if (rewarded != null && rewarded.CanShowAd())
        {
            rewarded.Show(_ => rewardAction?.Invoke());
            rewarded = null;
            LoadRewarded();
        }
        else
        {
            rewardAction?.Invoke();
        }
    }

    // =========================
    // GAME EVENTS
    // =========================
    public void OnLevelComplete()
    {
        int currentLevel = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);

        if (currentLevel == lastCountedLevel)
            return;

        lastCountedLevel = currentLevel;
        levelCompleteCount++;

        Debug.Log($"✅ Level Complete Count = {levelCompleteCount}");

        if (levelCompleteCount % 4 == 0)
            ShowInterstitial();
    }

    public void OnGameOver()
    {
        ShowInterstitial();
    }

    // 🔹 REQUIRED BY HintButton.cs
    public void OnReplay(Action replayAction)
    {
        ShowRewarded(replayAction);
    }
}
