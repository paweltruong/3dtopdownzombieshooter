using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class PlayerCastFireStrikeSystem : CastSpellSystemSystem
{
    protected override void OnUpdate()
    {
        CastSpell(KeyCode.Alpha1, SpellIds.FireStrike, GameDataManager.instance.FireStrikeCooldown);
    }
}
