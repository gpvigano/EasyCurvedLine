#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EasyCurvedLine
{
    public class CurvedLineMenu : MonoBehaviour
    {
        [MenuItem("GameObject/Effects/Curved Line")]
        private static void NewCurvedLineCommand(MenuCommand menuCommand)
        {
            GameObject newObj = new GameObject("CurvedLine");
            Undo.RegisterCreatedObjectUndo(newObj, "New curved line");
            GameObject selectedObject = Selection.activeGameObject;
            Material parentLineMaterial = null;
            if (selectedObject != null && selectedObject.GetComponent<CurvedLineRenderer>() == null)
            {
                newObj.transform.SetParent(selectedObject.transform,false);
                CurvedLinePoint parentLinePoint = selectedObject.GetComponent<CurvedLinePoint>();
                if (parentLinePoint != null)
                {
                    CurvedLineRenderer parentCurvedLine = parentLinePoint.GetComponentInParent<CurvedLineRenderer>();
                    if(parentCurvedLine!=null)
                    {
                        parentLineMaterial = parentCurvedLine.GetComponent<LineRenderer>().sharedMaterial;
                    }
                }
            }
            CurvedLineRenderer curvedLine = newObj.AddComponent<CurvedLineRenderer>();
            Selection.activeGameObject = newObj;
            UnityEditorInternal.ComponentUtility.MoveComponentUp(curvedLine);
            AddControlPoint(curvedLine);
            AddControlPoint(curvedLine);
            if(parentLineMaterial!=null)
            {
                newObj.GetComponent<LineRenderer>().sharedMaterial = parentLineMaterial;
            }
            curvedLine.Update();
        }


        [MenuItem("CONTEXT/CurvedLineRenderer/Add Control Point")]
        private static void AddLineControlPointCommand(MenuCommand menuCommand)
        {
            CurvedLineRenderer curvedLine = menuCommand.context as CurvedLineRenderer;
            CurvedLinePoint newLinePoint = AddControlPoint(curvedLine);
            Selection.activeGameObject = newLinePoint.gameObject;
        }


        // Validate the menu item defined by the function above.
        [MenuItem("CONTEXT/CurvedLineRenderer/Add Control Point", true)]
        private static bool ValidateAddLineControlPointCommand()
        {
            return ValidCurvedLineRendererSelected();
        }


        [MenuItem("CONTEXT/CurvedLineRenderer/Rename Control Points")]
        private static void RenameControlPointsCommand(MenuCommand menuCommand)
        {
            CurvedLineRenderer curvedLine = menuCommand.context as CurvedLineRenderer;
            RenameControlPoints(curvedLine.LinePoints);
        }


        // Validate the menu item defined by the function above.
        [MenuItem("CONTEXT/CurvedLineRenderer/Rename Control Points", true)]
        private static bool ValidateRenameControlPointsCommand()
        {
            return ValidCurvedLineRendererSelected();
        }


        [MenuItem("CONTEXT/CurvedLinePoint/Insert Control Point Before")]
        private static void InsertControlPointCommand(MenuCommand menuCommand)
        {
            CurvedLinePoint linePoint = menuCommand.context as CurvedLinePoint;
            CurvedLinePoint newLinePoint = InsertControlPoint(linePoint,true);
            Selection.activeGameObject = newLinePoint.gameObject;
        }


        [MenuItem("CONTEXT/CurvedLinePoint/Insert Control Point Before",true)]
        private static bool ValidateInsertControlPointCommand(MenuCommand menuCommand)
        {
            CurvedLinePoint linePoint = menuCommand.context as CurvedLinePoint;
            CurvedLineRenderer curvedLine = linePoint.GetComponentInParent<CurvedLineRenderer>();
            return curvedLine.LinePoints.Length>1;
        }


        [MenuItem("CONTEXT/CurvedLinePoint/Insert Control Point After")]
        private static void AddControlPointCommand(MenuCommand menuCommand)
        {
            CurvedLinePoint linePoint = menuCommand.context as CurvedLinePoint;
            CurvedLinePoint newLinePoint = InsertControlPoint(linePoint,false);
            Selection.activeGameObject = newLinePoint.gameObject;
        }


        private static bool ValidCurvedLineRendererSelected()
        {
            GameObject selectedObject = Selection.activeGameObject;
            return selectedObject != null && selectedObject.GetComponent<CurvedLineRenderer>() != null;
        }


        private static CurvedLinePoint InsertControlPoint(CurvedLinePoint linePoint, bool before)
        {
            CurvedLineRenderer curvedLine = linePoint.GetComponentInParent<CurvedLineRenderer>();
            CurvedLinePoint newLinePoint = AddControlPoint(curvedLine);
            List<CurvedLinePoint> linePointList = new List<CurvedLinePoint>(curvedLine.LinePoints);
            int idx = linePointList.IndexOf(linePoint);
            int siblingIndex = linePoint.transform.GetSiblingIndex();
            if (before)
            {
                CurvedLinePoint prevLinePoint = curvedLine.LinePoints[idx-1];
                newLinePoint.transform.SetSiblingIndex(siblingIndex);
                linePoint.transform.SetSiblingIndex(siblingIndex + 1);
                // set the new point in the middle between the current and the prervious point
                newLinePoint.transform.position = (prevLinePoint.transform.position + linePoint.transform.position) * 0.5f;
            }
            else
            {
                if(curvedLine.LinePoints.Length>idx+1)
                {
                    CurvedLinePoint nextLinePoint = curvedLine.LinePoints[idx+1];
                    newLinePoint.transform.SetSiblingIndex(siblingIndex + 1);
                    // set the new point in the middle between the current and the next point
                    newLinePoint.transform.position = (nextLinePoint.transform.position + linePoint.transform.position) * 0.5f;
                }
            }
            curvedLine.Update();
            RenameControlPoints(curvedLine.LinePoints);

            return newLinePoint;
        }


        private static CurvedLinePoint AddControlPoint(CurvedLineRenderer curvedLine, bool insertBefore = false)
        {
            CurvedLinePoint[] linePoints = curvedLine.LinePoints;
            int pointsCount = linePoints == null ? 0 : linePoints.Length;
            GameObject newObj;
            CurvedLinePoint newLinePoint;
            if (pointsCount == 0)
            {
                newObj = new GameObject("LinePoint");
                newLinePoint = newObj.AddComponent<CurvedLinePoint>();
            }
            else
            {
                newObj = GameObject.Instantiate(linePoints[0].gameObject);
                newLinePoint = newObj.GetComponent<CurvedLinePoint>();
            }
            newObj.name = "LinePoint" + (pointsCount + 1);
            newObj.transform.SetParent(curvedLine.transform, false);
            Undo.RegisterCreatedObjectUndo(newObj, "Add control point");
            if (pointsCount > 0)
            {
                if (pointsCount < 2)
                {
                    newObj.transform.position = linePoints[0].transform.position + Vector3.right;
                }
                else
                {
                    Vector3 offset = linePoints[pointsCount - 1].transform.position - linePoints[pointsCount - 2].transform.position;
                    newObj.transform.position = linePoints[pointsCount - 1].transform.position + offset;
                }
            }
            curvedLine.Update();
            RenameControlPoints(curvedLine.LinePoints);
            return newLinePoint;
        }


        private static void RenameControlPoints(CurvedLinePoint[] linePoints)
        {
            for(int i=0;i<linePoints.Length;i++)
            {
                linePoints[i].gameObject.name = "LinePoint" + (i + 1);
            }
        }

    }
}
#endif
