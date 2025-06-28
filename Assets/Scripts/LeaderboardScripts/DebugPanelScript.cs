using UnityEngine;
using UnityEngine.UI;

public class DebugPanelScript : MonoBehaviour
{
    public Leaderboard Leaderboard;
    public InputField PlayerName;
    public InputField PlayerScore;
    public Transform EnableBtn;

    public void SubmitScore()
    {
        if ( PlayerName.text == "" || PlayerScore.text == "")
        {
            return;
        }

        try
        {
            string playerName = PlayerName.text;
            int score = int.Parse(PlayerScore.text);

            Leaderboard.UpdatePlayerScore(playerName, score);
            
            EnableBtn.gameObject.SetActive(true);
            transform.gameObject.SetActive(false);
        }
        catch 
        { 
            return;
        }
    }
}
