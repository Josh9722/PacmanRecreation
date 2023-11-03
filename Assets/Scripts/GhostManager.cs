using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public GameObject gameManagers;
    public GameObject[] ghosts = new GameObject[4];
    private Animator[] animators = new Animator[4];

    public int currentState = 1; // 1 = Alive, 2 = Scared, 3 = Recovery, Dead

    private MapManager mapManager;
    private AudioManager audioManager;
    private HUDManager hudManager;



    // Start is called before the first frame update
    void Start()
    {
        mapManager = gameManagers.GetComponentInChildren<MapManager>();
        audioManager = gameManagers.GetComponentInChildren<AudioManager>();
        
        // Find In Scene
        hudManager = GameObject.Find("HUD").GetComponent<HUDManager>();

        // Get all the animators from the ghosts
        for (int i = 0; i < ghosts.Length; i++) {
            animators[i] = ghosts[i].GetComponent<Animator>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        // If scared do special stuff. 
        if (currentState == 2)
        {
            // Check if ghost timer is less than 3 seconds 
            if (hudManager.ghostTimerTimer.GetTime() < 3.0f)
            {
                // Change all ghosts to recovery
                changeState(3);
            }
        }

        // If recovery do special stuff.
        if (currentState == 3)
        {
            // Check if ghost timer is finished
            if (hudManager.ghostTimerTimer.IsFinished())
            {
                // Change all ghosts to alive
                changeState(1);

                // Change BackGroundMusic to normal alive music 
                audioManager.PlayNormalGhostMusic();

                // Reset ghost timer
                hudManager.resetGhostTimer();
            }
        }


    }

    public void powerPelletEaten() {
        // Change all ghosts to scared
        changeState(2);

        // Change BackGroundMusic to ScaredState
        audioManager.PlayScaredGhostMusic();

        // Set and start 10 second timer for UI 
        hudManager.enableGhostTimer();

        //  
    }

    private void changeState(int state) {
        currentState = state;
        foreach (Animator animator in animators)
        {
            if (currentState == 1)
            {
                // Alive 
                animator.SetBool("IsDead", false);
                animator.SetBool("IsRecovery", false);
                animator.SetBool("IsScared", false);
            }
            else if (currentState == 2)
            {
                // Scared
                animator.SetBool("IsDead", false);
                animator.SetBool("IsRecovery", false);
                animator.SetBool("IsScared", true);
            }
            else if (currentState == 3)
            {
                // Recovery
                animator.SetBool("IsDead", false);
                animator.SetBool("IsScared", false);
                animator.SetBool("IsRecovery", true);
            }
            else if (currentState == 4)
            {
                animator.SetBool("IsRecovery", false);
                animator.SetBool("IsScared", false);
                animator.SetBool("IsDead", true);
            }
        }
    }

}
