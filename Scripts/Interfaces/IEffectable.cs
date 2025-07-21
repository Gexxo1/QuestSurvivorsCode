using System.Collections.Generic;

public interface IEffectable
{
    /* questa interfaccia viene distinta da IHittable,
       poichè un oggetto tipo la pot non dovrebbe subire gli status effect
    */
    public void ApplyMultipleStatus(List<StatusEffectData> statusEffects);
    public void ApplyStatus(StatusEffectData statusEffect);
    public void RemoveStatus(StatusEffectData statusEffect);
    public void HandleStatus();
}