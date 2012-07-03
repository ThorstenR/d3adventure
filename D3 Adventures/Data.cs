using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using D3_Adventures.Structures;
using D3_Adventures.Memory_Handling;
using D3_Adventures.Enumerations;

namespace D3_Adventures
{
    public static class Data
    {
        private static MemoryManager mem = Globals.mem;

        public static uint toonID = 0x77BC0000; // your toon's guid

        public static Actor GetMe()
        {
            return ActorByGUID(toonID);
        }

        /// <summary>
        /// Returns the given Actor by it's unique ID from a fresh IterateActors()
        /// </summary>
        /// <param name="guid">Unique ID of the actor you would like to find in the collection</param>
        /// <returns>Matching Actor</returns>
        public static Actor ActorByGUID(uint guid)
        {
            return ActorByGUID(IterateActors(), guid);
        }

        /// <summary>
        /// Returns the given Actor by it's unique ID from the given collection of Actors
        /// </summary>
        /// <param name="actors">A collection of actors</param>
        /// <param name="guid">Unique ID of the actor you would like to find in the collection</param>
        /// <returns>Matching Actor</returns>
        public static Actor ActorByGUID(Actor[] actors, uint guid)
        {
            return actors.Where(o => o.id_actor == guid).FirstOrDefault();
        }

        public static uint GetActorCount()
        {
            uint count = mem.ReadMemoryAsUint(Offsets.itrObjectManagerCount);
            if (Globals.debugMessages) Console.WriteLine("Number of Actors: " + count);
            return count;
        }

        public static uint GetLocalActorCount()
        {
            return Offsets.LocalActor_Count;
        }

        public static uint GetAttributeCount()
        {
            return mem.ReadMemoryAsUint(Offsets.ActorAtrib_Count);
        }

        /*;;================================================================================
        ; Function:			GetCurrentPos($_offset)
        ; Description:		Returns your current offset in a array
        ; Parameter(s):		none
        ;								 
        ; Note(s):			This will return your current position as a array.
        ;					[0] = X, [1] = Y, [2] = Z
        ;==================================================================================*/
        public static Vec3 GetCurrentPos()
        {
            Vec3 ret;
            ret.x = mem.ReadMemoryAsFloat(Offsets.clickToMoveCurX);
            ret.y = mem.ReadMemoryAsFloat(Offsets.clickToMoveCurY);
            ret.z = mem.ReadMemoryAsFloat(Offsets.clickToMoveCurZ);
             
            // if all three == 0 then the user needs to click first
            //  could throw an error or just send a click to the center of the window
            return ret;//Globals.Me.Pos1;//
        }

        public static Actor[] IterateActors()
        {
            if (Globals.debugMessages)
            {
                Console.WriteLine("Iterating through Actors");
                Console.WriteLine("First Actor Location At: " + Offsets.itrObjectManagerD.ToString("X"));
            }

            uint curOffset = Offsets.itrObjectManagerD;
            uint count = GetActorCount();
            Actor[] actors = new Actor[count];

            for (int i = 0; i < count; i++)
            {
                actors[i] = (Actor)mem.ReadMemory(curOffset, typeof(Actor));
                actors[i].mem_location = curOffset;
                curOffset = curOffset + Offsets.objmanagerStrucSize;
            }

            return actors;
        }

        public static ActorCommonData[] IterateACD()
        {
            if (Globals.debugMessages)
            {
                Console.WriteLine("Iterating through ACD");
                Console.WriteLine("First ACD Location At: " + Offsets.firstACD.ToString("X"));
            }

            uint curOffset = Offsets.firstACD;
            uint count = Offsets.ACDCount;
            ActorCommonData[] acds = new ActorCommonData[count];

            for (int i = 0; i < count; i++)
            {
                acds[i] = (ActorCommonData)mem.ReadMemory(curOffset, typeof(ActorCommonData));
                curOffset = curOffset + Offsets.ACDSize;
            }

            return acds;
        }

