using System.Collections;
using System.Collections.Generic;
using BEST.BomberMan.Core;
using BEST.BomberMan.Management;
using BEST.BomberMan.ScriptableObjects;
using UnityEngine;

namespace BEST.BomberMan.Behaviours.Controllers
{
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyController : EntityController, IHittable
    {
        [SerializeField]
        private EnemyData enemyData;

        private LevelManager levelManager;

        private List<Node<ILevelBlock>> path;
        private Node<ILevelBlock> randomWalkableNode;

        private Vector3 currentTarget;
        private Rigidbody currentRigidbody;

        private int wayPointIndex = 0; 
        private PathFinder<ILevelBlock> pathFinder;

        private Color debuggerLineColor;
        private Vector3 previousPosition;

        private EnemiesManager enemiesManager;

        public Vector3 Location { get; set; }
        [field: SerializeField] public HittableType HittableType { get; set; }

        private void Start()
        {
            enemiesManager = FindObjectOfType<EnemiesManager>();
            debuggerLineColor = Random.ColorHSV();
            currentRigidbody = this.GetComponent<Rigidbody>();
            levelManager = FindObjectOfType<LevelManager>();
            pathFinder = new PathFinder<ILevelBlock>();
            randomWalkableNode = levelManager.GetRandomWalkableNode();
            path = new List<Node<ILevelBlock>>(
                pathFinder.FindPath(levelManager.WorldPositionToNode(this.transform.position), randomWalkableNode));

            currentTarget = levelManager.NodeToWorldPosition(path[wayPointIndex]);

            StartCoroutine(CheckForPosition());
        }

        private void Update()
        {
            this.transform.LookAt(currentTarget, Vector3.up);
            Location = this.transform.position;
        }

        private IEnumerator CheckForPosition()
        {
            while (true)
            {
                yield return new WaitForSeconds(2);
                var currentPosition = this.transform.position;

                if ((previousPosition - currentPosition).magnitude <= 0.3f)
                {
                    path = new List<Node<ILevelBlock>>(
                        pathFinder.FindPath(levelManager.WorldPositionToNode(previousPosition), levelManager.GetRandomWalkableNode()));
                    wayPointIndex = 0;
                    currentTarget = levelManager.NodeToWorldPosition(path[wayPointIndex]);

                    Debug.Log("Enemy remained idle - choosing another path");
                }
                
                previousPosition = currentPosition;
            }
        }
        
        private void FixedUpdate()
        {
            if (path.Count == 0)
                return;
            
            NavigatePath();
        }

        private void NavigatePath()
        {
            if (Vector3.Distance(this.transform.position, currentTarget) >= 0.2f)
            {
                Vector3 Loc = Vector3.MoveTowards(this.transform.position, currentTarget, enemyData.speed * Time.fixedDeltaTime);
                currentRigidbody.AddForce(enemyData.speed * (Loc - this.transform.position).normalized, ForceMode.VelocityChange);
                return;
            }
            
            wayPointIndex++;
            if (wayPointIndex == path.Count)
            {
                path = new List<Node<ILevelBlock>>(
                    pathFinder.FindPath(levelManager.WorldPositionToNode(this.transform.position), levelManager.GetRandomWalkableNode()));
                wayPointIndex = 0;
            }
            
            currentTarget = levelManager.NodeToWorldPosition(path[wayPointIndex]);
        }
        
        private void OnDrawGizmos()
        {
            // Draw the path
            if (path != null)
            {
                Gizmos.color = debuggerLineColor;
                for (int i = 0; i < path.Count - 1; i++)
                {
                    var startNode = new Vector3(path[i].X, 0.5f, path[i].Y);
                    var endNode = new Vector3(path[i + 1].X, 0.5f, path[i + 1].Y);
                    Gizmos.DrawLine(startNode, endNode);
                }
            }
        }

        private void OnDestroy()
        {
        #if UNITY_EDITOR
            if (!Application.isPlaying) return;
        #endif
            enemiesManager.EnemyKilled(this.gameObject);
            Debug.Log("Got destroyed!");
        }

        private void OnTriggerEnter(Collider other) 
        {
            if(other.gameObject.tag == "Player")
            {
                if(other.gameObject.TryGetComponent<IHittable>(out var component))
                {
                    component.RecieveHit();
                }
            }
        }

        public void RecieveHit()
        {
            enemiesManager.EnemyKilled(this.gameObject);
        }

    }
}