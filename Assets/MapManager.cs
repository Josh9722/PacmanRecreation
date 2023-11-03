using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    // Array of size 2 of gameobjects 
    public GameObject[] leftTeleporters = new GameObject[2];
    public GameObject[] rightTeleporters = new GameObject[2];
    public GameObject[] ghosts = new GameObject[4];
    public GameObject pacStudent;

    public GameObject gameManagers;
    private GhostManager ghostManager;
    
    
    // Start is called before the first frame update
    void Start()
    {
        ghostManager = gameManagers.GetComponentInChildren<GhostManager>();
        
    }

    // Update is called once per frame
    void Update()
    { 
        
    }

    


}
