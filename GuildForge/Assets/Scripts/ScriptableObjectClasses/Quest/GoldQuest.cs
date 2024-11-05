using UnityEngine;

[CreateAssetMenu(fileName = "GoldQuest", menuName = "Scriptable Objects/Quest/GoldQuest")]
public class GoldQuest : Quest
{
    [SerializeField] bool isCurrentAmount;
    public void CheckforCompletion(int amount)
    {
        GameObject GameMaster = GameObject.FindGameObjectWithTag("GameController");
        GameManager gameManager = GameMaster.GetComponent<GameManager>();

        if(isCurrentAmount)
        {
            currentProgress = gameManager.getCurGold();
        }
        else
        {
            currentProgress = gameManager.GetTotalGold();
        }
    }
}
