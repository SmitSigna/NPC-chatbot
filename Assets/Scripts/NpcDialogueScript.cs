using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NpcDialogueScript : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject ToActivate;

    [SerializeField] private Transform standingPoint;
    [SerializeField] private GameObject mouseMovement;
    
    Transform player;

    private characterMovement characterMove;
    private Animator anim;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;

            //to switch camera
            mainCamera.SetActive(false);
            ToActivate.SetActive(true);

            //to tp the player

            player.position = standingPoint.position;
            player.rotation = standingPoint.rotation;

            // to disable player movement
            characterMove = other.GetComponent<characterMovement>();
            if (characterMove != null)
            {
                characterMove.enabled = false;
                mouseMovement.SetActive(false);
            }

            //to play talking animation
            anim = other.GetComponent<Animator>();
            if (anim != null)
            {
                //Debug.Log("talking");
                anim.Play("Talking");
            }

            
        }
    }


    public void CloseDialogue()
    {
        // revert camera
        mainCamera.SetActive(true);
        ToActivate.SetActive(false);

        // enable player movement again
        if (characterMove != null)
        {
            characterMove.enabled = true;
            mouseMovement.SetActive(true);
        }

        // stop talking animation (go back to Idle)
        if (anim != null)
        {
            anim.Play("Breathing Idle");
        }
    }
}


    

   

