using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Planets
{
    public class CapturePointController : MonoBehaviour
    {
        private readonly List<PlanetType> _availableItems = new()
        {
            PlanetType.Jetpack,
            PlanetType.Stick,
            PlanetType.FreezeGun,
            PlanetType.RocketLauncher,
            PlanetType.SpeedIncrease,
        };
        
        private void Awake()
        {
            var capturePoints = FindObjectsOfType<CapturePoint>();
            var length = capturePoints.Length;

            if (length < _availableItems.Count)
            {
                throw new IndexOutOfRangeException("Not enough planets to fulfill items.");
            }

            while (_availableItems.Count > 1)
            {
                var thisPoint = capturePoints[Random.Range(0, length)];
                if (thisPoint.item != PlanetType.None) continue;

                thisPoint.item = _availableItems[0];
                _availableItems.RemoveAt(0);
            }
            //Debug.Log("Assigned all planets");
        }
    }
}