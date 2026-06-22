using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI Time;
    public TextMeshProUGUI Floors;
    public TextMeshProUGUI EnemyCounter;
    public TextMeshProUGUI HighscoreKills;
    public TextMeshProUGUI HighscoreFloors;

    private int killsHighscore;
    private int floorsHighscore;

    private void OnEnable()
    {
        PlayerHealth.OnSetRunTime += SetTime;
        PlayerHealth.OnSetFloorCount += SetFloorCount;


        SetKillCount(GameManager.Instance.KillCount);
        SetHighscore();
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

        if(floorCount > PlayerPrefs.GetInt("Floors", 0))
        {
            floorsHighscore = floorCount;
            PlayerPrefs.SetInt("Floors", floorCount);
        }

        Floors.text = "You survived " + floorCount + floors;

    }

    private void SetKillCount(int killCount)
    {
        if (killCount > PlayerPrefs.GetInt("Kills", 0))
        {
            killsHighscore = killCount;
            PlayerPrefs.SetInt("Kills", killCount);
        }

        EnemyCounter.text = "You killed " + killCount + " enemies.";
    }

    private void SetHighscore()
    {
        int kills = PlayerPrefs.GetInt("Kills", 0);
        int floors = PlayerPrefs.GetInt("Floors", 0);

        if(kills > 0)
        {
            if(kills == 1)
            {
                HighscoreKills.text = "Highscore :  " + kills + " kill.";
            }

            HighscoreKills.text = "Highscore :  " + kills + " kills.";
        }
        else
        {
            HighscoreKills.text = " ";
        }

        if(floors > 0)
        {
            if (floors == 1)
            {
                HighscoreKills.text = "Highscore :  " + floors + " floor.";
            }
            HighscoreFloors.text = "Highscore :  " + floors + " floors.";

        }
        else
        {
            HighscoreFloors.text = " ";

        }
    }
}
