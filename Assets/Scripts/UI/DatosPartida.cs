using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class DatosPartida : MonoBehaviour
{
    public static DatosPartida Instance;
    //public GameObject panelDificultad;
    public Sprite puzzleSeleccionado;
    public bool dificultadEstado;
    public AsyncOperationHandle<Sprite> handleActivo;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void SeleccionarPuzzle(Sprite sprite, AsyncOperationHandle<Sprite> handle)
    {
        // Liberar handle anterior si existía
        if (handleActivo.IsValid())
            Addressables.Release(handleActivo);

        puzzleSeleccionado = sprite;
        handleActivo = handle;
        MenuManager.instance.panelDificultad.SetActive(!MenuManager.instance.panelDificultad.activeSelf);
    }
    //public void DificultadFacil()
    //{
    //    ElegirDificultad(true);
    //}
    //public void DificultadDificil()
    //{
    //    ElegirDificultad(false);
    //}
    public void ElegirDificultad(bool estado)
    {
        dificultadEstado = estado;
        SceneManager.LoadScene("NivelPuzzle");
    }
}