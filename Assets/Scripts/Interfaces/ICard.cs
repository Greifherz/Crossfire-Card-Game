using System.Collections.Generic;

public interface ICard : IPlayable
{
    List<Damage> GetDamage();
    void OnPlayed();
    void OnUnplayed();
}