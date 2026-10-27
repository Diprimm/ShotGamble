using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private Vector3 originalPos;
    [SerializeField] private Quaternion originalRot;

    private void Awake()
    {
        originalPos = transform.localPosition;
    }

    public IEnumerator Shake(float duration, float magnitude, float rotationMagnitude)
    {
        float elapsed = 0;

        Quaternion rotationX = Quaternion.AngleAxis(-rotationMagnitude, Vector3.right);

        transform.localRotation = rotationX;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z + -0.09f);

            elapsed += Time.deltaTime;



            yield return null;
        }

        //transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, Time.deltaTime * 3);

        //transform.localPosition = originalPos;
    }

    private void LateUpdate()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, Time.deltaTime * 15);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, 15 * Time.deltaTime);
    }
}
