namespace DefaultNamespace
{
    public class HWEventCallback
    {
        private bool cancelled = false;
        
        public bool isCancelled()
        {
            return cancelled;
        }
        public void Cancel()
        {
            cancelled = true;
        }
        
    }
}