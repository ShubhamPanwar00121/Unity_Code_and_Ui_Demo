using UnityEngine;

public interface IRewardItem 
{
    void AddReward(int day,string name, int quantity, Sprite icon);
    void SetAvailable();
    void SetClaimed();
}