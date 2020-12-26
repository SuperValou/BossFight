namespace Assets.Scripts.Weaponry.Charges
{
    public interface IChargeNotifiable
    {
        void OnChargeBegin();
        void OnMinChargeThresholdReached();
        void OnFullyCharged();
        void OnChargeStop();
        void OnChargeClear();
    }
}