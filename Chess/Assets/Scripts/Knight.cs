using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    public GameObject indicator;
    public int side;
    public Camera camera;
    public GameObject cont;
    private List<GameObject> moves;
    private bool selected;
    private bool unmoved = true;
    private bool waitActive;

    // Use this for initialization
    void Start()
    {
        selected = false;
        moves = new List<GameObject>();
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

    void Moves()
    {
        if (!selected)
        {
            Debug.Log("KNIGHT MOVES");
            StartCoroutine(Wait(true));
            
            Vector3 standard = new Vector3(gameObject.transform.position.x + 2*side, 0, gameObject.transform.position.z+side);
            if (checkBounds(standard))
            {
                GameObject ind = Instantiate(indicator, standard, Quaternion.identity);
                moves.Add(ind);
                ind.SendMessage("ckCollison", "Take");
            }

            standard = new Vector3(gameObject.transform.position.x + 2 * side, 0, gameObject.transform.position.z - side);
            if (checkBounds(standard))
            {
                GameObject ind = Instantiate(indicator, standard, Quaternion.identity);
                moves.Add(ind);
                ind.SendMessage("ckCollison", "Take");
            }

            standard = new Vector3(gameObject.transform.position.x + side, 0, gameObject.transform.position.z + 2*side);
            if (checkBounds(standard))
            {
                GameObject ind = Instantiate(indicator, standard, Quaternion.identity);
                moves.Add(ind);
                ind.SendMessage("ckCollison", "Take");
            }

            standard = new Vector3(gameObject.transform.position.x + side, 0, gameObject.transform.position.z - 2 * side);
            if (checkBounds(standard))
            {
                GameObject ind = Instantiate(indicator, standard, Quaternion.identity);
                moves.Add(ind);
                ind.SendMessage("ckCollison", "Take");
            }

            standard = new Vector3(gameObject.transform.position.x - 2 * side, 0, gameObject.transform.position.z + side);
            if (checkBounds(standard))
            {
                GameObject ind = Instantiate(indicator, standard, Quaternion.identity);
                moves.Add(ind);
                ind.SendMessage("ckCollison", "Take");
            }

            standard = new Vector3(gameObject.transform.position.x - 2 * side, 0, gameObject.transform.position.z - side);
            if (checkBounds(standard))
            {
                GameObject ind = Instantiate(indicator, standard, Quaternion.identity);
                moves.Add(ind);
                ind.SendMessage("ckCollison", "Take");
            }

            standard = new Vector3(gameObject.transform.position.x - side, 0, gameObject.transform.position.z + 2 * side);
            if (checkBounds(standard))
            {
                GameObject ind = Instantiate(indicator, standard, Quaternion.identity);
                moves.Add(ind);
                ind.SendMessage("ckCollison", "Take");
            }

            standard = new Vector3(gameObject.transform.position.x - side, 0, gameObject.transform.position.z - 2 * side);
            if (checkBounds(standard))
            {
                GameObject ind = Instantiate(indicator, standard, Quaternion.identity);
                moves.Add(ind);
                ind.SendMessage("ckCollison", "Take");
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
