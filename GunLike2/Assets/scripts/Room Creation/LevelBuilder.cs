using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class LevelBuilder : MonoBehaviour
{
    public bool showDebugCubes;

    public Terrain currentTerrain;
    public List<Terrain> terrainList;
    public Transform terrainOptions;
    List<int> unusedTerrains;
    List<NavMeshDataInstance> addedNavData = new List<NavMeshDataInstance>();
    PlayerItem pi; GameDataManager gdm;
    public List<PlaceableObjectChances> chancesObjectsBASE;
    public List<PlaceableObjectChances> chancesFeaturesBASE;
    List<PlaceableObjectChances> chancesObjects;
    List<PlaceableObjectChances> chancesFeatures;
    public GameObject debugCube;
    public GameObject supportPillar;

    public List<Collider> outerWallsColliders;

    public List<GameObject> placed = new List<GameObject>();
    private void Start()
    {
        terrainList = new List<Terrain>();
        unusedTerrains = new List<int>();

        for(int i = 0; i < terrainOptions.childCount; i++)
        {
            Terrain terrain = terrainOptions.GetChild(i).gameObject.GetComponent<Terrain>();
            terrainList.Add(terrain); unusedTerrains.Add(terrainList.IndexOf(terrain));
            terrain.gameObject.SetActive(false);
        }
        currentTerrain = terrainList[0];
        currentTerrain.gameObject.SetActive(true);
        foreach (NavMeshSurface surface in currentTerrain.transform.GetComponentsInChildren<NavMeshSurface>()) { addedNavData.Add(NavMesh.AddNavMeshData(surface.navMeshData)); }

        gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>();
        pi = gdm.phm.playerItem;

        OddsUpdate();
    }
    private void OnApplicationQuit()
    {
        foreach (NavMeshDataInstance navData in addedNavData) { NavMesh.RemoveNavMeshData(navData); }
        addedNavData.Clear();
    }
    private void OnDestroy()
    {
        foreach (NavMeshDataInstance navData in addedNavData) { NavMesh.RemoveNavMeshData(navData); }
        addedNavData.Clear();
    }
    private void Update()
    {
        //Change odds relative to items!
        OddsUpdate();
    }
    void OddsUpdate()
    {
        chancesObjects = new List<PlaceableObjectChances>(); 
        foreach(PlaceableObjectChances poc in chancesObjectsBASE)
        {
            PlaceableObjectChances temp = new PlaceableObjectChances();
            temp.amount = poc.amount; temp.chancePerOne = poc.chancePerOne; temp.chancePerMore = poc.chancePerMore; temp.obj = poc.obj;
            chancesObjects.Add(temp);
        }
        chancesFeatures = new List<PlaceableObjectChances>();
        foreach(PlaceableObjectChances poc in chancesFeaturesBASE)
        {
            PlaceableObjectChances temp = new PlaceableObjectChances();
            temp.amount = poc.amount; temp.chancePerOne = poc.chancePerOne; temp.chancePerMore = poc.chancePerMore; temp.obj = poc.obj;
            chancesFeatures.Add(temp);
        }
        //Irradiated Difficulty
        if(gdm.difficultyIDSelected==2){foreach (PlaceableObjectChances poc in chancesObjects) {
                poc.amount = new Vector2(poc.amount.x, Mathf.CeilToInt(poc.amount.y * 1.2f));
        }}
        //Nuclear Difficulty
        if(gdm.difficultyIDSelected==3){foreach (PlaceableObjectChances poc in chancesObjects) {
                poc.amount = new Vector2(poc.amount.x, Mathf.CeilToInt(poc.amount.y * 1.5f));
        }}
        //Dirt Stained Coffin (ID 123) (OBJ 4)
        chancesObjects[4].chancePerOne = pi.leftItems[123] + pi.rightItems[123] * 100f;
        chancesObjects[4].chancePerMore = pi.leftItems[123] + pi.rightItems[123] * 25f;
        //Chaos Engine (ID 185) (ALL)
        if (pi.leftItems[185] + pi.rightItems[185] > 0){foreach (PlaceableObjectChances poc in chancesObjects){
                poc.amount = new Vector2(poc.amount.x, Mathf.CeilToInt(poc.amount.y * (1.5f * (pi.leftItems[185] + pi.rightItems[185]))));
        }}
        //High Sky Cloud (ID 188) (ALL)
        if (pi.leftItems[188] + pi.rightItems[188] > 0){foreach (PlaceableObjectChances poc in chancesObjects){
                poc.amount = new Vector2(poc.amount.x, Mathf.CeilToInt(poc.amount.y * 1.1f));
        }}
    }
    public void Activate()
    {
        gdm.RoomEnter();
        gdm.roomsUntilBoss--;
        if (gdm.roomsUntilBoss <= 0)
        {
            gdm.roomsUntilBoss = 0;
            currentTerrain = ChangeTerrain(0);

            Build(currentTerrain, false, true);

            gdm.phm.uiMan.deadlineDisabled = true;
            gdm.SpawnBoss("Chimera");
        }
        else
        {
            currentTerrain = ChangeTerrain(-1);

            Build(currentTerrain, true, true);

            gdm.phm.uiMan.deadlineDisabled = false;
            gdm.BeginSpawning();

            if(gdm.roomNumber < 1 && gdm.roofScript != null && gdm.roofScript.playIntro) { gdm.roofScript.OpenRoof(); }
        }
    }
    Terrain ChangeTerrain(int idOverride)
    {
        Terrain output;
        if(idOverride != -1) { output = terrainList[idOverride]; }
        else if(unusedTerrains.Count > 1)
        {
            int newID = unusedTerrains[Random.Range(1,unusedTerrains.Count)];
            unusedTerrains.Remove(newID);
            output = terrainList[newID];
        }
        else 
        { 
            unusedTerrains.Clear(); foreach(Terrain terrain in terrainList) { unusedTerrains.Add(terrainList.IndexOf(terrain)); }
            int newID = unusedTerrains[Random.Range(1, unusedTerrains.Count)];
            unusedTerrains.Remove(newID);
            output = terrainList[newID];
        }

        foreach(Terrain terrain in terrainList) { terrain.gameObject.SetActive(false); }
        output.gameObject.SetActive(true);
        foreach (NavMeshDataInstance navData in addedNavData) { NavMesh.RemoveNavMeshData(navData); } addedNavData.Clear();
        foreach (NavMeshSurface surface in output.transform.GetComponentsInChildren<NavMeshSurface>()) { addedNavData.Add(NavMesh.AddNavMeshData(surface.navMeshData)); }

        return output;
    }
    void Build(Terrain lvlTerrain, bool placeObj, bool placeFeatures)
    {
        if (placed.Count > 0) { foreach(GameObject obj in placed) { Destroy(obj); } placed = new List<GameObject>(); }
        List<Collider> terrainObjects = new List<Collider>();
        Transform terrainObjHolder = lvlTerrain.transform.GetChild(lvlTerrain.transform.childCount - 1);//get the last child
        for(int i = 0; i < terrainObjHolder.childCount; i++)
        {
            terrainObjects.AddRange(terrainObjHolder.GetChild(i).GetComponentsInChildren<Collider>());
        }
        List<Collider> blockingObjects = new List<Collider>();
        blockingObjects.AddRange(terrainObjects);
        blockingObjects.AddRange(outerWallsColliders);
        int resolution = 4;
        Vector3 tSize = lvlTerrain.terrainData.size;
        Vector3 tPos = lvlTerrain.transform.position;
        TerPlaceData[,] tDataFull = new TerPlaceData[Mathf.RoundToInt(tSize.x / resolution), Mathf.RoundToInt(tSize.z / resolution)];
        int maxHeight = 150;
        int paddingFromUnplaceable = Mathf.RoundToInt(50f / resolution);
        List<Vector2> placeableArrayIndex = new List<Vector2>();
        for (int x = 0; x < tDataFull.GetLength(0); x++)//build height map
        {
            for(int z = 0; z < tDataFull.GetLength(1); z++)
            {
                Vector3 worldPos = tPos + Vector3.right * x * resolution + Vector3.forward * z * resolution;
                tDataFull[x,z] = new TerPlaceData();
                tDataFull[x,z].myTerrain = lvlTerrain;
                tDataFull[x,z].height = lvlTerrain.SampleHeight(worldPos);
                tDataFull[x,z].worldPos = worldPos + Vector3.up*tDataFull[x,z].height;
                tDataFull[x,z].localPos = worldPos - tPos;
                tDataFull[x,z].arrayPos = new Vector2(x,z);
            }
        }
        for (int x = 0; x < tDataFull.GetLength(0); x++)//check for placeable spots
        {
            for (int z = 0; z < tDataFull.GetLength(1); z++)
            {
                bool placeable = true;
                tDataFull[x, z].placeable = false;
                if (tDataFull[x, z].height > maxHeight) { placeable = false; }
                else
                {
                    float maxHeightDiff = 2f;
                    if (Mathf.Abs(tDataFull[x, z].height - tDataFull[x + 1, z].height) > maxHeightDiff ||
                        Mathf.Abs(tDataFull[x, z].height - tDataFull[x, z + 1].height) > maxHeightDiff ||
                        Mathf.Abs(tDataFull[x, z].height - tDataFull[x + 1, z + 1].height) > maxHeightDiff ||
                        Mathf.Abs(tDataFull[x, z].height - tDataFull[x - 1, z].height) > maxHeightDiff ||
                        Mathf.Abs(tDataFull[x, z].height - tDataFull[x, z - 1].height) > maxHeightDiff ||
                        Mathf.Abs(tDataFull[x, z].height - tDataFull[x - 1, z - 1].height) > maxHeightDiff ||
                        Mathf.Abs(tDataFull[x, z].height - tDataFull[x + 1, z - 1].height) > maxHeightDiff ||
                        Mathf.Abs(tDataFull[x, z].height - tDataFull[x - 1, z + 1].height) > maxHeightDiff)
                    { placeable = false; }
                    else
                    {
                        float maxObjectDist = 8f;
                        for(int i = 0; i < blockingObjects.Count; i++)
                        {
                            if (Vector3.Distance(blockingObjects[i].ClosestPoint(tDataFull[x, z].worldPos), tDataFull[x, z].worldPos) < maxObjectDist)
                            { placeable = false; break; }
                        }
                        //OLD loop for checking if near the max height \/
                        //if (placeable)
                        //{
                            //for (int i = 0; i < paddingFromUnplaceable; i++)
                            //{
                            //    if (tDataFull[x + i, z].height > maxHeight ||
                            //        tDataFull[x, z + i].height > maxHeight ||
                            //        tDataFull[x + i, z + i].height > maxHeight ||
                            //        tDataFull[x - i, z].height > maxHeight ||
                            //        tDataFull[x, z - i].height > maxHeight ||
                            //        tDataFull[x - i, z - i].height > maxHeight ||
                            //        tDataFull[x - i, z + i].height > maxHeight ||
                            //        tDataFull[x + i, z - i].height > maxHeight)
                            //    { placeable = false; break; }
                            //}
                        //}
                    }
                }
                if (placeable) 
                { 
                    tDataFull[x, z].placeable = true; placeableArrayIndex.Add(new Vector2(x, z));
                    //Debug Cubes
                    if (showDebugCubes) { SpawnDebugCubes(tDataFull, x, z); }
                }
            }
        }
        foreach(PlaceableObjectChances obj in chancesObjects)
        {
            AttemptPlaceObject(chancesObjects[chancesObjects.IndexOf(obj)], placeableArrayIndex, lvlTerrain, tDataFull, tPos, resolution);
        }
        foreach (PlaceableObjectChances obj in chancesFeatures)
        {
            AttemptPlaceFeature(chancesFeatures[chancesFeatures.IndexOf(obj)], placeableArrayIndex, lvlTerrain, tDataFull, tPos, resolution);
        }
    }
    void SpawnDebugCubes(TerPlaceData[,] tDataFull, int x, int z)
    {
        GameObject spawned = Instantiate(debugCube, tDataFull[x, z].worldPos, transform.rotation); spawned.GetComponent<MeshRenderer>().material.color = Color.green; placed.Add(spawned);
    }
    void AttemptPlaceObject(PlaceableObjectChances objToPlace, List<Vector2> placeableArrayIndex, Terrain lvlTerrain, TerPlaceData[,] tDataFull, Vector3 tPos, int resolution)
    {
        int amount = 0; if (Random.Range(1, 100) < objToPlace.chancePerOne) { amount = (int)objToPlace.amount.x; }
        for(int i = 0; i < objToPlace.amount.y - objToPlace.amount.x; i++)
        {
            if (Random.Range(1, 100) < objToPlace.chancePerMore) { amount++; }
        }
        for(int i = 0; i < amount; i++) { PlaceObject(objToPlace.obj, placeableArrayIndex, lvlTerrain, tDataFull, tPos, resolution); }
    }
    void PlaceObject(GameObject objToPlace, List<Vector2> placeableArrayIndex, Terrain lvlTerrain, TerPlaceData[,] tDataFull, Vector3 tPos, int resolution)//Needs a flat ground
    {
        GameObject placedObject = Instantiate(objToPlace); placed.Add(placedObject);
        int rand = Random.Range(0, placeableArrayIndex.Count);
        TerPlaceData pointToBePlacedOn = tDataFull[Mathf.RoundToInt(placeableArrayIndex[rand].x), Mathf.RoundToInt(placeableArrayIndex[rand].y)];
        PlaceableObject objData = placedObject.GetComponent<PlaceableObject>();
        placedObject.transform.position = pointToBePlacedOn.worldPos;
        placedObject.transform.localEulerAngles = new Vector3(placedObject.transform.localEulerAngles.x, Random.Range(0, 360), placedObject.transform.localEulerAngles.z);
        PlaceSupport(placedObject, objData.footprint, tDataFull, pointToBePlacedOn, resolution, objData, placeableArrayIndex);
        //SetHeight(pointToBePlacedOn, pointToBePlacedOn.height, objData.footprint, resolution);
    }
    void AttemptPlaceFeature(PlaceableObjectChances objToPlace, List<Vector2> placeableArrayIndex, Terrain lvlTerrain, TerPlaceData[,] tDataFull, Vector3 tPos, int resolution)
    {
        int amount = 0; if (Random.Range(1, 100) < objToPlace.chancePerOne) { amount = (int)objToPlace.amount.x; }
        for (int i = 0; i < objToPlace.amount.y - objToPlace.amount.x; i++)
        {
            if (Random.Range(1, 100) < objToPlace.chancePerMore) { amount++; }
        }
        for (int i = 0; i < amount; i++) { PlaceFeature(objToPlace.obj, placeableArrayIndex, lvlTerrain, tDataFull, tPos, resolution); }
    }
    void PlaceFeature(GameObject objToPlace, List<Vector2> placeableArrayIndex, Terrain lvlTerrain, TerPlaceData[,] tDataFull, Vector3 tPos, int resolution)//Does not need a flat ground
    {
        GameObject placedObject = Instantiate(objToPlace); placed.Add(placedObject);
        int rand = Random.Range(0, placeableArrayIndex.Count);
        TerPlaceData pointToBePlacedOn = tDataFull[Mathf.RoundToInt(placeableArrayIndex[rand].x), Mathf.RoundToInt(placeableArrayIndex[rand].y)];
        PlaceableObject objData = placedObject.GetComponent<PlaceableObject>();
        placedObject.transform.position = pointToBePlacedOn.worldPos; placedObject.transform.position = new Vector3(placedObject.transform.position.x, 0, placedObject.transform.position.z);
        placedObject.transform.localEulerAngles = new Vector3(placedObject.transform.localEulerAngles.x, Random.Range(0, 360), placedObject.transform.localEulerAngles.z);
        PlaceSupport(placedObject, objData.footprint, tDataFull, pointToBePlacedOn, resolution, objData, placeableArrayIndex);
    }
    void PlaceSupport(GameObject placedObj, float footprint, TerPlaceData[,] tDataFull, TerPlaceData tpd, int resolution, PlaceableObject objData, List<Vector2> placeableArrayIndex)
    {
        float maxLocalHeight = 0;
        for(int x = 0; x < tDataFull.GetLength(0); x++)
        {for (int z = 0; z < tDataFull.GetLength(1); z++){
                if(Vector2.Distance(new Vector2(tDataFull[x,z].worldPos.x, tDataFull[x, z].worldPos.z),new Vector2(tpd.worldPos.x, tpd.worldPos.z)) < footprint + Mathf.CeilToInt(footprint/resolution)) 
                {
                    if(tDataFull[x, z].height > maxLocalHeight)
                    {
                        maxLocalHeight = tDataFull[x, z].height;
                    }
                    tDataFull[x, z].placeable = false;
                    placeableArrayIndex.Remove(tDataFull[x, z].arrayPos);
                }
            }
        }
        if (!objData.flatten || maxLocalHeight == 0) { return; }

        placedObj.transform.position = new Vector3(placedObj.transform.position.x, maxLocalHeight, placedObj.transform.position.z);
        GameObject pillar = Instantiate(supportPillar); placed.Add(pillar);
        pillar.transform.position = placedObj.transform.position;
        pillar.transform.localScale = new Vector3(footprint, maxLocalHeight, footprint);
    }
}
