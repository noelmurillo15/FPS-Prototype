/*
 * TimeManipulation - Used to change the timeScale immediately or progress towards a target
 * Created by : Allan N. Murillo
 * Last Edited : 3/26/2020
 */

using UnityEngine;
using System.Collections;

namespace ANM.Scriptable
{
    [CreateAssetMenu(menuName = "SingleInstance/TimeControl")]
    public class TimeManipulation : ScriptableObject
    {
        private float _currentTarget;
        private float _timeScaleToUse;


        private void OnEnable()
        {
            Reset();
        }

        private void OnDisable()
        {
            Reset();
        }

        public void Reset()
        {
            _currentTarget = 1;
            _timeScaleToUse = _currentTarget;
            Time.timeScale = _timeScaleToUse;
        }

        public float CurrentTarget()
        {
            return _currentTarget;
        }

        public float CurrentTimeScale()
        {
            return _currentTarget;
        }

        public IEnumerator IncrementTowardsTargetScale(float target, float time)
        {
            _currentTarget = target;
            while (!Mathf.Approximately(_timeScaleToUse, _currentTarget))
            {
                _timeScaleToUse = Mathf.MoveTowards(_timeScaleToUse, _currentTarget, Time.deltaTime / time);
                Time.timeScale = _timeScaleToUse;
                yield return null;
            }
        }
    }
}
