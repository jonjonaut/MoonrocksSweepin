using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Moonrocks_Sweepin
{
    public partial class Main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MineHandler mines = new MineHandler();
                Session["cells"] = mines.Init();
            }

            Button[,] cells = new Button[GameConfig.Width, GameConfig.Height];
            
            for (int i = 0; i < GameConfig.Height; i++) {
                for (int j = 0; j < GameConfig.Width; j++)
                {
                    HtmlGenericControl div = new HtmlGenericControl("div");
                    div.Attributes["class"] = "box";
                    div.ID = $"box {i} {j}";
                    
                    cells[j, i] = new Button
                    {
                        //ImageUrl = "images/1.png",
                        ID = $"{j} {i}",
                        Height = new Unit(24)
                    };
                    
                    cells[j, i].Click += ClickDelegate;

                    /*if (((bool?[,])Session["cells"])[j, i] == true)
                    {
                        //cell.ImageUrl = "images/rock.png";
                        cell.CssClass = "rock";
                    }*/

                    div.Controls.Add(cells[j, i]);
                    
                    Panel.Controls.Add(div);

                }
                Panel.Controls.Add(new LiteralControl("<div class=\"fakebr\"> </div>"));
            }

            Session["controls"] = cells;


        }
        
        private void ClickDelegate(object sender, EventArgs args)
        {
            Button cell = (Button)sender;
            Console.WriteLine(cell.ID); 
            
            Bitmap map = new Bitmap((bool?[,])Session["cells"]);
            ulong mask = Bitmap.MaskOnClick(int.Parse(cell.ID.Substring(0, cell.ID.IndexOf(" "))));

            ulong[] adjacent = map.AdjacentMask(mask, int.Parse(cell.ID.Substring(cell.ID.IndexOf(" "))));
            Console.WriteLine(Bitmap.PopcountMask(adjacent));

        }

    }
}