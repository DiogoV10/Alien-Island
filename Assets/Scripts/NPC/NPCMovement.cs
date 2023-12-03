using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{

    Vector3 initialPosition;
    Vector3 targetPosition;

    bool inGame = false;
    float walkZoneRadius = 5f, speed = 3f;
    int wayPointCount = 0;
    [SerializeField] int scenarioLayer;

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
        inGame = state == GameState.InGame;
    }

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        ChoseWayPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (inGame)
        {
            MovingToWayPoint();
        }
    }

    void ChoseWayPoint()
    {
        if (wayPointCount < 2)
        {
            Vector2 randomPoint = Random.insideUnitCircle * walkZoneRadius;
            targetPosition = new Vector3(randomPoint.x, 0, randomPoint.y) + transform.position;
            wayPointCount++;
        }
        else
        {
            targetPosition = initialPosition;
            wayPointCount = 0;
        }
    }

    void MovingToWayPoint()
    {
        transform.LookAt(targetPosition);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        if (Vector3.Distance(targetPosition, transform.position) < 0.1f)
        {
            ChoseWayPoint();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Scenario")
        {
            ChoseWayPoint();
        }
    }
}