        public static Actor[] IterateLocalActors()
        {
            uint count = GetLocalActorCount();
            uint currentOffset = Offsets.LocalActor_3;
            Actor[] localActors = new Actor[count];

            for (int i = 0; i < count; i++)
            {
                localActors[i] = (Actor)mem.ReadMemory(currentOffset, typeof(Actor));//mem.ReadMemory<Actor>(currentOffset);
                currentOffset += Offsets.ofs_LocalActor_StrucSize;
            }

            return localActors;
        }

        public static Actor[] GetItems()
        {
            return IterateActors().OrderBy(a => a.distanceFromMe).Where(a => a.unknown_data1 == 2 && a.unknown_data2 == uint.MaxValue).ToArray<Actor>();
        }

        public static Actor[] GetMonsters()
        {
            return IterateActors().OrderBy(a => a.distanceFromMe).Where(a => a.unknown_data2 == 29944).ToArray<Actor>();
        }
        //FOR maphack, the next version of maphack will use getMonsters.
        public static Actor[] GetMapItems()
        {
            List<Actor> filteredItems = new List<Actor>();
            Actor[] items = GetMonsters();
            filteredItems = items.Where(mob =>
                                        mob.id_acd != Data.toonID &&
                                        mob.distanceFromMe != 0 &&
                                        mob.Exists() &&            /*is do NOT check if the monster is dead - use Actor.Alive*/
                                        mob.unknown_1C4[0] == 1 && /* Were it is a "monster"*/
                                        mob.unknown_data2 == 29944 &&
                                        mob.isAlive() == true
                                        ).ToList();
            return filteredItems.ToArray();
        }

        /// <summary>
        /// Get an actor attribute (health, armor etc.)
        /// </summary>
        /// <typeparam name="T">int or float</typeparam>
        /// <param name="attribute">Attribute to get</param>
        /// <returns>Value of attribute</returns>
        /// // irc (??)
        public static T GetAttribute<T>(ActorAttribute attribute) where T : struct
        {
            uint ret = GetAttribute((uint)attribute.offset | 0xFFFFF000);

            //if (!ret.IsValid())
            //{
            //    return default(T);
            //}

            return mem.ReadMemory<T>(ret + 8);
        }

        private static uint GetAttribute(uint attribute)
        {
            uint CAttribFormula = mem.ReadMemory<uint>(Offsets.ActorAtrib_4 + 0x10);

            uint _418 = mem.ReadMemory<uint>(CAttribFormula + 0x418);

            uint AttributesMap = mem.ReadMemory<uint>(CAttribFormula + 0x8);

            uint IndexMask = (_418 & (attribute ^ (attribute >> 0x10)));

            uint _res = (AttributesMap + 4 * IndexMask);

            uint result = mem.ReadMemory<uint>(_res);

            if (result != 0)
            {
                while (mem.ReadMemory<uint>(result + 0x4) != attribute)
                {
                    result = mem.ReadMemory<uint>(result);

                    if (result == 0)
                    {
                        break;
                    }
                }
            }

            return result;
        }

        // AU3 Owned's
        public static int GetAttributeInt(uint guid, ActorAttribute attribute)
        {
            Actor[] localActors = IterateLocalActors();
            Actor actor = ActorByGUID(localActors, guid);
            uint count = GetAttributeCount();
            uint currentOffset = Offsets.ActorAtrib_4;
            uint actorAtrib, data, atribData;
            const uint ATTIBUTE_COUNT = 825;

            for (int i = 0; i < count; i++)
            {
                actorAtrib = mem.ReadMemoryAsUint(currentOffset);
                if (actorAtrib == actor.FAG)
                {
                    currentOffset = mem.ReadMemoryAsUint(currentOffset + 0x10);
                    for(int u = 0; u < ATTIBUTE_COUNT; u++)
                    {
                        data = mem.ReadMemoryAsUint(currentOffset);
                        currentOffset += 0x4;
                        if (data != 0x0)
                        {
                            atribData = mem.ReadMemoryAsUint(data + 0x4);
                            if (atribData.ToString("X").StartsWith("FFFFF")) // MUST BE  WAY BETTER WAY TO TEST LIKE SHIFTING BYTES
                            {
                                if (atribData.ToString("X").EndsWith(attribute.offset.ToString("X")))
                                {
                                    return mem.ReadMemoryAsInt(data+0x8);
                                }
                            }
                        }
                    }
                }
                currentOffset = currentOffset + Offsets.ofs_ActorAtrib_StrucSize;
            }
            return -1;
        }

