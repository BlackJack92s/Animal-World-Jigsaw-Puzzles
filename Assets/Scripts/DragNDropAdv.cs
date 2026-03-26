using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI; 
public class DragNDropAdv : MonoBehaviour
{ 
    public AudioClip clkpieceaudio; 
    public GameObject EndMenu;
    public GameObject SelectedPiece;

    int OIL = 1;
    public int PlacedPieces = 0;
    public int puzzleSize;
    private int monedasEasy = 25;
    private int monedasHard = 50;

    public TextMeshProUGUI txtTitulo;
    void Start()
    {
        if (DatosPartida.Instance.dificultadEstado)
        {
            txtTitulo.text = "Awesome! You received " + monedasEasy + " coins!"; 
        }
        else
        {
            txtTitulo.text = "Awesome! You received  " + monedasHard + " coins!"; 
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)  
            {
                if (hit.transform.CompareTag("PuzzleAdv"))
                {
                    if (!hit.transform.GetComponent<PiezaScript>().InRightPosition)
                    {
                        SelectedPiece = hit.transform.gameObject;
                        SelectedPiece.GetComponent<PiezaScript>().Selected = true;
                        AudioSource.PlayClipAtPoint(clkpieceaudio, transform.position, 0.1f);
                        SelectedPiece.GetComponent<SortingGroup>().sortingOrder = OIL;
                        OIL++;
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (SelectedPiece != null)
            {
                SelectedPiece.GetComponent<PiezaScript>().Selected = false;
                SelectedPiece = null;
            }
        }

        if (SelectedPiece != null)
        {
            Vector3 MousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SelectedPiece.transform.position = new Vector3(MousePoint.x, MousePoint.y, 0);
        }

        if (PlacedPieces == puzzleSize)
        {
            EndMenu.SetActive(true);
        }
    } 
    public void BacktoMenu()
    { 
        SceneManager.LoadScene("MenuPrincipal");
    }
    public void WinGotoMenu()
    {
        if (DatosPartida.Instance.dificultadEstado)
        {
            GameManager.Instance.GanarMonedas(monedasEasy);
        }
        else
        {
            GameManager.Instance.GanarMonedas(monedasHard);
        } 
        SceneManager.LoadScene("MenuPrincipal");
    }
    public void DuplicarMonedas()
    {
        if (DatosPartida.Instance.dificultadEstado)
        {
            GameManager.Instance.VerAnuncioDuplicar(monedasEasy * 2);
        }
        else
        {
            GameManager.Instance.VerAnuncioDuplicar(monedasHard * 2);
        }
        SceneManager.LoadScene("MenuPrincipal");
    }
    //private void OnMenuCargado(Scene scene, LoadSceneMode mode)
    //{
    //    // Desuscribirse para que no se repita
    //    SceneManager.sceneLoaded -= OnMenuCargado;

    //    if (MenuManager.instance != null)
    //        MenuManager.instance.InicializarMenu();
    //}
}
