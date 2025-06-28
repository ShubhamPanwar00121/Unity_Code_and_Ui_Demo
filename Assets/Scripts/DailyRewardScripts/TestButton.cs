using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    public Button button;
    public Text text;

    private void OnEnable()
    {
        text.text = DataStorage.DailyRewardNewClaimTime == 86400 ? "live\nmode" : "test\nmode";
        button.onClick.AddListener(OnBtnClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }

    private void OnBtnClick()
    {
        if (DataStorage.DailyRewardNewClaimTime == 86400)
        {
            DataStorage.DailyRewardNewClaimTime = 4;
        }
        else
        {
            DataStorage.DailyRewardNewClaimTime = 86400;
        }

        text.text = DataStorage.DailyRewardNewClaimTime == 86400 ? "live\nmode" : "test\nmode";
    }
}
