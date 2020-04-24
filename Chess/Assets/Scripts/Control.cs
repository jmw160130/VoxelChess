using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    public Camera camera;
    private int turn;
    private GameObject selected;

    // Start is called before the first frame update
    void Start()
    {
        turn = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Piece1" && turn == -1)
                {
                    Debug.Log("HIT PIECE1");
                    selected = hit.collider.gameObject;
                    hit.collider.gameObject.SendMessage("Moves");
                }
                else if (hit.transform.tag == "Piece2" && turn == 1)
                {
                    Debug.Log("HIT PIECE2");
                    selected = hit.collider.gameObject;
                    hit.collider.gameObject.SendMessage("Moves");
                }
            }
        }
    }

    void changeTurn()
    {
        Debug.Log("TURN CHANGE");
        turn = turn * -1;
    }
}
