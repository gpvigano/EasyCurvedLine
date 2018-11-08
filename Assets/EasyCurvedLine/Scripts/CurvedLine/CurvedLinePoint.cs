// Code from https://forum.unity.com/threads/easy-curved-line-renderer-free-utility.391219/

using UnityEngine;
using System.Collections;

namespace EasyCurvedLine
{
    public class CurvedLinePoint : MonoBehaviour
    {
        [HideInInspector] public bool showGizmo = true;
        [HideInInspector] public float gizmoSize = 0.1f;
        [HideInInspector] public Color gizmoColor = new Color(1, 0, 0, 0.5f);

        private void OnDrawGizmos()
        {
            if (showGizmo == true)
            {
                Gizmos.color = gizmoColor;

                Gizmos.DrawSphere(this.transform.position, gizmoSize);
            }
        }

        /// <summary>
        /// Update the parent line when this point is moved
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            CurvedLineRenderer curvedLine = this.transform.parent.GetComponent<CurvedLineRenderer>();

            if (curvedLine != null)
            {
                curvedLine.Update();
            }
        }
    }
}
