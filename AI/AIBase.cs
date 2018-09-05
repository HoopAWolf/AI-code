using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBase {

    protected EntityLivingBase
        ent_main, 
        ent_target;

    protected string
        s_ID, 
        s_display_name;

    protected int
        i_priority;

    protected bool
        b_is_interruptable;

    public string GetID()
    {
        return s_ID;
    }

    public string GetDisplayName()
    {
        return s_display_name;
    }

    public int GetPriority()
    {
        return i_priority;
    }

    public bool GetIsInteruptable()
    {
        return b_is_interruptable;
    }

    public EntityLivingBase GetTarget()
    {
        return ent_target;
    }

    virtual public bool StartAI()
    {
        return false;
    }

    virtual public bool ShouldContinueAI()
    {
        return false;
    }

    virtual public bool RunAI()
    {
        return false;
    }

    virtual public bool EndAI()
    {
        return false;
    }
}
