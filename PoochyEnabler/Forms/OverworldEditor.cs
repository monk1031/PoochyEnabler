using System;
using System.Drawing;
using System.Windows.Forms;

using PoochyEnabler.FileReaders;
using PoochyEnabler.Helpers;
using PoochyEnabler.Managers;

namespace PoochyEnabler.Forms
{
    public partial class OverworldEditor : Form
    {
        private readonly byte[] _romData = null;
        private readonly IniFileReader _config = null;
        private readonly TblFileReader _charmap = null;
        private readonly ReservationManager _reservationManager = null;
        private readonly StateManager _stateManager = null;
        private readonly Action _saveAction = null;

        private EntryManager<TrainerImageEntry> _imageManager = null;
        private EntryManager<TrainerPaletteEntry> _paletteManager = null;
        private EntryManager<TrainerYOffsetEntry> _yPositionManager = null;
        private EntryManager<TrainerAnimPointerEntry> _animPointerManager = null;

        private bool _isUpdatingUI = false;
        private int _currentEntryIdx = 0;

        public OverworldEditor()
        {
            InitializeComponent();
        }
    }
}
