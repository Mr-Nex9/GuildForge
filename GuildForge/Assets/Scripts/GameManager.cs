using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameState gameState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public float timePassed;
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > 5)
        {
            gameState.AddGold();
            timePassed = 0;
        }
    }
}
