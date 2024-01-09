using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class NPCInteraction : MonoBehaviour
{

    InputSystem inputSystem;
    Animator animator;

    [SerializeField] PlayerInput playerInput;
    [SerializeField] Camera mainCam, dialogueCam;
    [SerializeField] ChatBubble chatBubble;

    [SerializeField] Transform player;
    [SerializeField] Transform buttonT;
    Transform imageT;
    Transform chatBubbleClone;

    int playerMask = 3;

    bool insideZone = false, indialogue = false;

    public string speechString;

    void Awake()
    {
        GameManager.OnGameStateChange += GameManagerOnGameStateChange;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChange -= GameManagerOnGameStateChange;
    }

    private void GameManagerOnGameStateChange(GameState state)
    {
        indialogue = state == GameState.NpcDialogue;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if(inputSystem == null)
        {
            inputSystem = new InputSystem();
            inputSystem.Interactions.Interact.performed += i => ChangeState();
        }

        inputSystem.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(imageT != null)
        {
            imageT.rotation = Quaternion.Euler(0, mainCam.transform.rotation.eulerAngles.y, 0);
        }
    }

    void ChangeState()
    {
        if (!insideZone) return;

        indialogue = !indialogue;
        if (indialogue)
        {
            GameManager.Instance.UpdateGameState(GameState.NpcDialogue);
            //gameObject.GetComponent<NPCDialogue>().enabled = true;
            Destroy(imageT.gameObject);
            EnterDialogue();
        }
        else
        {
            GameManager.Instance.UpdateGameState(GameState.InGame);
            InstantiateInteractButton(transform, new Vector3(0, 2f, 0));
            ExitDialogue();
        }
    }

    Transform InstantiateInteractButton(Transform parent, Vector3 localPosition)
    {
        imageT = Instantiate(buttonT, parent);
        imageT.localPosition = localPosition;

        return imageT;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == playerMask)
        {
            insideZone = true;
            InstantiateInteractButton(transform, new Vector3(0, 2f, 0));
            gameObject.GetComponent<NPCDialogue>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == playerMask)
        {
            insideZone = false;
            Destroy(imageT.gameObject);
            gameObject.GetComponent<NPCDialogue>().enabled = false;
        }
    }

    void EnterDialogue()
    {

        dialogueCam.enabled = true;
        mainCam.enabled = false;

        transform.LookAt(new Vector3(player.transform.position.x, player.transform.position.y + 1f, player.transform.position.z));
        player.LookAt(new Vector3(transform.position.x, 0, transform.position.z));

        animator.SetBool("InDialogue", true);

        chatBubbleClone = chatBubble.CreateChatBubble(transform, new Vector3(0, 1.3f, 0), speechString);
        
        gameObject.GetComponent<NPCDialogue>().SetNPCText(chatBubbleClone);
    }

    void ExitDialogue()
    {

        dialogueCam.enabled = false;
        mainCam.enabled = true;

        animator.SetBool("InDialogue", false);

        Destroy(chatBubbleClone.gameObject);
    }
}