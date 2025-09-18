using UnityEngine;

public class AnimationEventProxy : MonoBehaviour
{
    public void DealDamage()
    {
        if (MeleeAttack.Instance != null)
            MeleeAttack.Instance.DealDamage();
    }

    public void EndAttack()
    {
        if (MeleeAttack.Instance != null)
            MeleeAttack.Instance.EndAttack();
    }
}
