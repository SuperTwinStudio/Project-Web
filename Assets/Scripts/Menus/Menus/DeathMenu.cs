using System.Collections;
using Botpa;
using UnityEngine;
using UnityEngine.UI;

public class DeathMenu : Menu {

    //Prefab
    public override string Name => MenusList.Death;

    //Components
    [Header("Components")]
    [SerializeField] private Image transition;
    [SerializeField] private Animator animator;

    private readonly Timer transitionTimer = new(TimerScale.Unscaled);

    private const float TRANSITION_DURATION = 2f;


      /*$$$$$   /$$                 /$$
     /$$__  $$ | $$                | $$
    | $$  \__//$$$$$$    /$$$$$$  /$$$$$$    /$$$$$$
    |  $$$$$$|_  $$_/   |____  $$|_  $$_/   /$$__  $$
     \____  $$ | $$      /$$$$$$$  | $$    | $$$$$$$$
     /$$  \ $$ | $$ /$$ /$$__  $$  | $$ /$$| $$_____/
    |  $$$$$$/ |  $$$$/|  $$$$$$$  |  $$$$/|  $$$$$$$
     \______/   \___/   \_______/   \___/   \______*/

    public override void OnUpdate() {
        //Animation
        if (transitionTimer.isActive) {
            //Get animation percentage
            float percent = Ease.InCubic(transitionTimer.percent);

            //Update transition
            transition.material.SetFloat("_Percent", percent);

            //Check if animation finished
            if (transitionTimer.finished) transitionTimer.Reset();
        }
    }

    public override bool OnBack() {
        return false; //Prevent close
    }


      /*$$$$$              /$$     /$$
     /$$__  $$            | $$    |__/
    | $$  \ $$  /$$$$$$$ /$$$$$$   /$$  /$$$$$$  /$$$$$$$   /$$$$$$$
    | $$$$$$$$ /$$_____/|_  $$_/  | $$ /$$__  $$| $$__  $$ /$$_____/
    | $$__  $$| $$        | $$    | $$| $$  \ $$| $$  \ $$|  $$$$$$
    | $$  | $$| $$        | $$ /$$| $$| $$  | $$| $$  | $$ \____  $$
    | $$  | $$|  $$$$$$$  |  $$$$/| $$|  $$$$$$/| $$  | $$ /$$$$$$$/
    |__/  |__/ \_______/   \___/  |__/ \______/ |__/  |__/|______*/

    private IEnumerator ShowMenu() {
        yield return null;
        /*
        //Reset
        transition.material.SetFloat("_Percent", 0);
        animator.gameObject.SetActive(false);

        //Wait a bit
        yield return new WaitForSecondsRealtime(1f);

        //Start death animation
        transitionTimer.Count(TRANSITION_DURATION);
        yield return new WaitForSecondsRealtime(TRANSITION_DURATION);

        //Show text
        Game.UnhideCursor(this);
        animator.SetTrigger("Show");
        animator.gameObject.SetActive(true);
        */
    }

    public void Quit() {
        Util.Quit();
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

        //Pause game & hide cursor
        Game.Pause(this);
        Game.HideCursor(this);

        //Show menu
        StartCoroutine(ShowMenu());
    }

    protected override void OnClose() {
        base.OnClose();

        //Unpause game
        Game.Unpause(this);
    }

}