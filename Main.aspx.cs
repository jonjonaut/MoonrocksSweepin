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
        public static bool[,] empty, revealed = new bool[GameConfig.Width, GameConfig.Height];
        private static Bitmap map;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MineHandler mines = new MineHandler();
                Session["cells"] = mines.Init();
                map = new Bitmap((bool[,])Session["cells"]);
            }


            //Button[,] cells = new Button[GameConfig.Width, GameConfig.Height];
            int[,] cellCounts = new int[GameConfig.Width, GameConfig.Height];

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

                    if (!IsPostBack)
                    {
                        ulong mask = Bitmap.MaskOnClick(j);

                        ulong[] adjacent = map.AdjacentMask(mask, i);
                        int popcount = Bitmap.PopcountMask(adjacent);
                        cellCounts[j, i] = popcount;
                    }

                    cell.Click += ClickDelegate;
                    
                    div.Controls.Add(cell);
                    
                    Panel.Controls.Add(div);

                }
                
                Panel.Controls.Add(new LiteralControl("<div class=\"fakebr\"> </div>"));
            }

            if (!IsPostBack) {
                Session["cellCounts"] = cellCounts;
                map.MaskEmpty((int[,])Session["cellCounts"], (bool[,])Session["cells"]);
            }
            
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

                        int popcount = ((int[,])Session["cellCounts"])[j, i];

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
            int j = int.Parse(id.Substring(0, id.IndexOf(' ')));
            int i = int.Parse(id.Substring(id.IndexOf(' ')));
            
            Console.Clear();
            
            if (((int[,])Session["cellCounts"])[j, i] == 0)
            {
                ulong[] empties = map.EmptyMask(j, i);

                foreach (ulong emptylong in empties)
                {
                    Console.WriteLine(Convert.ToString((long)emptylong, 2).PadLeft(GameConfig.Width, '0'));
                }
            }

            /*Console.Clear();

            foreach (var tup in map.AdjacentCoords(Bitmap.MaskOnClick(j), i)) {
                Console.WriteLine($"x={tup.Item1}, y={tup.Item2}");
            }*/
            
            /*foreach (ulong emptylong in empties)
            {
                Console.WriteLine(Convert.ToString((long)emptylong, 2).PadLeft(64, '0'));
            }*/
            
            revealed[j, i] = true;
        }

    }
}