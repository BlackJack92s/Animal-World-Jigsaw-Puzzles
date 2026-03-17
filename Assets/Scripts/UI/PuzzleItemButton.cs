using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PuzzleItemButton : MonoBehaviour
{
    public Image imagenPuzzle;
    public TextMeshProUGUI txtMonedas;
    public GameObject lockOverlay;
    public Button botonComprar;

    private string nombreAnimal;
    private int indicePuzzle;
    private int precio;
    private bool desbloqueado;

    // Guardamos el handle para liberar la memoria correctamente
    private AsyncOperationHandle<Sprite> handleCarga;

    public void Configurar(PuzzleInfo datos, string animal, int indice)
    {
        // 1. IMPORTANTE: Si ya había una carga previa, liberarla antes de iniciar la nueva
        if (handleCarga.IsValid())
        {
            Addressables.Release(handleCarga);
        }

        // 2. Iniciar la carga
        //handleCarga = datos.imagenPuzzle.LoadAssetAsync<Sprite>();
        handleCarga = Addressables.LoadAssetAsync<Sprite>(datos.imagenPuzzle);
        // Guardamos una referencia local a la operación actual para comparar en el callback
        AsyncOperationHandle<Sprite> operacionActual = handleCarga;

        operacionActual.Completed += (op) =>
        {
            // Verificamos que el objeto aún exista y que la operación sea la que lanzamos
            if (this == null) return;

            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                imagenPuzzle.sprite = op.Result;
            }
            else
            {
                Debug.LogError($"Error cargando puzzle: {animal}_{indice}");
            }
        };

        nombreAnimal = animal;
        indicePuzzle = indice;
        precio = datos.precioMonedas;

        txtMonedas.text = precio.ToString();

        string idGuardado = "unlocked_" + nombreAnimal + "_" + indicePuzzle;
        desbloqueado = PlayerPrefs.GetInt(idGuardado, 0) == 1;

        ActualizarUI();
    }

    void ActualizarUI()
    {
        lockOverlay.SetActive(!desbloqueado);
        botonComprar.gameObject.SetActive(!desbloqueado);
    }

    public void SeleccionarPuzzle()
    {
        if (!desbloqueado) return;

        if (AudioManager.Instance != null) AudioManager.Instance.PlayClick();
        if (MenuManager.instance != null) MenuManager.instance.SeleccionarPuzzle(imagenPuzzle);
    }

    public void ComprarPuzzle()
    {
        if (GameManager.Instance == null) return;

        bool comprado = GameManager.Instance.TryUnlockPuzzle(nombreAnimal, indicePuzzle, precio);

        if (comprado)
        {
            if (AudioManager.Instance != null) AudioManager.Instance.PlayClick();
            desbloqueado = true;
            ActualizarUI();
        }
    }

    // 3. CRÍTICO: Liberar la memoria cuando el botón desaparece (ej. al volver al menú principal)
    private void OnDestroy()
    {
        if (handleCarga.IsValid())
        {
            Addressables.Release(handleCarga);
        }
    }
}