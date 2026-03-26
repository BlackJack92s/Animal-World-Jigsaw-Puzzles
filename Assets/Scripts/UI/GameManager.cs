using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Economía")]
    public int monedasActuales;
    public TextMeshProUGUI txtMonedas;

    [SerializeField] private int ultimaGanancia;
    void Awake()
    {
        // Configuración de Singleton
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        
    }
    void Start()
    {
        CargarDatos();
    }
    // Método para intentar comprar un animal
    public bool TryUnlockPuzzle(string nombreAnimal, int indicePuzzle, int precio)
    {
        string idGuardado = "unlocked_" + nombreAnimal + "_" + indicePuzzle;

        if (monedasActuales >= precio)
        {
            monedasActuales -= precio; 
            PlayerPrefs.SetInt(idGuardado, 1);
            GuardarDatos();

            return true;
        }
        return false;
    }

    public void GanarMonedas(int cantidad)
    {
        monedasActuales = PlayerPrefs.GetInt("TotalMonedas", 0);
        monedasActuales += cantidad;
        GuardarDatos();
    }

    public void GuardarDatos()
    {
        PlayerPrefs.SetInt("TotalMonedas", monedasActuales);
        PlayerPrefs.Save();

        CargarDatos();
    }

    private void CargarDatos()
    { 
        monedasActuales = PlayerPrefs.GetInt("TotalMonedas", 0);

        if (txtMonedas != null)
        {
            txtMonedas.text = "Coins: " + monedasActuales;
        }
    }
    public void verAnuncio()
    {
        UnityAdsManager.Instance.ShowRewardedAd();
    }
    public void VerAnuncio100()
    {
        UnityAdsManager.Instance.ShowRewardedAd(UnityAdsManager.AdRewardType.Reward100);
    } 
    public void VerAnuncioDuplicar()
    {
        //ultimaGanancia = cantidadAGanar;
        UnityAdsManager.Instance.ShowRewardedAd(UnityAdsManager.AdRewardType.DoubleReward);
    } 
    public void OtorgarRecompensa(UnityAdsManager.AdRewardType tipo)
    {
        if (tipo == UnityAdsManager.AdRewardType.Reward100)
        {
            GanarMonedas(100);
        }
        else if (tipo == UnityAdsManager.AdRewardType.DoubleReward)
        {
            ultimaGanancia = DatosPartida.Instance.monedasAct;
            GanarMonedas(ultimaGanancia);
        }
    }
}
