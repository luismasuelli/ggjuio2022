using System;
using GameMeanMachine.Unity.NetRose.Authoring.Behaviours.Server;
using GameMeanMachine.Unity.WindRose.Authoring.Behaviours.World;
using GGJUIO2020.Server.Authoring.Behaviours.Protocols.Models;


namespace GGJUIO2020.Server
{
    namespace Authoring
    {
        namespace Behaviours
        {
            namespace Protocols
            {
                public class PlayerCharacterPrincipalProtocolServerSide : PrincipalObjectsNetRoseProtocolServerSide<PlayerCharacterServerSide>
                {
                    public void InstantiateCharacter(
                        ulong connectionId, Func<Map> toMap, ushort x, ushort y,
                        Action<PlayerCharacterServerSide> beforeAttach = null, Action<PlayerCharacterServerSide> afterAttach = null
                    )
                    {
                        InstantiatePrincipal(connectionId, "player", toMap, x, y, beforeAttach, afterAttach);
                    }

                    public void RemoveCharacter(ulong connectionId)
                    {
                        RemovePrincipal(connectionId);
                    }
                }
            }
        }
    }
}