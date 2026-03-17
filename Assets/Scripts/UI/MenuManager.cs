using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    [Header("Configuración del Grid")]
    public Transform gridContainer;
    public GameObject animalButtonPrefab;
    public GameObject animalItemPrefab;

    [Header("Barra de Letras")]
    public Transform letrasContainer; // Un objeto con Horizontal Layout Group
    public GameObject letraButtonPrefab;
    public List<SeccionData> todasLasSecciones;

    [Header("Paneles")]
    public GameObject panelPrincipal;
    public GameObject panelAnimales;
    public Transform panelAnimalesT;
    public TextMeshProUGUI txtTituloSeccion;

    public Sprite puzzleSeleccionado;
    public GameObject panelDificultad;
    public bool dificultadEstado;
    void Awake()
    {
        string idGuardado = "unlocked_Agapornis_0";
        if (!PlayerPrefs.HasKey(idGuardado))
        { 
            PlayerPrefs.SetInt(idGuardado, 1);
            PlayerPrefs.Save(); 
        }

        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    void Start()
    {
        CrearBarraDeLetras();

        if (todasLasSecciones.Count > 0)
        {
            CambiarSeccion(todasLasSecciones[0]);
        }
    }

    void CrearBarraDeLetras()
    {
        foreach (SeccionData s in todasLasSecciones)
        {
            GameObject btnLetra = Instantiate(letraButtonPrefab, letrasContainer);
            btnLetra.GetComponent<LetraButton>().Configurar(s, this);
        }
    }

    public void CambiarSeccion(SeccionData nuevaSeccion)
    { 
        foreach (Transform hijo in gridContainer)
        {
            Destroy(hijo.gameObject);
        } 
        foreach (AnimalData animal in nuevaSeccion.animalesDeEstaLetra)
        {
            GameObject btnObj = Instantiate(animalButtonPrefab, gridContainer);
            AnimalButton script = btnObj.GetComponent<AnimalButton>();
            script.Configurar(animal); 
        }
    }

    public void SeccionAnimales(AnimalData animalesData)
    {
        panelPrincipal.SetActive(!panelPrincipal.activeSelf);
        panelAnimales.SetActive(!panelAnimales.activeSelf);

        txtTituloSeccion.text = animalesData.nombreAnimal;

        foreach (Transform hijo in panelAnimalesT)
        {
            Destroy(hijo.gameObject);
        }
        for (int i = 0; i < animalesData.listaPuzzles.Length; i++)
        {
            PuzzleInfo info = animalesData.listaPuzzles[i];
            GameObject btnObj = Instantiate(animalItemPrefab, panelAnimalesT);
            PuzzleItemButton script = btnObj.GetComponent<PuzzleItemButton>();
            script.Configurar(info, animalesData.nombreAnimal, i);
        }

    }

    public void SeleccionarPuzzle(Image image)
    {

        puzzleSeleccionado = image.sprite;

        panelDificultad.SetActive(!panelDificultad.activeSelf);
        //SceneManager.LoadScene("NivelPuzzle"); // tu escena del puzzle
    }
    public void cerrarDificultad()
    {
        panelDificultad.SetActive(!panelDificultad.activeSelf);
    }
    public void ElegirDificultad(bool estado)
    {
        dificultadEstado = estado; 
        SceneManager.LoadScene("NivelPuzzle");
    }
    public void DificultadFacil()
    {
        ElegirDificultad(true);
    }
    public void DificultadDificil()
    {
        ElegirDificultad(false);
    }
    public void volverPanelPrincipal()
    {
        panelPrincipal.SetActive(!panelPrincipal.activeSelf);
        panelAnimales.SetActive(!panelAnimales.activeSelf);

    }
}
