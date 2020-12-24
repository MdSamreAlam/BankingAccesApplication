using BankingProject_Demo.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankingProject_Demo.Models
{
    /* 
COPY RIGHT @AUGMENTO LABS 2020
Created By Alam
*/

    //  Created method for User Authentication
    public class CustomerSecuritry
    {
        public static bool LogIn(string username, string password)
        {
            using (CustomerEntities entities = new CustomerEntities())
            {
                return entities.AccountHolderDetails.Any(user => user.Name.Equals(username, StringComparison.OrdinalIgnoreCase)
                && user.Password == password);
            }
        }
      

    }
}