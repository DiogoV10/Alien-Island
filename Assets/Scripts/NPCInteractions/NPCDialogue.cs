using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class NPCDialogue : MonoBehaviour
{
    int playerMask = 3;
    InputSystem inputSystem;
    [SerializeField] PlayerInput playerInput;
    bool insideZone = false, indialogue = false;
    [SerializeField] private GameObject image;
    [SerializeField] private Camera mainCam, dialogueCam;
    // Start is called before the first frame update

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
        image.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
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
        //if (!indialogue) return;
        //Interacted();
        image.transform.rotation = Quaternion.Euler(0, mainCam.transform.rotation.eulerAngles.y , 0);
    }

    void ChangeState()
    {
        indialogue = !indialogue;
        if (indialogue)
        {
            GameManager.Instance.UpdateGameState(GameState.NpcDialogue);
            image.SetActive(false);
            EnterDialogue();
        }
        else
        {
            GameManager.Instance.UpdateGameState(GameState.InGame);
            image.SetActive(true);
            ExitDialogue();
        }
    }

    //void Interacted()
    //{
    //    if (indialogue) EnterDialogue();
    //    else ExitDialogue();
    //}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == playerMask)
        {
            insideZone = true;
            image.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == playerMask)
        {
            insideZone = false;
            image.SetActive(false);
        }
    }

    void EnterDialogue()
    {
        dialogueCam.enabled = true;
        mainCam.enabled = false;
        inputSystem.Movement.Disable();
    }

    void ExitDialogue()
    {
        dialogueCam.enabled = false;
        mainCam.enabled = true;
    }
}
