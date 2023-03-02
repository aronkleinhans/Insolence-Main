using UnityEngine;
using OccaSoftware.Altos.Runtime;

namespace OccaSoftware.Altos.Demo
{
    public class TimeOfDayCallbacksDemo : MonoBehaviour
    {
		AltosSkyDirector skyDirector;
		private void OnEnable()
		{
			skyDirector = FindObjectOfType<AltosSkyDirector>();
			skyDirector.skyDefinition.OnDayChanged += OnDayChanged;
			skyDirector.skyDefinition.OnHourChanged += OnHourChanged;
			skyDirector.skyDefinition.OnPeriodChanged += OnPeriodChanged;
		}

		void OnDayChanged()
		{
			Debug.Log("The current day has changed.");
		}


		void OnHourChanged()
		{
			Debug.Log("The current hour has changed.");
		}

		void OnPeriodChanged()
		{
			Debug.Log("The current period of day has changed.");
		}

		private void OnDisable()
		{
			skyDirector.skyDefinition.OnDayChanged -= OnDayChanged;
			skyDirector.skyDefinition.OnHourChanged -= OnHourChanged;
			skyDirector.skyDefinition.OnPeriodChanged -= OnPeriodChanged;
		}
	}
}
