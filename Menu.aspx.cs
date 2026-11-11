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
            debugDiv.Visible = debugCheckbox.Checked;
            if (Request.Form["difficulty"] is string difficulty)
            {
                GameConfig.Width = GameConfig.Height = int.Parse(difficulty);
                GameConfig.Seed = string.IsNullOrEmpty(Seed.Text) ? (int?)null : int.Parse(Seed.Text);
                Response.Redirect("Main.aspx");
            }
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            GameConfig.Seed = string.IsNullOrEmpty(Seed.Text) ? (int?)null : int.Parse(Seed.Text);
            GameConfig.Height = int.Parse(Height.Text);
            GameConfig.Width = int.Parse(Width.Text);
            Response.Redirect("Main.aspx");
        }
        
    }

    public class GameConfig
    {
        public static int Height { get; set; }
        public static int Width { get; set; }
        public static int? Seed { get; set; }
    }
}