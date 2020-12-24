using BankingProject_Demo.DataAccess;
using BankingProject_Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace BankingProject_Demo.Controllers
{
    /* 
COPY RIGHT @AUGMENTO LABS 2020
Created By Alam
*/
    [RoutePrefix("Api/Customer")]
    public class AuthorizationController : ApiController
    {

        [BasicAuthentication]
        [HttpGet]
        [Route("CustomerDetails")]
        public HttpResponseMessage Get()
        {
            string username = Thread.CurrentPrincipal.Identity.Name;
            //string usernames = username;
            using (CustomerEntities dbContext = new CustomerEntities())
            {
                var entity = dbContext.AccountHolderDetails.Where(e => e.Name.ToLower() == username).ToList();
                if (entity.Count > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        dbContext.AccountHolderDetails.Include("Transcation_Details").Where((e => e.Name.ToLower() == username)).ToList());
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Password Of " + " " + username.ToString() + " " + " is not found");
                }
            }
        }
        //[HttpPost]
        //[Route("Insert")]
        //public HttpResponseMessage Post([FromBody] Transcation_Details transcation_Details)
        //{
        //    try
        //    {
        //        using (CustomerEntities dbContext = new CustomerEntities())
        //        {
        //            dbContext.Transcation_Details.Add(transcation_Details);
        //            if (transcation_Details.Amount < 0)
        //            {
        //                string amountmesage = " Please Enter valid amount";
        //                var messag1 = Request.CreateResponse(HttpStatusCode.Created, amountmesage);
        //                return messag1;
        //            }
        //            else
        //            {
        //                dbContext.SaveChanges();
        //                var message = Request.CreateResponse(HttpStatusCode.Created, transcation_Details);
        //                message.Headers.Location = new Uri(Request.RequestUri +
        //                    transcation_Details.TranscationId.ToString());
        //                return message;
        //            }
                   
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
        //    }

        //}
        [HttpPost]
        [Route("Insert")]
        public  HttpResponseMessage Post([FromBody] Transcation_Details transcation_Details)
        {
            try
            {
                if (transcation_Details.Amount <= 0)
                {
                    string message=   "Please Check the Amount to WithDraw";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, message);
                }
                using (CustomerEntities entities = new CustomerEntities())
                {
                    var findAccount = entities.AccountHolderDetails.FirstOrDefault(e => e.AccountNumber == transcation_Details.AccountNumber);
                    if (findAccount == null)
                    {
                       return Request.CreateResponse(HttpStatusCode.NotFound, "Account Number Not Found");
                    }
                    else if (findAccount.BalanceAmount >=0)
                    {

                        if (transcation_Details.TranscationType == "Credit" || transcation_Details.TranscationType=="Deposit")
                        {
                            var data = entities.AccountHolderDetails.Where(d => d.AccountNumber == findAccount.AccountNumber).First();
                            data.BalanceAmount = findAccount.BalanceAmount + transcation_Details.Amount;
                            transcation_Details.Amount = transcation_Details.Amount;
                            entities.Transcation_Details.Add(transcation_Details);
                            entities.SaveChanges();
                            return Request.CreateResponse(HttpStatusCode.OK, "Amount Has been" + " " + transcation_Details.TranscationType + " " + transcation_Details.Amount + " "+ "Rupees" +" "+ "Successfully");
                        }
                        else if (findAccount.BalanceAmount < transcation_Details.Amount)
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, " Please Check Account Balance");
                        }
                        else if(transcation_Details.TranscationType=="debit" || transcation_Details.TranscationType =="withdraw")
                        {
                            var data = entities.AccountHolderDetails.Where(d => d.AccountNumber == findAccount.AccountNumber).First();
                            data.BalanceAmount = findAccount.BalanceAmount - transcation_Details.Amount;
                            transcation_Details.Amount = transcation_Details.Amount;
                            entities.Transcation_Details.Add(transcation_Details);
                            entities.SaveChanges();
                            return Request.CreateResponse(HttpStatusCode.OK, "Amount Has been" + " " + transcation_Details.TranscationType + " "+transcation_Details.Amount+" "+ "Rupees" + " "+"Successfully");

                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Please select TranscationType to  Debit Yeah Credit amount");
                        }
                   
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Insufficient Balance in Account Number " +" "+ transcation_Details.AccountNumber);
                    }
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
