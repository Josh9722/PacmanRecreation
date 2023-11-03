using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject countDown; 

    private TextMeshProUGUI countdownText; 
    public GameObject gameManagers;
    public GameObject pacStudent; 

    private PacStudentController pacStudentController; 
    private AudioManager audioManager; 
    private HUDManager hudManager; 

    // Start is called before the first frame update
    void Start()
    {
        countdownText = countDown.GetComponentInChildren<TextMeshProUGUI>();
        pacStudentController = pacStudent.GetComponent<PacStudentController>();
        audioManager = gameManagers.GetComponentInChildren<AudioManager>();

        // Find in scene 
        hudManager = GameObject.Find("HUD").GetComponent<HUDManager>();


        StartCoroutine(RoundStartSequence());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator RoundStartSequence()
    {
        // Countdown from 3 to "GO!"
        countdownText.text = "3";
        yield return new WaitForSeconds(1f);

        countdownText.text = "2";
        yield return new WaitForSeconds(1f);

        countdownText.text = "1";
        yield return new WaitForSeconds(1f);

        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);

        // Hide the countdown 
        countDown.gameObject.SetActive(false);

        // Enable player control
        pacStudentController.hasGameStarted = true;
        
        // Start normal music 
        audioManager.PlayNormalGhostMusic();

        // Start game counter 
        hudManager.startGameTimer(); 
    }

}
