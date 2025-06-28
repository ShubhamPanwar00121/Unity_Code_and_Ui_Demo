using UnityEngine;
using UnityEngine.UI;

public class DailyRewardItem : MonoBehaviour, IRewardItem
{
    [SerializeField] protected Transform greenGlow, tick;
    [SerializeField] protected Image bg, reward;
    [SerializeField] protected Text dayText, quantityText;
    [SerializeField] protected Sprite normalBg, selectedBg;
    [SerializeField] protected Button claimBtn;
    [SerializeField] private ScaleAnimation scaleAnim;
    protected bool claimed
    {
        get => PlayerPrefs.GetInt($"{rewardName}+{dayText.text}", 0) == 1;
        set => PlayerPrefs.SetInt($"{rewardName}+{dayText.text}", value ? 1 : 0);
    }
    protected string rewardName;

    private void OnEnable()
    {
        claimBtn.onClick.AddListener(ClaimBtnClicked);
    }

    private void OnDisable()
    {
        claimBtn?.onClick.RemoveListener(ClaimBtnClicked);
    }

    public virtual void SetAvailable()
    {
        if (!claimed)
        {
            bg.sprite = selectedBg;
            scaleAnim.enabled = true;
            claimBtn.enabled = true;
            greenGlow.gameObject.SetActive(true);
        }
        else
        {
            tick.gameObject.SetActive(true);
        }
    }

    public virtual void AddReward(int day,string name, int quantity, Sprite icon)
    {
        dayText.text = $"DAY {day}";
        quantityText.text = $"{quantity}X";
        reward.sprite = icon;
        bg.sprite = normalBg;
        rewardName = name;
        greenGlow.gameObject.SetActive(false);
        tick.gameObject.SetActive(false);
        scaleAnim.enabled = false;
        claimBtn.enabled = false;
    }

    public void ClearClaimed()
    {
        claimed = false;
    }

    public virtual void ClaimBtnClicked()
    {
        claimed = true;

        bg.sprite = normalBg;
        scaleAnim.enabled = false;
        claimBtn.enabled = false;
        tick.gameObject.SetActive(true);
        greenGlow.gameObject.SetActive(false);
        DataStorage.ClaimedRewardsThisWeek++;
        EventHandlerCustom.CallAnimateReward(reward.sprite);

        Debug.Log("rewarded add to prefs codde here");
    }

    public void SetClaimed()
    {
    }
}
