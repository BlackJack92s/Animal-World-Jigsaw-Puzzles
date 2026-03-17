using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PiezaScript : MonoBehaviour
{
    private Vector3 RightPosition;
    public bool InRightPosition;
    public bool Selected;
    public AudioClip Piecescorrectplaceaud;

    void Start()
    {
        RightPosition = gameObject.transform.position;

        transform.position = new Vector3(Random.Range(4f, 7f), Random.Range(1.5f, -4.0f));
    }

    void Update()
    {
         
        if (Vector3.Distance(transform.position, RightPosition) < 0.5f)
        {
            if (!Selected)
            {
                if (InRightPosition == false)
                {
                    transform.position = RightPosition;
                    InRightPosition = true;
                     
                    GetComponent<SortingGroup>().sortingOrder = 0;
                    Camera.main.GetComponent<DragNDropAdv>().PlacedPieces++;
                    AudioSource.PlayClipAtPoint(Piecescorrectplaceaud, transform.position, 1f);
                }
            }
        }
    }
}
