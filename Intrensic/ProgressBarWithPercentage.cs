using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Intrensic
{
    public class ProgressBarWithPercentage : System.Windows.Forms.ProgressBar
    {
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x000F)
            {
                var flags = TextFormatFlags.VerticalCenter |
                            TextFormatFlags.HorizontalCenter |
                            TextFormatFlags.SingleLine |
                            TextFormatFlags.WordEllipsis;
                if (this.Value > 0)
                    TextRenderer.DrawText(CreateGraphics(),
                                          Decimal.Floor(((Decimal)this.Value / this.Maximum * 100)).ToString() + "%",
                                          Font,
                                          new Rectangle(0, 0, this.Width, this.Height),
                                          Color.Black,
                                          flags);
            }
        }
    }
}
