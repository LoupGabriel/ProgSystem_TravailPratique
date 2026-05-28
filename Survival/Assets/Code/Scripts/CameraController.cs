using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform m_target;


    private void Update()
    {
        transform.position = m_target.position;
    }
}
