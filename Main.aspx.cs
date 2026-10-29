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
        private static ImageButton[] cells;

        protected void Page_Load(object sender, EventArgs e)
        {
            cells = new ImageButton[GameConfig.Height * GameConfig.Width];
            Session["cells"] = cells;

            for (int i = 0; i < GameConfig.Height; i++) {
                for (int j = 0; j < GameConfig.Width; j++)
                {
                    ImageButton cell = new ImageButton
                    {
                        ImageUrl = "images/1.png",
                        ID = $"{i} {j}"
                    };

                    Panel.Controls.Add(cell);

                }
                Panel.Controls.Add(new LiteralControl("<br />"));
            }

            MineHandler mines = new MineHandler();
            mines.Init();

        }


    }
}