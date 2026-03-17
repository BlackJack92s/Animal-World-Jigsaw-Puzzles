using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Economía")]
    public int monedasActuales;
    public TextMeshProUGUI txtMonedas;

    void Awake()
    {
        // Configuración de Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

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
}
