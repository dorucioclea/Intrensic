using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Intrensic
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form() : base()
        {
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);

            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            DoubleBuffered = true;

            this.Icon = Intrensic.Properties.Resources.Intrensic;

            this.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor, true);

            this.Resize += Form_Resize;
        }

        void Form_Resize(object sender, EventArgs e)
        {
            this.Refresh();
        }


        //sometimes this makes errors

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
        //        return cp;
        //    }
        //} 
    }
}
