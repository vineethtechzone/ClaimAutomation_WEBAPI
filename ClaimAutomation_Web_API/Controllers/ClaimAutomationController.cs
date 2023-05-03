using Microsoft.AspNetCore.Mvc;
using ClaimAutomation.Model;


namespace ClaimAutomationController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClaimController : Controller
    { 
        [HttpPost(Name = "GenerateClaim")]
        public ActionResult<string> GenerateClaim(string inputText)
        {
            decimal total = 0;
            decimal salesTaxRate = 5;
            int startIndex, endIndex;
            if (string.IsNullOrEmpty(inputText)) { return BadRequest("Please provide valid claim details"); }
            ClaimModel claimModel = new ClaimModel();
            // Extract cost centre and total from XML
            startIndex = inputText.IndexOf("<expense>");
            endIndex = inputText.IndexOf("</expense>");
            if (startIndex >= 0 && endIndex >= 0)
            {
                string expenseXml = inputText.Substring(startIndex, endIndex - startIndex + 10);
                startIndex = expenseXml.IndexOf("<cost_centre>");
                endIndex = expenseXml.IndexOf("</cost_centre>");
                int costcentertag = startIndex * endIndex;
                if (startIndex >= 0 && endIndex >= 0)
                {
                    claimModel.cost_centre = expenseXml.Substring(startIndex + 13, endIndex - startIndex - 13);
                }
                else if(costcentertag <0)
                {
                    return BadRequest("Invalid cost center details");
                }

                startIndex = expenseXml.IndexOf("<total>");
                endIndex = expenseXml.IndexOf("</total>");
                if (startIndex >= 0 && endIndex >= 0)
                {
                    string totalString = expenseXml.Substring(startIndex + 7, endIndex - startIndex - 7);
                    if (!decimal.TryParse(totalString, out total))
                    { return BadRequest("Please provide valid total amount"); }
                    else { claimModel.total = Convert.ToDecimal(totalString); }
                }
                else
                {
                    return BadRequest("Please provide total amount");
                }

                startIndex = expenseXml.IndexOf("<payment_method>");
                endIndex = expenseXml.IndexOf("</payment_method>");
                if (startIndex >= 0 && endIndex >= 0)
                {
                    string paymentmethod = expenseXml.Substring(startIndex + 16, endIndex - startIndex - 16);
                    if (!string.IsNullOrEmpty(paymentmethod))
                    {
                        claimModel.payment_method = paymentmethod;
                    }
                }
                else
                {
                    return BadRequest("Invalid payment method details");
                }

                startIndex = inputText.IndexOf("<vendor>");
                endIndex = inputText.IndexOf("</vendor>");
                if (startIndex >= 0 && endIndex >= 0)
                {
                    string vendor = inputText.Substring(startIndex + 8, endIndex - startIndex - 8);
                    if (!string.IsNullOrEmpty(vendor))
                    {
                        claimModel.vendor = vendor;
                    }
                }
                else
                {
                    return BadRequest("Invalid vendor details");
                }

                startIndex = inputText.IndexOf("<description>");
                endIndex = inputText.IndexOf("</description>");
                if (startIndex >= 0 && endIndex >= 0)
                {
                    string description = inputText.Substring(startIndex + 13, endIndex - startIndex - 13);
                    if (!string.IsNullOrEmpty(description))
                    {
                        claimModel.description = description;
                    }
                }
                else
                {
                    return BadRequest("Invalid description details");
                }

                startIndex = inputText.IndexOf("<date>");
                endIndex = inputText.IndexOf("</date>");
                if (startIndex >= 0 && endIndex >= 0)
                {
                    string datestring = inputText.Substring(startIndex + 6, endIndex - startIndex - 6);
                    if (!string.IsNullOrEmpty(datestring))
                    {
                        claimModel.date = datestring;
                    }
                }
                else
                {
                    return BadRequest("Invalid date");
                }
            }
            else
            {
                return BadRequest("No expense information found");
            }

            if (string.IsNullOrEmpty(claimModel.cost_centre)) { claimModel.cost_centre = "UNKNOWN"; }
            // Calculate sales tax and total excluding tax
            claimModel.totalExcludingTax = Math.Round((total * 100) / (100 + salesTaxRate), 2);
            claimModel.salestaxamt = Math.Round(total - claimModel.totalExcludingTax, 2);
            claimModel.taxrate = salesTaxRate;

            // Construct response object
            var outputJson = Newtonsoft.Json.JsonConvert.SerializeObject(claimModel);
            return Ok(claimModel);
        }
    }
}
    


