using UnityEngine;
using UnityEngine.SceneManagement;
using EscapeRoom.Inventory;

public class SwitchSceneManager : MonoBehaviour
{
    private bool isLoading = false;


    private void SwitchScene(string sceneName)
    {
        if (isLoading) return;

        isLoading = true;
        try
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        } 
        catch
        {
            isLoading = false;
        }
    }

    public void SwitchToLab() => SwitchScene("ComputerLab");
    public void SwitchToOutdoor() => SwitchScene("Outdoor");
    public void SwitchToMainRoom() => SwitchScene("MainRoom");
}
