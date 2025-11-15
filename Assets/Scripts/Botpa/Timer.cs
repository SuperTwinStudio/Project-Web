using UnityEngine;

namespace Botpa {
    
    public enum TimerScale { Scaled, Unscaled, Fixed }

    public class Timer {

        //Time management
        private readonly TimerScale scale = TimerScale.Scaled;
        private float CurentTime => GetCurrentTime(scale);
        private float DeltaTime => GetDeltaTime(scale);

        private float startTime = 0;
        private float ElapsedTime => CurentTime - startTime;

        private float _duration = 0;

        ///<summary>
        ///The duration of the count
        ///</summary>
        public float Duration { get => _duration; private set => _duration = value; }

        ///<summary>
        ///If the timer is active.
        ///</summary>
        public bool IsActive => Duration > 0;

        ///<summary>
        ///The amout of time counted in this instant.
        ///</summary>
        public float Counted => IsActive ? Mathf.Clamp(ElapsedTime, 0, Duration) : 0;

        ///<summary>
        ///The percentage of progress of the timer count in <b>[0, 1]</b> range.
        ///</summary>
        public float Percent => IsActive ? Counted / Duration : 1;

        ///<summary>
        ///If the timer is counting.
        ///</summary>
        public bool IsCounting => IsActive && ElapsedTime < Duration;

        ///<summary>
        ///If the timer started counting this frame.
        ///</summary>
        public bool StartedThisFrame => IsActive && startTime == CurentTime;

        ///<summary>
        ///If the timer finished counting.
        ///</summary>
        public bool IsFinished => ElapsedTime >= Duration;

        ///<summary>
        ///If the timer finished counting this frame.
        ///</summary>
        public bool FinishedThisFrame => IsFinished && Duration > ElapsedTime - DeltaTime;


        //Constructors
        public Timer(float duration) : this(TimerScale.Scaled, duration) {}
        public Timer(TimerScale timerScale = TimerScale.Scaled, float duration = 0) {
            //Save scale
            scale = timerScale;

            //Start counting
            Count(duration);
        }

        ///<summary>
        ///Starts counting the specified duration.
        ///</summary>
        public bool Count(float duration) {
            //Invalid duration
            if (duration <= 0) return false;

            //Start timer
            startTime = CurentTime;
            Duration = duration;
            return true;
        }

        ///<summary>
        ///Extends the current count by the specified extension duration.
        ///<br/><br/>
        ///<b>NOTE:</b> If not counting, a new count will start.
        ///</summary>
        public bool Extend(float duration) {
            //Add time to timer
            if (IsCounting) {
                //Counting -> Extend duration
                Duration += duration;
                return true;
            } else {
                //Not counting -> Start count
                return Count(duration);
            }
        }

        ///<summary>
        ///Resets the current count.
        ///</summary>
        public void Reset() {
            //Reset timer
            Duration = 0;
        }

        ///<summary>
        ///Returns the value of current time based on TimerScale.
        ///</summary>
        public static float GetCurrentTime(TimerScale scale) {
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
        public static float GetDeltaTime(TimerScale scale) {
            return scale switch {
                TimerScale.Scaled => Time.deltaTime,
                TimerScale.Unscaled => Time.unscaledDeltaTime,
                TimerScale.Fixed => Time.fixedDeltaTime,
                _ => Time.deltaTime,
            };
        }

    }

}


