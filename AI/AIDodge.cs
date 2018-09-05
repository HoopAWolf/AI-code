using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDodge : AIBase
{
    private Vector3
           v3_target_position;

    private System.Type
        type_target;

    private float
        f_range;

    private int
        i_chance;

    public AIDodge(int _priority, EntityLivingBase _entity, System.Type _type, float _range, int _chance)
    {
        i_chance = _chance;
        f_range = _range;
        i_priority = _priority;
        ent_main = _entity;
        type_target = _type;
        s_ID = "Combat";
        s_display_name = "Dodge From Target - " + type_target;
        b_is_interruptable = true;

    }

    public override bool StartAI()
    {
        ent_target = null;
        return true;
    }

    public override bool ShouldContinueAI()
    {
        //if (ent_main.F_HP > ((ent_main.F_MaxHP * 0.3f)))
        //    return false;

        if((Random.Range(0, 100) > i_chance) && !ent_main.B_isDodging)
        {
            return false;
        }

        if (ent_target == null)
        {
            foreach (var list in ObjectPool.GetInstance().GetAllEntity())
            {
                foreach (GameObject l_go in list)
                {
                    if (type_target.Equals(l_go.GetComponent<EntityLivingBase>().GetType()))
                    {
                        if (!l_go.GetComponent<EntityLivingBase>().IsDead())
                        {
                            if (ent_target == null)
                            {
                                if (Vector3.Distance(ent_main.GetPosition(), l_go.transform.position) < f_range)
                                {
                                    ent_target = l_go.GetComponent<EntityLivingBase>();
                                    // v3_target_position = ent_entity_targetable.GetPosition();
                                }
                            }
                            else
                            {
                                if (Vector3.Distance(ent_main.GetPosition(), ent_target.transform.position) > Vector3.Distance(ent_main.GetPosition(), l_go.transform.position))
                                {
                                    ent_target = l_go.GetComponent<EntityLivingBase>();
                                    //  v3_target_position = ent_entity_targetable.GetPosition();
                                }
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        if (ent_target != null && ent_target.IsDead())
        {
            ent_target = null;
        }

        if (ent_target == null)
        {
            return false;
        }

        if (!(ent_main.GetPosition().x > ent_target.GetPosition().x - f_range && ent_main.GetPosition().x < ent_target.GetPosition().x + f_range
            &&
            ent_main.GetPosition().z > ent_target.GetPosition().z - f_range && ent_main.GetPosition().z < ent_target.GetPosition().z + f_range)
            || ent_main.B_isAttacking
            || !ent_main.B_isGrounded)
        {
            EndAI();
            return false;
        }

        if (ent_target != null)
        {
           // Debug.Log(ent_target.B_isAttacking.ToString());
            if (!ent_target.B_isAttacking)
            {
                EndAI();
                return false;
            }

            if (ent_main.GetAnimator().GetBool("DodgeTrigger") && ent_main.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= (ent_main.F_totalAnimationLength * 0.9f))
            {
                EndAI();
                return false;
            }
        }


        ent_main.GetAnimator().SetBool("DodgeTrigger", true);
        ent_main.GetAnimator().speed = ent_main.F_rolling_speed;
        return true;
    }

    public override bool RunAI()
    {
        ent_main.B_isDodging = true;
        return true;
    }

    public override bool EndAI()
    {
        ent_main.GetAnimator().SetBool("DodgeTrigger", false);
        ent_main.GetAnimator().speed = ent_main.F_defaultAnimationSpeed;
        ent_main.B_isDodging = false;

        StartAI();
        return true;
    }
}
