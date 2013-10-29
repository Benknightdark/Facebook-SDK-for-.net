using Facebook;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.ComplexEventProcessing;

public partial class fbtest : System.Web.UI.Page
{
    //string FBName;
    //string SportsPerson;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Check if already Signed In
            if (Session["AccessToken"] != null)
            {
                // Retrieve user information from database if stored or else create a new FacebookClient with this accesstoken and extract data again.
                GetUserData(Session["AccessToken"].ToString());

            }
            // Check if redirected from facebook
            else if (Request.QueryString["code"] != null)
            {
                string accessCode = Request.QueryString["code"].ToString();

                var fb = new FacebookClient();

                // throws OAuthException 
                dynamic result = fb.Post("oauth/access_token", new
                {

                    client_id = "180695878737541",

                    client_secret = "e6c2fbcde68f87e9566d42f1108c0d3a",

                    redirect_uri = "http://localhost/GED/FacebookSDKExample/FacebookSDKExample/",

                    code = accessCode

                });

               

                // Store the access token in the session
                Session["AccessToken"] = result.access_token;

                GetUserData(result.access_token);
            }

            else if (Request.QueryString["error"] != null)
            {
                // Notify the user as you like
                string error = Request.QueryString["error"];
                string errorResponse = Request.QueryString["error_reason"];
                string errorDescription = Request.QueryString["error_description"];

               
            }

            else
            {
                // User not connected, ask them to sign in again
                Response.Write("no sighn in");
            }
        }
    }

    protected void Login_Click(object sender, EventArgs e)
    {
        if (Login.Text == "Log Out")
            logout();
        else
        {
            var fb = new FacebookClient();

            var loginUrl = fb.GetLoginUrl(new
            {

                client_id = "180695878737541",

                redirect_uri = "http://localhost/GED/FacebookSDKExample/FacebookSDKExample/",

                response_type = "code",

                scope = "email,user_likes,publish_stream" // Add other permissions as needed

            });
            Response.Redirect(loginUrl.AbsoluteUri);
        }
    }

    private void logout()
    {
        var fb = new FacebookClient();

        var logoutUrl = fb.GetLogoutUrl(new
        {
            access_token = Session["AccessToken"],

            next = "http://localhost/GED/FacebookSDKExample/FacebookSDKExample/"

        });

        Session.Remove("AccessToken");

        Response.Redirect(logoutUrl.AbsoluteUri);
    }


    private void GetUserData(string accessToken)
    {
        var fb = new FacebookClient(accessToken);
        dynamic me = fb.Get("/me/statuses?limit=10000");
        Response.Write(me);
        
    
        /*
        dynamic me = fb.Get("me?fields=friends,name,email,favorite_athletes");

        string id = me.id; // Store in database
        string email = me.email; // Store in database
        string FBName = me.name; // Store in database            
        Response.Write(id);
        */
        /*
        ViewState["FBName"] = FBName; // Storing User's Name in ViewState

        var friends = me.friends;

        foreach (var friend in (JsonArray)friends["data"])
        {
            ListItem item = new ListItem((string)(((JsonObject)friend)["name"]), (string)(((JsonObject)friend)["id"]));
            FriendList.Items.Add(item);
        }

        var athletes = me.favorite_athletes;

        foreach (var athlete in (JsonArray)athletes)
        {
            SportsPersonList.Items.Add((string)(((JsonObject)athlete)["name"]));
        }
        */
        Login.Text = "Log Out";
    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }
}
  
