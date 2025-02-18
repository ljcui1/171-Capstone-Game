using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DestoryOnAnimationComplete : MonoBehaviour
{
    public float delay; // Additional delay in seconds

    void Start()
    {
        Animator animator = GetComponent<Animator>();
        StartCoroutine(DestroyAfterAnimation(animator));
    }

    IEnumerator DestroyAfterAnimation(Animator animator)
    {
        // Wait until the animation state information is available
        yield return null;

        // Get the length of the current animation clip
        float animTime = animator.GetCurrentAnimatorStateInfo(0).length;

        float elapsedTime = 0f;
        float totalDuration = animTime + delay;

        while (elapsedTime < totalDuration)
        {
            elapsedTime += Time.unscaledDeltaTime; // Use unscaled time
            yield return null;
        }

        Destroy(gameObject);
    }
}
