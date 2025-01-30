using System.Collections;
using System.Collections.Generic;
using BEST.BomberMan.Core;
using BEST.BomberMan.Management;
using UnityEngine;

namespace BEST.BomberMan.Behaviours.Debuggers
{
    public class DebugPathfinding : MonoBehaviour
    {
        [SerializeField]
        private LevelManager levelManager;

        private Vector3 endWorldPosition;
        private Camera currentCamera;

        private void Start() => currentCamera = Camera.main;

        private List<Node<ILevelBlock>> path;

        private Vector3 start;
        private Vector3 end;
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mousePos = Input.mousePosition;
                mousePos.z = currentCamera.transform.position.y;
                start = currentCamera.ScreenToWorldPoint(mousePos);
            }

            if (Input.GetMouseButtonDown(1))
            {
                var pathFinder = new PathFinder<ILevelBlock>();
                
                var end = Input.mousePosition;
                end.z = currentCamera.transform.position.y;
                endWorldPosition = currentCamera.ScreenToWorldPoint(end);
                
                path = pathFinder.FindPath(levelManager.WorldPositionToNode(start),
                    levelManager.WorldPositionToNode(endWorldPosition));
                
            }

            if (Input.GetMouseButtonDown(2))
            {
                var end = Input.mousePosition;
                end.z = currentCamera.transform.position.y;
                endWorldPosition = currentCamera.ScreenToWorldPoint(end);
                
                levelManager.MakeNodeNotWalkable(endWorldPosition);
            }
        }

        private void OnDrawGizmos()
        {
            // Draw the path
            if (path != null)
            {
                Gizmos.color = Color.red;
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Vector3 startNode = new Vector3(path[i].X, 0.5f, path[i].Y);
                    Vector3 endNode = new Vector3(path[i + 1].X, 0.5f, path[i + 1].Y);
                    Gizmos.DrawLine(startNode, endNode);
                }
            }
        }
    }
}