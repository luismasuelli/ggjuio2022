using AlephVault.Unity.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GGJUIO2020.Types
{
    namespace Protocols
    {
        namespace Messages
        {
            public class LoginFailed : ISerializable
            {
                public string Reason;

                public void Serialize(Serializer serializer)
                {
                    serializer.Serialize(ref Reason);
                }
            }
        }
    }
}
