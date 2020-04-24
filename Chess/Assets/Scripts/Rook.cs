using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : MonoBehaviour
{
    public GameObject indicator;
    public int side;
    public Camera camera;
    public GameObject cont;
    private List<GameObject> moves;
    private bool selected;
    private bool unmoved = true;
    private bool waitActive;

    private GameObject[] row;
    private GameObject[] collumn;
    private int placeC;
    private int placeR;
    private int checkTimer = -1;

    // Use this for initialization
    void Start()
    {
        selected = false;
        moves = new List<GameObject>();
        row = new GameObject[7];
        collumn = new GameObject[7];
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
                    cont.SendMessage("changeTurn");
                }
            }

            foreach (GameObject i in moves)
            {
                Destroy(i);
            }
            moves = new List<GameObject>();
            StartCoroutine(Wait(false));
        }
    }

    private void FixedUpdate()
    {
        if(checkTimer>0)
        {
            checkTimer--;
        }
        else if(checkTimer == 0)
        {
            bool ended = false;
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
            }

            ended = false;
            for (int e = 0; e < 7; e++)
            {
                if (e == placeC)
                {
                    ended = false;
                    Debug.Log("NEW LINE");
                }

                if (collumn[e] != null && ended)
                {
                    Destroy(collumn[e]);
                }

                if (collumn[e] == null || collumn[e].transform.position.y != 0)
                {
                    ended = true;
                    Debug.Log("END LINE");
                }
            }

            checkTimer = -1;
        }
    }

    void Moves()
    {
        if (!selected)
        {
            Debug.Log("ROOK MOVES");
            StartCoroutine(Wait(true));

            //generate full row to edit later
            Vector3 standard = this.gameObject.transform.position;
            bool forward = true;
            placeR = 0;
            for (int f = 0; f < 7; f++)
            {
                if (forward) { standard = new Vector3(standard.x + side, 0, standard.z); }
                else { standard = new Vector3(standard.x - side, 0, standard.z); }

                if (checkBounds(standard))
                {
                    GameObject ind = Instantiate(indicator, standard, Quaternion.identity);
                    moves.Add(ind);
                    row[f] = ind;
                    ind.SendMessage("ckCollison", "Take");
                }
                else {
                    placeR = f;
                    f--;
                    forward = false;
                    standard = this.gameObject.transform.position;
                }
            }

            //collumn
            standard = this.gameObject.transform.position;
            forward = true;
            placeR = 0;
            for (int f = 0; f < 7; f++)
            {
                if (forward) { standard = new Vector3(standard.x, 0, standard.z + side); }
                else { standard = new Vector3(standard.x, 0, standard.z - side); }

                if (checkBounds(standard))
                {
                    GameObject ind = Instantiate(indicator, standard, Quaternion.identity);
                    moves.Add(ind);
                    collumn[f] = ind;
                    ind.SendMessage("ckCollison", "Take");
                }
                else
                {
                    placeC = f;
                    f--;
                    forward = false;
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
