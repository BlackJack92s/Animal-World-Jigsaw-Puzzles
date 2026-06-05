using System;
using UnityEngine;
using Unity.Services.LevelPlay;

public class LevelPlayAdsManager : MonoBehaviour
{
    public static LevelPlayAdsManager Instance;

    [Header("LevelPlay App Keys (Reemplazar con los de tu panel)")]
    [SerializeField] private string androidAppKey = "269b9734d";
    [SerializeField] private string iOSAppKey = "TU_APP_KEY_IOS";
    private string AppKey
    {
        get
        {
#if UNITY_ANDROID
            return androidAppKey;
#elif UNITY_IOS
                return iOSAppKey; 
#else
                return  adnroidBannerUnitId;
#endif
        }
    }
    [Header("Banner Ad Unit IDs")]
    [SerializeField] private string adnroidBannerUnitId;
    [SerializeField] private string iosBannerUnitId;

    [Header("Interstitial Ad Unit IDs")]
    [SerializeField] private string adnroidInterstitialUnitId;
    [SerializeField] private string iosInterstitialUnitId;

    [Header("Rewarded Ad Unit IDs")]
    [SerializeField] private string adnroidRewardedUnitId;
    [SerializeField] private string iosRewardedUnitId;

    private string bannerAdUnitId
    {
        get
        {
#if UNITY_ANDROID
            return adnroidBannerUnitId;
# elif UNITY_IOS
    return iosBannerUnitId; 
#else
    return  adnroidBannerUnitId;
#endif
        }
    }
    private string InterstitialUnitId
    {
        get
        {
#if UNITY_ANDROID
            return adnroidInterstitialUnitId;
#elif UNITY_IOS
    return iosInterstitialUnitId; 
#else
    return  adnroidInterstitialUnitId;
#endif
        }
    }
    private string RewardedUnitId
    {
        get
        {
#if UNITY_ANDROID
            return adnroidRewardedUnitId;
#elif UNITY_IOS
    return iosRewardedUnitId; 
#else
    return  adnroidRewardedUnitId;
#endif
        }
    }
    [Header("Configuración")]

    // Objetos de anuncio de LevelPlay
    private LevelPlayBannerAd bannerAd;
    private LevelPlayInterstitialAd interstitialAd;
    private LevelPlayRewardedAd rewardedAd;

    // Utility wrappers for debuglog
    public delegate void DebugEvent(string msg);
    public static event DebugEvent OnDebugLog;

    public enum AdRewardType { Reward100, DoubleReward }
    private AdRewardType currentRewardType;

    private bool rewardedAdLoaded = false;
    private bool interstitialLoaded = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void Initialize()
    {
        // Suscribimos a los eventos globales de inicialización de LevelPlay
        LevelPlay.OnInitSuccess += OnInitializationComplete;
        LevelPlay.OnInitFailed += OnInitializationFailed;

        LevelPlay.Init(AppKey);
    }

    private void OnInitializationComplete(LevelPlayConfiguration configuration)
    {
        SetupBannerAd();
        SetupInterstitialAd();
        SetupRewardedAd();
    }

    private void OnInitializationFailed(LevelPlayInitError error)
    {
        DebugLog($"LevelPlay Init Failed: {error}");
    }

    #region Banner Logic
    private void SetupBannerAd()
    {
        var adConfig = new LevelPlayBannerAd.Config.Builder()
            .SetSize(LevelPlayAdSize.BANNER)
            .SetPosition(LevelPlayBannerPosition.TopCenter)
            .SetRespectSafeArea(true)
            .Build();
        bannerAd = new LevelPlayBannerAd(bannerAdUnitId, adConfig);

        bannerAd.OnAdLoaded += BannerOnAdLoadedEvent;
        bannerAd.OnAdLoadFailed += BannerOnAdLoadFailedEvent;
        bannerAd.OnAdDisplayed += BannerOnAdDisplayedEvent;
        bannerAd.OnAdDisplayFailed += BannerOnAdDisplayFailedEvent;
        bannerAd.OnAdClicked += BannerOnAdClickedEvent;
        bannerAd.OnAdCollapsed += BannerOnAdCollapsedEvent;
        bannerAd.OnAdLeftApplication += BannerOnAdLeftApplicationEvent;
        bannerAd.OnAdExpanded += BannerOnAdExpandedEvent;

        LoadBannerAd();
    }

