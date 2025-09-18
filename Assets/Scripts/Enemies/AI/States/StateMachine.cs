using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public State currentState;

    private void Start()
    {
        currentState?.OnEnter();
    }

    private void Update()
    {
        RunStateMachine();
    }

    private void RunStateMachine()
    {
        State nextState = currentState?.RunState();
        if (nextState != null && nextState != currentState)
        {
            SwitchState(nextState);
        }
    }

    private void SwitchState(State nextState)
    {
        currentState?.OnExit();

        // Preserve facing direction
        if (currentState != null && nextState != null)
        {

            nextState._isFacingRight = currentState._isFacingRight;
        }

        currentState = nextState;
        currentState?.OnEnter();
    }
}