        // shadwd
        public static int GetAttributeInt(uint FagGuid, uint attribute_index)
        {
            var fagPtr = AcdToFAG((int)FagGuid);
            if (fagPtr == IntPtr.Zero) return -1;

            var attrPtr = GetAttribute(fagPtr, attribute_index);
            if (attrPtr == IntPtr.Zero) return -1; ;

            return mem.ReadMemoryAsInt((uint)attrPtr);
        }

        private static IntPtr GetAttribute(IntPtr fagPtr, uint attribute_index)
        {
            attribute_index = attribute_index | 0xFFFFF000;

            int _38 = mem.ReadMemoryAsInt((uint)fagPtr + 0x38);
            int _c8 = mem.ReadMemoryAsInt((uint)fagPtr + 0xC8);
            int v4 = (int)(_c8 & (attribute_index ^ (attribute_index >> 0x10)));
            v4 = (_38 + 4 * v4);

            v4 = mem.ReadMemoryAsInt((uint)v4);
            if (v4 != 0)
            {
                while (mem.ReadMemoryAsUint((uint)(v4 + 4)) != attribute_index)
                {
                    v4 = mem.ReadMemoryAsInt((uint)(v4));
                    if (v4 == 0)
                        goto NEXT_SCAN;
                }

                return (IntPtr)v4 + 8;
            }

        NEXT_SCAN:
            int _10 = mem.ReadMemoryAsInt((uint)(fagPtr + 0x10));
            int _8 = mem.ReadMemoryAsInt((uint)(_10 + 0x8));
            int _418 = mem.ReadMemoryAsInt((uint)(_10 + 0x418));

            int v5 = (int)(_418 & (attribute_index ^ (attribute_index >> 0x10)));
            int _res = (_8 + 4 * v5);
            v5 = mem.ReadMemoryAsInt((uint)_res);
            if (v5 != 0)
            {
                while (mem.ReadMemoryAsUint((uint)(v5 + 4)) != attribute_index)
                {
                    v5 = mem.ReadMemoryAsInt((uint)(v5));
                    if (v5 == 0)
                        return IntPtr.Zero;
                }
                return (IntPtr)v5 + 8;
            }

            return IntPtr.Zero;

        }

        //These are direct translations of D3s lookup methods. This might need to stay in sync if they make code changes to it.
        //or if the structure offsets change.
        public static IntPtr AcdToFAG(int FagGuid)
        {
            int result = 0;
            var objMgr = mem.ReadMemoryAsUint(Offsets.objectManager);
            uint ptr = mem.ReadMemoryAsUint(objMgr + (0x844));
            ptr = mem.ReadMemoryAsUint(ptr + 0x70);


            uint max = mem.ReadMemoryAsUint(ptr + 0x100);
            int size = 0x180;

            if ((FagGuid & 0xFFFF) < max)
            {
                int _148 = mem.ReadMemoryAsInt(ptr + 0x148);
                int _18c = mem.ReadMemoryAsInt(ptr + 0x18c);

                int a = (_148 + 4 * 0);
                int b = (size * (FagGuid & ((1 << (int)_18c) - 1)));

                int v3 = mem.ReadMemoryAsInt((uint)(a)) + b;
                result = v3 & -1;
            }

            return (IntPtr)result;
        }
    }
}
