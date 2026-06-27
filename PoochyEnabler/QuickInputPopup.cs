using System;
using System.ComponentModel;
using System.Windows.Forms;

using PoochyEnabler.Helpers;

namespace PoochyEnabler
{
    public partial class QuickInputPopup : Form
    {
        public string _offset;
        public int _dataTypeIndex;
        public int _entryCount;

        public QuickInputPopup()
        {
            InitializeComponent();

            //btnApply.Click += btnApply_Click;

        }
    }
}
