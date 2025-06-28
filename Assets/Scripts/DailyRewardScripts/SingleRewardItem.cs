using UnityEngine;
using UnityEngine.UI;

public class SingleRewardItem : MonoBehaviour, IRewardItem
{
    [SerializeField] private Text quantity;
    [SerializeField] private Image icon;
    [SerializeField] private Transform tick;
    public ScaleAnimation scaleAnim;

    public void AddReward(int day, string name, int quantity, Sprite icon)
    {
        this.quantity.text = $"{quantity}X";
        this.icon.sprite = icon;
        tick.gameObject.SetActive(false);
    }

    public void SetAvailable()
    {
       scaleAnim.enabled = true;
    }

    public void SetClaimed()
    {
        scaleAnim.enabled = false;
        tick.gameObject.SetActive(true);
    }
}
