using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SeccionAlfabetica", menuName = "Puzzle/Seccion")]
public class SeccionData : ScriptableObject
{
    public char letra;
    public List<AnimalData> animalesDeEstaLetra;
}