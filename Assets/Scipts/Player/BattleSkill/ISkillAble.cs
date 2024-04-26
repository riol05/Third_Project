using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillAble
{
    public void Damage();
    public void SkillReady();
    public void isCoolDownNow();
    public void SkillActivate();
}
