using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour {
    
    //Components
    [Header("Components")]
    [SerializeField] private AudioSource musicSource;

    //Music
    [Header("Music")]
    [SerializeField] private AudioClip musicLobby;
    [SerializeField] private AudioClip musicDungeon;

    private readonly HashSet<string> lobbyScenes = new() { "Home", "Lobby" };
    private Coroutine changeMusicCoroutine = null;


    //State
    public void NotifyNewScene() {
        //Get scene name
        string sceneName = SceneManager.GetActiveScene().name;

        //Toggle music
        AudioClip musicClip = lobbyScenes.Contains(sceneName) ? musicLobby : musicDungeon;
        if (musicSource.clip != musicClip) {
            //Check clip
            if (musicSource.clip == null) {
                //No clip -> Start automatically
                musicSource.clip = musicClip;
                musicSource.Play();
            } else {
                //Change song
                if (changeMusicCoroutine != null) StopCoroutine(changeMusicCoroutine);
                changeMusicCoroutine = StartCoroutine(ToggleMusicCoroutine(musicClip, 0));
            }
        }
    }

    private IEnumerator ToggleMusicCoroutine(AudioClip musicClip, float target) {
        //Update volume
        musicSource.volume = Mathf.MoveTowards(musicSource.volume, target, Time.deltaTime);

        //Wait for next frame
        yield return new WaitForNextFrameUnit();

        //Check state
        if (musicSource.volume != target) {
            //Continue
            changeMusicCoroutine = StartCoroutine(ToggleMusicCoroutine(musicClip, target));
        } else if (target == 0) {
            //Finished lowering volume
            musicSource.clip = musicClip;
            musicSource.Play();
            changeMusicCoroutine = StartCoroutine(ToggleMusicCoroutine(musicClip, 1));
        }
    }

}
