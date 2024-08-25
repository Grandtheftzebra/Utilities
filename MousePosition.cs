using System;
using UnityEngine;
using DesignPatterns;
using TurnBasedShooter.Input_Manager;

namespace UnityUtils
{
    public class MousePosition : PersistentSingleton<MousePosition> // TODO: Maybe delete singleton, as class can be made static
    {
        [SerializeField] private LayerMask _mouseLayer;

        public static Vector3 GetMousePosition
        {
            get
            {
                Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.MousePosition);
                Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, Instance._mouseLayer);

                return hit.point;
            }
        }
    }
}
