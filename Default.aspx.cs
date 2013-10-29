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

                    client_id = "your app id",

                    client_secret = "your app secrect key",

                    redirect_uri = "your redirect website address",

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

                client_id = "your app id ",

                redirect_uri = "your redirect website address",

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

            next = "your redirect website address"

        });

        Session.Remove("AccessToken");

        Response.Redirect(logoutUrl.AbsoluteUri);
    }


    private void GetUserData(string accessToken)
    {
        var fb = new FacebookClient(accessToken);
        dynamic status = fb.Get("/me/statuses?limit=10000");//取得文字動態訊息，最多10000筆
        Response.Write(status);
        
        //另一種取得個人資料的方式
        /*
        dynamic me = fb.Get("me?fields=friends,name,email,favorite_athletes");

        string id = me.id; // Store in database
        string email = me.email; // Store in database
        string FBName = me.name; // Store in database            
        Response.Write(id);
        Response.Write(email);
        Response.Write(FBName);
        */
        Login.Text = "Log Out";
    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }
}
  
