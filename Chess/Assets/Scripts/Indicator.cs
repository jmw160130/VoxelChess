using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    private string collide;
    private int side;
    private GameObject space;
    private bool occupied = false;
    private int unOccClock = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        if (this.gameObject.transform.tag == "Indicator1") { side = 1; }
        else { side = 2; }
        occupied = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(unOccClock > 0)
        {
            unOccClock--;
        }
        if(unOccClock == 0)
        {
            if(!occupied)
            {
                Destroy(gameObject);
            }
        }
    }

    void ckCollison(string capture)
    {
        collide = capture;
        if(capture == "Unoccupied")
        {
            if(!occupied)
            {
                Debug.Log("UNOCCUPIED");
                unOccClock = 5;
            }
        }
    }

    void Take()
    {
        Destroy(space);
    }

    private void OnTriggerStay(Collider other)
    {
        space = other.gameObject;
        occupied = true;
        if ((other.transform.tag == "Piece1" && side == 1) || (other.transform.tag == "Piece2" && side == 2))
        {
            Debug.Log("SPACE OCCUPIED");
            Destroy(gameObject);
        }
        else if(collide == "NoTake")
        {
            Debug.Log("NO TAKE");
            Destroy(gameObject);
        }
        else if(collide == "Take" || collide == "Unoccupied")
        {
            Vector3 up = this.gameObject.transform.position;
            this.gameObject.transform.position = new Vector3(up.x, up.y+0.1f, up.z);
        }
    }
}
