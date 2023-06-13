using UnityEngine;

namespace managers
{
    public class GameManager : MonoBehaviour
    {
        public DataManager _DataManager { get; private set; }
        public PhysicsSimManager _PhysicsSimManager { get; private set; }
        public LevelManager _LevelManager { get; private set; }
    
        private void Start()
        {
            _PhysicsSimManager = new PhysicsSimManager();
            _DataManager = new DataManager(); //Handles the loading of assets - materials etc
            _LevelManager = new LevelManager(); //Handles loading the world from data or proc gen
        }

        public void loadLevel() //Options for seed/data load
        {
            _PhysicsSimManager.DisablePhysics();
            
        }

        public void unloadLevel()
        {
            _PhysicsSimManager.DisablePhysics();
            //Call datamanager to save
            //Call physics sim to remove stuff?
            
            
        }
    }
}