using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PuzzleGeneratorScript : MonoBehaviour
{
    public GameObject[] piecePrefab;
    private Sprite puzzle; // Ya no es public, lo traemos del manager
    public GameObject basePuzzle;
    public TextMeshProUGUI txtTitulo;

    void Start()
    {
        StartCoroutine(InicializarPuzzle());
    }

    IEnumerator InicializarPuzzle()
    {
        // 1. Esperamos un frame para asegurar que el MenuManager se asentó
        yield return null;

        if (DatosPartida.Instance != null && DatosPartida.Instance.puzzleSeleccionado != null)
        {
            puzzle = DatosPartida.Instance.puzzleSeleccionado;

            // Configurar la base
            SpriteRenderer baseRenderer = basePuzzle.GetComponent<SpriteRenderer>();
            if (baseRenderer != null)
            {
                baseRenderer.sprite = puzzle;
            }

            // Configurar textos y dificultad
            if (DatosPartida.Instance.dificultadEstado)
            {
                txtTitulo.text = "Level : Easy";
                basePuzzle.SetActive(true);
            }
            else
            {
                txtTitulo.text = "Level : Hard";
                basePuzzle.SetActive(false);
            }

            GeneratePuzzle();
        }
        else
        {
            Debug.LogError("Error: El sprite no llegó del MenuManager o es nulo.");
        }
    }

    void GeneratePuzzle()
    {
        foreach (GameObject piece in piecePrefab)
        {
            Transform childTransform = piece.transform.Find("PuzzleAdv");
            if (childTransform != null)
            {
                Image img = childTransform.GetComponent<Image>();

                if (img != null)
                { 
                    img.sprite = puzzle; 
                    //img.preserveAspect = true;
                }
                //SpriteRenderer sr = childTransform.GetComponent<SpriteRenderer>();
                //if (sr != null)
                //{
                //    sr.sprite = puzzle;
                //}
            }
        }
    }
}