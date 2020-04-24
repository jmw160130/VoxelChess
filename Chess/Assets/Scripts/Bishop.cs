using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : MonoBehaviour
{
    public GameObject indicator;
    public int side;
    public Camera camera;
    public GameObject cont;
    private List<GameObject> moves;
    public bool bish;
    private bool selected;
    private bool unmoved = true;
    private bool waitActive;

    private GameObject[] diags;
    int d1, d2, d3, d4;
    private int checkTimer = -1;

    // Use this for initialization
    void Start()
    {
        selected = false;
        moves = new List<GameObject>();
        diags = new GameObject[16];
        d1 = -1;
        d2 = -1;
        d3 = -1;
        d4 = -1;
    }

    // Update is called once per frame
    void Update()
    {
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
                    if(bish)
                    {
                        cont.SendMessage("changeTurn");
                    }
                }
            }

            foreach (GameObject i in moves)
            {
                Destroy(i);
            }
            moves = new List<GameObject>();
            diags = new GameObject[16];
            d1 = -1;
            d2 = -1;
            d3 = -1;
            d4 = -1;
            StartCoroutine(Wait(false));
        }
    }

    private void FixedUpdate()
    {
        if (checkTimer > 0)
        {
            checkTimer--;
        }
        else if (checkTimer == 0)
        {
            bool ended = false;
            for (int e = 0; e < d4; e++)
            {
                if(e == d1 || e == d2 || e ==d3)
                {
                    ended = false;
                    Debug.Log("NEW LINE");
                }

                if (diags[e] != null && ended)
                {
                    Destroy(diags[e]);
                }

                if (diags[e] == null || diags[e].transform.position.y != 0)
                {
                    ended = true;
                    Debug.Log("END LINE");
                }
            }
            
            /*bool ended = false;
            for (int e = 0; e < 7; e++)
            {
                if (e == placeR)
                {
                    ended = false;
                    Debug.Log("NEW LINE");
                }

                if (row[e] != null && ended)
                {
                    Destroy(row[e]);
                }

                if (row[e] == null || row[e].transform.position.y != 0)
                {
                    ended = true;
                    Debug.Log("END LINE");
                }
            }*/

            checkTimer = -1;
        }
    }

    void Moves()
    {
        if (!selected)
        {
            Debug.Log("Bishop MOVES");
            StartCoroutine(Wait(true));
            int cross = 0;
            int place = 0;
            //generate in a cross
            Vector3 standard = this.gameObject.transform.position;
            while (d4 == -1)
            {
                if (cross == 0) { standard = new Vector3(standard.x + side, 0, standard.z + side); }
                else if(cross == 1) { standard = new Vector3(standard.x - side, 0, standard.z - side); }
                else if (cross == 2) { standard = new Vector3(standard.x + side, 0, standard.z - side); }
                else if (cross == 3) { standard = new Vector3(standard.x - side, 0, standard.z + side); }

                if (checkBounds(standard))
                {
                    GameObject ind = Instantiate(indicator, standard, Quaternion.identity);
                    moves.Add(ind);
                    diags[place] = ind;
                    ind.SendMessage("ckCollison", "Take");
                    place++;
                }
                else
                {
                    if (d1 == -1) {
                        d1 = place;
                        cross = 1;
                    }
                    else if (d2 == -1)
                    {
                        d2 = place;
                        cross = 2;
                    }
                    else if (d3 == -1)
                    {
                        d3 = place;
                        cross = 3;
                    }
                    else{d4 = place;}

                    standard = this.gameObject.transform.position;
                }
            }

            checkTimer = 3;
        }
    }

    IEnumerator Wait(bool w)
    {
        waitActive = true;
        yield return new WaitForSeconds(0.05f);
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
