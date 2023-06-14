using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


namespace voxel
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshCollider))]
    public class VoxelRigidBody : MonoBehaviour
    {
        public Rigidbody rigidbody;
        public MeshRenderer meshRenderer;
        public MeshFilter meshFilter;
        public MeshCollider collider;

        public VoxelChunk _VoxelChunk;
        
        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            meshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();
            collider = GetComponent<MeshCollider>();
            
            //Do any init stuff on these
            
            //Add to phys manager

            _VoxelChunk = new VoxelChunk();
            //Load data to the chunk here?
            
            
            
            _VoxelChunk._voxels[new Vector3(3, 4, 5)] = new Voxel(WorldMaterial.WorldMaterials[new AssetLoc("default", "test mat")]);
            
            
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            
            loadMeshData();
        }

        public void loadMeshData()
        {

            List<Vector3> verts = new List<Vector3>();
            List<int> tris = new List<int>();
            List<Vector2> UVs = new List<Vector2>();

            Mesh mesh = meshFilter.mesh;
            //mesh.Clear();
            
            Vector3[] faceVertices = new Vector3[4];
            Vector2[] faceUVs = new Vector2[4];

            int counter = 0;
            
            foreach (var voxelChunkVoxel in _VoxelChunk._voxels)
            {
                var voxel = voxelChunkVoxel.Value;
                var pos = voxelChunkVoxel.Key;
                
                
                if (!voxel._Material.Occluder())
                    continue;

                for (int i = 0; i < 6; i++)
                {
                    var checkPos = pos + voxelFaceChecks[i];
                    
                    if (!_VoxelChunk._voxels.ContainsKey(checkPos) || _VoxelChunk._voxels[checkPos]._Material.Occluder())
                        continue;

                    for (int j = 0; j < 4; j++)
                    {
                        faceVertices[j] = voxelVertices[voxelVertexIndex[i, j]] + pos;
                        faceUVs[j] = voxelUVs[j];
                    }

                    for (int j = 0; j < 6; j++)
                    {
                        
                        verts.Add(faceVertices[voxelTris[i, j]]);
                        UVs.Add(faceUVs[voxelTris[i, j]]);

                        tris.Add(counter++);
                    }
                }
                
            }
            
            
            mesh.SetVertices(verts);
            mesh.SetTriangles(tris, 0, false);
            mesh.SetUVs(0, UVs);

            mesh.Optimize();
            
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.RecalculateTangents();
            
            mesh.UploadMeshData(false);

            collider.sharedMesh = mesh;



        }
        
        //Taken from https://github.com/pixelreyn/VoxelProjectSeries/blob/Part2-FirstChunk/Assets/VoxelProjectSeries/Data/Container.cs
        #region Voxel Statics

        static readonly Vector3[] voxelVertices = new Vector3[8]
        {
            new Vector3(0,0,0),//0
            new Vector3(1,0,0),//1
            new Vector3(0,1,0),//2
            new Vector3(1,1,0),//3

            new Vector3(0,0,1),//4
            new Vector3(1,0,1),//5
            new Vector3(0,1,1),//6
            new Vector3(1,1,1),//7
        };
        static readonly Vector3[] voxelFaceChecks = new Vector3[6]
        {
            new Vector3(0,0,-1),//back
            new Vector3(0,0,1),//front
            new Vector3(-1,0,0),//left
            new Vector3(1,0,0),//right
            new Vector3(0,-1,0),//bottom
            new Vector3(0,1,0)//top
        };

        static readonly int[,] voxelVertexIndex = new int[6, 4]
        {
            {0,1,2,3},
            {4,5,6,7},
            {4,0,6,2},
            {5,1,7,3},
            {0,1,4,5},
            {2,3,6,7},
        };

        static readonly Vector2[] voxelUVs = new Vector2[4]
        {
            new Vector2(0,0),
            new Vector2(0,1),
            new Vector2(1,0),
            new Vector2(1,1)
        };

        static readonly int[,] voxelTris = new int[6, 6]
        {
            {0,2,3,0,3,1},
            {0,1,2,1,3,2},
            {0,2,3,0,3,1},
            {0,1,2,1,3,2},
            {0,1,2,1,3,2},
            {0,2,3,0,3,1},
        };

        #endregion
    }
}