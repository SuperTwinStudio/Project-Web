using System;
using UnityEngine;

public class ParticleHelper : MonoBehaviour
{

    private Loadout.ClassChanged ChangeActiveWeaponAction;

    private Weapon activeWeapon;

    void Start()
    {
        activeWeapon = Game.Current.Level.Player.Loadout.CurrentWeapon;
        ChangeActiveWeaponAction += ChangeActiveWeapon;
        Game.Current.Level.Player.Loadout.AddOnWeaponChanged(ChangeActiveWeaponAction);
    }

    private void OnDestroy() {
        Game.Current.Level.Player.Loadout.RemoveOnWeaponChanged(ChangeActiveWeaponAction);
        ChangeActiveWeaponAction -= ChangeActiveWeapon;
    }

    private void ChangeActiveWeapon(Weapon oldWeapon, Weapon newWeapon)
    {
        activeWeapon = newWeapon;
    }

    public void EmitParticle(String name)
    {
        activeWeapon.EmitParticle(name);
    }
}
