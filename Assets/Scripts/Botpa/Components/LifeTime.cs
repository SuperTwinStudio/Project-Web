using UnityEngine;
using UnityEngine.Events;

namespace Botpa {

    public class LifeTime : MonoBehaviour {

        //Life time
        [Header("Life time")]
        [SerializeField, Min(0)] private float lifeTime = 0;
        [SerializeField] private UnityEvent onLifeEnd;

        private readonly Timer lifeTimer = new();


        //State
        private void Start() {
            //Check life time
            if (lifeTime <= 0) 
                //No life time -> End life
                EndLife();
            else
                //Wait for life to end
                lifeTimer.Count(lifeTime);
        }

        private void Update() {
            //Timer finished -> End life
            if (lifeTimer.IsFinished) EndLife();
        }

        //Helpers
        public void EndLife() {
            //Destroy object & call onLifeEnd event
            lifeTimer.Reset();
            Destroy(gameObject);
            onLifeEnd.Invoke();
        }

    }

}

