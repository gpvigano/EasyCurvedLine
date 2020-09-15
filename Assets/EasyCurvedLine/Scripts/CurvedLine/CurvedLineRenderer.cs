// Code from https://forum.unity.com/threads/easy-curved-line-renderer-free-utility.391219/
// and https://github.com/gpvigano/EasyCurvedLine

using System.Collections.Generic;
using UnityEngine;

namespace EasyCurvedLine
{
    /// <summary>
    /// Render in 3D a curved line based on its control points.
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    public class CurvedLineRenderer : MonoBehaviour
    {
        /// <summary>
        /// Size of line segments (in meters) used to approximate the curve.
        /// </summary>
        [Tooltip("Size of line segments (in meters) used to approximate the curve")]
        public float lineSegmentSize = 0.15f;

        /// <summary>
        /// Thickness of the line (initial thickness if useCustomEndWidth is true).
        /// </summary>
        [Tooltip("Width of the line (initial width if useCustomEndWidth is true)")]
        public float lineWidth = 0.1f;

        /// <summary>
        /// Use a different thickness for the line end.
        /// </summary>
        [Tooltip("Enable this to set a custom width for the line end")]
        public bool useCustomEndWidth = false;

        /// <summary>
        /// Thickness of the line at its end point (initial thickness is lineWidth).
        /// </summary>
        [Tooltip("Custom width for the line end")]
        public float endWidth = 0.1f;
        [Header("Gizmos")]

        /// <summary>
        /// Show gizmos at control points in Unity Editor.
        /// </summary>
        [Tooltip("Show gizmos at control points.")]
        public bool showGizmos = true;

        /// <summary>
        /// Size of the gizmos of control points.
        /// </summary>
        [Tooltip("Size of the gizmos of control points.")]
        public float gizmoSize = 0.1f;

        /// <summary>
        /// Color for rendering the gizmos of control points.
        /// </summary>
        [Tooltip("Color for rendering the gizmos of control points.")]
        public Color gizmoColor = new Color(1, 0, 0, 0.5f);

        /// <summary>
        /// Automatically update the line.
        /// </summary>
        [Tooltip("Automatically update the line.")]
        public bool autoUpdate = true;

        private CurvedLinePoint[] linePoints = new CurvedLinePoint[0];
        private Vector3[] linePositions = new Vector3[0];
        private Vector3[] linePositionsOld = new Vector3[0];
        private LineRenderer lineRenderer = null;
        private Material lineRendererMaterial = null;

        public CurvedLinePoint[] LinePoints
        {
            get
            {
                return linePoints;
            }
        }

        /// <summary>
        /// Collect control points positions and update the line renderer.
        /// </summary>
        public void UpdateLineRenderer()
        {
            GetPoints();
            SetPointsToLine();
            UpdateMaterial();
        }


        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        /// <summary>
        /// Collect control points positions and update the line renderer.
        /// </summary>
        public void Update()
        {
            if (autoUpdate)
            {
                UpdateLineRenderer();
            }
        }

        private void GetPoints()
        {
            // find curved points in children
            // scan only the first hierarchy level to allow nested curved lines (like modelling a tree or a coral)
            List<CurvedLinePoint> curvedLinePoints = new List<CurvedLinePoint>();
            for (int i = 0; i < transform.childCount; i++)
            {
                CurvedLinePoint childPoint = transform.GetChild(i).GetComponent<CurvedLinePoint>();
                if (childPoint != null)
                {
                    curvedLinePoints.Add(childPoint);
                }
            }
            linePoints = curvedLinePoints.ToArray();

            //add positions
            if (linePositions.Length != linePoints.Length)
            {
                linePositions = new Vector3[linePoints.Length];
            }
            for (int i = 0; i < linePoints.Length; i++)
            {
                linePositions[i] = linePoints[i].transform.position;
            }
        }


        private void SetPointsToLine()
        {
            bool rebuild = false;

            // create old positions if they don't match
            if (linePositionsOld.Length != linePositions.Length)
            {
                linePositionsOld = new Vector3[linePositions.Length];
                rebuild = true;
            }
            else
            {
                // check if line points have moved
                for (int i = 0; i < linePositions.Length; i++)
                {
                    //compare
                    if (linePositions[i] != linePositionsOld[i])
                    {
                        rebuild = true;
                        break;
                    }
                }
            }

            // update if line points were modified
            if (rebuild)
            {
                linePositions.CopyTo(linePositionsOld, 0);
                if (lineRenderer == null)
                {
                    lineRenderer = GetComponent<LineRenderer>();
                }
                // get smoothed values
                Vector3[] smoothedPoints = LineSmoother.SmoothLine(linePositions, lineSegmentSize);

                // set line settings
                lineRenderer.positionCount = smoothedPoints.Length;
                lineRenderer.SetPositions(smoothedPoints);
                lineRenderer.startWidth = lineWidth;
                lineRenderer.endWidth = useCustomEndWidth ? endWidth : lineWidth;
            }
        }


        private void OnDrawGizmosSelected()
        {
            UpdateLineRenderer();
        }


        private void OnDrawGizmos()
        {
            if (linePoints.Length == 0)
            {
                GetPoints();
            }

            // settings for gizmos
            foreach (CurvedLinePoint linePoint in linePoints)
            {
                linePoint.showGizmo = showGizmos;
                linePoint.gizmoSize = gizmoSize;
                linePoint.gizmoColor = gizmoColor;
            }
        }

        private void UpdateMaterial()
        {
            if (lineRenderer == null)
            {
                lineRenderer = GetComponent<LineRenderer>();
            }
            Material lineMaterial = lineRenderer.sharedMaterial;
            if (lineRendererMaterial != lineMaterial)
            {
                if (lineMaterial != null)
                {
                    lineRenderer.generateLightingData = !lineMaterial.shader.name.StartsWith("Unlit");
                }
                else
                {
                    lineRenderer.generateLightingData = false;
                }
            }
            lineRendererMaterial = lineMaterial;
        }
    }
}
