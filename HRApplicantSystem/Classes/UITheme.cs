using System;
using System.Drawing;
using System.Windows.Forms;

namespace HRApplicantSystem.Classes
{
    public static class UITheme
    {
        // Modern Dark Blue / Slate palette
        public static Color ColorPrimary = Color.FromArgb(30, 41, 59);     // Deep Slate (Sidebars / Header)
        public static Color ColorSecondary = Color.FromArgb(71, 85, 105);  // Mid Slate (Text, Icons)
        public static Color ColorAccent = Color.FromArgb(59, 130, 246);     // Indigo/Blue (Highlights, Actions)
        public static Color ColorBg = Color.FromArgb(248, 250, 252);       // Light Grey (Forms background)
        public static Color ColorGridLine = Color.FromArgb(241, 245, 249); // Divider Slate

        /// <summary>
        /// Styles active action buttons (Save, Register, Submit).
        /// </summary>
        public static void StylePrimaryButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = ColorAccent;
            btn.ForeColor = Color.White;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Styles minor secondary actions (Clear, Cancel, Reset).
        /// </summary>
        public static void StyleSecondaryButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = Color.White;
            btn.ForeColor = ColorSecondary;
            btn.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240); // Soft grey border
            btn.FlatAppearance.BorderSize = 1;
            btn.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular);
            btn.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Formats default DataGridViews into flat, modern components.
        /// </summary>
        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BorderStyle = BorderStyle.None;
            dgv.BackgroundColor = Color.White;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = ColorBg;
            dgv.EnableHeadersVisualStyles = false;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = ColorGridLine;

            // Header configuration
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = ColorPrimary;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 35;

            // Row cell and selection layout
            dgv.DefaultCellStyle.SelectionBackColor = ColorAccent;
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.RowTemplate.Height = 30;
        }
    }
}