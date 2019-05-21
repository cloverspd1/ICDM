namespace BEL.ItemCodeCreationPreProcess.Controllers.PowerBIReport
{
    using System.Threading.Tasks;
    using System.Web.Mvc;

    /// <summary>
    /// Power BI Report Controller
    /// </summary>
    /// <seealso cref="BEL.ItemCodeCreationPreProcess.Controllers.BaseController" />
    public class PowerBIReportController : BaseController
    {
        //  private static readonly string Username = ConfigurationManager.AppSettings["spUserName"];
        //  //private static readonly string Password = ConfigurationManager.AppSettings["spPassword"];
        //  private static readonly string AuthorityUrl = ConfigurationManager.AppSettings["authorityUrl"];
        //  private static readonly string ResourceUrl = ConfigurationManager.AppSettings["resourceUrl"];
        //  private static readonly string ClientId = ConfigurationManager.AppSettings["PowerBiClientId"];
        //  private static readonly string ClientSecret = ConfigurationManager.AppSettings["PowerBiClientSecret"];
        //  private static readonly string ApiUrl = ConfigurationManager.AppSettings["apiUrl"];
        //  private static readonly string GroupId = ConfigurationManager.AppSettings["groupId"];
        //  private static readonly string ReportId = ConfigurationManager.AppSettings["reportId"];
        ////  private static readonly string WebRoleUrl = ConfigurationManager.AppSettings["WebRoleUrl"];

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        //public void BIReport()
        //{
        //    GetAuthorizationCode();
        //    //  EmbedReport();     
        //}

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        /// <summary>
        /// Embeds the report.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> EmbedReport()
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            //var result = new EmbedConfig();

            //string username = string.Empty;
            //string roles = string.Empty;
            //try
            //{
            //    result = new EmbedConfig { Username = username, Roles = roles };
            //    var error = GetWebConfigErrors();
            //    if (error != null)
            //    {
            //        result.ErrorMessage = error;
            //        return View(result);
            //    }
            //    // Create a user password cradentials.
            //    //var credential = new UserPasswordCredential(Username, Password);
            //    var clientcredentials = new ClientCredential(ClientId, ClientSecret);
            //    // Authenticate using created credentials
            //    AuthenticationContext authenticationContext = new AuthenticationContext(AuthorityUrl);
            //    AuthenticationResult authenticationResult = await authenticationContext.AcquireTokenByAuthorizationCodeAsync(Request.Params.GetValues("code")[0], new Uri("http://localhost:62403/PowerBIReport/EmbedReport"), clientcredentials);
            //    if (authenticationResult == null)
            //    {
            //        result.ErrorMessage = "Authentication Failed.";
            //        return View(result);
            //    }
            //    //Set token from authentication result
            //    string data = authenticationResult.AccessToken;
            //    //string d = authenticationContext.AcquireTokenByAuthorizationCodeAsync(
            //    //   Request.Params.GetValues("code")[0],
            //    //     new Uri("http://localhost:"), clientcredentials).AccessToken;
            //    var tokenCredentials = new TokenCredentials(data, authenticationResult.AccessTokenType);
            //    // Create a Power BI Client object. It will be used to call Power BI APIs.
            //    using (var client = new PowerBIClient(new Uri(ApiUrl), tokenCredentials))
            //    {
            //        // Get a list of reports.
            //        var groups = client.Groups.GetGroups();
            //        var reports = await client.Reports.GetReportsAsync(ReportId);
            //        Report report;
            //        if (string.IsNullOrEmpty(ReportId))
            //        {
            //            // Get the first report in the group.
            //            report = reports.Value.FirstOrDefault();
            //        }
            //        else
            //        {
            //            report = reports.Value.FirstOrDefault(r => r.Id == ReportId);
            //        }
            //        if (report == null)
            //        {
            //            result.ErrorMessage = "Group has no reports.";
            //            return View(result);
            //        }
            //        var datasets = await client.Datasets.GetDatasetByIdInGroupAsync(GroupId, report.DatasetId);
            //        result.IsEffectiveIdentityRequired = datasets.IsEffectiveIdentityRequired;
            //        result.IsEffectiveIdentityRolesRequired = datasets.IsEffectiveIdentityRolesRequired;
            //        GenerateTokenRequest generateTokenRequestParameters;
            //        // This is how you create embed token with effective identities
            //        if (!string.IsNullOrEmpty(username))
            //        {
            //            var rls = new EffectiveIdentity(username, new List<string> { report.DatasetId });
            //            if (!string.IsNullOrWhiteSpace(roles))
            //            {
            //                var rolesList = new List<string>();
            //                rolesList.AddRange(roles.Split(','));
            //                rls.Roles = rolesList;
            //            }
            //            // Generate Embed Token with effective identities.
            //            generateTokenRequestParameters = new GenerateTokenRequest(accessLevel: "view", identities: new List<EffectiveIdentity> { rls });
            //        }
            //        else
            //        {
            //            // Generate Embed Token for reports without effective identities.
            //            generateTokenRequestParameters = new GenerateTokenRequest(accessLevel: "view");
            //        }
            //        var tokenResponse = await client.Reports.GenerateTokenInGroupAsync(GroupId, report.Id, generateTokenRequestParameters);
            //        if (tokenResponse == null)
            //        {
            //            result.ErrorMessage = "Failed to generate embed token.";
            //            return View(result);
            //        }
            //        // Generate Embed Configuration.
            //        result.EmbedToken = tokenResponse;
            //        result.EmbedUrl = report.EmbedUrl;
            //        result.Id = report.Id;
            //        return View(result);
            //    }
            //}
            //catch (HttpOperationException exc)
            //{
            //    result.ErrorMessage = string.Format("Status: {0} ({1})\r\nResponse: {2}\r\nRequestId: {3}", exc.Response.StatusCode, (int)exc.Response.StatusCode, exc.Response.Content, exc.Response.Headers["RequestId"].FirstOrDefault());
            //}
            //catch (Exception exc)
            //{
            //    result.ErrorMessage = exc.ToString();
            //}
            return View();
        }

        //public void GetAuthorizationCode()
        //{
        //    //NOTE: Values are hard-coded for sample purposes.
        //    //Create a query string
        //    //Create a sign-in NameValueCollection for query string
        //    var @params = new NameValueCollection
        //    {
        //        //Azure AD will return an authorization code. 
        //        {"response_type", "code"},

        //        //Client ID is used by the application to identify themselves to the users that they are requesting permissions from. 
        //        //You get the client id when you register your Azure app.
        //        {"client_id", ClientId},

        //        //Resource uri to the Power BI resource to be authorized
        //        //The resource uri is hard-coded for sample purposes
        //        {"resource", ResourceUrl},

        //        //After app authenticates, Azure AD will redirect back to the web app. In this sample, Azure AD redirects back
        //        //to Default page (Default.aspx).
        //        { "redirect_uri", "http://localhost:62403/PowerBIReport/EmbedReport"}
        //    };
        //    var queryString = HttpUtility.ParseQueryString(string.Empty);
        //    queryString.Add(@params);

        //    //Redirect to Azure AD Authority
        //    //  Authority Uri is an Azure resource that takes a client id and client secret to get an Access token
        //    //  QueryString contains 
        //    //      response_type of "code"
        //    //      client_id that identifies your app in Azure AD
        //    //      resource which is the Power BI API resource to be authorized
        //    //      redirect_uri which is the uri that Azure AD will redirect back to after it authenticates

        //    //Redirect to Azure AD to get an authorization code
        //    Response.Redirect(String.Format(AuthorityUrl + "?{0}", queryString));

        //}

        ///// <summary>
        ///// Check if web.config embed parameters have valid values.
        ///// </summary>
        ///// <returns>Null if web.config parameters are valid, otherwise returns specific error string.</returns>
        //private string GetWebConfigErrors()
        //{
        //    // Client Id must have a value.
        //    if (string.IsNullOrEmpty(ClientId))
        //    {
        //        return "ClientId is empty. please register your application as Native app in https://dev.powerbi.com/apps and fill client Id in web.config.";
        //    }

        //    // Client Id must be a Guid object.
        //    Guid result;
        //    if (!Guid.TryParse(ClientId, out result))
        //    {
        //        return "ClientId must be a Guid object. please register your application as Native app in https://dev.powerbi.com/apps and fill client Id in web.config.";
        //    }

        //    // Group Id must have a value.
        //    if (string.IsNullOrEmpty(GroupId))
        //    {
        //        return "GroupId is empty. Please select a group you own and fill its Id in web.config";
        //    }

        //    // Group Id must be a Guid object.
        //    if (!Guid.TryParse(GroupId, out result))
        //    {
        //        return "GroupId must be a Guid object. Please select a group you own and fill its Id in web.config";
        //    }

        //    // Username must have a value.
        //    if (string.IsNullOrEmpty(Username))
        //    {
        //        return "Username is empty. Please fill Power BI username in web.config";
        //    }

        //    // Password must have a value.
        //    //if (string.IsNullOrEmpty(Password))
        //    //{
        //    //    return "Password is empty. Please fill password of Power BI username in web.config";
        //    //}

        //    return null;
        //}
    }
}