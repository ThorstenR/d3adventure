using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3_Adventures.Structures;
using D3_Adventures;

namespace D3Bloader.Game
{
    public static class Helpers
    {
        public static  bool hasValidTarget(Bot owner)
        {
            if (!owner.CurrentTarget.HasValue) return false;
            else if (!owner.CurrentTarget.Value.Exists() || owner.CurrentTarget.Value.id_acd.ToString("X") == "FFFFFFFF")
            {
                if (owner.isAttacking)
                {
                    Log.write("target dead");
                }

                owner.CurrentTarget = null;
                return false;
            }
            else
            {
                if (owner.CurrentTarget.Value.distanceFromMe >= 50 || Math.Abs(owner.CurrentTarget.Value.Pos1.z - Data.getCurrentPos().z) > 5)
                {
                    Log.write("LOST TARGET");
                    owner.CurrentTarget = null;
                    return false;
                }
            }

            return true;
        }

        //THIS NEEDS A LOT OF WORK.
        public static Actor? getNearestEnemy()
        {
            var monsters = Data.getMonsters();

            Actor? m = null;

            //should already be sorted by distance.
            foreach (var monster in monsters)
            {
                if (monster.unknown_data2 == 29944 && monster.id_acd != Data.toonID && Math.Abs(monster.Pos1.z - Data.getCurrentPos().z) < 5 &&
                    monster.Exists() && monster.distanceFromMe != 0.0 && monster.distanceFromMe < 50 && monster.id_acd.ToString("X") != "FFFFFFFF")
                {
                    if (monster.name.ToLower().Contains("leah")) continue;
                    if (monster.name.ToLower().Contains("floor")) continue;

                    Log.write("FOUND ENEMY {0}", monster.name);
                    m = monster;
                    break;
                }
            }

            return m;
        }

        public static bool isInAction(Bot owner)
        {
            if (!owner.isMoving && !hasValidTarget(owner))
            {
                owner.isAttacking = false;
                return false;
            }

            if (owner.isMoving)
            {
                Vec3 pos = Data.getCurrentPos();

//                 float xd = _vMovingTo.x - pos.x;
//                 float yd = _vMovingTo.y - pos.y;
//                 float zd = _vMovingTo.z - pos.z;
//                 double distance = Math.Sqrt(pos.x * pos.x + pos.y * pos.y + pos.z * pos.z);
// 
//                 if (distance < 2)
//                 {
//                     _bMoving = false;
//                     return false;
//                 }

                return true;
            }
            else if (owner.isAttacking)
            {
                if (!Helpers.hasValidTarget(owner))
                {
                    owner.isAttacking = false;
                    return false;
                }

                Vec3 pos = Data.getCurrentPos();
                double distance = Math.Sqrt(pos.x * pos.x + pos.y * pos.y + pos.z * pos.z);
                if (distance < 2 || Globals.mem.ReadMemoryAsFloat(Offsets.clickToMoveToggle) == 0)
                {
                    owner.isAttacking = false;
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}
