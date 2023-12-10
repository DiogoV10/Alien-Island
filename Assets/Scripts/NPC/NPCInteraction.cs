using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class NPCInteraction : MonoBehaviour
{
    int playerMask = 3;
    InputSystem inputSystem;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Transform player;
    bool insideZone = false, indialogue = false;
    private Transform imageT;
    [SerializeField] private Camera mainCam, dialogueCam;
    [SerializeField] ChatBubble chatBubble;
    [SerializeField] Transform buttonT;
    Transform chatBubbleClone;

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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == playerMask)
        {
            insideZone = false;
            Destroy(imageT.gameObject);
        }
    }

    void EnterDialogue()
    {
        dialogueCam.enabled = true;
        mainCam.enabled = false;
        transform.LookAt(new Vector3(player.transform.position.x, player.transform.position.y + 1f, player.transform.position.z));
        player.LookAt(new Vector3(transform.position.x, 0, transform.position.z));
        chatBubbleClone = chatBubble.CreateChatBubble(transform, new Vector3(0, 1.3f, 0), speechString);
        gameObject.GetComponent<NPCDialogue>().SetNPCText(chatBubbleClone);
    }

    void ExitDialogue()
    {
        dialogueCam.enabled = false;
        mainCam.enabled = true;
        Destroy(chatBubbleClone.gameObject);
    }
}
