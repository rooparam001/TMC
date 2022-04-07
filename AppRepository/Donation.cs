using EntitesInterfaces.AppModels;
using EntitesInterfaces.DBEntities;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using TMC.DBConnections;

namespace TMC.AppRepository
{
    public static class Donation
    {
        public static DonationAddModel Save(DonationAddModel model)
        {
            if (!string.IsNullOrEmpty(model.CITY) || !string.IsNullOrEmpty(model.CONTACTNUMBER) || !string.IsNullOrEmpty(model.EMAILID) || !string.IsNullOrEmpty(model.FULLNAME) || model.TOTAMOUNT > 0)
            {
                var obj = new TBL_DONATIONMASTER()
                {
                    CITYID = new TMCDBContext().fn_SaveCity(model.CITY.Trim()),
                    CONTACTNUMBER = model.CONTACTNUMBER.Trim(),
                    DATECREATED = DateTime.Now,
                    EMAILID = model.EMAILID.Trim(),
                    FULLNAME = model.FULLNAME.Trim(),
                    ISPAID = false,
                    TOTAMOUNT = model.TOTAMOUNT,
                    ID = model.ID,
                    RAZORPAYORDER = model.PKey
                };
                obj = new TMCDBContext().fn_SaveDonation(obj);
                model.ID = obj.ID;
            }

            return model;
        }

        public static string GenerateOrder(string keyID, string secretKey, DonationAddModel model)
        {
            var resp = "";
            var recptID = "";
            try
            {
                RazorpayClient client = new RazorpayClient(keyID, secretKey);
                recptID = GenerateReceiptNumber();
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", model.TOTAMOUNT);
                options.Add("receipt", recptID);
                options.Add("currency", "INR");
                Order order = client.Order.Create(options);
                resp = order["id"];
                new TMCDBContext().fn_SaveOrder_Razorpay(new TBL_ORDERGENERATORMASTER()
                {
                    DATECREATED = DateTime.Now,
                    ORDERID = resp,
                    RECEIPTID = recptID,
                    TOTAMOUNT = model.TOTAMOUNT
                });
            }
            catch { resp = ""; }
            return resp;
        }

        public static bool SavePaymentOrder(TBL_ORDERGENERATORMASTER model)
        {
            var resp = false;
            var modelObj = new TBL_ORDERGENERATORMASTER();
            modelObj = new TMCDBContext().fn_GetOrderbyOrderID(model);
            if (modelObj != null)
            {
                if (modelObj.ID > 0)
                {
                    modelObj.RAZORPAY_CODE = model.RAZORPAY_CODE;
                    modelObj.RAZORPAY_DESCRIPTION = model.RAZORPAY_DESCRIPTION;
                    modelObj.RAZORPAY_PAYMENT_ID = model.RAZORPAY_PAYMENT_ID;
                    modelObj.RAZORPAY_REASON = model.RAZORPAY_REASON;
                    modelObj.RAZORPAY_SIGNATURE = model.RAZORPAY_SIGNATURE;
                    modelObj.RAZORPAY_SOURCE = model.RAZORPAY_SOURCE;
                    modelObj.RAZORPAY_STEP = model.RAZORPAY_STEP;
                    new TMCDBContext().fn_SaveOrder_Razorpay(modelObj);
                    resp = true;
                }
            }
            return resp;
        }

        public static string GenerateReceiptNumber()
        {
            try
            {
                var lastID = new TMCDBContext().fn_GetLastOrderID();
                if (lastID > 0)
                    return string.Concat("order_rcptid_", lastID++);
                else
                    return "";
            }
            catch (Exception ex)
            {
                return "";
            }

        }
    }
}
