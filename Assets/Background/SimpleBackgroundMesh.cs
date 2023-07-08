using System;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace.Background
{
    [RequireComponent(typeof(MeshRenderer))]
    public class SimpleBackgroundMesh : BackgroundChangeWatcher
    {

        private MeshRenderer _meshRenderer;
        
        private void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        protected override void OnBackgroundChange(BackgroundManager.BackgroundChangeContext context, HWEventCallback callback)
        {
            if (activeBackgrounds.Contains(context))
            {
                _meshRenderer.enabled = true;
            }
            else
            {
                _meshRenderer.enabled = false;
            }
        }
    }
}