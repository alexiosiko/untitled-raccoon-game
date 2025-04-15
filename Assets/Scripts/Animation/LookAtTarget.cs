using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] bool lockVerticalRotation = false;
    
    void Update()
    {
        if (!target) return;
        
        // Calculate the direction to the target
        Vector3 directionToTarget = target.position - transform.position;
        
        // Optional: Remove vertical component if needed
        if (lockVerticalRotation)
            directionToTarget.y = 0;
        
        // Only rotate if we have a valid direction
        if (directionToTarget != Vector3.zero)
        {
            // Calculate the target rotation
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            
            // Smoothly interpolate between current and target rotation
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}