using System;
using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine;

public class UnityAdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static UnityAdsManager Instance;
    [SerializeField] private string androidGameId = "6073535";
    [SerializeField] private string iOSGameId = "6073534";

    private string gameId;

    private string BANNER_PLACEMENT = "Banner_Android";
    private string VIDEO_PLACEMENT = "Interstitial_Android";
    private string REWARDED_VIDEO_PLACEMENT = "Rewarded_Android";

    [SerializeField] private BannerPosition bannerPosition = BannerPosition.TOP_CENTER;
    private bool testMode = false;

    //utility wrappers for debuglog
    public delegate void DebugEvent(string msg);
    public static event DebugEvent OnDebugLog;

    public enum AdRewardType { Reward100, DoubleReward }
    private AdRewardType currentRewardType;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupPlatformIDs();
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void SetupPlatformIDs()
    {
        // Detecta la plataforma actual y asigna los IDs correspondientes
#if UNITY_IOS
        gameId = iOSGameId;
        BANNER_PLACEMENT = "Banner_iOS";
        VIDEO_PLACEMENT = "Interstitial_iOS";
        REWARDED_VIDEO_PLACEMENT = "Rewarded_iOS";
#elif UNITY_ANDROID
        gameId = androidGameId;
        BANNER_PLACEMENT = "Banner_Android";
        VIDEO_PLACEMENT = "Interstitial_Android";
        REWARDED_VIDEO_PLACEMENT = "Rewarded_Android";
#else
        gameId = androidGameId; // Por defecto para el Editor
#endif
    }
    public void Initialize()
    {
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId, testMode, this);
        }

    }
    public void LoadBanner()
    {
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };
        Advertisement.Banner.SetPosition(bannerPosition);
        Advertisement.Banner.Load(BANNER_PLACEMENT, options);
    }
    public void ToggleBanner()
    {
        Advertisement.Banner.SetPosition(bannerPosition);
        Advertisement.Banner.Show(BANNER_PLACEMENT);
    }

    public void LoadRewardedAd()
    {
        Advertisement.Load(REWARDED_VIDEO_PLACEMENT, this);
    }

    public void ShowRewardedAd()
    {
        Advertisement.Show(REWARDED_VIDEO_PLACEMENT, this);
    }
    public void ShowRewardedAd(AdRewardType rewardType)
    {
        currentRewardType = rewardType;
        Advertisement.Show(REWARDED_VIDEO_PLACEMENT, this);
    }
    public void LoadNonRewardedAd()
    {
        Advertisement.Load(VIDEO_PLACEMENT, this);
    }

    public void ShowNonRewardedAd()
    {
        Advertisement.Show(VIDEO_PLACEMENT, this);
    }

    #region Interface Implementations
    public void OnInitializationComplete()
    {
        DebugLog("Init Success");
        LoadBanner();
        LoadRewardedAd();
        LoadNonRewardedAd();
    }
    private void OnBannerLoaded() => ToggleBanner(); // Muestra el banner en cuanto cargue
    private void OnBannerError(string message) => DebugLog($"Banner Error: {message}");
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        DebugLog($"Init Failed: [{error}]: {message}");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        DebugLog($"Load Success: {placementId}");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        DebugLog($"Load Failed: [{error}:{placementId}] {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        DebugLog($"OnUnityAdsShowFailure: [{error}]: {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        DebugLog($"OnUnityAdsShowStart: {placementId}");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        DebugLog($"OnUnityAdsShowClick: {placementId}");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == REWARDED_VIDEO_PLACEMENT && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            // En lugar de manejar PlayerPrefs aquí, llamamos al GameManager
            GameManager.Instance.OtorgarRecompensa(currentRewardType);

            // Recargamos el anuncio para la próxima vez
            LoadRewardedAd();
        }
        //if (placementId == REWARDED_VIDEO_PLACEMENT && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        //{ 
        //    // Aquí puedes llamar a tu sistema de recompensas, por ejemplo:
        //    // player.GanarMonedas(100);
        //    int globalScore = PlayerPrefs.GetInt("TotalMonedas", 0);
        //    PlayerPrefs.SetInt("TotalMonedas", (globalScore + 100));
        //    PlayerPrefs.Save();
        //}
        //else if (showCompletionState == UnityAdsShowCompletionState.SKIPPED)
        //{
        //    DebugLog("El anuncio fue saltado, no se otorga recompensa.");
        //}
    }
    #endregion

    //wrapper around debug.log to allow broadcasting log strings to the UI
    void DebugLog(string msg)
    {
        OnDebugLog?.Invoke(msg);
        Debug.Log(msg);
    }
}
