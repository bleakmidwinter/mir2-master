using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;
using System.Drawing;

namespace Server.MirObjects.Monsters
{
    class HellKeeper2 : MonsterObject
    {
        protected override bool CanMove { get { return false; } }
        //protected override bool CanRegen { get { return false; } }

        public long PullTime;
        public bool Range;
        private long MassAttackTime;
        private bool rage30 = false, rage50 = false;
        private List<MapObject> targetsInRange;

        protected internal HellKeeper2(MonsterInfo info) : base(info)
        {
            Direction = MirDirection.Up;
            PullTime = Envir.Time;
        }

        protected bool CanPull
        {
            get
            {
                return Range && Envir.Time >= PullTime;
            }
        }

        protected override bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;
            if (Target.CurrentLocation == CurrentLocation) return false;

            if (!Functions.InRange(CurrentLocation, Target.CurrentLocation, Info.ViewRange))
                return false;

            int x = Math.Abs(Target.CurrentLocation.X - CurrentLocation.X);
            int y = Math.Abs(Target.CurrentLocation.Y - CurrentLocation.Y);

            Range = x > 1 || y > 1;

            return (!Range || CanPull) ? true : false;
        }

        public override void Turn(MirDirection dir)
        {
        }
        public override bool Walk(MirDirection dir) { return false; }


        //public override int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility)
        //{
        //    int armour = 0;

        //    switch (type)
        //    {
        //        case DefenceType.ACAgility:
        //            if (Envir.Random.Next(Agility + 1) > attacker.Accuracy) return 0;
        //            armour = GetDefencePower(MinAC, MaxAC);
        //            break;
        //        case DefenceType.AC:
        //            armour = GetDefencePower(MinAC, MaxAC);
        //            break;
        //        case DefenceType.MACAgility:
        //            if (Envir.Random.Next(Agility + 1) > attacker.Accuracy) return 0;
        //            armour = GetDefencePower(MinMAC, MaxMAC);
        //            break;
        //        case DefenceType.MAC:
        //            armour = GetDefencePower(MinMAC, MaxMAC);
        //            break;
        //        case DefenceType.Agility:
        //            if (Envir.Random.Next(Agility + 1) > attacker.Accuracy) return 0;
        //            break;
        //    }

        //    if (armour >= damage) return 0;

        //    ShockTime = 0;

        //    for (int i = PoisonList.Count - 1; i >= 0; i--)
        //    {
        //        if (PoisonList[i].PType != PoisonType.LRParalysis) continue;

        //        PoisonList.RemoveAt(i);
        //        OperateTime = 0;
        //    }

        //    if (attacker.Info.AI == 6)
        //        EXPOwner = null;
        //    else if (attacker.Master != null)
        //    {
        //        if (EXPOwner == null || EXPOwner.Dead)
        //            EXPOwner = attacker.Master;

        //        if (EXPOwner == attacker.Master)
        //            EXPOwnerTime = Envir.Time + EXPOwnerDelay;

        //    }

        //    Broadcast(new S.ObjectStruck { ObjectID = ObjectID, AttackerID = attacker.ObjectID, Direction = Direction, Location = CurrentLocation });

        //    ChangeHP(armour - damage);
        //    return 1;
        //}
        //public override int Attacked(PlayerObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true)
        //{
        //    int armour = 0;

        //    switch (type)
        //    {
        //        case DefenceType.ACAgility:
        //            if (Envir.Random.Next(Agility + 1) > attacker.Accuracy) return 0;
        //            armour = GetDefencePower(MinAC, MaxAC);
        //            break;
        //        case DefenceType.AC:
        //            armour = GetDefencePower(MinAC, MaxAC);
        //            break;
        //        case DefenceType.MACAgility:
        //            if (Envir.Random.Next(Agility + 1) > attacker.Accuracy) return 0;
        //            armour = GetDefencePower(MinMAC, MaxMAC);
        //            break;
        //        case DefenceType.MAC:
        //            armour = GetDefencePower(MinMAC, MaxMAC);
        //            break;
        //        case DefenceType.Agility:
        //            if (Envir.Random.Next(Agility + 1) > attacker.Accuracy) return 0;
        //            break;
        //    }

        //    if (armour >= damage) return 0;

        //    if (damageWeapon)
        //        attacker.DamageWeapon();

        //    ShockTime = 0;

        //    for (int i = PoisonList.Count - 1; i >= 0; i--)
        //    {
        //        if (PoisonList[i].PType != PoisonType.LRParalysis) continue;

        //        PoisonList.RemoveAt(i);
        //        OperateTime = 0;
        //    }

        //    if (Master != null && Master != attacker)
        //        if (Envir.Time > Master.BrownTime && Master.PKPoints < 200)
        //            attacker.BrownTime = Envir.Time + Settings.Minute;

        //    if (EXPOwner == null || EXPOwner.Dead)
        //        EXPOwner = attacker;

        //    if (EXPOwner == attacker)
        //        EXPOwnerTime = Envir.Time + EXPOwnerDelay;

