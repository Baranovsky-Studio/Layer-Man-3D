using UnityEngine;

public class TurnerOff : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TurnerOff"))
        {
            gameObject.SetActive(false);
        }
    }
}
