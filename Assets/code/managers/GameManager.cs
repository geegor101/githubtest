using Steamworks;
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
            if (SteamManager.Initialized)
                TestSteam();
        }

        private void TestSteam()
        {
            //Debug.Log(SteamFriends.GetPersonaName());
            //Debug.Log( "Friends:" + SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate));
            //SteamFriends.get
            //SteamFriends.GetFriendByIndex(1, EFriendFlags.k_EFriendFlagAll)
                
            
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