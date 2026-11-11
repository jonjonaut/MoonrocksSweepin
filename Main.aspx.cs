using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;
using System.Web;
using System.Web.Instrumentation;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Moonrocks_Sweepin
{
    public partial class Main : System.Web.UI.Page
    {        
        private static bool[,] revealed;
        private static int CellsRevealed;
        private static Bitmap map;
        private static bool isFlag;
        private static List<(int, int)> flags = new List<(int, int)>();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (GameConfig.BgColorLookup.TryGetValue(GameConfig.Wins, out string bgColor))
                {
                    pagin.Style["background-color"] = bgColor;
                }
                
                Session["tutorial"] = false;
                pagin.Style["zoom"] = Session["zoom"].ToString();
                revealed = new bool[GameConfig.Width, GameConfig.Height];
                MineHandler mines = new MineHandler();
                Session["cells"] = mines.Init(GameConfig.Seed);
                map = new Bitmap((bool[,])Session["cells"]);
            }


            //Button[,] cells = new Button[GameConfig.Width, GameConfig.Height];
            int[,] cellCounts = new int[GameConfig.Width, GameConfig.Height];

            for (int i = 0; i < GameConfig.Height; i++) {
                for (int j = 0; j < GameConfig.Width; j++)
                {
                    HtmlGenericControl div = new HtmlGenericControl("div");
                    
                    div.Attributes["class"] = "box";

                    ImageButton cell = new ImageButton
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
                
            }

            if (!IsPostBack) {
                Session["cellCounts"] = cellCounts.Clone();
                map.MaskEmpty((int[,])Session["cellCounts"], (bool[,])Session["cells"]);
            }
            
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Panel.Controls.Clear();

            if (!(Session["win"] is null))
            {
                restart.ImageUrl = "images/won.png";
                Return.Visible = true;
            }
            
            for (int i = 0; i < GameConfig.Height; i++)
            {
                HtmlGenericControl cellRow = new HtmlGenericControl("div");
                
                cellRow.Attributes["class"] = "row";
                
                for (int j = 0; j < GameConfig.Width; j++)
                {
                    HtmlGenericControl div = new HtmlGenericControl("div");
                    
                    if (revealed[j, i])
                    {
                        div.Attributes["class"] = "box revealed ";

                        int popcount = ((int[,])Session["cellCounts"])[j, i];

                        System.Web.UI.WebControls.Image revealedImg = new System.Web.UI.WebControls.Image
                        {
                            ID = $"image: {j} {i}",
                            CssClass = popcount == 0 ? "cell" : "rock",
                            ImageUrl = popcount == 0 ? "" : $"images/{popcount}.png"
                        };

                        if (((bool[,])Session["cells"])[j, i]) revealedImg.ImageUrl = "images/rock.png";

                        if (Session["lost"] as (int, int)? == (j, i)) div.Attributes["class"] += "lost";
                        
                        div.Controls.Add(revealedImg);
                    }
                    else
                    {
                        div.Attributes["class"] = "box";
                        
                        ImageButton cell = new ImageButton
                        {
                            ID = $"{j} {i}",
                            Height = new Unit(100, UnitType.Percentage),
                            CssClass = "button",
                        };
                        
                        if (!(Session["lost"] is null))
                        {
                            cell.Enabled = false;
                            Return.Visible = true;
                        }

                        if (flags.Contains((j, i)))
                        {
                            cell.CssClass = "flagImage";
                            cell.ImageUrl = "images/flag.svg";
                        }
                        
                        div.Controls.Add(cell);
                    }
                    
                    cellRow.Controls.Add(div);

                }
                
                Panel.Controls.Add(cellRow);
            }
            
            //Session["controls"] = cells;
            
        }
        
        protected void Restart(object sender, ImageClickEventArgs args)
        {
            //reset everything hmmm
            CellsRevealed = 0;
            isFlag = false;
            flags = new List<(int, int)>(); 
            GameConfig.Seed = string.IsNullOrEmpty(Seed.Text) ? (int?)null : int.Parse(Seed.Text);
            Session["lost"] = null;
            Session["win"] = null;
            
            Response.Redirect("Main.aspx");
        }

        protected void ReturnToMenu(object sender, EventArgs e)
        {
            //reset everything hmmm
            CellsRevealed = 0;
            isFlag = false;
            flags = new List<(int, int)>(); 
            GameConfig.Seed = string.IsNullOrEmpty(Seed.Text) ? (int?)null : int.Parse(Seed.Text);
            Session["lost"] = null;
            Session["win"] = null;
            
            Response.Redirect("Menu.aspx");
        }
        
        private void ClickDelegate(object sender, ImageClickEventArgs args)
        {
            ImageButton cell = (ImageButton)sender;
            string id = cell.ID;
            int j = int.Parse(id.Substring(0, id.IndexOf(' ')));
            int i = int.Parse(id.Substring(id.IndexOf(' ')));

            bool[,] cells = (bool[,])Session["cells"];
            int[,] cellCounts = (int[,])Session["cellCounts"];
            
            //On flag mode/flag
            
            if (isFlag)
            {
                if (!flags.Contains((j, i)))
                {
                    flags.Add((j, i));
                    return;
                }
                flags.Remove((j, i));
                return;
            }

            if (flags.Contains((j, i))) return;
            
            //Mine on first attempt
            
            if (CellsRevealed == 0 && cells[j, i])
            {
                cells[j, i] = false;
                map.RemoveMine(Bitmap.MaskOnClick(j), i);
                ((bool[,])Session["cells"])[j, i] = false;
                
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    for (int x = 0; x < cells.GetLength(0); x++)
                    {
                        ulong mask = Bitmap.MaskOnClick(x);

                        ulong[] adjacent = map.AdjacentMask(mask, y);
                        int popcount = Bitmap.PopcountMask(adjacent);
                        cellCounts[x, y] = popcount;
                    }
                }
                
                Session["cellCounts"] = cellCounts.Clone();
                map.MaskEmpty((int[,])Session["cellCounts"], (bool[,])Session["cells"]);

                CellsRevealed++;
            }
            
            //On lost
            
            if (cells[j, i])
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    for (int x = 0; x < cells.GetLength(0); x++)
                    {
                        if (cells[x, y]) revealed[x, y] = true;
                    }
                }

                GameConfig.Losses++;
                Session["lost"] = (j, i);
                return;
            }
            
            //On win
            
            if (CellsRevealed + Bitmap.PopcountMask(map.Field) == cells.Length - 1)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    for (int x = 0; x < cells.GetLength(0); x++)
                    {
                        if (cells[x, y]) flags.Add((x, y));
                    }
                }

                GameConfig.Wins++;
                Session["win"] = true;
            }
            Console.WriteLine($"{CellsRevealed} {Bitmap.PopcountMask(map.Field)} {cells.Length}");
            
            //On Empty
            
            if (cellCounts[j, i] == 0)
            {
                ulong[] empties = map.BorderMask(map.EmptyMask(j, i));
                
                bool[,] revEmpties = map.ReverseBitboardize(empties);
                
                for (int y = 0; y < revEmpties.GetLength(1); y++)
                {
                    for (int x = 0; x < revEmpties.GetLength(0); x++)
                    {
                        if(revEmpties[x, y])
                        {
                            if (!revealed[x, y]) CellsRevealed++;
                            revealed[x, y] = true;
                        }
                    }
                }

                return;
            }
            
            CellsRevealed++;
            revealed[j, i] = true;

        }

        protected void ToggleFlag(object sender, EventArgs e)
        {
            isFlag = !isFlag;
            flagMode.Visible = isFlag;
        }
    }
}