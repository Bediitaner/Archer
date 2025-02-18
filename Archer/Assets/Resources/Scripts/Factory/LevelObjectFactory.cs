using UnityEngine;

namespace ArcheroClone.Factory
{
    public class LevelObjectFactory : MonoBehaviour
    {
        public static GameObject CreateWall(GameObject wallPrefab, Vector3 position, Transform parent = null)
        {
            GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity, parent);
            
            // Ensure the wall has proper collider
            if (wall.GetComponent<BoxCollider2D>() == null)
            {
                BoxCollider2D collider = wall.AddComponent<BoxCollider2D>();
                collider.isTrigger = false;
            }
            
            // Tag the wall
            wall.tag = "Wall";
            
            return wall;
        }
        
        public static GameObject CreateFloor(GameObject floorPrefab, Vector3 position, Transform parent = null)
        {
            GameObject floor = Instantiate(floorPrefab, position, Quaternion.identity, parent);
            
            // No collider needed for floor (just visual)
            
            // Tag the floor
            floor.tag = "Floor";
            
            return floor;
        }
        
        public static GameObject CreateDoor(GameObject doorPrefab, Vector3 position, Transform parent = null)
        {
            GameObject door = Instantiate(doorPrefab, position, Quaternion.identity, parent);
            
            // Ensure the door has proper collider
            if (door.GetComponent<BoxCollider2D>() == null)
            {
                BoxCollider2D collider = door.AddComponent<BoxCollider2D>();
                collider.isTrigger = true;
            }
            
            // Tag the door
            door.tag = "Door";
            
            // Add door controller
            if (door.GetComponent<ArcheroClone.Controller.DoorController>() == null)
            {
                door.AddComponent<ArcheroClone.Controller.DoorController>();
            }
            
            return door;
        }
    }
}