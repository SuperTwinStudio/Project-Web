using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialMenu : Menu {
    
    //Prefab
    public override string Name => MenusList.Tutorial;

    //Components
    [Header("Components")]
    [SerializeField] private Selectable defaultSelectable;
    [SerializeField] private GameObject backgroundHome;
    [SerializeField] private GameObject backgroundGame;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;

    //Tutorials
    [Header("Tutorials")]
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private VideoPlayer tutorialVideo;
    [SerializeField] private List<TutorialItem> tutorials = new();

    private int tutorialIndex = 0;


      /*$$$$$              /$$     /$$
     /$$__  $$            | $$    |__/
    | $$  \ $$  /$$$$$$$ /$$$$$$   /$$  /$$$$$$  /$$$$$$$   /$$$$$$$
    | $$$$$$$$ /$$_____/|_  $$_/  | $$ /$$__  $$| $$__  $$ /$$_____/
    | $$__  $$| $$        | $$    | $$| $$  \ $$| $$  \ $$|  $$$$$$
    | $$  | $$| $$        | $$ /$$| $$| $$  | $$| $$  | $$ \____  $$
    | $$  | $$|  $$$$$$$  |  $$$$/| $$|  $$$$$$/| $$  | $$ /$$$$$$$/
    |__/  |__/ \_______/   \___/  |__/ \______/ |__/  |__/|______*/

    //Navigation
    private void SelectTutorial(int index) {
        //Out of bounds -> Close tutorial
        if (index >= tutorials.Count) {
            CloseFromUI();
            return;
        }

        //Select tutorial
        TutorialItem tutorial = tutorials[index];
        tutorialIndex = index;
        tutorialText.SetText(tutorial.Name);
        tutorialVideo.clip = tutorial.Video;
        tutorialVideo.Play();

        //Toggle buttons
        previousButton.interactable = index > 0;
        nextButton.interactable = index < tutorials.Count - 1;

        //Nothing selected -> Select default
        if (!EventSystem.current.currentSelectedGameObject) defaultSelectable.Select();
    }

    public void PreviousTutorial() {
        SelectTutorial(tutorialIndex - 1);
    }

    public void NextTutorial() {
        SelectTutorial(tutorialIndex + 1);
    }


     /*$$$$$$$                            /$$
    |__  $$__/                           | $$
       | $$  /$$$$$$   /$$$$$$   /$$$$$$ | $$  /$$$$$$
       | $$ /$$__  $$ /$$__  $$ /$$__  $$| $$ /$$__  $$
       | $$| $$  \ $$| $$  \ $$| $$  \ $$| $$| $$$$$$$$
       | $$| $$  | $$| $$  | $$| $$  | $$| $$| $$_____/
       | $$|  $$$$$$/|  $$$$$$$|  $$$$$$$| $$|  $$$$$$$
       |__/ \______/  \____  $$ \____  $$|__/ \_______/
                      /$$  \ $$ /$$  \ $$
                     |  $$$$$$/|  $$$$$$/
                      \______/  \_____*/

    protected override void OnOpen(object args = null) {
        base.OnOpen();

        //Not playing
        if (!Application.isPlaying) return;

        //Select tutorial
        SelectTutorial(0);

        //Get scene name
        string scene = SceneManager.GetActiveScene().name;

        //Toggle backgrounds
        backgroundHome.SetActive(scene == "Home");
        backgroundGame.SetActive(scene != "Home");

        //Select default button (for controller navigation)
        defaultSelectable.Select();

        //Pause game
        Game.Pause(this);
    }

    protected override void OnClose() {
        base.OnClose();

        //Not playing
        if (!Application.isPlaying) return;

        //Unpause game
        Game.Unpause(this);
    }

}

[Serializable]
public class TutorialItem {
    
    [SerializeField] private LocalizedString _name;
    [SerializeField] private VideoClip _video;
    
    public string Name => _name.GetLocalizedString();
    public VideoClip Video => _video;

}