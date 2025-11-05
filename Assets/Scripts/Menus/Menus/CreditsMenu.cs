using Botpa;
using UnityEngine;
using UnityEngine.UI;

public class CreditsMenu : Menu {

    //Prefab
    public override string Name => MenusList.Credits;

    [Header("Components")]
    [SerializeField] private Selectable defaultSelectable;
    [SerializeField] private RectTransform area;
    [SerializeField] private RectTransform content;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject resumeButton;

    //Movement
    [Header("Movement")]
    [SerializeField] private float delayDuration = 5;
    [SerializeField] private float moveDuration = 3;

    private float translation, translationMin, translationMax;
    private bool movingUp, isScrolling = true;
    private readonly Timer delayTimer = new(TimerScale.Unscaled);


      /*$$$$$   /$$                 /$$
     /$$__  $$ | $$                | $$
    | $$  \__//$$$$$$    /$$$$$$  /$$$$$$    /$$$$$$
    |  $$$$$$|_  $$_/   |____  $$|_  $$_/   /$$__  $$
     \____  $$ | $$      /$$$$$$$  | $$    | $$$$$$$$
     /$$  \ $$ | $$ /$$ /$$__  $$  | $$ /$$| $$_____/
    |  $$$$$$/ |  $$$$/|  $$$$$$$  |  $$$$/|  $$$$$$$
     \______/   \___/   \_______/   \___/   \______*/

    public override bool OnBack() {
        return false; //Prevent close
    }

    public override void OnUpdate() {
        //Scrolling is paused | Credits are too small to move | Waiting to move
        if (!isScrolling || translationMax < 0 || delayTimer.counting) return;

        //Move credits
        translation = Mathf.Clamp(translation + Time.unscaledDeltaTime * (movingUp ? -1 : 1) * translationMax / moveDuration, translationMin, translationMax);
        UpdateScroll();

        //Check if finished moving
        if ((movingUp && translation <= translationMin) || (!movingUp && translation >= translationMax)) {
            //Finished -> Reverse direction & start move delay
            movingUp = !movingUp;
            translationMin = 0;
            delayTimer.Count(delayDuration);
        }
    }


      /*$$$$$              /$$     /$$
     /$$__  $$            | $$    |__/
    | $$  \ $$  /$$$$$$$ /$$$$$$   /$$  /$$$$$$  /$$$$$$$   /$$$$$$$
    | $$$$$$$$ /$$_____/|_  $$_/  | $$ /$$__  $$| $$__  $$ /$$_____/
    | $$__  $$| $$        | $$    | $$| $$  \ $$| $$  \ $$|  $$$$$$
    | $$  | $$| $$        | $$ /$$| $$| $$  | $$| $$  | $$ \____  $$
    | $$  | $$|  $$$$$$$  |  $$$$/| $$|  $$$$$$/| $$  | $$ /$$$$$$$/
    |__/  |__/ \_______/   \___/  |__/ \______/ |__/  |__/|______*/

    //Scrolling
    private void UpdateScroll() {
        var position = content.anchoredPosition;
        position.y = translation;
        content.anchoredPosition = position;
    }

    public void ToggleScroll() {
        //Toggle scrolling
        isScrolling = !isScrolling;

        //Toggle buttons
        pauseButton.SetActive(isScrolling);
        resumeButton.SetActive(!isScrolling);

        //Select button (for controller navigation)
        (isScrolling ? pauseButton : resumeButton).GetComponent<Selectable>().Select();
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

        //Select default button (for controller navigation)
        defaultSelectable.Select();

        //Init translations
        float contentHeight = Util.GetRealHeight(content);
        float areaHeight = Util.GetRealHeight(area);
        translation = -areaHeight;
        translationMin = translation;
        translationMax = contentHeight - areaHeight;
        movingUp = false;
        UpdateScroll();
    }

    protected override void OnClose() {
        base.OnClose();

        //Not playing
        if (!Application.isPlaying) return;
    }

}