using UnityEngine;
using UnityEngine.SceneManagement;

namespace EscapeRoom.Baseball
{
    public class PortalTriger : MonoBehaviour
    {
        private bool isLoading = false;

        private void OnTriggerEnter(Collider other)
        {
            if (isLoading) return;

            isLoading = true;
            try
            {
                SceneManager.LoadSceneAsync("ComputerLab", LoadSceneMode.Single);
            } 
            catch
            {
                isLoading = false;
            }
        }
    }
}
