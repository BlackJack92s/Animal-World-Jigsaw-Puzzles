using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PuzzleGeneratorScript : MonoBehaviour
{ 
    public GameObject[] piecePrefab; 
    public Sprite puzzle;
    public GameObject basePuzzle;

    public TextMeshProUGUI txtTitulo;
    void Start()
    {
        puzzle = MenuManager.instance.puzzleSeleccionado;

        SpriteRenderer baseRenderer = basePuzzle.GetComponent<SpriteRenderer>();
        if (baseRenderer != null)
        {
            baseRenderer.sprite = puzzle;
        }
        if (MenuManager.instance.dificultadEstado)
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
     
    void GeneratePuzzle()
    {
        foreach (GameObject piece in piecePrefab)
        { 
            Transform childTransform = piece.transform.Find("PuzzleAdv"); 
            if (childTransform != null)
            { 
                SpriteRenderer sr = childTransform.GetComponent<SpriteRenderer>(); 
                if (sr != null)
                {
                    sr.sprite = puzzle;
                }
                else
                {
                    Debug.LogWarning($"El hijo 'PuzzleAdv' en {piece.name} no tiene un SpriteRenderer.");
                }
            }
            else
            {
                Debug.LogWarning($"No se encontró el hijo 'PuzzleAdv' en el objeto: {piece.name}");
            }
        }
    }
}