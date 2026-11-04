using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Moonrocks_Sweepin
{
    public partial class Main : System.Web.UI.Page
    {
        public static Bitmap map;
        
        public static bool[,] revealed = new bool[GameConfig.Width, GameConfig.Height];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MineHandler mines = new MineHandler();
                Session["cells"] = mines.Init();
            }
            else map = new Bitmap((bool[,])Session["cells"]);

            //Button[,] cells = new Button[GameConfig.Width, GameConfig.Height];
            
            for (int i = 0; i < GameConfig.Height; i++) {
                for (int j = 0; j < GameConfig.Width; j++)
                {
                    HtmlGenericControl div = new HtmlGenericControl("div");
                    
                    div.Attributes["class"] = "box";

                    Button cell = new Button
                    {
                        //ImageUrl = "images/1.png",
                        ID = $"{j} {i}",
                        Height = new Unit(24),
                        Width = new Unit(24),
                        CssClass = "button"
                    };
                    
                    cell.Click += ClickDelegate;
                    
                    div.Controls.Add(cell);
                    
                    Panel.Controls.Add(div);

                }
                
                Panel.Controls.Add(new LiteralControl("<div class=\"fakebr\"> </div>"));
            }

            //if (!IsPostBack) Session["controls"] = cells;
            
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Panel.Controls.Clear();
            
            for (int i = 0; i < GameConfig.Height; i++)
            {
                for (int j = 0; j < GameConfig.Width; j++)
                {
                    HtmlGenericControl div = new HtmlGenericControl("div");
                    
                    if (revealed[j, i])
                    {
                        div.Attributes["class"] = "box revealed";
                        
                        ulong mask = Bitmap.MaskOnClick(j);

                        ulong[] adjacent = map.AdjacentMask(mask, i);
                        int popcount = Bitmap.PopcountMask(adjacent);

                        System.Web.UI.WebControls.Image revealedImg = new System.Web.UI.WebControls.Image
                        {
                            ID = $"image: {j} {i}",
                            CssClass = popcount == 0 ? "cell" : "rock",
                            ImageUrl = popcount == 0 ? "" : $"images/{popcount}.png"
                        };

                        if (((bool[,])Session["cells"])[j, i]) revealedImg.ImageUrl = "images/rock.png";

                        div.Controls.Add(revealedImg);
                    }
                    else
                    {
                        div.Attributes["class"] = "box";
                        
                        Button cell = new Button
                        {
                            ID = $"{j} {i}",
                            Height = new Unit(24),
                            Width = new Unit(24),
                            CssClass = "button"
                        };
                        
                        div.Controls.Add(cell);
                    }
                    
                    Panel.Controls.Add(div);

                }
                
                Panel.Controls.Add(new LiteralControl("<div class=\"fakebr\"> </div>"));
            }
            
            //Session["controls"] = cells;
            
        }

        private void ClickDelegate(object sender, EventArgs args)
        {
            Button cell = (Button)sender;
            string id = cell.ID;
            
            revealed[int.Parse(id.Substring(0, id.IndexOf(' '))), int.Parse(id.Substring(id.IndexOf(' ')))] = true;
        }

    }
}