    public void LoadBannerAd()
    {
        bannerAd.LoadAd();
    }
    public void ShowBannerAd()
    {
        if (bannerAd != null) bannerAd.ShowAd();
    }
    public void HideBannerAd()
    {
        if (bannerAd != null) bannerAd.HideAd();
    }
    public void DestroyBannerAd()
    {
        bannerAd.DestroyAd();
    }
    public void ToggleBanner()
    {
        ShowBannerAd();
    }
    public void BannerOnAdLoadedEvent(LevelPlayAdInfo adInfo)
    {
        ShowBannerAd();
    }
    public void BannerOnAdLoadFailedEvent(LevelPlayAdError ironSourceError)
    {
        Invoke(nameof(LoadBannerAd), 5f);
    }
    public void BannerOnAdClickedEvent(LevelPlayAdInfo adInfo) { }
    public void BannerOnAdDisplayedEvent(LevelPlayAdInfo adInfo) { }
    public void BannerOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError error) { }
    public void BannerOnAdCollapsedEvent(LevelPlayAdInfo adInfo) { }
    public void BannerOnAdLeftApplicationEvent(LevelPlayAdInfo adInfo) { }
    public void BannerOnAdExpandedEvent(LevelPlayAdInfo adInfo) { }
    #endregion

    #region Interstitial Logic
    private void SetupInterstitialAd()
    {
        interstitialAd = new LevelPlayInterstitialAd(InterstitialUnitId);

        interstitialAd.OnAdLoaded += InterstitialOnAdLoadedEvent;
        interstitialAd.OnAdLoadFailed += InterstitialOnAdLoadFailedEvent;
        interstitialAd.OnAdDisplayed += InterstitialOnAdDisplayedEvent;
        interstitialAd.OnAdDisplayFailed += InterstitialOnAdDisplayFailedEvent;
        interstitialAd.OnAdClicked += InterstitialOnAdClickedEvent;
        interstitialAd.OnAdClosed += InterstitialOnAdClosedEvent;
        interstitialAd.OnAdInfoChanged += InterstitialOnAdInfoChangedEvent;

        LoadInterstitialAd();
    }

    public void LoadInterstitialAd()
    {
        interstitialAd.LoadAd();
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd.IsAdReady())
        {
            interstitialAd.ShowAd();
        }
    }
    void DestroyInterstitialAd()
    {
        interstitialAd.DestroyAd();
    }
    // Implement InterstitialAd events
    void InterstitialOnAdLoadedEvent(LevelPlayAdInfo adInfo) { }
    void InterstitialOnAdLoadFailedEvent(LevelPlayAdError error) { }
    void InterstitialOnAdClickedEvent(LevelPlayAdInfo adInfo) { }
    void InterstitialOnAdDisplayedEvent(LevelPlayAdInfo adInfo) { }
    void InterstitialOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError error) { }
    void InterstitialOnAdClosedEvent(LevelPlayAdInfo adInfo) { }
    void InterstitialOnAdInfoChangedEvent(LevelPlayAdInfo adInfo) { }
    #endregion

    #region Rewarded Logic
    private void SetupRewardedAd()
    {
        rewardedAd = new LevelPlayRewardedAd(RewardedUnitId);

        rewardedAd.OnAdLoaded += OnAdLoaded;
        rewardedAd.OnAdLoadFailed += OnAdLoadFailed;
        rewardedAd.OnAdDisplayed += OnAdDisplayed;
        rewardedAd.OnAdRewarded += OnAdRewarded;
        rewardedAd.OnAdClicked += OnAdClicked;
        rewardedAd.OnAdClosed += OnAdClosed;
        rewardedAd.OnAdInfoChanged += OnAdInfoChanged;

        LoadRewardedAd();
    }

    public void LoadRewardedAd()
    {
        rewardedAd.LoadAd();
    }
    public void ShowRewardedAd()
    {
        if (rewardedAd.IsAdReady())
        {
            rewardedAd.ShowAd();
        }
    }
    public void ShowRewardedAd(AdRewardType rewardType)
    {
        currentRewardType = rewardType;
        if (rewardedAd.IsAdReady())
        {
            rewardedAd.ShowAd();
        }
    }
    void OnAdLoaded(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"Rewarded ad loaded with ad info {adInfo}");
    }

    void OnAdLoadFailed(LevelPlayAdError adError)
    {
        Debug.Log($"Rewarded ad failed to load with ad error {adError}");
    }

    void OnAdDisplayed(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"Rewarded ad displayed with ad info {adInfo}");
    }

    void OnAdRewarded(LevelPlayAdInfo adInfo, LevelPlayReward adReward)
    {
        if (currentRewardType != null)
        {
            GameManager.Instance.OtorgarRecompensa(currentRewardType);
        } 
    }

    void OnAdClicked(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"Rewarded ad clicked with ad info {adInfo}");
    }

    void OnAdClosed(LevelPlayAdInfo adInfo)
    {
        LoadRewardedAd();
    }

    void OnAdInfoChanged(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"Rewarded ad info changed with ad info {adInfo}");
    }
    #endregion

    // Wrapper around debug.log to allow broadcasting log strings to the UI
    void DebugLog(string msg)
    {
        OnDebugLog?.Invoke(msg);
        Debug.Log(msg);
    }

    // Es buena práctica destruir los objetos de anuncio al salir o destruir el Manager
    private void OnDestroy()
    {
        if (bannerAd != null) bannerAd.DestroyAd();
        LevelPlay.OnInitSuccess -= OnInitializationComplete;
        LevelPlay.OnInitFailed -= OnInitializationFailed;
    }
}