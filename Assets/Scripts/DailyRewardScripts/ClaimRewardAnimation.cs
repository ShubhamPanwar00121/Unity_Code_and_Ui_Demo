using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClaimRewardAnimation : MonoBehaviour
{
    [SerializeField] private Image rewardIcon;
    [SerializeField] private Transform targetPosition;

    public void SetAnim(Sprite icon)
    {
        rewardIcon.sprite = icon;
        rewardIcon.transform.position = Vector3.zero;
        rewardIcon.transform.localScale = Vector3.zero;
        StartCoroutine(AnimateReward());
    }

    private IEnumerator AnimateReward() // I can do it using dotween also
    {
        float scaleTime = 0.5f;
        float elapsed = 0f;

        while (elapsed < scaleTime)
        {
            float t = elapsed / scaleTime;
            rewardIcon.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        rewardIcon.transform.localScale = Vector3.one;

        Vector3 startPos = rewardIcon.transform.position;
        Vector3 endPos = targetPosition.position;
        float moveTime = 1f;
        elapsed = 0f;

        while (elapsed < moveTime)
        {
            float t = elapsed / moveTime;
            rewardIcon.transform.position = Vector3.Lerp(startPos, endPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rewardIcon.transform.position = endPos;

        gameObject.SetActive(false);
    }
}
