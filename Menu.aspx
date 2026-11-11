<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="Moonrocks_Sweepin.Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" xmlns:asp="http://www.w3.org/1999/html">
<head runat="server">
    <title>Moonrocks</title>
    <link rel="stylesheet" href="stylishsheet.css" />
</head>
<body>
    <form id="form1" runat="server">
        <details class="stats">
            <summary class="statsSum">Stats</summary>
            <div class="statsText">
                <asp:Label ID="winCount" Text="Wins: " class="statslabe" runat="server"></asp:Label>
            </div>
            <div class="statsText">
                <asp:Label ID="loseCount" Text="Loses: " class="statslabe" runat="server"></asp:Label>
            </div>
        </details>
        
        <details class="details">
            <summary class="settings">Settings</summary>
            <div class="text">Seed:</div> <asp:TextBox ID="Seed" CssClass="seedBox" runat="server"></asp:TextBox>
            <br/><div class="debugDiv text">Debug Mode</div> <asp:CheckBox ID="debugCheckbox" CssClass="debugCheckbox" CausesValidation="false" runat="server" AutoPostBack="True"/>
        </details>
        
        <div class="difficulty">
            <button name="difficulty" value="10" class="difficultyBtn" runat="server">Wacky</button>
            <button name="difficulty" value="32" class="difficultyBtn" runat="server">Normal</button>
            <button name="difficulty" value="64" class="difficultyBtn ohno" runat="server">Ohno</button>  
        </div>
        
        <div id="debugDiv" class="debug" runat="server">
            <asp:TextBox ID="Height" runat="server"></asp:TextBox>
            <asp:TextBox ID="Width" runat="server"></asp:TextBox>
            <asp:Button ID="Btn" runat="server" Text="submit" OnClick="submit_Click" />
        </div>
        
        <br/>
        <p id="tutorial" runat="server" class="p">Press <span style="color: #ff7edb">F</span> or (<span style="color: #ff7edb">Alt Shift F</span> on <span style="color: #df731c">Firefox</span>) to enable <span class="flagmode">Flag Mode!</span></p>
    </form>
</body>
</html>
