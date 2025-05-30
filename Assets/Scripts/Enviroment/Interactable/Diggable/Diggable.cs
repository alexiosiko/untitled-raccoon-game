using System.Collections;
using UnityEngine;

public class Diggable : MonoBehaviour
{
    [SerializeField] GameObject[] piecesOfDirt;
    [SerializeField] ParticleSystem digDustFX;
    [HideInInspector] public Vector3 digDustFromPoint;

    bool digging = false;
    Coroutine digRoutine;

    public void EnterDig(Vector3 digDirection)
    {
        digging = true;

        if (digDustFX != null)
        {
            digDustFX.transform.position = digDustFromPoint;
            digDustFX.Play();
        }

        digRoutine = StartCoroutine(ShootDirt(digDirection));
    }

    public void ExitDig()
    {
        digging = false;

        if (digRoutine != null)
            StopCoroutine(digRoutine);

        if (digDustFX != null)
            digDustFX.Stop();
    }

    IEnumerator ShootDirt(Vector3 digDirection)
    {
        while (digging)
        {
            GameObject dirt = Instantiate(piecesOfDirt[Random.Range(0, piecesOfDirt.Length)], digDustFromPoint, Quaternion.identity);
            Rigidbody rb = dirt.GetComponent<Rigidbody>();

            Vector3 randomDir = Quaternion.Euler(
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f),
                0f
            ) * digDirection;

            float force = Random.Range(2f, 4f);
            rb.AddForce(randomDir * force, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * 5f, ForceMode.Impulse);

            dirt.transform.localScale *= Random.Range(0.8f, 1.2f);

            yield return new WaitForSeconds(0.25f);
        }
    }
}
