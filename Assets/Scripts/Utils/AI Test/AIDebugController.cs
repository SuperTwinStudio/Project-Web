using UnityEngine;
using UnityEngine.SceneManagement;

public class AIDebugController : MonoBehaviour
{
    private void Start()
    {
        Game.Current.Level.Player.Loadout.AddGold(120);
    }

    public void GoToRoom(int id)
    {
        SceneManager.LoadScene(id + 2);
    }
}
