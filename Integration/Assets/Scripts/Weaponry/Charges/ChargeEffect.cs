using UnityEngine;

namespace Assets.Scripts.Weaponry.Charges
{
    public class ChargeEffect : MonoBehaviour, IChargeNotifiable
    {
        // -- Editor

        [Header("Parts")]
        public ParticleSystem chargingParticleSystem;
        public ParticleSystem chargedParticleSystem;


        // -- Class

        public void OnChargeBegin()
        {
            chargingParticleSystem.Play();
        }

        public void OnMinChargeThresholdReached()
        {
            // do nothing
        }

        public void OnFullyCharged()
        {
            chargedParticleSystem.Play();
        }

        public void OnChargeStop()
        {
            chargingParticleSystem.Stop();
            chargingParticleSystem.Clear();

        }

        public void OnChargeClear()
        {
            // do nothing
        }
    }
}