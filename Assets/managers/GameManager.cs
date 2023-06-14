using UnityEngine;
using voxel;

namespace managers
{
    public class GameManager : MonoBehaviour
    {
        
    
        private void Start()
        {
            //Load data through DTM here
            //Instantiate(new VoxelRigidBody());
            DataManager.init();
            
            var go = new GameObject("Test object!");
            go.AddComponent<VoxelRigidBody>();
        }

        //Move these to LVLMan
        public void loadLevel() //Options for seed/data load
        {
            //_PhysicsSimManager.DisablePhysics();
            
        }

        public void unloadLevel()
        {
            PhysicsSimManager.DisablePhysics();
            //Save data using lvl
            //LevelManager.mainLevel.Clear();
            //LevelManager.floatingChunks.Clear();
            
            
        }
    }
}