using UnityEngine;
using System.Collections;

public class TerrainEditor : MonoBehaviour {

    public Terrain terrain;
    TerrainData tData;
    public Terrain resetTerrainObject;

    private int mapWidth, mapHeight;
    private float[,] heights;
    private float[,] height;
    private float[,] gridHeightMap;
    public float resetHeight;
    public  int brushSize;
    // The length of a grid position.
    public int gridLength;
    public float raise;
    // Currently hitting somewhere.
    public bool currentlyClicking = false;
    // The current grid position.
    public int gridX;
    public int gridY;
    // The origin posiiton of the click/touch which is currently occurring in screen space.
    private Vector3 originClick;
    // The original height of the terrain at the grid square currently selected.
    private float originHeight;
    // How drastically to change based on mouse/finger movement.
    public float clickDampingFactor = 200;

    // The maximum height terrain can be modified to.
    public float maxHeight = 1;

    public bool isModeActive = false;

    void Start () {

        tData = terrain.terrainData;
        mapWidth = tData.heightmapWidth;
        mapHeight = tData.heightmapHeight;
        heights = tData.GetHeights(0, 0, mapWidth, mapHeight);
        height = new float[brushSize, brushSize];
        int gridX = mapWidth / brushSize;
        int gridY = mapHeight / brushSize;        

        gridHeightMap = new float[gridX, gridY];

        if (mapWidth != resetTerrainObject.terrainData.heightmapWidth || mapHeight != resetTerrainObject.terrainData.heightmapHeight) {
            Debug.Log("Heightmap width and height of reset map don't match original heightmap!");
        } else {
            float[,] resetHeights = resetTerrainObject.terrainData.GetHeights(0, 0, mapWidth, mapHeight);
            terrain.terrainData.SetHeights(0, 0, resetHeights);
            heights = resetHeights;
            
            // Sample heights for grid height.
            for(int i = 0; i < gridX; i++) {
                for(int j = 0; j < gridY; j++) {
                    // Just grab the height in the middle.
                    gridHeightMap[i,j] = heights[i * brushSize + brushSize/2, j * brushSize + brushSize/2];
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (isModeActive) {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1)) {
                RaycastHit hit;
                if (currentlyClicking == false) {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit)) {
                        Vector3 terrainPos = hit.point;

                        gridX = (int)((terrainPos.x / terrain.terrainData.size.x) * mapWidth / brushSize);
                        gridY = (int)((terrainPos.z / terrain.terrainData.size.z) * mapWidth / brushSize);

                        originHeight = gridHeightMap[gridX, gridY];
                        originClick = Input.mousePosition;

                        currentlyClicking = true;
                        terrain.materialTemplate.SetFloat("_shaderMinX", gridX * brushSize);
                        terrain.materialTemplate.SetFloat("_shaderMinZ", gridY * brushSize);
                        terrain.materialTemplate.SetFloat("_shaderWidth", brushSize + brushSize / 2);
                        terrain.materialTemplate.SetFloat("_shaderLength", brushSize + brushSize / 2);
                    } else {
                        terrain.materialTemplate.SetFloat("_shaderMinX", -1);
                        terrain.materialTemplate.SetFloat("_shaderMinZ", -1);
                        terrain.materialTemplate.SetFloat("_shaderWidth", 0);
                        terrain.materialTemplate.SetFloat("_shaderLength", 0);
                    }
                }
            }
            if (Input.GetMouseButton(0)) {
                // Should be clicking if ray hit.
                if (currentlyClicking) {
                    Vector3 clickDelta = (Input.mousePosition - originClick);
                    float heightDelta = clickDelta.y / clickDampingFactor;
                    raiseTerrainAreaGrid(gridX, gridY, originHeight + heightDelta, brushSize);
                }
            }
            if (Input.GetMouseButtonUp(0)) {
                currentlyClicking = false;
            }


            //resets the map flat
            if (Input.GetKey(KeyCode.R)) {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit)) {
                    resetTerrain(hit.point);
                }
            }
        }
	}

    // brushSize should be used as if it is the class value, may be worth factoring out.
    void raiseTerrainAreaGrid(int gridX, int gridY, float newHeight1, int brushSize)
    {

        int terX = (int)(gridX * brushSize);
        int terZ = (int)(gridY * brushSize);

        float newHeight;

        if (newHeight1 < 0) {
            gridHeightMap[gridX, gridY] = 0;
            newHeight = 0;
        } else if (newHeight1*tData.heightmapHeight >= maxHeight) {
            gridHeightMap[gridX, gridY] = maxHeight/tData.heightmapHeight;
            newHeight = maxHeight / tData.heightmapHeight;
        } else {
            gridHeightMap[gridX, gridY] = newHeight1;
            newHeight = newHeight1;
        }

        if (height.GetLength(0) != brushSize || height.GetLength(1) != brushSize)
            height = new float[brushSize, brushSize];

        for (int y = 0; y < brushSize; y++)
        {
            for (int x = 0; x < brushSize; x++)
            {
                height[x, y] = newHeight;
                heights[terX + x, terZ + y] = newHeight;
            }
        }

        
        terrain.terrainData.SetHeightsDelayLOD(terX, terZ, height);

    }
    //doesnt work yet
    void lowerTerrain(Vector3 point)
    {
        
        int terX = (int)((point.x / terrain.terrainData.size.x) * mapWidth);
        int terZ = (int)((point.z / terrain.terrainData.size.z) * mapHeight);

        float[,] height = new float[1, 1];
        height[0, 0] += 0.001f;
        heights[terX, terZ] += 0.001f;
        terrain.terrainData.SetHeightsDelayLOD(terX, terZ, height);

    }

    //this lags a lot. Im not sure why.
    void resetTerrain(Vector3 point)
    {
        int terX = (int)((point.x / terrain.terrainData.size.x) * mapWidth);
        int terZ = (int)((point.z / terrain.terrainData.size.z) * mapHeight);


        float currHeight = heights[terX, terZ];
        
        float[,] height = new float[brushSize, brushSize];
        

        for(int y = 0; y < brushSize; y++)
        {
            for(int x = 0; x < brushSize; x++)
            {
                height[x, y] = currHeight;
                heights[terX + x, terZ + y] = currHeight;
            }
        }

        
        terrain.terrainData.SetHeightsDelayLOD(terX, terZ, height);
    }
}



