using CodeITDL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Intrensic.Administration
{
    public partial class frmAuditLog : Form
    {
        public frmAuditLog()
        {
            InitializeComponent();
            
            this.BackgroundImage = Context.GetImageForCustomer();

            List<CodeITDL.User> users = new List<CodeITDL.User>();
            using (CodeITDL.CodeITDbContext ctx = new CodeITDL.CodeITDbContext(Context.UserId))
            {
                users = ctx.Users.Where(x => x.CustomerId == Context.CustomerId).ToList();
            }
            users.Insert(0, new CodeITDL.User() { Id = -1, CustomerId = Context.CustomerId, FirstName = "Select All" });



            cbName.DisplayMember = "FullName";
            cbName.ValueMember = "Id";
            cbName.DataSource = users;

            if (cbName.Items.Count > 0)
                cbName.SelectedIndex = 0;

            if (cbAction.Items.Count > 0)
                cbAction.SelectedIndex = 0;

            if (cbItems.Items.Count > 0)
                cbItems.SelectedIndex = 0;

        }
        bool isLoaded = false;
        private void btnSearch_Click(object sender, EventArgs e)
        {
            isLoaded = false;
            List<AuditLog> auditLog = new List<AuditLog>();
            lvResults.Items.Clear();
            lvDetails.Items.Clear();


            using (CodeITDL.CodeITDbContext ctx = new CodeITDL.CodeITDbContext(Context.UserId))
            {
                DateTime to = dtpTo.Value.Date.AddHours(23).AddMinutes(59);
                auditLog = ctx.AuditLogs
                    .Where(x => x.ObjectId == ("CodeITDL." + cbItems.SelectedItem.ToString()) || cbItems.SelectedIndex == 0)
                    .Where(x => x.CreatedBy == (int)cbName.SelectedValue || (int)cbName.SelectedValue == -1)
                    .Where(x => x.CreatedOn >= dtpFrom.Value.Date && x.CreatedOn <= to)
                    .Where(x => x.Action == cbAction.SelectedItem.ToString().Substring(0, 1) || cbAction.SelectedIndex == 0)
                    .OrderByDescending(x=>x.CreatedOn)
                    .ToList();
            }

            
            ListViewItem[] listItems = new ListViewItem[auditLog.Count];
            int i = 0;
            if (auditLog.Count > 0)
                using (CodeITDbContext ctx = new CodeITDbContext(Context.UserId))
                    foreach (AuditLog alItem in auditLog)
                    {
                        lvResults.BeginUpdate();
                        ListViewItem lvi = new ListViewItem(alItem.CreatedOn.ToString());
                        if (ctx.Users.Where(x => x.Id == alItem.CreatedBy).Count() > 0)
                            lvi.SubItems.Add(ctx.Users.Where(x => x.Id == alItem.CreatedBy).FirstOrDefault().FullName);
                        else
                            lvi.SubItems.Add("-");

                        lvi.SubItems.Add((alItem.Action.Equals("I") ? "Insert" :
                            (alItem.Action.Equals("M") ? "Update" : "Delete")));
                        lvi.SubItems.Add(alItem.OldObject);
                        lvi.SubItems.Add(alItem.NewObject);
                        lvi.Tag = alItem;

                        listItems[i] = lvi;

                        i++;
                        lvResults.Items.Add(lvi);
                        lvResults.EndUpdate();
                    }

            isLoaded = true;
        }

        private void lvResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoaded) return;

            lvDetails.Items.Clear();

            if (lvResults.SelectedItems.Count == 0) return;

            AuditLog alItem = (AuditLog)lvResults.SelectedItems[0].Tag;


            MemoryStream newStream = new MemoryStream();
            StreamWriter writerNew = new StreamWriter(newStream);
            writerNew.Write(alItem.NewObject);
            writerNew.Flush();
            newStream.Position = 0;

            XmlReader rdr = XmlReader.Create(newStream);

            MemoryStream oldStream = new MemoryStream();

            if (!string.IsNullOrEmpty(alItem.OldObject))
            {
                oldStream = new MemoryStream();
                StreamWriter writerOld = new StreamWriter(oldStream);
                writerOld.Write(alItem.OldObject);
                writerOld.Flush();
                oldStream.Position = 0;
            }



            ListViewItem lvi = new ListViewItem();


            string currentElement = string.Empty;
            string currentNamespace = string.Empty;
            while (rdr.Read())
            {
                if (rdr.Depth == 0) //root element
                    continue;


                if (rdr.NodeType == XmlNodeType.Element)
                {
                    currentElement = rdr.LocalName;
                    currentNamespace = rdr.NamespaceURI;
                    lvi = new ListViewItem(rdr.LocalName);
                    Console.Write(rdr.LocalName + "    ----->     " + rdr.Value);
                }
                if (rdr.NodeType == XmlNodeType.Text)
                {
                    string oldValue = "-----";

                    if (!string.IsNullOrEmpty(alItem.OldObject))
                    {//has old value
                    
                        oldStream.Position = 0;

                        using (XmlReader rdrOld = XmlReader.Create(oldStream))
                            if (rdrOld.ReadToFollowing(currentElement))
                                oldValue = rdrOld.ReadElementContentAsString();

                        if (!string.IsNullOrEmpty(oldValue))
                            lvi.SubItems.Add(oldValue);
                        else
                            lvi.SubItems.Add(string.Empty);

                        Console.Write(oldValue + " <--------->");
                    }
                    else
                        lvi.SubItems.Add(string.Empty);

                    if (!oldValue.Equals("-----") && oldValue != rdr.Value)
                        lvi.ForeColor = Color.Red;

                    lvi.SubItems.Add(rdr.Value);

                    Console.Write(rdr.Value);
                }
                if (rdr.NodeType == XmlNodeType.EndElement)
                {
                    lvDetails.Items.Add(lvi);
                    Console.WriteLine();
                }
            }

        }
    }
}
