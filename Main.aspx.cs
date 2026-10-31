using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
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


            for (int i = 0; i < GameConfig.Height; i++) {
                for (int j = 0; j < GameConfig.Width; j++)
                {
                    ImageButton cell = new ImageButton
                    {
                        ImageUrl = "images/1.png",
                        ID = $"{j} {i}"
                    };
                    
                    cell.Click += ClickDelegate;
                    
                    if (((bool?[,])Session["cells"])[j, i] == true) cell.ImageUrl = "images/rock.png";

                    Panel.Controls.Add(cell);

                }
                Panel.Controls.Add(new LiteralControl("<br />"));
            }


        }

        protected void ClickDelegate(object sender, ImageClickEventArgs args)
        {
            ImageButton cell = (ImageButton)sender;
            Console.WriteLine(cell.ID); 
            
            Bitmap map = new Bitmap((bool?[,])Session["cells"]);
            ulong mask = Bitmap.MaskOnClick(int.Parse(cell.ID.Substring(0, cell.ID.IndexOf(" "))));

            ulong[] adjacent = map.AdjacentMask(mask, int.Parse(cell.ID.Substring(cell.ID.IndexOf(" "))));
            Console.WriteLine(Bitmap.PopcountMask(adjacent));

        }

    }
}