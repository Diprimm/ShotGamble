using UnityEngine;

public class Gatling : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float recoverSpeed;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            transform.Rotate(new Vector3(0, 0, rotationSpeed));
        }

        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                transform.localRotation = Quaternion.Euler(0, 0, -180);
            }

            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * recoverSpeed);
        }
    }
}
