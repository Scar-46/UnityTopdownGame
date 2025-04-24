using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class RoamingState : State
{
    [SerializeField]
    Transform target;

    [SerializeField]
    TargetDetector targetDetector;

    [SerializeField]
    AIData aiData;

    NavMeshAgent agent;

    Animator _Animator;

    //States
    [SerializeField]
    ChaseState _ChaseState;

    //Tiles
    public Vector3Int centerCell;
    public int radius = 5;
    public Tilemap tilemap;
    public List<Vector3> tiles;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        _Animator = GetComponent<Animator>();
    }

    public override State RunState()
    {
        State nextState = this;

        agent.stoppingDistance = 0.5f;

        if (targetDetector.Detect(aiData) == false)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                GetTilePosition();
                Vector2 target = tiles[Random.Range(0, tiles.Count)];
                Flip(target);
                agent.SetDestination(target);
                _Animator.SetFloat("Speed", 0.06f);
            }
            else
            {
                _Animator.SetFloat("Speed", 0.4f);
            }
        }
        else
        {
            _ChaseState._isFacingRight = _isFacingRight;
            nextState = _ChaseState;
        }
        return nextState;
    }

    public void Flip(Vector3 target)
    {
        if (target.x > this.transform.position.x && !_isFacingRight)
        {
            _isFacingRight = !_isFacingRight;
            this.transform.Rotate(0, 180, 0);
        }
        else if (target.x < this.transform.position.x && _isFacingRight)
        {
            _isFacingRight = !_isFacingRight;
            this.transform.Rotate(0, 180, 0);
        }
    }

    void GetTilePosition()
    {
        Vector3 objectPosition = transform.position;
        centerCell = tilemap.WorldToCell(objectPosition);;
        GetTilesInRadius(centerCell, radius);
    }

    void GetTilesInRadius(Vector3Int center, int radius)
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

    bool IsWithinRadius(Vector3Int center, Vector3Int currentCell, int radius)
    {
        return Vector3Int.Distance(center, currentCell) <= radius;
    }

}
