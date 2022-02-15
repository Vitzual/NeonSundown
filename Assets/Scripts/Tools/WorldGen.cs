using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class WorldGen : MonoBehaviour
{
    // Active instance
    public static WorldGen active;

    // Point to the active viewer
    public Transform viewer;
    public static Vector2 viewerPosition;

    // Chunk variables
    public int seed = 0;
    public int tileSize = 250;
    public int chunkSize = 10;
    public int viewDistance = 5;
    public GameObject emptyChunk;

    // List of environment tiles
    public Tilemap environment;

    // Loaded chunks
    public Dictionary<Vector2Int, GameObject> loadedChunks;

    public void Start()
    {
        // Get active instance
        active = this;

        // Create a new resource grid
        loadedChunks = new Dictionary<Vector2Int, GameObject>();
    }

    public void Update()
    {
        if (viewer != null)
        {
            viewerPosition = new Vector2(viewer.position.x, viewer.position.y);
            UpdateChunks();
        }
    }

    // Generate resources
    public void UpdateChunks()
    {
        // Get chunk coordinates
        Vector2Int coords = Vector2Int.RoundToInt(new Vector2(
            viewerPosition.x / tileSize / chunkSize,
            viewerPosition.y / tileSize / chunkSize));

        // Create new chunks list
        List<Vector2Int> chunks = GetChunks(coords);
        List<GameObject> chunksLoaded = new List<GameObject>();

        // Loops through chunks
        foreach (Vector2Int chunkCoords in chunks)
        {
            // Check that the chunk isn't loaded
            if (loadedChunks.ContainsKey(chunkCoords))
            {
                loadedChunks[chunkCoords].SetActive(true);
                chunksLoaded.Add(loadedChunks[chunkCoords]);
            }
            else
            {
                GameObject newChunk = GenerateNewChunk(chunkCoords);
                loadedChunks.Add(chunkCoords, newChunk);
                chunksLoaded.Add(newChunk);
            }
        }

        // Disable chunks that are out of sight
        foreach (GameObject loadedChunk in loadedChunks.Values)
            if (!chunksLoaded.Contains(loadedChunk))
                loadedChunk.gameObject.SetActive(false);
    }

    // Loops through a new chunk and spawns resources based on perlin noise values
    private GameObject GenerateNewChunk(Vector2Int newChunk)
    {
        // Get tile coordinate
        Vector2Int tileCoords = new Vector2Int(
            newChunk.x * chunkSize,
            newChunk.y * chunkSize);

        // Convert the chunk coordinates to world position
        Vector2 worldPosition = new Vector2(tileCoords.x * 5, tileCoords.y * 5);
        GameObject chunk = Instantiate(emptyChunk, worldPosition, Quaternion.identity);
        chunk.name = newChunk.x + " " + newChunk.y;

        // Spawn background tiles
        SetBackground(Gamemode.arena.arenaBackground, tileCoords);

        return chunk;
    }

    // Try and spawn a biome in a specified chunk
    private void SetBackground(TileBase tile, Vector2Int tileCoords)
    {
        // Iterate through each perlin pixel
        for (int y = 0; y < chunkSize; y++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                // Get the tile position of the noise pixel
                Vector3Int tilePosition = new Vector3Int(tileCoords.x + x, tileCoords.y + y, 0);

                // Check if the map is already filled here
                if (!environment.HasTile(tilePosition))
                    environment.SetTile(tilePosition, tile);
            }
        }
    }

    private List<Vector2Int> GetChunks(Vector2Int chunk)
    {
        // Create new chunks list
        List<Vector2Int> chunks = new List<Vector2Int>();

        // Get surrounding chunks based on render distance
        for (int x = chunk.x - viewDistance; x < chunk.x + viewDistance; x++)
            for (int y = chunk.y - viewDistance; y < chunk.y + viewDistance; y++)
                chunks.Add(new Vector2Int(x, y));

        // Return chunk coordinates
        return chunks;
    }
}