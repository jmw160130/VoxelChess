using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour {
    public GameObject indicator;
    public int side;
    public Camera camera;
    public GameObject cont;
    private List<GameObject> moves;
    private bool selected;
    private bool unmoved = true;
    private bool waitActive;

    // Use this for initialization
    void Start() {
        selected = false;
        moves = new List<GameObject>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0) && selected)
        {
            Debug.Log("MOVE AWAY");
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Indicator1" || hit.transform.tag == "Indicator2")
                {
                    Debug.Log("HIT INDICATOR");
                    hit.transform.gameObject.SendMessage("Take");
                    unmoved = false;
                    StartCoroutine(Wait(false));
                    this.transform.position = new Vector3(hit.transform.position.x, this.transform.position.y, hit.transform.position.z);
                    cont.SendMessage("changeTurn");
                }
            }
            
            foreach(GameObject i in moves)
            {
                Destroy(i);
            }
            moves = new List<GameObject>();
            StartCoroutine(Wait(false));
        }
    }

    void Moves()
    {
        if (!selected)
        {
            Debug.Log("PAWN MOVES");
            StartCoroutine(Wait(true));

            //check in front
            Vector3 standard = new Vector3(gameObject.transform.position.x + side, 0, gameObject.transform.position.z);
            if(checkBounds(standard))
            {
                GameObject ind1 = Instantiate(indicator, standard, Quaternion.identity);
                moves.Add(ind1);
                ind1.SendMessage("ckCollison", "NoTake");
            }

            //check diagonal
            standard = new Vector3(standard.x, standard.y, gameObject.transform.position.z + side);
            if (checkBounds(standard))
            {
                GameObject ind2 = Instantiate(indicator, standard, Quaternion.identity);
                moves.Add(ind2);
                ind2.SendMessage("ckCollison", "Unoccupied");
            }

            standard = new Vector3(standard.x, standard.y, gameObject.transform.position.z - side);
            if (checkBounds(standard))
            {
                GameObject ind3 = Instantiate(indicator, standard, Quaternion.identity);
                moves.Add(ind3);
                ind3.SendMessage("ckCollison", "Unoccupied");
            }
            

            //check if the pawn hasnt moved yet
            if (unmoved)
            {
                Debug.Log("FIRST BLOOD");
                standard = new Vector3(standard.x + side, standard.y, gameObject.transform.position.z);
                GameObject ind4 = Instantiate(indicator, standard, Quaternion.identity);
                moves.Add(ind4);
                ind4.SendMessage("ckCollison", "NoTake");
            }

            Debug.Log("MOVE ADDED");
        }
    }

    IEnumerator Wait(bool w)
    {
        waitActive = true;
        yield return new WaitForFixedUpdate();
        selected = w;
        waitActive = false;
    }

    public bool checkBounds(Vector3 cVal)
    {
        bool inBounds = false;
        if (cVal.x < 3.9 && cVal.x > -4 && cVal.z > -3.9 && cVal.z < 4)
        {
            inBounds = true;
        }
        else { Debug.Log(cVal); }

        return inBounds;
    }
}
