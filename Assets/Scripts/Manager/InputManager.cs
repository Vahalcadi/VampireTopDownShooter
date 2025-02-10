using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : SingletonA<InputManager>
{
    private PlayerControls playerControls;

    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();

    }

    public Vector2 Movement()
    {
        return playerControls.Player.Movement.ReadValue<Vector2>();
    }

    public bool Shoot()
    {
        if ((bool)Weapon.CanShootRepeatedly?.Invoke())
            return Input.GetMouseButton(0);
        else
            return Input.GetMouseButtonDown(0);
    }

    public bool Aim()
    {
        return Input.GetMouseButton(1);
    }

    public float ChangeWeapon()
    {
        return playerControls.Player.ScrollWeapons.ReadValue<float>();
    }

    public bool Dash()
    {
        return playerControls.Player.Dash.triggered;
    }

    public bool MeleeAttack()
    {
        return playerControls.Player.MeleeAttack.triggered;
    }

    public bool Reload()
    {
        return playerControls.Player.Reload.triggered;
    }

    public bool Grenade()
    {
        return playerControls.Player.Grenade.triggered;
    }

    public bool Interact()
    {
        return playerControls.Player.Interact.triggered;
    }

    public bool SkipWave()
    {
        return playerControls.Player.SkipWave.triggered;
    }

    public void OnEnable()
    {
        Weapon.Aiming += Aim;
        playerControls.Enable();
    }

    public void OnDisable()
    {
        Weapon.Aiming -= Aim;
        playerControls.Disable();
    }
}
