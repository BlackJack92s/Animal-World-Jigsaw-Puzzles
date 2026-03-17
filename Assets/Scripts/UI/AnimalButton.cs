using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AnimalButton : MonoBehaviour
{
    public Image imgIcono;
    public TextMeshProUGUI txtNombre;
    private AnimalData datosAnimal;

    private AsyncOperationHandle<Sprite> handleIcono;

    public void Configurar(AnimalData datos)
    {
        datosAnimal = datos;
        txtNombre.text = datos.nombreAnimal;

        // 1. Liberar el handle previo del botón si existe
        if (handleIcono.IsValid())
        {
            Addressables.Release(handleIcono);
        }

        // 2. CAMBIO CLAVE: Usar Addressables.LoadAssetAsync pasando la referencia
        // Esto evita el error de "Already been loaded"
        handleIcono = Addressables.LoadAssetAsync<Sprite>(datos.iconoPrincipal);

        handleIcono.Completed += (op) => {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                if (imgIcono != null) imgIcono.sprite = op.Result;
            }
        };
    }

    public void OnClick()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClick();
        if (MenuManager.instance != null) MenuManager.instance.SeccionAnimales(datosAnimal);
    }

    // MUY IMPORTANTE para evitar el error de "Invalid Operation Handle" al cerrar el menú
    private void OnDestroy()
    {
        if (handleIcono.IsValid())
        {
            Addressables.Release(handleIcono);
        }
    }
}