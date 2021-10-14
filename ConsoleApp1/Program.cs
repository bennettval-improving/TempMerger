using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            // e.g. https://yourorg.crm.dynamics.com
            string url = "https://orga0d82b67.crm.dynamics.com/";
            // e.g. you@yourorg.onmicrosoft.com
            string userName = "admin@CRM305681.onmicrosoft.com";
            // e.g. y0urp455w0rd 
            string password = "oGQhYr5K5C";

            string conn = $@"
                Url = {url};
                AuthType = OAuth;
                UserName = {userName};
                Password = {password};
                AppId = 51f81489-12ee-4a9e-aaae-a2591f45987d;
                RedirectUri = app://58145B91-0C36-4500-8554-080854F2AC97;
                LoginPrompt=Auto;
                RequireNewInstance = True";

            using (var svc = new CrmServiceClient(conn))
            {

                // Create the target for the request.
                //var target = new EntityReference();
                var attributes = new ColumnSet(new string[] { "accountid", "name", "telephone1", "fax" });
                var target = svc.Retrieve("account", Guid.Parse("0c4e458f-3f2c-ec11-b6e5-000d3a4f9c59"), attributes);
                

                // Id is the GUID of the account that is being merged into.
                // LogicalName is the type of the entity being merged to, as a string
                //target.Id = _account1Id;
                //target.LogicalName = Account.EntityLogicalName;

                // Create the request.
                //var merge = new MergeRequest();
                var merge = new MergeRequest();

                // SubordinateId is the GUID of the account merging.
                var subordinate = svc.Retrieve("account", Guid.Parse("9bea4a38-4c2c-ec11-b6e5-000d3a4f9c59"), attributes);
                merge.SubordinateId = subordinate.Id;
                merge.Target = target.ToEntityReference();
                merge.PerformParentingChecks = true; //false;
                //Console.WriteLine("\nMerging account2 into account1 and adding " + "\"test\" as Address 1 Line 1");

                // Create another account to hold new data to merge into the entity.
                // If you use the subordinate account object, its data will be merged.
                //var updateContent = new Account();
                //updateContent.Address1_Line1 = "test";
                var updateContent = new Entity("account");
                updateContent["fax"] = subordinate["fax"];
                updateContent["telephone1"] = subordinate["telephone1"];
                merge.UpdateContent = updateContent;

                // Set the content you want updated on the merged account
                //merge.UpdateContent = updateContent;

                // Execute the request.
                var merged = (MergeResponse)svc.Execute(merge);
                if (merged != null) Console.WriteLine("It worked???: ", merged.ResponseName);
            }
        }
    }
}
