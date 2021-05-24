using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsinpa.View
{
    public class ToolItemObject : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem particle;

        public bool vibrate;
        public bool deltaOrientation;

        public void EnableParticle(bool p_enable)
        {
            if (p_enable && !particle.isPlaying)
                particle.Play();
            else if (!p_enable)
                particle.Stop();
        }
    }
}