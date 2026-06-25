using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoochyEnabler.Managers
{
    public class EditorDataManager<T> where T : class, new()
    {
        // associate with UI
        private T _editingEntry;
        // temporary data
        private Dictionary<string, byte[]> _binaries = new Dictionary<string, byte[]>();

        public EditorDataManager(
            T entry
            )
        {
            _editingEntry = entry;



        }




    }
}
