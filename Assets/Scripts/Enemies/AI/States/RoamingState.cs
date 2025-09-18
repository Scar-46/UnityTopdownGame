using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class RoamingState : State
{
    [SerializeField] private TargetDetector targetDetector;
    [SerializeField] private AIData aiData;

    private NavMeshAgent agent;
    private Animator animator;

    // States
    [SerializeField] private ChaseState chaseState;

    // Tiles
    public Vector3Int centerCell;
    public int radius = 5;
    public Tilemap tilemap;
    private List<Vector3> tiles = new List<Vector3>();

    void Awake() { 
        agent = GetComponent<NavMeshAgent>(); 
        agent.updateRotation = false; 
        agent.updateUpAxis = false; 
        animator = GetComponent<Animator>(); }

    public override void OnEnter()
    {
        _isFacingRight = transform.localScale.x > 0;
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
        agent.updateUpAxis = false;
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        agent.stoppingDistance = 0.5f;
        animator.SetFloat("Speed", 0f);
    }

    public override void OnExit()
    {
        // Optional cleanup when leaving roaming
        agent.ResetPath();
        animator.SetFloat("Speed", 0f);
    }

    public override State RunState()
    {
        State nextState = this;

        // Check for target first
        if (targetDetector.Detect(aiData))
        {
            return chaseState;
        }

        // Roaming logic
        if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
        {
            GetTilePosition();

            if (tiles.Count > 0)
            {
                Vector2 target = tiles[Random.Range(0, tiles.Count)];
                Flip(target);
                agent.SetDestination(target);
                animator.SetFloat("Speed", 0.06f);
            }
        }
        else
        {
            animator.SetFloat("Speed", 0.4f);
        }

        return nextState;
    }

    private void GetTilePosition()
    {
        tiles.Clear(); // avoid duplicate accumulation
        Vector3 objectPosition = transform.position;
        centerCell = tilemap.WorldToCell(objectPosition);
        GetTilesInRadius(centerCell, radius);
    }

    private void GetTilesInRadius(Vector3Int center, int radius)
    {
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                Vector3Int currentCell = center + new Vector3Int(x, y, 0);
                if (IsWithinRadius(center, currentCell, radius))
                {
                    TileBase tile = tilemap.GetTile(currentCell);
                    if (tile != null)
                    {
                        tiles.Add(tilemap.CellToWorld(currentCell));
                    }
                }
            }
        }
    }

    private bool IsWithinRadius(Vector3Int center, Vector3Int currentCell, int radius)
    {
        return Vector3Int.Distance(center, currentCell) <= radius;
    }
}
