using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;
using System;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance;

    // =========================
    // TEST AD IDS (ADMOB)
    // =========================
    const string BANNER_ID = "ca-app-pub-3940256099942544/6300978111";
    const string INTERSTITIAL_ID = "ca-app-pub-3940256099942544/1033173712";
    const string REWARDED_ID = "ca-app-pub-3940256099942544/5224354917";

    BannerView banner;
    InterstitialAd interstitial;
    RewardedAd rewarded;

    int levelCompleteCount = 0;
    int gameOverCount = 0;
    int replayTapCount = 0;

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

        // 🔥 Listen for scene changes
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // =========================
    // 🔁 SCENE HANDLER
    // =========================
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Show banner ONLY in gameplay scenes
        if (scene.name == "SampleScene" || scene.name == "Chapter_2")
        {
            ShowBanner();
        }
        else
        {
            HideBanner();
        }
    }

    // =========================
    // 📢 BANNER AD (GAMEPLAY ONLY)
    // =========================
    public void ShowBanner()
    {
        // 🔥 ALWAYS recreate banner (fix)
        if (banner != null)
        {
            banner.Destroy();
            banner = null;
        }

        banner = new BannerView(
            BANNER_ID,
            AdSize.Banner,
            AdPosition.Bottom
        );

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
    // INTERSTITIAL AD (SDK v8+)
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
    }

    // =========================
    // REWARDED AD (SDK v8+)
    // =========================
    void LoadRewarded()
    {
        RewardedAd.Load(
            REWARDED_ID,
            new AdRequest(),
            (ad, error) =>
            {
                if (error != null || ad == null)
                {
                    rewarded = null;
                    return;
                }

                rewarded = ad;
            });
    }

    void ShowRewarded(Action onReward)
    {
        if (rewarded != null && rewarded.CanShowAd())
        {
            rewarded.Show(_ =>
            {
                onReward?.Invoke();
            });

            rewarded = null;
            LoadRewarded();
        }
        else
        {
            onReward?.Invoke();
        }
    }

    // =========================
    // 🎮 GAME EVENTS (CALL THESE)
    // =========================

    // 🔹 Level Complete → Interstitial every 3
    public void OnLevelComplete()
    {
        levelCompleteCount++;
        if (levelCompleteCount % 3 == 0)
            ShowInterstitial();
    }

    // 🔹 Game Over → Interstitial every 3
    public void OnGameOver()
    {
        gameOverCount++;
        if (gameOverCount % 3 == 0)
            ShowInterstitial();
    }

    // 🔹 Replay → Rewarded every 2 taps
    public void OnReplay(Action replayAction)
    {
        replayTapCount++;

        if (replayTapCount % 2 == 0)
            ShowRewarded(replayAction);
        else
            replayAction?.Invoke();
    }
}
