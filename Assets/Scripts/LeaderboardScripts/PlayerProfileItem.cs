using UnityEngine;
using UnityEngine.UI;

public class PlayerProfileItem : MonoBehaviour
{
    [SerializeField] private Text playerNameText, playerScoreText, playerRankText;
    [SerializeField] private Image playerRankBg;
    public RectTransform rectTransform;

    private void OnEnable()
    {
        EventHandlerCustom.PlayerScoreUpdate += ListenToUpdatePlayerScore;
    }

    private void OnDisable()
    {
        EventHandlerCustom.PlayerScoreUpdate -= ListenToUpdatePlayerScore;
    }

    public string GetPlayerName()
    {
        return playerNameText.text;
    }

    public void SetPlayerProfile(string playerName, int playerScore, int playerRank, Sprite playerRankBgSprite)
    {
        playerNameText.text = playerName;
        playerScoreText.text = playerScore.ToString();
        playerRankText.text = playerRank.ToString();

        playerRankBg.sprite = playerRankBgSprite;
    }

    private void ListenToUpdatePlayerScore(string playerName, int playerScore, int playerRank, Sprite playerRankBgSprite)
    {
        if (string.Equals(playerName, playerNameText.text))
        {
            playerNameText.text = playerName;
            playerScoreText.text = playerScore.ToString();
            playerRankText.text = playerRank.ToString();

            playerRankBg.sprite = playerRankBgSprite;
        }
    }
}
