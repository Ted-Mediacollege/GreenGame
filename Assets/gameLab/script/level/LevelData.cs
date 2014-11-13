using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelData : MonoBehaviour{

	[SerializeField]
	private GameObject[] tiles;
	[SerializeField]
	private GameObject[] objects;
	[SerializeField]
	private GameObject[] buildings;

	//PREFABS
	private static GameObject[] staticTiles;
	private static GameObject[] staticObjects;
	public static GameObject[] staticBuildings;

	private static IGenerator generator;

	public static GameObject[,] LoadedGroundTiles;
	public static List<MapObject> mapObjects;
	public static int width,height;
	public static int[,] objectData;
	private static bool[,] collsionData;
	//public static List<Building> buildingList;

    private static GameObject levelHolder;
    private static GameObject buildingHolder;
    private static GameObject unitHolder;
    private static GameObject tileHolder;
	
	public static bool[,] CollsionData{
		get{
			return collsionData;
		}
	}

	public static int size;
	private static int w,h;

	private void Start(){
        levelHolder = new GameObject("LevelHolder");
        buildingHolder = new GameObject("buildingHolder");
        buildingHolder.transform.parent = levelHolder.transform;
        unitHolder = new GameObject("unitHolder");
        unitHolder.transform.parent = levelHolder.transform;
        tileHolder = new GameObject("tileHolder");
        tileHolder.transform.parent = levelHolder.transform;
		staticTiles = tiles;
		staticObjects = objects;
		staticBuildings = buildings;

		generator = new GeneratorTest();

		LoadLevelData ();
	}

    public static MapObject GetMapObjects(int x,int y)
    {
        for (int i = 0; i < mapObjects.Count; i++)
        {
            if (mapObjects[i].pos.x == x && mapObjects[i].pos.y == y)
            {
                return mapObjects[i];
            }
        }
        return null;
    }

    public static Building[] GetAllBuildings()
    {
        List<Building> returnBuildings = new List<Building>();
        for (int i = 0; i < mapObjects.Count; i++)
        {
            if (mapObjects[i] is Building)
            {
                returnBuildings.Add((Building)mapObjects[i]);
            }
        }
        return returnBuildings.ToArray();
    }

	public static void LoadLevelData () {
		
        size = 50;
		generator.Generate(size);
		width = generator.data.GetLength(0);
        height = generator.data.GetLength(1);
        
        BuildTiles(generator.data);

        collsionData = new bool[width, height];

        objectData = RandomData.RandomTestData(size, size, new int[] { 
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,2 
        });
		BuildObjects(objectData);

        //buildingList = new List<Building>();
		for(int j = 0; j < 1; j++) {
			//constructBuilding(5 , 6 + j * 2     , 0, 2);
			constructBuilding(9 , 6 + j * 2     , 1, 2);
			constructBuilding(13, 6 + j * 2     , 2, 2);
			constructBuilding(15, 6 + j * 2     , 3, 1);
			constructBuilding(15, 6 + j * 2 + 1 , 3, 1);
		}
	}

	public static bool constructBuilding(int x, int y, int id, int size) {
		for(int i = 0; i < size; i++) {
			for(int j = 0; j < size; j++) {
				if(collsionData[x - i, y - j]) {
					return false;
				}
			}
		}

		for(int k = 0; k < size; k++) {
			for(int l = 0; l < size; l++) {
				collsionData[x - k, y - l] = true;
			}
		}
		float allSizesX = 0;
		float allSizesY = 0;
		if(size == 1)
		{
			allSizesX = -0.1f;
			allSizesY = 0.4f;
		}else if(size == 3)
		{
			allSizesX = 0.47f;
			allSizesY = -0.3f;
		}
		Vector2 pos = IsoMath.tileToWorld(x, y);
		pos += new Vector2 (allSizesX, allSizesY);
		GameObject building = (GameObject)GameObject.Instantiate (staticBuildings[id], new Vector3 (pos.x, pos.y, 0f), new Quaternion());
        building.transform.parent = buildingHolder.transform;
		//IBuilding s = (IBuilding)building.GetComponent(typeof(Building));  <--- is now done in the building start() func.
		//buildingList.Add(s);
		//calculateEnegy();
		return true;
	}
	
	private static void BuildObjects (int[,] data) {
		//Debug.Log ("width: "+width+" height: "+height);
		mapObjects = new List<MapObject>();
		for (h = 0; h < height; h++) {
			for (w = 0; w < width; w++) {
				int objectID = data[w,h];
				if(objectID != 0){
					objectID -= 1;
					Vector2 pos = IsoMath.tileToWorld(w,h);
                    GameObject newBuilding = (GameObject)GameObject.Instantiate(staticObjects[objectID], new Vector3(pos.x, pos.y, (h + (size - w)) / 2.5f + 2f), new Quaternion());
                    MapObject newBuildingMapObject = (MapObject)newBuilding.GetComponent<MapObject>();
                    newBuilding.transform.parent = unitHolder.transform;
                    newBuildingMapObject.pos = new VecInt(w, h);
                    mapObjects.Add(newBuildingMapObject);
					collsionData[w,h] = true;
				}
			}
		}
	}

	private static void BuildTiles (int[,] data) {
		LoadedGroundTiles = new GameObject[width,height];
		for (h = 0; h < height; h++) {
			for (w = 0; w < width; w++) {
				Vector2 pos = IsoMath.tileToWorld(w,h); 
				GameObject tile = (GameObject)GameObject.Instantiate (staticTiles[data[w,h]], new Vector3 (pos.x, pos.y, (h + (size - w)) / 2.5f + 10f), new Quaternion ());
                tile.transform.parent = tileHolder.transform;
				LoadedGroundTiles[w,h] = (GameObject)tile;
			}
		}
	}
}

