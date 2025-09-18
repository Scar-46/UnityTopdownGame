using UnityEngine;

public abstract class State : MonoBehaviour
{
    public bool _isFacingRight = true;

    // Called when state becomes active
    public virtual void OnEnter() { }

    // Called before switching to another state
    public virtual void OnExit() { }

    // Called every frame
    public abstract State RunState();

    protected void Flip(Transform target)
    {
        bool shouldFaceRight = target.position.x > transform.position.x;

        if (shouldFaceRight != _isFacingRight)
        {
            _isFacingRight = shouldFaceRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
    protected void Flip(Vector3 target)
    {
        bool shouldFaceRight = target.x > transform.position.x;

        if (shouldFaceRight != _isFacingRight)
        {
            _isFacingRight = shouldFaceRight;
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (_isFacingRight ? 1 : -1);
            transform.localScale = scale;
        }
    }
}
