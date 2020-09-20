//Mesh Data will store all the verticies and elements of the mesh data for a chunk.
//Dylan Buchheim, 2019

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    //Global Variables -----------------------------------
    public List<Vector3> verticies = new List<Vector3>();
    public List<int> triangles = new List<int>();
    public List<Vector2> uv = new List<Vector2>();
    public List<Vector2> uv3 = new List<Vector2>();
    public List<Vector3> colVerticies = new List<Vector3>();
    public List<int> colTriangles = new List<int>();

    public bool useRenderDataForCol;
    //Mesh Data Constructor ------------------------------
    public MeshData() {

    }

    public void AddQuadTriangles() {
        triangles.Add(verticies.Count - 4);
        triangles.Add(verticies.Count - 3);
        triangles.Add(verticies.Count - 2);
        triangles.Add(verticies.Count - 4);
        triangles.Add(verticies.Count - 2);
        triangles.Add(verticies.Count - 1);
        if (useRenderDataForCol) {
            colTriangles.Add(colVerticies.Count - 4);
            colTriangles.Add(colVerticies.Count - 3);
            colTriangles.Add(colVerticies.Count - 2);
            colTriangles.Add(colVerticies.Count - 4);
            colTriangles.Add(colVerticies.Count - 2);
            colTriangles.Add(colVerticies.Count - 1);
        }
    }

    public void AddVertex(Vector3 vertex) {
        verticies.Add(vertex);
        if (useRenderDataForCol) {
            colVerticies.Add(vertex);
        }
    }

    public void AddTriangle(int tri){
        triangles.Add(tri);
        if (useRenderDataForCol) {
            colTriangles.Add(tri - (verticies.Count - colVerticies.Count));
        }
    }
}
