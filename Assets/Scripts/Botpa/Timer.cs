using UnityEngine;

namespace Botpa {
    
    public enum TimerScale { Scaled, Unscaled, Fixed }

    public class Timer {

        //Time management
        private readonly TimerScale scale = TimerScale.Scaled;
        private float curentTime => CurrentTime(scale);
        private float deltaTime => DeltaTime(scale);

        private float startTime = -1;
        private float elapsed => curentTime - startTime;

        private float _duration = -1;

        ///<summary>
        ///The duration of the count
        ///</summary>
        public float duration { get => _duration; private set => _duration = value; }

        ///<summary>
        ///If the timer is active (counting or finished).
        ///</summary>
        public bool isActive => duration != -1;

        ///<summary>
        ///The amout of time counted in this instant.
        ///</summary>
        public float counted => isActive ? Mathf.Clamp(elapsed, 0, duration) : 0;

        ///<summary>
        ///The percentage of progress of the timer count in the <b>[0, 1]</b> range.
        ///</summary>
        public float percent => isActive ? counted / duration : 0;

        ///<summary>
        ///If the timer is counting.
        ///</summary>
        public bool counting => isActive && elapsed < duration;

        ///<summary>
        ///If the timer started counting this frame.
        ///</summary>
        public bool startedThisFrame => isActive && startTime == curentTime;

        ///<summary>
        ///If the timer finished counting.
        ///</summary>
        public bool finished => isActive && elapsed >= duration;
        
        ///<summary>
        ///If the timer finished counting this frame.
        ///</summary>
        public bool finishedThisFrame => finished && duration > elapsed - deltaTime;


        //Constructors
        public Timer() : this(false, TimerScale.Scaled, -1) {}

        public Timer(bool startActive)      : this(startActive, TimerScale.Scaled, -1) {}
        public Timer(TimerScale timerScale) : this(false, timerScale, -1) {}
        public Timer(float duration)        : this(false, TimerScale.Scaled, duration) {}

        public Timer(bool startActive, TimerScale timerScale) : this(startActive, timerScale, -1) {}
        public Timer(bool startActive, float duration)        : this(startActive, TimerScale.Scaled, duration) {}
        public Timer(TimerScale timerScale, float duration)   : this(false, timerScale, duration) {}

        public Timer(bool startActive, TimerScale timerScale, float duration) {
            this.duration = startActive ? 0 : -1;
            scale = timerScale;
            if (duration != -1) Count(duration);
        }


        ///<summary>
        ///Starts counting the specified duration.
        ///</summary>
        public void Count(float duration) {
            //Start timer
            this.duration = duration;
            startTime = curentTime;
        }

        ///<summary>
        ///Extends the current count by the specified extension duration.
        ///<br/><br/>
        ///<b>NOTE:</b> If not counting, a new count will start.
        ///</summary>
        public void Extend(float duration) {
            //Add time to timer
            if (counting) {
                //Counting -> Extend duration
                this.duration += duration;
            } else {
                //Not counting -> Start count
                Count(duration);
            }
        }

        ///<summary>
        ///Resets the current count.
        ///</summary>
        public void Reset() {
            //Reset timer
            duration = -1;
        }

        
        ///<summary>
        ///Returns the value of current time based on TimerScale.
        ///</summary>
        public static float CurrentTime(TimerScale scale) {
            return scale switch { 
                TimerScale.Scaled => Time.time, 
                TimerScale.Unscaled => Time.unscaledTime, 
                TimerScale.Fixed => Time.fixedTime,
                _ => Time.time,
            };
        }

        ///<summary>
        ///Returns the value of delta time based on TimerScale.
        ///</summary>
        public static float DeltaTime(TimerScale scale) {
            return scale switch {
                TimerScale.Scaled => Time.deltaTime,
                TimerScale.Unscaled => Time.unscaledDeltaTime,
                TimerScale.Fixed => Time.fixedDeltaTime,
                _ => Time.deltaTime,
            };
        }
    }

}


