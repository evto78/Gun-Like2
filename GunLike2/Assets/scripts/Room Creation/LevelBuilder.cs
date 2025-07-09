using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    public Terrain terrain;
    public int minPlacedObjects;
    public int maxPlacedObjects;
    public int minPlacedFeatures;
    public int maxPlacedFeatures;
    int placedObj;
    int placedFeats;
    public List<GameObject> placeableObjects;
    public List<GameObject> placeableFeatures;
    public GameObject debugCube;
    public GameObject supportPillar;

    void Start()
    {
        terrain = Terrain.activeTerrains[0];

        Build(terrain);
    }
    void Build(Terrain lvlTerrain)
    {
        placedObj = 0; placedFeats = 0;
        int objectsToPlace = Random.Range(minPlacedObjects, maxPlacedObjects + 1);
        int featuresToPlace = Random.Range(minPlacedFeatures, maxPlacedFeatures + 1);
        int resolution = 4;
        Vector3 tSize = lvlTerrain.terrainData.size;
        Vector3 tPos = lvlTerrain.transform.position;
        TerPlaceData[,] tDataFull = new TerPlaceData[Mathf.RoundToInt(tSize.x / resolution), Mathf.RoundToInt(tSize.z / resolution)];
        int maxHeight = 150;
        int paddingFromUnplaceable = Mathf.RoundToInt(80f / resolution);
        List<Vector2> placeableArrayIndex = new List<Vector2>();
        List<Vector2> onRoadArrayIndex = new List<Vector2>();
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
                    for (int i = 0; i < paddingFromUnplaceable; i++)
                    {
                        if (tDataFull[x+i,z].height > maxHeight ||
                            tDataFull[x,z+i].height > maxHeight ||
                            tDataFull[x+i,z+i].height > maxHeight ||
                            tDataFull[x-i,z].height > maxHeight ||
                            tDataFull[x,z-i].height > maxHeight ||
                            tDataFull[x-i,z-i].height > maxHeight ||
                            tDataFull[x-i,z+i].height > maxHeight ||
                            tDataFull[x+i,z-i].height > maxHeight ) 
                        { placeable = false; break; }
                    }
                }
                if (placeable) 
                { 
                    tDataFull[x, z].placeable = true; placeableArrayIndex.Add(new Vector2(x, z));
                    //GameObject spawned = Instantiate(debugCube, tDataFull[x,z].worldPos, transform.rotation); spawned.GetComponent<MeshRenderer>().material.color = Color.green;
                    tDataFull[x,z].onRoad = GetMainTexture(tDataFull[x, z].worldPos, lvlTerrain) == 0;
                    if (tDataFull[x, z].onRoad) { onRoadArrayIndex.Add(new Vector2(x, z)); } //spawned.GetComponent<MeshRenderer>().material.color = Color.blue;
                }
            }
        }
        while (placedObj < objectsToPlace)
        {
            PlaceObject(placeableArrayIndex, lvlTerrain, tDataFull, tPos, resolution);
            placedObj++;
        }
        while(placedFeats < featuresToPlace)
        {
            PlaceFeature();
            placedFeats++;
        }
    }
    void PlaceObject(List<Vector2> placeableArrayIndex, Terrain lvlTerrain, TerPlaceData[,] tDataFull, Vector3 tPos, int resolution)//Needs a flat ground
    {
        GameObject placedObject = Instantiate(placeableObjects[0]);
        int rand = Random.Range(0, placeableArrayIndex.Count);
        TerPlaceData pointToBePlacedOn = tDataFull[Mathf.RoundToInt(placeableArrayIndex[rand].x), Mathf.RoundToInt(placeableArrayIndex[rand].y)];
        PlaceableObject objData = placedObject.GetComponent<PlaceableObject>();
        placedObject.transform.position = pointToBePlacedOn.worldPos;
        if (!objData.flatten) {return; }
        PlaceSupport(placedObject, objData.footprint, tDataFull, pointToBePlacedOn, resolution);
        //SetHeight(pointToBePlacedOn, pointToBePlacedOn.height, objData.footprint, resolution);
    }
    void PlaceFeature()//Does not need a flat ground
    {

    }
    void PlaceSupport(GameObject placedObj, float footprint, TerPlaceData[,] tDataFull, TerPlaceData tpd, int resolution)
    {
        float maxLocalHeight = 0;
        for(int x = 0; x < tDataFull.GetLength(0); x++)
        {
            for (int z = 0; z < tDataFull.GetLength(1); z++)
            {
                if(Vector2.Distance(new Vector2(tDataFull[x,z].worldPos.x, tDataFull[x, z].worldPos.z),new Vector2(tpd.worldPos.x, tpd.worldPos.z)) < footprint + Mathf.CeilToInt(footprint/resolution) && tDataFull[x, z].height > maxLocalHeight) 
                {
                    maxLocalHeight = tDataFull[x, z].height;
                }
            }
        }
        placedObj.transform.position = new Vector3(placedObj.transform.position.x, maxLocalHeight, placedObj.transform.position.z);
        GameObject pillar = Instantiate(supportPillar);
        pillar.transform.position = placedObj.transform.position;
        pillar.transform.localScale = new Vector3(footprint, maxLocalHeight, footprint);
    }
    void SetHeight(TerPlaceData tpd, float newHeight, float footprint, int resolution)
    {
        TerrainData tData = tpd.myTerrain.terrainData;

        Vector3 tempCoord = transform.position - tpd.myTerrain.gameObject.transform.position;
        Vector3 coord;

        coord.x = tempCoord.x / tData.size.x;
        coord.y = tempCoord.y / tData.size.y;
        coord.z = tempCoord.z / tData.size.z;

        int hmScale = tData.heightmapResolution;

        float posXInTerrain = coord.x * hmScale * 1.2f;
        float posYInTerrain = coord.z * hmScale * 1.2f;

        float[,] heights = tData.GetHeights(Mathf.RoundToInt(posXInTerrain), Mathf.RoundToInt(posYInTerrain), Mathf.RoundToInt(footprint*2), Mathf.RoundToInt(footprint*2));
        
        for(int x = 0; x < heights.GetLength(0); x++)
        {
            for(int z = 0; z < heights.GetLength(1); z++)
            {
                heights[x, z] = newHeight / tData.size.y;
            }
        }
        tData.SetHeights(Mathf.RoundToInt(tpd.localPos.x), Mathf.RoundToInt(tpd.localPos.z), heights);
    }


    //Helperscript from online forum \/
    public static float[] GetTextureMix(Vector3 worldPos, Terrain tarTerrain)
    {

        // returns an array containing the relative mix of textures
        // on the main terrain at this world position.

        // The number of values in the array will equal the number
        // of textures added to the terrain.

        Terrain terrain = tarTerrain;
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainPos = terrain.transform.position;

        // calculate which splat map cell the worldPos falls within (ignoring y)
        int mapX = (int)(((worldPos.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth);
        int mapZ = (int)(((worldPos.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight);

        // get the splat data for this cell as a 1x1xN 3d array (where N = number of textures)
        float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

        // extract the 3D array data to a 1D array:
        float[] cellMix = new float[splatmapData.GetUpperBound(2) + 1];
        for (int n = 0; n < cellMix.Length; ++n)
        {
            cellMix[n] = splatmapData[0, 0, n];
        }

        return cellMix;

    }
    public static int GetMainTexture(Vector3 worldPos, Terrain tarTerrain)
    {

        // returns the zero-based index of the most dominant texture
        // on the main terrain at this world position.

        float[] mix = GetTextureMix(worldPos, tarTerrain);

        float maxMix = 0;
        int maxIndex = 0;

        // loop through each mix value and find the maximum
        for (int n = 0; n < mix.Length; ++n)
        {
            if (mix[n] > maxMix)
            {
                maxIndex = n;
                maxMix = mix[n];
            }
        }

        return maxIndex;

    }
}
