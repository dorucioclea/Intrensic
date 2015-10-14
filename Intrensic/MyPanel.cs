using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Intrensic
{
    public class MyPanel : Panel
    {

        protected override void OnSizeChanged(EventArgs e)
        {

            if (this.Handle != null)
            {

                this.BeginInvoke((MethodInvoker)delegate
                {

                    base.OnSizeChanged(e);

                });

            }

        }

    } 

}
