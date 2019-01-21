// Code from https://forum.unity.com/threads/easy-curved-line-renderer-free-utility.391219/
// and https://github.com/gpvigano/EasyCurvedLine

using UnityEngine;

namespace EasyCurvedLine
{
    /// <summary>
    /// Curved line point class, holding also gizmo parameters for rendering in Unity Editor.
    /// </summary>
    public class CurvedLinePoint : MonoBehaviour
    {
        /// <summary>
        /// Render the sphere gizmo in Unity Editor.
        /// </summary>
        [HideInInspector] public bool showGizmo = true;
        /// <summary>
        /// Radius of the sphere gizmo rendered in Unity Editor.
        /// </summary>
        [HideInInspector] public float gizmoSize = 0.1f;
        /// <summary>
        /// Color of the sphere gizmo rendered in Unity Editor.
        /// </summary>
        [HideInInspector] public Color gizmoColor = new Color(1, 0, 0, 0.5f);


        /// <summary>
        /// If gizmo is shown render it with the given color and radius.
        /// </summary>
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
