using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityLivingBase : BaseGameEntity {

    public enum Attack_Defence_Type
    {
        NONE,

        BLUNT,
        SLASH,
        STAB,

        TOTAL_RESISTANCE
    }

    public enum RANK_TYPE
    {
        NORMAL,

        ELITE,
        BOSS,

        TOTAL_RANK_NUMBER
    }

    [SerializeField]
    private float
        f_HP,
        f_MaxHP,
        f_speed,
        f_defence,
        f_damage,
        f_mass,
        f_attack_speed,
        f_rolling_speed,

        f_hitTimer, 
        f_AI_task_change_timer, 
        f_regenTimer, 
        
        f_ground_height,
        f_death_timer;

    protected Animator
        an_animator;

    private float
         f_defaultAnimationSpeed,
         f_totalAnimationLength;

    private Attack_Defence_Type
        m_resistancetype,
        m_attacktype;

    private RANK_TYPE
        m_ranktype;

    private RaycastHit 
        _raycast;

    private bool
        b_isHit, 
        b_isGrounded,
        b_isAttacking, 
        b_isDodging;

    private string
        s_last_hit, 
        s_name;

    private Dictionary<string, SortedDictionary<int, List<AIBase>>>
        dic_AI_list;

    private Dictionary<string, AIBase>
       dic_running_AI_list;

    public virtual void Awake()
    {
        dic_AI_list = new Dictionary<string, SortedDictionary<int, List<AIBase>>>();
        dic_running_AI_list = new Dictionary<string, AIBase>();
        _raycast = new RaycastHit();

        f_AI_task_change_timer = 0.0f;
        F_damage = 0.0f;
    }

    public float F_rolling_speed
    {
        get
        {
            return f_rolling_speed;
        }

        set
        {
            f_rolling_speed = value;
        }
    }

    public float F_HP
    {
        get
        {
            return f_HP;
        }

        set
        {
            f_HP = value;

            if (f_HP > F_MaxHP)
                f_HP = F_MaxHP;
            else if (f_HP < 0)
                f_HP = 0;
        }
    }

    public virtual float F_MaxHP
    {
        get
        {
            return f_MaxHP;
        }

        set
        {
            f_MaxHP = value;

            if (f_MaxHP > int.MaxValue)
                f_MaxHP = int.MaxValue;
            else if (f_MaxHP < 0)
                f_MaxHP = 0;
        }
    }

    public virtual float F_speed
    {
        get
        {
            return f_speed;
        }

        set
        {
            f_speed = value;

            if (f_speed > int.MaxValue)
                f_speed = int.MaxValue;
            else if (f_speed < 0)
                f_speed = 0;
        }
    }

    public virtual float F_defence
    {
        get
        {
            return f_defence;
        }

        set
        {
            f_defence = value;

            if (f_defence > 100)
                f_defence = 100;
            else if (f_defence < 0)
                f_defence = 0;
        }
    }

    public virtual float F_damage
    {
        get
        {
            return f_damage;
        }

        set
        {
            f_damage = value;

            if (f_damage > int.MaxValue)
                f_damage = int.MaxValue;
            else if (f_damage < 0)
                f_damage = 0;
        }
    }

    public virtual float F_mass
    {
        get
        {
            return f_mass;
        }

        set
        {
            f_mass = value;

            if (f_mass > int.MaxValue)
                f_mass = int.MaxValue;
            else if (f_mass < 0)
                f_mass = 0;
        }
    }

    public float F_hitTimer
    {
        get
        {
            return f_hitTimer;
        }

        set
        {
            f_hitTimer = value;
        }
    }

    public bool B_isHit
    {
        get
        {
            return b_isHit;
        }

        set
        {
            b_isHit = value;
        }
    }

    public bool B_isGrounded
    {
        get
        {
            return b_isGrounded;
        }

        set
        {
            b_isGrounded = value;
        }
    }

    public bool B_isAttacking
    {
        get
        {
            return b_isAttacking;
        }

        set
        {
            b_isAttacking = value;
        }
    }

    public float F_defaultAnimationSpeed
    {
        get
        {
            return f_defaultAnimationSpeed;
        }

        set
        {
            f_defaultAnimationSpeed = value;
        }
    }

    public float F_totalAnimationLength
    {
        get
        {
            return f_totalAnimationLength;
        }

        set
        {
            f_totalAnimationLength = value;
        }
    }

    public string S_last_hit
    {
        get
        {
            return s_last_hit;
        }

        set
        {
            s_last_hit = value;
        }
    }

    public string S_name
    {
        get
        {
            return s_name;
        }

        set
        {
            s_name = value;
        }
    }

    public float F_death_timer
    {
        get
        {
            return f_death_timer;
        }

        set
        {
            f_death_timer = value;
        }
    }

    public virtual float F_attack_speed
    {
        get
        {
            return f_attack_speed;
        }

        set
        {
            f_attack_speed = value;
        }
    }

    public float F_regenTimer
    {
        get
        {
            return f_regenTimer;
        }

        set
        {
            f_regenTimer = value;
        }
    }

    public bool B_isDodging
    {
        get
        {
            return b_isDodging;
        }

        set
        {
            b_isDodging = value;
        }
    }

    public Attack_Defence_Type GetResistanceType()
    {
        return m_resistancetype;
    }

    public Attack_Defence_Type GetAttackType()
    {
        return m_attacktype;
    }

    public RANK_TYPE GetRankType()
    {
        return m_ranktype;
    }

    public Animator GetAnimator()
    {
        return an_animator;
    }

    public bool IsDead()
    {
        return F_HP <= 0;
    }

    public void SetResistanceType(Attack_Defence_Type _type)
    {
        m_resistancetype = _type;
    }

    public void SetAttackType(Attack_Defence_Type _type)
    {
        m_attacktype = _type;
    }

    public void SetRankType(RANK_TYPE _type)
    {
        m_ranktype = _type;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetPosition(Vector3 _input)
    {
        transform.position = _input;
    }

    public void RegisterAITask(AIBase _ai)
    {
        if(dic_AI_list == null)
            dic_AI_list = new Dictionary<string, SortedDictionary<int, List<AIBase>>>();

        if (!dic_AI_list.ContainsKey(_ai.GetID()))
        {
            dic_AI_list.Add(_ai.GetID(), new SortedDictionary<int, List<AIBase>>());
        }

        if (!dic_AI_list[_ai.GetID()].ContainsKey(_ai.GetPriority()))
        {
            dic_AI_list[_ai.GetID()].Add(_ai.GetPriority(), new List<AIBase>());
        }

        dic_AI_list[_ai.GetID()][_ai.GetPriority()].Add(_ai);
    }

    public void ClearAITask()
    {
        dic_AI_list.Clear();
    }

    // Update is called once per frame
    public virtual void Update ()
    {
        f_AI_task_change_timer += Time.deltaTime;
        F_regenTimer += Time.deltaTime;

        if (dic_AI_list.Count > 0)
        {
         //   if (f_AI_task_change_timer > 1.0f)
         //   {
                foreach (var dic1 in dic_AI_list)
                {
                    foreach (var dic2 in dic1.Value)
                    {
                        bool done = false;
                        foreach (AIBase ai in dic2.Value)
                        {
                            if (dic_running_AI_list.ContainsKey(ai.GetID()) && dic_running_AI_list[ai.GetID()] != null)
                            {
                                if (!dic_running_AI_list[ai.GetID()].ShouldContinueAI())
                                {
                                    dic_running_AI_list[ai.GetID()].EndAI();
                                    dic_running_AI_list[ai.GetID()] = null;
                                }

                                if (dic_running_AI_list[ai.GetID()] == null || (dic_running_AI_list[ai.GetID()].GetPriority() > ai.GetPriority() && dic_running_AI_list[ai.GetID()].GetIsInteruptable()))
                                {
                                    if (ai.ShouldContinueAI())
                                    {
                                        if (dic_running_AI_list[ai.GetID()] != null)
                                            dic_running_AI_list[ai.GetID()].EndAI();

                                        dic_running_AI_list[ai.GetID()] = ai;
                                        dic_running_AI_list[ai.GetID()].StartAI();

                                        done = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (ai.ShouldContinueAI())
                                {
                                    if (!dic_running_AI_list.ContainsKey(ai.GetID()))
                                        dic_running_AI_list.Add(ai.GetID(), ai);
                                    else
                                        dic_running_AI_list[ai.GetID()] = ai;

                                    dic_running_AI_list[ai.GetID()].StartAI();

                                    done = true;
                                    break;
                                }
                            }
                        }

                        if (done)
                            break;
                    }
                }

                f_AI_task_change_timer = 0.0f;
          //  }
        }

        if(dic_running_AI_list.Count > 0)
        {
            foreach(var dic in dic_running_AI_list)
            {
                if (dic.Value != null)
                {
                    dic.Value.RunAI();
                 //   Debug.Log("Size of AI Task: " + dic_running_AI_list.Count);
                    //Debug.Log("Running AI of: " + dic.Value.GetID() + " - " + dic.Value.GetDisplayName() +  " With priority: " + dic.Value.GetPriority());
                }
            }   
        }

        if (!IsDead())
        {
            if (F_regenTimer > 2.0f)
            {
                F_regenTimer = 0.0f;

                F_HP += 1;
            }
        }

        if (B_isHit)
        {
            F_regenTimer = 0;
            F_hitTimer -= Time.deltaTime;

            if (F_hitTimer <= 0)
                B_isHit = false;
        }

        UpdateYOffset();
    }

    public virtual void OnAttack()
    {
        GameObject obj = ObjectPool.GetInstance().GetHitboxObjectFromPool();
        Hitbox obj_hitbox = obj.GetComponent<Hitbox>();

        DamageSource dmgsrc = new DamageSource();

        dmgsrc.SetUpDamageSource("Name",
            gameObject.tag,
            gameObject.GetInstanceID().ToString(),
            F_damage, 
            GetAttackType());

        obj_hitbox.SetHitbox(dmgsrc, new Vector3(2, 1, 2));

        obj_hitbox.transform.position = transform.position + (transform.forward * (obj_hitbox.transform.localScale * 0.5f).z);
        obj_hitbox.transform.rotation = transform.rotation;
    }

    public virtual void OnAttacked(DamageSource _damagesource, float _timer = 0.5f)
    {
        if (!B_isHit && !B_isDodging)
        {
            S_last_hit = _damagesource.GetName();
            F_HP -= (_damagesource.GetDamage() * ((100 - F_defence) / 100)) * ((GetResistanceType() == _damagesource.GetAttackType()) ? 0.9f : 1);
            ResetOnHit(_timer);

         //   Debug.Log("Attacked by: " + S_last_hit);
        }
    }

    public virtual void MoveTowardsTarget(EntityLivingBase _target)
    {        
            SetPosition(new Vector3(
           GetPosition().x + (((GetPosition().x - _target.GetPosition().x) < 0 ? -F_speed : F_speed) * Time.deltaTime),
           GetPosition().y,
           GetPosition().z + (((GetPosition().z - _target.GetPosition().z) < 0 ? -F_speed : F_speed)) * Time.deltaTime));

    }

    public virtual void MoveTowardsPosition(Vector3 _pos)
    {
            SetPosition(new Vector3(
                GetPosition().x + (((_pos.x - GetPosition().x) < 0 ? -F_speed : F_speed) * Time.deltaTime),
                GetPosition().y,
                GetPosition().z + (((_pos.z - GetPosition().z) < 0 ? -F_speed : F_speed)) * Time.deltaTime));


    }

    public void RotateTowardsTargetPosition(Vector3 _pos)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,
     Quaternion.LookRotation(_pos - GetPosition()), F_speed * Time.deltaTime * 10);
    }

    public virtual void HardReset()
    {
        dic_AI_list = new Dictionary<string, SortedDictionary<int, List<AIBase>>>();
        dic_running_AI_list = new Dictionary<string, AIBase>();
        _raycast = new RaycastHit();

        F_death_timer = 0.0f;
        f_AI_task_change_timer = 0.0f;
    }

    public void ResetOnHit(float _timer = 0.5f)
    {
        F_hitTimer = _timer;
        B_isHit = true;
    }

    public virtual void UpdateYOffset()
    {
        float _gravity = -9.8f;      

            if (Physics.Raycast(
                GetPosition(),
                -gameObject.transform.up,
                out _raycast
            ))
        {
            f_ground_height = _raycast.point.y;
        }
 
        if (System.Math.Round(f_ground_height, 2) == System.Math.Round(GetPosition().y, 2))
        {
            B_isGrounded = true;
           // Debug.Log("Grounded");
        }
        else
        {
            B_isGrounded = false;
           // Debug.Log("Not Grounded");
        }

        if (!B_isGrounded)
            SetPosition(new Vector3(GetPosition().x, GetPosition().y + (_gravity * F_mass * Time.deltaTime), GetPosition().z));

        if (GetPosition().y < f_ground_height)
        {
            SetPosition(new Vector3(GetPosition().x, f_ground_height, GetPosition().z));
        } 

    }
}
