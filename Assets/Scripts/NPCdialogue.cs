using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class NPCdialogue : MonoBehaviour
{ [Header("UI Elements")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;

    [Header("Dialogue Lines")]
    [TextArea(2, 5)]
    public string[] dialogueLines;

    private int currentLine = 0;
    private bool isPlayerNearby = false;

    private InputSystem_Actions controls;

    private Animator anim;

    private void Awake()
    {
        controls = new InputSystem_Actions();
        anim = GetComponent<Animator>();

    }

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Interact.performed += OnInteract ;

    }

    private void OnDisable()
    {
        controls.Player.Interact.performed -= OnInteract;
        controls.Player.Disable();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (isPlayerNearby)
        {
            if (!dialoguePanel.activeInHierarchy)
            {
                OpenDialogue();
            }
            else
            {
                NextLine();
            }
        }
    }

    void OpenDialogue()
    {
        anim.SetBool("isTalking", true);
        dialoguePanel.SetActive(true);
        dialogueText.text = dialogueLines[currentLine];
    }

    void NextLine()
    {
        currentLine++;
        if (currentLine < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLine];
        }
        else
        {
            anim.SetBool("isTalking", false);
            dialoguePanel.SetActive(false);
            currentLine = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Player entered NPC trigger");
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            dialoguePanel.SetActive(false);
            currentLine = 0;
        }
    }
}
