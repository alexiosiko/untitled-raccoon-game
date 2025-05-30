using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    public float bobbingSpeed = 14f;
    public float bobbingAmount = 0.05f;
    private float defaultPosY = 0f;
    private float timer = 0f;
    private CharacterController controller;
    void Start()
    {

		defaultPosY = transform.localPosition.y;
        controller = GetComponentInParent<CharacterController>();
    }

    void Update()
    {
        if (controller != null && controller.velocity.magnitude > 0.1f)
        {
            // Player is moving
            timer += Time.deltaTime * bobbingSpeed;
            float newY = defaultPosY + Mathf.Sin(timer) * bobbingAmount;
            transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        }
        else
        {
            // Player is idle
            timer = 0f;
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, defaultPosY, Time.deltaTime * bobbingSpeed), transform.localPosition.z);
        }
    }
}
