using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using CodeITLicence;

namespace CodeITDL
{

    //Audit Action 
    //I = insert
    //M = modify
    //D = delete


    public partial class CodeITDbContext : DbContext
    {
        //public IntrensicEntities()
        //    : base("name=IntrensicEntities")
        //{
        //}



        //public DbSet<AuditLog> AuditLogs { get; set; }
        //public DbSet<Customer> Customers { get; set; }
        //public DbSet<File> Files { get; set; }
        //public DbSet<Note> Notes { get; set; }
        //public DbSet<Setting> Settings { get; set; }
        //public DbSet<User> Users { get; set; }


        private int userId;

        //private CodeITDbContext() : base("name=IntrensicEntities") { }		
        public CodeITDbContext(int userId)            
        {
            this.userId = userId;
        }

        public int SaveChangesWithoutAudit()
        {
            return base.SaveChanges();
        }

        public override int SaveChanges()
        {



            List<AuditLog> newAuditItems = new List<AuditLog>();



            foreach (var itm in this.ChangeTracker.Entries().Where(x => x.State == System.Data.Entity.EntityState.Deleted))
            {
                AuditLog alDelete = new AuditLog();
                alDelete.Action = "D";
                alDelete.CreatedBy = userId;
                alDelete.NewObject = string.Empty;
                alDelete.CreatedOn = DateTime.Now;
                string oldObject = string.Empty;
                using (MemoryStream memStm = new MemoryStream())
                {
                    DataContractSerializer serializer = new DataContractSerializer(itm.Entity.GetType());

                    serializer.WriteObject(memStm, itm.Entity);
                    oldObject = System.Text.Encoding.UTF8.GetString(memStm.ToArray());
                }
                alDelete.ObjectId = itm.Entity.ToString();
                alDelete.OldObject = oldObject;
                alDelete.CustomerId = Licence.ClientId;

                newAuditItems.Add(alDelete);

            }


            foreach (var itm in this.ChangeTracker.Entries().Where(x => x.State == System.Data.Entity.EntityState.Added))
            {
                AuditLog alInsert = new AuditLog();
                alInsert.Action = "I";
                alInsert.CreatedBy = userId;
                alInsert.OldObject = string.Empty;

                alInsert.CreatedOn = DateTime.Now;
                
                string newObject = string.Empty;
                using (MemoryStream memStm = new MemoryStream())
                {
                    DataContractSerializer serializer = new DataContractSerializer(itm.Entity.GetType());                    
                    serializer.WriteObject(memStm, itm.Entity);
                    newObject = System.Text.Encoding.UTF8.GetString(memStm.ToArray());
                }

                alInsert.NewObject = newObject;
                alInsert.ObjectId = itm.Entity.ToString();
                alInsert.CustomerId = Licence.ClientId;


                newAuditItems.Add(alInsert);

            }


            foreach (var itm in this.ChangeTracker.Entries().Where(x => x.State == System.Data.Entity.EntityState.Modified))
            {
                AuditLog alModify = new AuditLog();
                alModify.Action = "M";
                alModify.CreatedBy = userId;

                var oldObject = Activator.CreateInstance(itm.Entity.GetType());

                foreach (PropertyInfo propertyInfo in oldObject.GetType().GetProperties())
                {
                    if (Attribute.IsDefined(propertyInfo, typeof(System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute)))
                        continue;

                    oldObject.GetType().GetProperty(propertyInfo.Name).SetValue(oldObject, itm.OriginalValues[propertyInfo.Name]);
                }


                

                string newObject = string.Empty;
                using (MemoryStream memStm = new MemoryStream())
                {
                    DataContractSerializer serializer = new DataContractSerializer(oldObject.GetType());

                    serializer.WriteObject(memStm, oldObject);

                    alModify.OldObject = System.Text.Encoding.UTF8.GetString(memStm.ToArray());

                }
                using (MemoryStream memStm = new MemoryStream())
                {
                    DataContractSerializer serializer = new DataContractSerializer(itm.Entity.GetType());

                    serializer.WriteObject(memStm, itm.Entity);

                    alModify.NewObject = System.Text.Encoding.UTF8.GetString(memStm.ToArray());
                }

                alModify.CreatedOn = DateTime.Now;
                alModify.CustomerId = Licence.ClientId;
                alModify.ObjectId = itm.Entity.ToString();

                newAuditItems.Add(alModify);

            }






            foreach (AuditLog item in newAuditItems)
            {
                //item.ObjectId = string.Empty;
                //item.CreatedOn = DateTime.Now;
                this.Set<AuditLog>().Add(item);

            }

            return base.SaveChanges();
        }

    }
}
