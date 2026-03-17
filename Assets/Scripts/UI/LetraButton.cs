using UnityEngine;
using TMPro;

public class LetraButton : MonoBehaviour
{
    public TextMeshProUGUI txtLetra;
    private SeccionData datosSeccion;
    private MenuManager menuManager;

    public void Configurar(SeccionData seccion, MenuManager manager)
    {
        datosSeccion = seccion;
        menuManager = manager;
        txtLetra.text = seccion.letra.ToString(); 
    }

    public void OnClick()
    {
        AudioManager.Instance.PlayClick(); 
        menuManager.CambiarSeccion(datosSeccion);
    }
}