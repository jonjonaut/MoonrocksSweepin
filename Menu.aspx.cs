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
            if (!Page.IsPostBack)
            {
                if (GameConfig.BgColorLookup.TryGetValue(GameConfig.Wins, out string bgColor))
                {
                    menuin.Style["background-color"] = bgColor;
                }
                
                winCount.Text += $"{GameConfig.Wins}";
                loseCount.Text += $"{GameConfig.Losses}";
            }
            
            if (Session["tutorial"] != null)
            {
                tutorial.Visible = (bool)Session["tutorial"];
            }
            
            
            debugDiv.Visible = debugCheckbox.Checked;
            if (Request.Form["difficulty"] is string difficulty)
            {
                GameConfig.Width = GameConfig.Height = int.Parse(difficulty);
                GameConfig.Seed = string.IsNullOrEmpty(Seed.Text) ? (int?)null : int.Parse(Seed.Text);
                var zoom = new Dictionary<string, double>
                {
                    { "10", 1 },
                    { "32", 1 },
                    { "64", 0.6 }
                };
                
                Session["zoom"] = zoom[difficulty];
                Response.Redirect("Main.aspx", false);
            }
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            GameConfig.Seed = string.IsNullOrEmpty(Seed.Text) ? (int?)null : int.Parse(Seed.Text);
            GameConfig.Height = int.Parse(Height.Text);
            GameConfig.Width = int.Parse(Width.Text);
            tutorial.Visible = false;
            Response.Redirect("Main.aspx");
        }
        
    }

    public class GameConfig
    {
        public static int Height { get; set; }
        public static int Width { get; set; }
        public static int? Seed { get; set; }
        public static int Wins = 0;
        public static int Losses = 0;
        
        public static Dictionary<int, string> BgColorLookup = new Dictionary<int, string>
        {
            { 1, "#224dda" },
            { 2, "#077a12" },
            { 3, "#ff2200"},
            { 4, "#2000af" },
            { 5, "#790000" },
            { 6, "#005e79" },
            { 7, "#ffffff"},
            { 8, "#484848" },
        };
    }
}