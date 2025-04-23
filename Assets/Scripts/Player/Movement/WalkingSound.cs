using System.Collections;
using UnityEngine;

#nullable enable

public class WalkingSound : StateMachineBehaviour
{
    private float footStepTimer = 0f;
    private float footStepCooldown = 0.5f;
    public string sound = "Walk";

    // Called when the state starts
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        footStepTimer = 0f;
        AudioManager.Instance.Play(sound);
    }

    // Called every frame while in this state
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        footStepTimer += Time.deltaTime;
        if (footStepTimer >= footStepCooldown)
        {
            AudioManager.Instance.Play(sound);
            footStepTimer = 0f;
        }
    }

    // Called when the state ends
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AudioManager.Instance.Stop(sound);
    }
}
