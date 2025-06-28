using UnityEngine;

public class DailyRewardWeekendItem : DailyRewardItem
{
    [SerializeField] private SingleRewardItem singeReward;
    [SerializeField] private Transform rewardsParent;

    public override void SetAvailable()
    {
        if (claimed)
        {
            foreach (Transform t in rewardsParent.transform)
            {
                t.GetComponent<IRewardItem>().SetClaimed();
            }
        }
        else 
        {
            greenGlow.gameObject.SetActive(true);
            bg.sprite = selectedBg;
            foreach (Transform t in rewardsParent.transform)
            {
                t.GetComponent<IRewardItem>().SetAvailable();
            }
        }
    }

    public override void AddReward(int day, string name, int quantity, Sprite icon)
    {
        SingleRewardItem item = Instantiate(singeReward, rewardsParent);
        item.AddReward(day, name, quantity, icon);
        item.scaleAnim.enabled = false;

        dayText.text = $"DAY {day}";
        rewardName = name;
        greenGlow.gameObject.SetActive(false);
        bg.sprite = normalBg;
    }

    public override void ClaimBtnClicked()
    {
        claimed = true;

        bg.sprite = normalBg;
        foreach (Transform t in rewardsParent.transform)
        {
            t.GetComponent<IRewardItem>().SetClaimed();
        }
        claimBtn.enabled = false;
        greenGlow.gameObject.SetActive(false);
        DataStorage.ClaimedRewardsThisWeek++;

        Debug.Log("rewarded add to prefs codde here");
    }
}
