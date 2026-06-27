using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PoochyEnabler.Managers
{
    public sealed class ReservationManager
    {
        private readonly Dictionary<TextBox, ReservationInfo> _reservations
            = new Dictionary<TextBox, ReservationInfo>();

        public sealed class ReservationInfo
        {
            public int Offset { get; }
            public string StateKey { get; }
            public Color BackColor { get; }

            public ReservationInfo(
                int offset,
                string stateKey,
                Color backColor)
            {
                Offset = offset;
                StateKey = stateKey;
                BackColor = backColor;
            }
        }

        public void SetReservation(TextBox textBox, int offset, string stateKey)
        {
            ClearReservation(textBox, redraw: false);

            var info = new ReservationInfo(
                            offset,
                            stateKey,
                            textBox.BackColor);
            _reservations[textBox] = info;

            textBox.Text = offset.ToString("X8");
            textBox.BackColor = Color.LightPink;

            textBox.TextChanged -= OnTextChanged;
            textBox.TextChanged += OnTextChanged;
        }

        public ReservationInfo GetReservation(TextBox textBox)
        {
            return _reservations.TryGetValue(textBox, out var info)
                ? info
                : null;
        }

        public void ClearReservation(TextBox textBox, bool redraw = true)
        {
            if (textBox == null) return;
            if (!_reservations.TryGetValue(textBox, out var info)) return;

            // clear all settings
            textBox.TextChanged -= OnTextChanged;
            _reservations.Remove(textBox);
            if (redraw)
            {
                textBox.BackColor = info.BackColor;
            }
        }

        public void ClearAllReservations()
        {
            foreach (var textBox in _reservations.Keys.ToArray())
            {
                ClearReservation(textBox);
            }
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            if (!(sender is TextBox textBox)) return;
            if (!_reservations.TryGetValue(textBox, out var info)) return;

            string reservedText = info.Offset.ToString("X8");

            if (!string.Equals(
                textBox.Text.Trim(),
                reservedText,
                StringComparison.OrdinalIgnoreCase))
            {
                ClearReservation(textBox);
            }
        }

        public List<ReservationInfo> GetAllReservations()
        {
            return _reservations.Values.ToList();
        }
    }
}