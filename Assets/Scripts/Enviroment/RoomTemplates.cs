using NavMeshPlus.Components;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    public GameObject[] bossRooms;
    public bool bossSpawned = false;
    public bool bossDestroy = false;

    public int maxRooms;
    public int minRooms;
    public int roomCounter;

    public GameObject origin;
    private int lastCounter = 0;

    private NavMeshSurface meshSurface;

    private void Start()
    {
        meshSurface = GameObject.FindGameObjectWithTag("NavMesh").GetComponent<NavMeshSurface>();
        Instantiate(origin, transform.position, Quaternion.identity);
        InvokeRepeating("check", 0.7f, 0.7f);
    }
    private void check()
    {
        if((lastCounter == roomCounter && roomCounter < maxRooms) || roomCounter >= maxRooms && bossDestroy || roomCounter >= maxRooms && bossSpawned == false)
        {
            GameObject[] rooms = GameObject.FindGameObjectsWithTag("Rooms");
            foreach (var rom in rooms)
            {
                Destroy(rom);
            }
            roomCounter = 0;
            bossDestroy = false;
            bossSpawned = false;
            Instantiate(origin, transform.position, Quaternion.identity);
        } else if (roomCounter >= maxRooms && bossDestroy == false && bossSpawned)
        {
            meshSurface.BuildNavMesh();
            GameObject[] rooms = GameObject.FindGameObjectsWithTag("Rooms");
            foreach (var room in rooms)
            {
                TilemapSize tilemapSize = room.GetComponent<TilemapSize>();
                if (tilemapSize != null)
                {
                    tilemapSize.RemoveDoors();
                    tilemapSize.enabled = false;
                }
            }
            Destroy(gameObject);
        }
        else
        {
            lastCounter = roomCounter;
        }
    }
}
