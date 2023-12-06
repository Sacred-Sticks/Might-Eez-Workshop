using UnityEngine;

public class Billboard : MonoBehaviour
{
    private void Start()
    {
        // Set the up axis to be facing the camera
        transform.LookAt(Camera.main.transform, Vector3.right);
    }
}