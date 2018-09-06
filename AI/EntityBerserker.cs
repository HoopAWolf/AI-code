using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBerserker : EntityLivingBase
{

    public override void Awake()
    {
        base.Awake();
        ClearAITask();

        SetResistanceType(Attack_Defence_Type.SLASH);
        SetAttackType(Attack_Defence_Type.BLUNT);
        SetRankType(RANK_TYPE.NORMAL);

        F_MaxHP = 50;
        F_HP = F_MaxHP;
        F_speed = 0.5f;
        F_defence = 20.0f;
        F_damage = 2.0f;
        F_rolling_speed = 1.0f;
        F_mass = 2.0f;
        F_attack_speed = 0.5f;

        F_totalAnimationLength = 0.9f;
        F_defaultAnimationSpeed = 1.0f;
        B_isHit = false;
        B_isGrounded = false;

        // RegisterAITask(new AIIdle(2, this));
        RegisterAITask(new AIAttackMelee(1, this, typeof(EntityPlayer), 2.0f));
        RegisterAITask(new AIDodge(0, this, typeof(EntityPlayer), 3.0f, 40));

        RegisterAITask(new AIChaseTarget(1, this, typeof(EntityPlayer), 20.0f));

        RegisterAITask(new AIRoam(3, this, 5.0f));

        if (GetRankType() == RANK_TYPE.ELITE)
        {
            RegisterAITask(new AIDodge(0, this, typeof(EntityPlayer), 3.0f, 80));
        }

        if (an_animator = GetComponentInChildren<Animator>())
        {
        }
        else
        {
            Debug.LogError("ERROR: There is no animator for character.");
            Destroy(this);
        }

        S_name = "Berserker";
    }

    public override void Update()
    {
        if (!IsDead())
            base.Update();
        else
        {
            F_death_timer += Time.deltaTime;
            GetAnimator().SetBool("DeadTrigger", true);

            if (F_death_timer > 5.0f)
            {
                gameObject.SetActive(false);

                GameObject go = ObjectPool.GetInstance().GetItemObjectFromPool();
                go.GetComponent<EntityPickUp>().SetPosition(GetPosition());
            }
        }
    }

    public override void OnAttack()
    {
        GameObject obj = ObjectPool.GetInstance().GetHitboxObjectFromPool();
        Hitbox obj_hitbox = obj.GetComponent<Hitbox>();

        DamageSource dmgsrc = new DamageSource();

        dmgsrc.SetUpDamageSource(S_name + " " + gameObject.GetInstanceID().ToString(),
            gameObject.tag,
            gameObject.GetInstanceID().ToString(),
            F_damage, 
            GetAttackType());

        obj_hitbox.SetHitbox(dmgsrc, new Vector3(1.5f, 1, 1.5f));

        obj_hitbox.transform.position = transform.position + (transform.forward * (obj_hitbox.transform.localScale * 0.8f).z);
        obj_hitbox.transform.position = new Vector3(obj_hitbox.transform.position.x, obj_hitbox.transform.position.y + 1, obj_hitbox.transform.position.z);

        obj_hitbox.transform.rotation = transform.rotation;
    }

    public void HardReset(RANK_TYPE _type = RANK_TYPE.NORMAL)
    {
        base.HardReset();

        base.Awake();
        ClearAITask();

        SetResistanceType(Attack_Defence_Type.SLASH);
        SetAttackType(Attack_Defence_Type.BLUNT);
        SetRankType(_type);

        F_MaxHP = 50;
        F_HP = F_MaxHP;
        F_speed = 0.5f;
        F_defence = 50.0f;
        F_damage = 30.0f;
        F_rolling_speed = 1.0f;
        F_mass = 2.0f;
        F_attack_speed = 0.5f;

        F_totalAnimationLength = 0.9f;
        F_defaultAnimationSpeed = 1.0f;
        B_isHit = false;
        B_isGrounded = false;

        RegisterAITask(new AIAttackMelee(1, this, typeof(EntityPlayer), 2.0f));
        RegisterAITask(new AIDodge(0, this, typeof(EntityPlayer), 3.0f, 40));

        RegisterAITask(new AIChaseTarget(1, this, typeof(EntityPlayer), 20.0f));

        RegisterAITask(new AIRoam(3, this, 5.0f));

        if (GetRankType() == RANK_TYPE.ELITE)
        {
            RegisterAITask(new AIDodge(0, this, typeof(EntityPlayer), 3.0f, 80));
        }

        S_name = "Berserker";
    }

    public override void HardReset()
    {
        base.HardReset();

        base.Awake();
        ClearAITask();

        SetResistanceType(Attack_Defence_Type.SLASH);
        SetAttackType(Attack_Defence_Type.BLUNT);
        SetRankType(RANK_TYPE.NORMAL);

        F_MaxHP = 50;
        F_HP = F_MaxHP;
        F_speed = 0.5f;
        F_defence = 50.0f;
        F_damage = 30.0f;
        F_rolling_speed = 1.0f;
        F_mass = 2.0f;
        F_attack_speed = 0.5f;

        F_totalAnimationLength = 0.9f;
        F_defaultAnimationSpeed = 1.0f;
        B_isHit = false;
        B_isGrounded = false;

        RegisterAITask(new AIAttackMelee(1, this, typeof(EntityPlayer), 2.0f));
        RegisterAITask(new AIDodge(0, this, typeof(EntityPlayer), 3.0f, 40));

        RegisterAITask(new AIChaseTarget(1, this, typeof(EntityPlayer), 20.0f));

        RegisterAITask(new AIRoam(3, this, 5.0f));

        if (GetRankType() == RANK_TYPE.ELITE)
        {
            RegisterAITask(new AIDodge(0, this, typeof(EntityPlayer), 3.0f, 80));
        }

        S_name = "Berserker";
    }
}
