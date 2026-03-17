using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AddressableAssets; // <-- ¡ESTO ES CLAVE!

[CreateAssetMenu(fileName = "NuevoAnimal", menuName = "Puzzle/Animal")]
public class AnimalData : ScriptableObject
{
    public string nombreAnimal; 
    //public Sprite iconoPrincipal;
    public AssetReferenceSprite iconoPrincipal;

    public PuzzleInfo[] listaPuzzles;// Aquí guardas tus 5 o 10 imágenes
}

[Serializable]
public class PuzzleInfo
{
    public AssetReferenceSprite imagenPuzzle;
    //public Sprite imagenPuzzle;
    //public Sprite imagenPuzzle;
    public int precioMonedas;
}