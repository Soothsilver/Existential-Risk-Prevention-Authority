using Auxiliary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MainGameSpace
{
    class SaveLoad
    {
        public static void Save(Session session)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(session.Year + ".dat", FileMode.Create);
                bf.Serialize(fs, session);
                fs.Close();
            }
            catch
            {
                Root.SendToast("Autosave failed.");
            }
        }
    }
}
