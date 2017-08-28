using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpCenter.Models
{
    public class tbl_Videos
    {   
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Subido Por")]
        public int Created_User { get; set; }

        [Display(Name = "Subido")]
        public DateTime Created_at { get; set; }

        public int Updated_User { get; set; }

        public DateTime Updated_at { get; set; }
    }

    public class HelpCenterDBContext : DbContext
    {
        public DbSet<tbl_Videos> tbl_Videos { get; set; }



        /*public void UpdateCustomerCredentials()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateCustomer", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    cmd.Parameters.Add(new SqlParameter("@FirstName", firstName));
                    cmd.Parameters.Add(new SqlParameter("@LastName", lastName));
                    cmd.Parameters.Add(new SqlParameter("@Email", email));
                    cmd.Parameters.Add(new SqlParameter("@MobilePhoneNumber", mobilePhoneNumber));
                    cmd.Parameters.Add(new SqlParameter("@ImageId", GetParamValue(imageId)));
                    cmd.Parameters.Add(new SqlParameter("@Price", price));
                    cmd.Parameters.Add(new SqlParameter("@Notes", notes));

                    try
                    {
                        con.Open();

                        cmd.ExecuteReader();

                        con.Close();
                    }
                    catch (SqlException ex)
                    {
                        cmd.Dispose();
                        throw ex;
                    }

                    finally
                    {
                        cmd.Dispose();
                    }
                }
            }
        }*/
    }
}