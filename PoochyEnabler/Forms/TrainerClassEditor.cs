using System;
using System.Linq;
using System.Windows.Forms;

using PoochyEnabler.FileReaders;
using PoochyEnabler.Managers;

namespace PoochyEnabler.Forms
{
    public partial class TrainerClassEditor : Form
    {
        private readonly byte[] _romData;
        private readonly IniFileReader _config;
        private readonly TblFileReader _charmap;
        private readonly ReservationManager _reservationManager;
        private readonly Action _saveAction;

        public TrainerClassEditor(
            byte[] romData,
            IniFileReader config,
            TblFileReader charmap,
            ReservationManager reservationManager,
            Action saveAction)
        {
            InitializeComponent();
            _romData = romData;
            _config = config;
            _charmap = charmap;
            _reservationManager = reservationManager;
            _saveAction = saveAction;
        }
    }
}
