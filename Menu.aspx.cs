using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Moonrocks_Sweepin
{
    public partial class Menu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void submit_Click(object sender, EventArgs e)
        {
            GameConfig.Height = int.Parse(Height.Text);
            GameConfig.Width = int.Parse(Width.Text);
            Response.Redirect("Main.aspx");

        }
    }

    public class GameConfig
    {
        public static int Height { get; set; }
        public static int Width { get; set; }
        
    }
}