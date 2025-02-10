public class PlayerReloadState : PlayerState
{
    WeaponType weaponType;
    int audioIndex = 0;

    public PlayerReloadState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        AmmoInfos ammoInfos = Weapon.AmmoInfos?.Invoke();
        weaponType = (WeaponType)Weapon.OnWeaponType?.Invoke();

        if (ammoInfos.CurrentMagazineAmmo == ammoInfos.MaxMagazine)
            stateMachine.ChangeState(player.MoveState);
        else
        {
            stateTimer = ammoInfos.ReloadTime;
            ReloadSFXIndex();
            AudioManager.Instance.PlaySFX(audioIndex, player.transform);
        }        
    }

    public override void Exit()
    {
        AudioManager.Instance.StopSFX(audioIndex);
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.ReloadingMovement();

        if (stateTimer < 0)
        {
            player.Reload();
            stateMachine.ChangeState(player.MoveState);
        }
    }

    private void ReloadSFXIndex()
    {
        switch (weaponType)
        {
            case WeaponType.Pistol:
                audioIndex = 9;
                break;
            case WeaponType.AutoRifle:
                audioIndex = 18;
                break;
            case WeaponType.Shotgun:
                audioIndex = 10;
                break;
            case WeaponType.Sniper:
                audioIndex = 19;
                break;
            case WeaponType.Minigun:
                break;
        }
    }
}
