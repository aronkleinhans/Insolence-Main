using UnityEngine;
using Insolence.SaveUtility;
using OccaSoftware.Altos.Runtime;

namespace Insolence.Core
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance { get; private set; }
        
        [Header("Time settings")]
        // Custom timescale variable, 1 = real-time
        [SerializeField] private int timeScale = 10;

        // Time of day in hours
        [SerializeField] private float timeOfDay = 0;

        // current date in an int array
        [SerializeField] private int[] date = new int[3];

        // Day-night cycle duration in hours
        [SerializeField] private float dayNightCycleDuration = 24;

        // Length of a month in days
        [SerializeField] private float monthLength = 30;

        // Length of a year in months
        [SerializeField] private int yearLength = 12;

        // Current season
        [SerializeField] private int currentSeason = 0;

        [SerializeField] SkyDefinition skyDefinition;
        [SerializeField] float currentAltosTime;

        GameManager gm;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {

            //set date to 100/1/1
            date[0] = 1;
            date[1] = 1;
            date[2] = 100;
        }
        // Update the time
        private void Update()
        {
            //calculate time of day
            timeOfDay += timeScale * Time.deltaTime / 3600;

            //get references to GameManager and Altos SkyDirector after the scene loaded
            if (gm == null)
            {
                gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            }

            if (skyDefinition == null && gm.sceneReady)
            {
                skyDefinition = GameObject.Find("Sky Director").GetComponent<AltosSkyDirector>().skyDefinition;
            }

            // Reset timeOfDay to 0 when it exceeds dayNightCycleDuration
            if (timeOfDay >= dayNightCycleDuration)
            {
                timeOfDay = 0;

                // calculate current date
                date[0] += 1;
                if (date[0] > monthLength)
                {
                    date[0] = 1;
                    date[1] += 1;
                    if (date[1] > yearLength)
                    {
                        date[1] = 1;
                        date[2] += 1;
                    }
                }
            }
            // Update the current season
            currentSeason = date[1] / (yearLength / 4);


            if (skyDefinition != null && gm.sceneReady)
            {
                // set the time of day in the sky definition of Altos
                skyDefinition.SetDayAndTime(date[0], timeOfDay);
                currentAltosTime = skyDefinition.CurrentTime;
            }

        }
        /// <summary>
        /// Get the current time of day in hours
        /// </summary>
        /// <returns> float timeOfDay</returns>
        public float GetTimeOfDay()
        {
            return timeOfDay;
        }
        /// <summary>
        /// Get the current date in an int array
        /// </summary>
        /// <returns>int[days, months, years]</returns>
        public int[] GetDate()
        {
            return date;
        }
        /// <summary>
        /// Get the current season
        /// </summary>
        /// <returns>int currentSeason</returns>
        public int GetSeason()
        {
            return currentSeason;
        }
        /// <summary>
        /// Get the current timescale
        /// </summary>
        /// <returns></returns>
        public int GetTimeScale()
        {
            return timeScale;
        }
        /// <summary>
        /// Set the current time of day in hours
        /// </summary>
        /// <param name="timeOfDay"></param>
        public void SetTimeOfDay(float timeOfDay)
        {
            this.timeOfDay = timeOfDay;
        }
        /// <summary>
        /// Set the current date in an int array
        /// </summary>
        /// <param name="date"></param>
        public void SetDate(int[] date)
        {
            this.date = date;
        }
        /// <summary>
        /// Set the current season (0-3)
        /// </summary>
        /// <param name="season"></param>
        public void SetSeason(int season)
        {
            this.currentSeason = season;
        }
        /// <summary>
        /// set the current timescale
        /// </summary>
        /// <param name="timeScale"></param>
        public void SetTimeScale(int timeScale)
        {
            this.timeScale = timeScale;
        }
    }
}
