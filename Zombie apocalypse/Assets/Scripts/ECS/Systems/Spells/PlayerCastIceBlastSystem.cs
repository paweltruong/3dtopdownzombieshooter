using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class PlayerCastIceBlastSystem : CastSpellSystemSystem
{
    protected override void OnUpdate()
    {
        CastSpell(KeyCode.Alpha2, SpellIds.IceBlast, GameDataManager.instance.IceBlastCooldown);
    }
}
