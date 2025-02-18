using UnityEngine;

public class RaccoonRotation : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 10f;
    private Vector3 _previousPosition;
    private Vector3 _currentVelocity;
    void Update()
    {
        CalculateVelocity();
        RotateTowardsMovement();
    }
    void CalculateVelocity()
    {
        _currentVelocity = (transform.position - _previousPosition) / Time.deltaTime;
        _previousPosition = transform.position;
    }

    void RotateTowardsMovement()
    {
        Vector3 movementDirection = new Vector3(_currentVelocity.x, 0, _currentVelocity.z);

        if (movementDirection.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection.normalized);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}
