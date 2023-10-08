using UnityEngine;

namespace Camera
{
    public class CameraController : MonoBehaviour
    {
        public Transform player;

        [SerializeField] private float followSpeed = 2.0f;

        private void LateUpdate()
        {
            Vector3 targetPosition = new Vector3(player.position.x, player.position.y, -1.0f);

            transform.position = Vector3.Slerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }
}
