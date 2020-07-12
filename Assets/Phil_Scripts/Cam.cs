using System.Collections;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public GameObject player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    public float levelStartViewSpeed = 0.1f;

    bool followPlayer = false;
    bool allowCameraMovement = false;

    private void Start()
    {
        followPlayer = false;
        allowCameraMovement = false;
        StartCoroutine(StartUp());
    }

    private IEnumerator StartUp()
    {
        yield return new WaitForSeconds(0.5f);
        allowCameraMovement = true;
    }

    void Update()
    {
        Vector3 finalPosition = player.transform.position + offset;
        Vector3 currentPosition = transform.position;

        if (followPlayer)
        {
            Debug.Log("Cam lerps");
            currentPosition = Vector3.Lerp(transform.position, finalPosition, smoothSpeed * Time.deltaTime);
        }
        else if(allowCameraMovement)
        {
            Debug.Log("Cam moves");
            var direction = finalPosition - transform.position;
            direction = direction.normalized;

            currentPosition = transform.position + direction * levelStartViewSpeed * Time.deltaTime;

            if((currentPosition - finalPosition).sqrMagnitude < 0.01f)
            {
                followPlayer = true;
            }
        }
        transform.position = currentPosition;
    }
}
