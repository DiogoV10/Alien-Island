using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectInteraction : MonoBehaviour
{
    int playerMask = 3;
    InputSystem inputSystem;
    bool insideZone = false, inObjectInteraction = false;
    [SerializeField] private GameObject image;
    [SerializeField] private Camera mainCam, dialogueCam;
    [SerializeField] Transform buttonT;
    private Transform imageT;

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
        inObjectInteraction = state == GameState.NpcDialogue;
    }

    void Start()
    {
        image.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
    }

    private void OnEnable()
    {
        if (inputSystem == null)
        {
            inputSystem = new InputSystem();
            inputSystem.Interactions.Interact.performed += i => ChangeState();
        }

        inputSystem.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (imageT != null)
        {
            imageT.rotation = Quaternion.Euler(0, mainCam.transform.rotation.eulerAngles.y, 0);
        }
    }

    void ChangeState()
    {
        if (!insideZone) return;

        inObjectInteraction = !inObjectInteraction;
        if (inObjectInteraction)
        {
            GameManager.Instance.UpdateGameState(GameState.NpcDialogue);
            Destroy(imageT.gameObject);

            EnterDialogue();
        }
        else
        {
            GameManager.Instance.UpdateGameState(GameState.InGame);
            InstantiateInteractButton(transform, new Vector3(0, 1.3f, 0));

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
        if (other.gameObject.layer == playerMask)
        {
            insideZone = true;
            InstantiateInteractButton(transform, new Vector3(0, 1.3f, 0));
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
    }

    void ExitDialogue()
    {
        dialogueCam.enabled = false;
        mainCam.enabled = true;
    }
}