        //    Broadcast(new S.ObjectStruck { ObjectID = ObjectID, AttackerID = attacker.ObjectID, Direction = Direction, Location = CurrentLocation });
        //    attacker.GatherElement();

        //    ChangeHP(armour - damage);
        //    return 1;
        //}

        public override void ApplyPoison(Poison p, MapObject Caster = null, bool NoResist = false, bool ignoreDefence = true)
        {

        }
        protected override void ProcessTarget()
        {
            if (!CanAttack) return;

            targetsInRange = FindAllTargets(Info.ViewRange, CurrentLocation);
            if (targetsInRange.Count == 0) return;

            ShockTime = 0;

            for (int i = 0; i < targetsInRange.Count; i++)
            {
                //Target = targets[i];

                int x = Math.Abs(targetsInRange[i].CurrentLocation.X - CurrentLocation.X);
                int y = Math.Abs(targetsInRange[i].CurrentLocation.Y - CurrentLocation.Y);

                if ((x > 1 || y > 1) && (Envir.Random.Next(3) == 0) && Envir.Time > PullTime)
                {
                    //Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

                    //Pull all targets in
                    PullAttack();
                    return;
                }
            }

            Attack();
        }

        protected override void Attack()
        {
            //if (!Target.IsAttackTarget(this))
            //{
            //    Target = null;
            //    return;
            //}

            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });
            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;
            //PullTime = Envir.Time + 5000;

            if (((HP * 100 / MaxHP) < 50) && (MassAttackTime < Envir.Time) && (!rage50))
            {
                Rage();
                rage50 = true;
                return;
            }

            if (((HP * 100 / MaxHP) < 30) && (MassAttackTime < Envir.Time) && (!rage30))
            {
                Rage();
                rage30 = true;
                return;
            }

            int damage = GetAttackPower(MinDC, MaxDC);

            if (damage == 0) return;

            //Target.Attacked(this, damage);
            //Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });

            foreach (MapObject ob in targetsInRange)
            {
                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 350, ob, damage, DefenceType.ACAgility);
                ActionList.Add(action);

                if (Envir.Random.Next(10) == 0)
                {
                    ob.ApplyPoison(new Poison { Owner = this, Duration = 5, PType = PoisonType.Stun, TickSpeed = 1000 }, this);
                }
            }


        }

        private void Rage()
        {
            //ShockTime = 0;
            //ActionTime = Envir.Time + 500;
            //AttackTime = Envir.Time + (AttackSpeed);

            //Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });

            targetsInRange = FindAllTargets(15, CurrentLocation, false);

            if (targetsInRange.Count == 0) return;

            int damage = GetAttackPower(MinDC, MaxDC) * 4;

            for (int i = 0; i < targetsInRange.Count; i++)
            {
                //Target = targets[i];

                int delay = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) * 50 + 750; //50 MS per Step

                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, targetsInRange[i], damage, DefenceType.ACAgility);
                ActionList.Add(action);

                if (Envir.Random.Next(1) == 0)
                {
                    targetsInRange[i].ApplyPoison(new Poison { Owner = this, Duration = 5, PType = PoisonType.Paralysis, TickSpeed = 1000 }, this);
                }
            }

            //Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
            //ActionTime = Envir.Time + 300;
            //AttackTime = Envir.Time + AttackSpeed;
            //PullTime = Envir.Time + 5000;

            //MassAttackTime = Envir.Time + 2000 + (Envir.Random.Next(5) * 1000);
            //ActionTime = Envir.Time + 800;
            //AttackTime = Envir.Time + (AttackSpeed);
        }

        private void PullAttack()
        {
            if (targetsInRange == null || targetsInRange.Count == 0)
                return;

            int damage = GetAttackPower(MinDC, MaxDC) * 2;

            foreach (MapObject ob in targetsInRange)
            {
                MirDirection pushdir = Functions.DirectionFromPoint(ob.CurrentLocation, CurrentLocation);
                if (Envir.Random.Next(Settings.MagicResistWeight) < Target.MagicResist) return;
                int distance = Functions.MaxDistance(ob.CurrentLocation, CurrentLocation) - 1;
                //if (distance <= 0) return;
                //if (distance > 4) distance = 4;

                ob.Pushed(this, pushdir, distance);

                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 350, ob, damage, DefenceType.ACAgility);
                ActionList.Add(action);

                if (Envir.Random.Next(1) == 0)
                        ob.ApplyPoison(new Poison { Owner = this, Duration = 5, PType = PoisonType.Paralysis, TickSpeed = 1000 }, this);
            }

            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;
            PullTime = Envir.Time + 5000;
        }

        private void HypnoAttack()
        {
            int damage = GetAttackPower(MinMC, MaxMC);
            if (damage == 0) return;

            if (Target.Attacked(this, damage, DefenceType.MACAgility) <= 0) return;

            if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.PoisonResist)
            {
                if (Envir.Random.Next(10) == 0)
                {
                    Target.ApplyPoison(new Poison { Owner = this, Duration = GetAttackPower(MinMC, MaxMC), PType = PoisonType.Stun, TickSpeed = 1000 }, this);
                }
            }
        }

        protected override void ProcessRoam() { }
    }
}
