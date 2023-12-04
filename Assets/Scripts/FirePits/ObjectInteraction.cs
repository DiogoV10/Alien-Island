using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectInteraction : MonoBehaviour
{
    int playerMask = 3;
    [SerializeField] GameObject player;
    [SerializeField] PlayerHealthManager playerHealthManager;
    InputSystem inputSystem;
    bool insideZone = false, inObjectInteraction = false;
    //[SerializeField] private GameObject image;
    [SerializeField] private Camera mainCam;
    [SerializeField] Transform buttonT;
    private Transform imageT;
    float playerHealthFillTime = 2f;

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
        //image.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        player.GetComponent<PlayerHealthManager>();
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
            SkillSelectionUI.Instance.Show();
            if (playerHealthManager.playerHealth < 100)
            {
                GameManager.Instance.UpdateGameState(GameState.NpcDialogue);
                StartCoroutine(FillPlayerLife());
            }
            else
            {
                GameManager.Instance.UpdateGameState(GameState.NpcDialogue);
                Destroy(imageT.gameObject);
            } 
        }
        else
        {
            SkillSelectionUI.Instance.Hide();
            GameManager.Instance.UpdateGameState(GameState.InGame);
            InstantiateInteractButton(transform, new Vector3(2.5f, 0 , 0));
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
            InstantiateInteractButton(transform, new Vector3(2.5f, 0, 0));
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

    IEnumerator FillPlayerLife()
    {
        Destroy(imageT.gameObject);
        while (playerHealthManager.playerHealth < 100)
        {
            playerHealthFillTime -= Time.deltaTime;
            if(playerHealthFillTime < 0.1f)
            {
                playerHealthManager.GetLife();
                playerHealthFillTime = 2f;
                if (playerHealthManager.playerHealth > 100)
                {
                    playerHealthManager.playerHealth = 100;
                    InstantiateInteractButton(transform, new Vector3(2.5f, 0, 0));
                    GameManager.Instance.UpdateGameState(GameState.InGame);
                }
                Debug.Log(playerHealthManager.playerHealth);
            }
            yield return null;
        }
        
    }
}
