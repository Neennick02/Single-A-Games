using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI Time;
    public TextMeshProUGUI Floors;
    public TextMeshProUGUI EnemyCounter;

    private void OnEnable()
    {
        PlayerHealth.OnSetRunTime += SetTime;
        PlayerHealth.OnSetFloorCount += SetFloorCount;
    }

    private void OnDisable()
    {
        PlayerHealth.OnSetRunTime -= SetTime;
        PlayerHealth.OnSetFloorCount -= SetFloorCount;
    }

    private void SetTime(float time)
    {
        float min = time / 60;


        Time.text = "Time played " + Mathf.Floor(min) + " minutes.";
    }
    private void SetFloorCount(int floorCount)
    {
        string floors = " floors.";

        if(floorCount == 1)
        {
            floors = " floor.";
        }
        Floors.text = "You survived " + floorCount + floors;

    }

    private void SetEnemyCount(int enemyCount)
    {
        EnemyCounter.text = "You killed " + enemyCount + " enemies.";
    }
}
